/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_employeeloan_indate`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeeloan_indate`(IN `eloan_EmployeeID` INT, IN `eloan_OrganizationID` INT, IN `effectivedatefrom` DATE, IN `effectivedateto` DATE)
    DETERMINISTIC
BEGIN

DECLARE isEndOfMonth CHAR(1);

DECLARE month_type VARCHAR(150);


SELECT pp.`Half` FROM payperiod pp WHERE pp.OrganizationID=eloan_OrganizationID AND pp.PayFromDate=effectivedatefrom AND pp.PayToDate=effectivedateto AND pp.TotalGrossSalary=1 INTO isEndOfMonth;

SET month_type = IF(isEndOfMonth = '0', 'End of the month', 'First half');


SELECT
IFNULL(eloan.LoanNumber,'')                         `LoanNumber`
,IFNULL(FORMAT(eloan.TotalLoanAmount,2),0.00)   `TotalLoanAmount`
,IFNULL(FORMAT(eloan.TotalBalanceLeft,2),0.00)  `TotalBalanceLeft`
,IFNULL(FORMAT(eloan.DeductionAmount,2),0.00)   `DeductionAmount`
,IFNULL(FORMAT(eloan.DeductionPercentage,2),0.00)   `DeductionPercentage`
,IFNULL(eloan.DeductionSchedule,'')                 `DeductionSchedule`
,IFNULL(eloan.NoOfPayPeriod,0)                      `NoOfPayPeriod`
,IFNULL(eloan.Comments,'')                              `Comments`
,eloan.RowID
,IFNULL(eloan.`Status`,'')                  `Status`
,p.PartNo
 FROM employeeloanschedule eloan
 LEFT JOIN product p ON eloan.LoanTypeID=p.RowID
 WHERE eloan.EmployeeID=eloan_EmployeeID
 AND eloan.OrganizationID=eloan_OrganizationID
 AND eloan.DeductionSchedule IN (month_type,'Per pay period')
AND IF(eloan.DedEffectiveDateFrom < effectivedateto
,IF(MONTH(eloan.DedEffectiveDateFrom) = MONTH(effectivedateto), IF(DAY(eloan.DedEffectiveDateFrom) BETWEEN DAY(effectivedatefrom) AND DAY(effectivedateto), eloan.DedEffectiveDateFrom BETWEEN effectivedatefrom AND effectivedateto, eloan.DedEffectiveDateFrom<=effectivedateto), eloan.DedEffectiveDateFrom<=effectivedateto)
,eloan.DedEffectiveDateFrom<=effectivedateto);



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
