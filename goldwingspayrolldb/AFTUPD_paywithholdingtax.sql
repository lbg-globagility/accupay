/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTUPD_paywithholdingtax`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_paywithholdingtax` AFTER UPDATE ON `paywithholdingtax` FOR EACH ROW BEGIN

DECLARE viewID INT(11);

DECLARE OrgRowID INT(11);

SET OrgRowID = 2;

SELECT RowID FROM `view` WHERE ViewName='Withholding Tax Table' AND OrganizationID=OrgRowID LIMIT 1 INTO viewID;

IF OLD.PayFrequencyID != NEW.PayFrequencyID THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'PayFrequencyID',NEW.RowID,OLD.PayFrequencyID,NEW.PayFrequencyID,'Update');

END IF;

IF OLD.FilingStatusID != NEW.FilingStatusID THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'FilingStatusID',NEW.RowID,OLD.FilingStatusID,NEW.FilingStatusID,'Update');

END IF;

IF OLD.EffectiveDateFrom != NEW.EffectiveDateFrom THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'EffectiveDateFrom',NEW.RowID,IFNULL(OLD.EffectiveDateFrom,''),IFNULL(NEW.EffectiveDateFrom,''),'Update');

END IF;

IF OLD.EffectiveDateTo != NEW.EffectiveDateTo THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'EffectiveDateTo',NEW.RowID,IFNULL(OLD.EffectiveDateTo,''),IFNULL(NEW.EffectiveDateTo,''),'Update');

END IF;

IF OLD.ExemptionAmount != NEW.ExemptionAmount THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'ExemptionAmount',NEW.RowID,OLD.ExemptionAmount,NEW.ExemptionAmount,'Update');

END IF;

IF OLD.ExemptionInExcessAmount != NEW.ExemptionInExcessAmount THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'ExemptionInExcessAmount',NEW.RowID,OLD.ExemptionInExcessAmount,NEW.ExemptionInExcessAmount,'Update');

END IF;

IF OLD.TaxableIncomeFromAmount != NEW.TaxableIncomeFromAmount THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'TaxableIncomeFromAmount',NEW.RowID,OLD.TaxableIncomeFromAmount,NEW.TaxableIncomeFromAmount,'Update');

END IF;

IF OLD.TaxableIncomeToAmount != NEW.TaxableIncomeToAmount THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'TaxableIncomeToAmount',NEW.RowID,OLD.TaxableIncomeToAmount,NEW.TaxableIncomeToAmount,'Update');

END IF;



END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
