/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_PAGIBIG_Monthly`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_PAGIBIG_Monthly`(IN `OrganizID` INT, IN `paramDate` DATE)
    DETERMINISTIC
BEGIN

DECLARE deduc_sched VARCHAR(50);

DECLARE semi_payfrom DATE;

DECLARE semi_payto DATE;


DECLARE weekly_payfrom DATE;

DECLARE weekly_payto DATE;


DECLARE row_counts INT(11);

DECLARE month_date INT(11);

DECLARE dedcutioncategID INT(11);


SELECT RowID FROM category WHERE CategoryName='Deductions' AND OrganizationID=OrganizID INTO dedcutioncategID;


SET month_date = DATE_FORMAT(paramDate,'%m');

SELECT PagIbigDeductionSchedule FROM organization WHERE RowID=OrganizID INTO deduc_sched;


SELECT COUNT(pyp.RowID) FROM payperiod pyp WHERE pyp.OrganizationID=OrganizID AND pyp.`Month`=month_date AND pyp.`Year`=YEAR(paramDate) AND pyp.TotalGrossSalary=1 INTO row_counts;

SELECT pyp.PayFromDate FROM payperiod pyp WHERE pyp.OrganizationID=OrganizID AND pyp.`Month`=month_date AND pyp.`Year`=YEAR(paramDate) AND pyp.TotalGrossSalary=1 ORDER BY pyp.PayFromDate, pyp.PayToDate LIMIT 1 INTO semi_payfrom;

IF row_counts > 0 THEN

    SET row_counts = row_counts - 1;

END IF;

SELECT pyp.PayToDate FROM payperiod pyp WHERE pyp.OrganizationID=OrganizID AND pyp.`Month`=month_date AND pyp.`Year`=YEAR(paramDate) AND pyp.TotalGrossSalary=1 ORDER BY pyp.PayFromDate, pyp.PayToDate LIMIT row_counts,1 INTO semi_payto;




SELECT COUNT(pyp.RowID) FROM payperiod pyp WHERE pyp.OrganizationID=OrganizID AND pyp.`Month`=month_date AND pyp.`Year`=YEAR(paramDate) AND pyp.TotalGrossSalary=4 INTO row_counts;

SELECT pyp.PayFromDate FROM payperiod pyp WHERE pyp.OrganizationID=OrganizID AND pyp.`Month`=month_date AND pyp.`Year`=YEAR(paramDate) AND pyp.TotalGrossSalary=4 ORDER BY pyp.PayFromDate, pyp.PayToDate LIMIT 1 INTO weekly_payfrom;

IF row_counts > 0 THEN

    SET row_counts = row_counts - 1;

END IF;

SELECT pyp.PayToDate FROM payperiod pyp WHERE pyp.OrganizationID=OrganizID AND pyp.`Month`=month_date AND pyp.`Year`=YEAR(paramDate) AND pyp.TotalGrossSalary=4 ORDER BY pyp.PayFromDate DESC, pyp.PayToDate DESC LIMIT 1 INTO weekly_payto;




SELECT pyp.PayFromDate FROM payperiod pyp WHERE pyp.OrganizationID=OrganizID AND pyp.`Year`=YEAR(paramDate) AND pyp.`Month`=(MONTH(paramDate) * 1) AND pyp.TotalGrossSalary=1 ORDER BY pyp.PayFromDate, pyp.PayToDate LIMIT 1 INTO semi_payfrom;

SELECT pyp.PayToDate FROM payperiod pyp WHERE pyp.OrganizationID=OrganizID AND pyp.`Year`=YEAR(paramDate) AND pyp.`Month`=(MONTH(paramDate) * 1) AND pyp.TotalGrossSalary=1 ORDER BY pyp.PayFromDate DESC, pyp.PayToDate DESC LIMIT 1 INTO semi_payto;


SELECT pyp.PayFromDate FROM payperiod pyp WHERE pyp.OrganizationID=OrganizID AND pyp.`Year`=YEAR(paramDate) AND pyp.`Month`=(MONTH(paramDate) * 1) AND pyp.TotalGrossSalary=4 ORDER BY pyp.PayFromDate, pyp.PayToDate LIMIT 1 INTO  weekly_payfrom;

SELECT pyp.PayToDate FROM payperiod pyp WHERE pyp.OrganizationID=OrganizID AND pyp.`Year`=YEAR(paramDate) AND pyp.`Month`=(MONTH(paramDate) * 1) AND pyp.TotalGrossSalary=4 ORDER BY pyp.PayFromDate DESC, pyp.PayToDate DESC LIMIT 1 INTO weekly_payto;

SELECT
    e.HDMFNo
    ,CONCAT(e.LastName,',',e.FirstName, IF(e.MiddleName='','',','),INITIALS(e.MiddleName,'. ','1')) AS Fullname
    ,psi.PayAmount AS EmployeeContributionAmount
    ,ps.TotalCompHDMF AS EmployerContributionAmount
    ,psi.PayAmount + ps.TotalCompHDMF AS TotalContribution
    FROM paystubitem psi
    INNER JOIN employee e ON e.OrganizationID=OrganizID AND e.PayFrequencyID=1 AND e.EmploymentStatus='Regular'
    INNER JOIN paystub ps ON ps.OrganizationID=OrganizID AND ps.EmployeeID=e.RowID AND (ps.PayFromDate>=semi_payfrom OR ps.PayToDate>=semi_payfrom) AND (ps.PayFromDate<=semi_payto OR ps.PayToDate<=semi_payto)
    JOIN category c ON c.OrganizationID=OrganizID AND c.CategoryName='Deductions'
    JOIN product p ON p.CategoryID=c.RowID AND p.OrganizationID=OrganizID AND p.PartNo = '.PAGIBIG'
    WHERE psi.ProductID=p.RowID
    AND psi.PayStubID=ps.RowID
    AND IFNULL(psi.PayAmount,0)!=0
UNION ALL
    SELECT
    e.HDMFNo
    ,CONCAT(e.LastName,',',e.FirstName, IF(e.MiddleName='','',','),INITIALS(e.MiddleName,'. ','1')) AS Fullname
    ,psi.PayAmount AS EmployeeContributionAmount
    ,ps.TotalCompHDMF AS EmployerContributionAmount
    ,psi.PayAmount + ps.TotalCompHDMF AS TotalContribution
    FROM paystubitem psi
    INNER JOIN employee e ON e.OrganizationID=OrganizID AND e.PayFrequencyID=4 AND e.EmploymentStatus='Regular'
    INNER JOIN paystub ps ON ps.OrganizationID=OrganizID AND ps.EmployeeID=e.RowID AND (ps.PayFromDate>=weekly_payfrom OR ps.PayToDate>=weekly_payfrom) AND (ps.PayFromDate<=weekly_payto OR ps.PayToDate<=weekly_payto)
    JOIN category c ON c.OrganizationID=OrganizID AND c.CategoryName='Deductions'
    JOIN product p ON p.CategoryID=c.RowID AND p.OrganizationID=OrganizID AND p.PartNo = '.PAGIBIG'
    WHERE psi.ProductID=p.RowID
    AND psi.PayStubID=ps.RowID
    AND IFNULL(psi.PayAmount,0)!=0;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
