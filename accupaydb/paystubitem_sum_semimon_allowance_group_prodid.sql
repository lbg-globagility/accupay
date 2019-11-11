/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP VIEW IF EXISTS `paystubitem_sum_semimon_allowance_group_prodid`;
DROP TABLE IF EXISTS `paystubitem_sum_semimon_allowance_group_prodid`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`127.0.0.1` SQL SECURITY DEFINER VIEW `paystubitem_sum_semimon_allowance_group_prodid` AS SELECT
        et.RowID AS `etRowID`,
        ea.RowID `eaRowID`,
        (ea.ProductID) AS ProductID,
        et.EmployeeID,
        et.OrganizationID,
        et.`Date`,
        0 AS Column1,
        IF(
            pr.PayType = 'Special Non-Working Holiday' AND e.CalcSpecialHoliday = TRUE,
            IFNULL(et.RegularHoursWorked * (ea.AllowanceAmount / (e.WorkDaysPerYear / 12 / 2) / sh.DivisorToDailyRate) * (pr.PayRate - 1), 0),
            IF(
                pr.PayType = 'Regular Holiday' AND e.CalcHoliday = TRUE,
                IFNULL(et.RegularHoursWorked * (ea.AllowanceAmount / (e.WorkDaysPerYear / 12 / 2)) * (pr.PayRate - 1), 0),
                0
            )
        ) AS HolidayAllowance,
        ea.TaxableFlag,
        (et.HoursLate + et.UndertimeHours + et.AbsentHours) AS HoursToLess,
        ea.AllowanceAmount,
        e.WorkDaysPerYear,
        p.`Fixed`,
        PAYFREQUENCY_DIVISOR(pf.PayFrequencyType) AS PAYFREQDIV
    FROM employeetimeentry et
    INNER JOIN employee e
    ON e.OrganizationID = et.OrganizationID AND
        e.RowID = et.EmployeeID AND
        e.EmploymentStatus NOT IN ('Resigned','Terminated') AND
        e.EmployeeType != 'Fixed'
    INNER JOIN payfrequency pf
    ON pf.RowID = e.PayFrequencyID
    LEFT JOIN employeeshift es
    ON es.RowID = et.EmployeeShiftID
    LEFT JOIN shift sh
    ON sh.RowID = es.ShiftID
    INNER JOIN employeeallowance ea
    ON ea.AllowanceFrequency = 'Semi-monthly' AND
        ea.EmployeeID = e.RowID AND
        ea.OrganizationID = e.OrganizationID AND
        et.`Date` BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate
    INNER JOIN product p
    ON p.RowID = ea.ProductID
    INNER JOIN payrate pr
    ON pr.RowID = et.PayRateID
    
UNION
    SELECT
        et.RowID AS `etRowID`,
        ea.RowID `eaRowID`,
        (ea.ProductID) AS ProductID,
        et.EmployeeID,
        et.OrganizationID,
        et.`Date`,
        0 AS Column1,
        0 AS HolidayAllowance,
        ea.TaxableFlag,
        (et.HoursLate + et.UndertimeHours) AS HoursToLess,
        ea.AllowanceAmount,
        e.WorkDaysPerYear,
        p.`Fixed`,
        PAYFREQUENCY_DIVISOR(pf.PayFrequencyType) AS PAYFREQDIV
    FROM employeetimeentry et
    INNER JOIN employee e ON e.OrganizationID=et.OrganizationID AND e.RowID=et.EmployeeID AND e.EmploymentStatus NOT IN ('Resigned','Terminated') AND e.EmployeeType='Fixed'
    INNER JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID
    INNER JOIN employeeshift es ON es.RowID=et.EmployeeShiftID
    INNER JOIN shift sh ON sh.RowID=es.ShiftID
    INNER JOIN employeeallowance ea ON ea.AllowanceFrequency='Semi-monthly' AND ea.EmployeeID=e.RowID AND ea.OrganizationID=e.OrganizationID AND et.`Date` BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate
    INNER JOIN product p ON p.RowID=ea.ProductID
    INNER JOIN payrate pr ON pr.RowID=et.PayRateID ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
