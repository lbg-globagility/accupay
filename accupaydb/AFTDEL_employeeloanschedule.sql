/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTDEL_employeeloanschedule`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTDEL_employeeloanschedule` AFTER DELETE ON `employeeloanschedule` FOR EACH ROW BEGIN

DECLARE loan_view_name TEXT DEFAULT 'Employee Loan Schedule';

DECLARE user_rowid
        ,view_id INT(11);

SET user_rowid = IFNULL(OLD.LastUpdBy, OLD.CreatedBy);

SELECT v.RowID
FROM `view` v
WHERE v.OrganizationID=OLD.OrganizationID
AND v.ViewName=loan_view_name
LIMIT 1
INTO view_id
;

INSERT INTO audittrail(Created,CreatedBy,LastUpd,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES
(CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'RowID', OLD.RowID, OLD.RowID, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'OrganizationID', OLD.RowID, OLD.OrganizationID, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'Created', OLD.RowID, OLD.Created, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'CreatedBy', OLD.RowID, OLD.CreatedBy, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'LastUpd', OLD.RowID, OLD.LastUpd, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'LastUpdBy', OLD.RowID, OLD.LastUpdBy, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'EmployeeID', OLD.RowID, OLD.EmployeeID, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'LoanNumber', OLD.RowID, OLD.LoanNumber, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'DedEffectiveDateFrom', OLD.RowID, OLD.DedEffectiveDateFrom, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'DedEffectiveDateTo', OLD.RowID, OLD.DedEffectiveDateTo, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'TotalLoanAmount', OLD.RowID, OLD.TotalLoanAmount, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'DeductionSchedule', OLD.RowID, OLD.DeductionSchedule, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'TotalBalanceLeft', OLD.RowID, OLD.TotalBalanceLeft, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'DeductionAmount', OLD.RowID, OLD.DeductionAmount, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'Status', OLD.RowID, OLD.Status, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'LoanTypeID', OLD.RowID, OLD.LoanTypeID, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'DeductionPercentage', OLD.RowID, OLD.DeductionPercentage, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'NoOfPayPeriod', OLD.RowID, OLD.NoOfPayPeriod, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'LoanPayPeriodLeft', OLD.RowID, OLD.LoanPayPeriodLeft, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'Comments', OLD.RowID, OLD.Comments, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'BonusID', OLD.RowID, OLD.BonusID, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'LoanName', OLD.RowID, OLD.LoanName, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'LoanPayPeriodLeftForBonus', OLD.RowID, OLD.LoanPayPeriodLeftForBonus, NULL, 'Delete')
, (CURRENT_TIMESTAMP(), user_rowid, CURRENT_TIMESTAMP(), user_rowid, OLD.OrganizationID, view_id, 'BonusPotentialPaymentForLoan', OLD.RowID, OLD.BonusPotentialPaymentForLoan, NULL, 'Delete')
;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
