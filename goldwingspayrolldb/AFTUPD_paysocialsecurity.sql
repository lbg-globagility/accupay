/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTUPD_paysocialsecurity`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_paysocialsecurity` AFTER UPDATE ON `paysocialsecurity` FOR EACH ROW BEGIN

DECLARE viewID INT(11);

DECLARE OrgRowID INT(11);

SET OrgRowID = 2;

SELECT RowID FROM `view` WHERE ViewName='SSS Contribution Table' AND OrganizationID=OrgRowID LIMIT 1 INTO viewID;

IF OLD.RangeFromAmount != NEW.RangeFromAmount THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'RangeFromAmount',NEW.RowID,OLD.RangeFromAmount,NEW.RangeFromAmount,'Update');

END IF;

IF OLD.RangeToAmount != NEW.RangeToAmount THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'RangeToAmount',NEW.RowID,OLD.RangeToAmount,NEW.RangeToAmount,'Update');

END IF;

IF OLD.MonthlySalaryCredit != NEW.MonthlySalaryCredit THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'MonthlySalaryCredit',NEW.RowID,OLD.MonthlySalaryCredit,NEW.MonthlySalaryCredit,'Update');

END IF;

IF OLD.EmployeeContributionAmount != NEW.EmployeeContributionAmount THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'EmployeeContributionAmount',NEW.RowID,OLD.EmployeeContributionAmount,NEW.EmployeeContributionAmount,'Update');

END IF;

IF OLD.EmployerContributionAmount != NEW.EmployerContributionAmount THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'EmployerContributionAmount',NEW.RowID,OLD.EmployerContributionAmount,NEW.EmployerContributionAmount,'Update');

END IF;

IF OLD.EmployeeECAmount != NEW.EmployeeECAmount THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,OrgRowID,viewID,'EmployeeECAmount',NEW.RowID,OLD.EmployeeECAmount,NEW.EmployeeECAmount,'Update');

END IF;


















END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
