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

-- Dumping structure for trigger goldwingspayrolldb.BEFINS_employeetimeentrydetails
DROP TRIGGER IF EXISTS `BEFINS_employeetimeentrydetails`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFINS_employeetimeentrydetails` BEFORE INSERT ON `employeetimeentrydetails` FOR EACH ROW BEGIN

DECLARE ultifirsttime TIME DEFAULT '00:00:00';

DECLARE ultilasttime TIME DEFAULT TIME(TIME_FORMAT(TIMESTAMPADD(SECOND,-1,TIMESTAMP(DATE_FORMAT(CURDATE(),CONCAT(@@date_format,' ',ultifirsttime)))), @@time_format));

DECLARE timestampI_date DATE;

DECLARE timestampO_date DATE;

DECLARE anyint INT(11);

SET timestampI_date = NEW.`Date`;

SET timestampO_date = NEW.`Date`;

IF NEW.TimeIn IS NOT NULL && NEW.TimeIn IS NOT NULL THEN

	
	IF (TIME_FORMAT(NEW.TimeIn,'%p')='AM' AND TIME_FORMAT(NEW.TimeOut,'%p')='AM')
			AND ultifirsttime <= NEW.TimeIn AND ultifirsttime <= NEW.TimeOut THEN
		SET timestampI_date = NEW.`Date`;
		SET timestampO_date = ADDDATE(NEW.`Date`, INTERVAL 1 DAY);

	
		
		
		
		
	ELSEIF (TIME_FORMAT(NEW.TimeIn,'%p')='PM' AND TIME_FORMAT(NEW.TimeOut,'%p')='AM')
			AND ultilasttime BETWEEN NEW.TimeIn AND ADDTIME(NEW.TimeOut,'24:00:00') THEN
		SET timestampI_date = NEW.`Date`;
		SET timestampO_date = ADDDATE(NEW.`Date`, INTERVAL 1 DAY);
		
	END IF;

END IF;








SET NEW.TimeStampIn = ADDTIME(TIMESTAMP(timestampI_date),NEW.TimeIn);
SET NEW.TimeStampOut = ADDTIME(TIMESTAMP(timestampO_date),NEW.TimeOut);

IF NEW.TimeStampIn IS NOT NULL THEN SELECT INSUPD_timeentrylog(NEW.OrganizationID,EmployeeID,NEW.TimeStampIn,1) FROM employee WHERE RowID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID INTO anyint; END IF;

IF NEW.TimeStampOut IS NOT NULL THEN SELECT INSUPD_timeentrylog(NEW.OrganizationID,EmployeeID,NEW.TimeStampOut,1) FROM employee WHERE RowID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID INTO anyint; END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
