/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_payroll_legder`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_payroll_legder`(IN `OrganizID` INT, IN `PayPerID1` INT, IN `PayPerID2` INT, IN `psi_undeclared` CHAR(1))
    DETERMINISTIC
BEGIN

DECLARE PayP_Date1 DATE;
DECLARE PayP_Date2 DATE;

SELECT PayFromDate
FROM payperiod
WHERE RowID = PayPerID1
INTO PayP_Date1;

SELECT PayToDate
FROM payperiod
WHERE RowID = PayPerID2
INTO PayP_Date2;

SELECT
    ee.RowID AS `DatCol1`,
    ee.EmployeeID AS `DatCol2`,
    CONCAT(ee.LastName,',',ee.FirstName, IF(ee.MiddleName='','',','),INITIALS(ee.MiddleName,'. ','1')) AS `DatCol3`,
    DATE_FORMAT(ps.PayFromDate, '%m/%d/%Y') AS `DatCol4`,
    DATE_FORMAT(ps.PayToDate, '%m/%d/%Y') AS `DatCol5`,
    ps.TotalGrossSalary AS `DatCol6`,
    ps.TotalEmpSSS AS `DatCol7`,
    ps.TotalEmpPhilHealth AS `DatCol8`,
    ps.TotalEmpHDMF AS `DatCol9`,
    ps.TotalLoans AS `DatCol10`,
    ps.TotalAdjustments AS `DatCol11`,
    ps.TotalNetSalary AS `DatCol12`
FROM paystub ps
INNER JOIN employee ee
ON ee.RowID = ps.EmployeeID AND FIND_IN_SET(ee.EmploymentStatus, UNEMPLOYEMENT_STATUSES()) = 0
WHERE ps.OrganizationID = OrganizID AND
    ps.PayPeriodID BETWEEN PayPerID1 AND PayPerID2
ORDER BY ee.LastName, ps.PayToDate;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
