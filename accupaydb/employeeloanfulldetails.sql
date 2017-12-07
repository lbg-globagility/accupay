/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `employeeloanfulldetails`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `employeeloanfulldetails`(IN `OrganizID` INT, IN `paypPayFreqID` INT, IN `pay_fromdate` DATE, IN `pay_todate` DATE)
    DETERMINISTIC
BEGIN

SELECT e.RowID AS EmpRowID
,e.EmployeeID
,ps.RowID
,p.RowID
,p.PartNo
,SUM(IFNULL(psi.PayAmount,0)) AS PayAmount
,SUM(IFNULL(elh.DeductionAmount,0)) AS DeductionAmount
,IFNULL((els.TotalLoanAmount - ehi.PaidLoanAmount),0) AS CurrentBalance
FROM product p
INNER JOIN employee e ON e.OrganizationID=OrganizID AND e.PayFrequencyID=paypPayFreqID
LEFT JOIN payperiod pyp ON pyp.OrganizationID=e.OrganizationID AND pyp.TotalGrossSalary=e.PayFrequencyID AND pyp.PayFromDate=pay_fromdate AND pyp.PayToDate=pay_todate
LEFT JOIN paystub ps ON ps.OrganizationID=e.OrganizationID AND ps.EmployeeID=e.RowID AND ps.PayPeriodID=pyp.RowID
LEFT JOIN paystubitem psi ON psi.ProductID=p.RowID AND psi.OrganizationID=e.OrganizationID AND psi.PayStubID=ps.RowID
LEFT JOIN employeeloanhistory elh ON elh.EmployeeID=e.RowID AND elh.OrganizationID=e.OrganizationID AND elh.Comments=p.PartNo AND elh.PayStubID=ps.RowID
LEFT JOIN employeeloanhistoitem ehi ON ehi.LoanHistoID=elh.RowID
LEFT JOIN employeeloanschedule els ON els.RowID=ehi.EmpLoanID AND LOCATE(els.DeductionSchedule, IF(pyp.`Half`='0', 'End of the month,Per pay period', IF(pyp.`Half`='2', 'First half,Per pay period', 'Per pay period'))) > 0
WHERE p.OrganizationID=OrganizID
AND p.`Category`='Loan Type'
GROUP BY e.RowID,p.RowID
ORDER BY e.EmployeeID,p.PartNo;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
