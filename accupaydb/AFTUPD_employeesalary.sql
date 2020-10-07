/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTUPD_employeesalary`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_employeesalary` AFTER UPDATE ON `employeesalary` FOR EACH ROW BEGIN

DECLARE viewID INT(11);

SELECT RowID FROM `view` WHERE ViewName='Employee Salary' AND OrganizationID=NEW.OrganizationID LIMIT 1 INTO viewID;

IF OLD.FilingStatusID != NEW.FilingStatusID THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'FilingStatusID',NEW.RowID,OLD.FilingStatusID,NEW.FilingStatusID,'Update');

END IF;

IF OLD.HDMFAmount != NEW.HDMFAmount THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'HDMFAmount',NEW.RowID,OLD.HDMFAmount,NEW.HDMFAmount,'Update');

END IF;

IF OLD.BasicPay != NEW.BasicPay THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'BasicPay',NEW.RowID,OLD.BasicPay,NEW.BasicPay,'Update');

END IF;

IF OLD.Salary != NEW.Salary THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'Salary',NEW.RowID,OLD.Salary,NEW.Salary,'Update');

END IF;

IF OLD.BasicDailyPay != NEW.BasicDailyPay THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'BasicDailyPay',NEW.RowID,OLD.BasicDailyPay,NEW.BasicDailyPay,'Update');

END IF;

IF OLD.BasicHourlyPay != NEW.BasicHourlyPay THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'BasicHourlyPay',NEW.RowID,OLD.BasicHourlyPay,NEW.BasicHourlyPay,'Update');

END IF;

IF OLD.NoofDependents != NEW.NoofDependents THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'NoofDependents',NEW.RowID,OLD.NoofDependents,NEW.NoofDependents,'Update');

END IF;

IF OLD.MaritalStatus != NEW.MaritalStatus THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'MaritalStatus',NEW.RowID,OLD.MaritalStatus,NEW.MaritalStatus,'Update');

END IF;

IF OLD.PositionID != NEW.PositionID THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'PositionID',NEW.RowID,OLD.PositionID,NEW.PositionID,'Update');

END IF;

IF OLD.EffectiveDateFrom != NEW.EffectiveDateFrom THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'EffectiveDateFrom',NEW.RowID,OLD.EffectiveDateFrom,NEW.EffectiveDateFrom,'Update');

END IF;


END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
