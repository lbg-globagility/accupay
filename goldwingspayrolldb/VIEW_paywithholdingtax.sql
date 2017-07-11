/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_paywithholdingtax`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_paywithholdingtax`(IN `Taxable_Income` DECIMAL(10,6), IN `e_EmployeeID` INT, IN `e_OrganizationID` INT)
    DETERMINISTIC
BEGIN

DECLARE fstat_MaritalStatus VARCHAR(50);

DECLARE fstat_Dependent INT(11) DEFAULT 1;

DECLARE pfq_RowID INT(11) DEFAULT 1;

DECLARE empBasicPay DECIMAL(11,6) DEFAULT 1;

DECLARE wtax_taxabinc DECIMAL(11,6) DEFAULT 1;

DECLARE p_phhealth DECIMAL(11,6) DEFAULT 1;

DECLARE p_sss DECIMAL(11,6) DEFAULT 1;

DECLARE p_hdmf DECIMAL(11,6) DEFAULT 50;

SELECT MaritalStatus FROM employee WHERE RowID=e_EmployeeID AND OrganizationID=e_OrganizationID INTO fstat_MaritalStatus;

SELECT NoOfDependents FROM employee WHERE RowID=e_EmployeeID AND OrganizationID=e_OrganizationID INTO fstat_Dependent;

SELECT COALESCE(PayFrequencyID,1) FROM employee WHERE RowID=e_EmployeeID AND OrganizationID=e_OrganizationID INTO pfq_RowID;

SELECT BasicPay FROM employeesalary WHERE EmployeeID=e_EmployeeID AND OrganizationID=e_OrganizationID AND DATE(DATE_FORMAT(NOW(),'%Y-%m-%d')) BETWEEN DATE(COALESCE(EffectiveDateFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveDateTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) INTO empBasicPay;

SELECT IF(pfq_RowID = 1,EmployeeShare / 2,EmployeeShare) FROM payphilhealth WHERE empBasicPay BETWEEN SalaryRangeFrom AND SalaryRangeTo INTO p_phhealth;

SELECT IF(pfq_RowID = 1,EmployeeContributionAmount / 2,EmployeeContributionAmount) FROM paysocialsecurity WHERE empBasicPay BETWEEN RangeFromAmount AND RangeToAmount INTO p_sss;

SET p_hdmf = IF(pfq_RowID = 1,IF(empBasicPay * 0.02 > 50,p_hdmf,empBasicPay * 0.02),IF(empBasicPay * 0.02 > 50,100,empBasicPay * 0.02));

SET wtax_taxabinc = (empBasicPay) - (p_phhealth + p_sss + p_hdmf);

SELECT pwtax.RowID 'pwtax_RowID'
,pwtax.Created 'pwtax_Created'
,COALESCE(pwtax.LastUpd,'') 'pwtax_LastUpd'
,pwtax.CreatedBy 'pwtax_CreatedBy'
,COALESCE(pwtax.LastUpdBy,'') 'pwtax_LastUpdBy'
,pwtax.PayFrequencyID 'pwtax_PayFrequencyID'
,pwtax.FilingStatusID 'pwtax_FilingStatusID'
,fstat.FilingStatus  'fstat_FilingStatus'
,fstat.MaritalStatus 'fstat_MaritalStatus'
,fstat.Dependent 'fstat_Dependent'
,COALESCE(pwtax.EffectiveDateFrom,NOW()) 'pwtax_EffectiveDateFrom'
,COALESCE(pwtax.EffectiveDateTo,NOW()) 'pwtax_EffectiveDateTo'
,pwtax.ExemptionAmount 'pwtax_ExemptionAmount'
,pwtax.ExemptionInExcessAmount 'pwtax_ExemptionInExcessAmount'
,pwtax.TaxableIncomeFromAmount 'pwtax_TaxableIncomeFromAmount'
,pwtax.TaxableIncomeToAmount 'pwtax_TaxableIncomeToAmount'
,Taxable_Income - (pwtax.ExemptionAmount + ((Taxable_Income - pwtax.TaxableIncomeFromAmount) * pwtax.ExemptionInExcessAmount)) 'computed_wtax'
,wtax_taxabinc - (pwtax.ExemptionAmount + ((wtax_taxabinc - pwtax.TaxableIncomeFromAmount) * pwtax.ExemptionInExcessAmount)) 'partial_computed_wtax'
 FROM paywithholdingtax pwtax
 LEFT JOIN filingstatus fstat ON fstat.RowID=pwtax.FilingStatusID
 WHERE Taxable_Income BETWEEN pwtax.TaxableIncomeFromAmount AND pwtax.TaxableIncomeToAmount
AND fstat.MaritalStatus=fstat_MaritalStatus
AND fstat.Dependent=fstat_Dependent
AND pwtax.PayFrequencyID=pfq_RowID
AND NOW() BETWEEN COALESCE(pwtax.EffectiveDateFrom,NOW()) AND COALESCE(pwtax.EffectiveDateTo,NOW());




END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
