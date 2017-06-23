-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.11-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.0.0.4396
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for trigger goldwingspayrolldb.AFTUPD_position
DROP TRIGGER IF EXISTS `AFTUPD_position`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_position` AFTER UPDATE ON `position` FOR EACH ROW BEGIN

DECLARE viewID INT(11);

SELECT RowID FROM `view` WHERE ViewName='Position' AND OrganizationID=NEW.OrganizationID LIMIT 1 INTO viewID;

IF OLD.PositionName != NEW.PositionName THEN

	INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'PositionName',NEW.RowID,OLD.PositionName,NEW.PositionName,'Update');

END IF;

IF OLD.ParentPositionID != NEW.ParentPositionID THEN

	INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'ParentPositionID',NEW.RowID,OLD.ParentPositionID,NEW.ParentPositionID,'Update');

END IF;

IF OLD.DivisionId != NEW.DivisionId THEN

	INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'DivisionId',NEW.RowID,OLD.DivisionId,NEW.DivisionId,'Update');
	
	UPDATE employee e
	INNER JOIN `division` d ON d.RowID=NEW.DivisionId
	SET e.PayFrequencyID=d.PayFrequencyID
	,e.WorkDaysPerYear=d.WorkDaysPerYear
	,e.LateGracePeriod=d.GracePeriod
	,e.LastUpdBy=NEW.LastUpdBy
	WHERE e.OrganizationID=NEW.OrganizationID
	AND e.PositionID=NEW.RowID;

END IF;



END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
