/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_AlphaListempdependents`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_AlphaListempdependents`(IN `OrganizID` INT)
    DETERMINISTIC
BEGIN

SELECT
ed.ParentEmployeeID
,ed.FirstName
,CONCAT(IF(ed.MiddleName = '', '', ' '),INITIALS(ed.MiddleName,'.','1')) MiddleName
,IF(ed.LastName = '', ed.LastName, CONCAT(' ',ed.LastName)) LastName
,DATE_FORMAT(ed.Birthdate,'%m%d%Y') Birthdate
FROM employeedependents ed
WHERE ed.OrganizationID=OrganizID
AND ed.ActiveFlag='Y';

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
