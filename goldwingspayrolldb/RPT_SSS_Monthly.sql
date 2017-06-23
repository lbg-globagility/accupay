-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.11-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.0.0.4396
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for procedure goldwingspayrolldb.RPT_SSS_Monthly
DROP PROCEDURE IF EXISTS `RPT_SSS_Monthly`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_SSS_Monthly`(IN `OrganizID` INT, IN `paramDate` DATE)
    DETERMINISTIC
BEGIN

DECLARE deduc_sched VARCHAR(50);

DECLARE ii INT(11) DEFAULT -1;

DECLARE indx INT(11) DEFAULT -1;

DECLARE mirr_EmpID INT(11) DEFAULT -1;

DECLARE mirr_Amount DECIMAL(10,2) DEFAULT -1;


DECLARE semimo_paydatefrom DATE;

DECLARE semimo_paydateto DATE;

DECLARE wk_paydatefrom DATE;

DECLARE wk_paydateto DATE;


SELECT PagIbigDeductionSchedule FROM organization WHERE RowID=OrganizID INTO deduc_sched;




SELECT pyp.PayFromDate FROM payperiod pyp WHERE pyp.OrganizationID=OrganizID AND pyp.`Year`=YEAR(paramDate) AND pyp.`Month`=(MONTH(paramDate) * 1) AND pyp.TotalGrossSalary=1 ORDER BY pyp.PayFromDate, pyp.PayToDate LIMIT 1 INTO semimo_paydatefrom;
	
SELECT pyp.PayToDate FROM payperiod pyp WHERE pyp.OrganizationID=OrganizID AND pyp.`Year`=YEAR(paramDate) AND pyp.`Month`=(MONTH(paramDate) * 1) AND pyp.TotalGrossSalary=1 ORDER BY pyp.PayFromDate DESC, pyp.PayToDate DESC LIMIT 1 INTO semimo_paydateto;
	
	
SELECT pyp.PayFromDate FROM payperiod pyp WHERE pyp.OrganizationID=OrganizID AND pyp.`Year`=YEAR(paramDate) AND pyp.`Month`=(MONTH(paramDate) * 1) AND pyp.TotalGrossSalary=4 ORDER BY pyp.PayFromDate, pyp.PayToDate LIMIT 1 INTO wk_paydatefrom;
	
SELECT pyp.PayToDate FROM payperiod pyp WHERE pyp.OrganizationID=OrganizID AND pyp.`Year`=YEAR(paramDate) AND pyp.`Month`=(MONTH(paramDate) * 1) AND pyp.TotalGrossSalary=4 ORDER BY pyp.PayFromDate DESC, pyp.PayToDate DESC LIMIT 1 INTO wk_paydateto;
	
	SELECT 
	ee.SSSNo
	,CONCAT(ee.LastName,',',ee.FirstName, IF(ee.MiddleName='','',','),INITIALS(ee.MiddleName,'. ','1')) AS Fullname
	,psi.PayAmount AS EmployeeContributionAmount
	,pss.EmployerContributionAmount
	,pss.EmployeeECAmount 
	,(psi.PayAmount + (pss.EmployerContributionAmount + pss.EmployeeECAmount)) 'Total'
	FROM paystub ps
	INNER JOIN employee ee ON ee.RowID=ps.EmployeeID AND ee.PayFrequencyID=1
	INNER JOIN product p ON p.PartNo='.SSS' AND p.OrganizationID=OrganizID
	INNER JOIN paystubitem psi ON psi.PayStubID=ps.RowID AND psi.OrganizationID=OrganizID AND psi.ProductID=p.RowID
	INNER JOIN paysocialsecurity pss ON pss.EmployeeContributionAmount=psi.PayAmount
	WHERE ps.OrganizationID=OrganizID
	AND (ps.PayFromDate>=semimo_paydatefrom OR ps.PayToDate>=semimo_paydatefrom)
	AND (ps.PayToDate<=semimo_paydateto OR ps.PayToDate<=semimo_paydateto)
	AND IFNULL(psi.PayAmount,0)!=0
UNION
	SELECT 
	ee.SSSNo
	,CONCAT(ee.LastName,',',ee.FirstName, IF(ee.MiddleName='','',','),INITIALS(ee.MiddleName,'. ','1')) AS Fullname
	,psi.PayAmount AS EmployeeContributionAmount
	,pss.EmployerContributionAmount
	,pss.EmployeeECAmount 
	,(psi.PayAmount + (pss.EmployerContributionAmount + pss.EmployeeECAmount)) 'Total'
	FROM paystub ps
	INNER JOIN employee ee ON ee.RowID=ps.EmployeeID AND ee.PayFrequencyID=4
	INNER JOIN product p ON p.PartNo='.SSS' AND p.OrganizationID=OrganizID
	INNER JOIN paystubitem psi ON psi.PayStubID=ps.RowID AND psi.OrganizationID=OrganizID AND psi.ProductID=p.RowID
	INNER JOIN paysocialsecurity pss ON pss.EmployeeContributionAmount=psi.PayAmount
	WHERE ps.OrganizationID=OrganizID
	AND (ps.PayFromDate>=wk_paydatefrom OR ps.PayToDate>=wk_paydatefrom)
	AND (ps.PayToDate<=wk_paydateto OR ps.PayToDate<=wk_paydateto)
	AND IFNULL(psi.PayAmount,0)!=0;



END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
