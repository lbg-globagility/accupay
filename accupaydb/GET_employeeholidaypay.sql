/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `GET_employeeholidaypay`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `GET_employeeholidaypay`(IN `OrganizID` INT, IN `etentDateFrom` DATE, IN `etentDateTo` DATE)
    DETERMINISTIC
BEGIN

SELECT
ete.EmployeeID
,SUM(((GET_employeerateperhour(ete.EmployeeID,ete.OrganizationID,ete.Date) *
IF(ete.TotalHoursWorked - ete.RegularHoursWorked < 0, ete.TotalHoursWorked, ete.TotalHoursWorked - (ete.TotalHoursWorked - ete.RegularHoursWorked))) *
IF(esh.NightShift='0',pr.PayRate,pr.NightDifferentialRate)) *
(IF(esh.NightShift='0',pr.PayRate,pr.NightDifferentialRate) - 1) /
IF(esh.NightShift='0',pr.PayRate,pr.NightDifferentialRate)) AS HolidayPayResult
FROM employeetimeentry ete
LEFT JOIN employeeshift esh ON esh.EmployeeID=ete.EmployeeID AND ete.Date BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
LEFT JOIN payrate pr ON pr.RowID=ete.PayRateID
WHERE pr.PayType='Regular Holiday'
AND ete.OrganizationID=OrganizID
AND ete.Date BETWEEN etentDateFrom AND etentDateTo
GROUP BY ete.EmployeeID;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
