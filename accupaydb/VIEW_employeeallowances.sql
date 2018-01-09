/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_employeeallowances`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeeallowances`(IN `eallow_EmployeeID` INT, IN `eallow_OrganizationID` INT, IN `effective_datefrom` DATE, IN `effective_dateto` DATE, IN `ExceptThisAllowance` TEXT)
    DETERMINISTIC
BEGIN

DECLARE AllowanceFrequenzy VARCHAR(50) DEFAULT 'Daily';

DECLARE is_ecola_compress BOOL DEFAULT FALSE;

    /*
     * Breakdown of daily allowances
     */
    /*SELECT
        prd.PartNo AS `PartNo`,
        sum.Date AS `Date`,
        sum.TotalAllowanceAmt AS `Amount`
    # FROM paystubitem_sum_daily_allowance_group_prodid sum
    FROM paystubitem_sum_daily_allowance_group_prodid_compress sum    
    INNER JOIN product prd
    ON prd.RowID = sum.ProductID
    WHERE sum.EmployeeID = eallow_EmployeeID AND
        sum.Date BETWEEN effective_datefrom AND effective_dateto
    ORDER BY prd.PartNo, sum.Date;*/
    
    
SELECT
EXISTS(SELECT lv.RowID
		 FROM listofval lv
		 WHERE lv.LIC = 'EcolaCompressed'
		 AND lv.`Type` = 'MiscAllowance'
		 AND lv.DisplayValue = '1')
INTO is_ecola_compress;

IF AllowanceFrequenzy = 'Daily'
   AND is_ecola_compress = FALSE THEN

    SET @day_pay = 0.0;
    SET @day_pay1 = 0.0;
    SET @day_pay2 = 0.0;
	
	SELECT
	     ii.PartNo,
        ii.`Date`,
        ii.`TotalAllowanceAmount` `Amount`
	FROM (
        SELECT
            i.etRowID,
            i.EmployeeID,
            i.`Date`,
            0 AS Equatn,
            0 AS `timediffcount`,
            i.TotalAllowanceAmt AS TotalAllowanceAmount,
            NULL AS ShiftID,
            'First SELECT statement' AS Result,
            p.PartNo
        FROM paystubitem_sum_daily_allowance_group_prodid i
        INNER JOIN product p ON p.RowID=i.ProductID
        WHERE i.OrganizationID=eallow_OrganizationID AND
            # i.TaxableFlag=IsTaxable AND
            i.`Date` BETWEEN effective_datefrom AND effective_dateto AND
			i.EmployeeID = eallow_EmployeeID AND
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
            'Second SELECT statement' AS Result,
            p.PartNo
        FROM employeetimeentry et
        INNER JOIN payrate pr
        ON pr.RowID = et.PayRateID AND
            pr.PayType = 'Regular Holiday'
        INNER JOIN employee e
        ON e.OrganizationID = eallow_OrganizationID AND
            e.RowID = et.EmployeeID AND
			e.RowID = eallow_EmployeeID AND
            e.EmploymentStatus NOT IN ('Resigned', 'Terminated') AND
            e.CalcHoliday = '1'
        INNER JOIN employeeshift es
        ON es.RowID = et.EmployeeShiftID
        INNER JOIN shift sh
        ON sh.RowID = es.ShiftID
        INNER JOIN employeeallowance ea
        ON ea.AllowanceFrequency = AllowanceFrequenzy AND
            # ea.TaxableFlag = IsTaxable AND
            ea.EmployeeID = e.RowID AND
            ea.OrganizationID = eallow_OrganizationID AND
            et.`Date` BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate
        INNER JOIN product p
        ON p.RowID = ea.ProductID AND
            p.`Fixed` = 0
        WHERE et.OrganizationID = eallow_OrganizationID AND
            et.`Date` BETWEEN effective_datefrom AND effective_dateto AND
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
            'Third SELECT statement' AS Result,
            p.PartNo
        FROM employeetimeentry et
        INNER JOIN payrate pr
        ON pr.RowID = et.PayRateID AND
            pr.PayType = 'Special Non-Working Holiday'
        INNER JOIN employee e
        ON e.OrganizationID = eallow_OrganizationID AND
            e.RowID = et.EmployeeID AND
			e.RowID = eallow_EmployeeID AND
            e.EmploymentStatus NOT IN ('Resigned','Terminated') AND
            e.EmployeeType != 'Daily'
        INNER JOIN employeeshift es
        ON es.RowID = et.EmployeeShiftID
        INNER JOIN shift sh
        ON sh.RowID = es.ShiftID
        INNER JOIN employeeallowance ea
        ON ea.AllowanceFrequency = AllowanceFrequenzy AND
            # IF(IsTaxable = '1', (ea.TaxableFlag = IsTaxable), FALSE) AND
            ea.EmployeeID = e.RowID AND
            ea.OrganizationID = eallow_OrganizationID AND
            et.`Date` BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate
        INNER JOIN product p
        ON p.RowID = ea.ProductID AND
            LOCATE('cola',p.PartNo) > 0
        WHERE et.OrganizationID = eallow_OrganizationID AND
            et.RegularHoursAmount = 0 AND
            et.TotalDayPay > 0 AND
            et.`Date` BETWEEN effective_datefrom AND effective_dateto
	      ) ii
	
