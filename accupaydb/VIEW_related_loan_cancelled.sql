/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_related_loan_cancelled`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `VIEW_related_loan_cancelled`(IN `OrganizID` INT, IN `LoanCancelledRowID` INT)
    DETERMINISTIC
BEGIN

SELECT
els.RowID
,IFNULL(p.PartNo,'') AS PartNo
,els.LoanNumber
,els.TotalLoanAmount
,els.TotalBalanceLeft
,els.DedEffectiveDateFrom
,els.DeductionSchedule
,els.NoOfPayPeriod
,els.DedEffectiveDateTo
,els.LoanPayPeriodLeft
,els.DeductionAmount
,els.`Status`
,els.DeductionPercentage
,els.Comments
FROM employeeloanschedule els
LEFT JOIN product p ON p.RowID=els.LoanTypeID AND p.OrganizationID=els.OrganizationID
WHERE els.ReferenceLoanID=LoanCancelledRowID
AND els.OrganizationID=OrganizID
ORDER BY els.DedEffectiveDateFrom
			,els.DedEffectiveDateTo;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
