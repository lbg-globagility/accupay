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

-- Dumping structure for trigger BEFUPD_employeetimeentrydetails
DROP TRIGGER IF EXISTS `BEFUPD_employeetimeentrydetails`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFUPD_employeetimeentrydetails` BEFORE UPDATE ON `employeetimeentrydetails` FOR EACH ROW BEGIN

DECLARE anyint INT(11);

DECLARE old_timein_sec INT(11);

DECLARE old_timeout_sec INT(11);

SET old_timein_sec = IFNULL(SECOND(OLD.TimeIn),0);
SET old_timeout_sec = IFNULL(SECOND(OLD.TimeOut),0);

/*
DECLARE custom_datetime_format VARCHAR(50) DEFAULT '%Y-%m-%d %H:%i:';


DECLARE old_timestampI_sec INT(11);

DECLARE old_timestampO_sec INT(11);

DECLARE ultifirsttime TIME DEFAULT '00:00:00';

DECLARE ultilasttime TIME DEFAULT SUBDATE(TIMESTAMP(CURDATE()), INTERVAL 1 SECOND); # TIME(TIME_FORMAT(TIMESTAMPADD(SECOND,-1,TIMESTAMP(DATE_FORMAT(CURDATE(),CONCAT(@@date_format,' ',ultifirsttime)))), @@time_format));

DECLARE timestampI_date DATE;

DECLARE timestampO_date DATE;


SET timestampI_date = NEW.`Date`;

SET timestampO_date = NEW.`Date`;


SET old_timestampI_sec = IF(OLD.TimeStampIn IS NULL, old_timein_sec, IFNULL(SECOND(OLD.TimeStampIn),0));
SET old_timestampO_sec = IF(OLD.TimeStampOut IS NULL, old_timeout_sec, IFNULL(SECOND(OLD.TimeStampOut),0));


IF NEW.TimeIn IS NOT NULL && NEW.TimeIn IS NOT NULL AND IS_TIMERANGE_REACHTOMORROW(NEW.TimeIn, NEW.TimeOut) = TRUE THEN

	
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


SET NEW.TimeStampIn = TIMESTAMP(DATE_FORMAT(NEW.TimeStampIn,CONCAT(timestampI_date,' %H:%i:%s')));

SET NEW.TimeStampOut = TIMESTAMP(DATE_FORMAT(NEW.TimeStampOut,CONCAT(timestampO_date,' %H:%i:%s')));

SET NEW.TimeIn = MAKETIME(HOUR(NEW.TimeIn),MINUTE(NEW.TimeIn),old_timein_sec);
SET NEW.TimeOut = MAKETIME(HOUR(NEW.TimeOut),MINUTE(NEW.TimeOut),old_timeout_sec);

SET NEW.TimeStampIn = ADDTIME(TIMESTAMP(timestampI_date),NEW.TimeIn);
SET NEW.TimeStampOut = ADDTIME(TIMESTAMP(timestampO_date),NEW.TimeOut);

SET NEW.TimeStampIn = DATE_FORMAT(NEW.TimeStampIn,CONCAT(custom_datetime_format,LPAD(old_timestampI_sec, 2, 0)));
SET NEW.TimeStampOut = DATE_FORMAT(NEW.TimeStampOut,CONCAT(custom_datetime_format,LPAD(old_timestampO_sec, 2, 0)));
*/
SET @time_in_format = CONCAT('%H:%i:', LPAD(old_timein_sec, 2, 0));
SET @time_out_format = CONCAT('%H:%i:', LPAD(old_timeout_sec, 2, 0));

SET @is_start_time_reachedtomorrow = (SUBDATE(TIMESTAMP(TIME(0)), INTERVAL 1 SECOND)
                                      = SUBDATE(TIMESTAMP(MAKETIME(HOUR(NEW.TimeIn),0,0)), INTERVAL 1 SECOND));

SET @is_start_time_reachedtomorrow = IFNULL(@is_start_time_reachedtomorrow, FALSE);

SET NEW.TimeStampIn = CONCAT_DATETIME(ADDDATE(NEW.`Date`, INTERVAL @is_start_time_reachedtomorrow DAY), TIME_FORMAT(NEW.TimeIn, @time_in_format));
SET NEW.TimeStampOut = CONCAT_DATETIME(ADDDATE(NEW.`Date`, INTERVAL IS_TIMERANGE_REACHTOMORROW(NEW.TimeIn, NEW.TimeOut) DAY), TIME_FORMAT(NEW.TimeOut, @time_out_format));

IF NEW.TimeStampIn IS NOT NULL THEN SELECT INSUPD_timeentrylog(NEW.OrganizationID,EmployeeID,NEW.TimeStampIn,1) FROM employee WHERE RowID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID INTO anyint; END IF;

IF NEW.TimeStampOut IS NOT NULL THEN SELECT INSUPD_timeentrylog(NEW.OrganizationID,EmployeeID,NEW.TimeStampOut,1) FROM employee WHERE RowID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID INTO anyint; END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
