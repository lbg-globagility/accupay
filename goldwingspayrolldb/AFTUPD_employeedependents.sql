/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTUPD_employeedependents`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO';
DELIMITER //
CREATE TRIGGER `AFTUPD_employeedependents` AFTER UPDATE ON `employeedependents` FOR EACH ROW BEGIN

DECLARE viewID INT(11);

SELECT RowID FROM `view` WHERE ViewName='Employee Dependents' AND OrganizationID=NEW.OrganizationID LIMIT 1 INTO viewID;


IF OLD.Salutation != NEW.Salutation THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'Salutation',NEW.RowID,OLD.Salutation,NEW.Salutation,'Update');

END IF;

IF OLD.FirstName != NEW.FirstName THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'FirstName',NEW.RowID,OLD.FirstName,NEW.FirstName,'Update');

END IF;

IF OLD.MiddleName != NEW.MiddleName THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'MiddleName',NEW.RowID,OLD.MiddleName,NEW.MiddleName,'Update');

END IF;

IF OLD.LastName != NEW.LastName THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'LastName',NEW.RowID,OLD.LastName,NEW.LastName,'Update');

END IF;

IF OLD.Surname != NEW.Surname THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'Surname',NEW.RowID,OLD.Surname,NEW.Surname,'Update');

END IF;

IF OLD.TINNo != NEW.TINNo THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'TINNo',NEW.RowID,OLD.TINNo,NEW.TINNo,'Update');

END IF;

IF OLD.SSSNo != NEW.SSSNo THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'SSSNo',NEW.RowID,OLD.SSSNo,NEW.SSSNo,'Update');

END IF;

IF OLD.HDMFNo != NEW.HDMFNo THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'HDMFNo',NEW.RowID,OLD.HDMFNo,NEW.HDMFNo,'Update');

END IF;

IF OLD.PhilHealthNo != NEW.PhilHealthNo THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'PhilHealthNo',NEW.RowID,OLD.PhilHealthNo,NEW.PhilHealthNo,'Update');

END IF;

IF OLD.EmailAddress != NEW.EmailAddress THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'EmailAddress',NEW.RowID,OLD.EmailAddress,NEW.EmailAddress,'Update');

END IF;

IF OLD.WorkPhone != NEW.WorkPhone THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'WorkPhone',NEW.RowID,OLD.WorkPhone,NEW.WorkPhone,'Update');

END IF;

IF OLD.HomePhone != NEW.HomePhone THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'HomePhone',NEW.RowID,OLD.HomePhone,NEW.HomePhone,'Update');

END IF;

IF OLD.MobilePhone != NEW.MobilePhone THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'MobilePhone',NEW.RowID,OLD.MobilePhone,NEW.MobilePhone,'Update');

END IF;

IF OLD.HomeAddress != NEW.HomeAddress THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'HomeAddress',NEW.RowID,OLD.HomeAddress,NEW.HomeAddress,'Update');

END IF;

IF OLD.Nickname != NEW.Nickname THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'Nickname',NEW.RowID,OLD.Nickname,NEW.Nickname,'Update');

END IF;

IF OLD.JobTitle != NEW.JobTitle THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'JobTitle',NEW.RowID,OLD.JobTitle,NEW.JobTitle,'Update');

END IF;

IF OLD.Gender != NEW.Gender THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'Gender',NEW.RowID,OLD.Gender,NEW.Gender,'Update');

END IF;

IF OLD.RelationToEmployee != NEW.RelationToEmployee THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'RelationToEmployee',NEW.RowID,OLD.RelationToEmployee,NEW.RelationToEmployee,'Update');

END IF;

IF OLD.ActiveFlag != NEW.ActiveFlag THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'ActiveFlag',NEW.RowID,OLD.ActiveFlag,NEW.ActiveFlag,'Update');

END IF;

IF OLD.Birthdate != NEW.Birthdate THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'Birthdate',NEW.RowID,OLD.Birthdate,NEW.Birthdate,'Update');

END IF;





END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
