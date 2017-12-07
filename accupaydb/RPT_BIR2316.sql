/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_BIR2316`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_BIR2316`(IN `OrganizID` INT, IN `paramDateFrom` DATE, IN `paramDateTo` DATE)
    DETERMINISTIC
BEGIN

SELECT e.RowID
,IFNULL(REPLACE(e.TINNo,'-',' '),'') 'EmployeeTIN'
,CONCAT(e.LastName,', ',e.FirstName,IF(e.MiddleName='','',CONCAT(', ',e.MiddleName))) 'EmployeeFullName'
,e.HomeAddress
,DATE_FORMAT(e.Birthdate,'%m%d%Y') 'Birthdate'
,e.HomePhone
,e.MaritalStatus
,IFNULL(REPLACE(og.TINNo,'-',' '),'') 'og_TIN'
,og.Name
,CONCAT(ad.StreetAddress1,IF_NOT_EMPTY_STRING(ad.StreetAddress2,CONCAT(', ',ad.StreetAddress2)),IF_NOT_EMPTY_STRING(ad.Barangay,CONCAT(', ',ad.Barangay)),IF_NOT_EMPTY_STRING(ad.CityTown,CONCAT(', ',ad.CityTown)),IF_NOT_EMPTY_STRING(ad.State,CONCAT(', ',ad.State)),IF_NOT_EMPTY_STRING(ad.Country,CONCAT(', ',ad.Country))) 'FullAddress'
,ad.ZipCode
FROM employee e
LEFT JOIN employeesalary es ON es.EmployeeID=e.RowID
LEFT JOIN paystub ps ON ps.EmployeeID=e.RowID
LEFT JOIN paystubitem psi ON psi.PayStubID=ps.RowID
LEFT JOIN organization og ON og.RowID=e.OrganizationID
LEFT JOIN address ad ON ad.RowID=og.PrimaryAddressID
WHERE e.OrganizationID=OrganizID
AND e.EmploymentStatus NOT IN ('Resigned','Terminated')
GROUP BY e.RowID
ORDER BY e.LastName,e.FirstName,e.MiddleName;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
