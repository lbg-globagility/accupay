/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTUPD_payphilhealth`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_payphilhealth` AFTER UPDATE ON `payphilhealth` FOR EACH ROW BEGIN

DECLARE viewID INT(11);

DECLARE OrgRowID INT(11);

SET OrgRowID = 2;

SELECT RowID FROM `view` WHERE ViewName='PhilHealth Contribution Table' AND OrganizationID=OrgRowID LIMIT 1 INTO viewID;

IF OLD.SalaryBracket != NEW.SalaryBracket THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'SalaryBracket',NEW.RowID,OLD.SalaryBracket,NEW.SalaryBracket,'Update');

END IF;

IF OLD.SalaryRangeTo != NEW.SalaryRangeTo THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'SalaryRangeTo',NEW.RowID,OLD.SalaryRangeTo,NEW.SalaryRangeTo,'Update');

END IF;

IF OLD.SalaryBase != NEW.SalaryBase THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'SalaryBase',NEW.RowID,OLD.SalaryBase,NEW.SalaryBase,'Update');

END IF;

IF OLD.TotalMonthlyPremium != NEW.TotalMonthlyPremium THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'TotalMonthlyPremium',NEW.RowID,OLD.TotalMonthlyPremium,NEW.TotalMonthlyPremium,'Update');

END IF;

IF OLD.EmployeeShare != NEW.EmployeeShare THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'EmployeeShare',NEW.RowID,OLD.EmployeeShare,NEW.EmployeeShare,'Update');

END IF;

IF OLD.EmployerShare != NEW.EmployerShare THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'EmployerShare',NEW.RowID,OLD.EmployerShare,NEW.EmployerShare,'Update');

END IF;


















END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
