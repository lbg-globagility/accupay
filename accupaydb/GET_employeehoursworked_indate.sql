/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `GET_employeehoursworked_indate`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GET_employeehoursworked_indate`(`employee_ID` INT, `d_from` DATE, `d_to` DATE, `organiz_ID` INT) RETURNS decimal(10,2)
    DETERMINISTIC
BEGIN

DECLARE returnval DECIMAL(10,2) DEFAULT 0;

SELECT SUM(IF(esh.NightShift = '1', ete.NightDifferentialHours, ete.RegularHoursWorked))
FROM employeetimeentry ete
LEFT JOIN employeeshift esh ON esh.RowID=ete.EmployeeShiftID
WHERE ete.EmployeeID=employee_ID
AND ete.OrganizationID=organiz_ID
AND ete.Date BETWEEN d_from AND d_to
INTO returnval;

SET returnval = IFNULL(returnval,0);

RETURN returnval;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
