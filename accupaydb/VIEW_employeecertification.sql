/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_employeecertification`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeecertification`(IN `ecert_EmployeeID` INT, IN `ecert_OrganizationID` INT)
    DETERMINISTIC
    COMMENT 'view all employee''s certification(s) base on employee RowID and organization'
BEGIN

SELECT
ecer.RowID
,COALESCE(ecer.EmployeeID,'') 'EmployeeID'
,COALESCE(ecer.CertificationType,'') 'CertificationType'
,COALESCE(ecer.IssuingAuthority,'') 'IssuingAuthority'
,COALESCE(ecer.CertificationNo,'') 'CertificationNo'
,COALESCE(DATE_FORMAT(ecer.IssueDate,'%m-%d-%Y'),'') 'IssueDate'
,COALESCE(DATE_FORMAT(ecer.ExpirationDate,'%m-%d-%Y'),'') 'ExpirationDate'
,COALESCE(ecer.Comments,'') 'Comments'
,DATE_FORMAT(ecer.Created,'%m-%d-%Y') 'Created'
,CONCAT(u.FirstName,' ',u.LastName) 'CreatedBy'
,COALESCE(DATE_FORMAT(ecer.LastUpd,'%m-%d-%Y'),'') 'LastUpd'
,COALESCE((SELECT CONCAT(FirstName,' ',LastName) FROM user WHERE RowID=ecer.LastUpdBy),'') 'LastUpdBy'
FROM employeecertification ecer
LEFT JOIN user u ON u.RowID=ecer.CreatedBy
WHERE ecer.OrganizationID=ecert_OrganizationID
AND ecer.EmployeeID=ecert_EmployeeID;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
