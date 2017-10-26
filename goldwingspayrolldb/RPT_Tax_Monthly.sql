/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_Tax_Monthly`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_Tax_Monthly`(IN `OrganizID` INT, IN `paramDateFrom` DATE, IN `paramDateTo` DATE)
    DETERMINISTIC
BEGIN

DECLARE deduc_sched VARCHAR(50);

SELECT PagIbigDeductionSchedule FROM organization WHERE RowID=OrganizID INTO deduc_sched;









SELECT
ee.TINNo 'TIN'
,CONCAT(ee.LastName,',',ee.FirstName, IF(ee.MiddleName='','',','),INITIALS(ee.MiddleName,'. ','1')) AS Fullname
,FORMAT(SUM(ps.TotalTaxableSalary),2) 'Taxable Gross'
,FORMAT(SUM(ps.TotalEmpWithholdingTax),2) 'Tax Withheld'
,(SELECT FORMAT(SUM(TotalTaxableSalary),2) FROM paystub WHERE EmployeeID=ps.EmployeeID AND OrganizationID=OrganizID AND PayFromDate>=MAKEDATE(YEAR(paramDateTo),1) AND PayToDate<=paramDateTo) 'Year To Date Tax Gross'
,(SELECT FORMAT(SUM(TotalEmpWithholdingTax),2) FROM paystub WHERE EmployeeID=ps.EmployeeID AND OrganizationID=OrganizID AND PayFromDate>=MAKEDATE(YEAR(paramDateTo),1) AND PayToDate<=paramDateTo) 'Year To Date Tax Withheld'
FROM paystub ps
LEFT JOIN employee ee ON ee.RowID=ps.EmployeeID AND ee.OrganizationID=ps.OrganizationID

INNER JOIN product pd ON pd.OrganizationID=OrganizID AND pd.PartNo='Gross Income'
LEFT JOIN paystubitem psi ON psi.PayStubID=ps.RowID AND psi.ProductID=pd.RowID AND psi.`Undeclared`='0'

WHERE ps.OrganizationID=OrganizID
AND ps.PayFromDate>=paramDateFrom
AND ps.PayToDate<=paramDateTo
GROUP BY ps.EmployeeID
ORDER BY ee.LastName;


END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
