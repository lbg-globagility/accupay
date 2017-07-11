/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTUPD_employeeloanschedule`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_employeeloanschedule` AFTER UPDATE ON `employeeloanschedule` FOR EACH ROW BEGIN

DECLARE viewID INT(11);

SELECT RowID FROM `view` WHERE ViewName='Employee Loan Schedule' AND OrganizationID=NEW.OrganizationID LIMIT 1 INTO viewID;

IF OLD.LoanNumber != NEW.LoanNumber THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'LoanNumber',NEW.RowID,OLD.LoanNumber,NEW.LoanNumber,'Update');

END IF;

IF OLD.DedEffectiveDateFrom != NEW.DedEffectiveDateFrom THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'DedEffectiveDateFrom',NEW.RowID,OLD.DedEffectiveDateFrom,NEW.DedEffectiveDateFrom,'Update');

END IF;

IF OLD.DedEffectiveDateTo != NEW.DedEffectiveDateTo THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'DedEffectiveDateTo',NEW.RowID,OLD.DedEffectiveDateTo,NEW.DedEffectiveDateTo,'Update');

END IF;

IF OLD.TotalLoanAmount != NEW.TotalLoanAmount THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'TotalLoanAmount',NEW.RowID,OLD.TotalLoanAmount,NEW.TotalLoanAmount,'Update');

END IF;

IF OLD.DeductionSchedule != NEW.DeductionSchedule THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'DeductionSchedule',NEW.RowID,OLD.DeductionSchedule,NEW.DeductionSchedule,'Update');

END IF;

IF OLD.TotalBalanceLeft != NEW.TotalBalanceLeft THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'TotalBalanceLeft',NEW.RowID,OLD.TotalBalanceLeft,NEW.TotalBalanceLeft,'Update');

END IF;

IF OLD.DeductionAmount != NEW.DeductionAmount THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'DeductionAmount',NEW.RowID,OLD.DeductionAmount,NEW.DeductionAmount,'Update');

END IF;

IF OLD.Status != NEW.Status THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'Status',NEW.RowID,OLD.Status,NEW.Status,'Update');

END IF;

IF OLD.LoanTypeID != NEW.LoanTypeID THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'LoanTypeID',NEW.RowID,OLD.LoanTypeID,NEW.LoanTypeID,'Update');

END IF;

IF OLD.DeductionPercentage != NEW.DeductionPercentage THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'DeductionPercentage',NEW.RowID,OLD.DeductionPercentage,NEW.DeductionPercentage,'Update');

END IF;

IF OLD.NoOfPayPeriod != NEW.NoOfPayPeriod THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'NoOfPayPeriod',NEW.RowID,OLD.NoOfPayPeriod,NEW.NoOfPayPeriod,'Update');

END IF;

IF OLD.LoanPayPeriodLeft != NEW.LoanPayPeriodLeft THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'LoanPayPeriodLeft',NEW.RowID,OLD.LoanPayPeriodLeft,NEW.LoanPayPeriodLeft,'Update');

END IF;

IF OLD.Comments != NEW.Comments THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'Comments',NEW.RowID,OLD.Comments,NEW.Comments,'Update');

END IF;




END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
