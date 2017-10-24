-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.12-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for trigger AFTINS_employeepreviousemployer
DROP TRIGGER IF EXISTS `AFTINS_employeepreviousemployer`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_employeepreviousemployer` AFTER INSERT ON `employeepreviousemployer` FOR EACH ROW BEGIN

DECLARE viewID INT(11);

SELECT RowID FROM `view` WHERE ViewName='Employee Previous Employer' AND OrganizationID=NEW.OrganizationID LIMIT 1 INTO viewID;

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EmployeeID',NEW.RowID,'',NEW.EmployeeID,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'MainPhone',NEW.RowID,'',NEW.MainPhone,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'FaxNumber',NEW.RowID,'',NEW.FaxNumber,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'JobTitle',NEW.RowID,'',NEW.JobTitle,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'ExperienceFromTo',NEW.RowID,'',NEW.ExperienceFromTo,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'BusinessAddress',NEW.RowID,'',NEW.BusinessAddress,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'ContactName',NEW.RowID,'',NEW.ContactName,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EmailAddress',NEW.RowID,'',NEW.EmailAddress,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'AltEmailAddress',NEW.RowID,'',NEW.AltEmailAddress,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'AltPhone',NEW.RowID,'',NEW.AltPhone,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'URL',NEW.RowID,'',NEW.URL,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TINNo',NEW.RowID,'',NEW.TINNo,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'JobFunction',NEW.RowID,'',NEW.JobFunction,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'OrganizationType',NEW.RowID,'',NEW.OrganizationType,'Insert');



END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
