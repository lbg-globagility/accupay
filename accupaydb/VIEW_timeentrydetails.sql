/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_timeentrydetails`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `VIEW_timeentrydetails`(IN `OrganizID` INT, IN `Page_Number` INT)
    DETERMINISTIC
BEGIN

SELECT

DATE_FORMAT(etdet.Created,'%m/%d/%Y %h:%i %p') 'Created'
,DATE_FORMAT(etdet.Created,'%Y-%m-%d %H:%i:%s') 'createdmilit'
,COALESCE(CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1))
,SUBSTRING(u.LastName, 2))),'') 'Created by'
,COALESCE(DATE_FORMAT(etdet.LastUpd,'%b-%d-%Y'),'') 'Last Update'
,COALESCE((SELECT CONCAT(CONCAT(UCASE(LEFT(FirstName, 1)), SUBSTRING(FirstName, 2)),' ',CONCAT(UCASE(LEFT(LastName, 1))
,SUBSTRING(LastName, 2))) FROM user WHERE RowID=etdet.LastUpd),'') 'Last update by'
,etdet.TimeentrylogsImportID



FROM employeetimeentrydetails etdet
LEFT JOIN user u ON etdet.CreatedBy=u.RowID
LEFT JOIN employee e ON etdet.EmployeeID = e.RowID
WHERE etdet.OrganizationID=OrganizID
GROUP BY etdet.TimeentrylogsImportID
ORDER BY etdet.RowID DESC LIMIT Page_Number,100;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
