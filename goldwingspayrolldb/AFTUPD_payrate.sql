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

-- Dumping structure for trigger AFTUPD_payrate
DROP TRIGGER IF EXISTS `AFTUPD_payrate`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_payrate` AFTER UPDATE ON `payrate` FOR EACH ROW BEGIN

DECLARE viewRowID INT(11);

DECLARE auditrowid INT(11);


SELECT RowID FROM `view` WHERE OrganizationID=NEW.OrganizationID AND ViewName='Pay rate' INTO viewRowID;



SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewRowID,'PayType',NEW.RowID,OLD.PayType,NEW.PayType,'Update') INTO auditrowid;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewRowID,'DayBefore',NEW.RowID,OLD.DayBefore,NEW.DayBefore,'Update') INTO auditrowid;



END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
