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

DECLARE custom_dateformat VARCHAR(50) DEFAULT '%c/%e/%Y';

SELECT
e.EmployeeID `DatCol1`
,CONCAT_WS(', ', e.LastName, e.FirstName, e.MiddleName) `DatCol2`
,pe.Name `DatCol3`
,pe.JobFunction `DatCol4`

,(@date_from := SUBSTRING_INDEX(pe.ExperienceFromTo, '@', 1))
,(@date_to := SUBSTRING_INDEX(pe.ExperienceFromTo, '@', -1))

,DATE_FORMAT(@date_from, custom_dateformat) `DatCol5`
,DATE_FORMAT(@date_to, custom_dateformat) `DatCol6`

FROM employeepreviousemployer pe
INNER JOIN employee e
        ON e.RowID=pe.EmployeeID AND e.OrganizationID=pe.OrganizationID
        AND FIND_IN_SET(e.EmploymentStatus, UNEMPLOYEMENT_STATUSES()) = 0
WHERE pe.OrganizationID=OrganizatID
ORDER BY CONCAT(e.LastName, e.FirstName)
         ,STR_TO_DATE(@date_from, @@date_format) DESC
         ,STR_TO_DATE(@date_to, @@date_format) DESC
;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
