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
ee.EmployeeID
,CONCAT(ee.LastName,',',ee.FirstName, IF(ee.MiddleName='','',','),INITIALS(ee.MiddleName,'. ','1')) 'Fullname'
,DATE_FORMAT(ep.EffectiveDate,'%m/%e/%Y') 'EffectiveDate'
,IFNULL(esal.BasicPay,(SELECT BasicPay FROM employeesalary WHERE EmployeeID=ep.EmployeeID AND OrganizationID=ep.OrganizationID AND ep.EffectiveDate BETWEEN EffectiveDateFrom AND IFNULL(EffectiveDateTo,CURDATE()) ORDER BY IFNULL(EffectiveDateTo,CURDATE()) DESC LIMIT 1)) 'BasicPay'
FROM employeepromotions ep
LEFT JOIN employeesalary esal ON esal.RowID=ep.EmployeeSalaryID AND esal.OrganizationID=ep.OrganizationID
LEFT JOIN employee ee ON ee.RowID=ep.EmployeeID AND ee.OrganizationID=ep.OrganizationID
WHERE ep.OrganizationID=OrganizID
AND ep.EffectiveDate BETWEEN PayPerDate1 AND PayPerDate2
GROUP BY ep.RowID
ORDER BY ee.LastName,ep.EffectiveDate;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
