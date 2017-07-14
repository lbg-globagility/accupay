/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `HasWorkedLastWorkingDay`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `HasWorkedLastWorkingDay`(
	`employeeID` INT,
	`dateToCheck` DATETIME


) RETURNS tinyint(1)
BEGIN

    DECLARE regularHours DECIMAL(11, 6);
    DECLARE leaveHours DECIMAL(11, 6);

    SELECT
        employeetimeentry.RegularHoursWorked,
        (
            employeetimeentry.SickLeaveHours +
            employeetimeentry.MaternityLeaveHours +
            employeetimeentry.VacationLeaveHours +
            employeetimeentry.OtherLeaveHours
        )
    FROM employeetimeentry
    LEFT JOIN employee
        ON employee.RowID = employeetimeentry.EmployeeID
    LEFT JOIN payrate
        ON payrate.Date = employeetimeentry.Date
        AND payrate.OrganizationID = employeetimeentry.OrganizationID
    LEFT JOIN employeeshift
        ON employeeshift.RowID = employeetimeentry.EmployeeShiftID
    WHERE employeetimeentry.EmployeeID = employeeID
        AND employeetimeentry.EmployeeShiftID IS NOT NULL
        AND employeetimeentry.Date BETWEEN DATE_SUB(dateToCheck, INTERVAL 7 DAY) AND DATE_SUB(dateToCheck, INTERVAL 1 DAY)
        AND IF(
            employeeshift.RestDay OR (employee.DayOfRest = DAYOFWEEK(employeetimeentry.Date)),
            employeetimeentry.TotalDayPay > 0,
            TRUE
        )
        AND IF(
            payrate.PayType = 'Regular Holiday' OR payrate.PayType = 'Special Non-Working Holiday',
            employeetimeentry.TotalDayPay > 0,
            TRUE
        )
    ORDER BY employeetimeentry.Date DESC
    LIMIT 1
    INTO
        regularHours,
        leaveHours;
    
    RETURN regularHours > 0 OR leaveHours > 0;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
