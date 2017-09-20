/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `GET_employee_allowanceofthisperiod`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `GET_employee_allowanceofthisperiod`(
	IN `OrganizID` INT,
	IN `AllowanceFrequenzy` VARCHAR(50),
	IN `IsTaxable` CHAR(1),
	IN `DatePayFrom` DATE,
	IN `DatePayTo` DATE
)
    DETERMINISTIC
BEGIN

DECLARE isEndOfMonth CHAR(1);
DECLARE MonthCount DECIMAL(11,2) DEFAULT 12.0;

DECLARE firstdate DATE;
DECLARE thismonth VARCHAR(2);
DECLARE thisyear INT(11);

SET @timediffcount = 0.00;



































IF AllowanceFrequenzy = 'Monthly' THEN

    SELECT (`Half` = 0),
        `Month`,
        `Year`
    FROM payperiod
    WHERE OrganizationID = OrganizID AND
        PayFromDate = DatePayFrom AND
        PayToDate = DatePayTo
    LIMIT 1
    INTO isEndOfMonth,
        thismonth,
        thisyear;

    IF isEndOfMonth = '1' THEN

        SELECT PayFromDate
        FROM payperiod
        WHERE OrganizationID = OrganizID AND
            `Month` = thismonth AND
            `Year` = thisyear
        ORDER BY PayFromDate,
            PayToDate
        LIMIT 1
        INTO firstdate;

        SELECT (SELECT @dailyallowanceamount := ROUND((ea.AllowanceAmount / (e.WorkDaysPerYear / MonthCount)),2)),
            (SELECT @timediffcount := COMPUTE_TimeDifference(sh.TimeFrom,sh.TimeTo)),
            (SELECT @timediffcount := IF(@timediffcount < 5,@timediffcount,(@timediffcount - 1.0))),
            ROUND((@dailyallowanceamount - ( ( (et.HoursLate + et.UndertimeHours) / @timediffcount ) * @dailyallowanceamount )),2) AS TotalAllowanceAmount,
            et.EmployeeID,
            ea.ProductID
        FROM employeetimeentry et
        INNER JOIN employee e
        ON e.OrganizationID = OrganizID AND
            e.RowID = et.EmployeeID AND
            e.EmploymentStatus NOT IN ('Resigned','Terminated')
        INNER JOIN payfrequency pf
        ON pf.RowID = e.PayFrequencyID
        INNER JOIN employeeshift es
        ON es.RowID = et.EmployeeShiftID
        INNER JOIN shift sh
        ON sh.RowID = es.ShiftID
        INNER JOIN employeeallowance ea
        ON ea.AllowanceFrequency = AllowanceFrequenzy AND
            ea.TaxableFlag = IsTaxable AND
        ea.EmployeeID = e.RowID AND
        ea.OrganizationID = OrganizID AND
        et.`Date` BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate
        INNER JOIN product p
        ON p.RowID = ea.ProductID AND
            p.`Fixed` = 0
        WHERE et.OrganizationID = OrganizID AND
            et.`Date` BETWEEN firstdate AND DatePayTo;

    ELSE
        SELECT 0 AS TotalAllowanceAmount,
            '' AS EmployeeID,
            0 AS ProductID;
    END IF;

ELSEIF AllowanceFrequenzy = 'Semi-monthly' THEN

    SELECT i.*,
        ii.AllowanceAmount - (SUM(i.HoursToLess) * ((i.AllowanceAmount / (i.WorkDaysPerYear / (i.PAYFREQDIV * 12))) / 8)) AS TotalAllowanceAmount
    FROM paystubitem_sum_semimon_allowance_group_prodid i
    INNER JOIN (
        SELECT ea.*,
            MIN(d.DateValue) AS DateRange1,
            MAX(d.DateValue) AS DateRange2
        FROM dates d
        INNER JOIN employeeallowance ea
        ON ea.AllowanceFrequency = 'Semi-monthly' AND
            TaxableFlag = IsTaxable AND
            ea.OrganizationID = OrganizID AND
            d.DateValue BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate
        WHERE d.DateValue BETWEEN DatePayFrom AND DatePayTo
        GROUP BY ea.RowID
        ORDER BY d.DateValue
    ) ii
    ON i.EmployeeID = ii.EmployeeID AND
        i.OrganizationID = ii.OrganizationID AND
        i.`Date` BETWEEN ii.DateRange1 AND ii.DateRange2 AND
        i.`Fixed` = 0
    GROUP BY i.EmployeeID,
        ii.RowID;




































