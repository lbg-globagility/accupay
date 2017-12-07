/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_salary_increase_histo`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_salary_increase_histo`(IN `OrganizID` INT, IN `PayPerDate1` DATE, IN `PayPerDate2` DATE)
    DETERMINISTIC
BEGIN


SELECT
    ee.RowID AS `DatCol1`,
    es.RowID AS `DatCol2`,
    ee.EmployeeID AS `DatCol3`,
    CONCAT(ee.LastName, ', ', ee.FirstName) AS `DatCol4`,
    DATE_FORMAT(es.EffectiveDateFrom, '%m/%d/%Y') AS `DatCol5`,
    DATE_FORMAT(es.EffectiveDateTo, '%m/%d/%Y') AS `DatCol6`,
    es.Salary AS `DatCol7`,
    es.UndeclaredSalary AS `DatCol8`,
    (es.Salary + es.UndeclaredSalary) AS `DatCol9`
FROM employeesalary es
INNER JOIN employee ee
ON ee.RowID = es.EmployeeID
WHERE es.OrganizationID = OrganizID
ORDER BY ee.LastName,
    ee.FirstName,
    ee.MiddleName,
    es.EffectiveDateFrom;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
