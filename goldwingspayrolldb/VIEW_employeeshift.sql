/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_employeeshift`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeeshift`(IN `OrganizID` INT, IN `EmpRowID` INT, IN `pagenumber` INT)
    DETERMINISTIC
BEGIN

SELECT
    ee.EmployeeID,
    CONCAT(ee.LastName,',',ee.FirstName,',',INITIALS(ee.MiddleName,'.','1')) AS Name,
    es.RowID,
    es.EffectiveFrom,
    IF(es.EffectiveTo != es.EffectiveFrom, es.EffectiveTo, NULL),
    COALESCE(TIME_FORMAT(s.TimeFrom, '%H:%i'), '') timef,
    COALESCE(TIME_FORMAT(s.TimeTo, '%H:%i'), '') timet,
    TIME_FORMAT(s.BreaktimeFrom, '%H:%i'),
    TIME_FORMAT(s.BreaktimeTo, '%H:%i')
FROM employeeshift es
LEFT JOIN shift s ON es.ShiftID = s.RowID
INNER JOIN employee ee ON es.EmployeeID = ee.RowID
WHERE es.OrganizationID = OrganizID AND
    ee.RowID = EmpRowID
ORDER BY es.EffectiveTo DESC, es.EffectiveFrom DESC
LIMIT pagenumber, 50;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