ELSEIF AllowanceFrequenzy = 'Daily' THEN

    SET @day_pay = 0.0;
    SET @day_pay1 = 0.0;
    SET @day_pay2 = 0.0;

        SELECT
            i.etRowID,
            i.EmployeeID,
            i.`Date`,
            0 AS Equatn,
            0 AS `timediffcount`,
            i.TotalAllowanceAmt AS TotalAllowanceAmount,
            NULL AS ShiftID,
            'First SELECT statement' AS Result
        FROM paystubitem_sum_daily_allowance_group_prodid i
        WHERE i.TaxableFlag=IsTaxable AND
            i.OrganizationID=OrganizID AND
            i.`Date` BETWEEN DatePayFrom AND DatePayTo AND
            i.`Fixed` = 0
    UNION
        SELECT
            et.RowID,
            et.EmployeeID,
            et.`Date`,
            (@day_pay1 := GET_employeerateperday(et.EmployeeID,et.OrganizationID,et.`Date`)) AS Equatn,
            0 AS `timediffcount`,
            ea.AllowanceAmount * ((et.HolidayPayAmount + ((et.VacationLeaveHours + et.SickLeaveHours + et.MaternityLeaveHours + et.OtherLeaveHours) * (@day_pay1 / sh.DivisorToDailyRate))) / @day_pay1) AS TotalAllowanceAmount,
            es.ShiftID,
            'Second SELECT statement' AS Result
        FROM employeetimeentry et
        INNER JOIN payrate pr
        ON pr.RowID = et.PayRateID AND
            pr.PayType = 'Regular Holiday'
        INNER JOIN employee e
        ON e.OrganizationID = OrganizID AND
            e.RowID = et.EmployeeID AND
            e.EmploymentStatus NOT IN ('Resigned', 'Terminated') AND
            e.CalcHoliday = '1'
        INNER JOIN employeeshift es
        ON es.RowID = et.EmployeeShiftID
        INNER JOIN shift sh
        ON sh.RowID = es.ShiftID
        INNER JOIN employeeallowance ea
        ON ea.AllowanceFrequency = AllowanceFrequenzy AND
            ea.TaxableFlag = IsTaxable AND
            ea.EmployeeID = e.RowID AND
            ea.OrganizationID = OrganizID AND
            et.`Date` BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate
        INNER JOIN product p
        ON p.RowID = ea.ProductID AND
            p.`Fixed` = 0
        WHERE et.OrganizationID = OrganizID AND
            et.`Date` BETWEEN DatePayFrom AND DatePayTo AND
            FALSE
    UNION
        SELECT
            et.RowID,
            et.EmployeeID,
            et.`Date`,
            (@day_pay1 := GET_employeerateperday(et.EmployeeID,et.OrganizationID,et.`Date`)) AS Equatn,
            0 AS `timediffcount`,
            ea.AllowanceAmount * ((et.HolidayPayAmount + ((et.VacationLeaveHours + et.SickLeaveHours + et.MaternityLeaveHours + et.OtherLeaveHours) * (@day_pay1 / sh.DivisorToDailyRate))) / @day_pay1) AS TotalAllowanceAmount,
            es.ShiftID,
            'Third SELECT statement' AS Result
        FROM employeetimeentry et
        INNER JOIN payrate pr
        ON pr.RowID = et.PayRateID AND
            pr.PayType = 'Special Non-Working Holiday'
        INNER JOIN employee e
        ON e.OrganizationID = OrganizID AND
            e.RowID = et.EmployeeID AND
            e.EmploymentStatus NOT IN ('Resigned','Terminated') AND
            e.EmployeeType != 'Daily'
        INNER JOIN employeeshift es
        ON es.RowID = et.EmployeeShiftID
        INNER JOIN shift sh
        ON sh.RowID = es.ShiftID
        INNER JOIN employeeallowance ea
        ON ea.AllowanceFrequency = AllowanceFrequenzy AND
            IF(IsTaxable = '1', (ea.TaxableFlag = IsTaxable), FALSE) AND
            ea.EmployeeID = e.RowID AND
            ea.OrganizationID = OrganizID AND
            et.`Date` BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate
        INNER JOIN product p
        ON p.RowID = ea.ProductID AND
            LOCATE('cola',p.PartNo) > 0
        WHERE et.OrganizationID = OrganizID AND
            et.RegularHoursAmount = 0 AND
            et.TotalDayPay > 0 AND
            et.`Date` BETWEEN DatePayFrom AND DatePayTo;

END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