;

ELSEIF AllowanceFrequenzy = 'Daily'
       AND is_ecola_compress = TRUE THEN

    SET @day_pay = 0.0;
    SET @day_pay1 = 0.0;
    SET @day_pay2 = 0.0;

	SELECT
	     ii.PartNo,
        ii.`Date`,
        ii.`TotalAllowanceAmount` `Amount`
	FROM (
        SELECT
            i.etRowID,
            i.EmployeeID,
            i.`Date`,
            0 AS Equatn,
            0 AS `timediffcount`,
            i.TotalAllowanceAmt AS TotalAllowanceAmount,
            NULL AS ShiftID,
            'First SELECT statement' AS Result,
            p.PartNo
        FROM paystubitem_sum_daily_allowance_group_prodid_compress i
        INNER JOIN product p ON p.RowID=i.ProductID
        WHERE i.OrganizationID=eallow_OrganizationID AND
            # i.TaxableFlag=IsTaxable AND
            i.`Date` BETWEEN effective_datefrom AND effective_dateto AND
			i.EmployeeID = eallow_EmployeeID AND
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
            'Second SELECT statement' AS Result,
            p.PartNo
        FROM employeetimeentry et
        INNER JOIN payrate pr
        ON pr.RowID = et.PayRateID AND
            pr.PayType = 'Regular Holiday'
        INNER JOIN employee e
        ON e.OrganizationID = eallow_OrganizationID AND
            e.RowID = et.EmployeeID AND
			e.RowID = eallow_EmployeeID AND
            e.EmploymentStatus NOT IN ('Resigned', 'Terminated') AND
            e.CalcHoliday = '1'
        INNER JOIN employeeshift es
        ON es.RowID = et.EmployeeShiftID
        INNER JOIN shift sh
        ON sh.RowID = es.ShiftID
        INNER JOIN employeeallowance ea
        ON ea.AllowanceFrequency = AllowanceFrequenzy AND
            # ea.TaxableFlag = IsTaxable AND
            ea.EmployeeID = e.RowID AND
            ea.OrganizationID = eallow_OrganizationID AND
            et.`Date` BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate
        INNER JOIN product p
        ON p.RowID = ea.ProductID AND
            p.`Fixed` = 0
        WHERE et.OrganizationID = eallow_OrganizationID AND
            et.`Date` BETWEEN effective_datefrom AND effective_dateto AND
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
            'Third SELECT statement' AS Result,
            p.PartNo
        FROM employeetimeentry et
        INNER JOIN payrate pr
        ON pr.RowID = et.PayRateID AND
            pr.PayType = 'Special Non-Working Holiday'
        INNER JOIN employee e
        ON e.OrganizationID = eallow_OrganizationID AND
            e.RowID = et.EmployeeID AND
			e.RowID = eallow_EmployeeID AND
            e.EmploymentStatus NOT IN ('Resigned','Terminated') AND
            e.EmployeeType != 'Daily'
        INNER JOIN employeeshift es
        ON es.RowID = et.EmployeeShiftID
        INNER JOIN shift sh
        ON sh.RowID = es.ShiftID
        INNER JOIN employeeallowance ea
        ON ea.AllowanceFrequency = AllowanceFrequenzy AND
            # IF(IsTaxable = '1', (ea.TaxableFlag = IsTaxable), FALSE) AND
            ea.EmployeeID = e.RowID AND
            ea.OrganizationID = eallow_OrganizationID AND
            et.`Date` BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate
        INNER JOIN product p
        ON p.RowID = ea.ProductID AND
            LOCATE('cola',p.PartNo) > 0
        WHERE et.OrganizationID = eallow_OrganizationID AND
            et.RegularHoursAmount = 0 AND
            et.TotalDayPay > 0 AND
            et.`Date` BETWEEN effective_datefrom AND effective_dateto

	      ) ii
;

END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
