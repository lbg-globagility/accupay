/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_employment_record`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_employment_record`(IN `OrganizatID` INT)
    DETERMINISTIC
BEGIN

SELECT
ee.EmployeeID
,CONCAT(ee.LastName,',',ee.FirstName, IF(ee.MiddleName='','',','),INITIALS(ee.MiddleName,'. ','1')) 'Fullname'
,epe.Name
,epe.JobTitle
,DATE_FORMAT((SELECT IF(LOCATE('@',ExperienceFromTo) > 0, SUBSTRING_INDEX(ExperienceFromTo, '@', 1), ExperienceFromTo) FROM employeepreviousemployer WHERE RowID=epe.RowID),'%b %e, %Y') AS ExperienceFrom
,DATE_FORMAT((SELECT IF(LOCATE('@',ExperienceFromTo) > 0, REVERSE(SUBSTRING_INDEX(REVERSE(ExperienceFromTo), '@', 1)), ExperienceFromTo) FROM employeepreviousemployer WHERE RowID=epe.RowID),'%b %e, %Y') AS ExperienceTo
FROM employeepreviousemployer epe
LEFT JOIN employee ee ON ee.RowID=epe.EmployeeID
WHERE epe.OrganizationID=OrganizatID
ORDER BY ee.LastName,ExperienceTo DESC;




END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
