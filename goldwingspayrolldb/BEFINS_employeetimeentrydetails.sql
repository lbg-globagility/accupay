/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFINS_employeetimeentrydetails`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFINS_employeetimeentrydetails` BEFORE INSERT ON `employeetimeentrydetails` FOR EACH ROW BEGIN

DECLARE anyint INT(11);

/*
DECLARE ultifirsttime TIME DEFAULT '00:00:00';

DECLARE ultilasttime TIME DEFAULT SUBDATE(TIMESTAMP(CURDATE()), INTERVAL 1 SECOND); # TIME(TIME_FORMAT(TIMESTAMPADD(SECOND,-1,TIMESTAMP(DATE_FORMAT(CURDATE(),CONCAT(@@date_format,' ',ultifirsttime)))), @@time_format));

DECLARE timestampI_date DATE;

DECLARE timestampO_date DATE;

DECLARE custom_timeformat VARCHAR(50) DEFAULT '%H:%i:00';

SET timestampI_date = NEW.`Date`;

SET timestampO_date = NEW.`Date`;

SET NEW.TimeIn = TIME_FORMAT(NEW.TimeIn, custom_timeformat); # %H:%i:%s

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








SET NEW.TimeStampIn = ADDTIME(TIMESTAMP(timestampI_date),NEW.TimeIn);
SET NEW.TimeStampOut = ADDTIME(TIMESTAMP(timestampO_date),NEW.TimeOut);
*/
SET @is_start_time_reachedtomorrow = (SUBDATE(TIMESTAMP(TIME(0)), INTERVAL 1 SECOND)
                                      = SUBDATE(TIMESTAMP(MAKETIME(HOUR(NEW.TimeIn),0,0)), INTERVAL 1 SECOND));

SET @is_start_time_reachedtomorrow = IFNULL(@is_start_time_reachedtomorrow, FALSE);

SET NEW.TimeStampIn = CONCAT_DATETIME(ADDDATE(NEW.`Date`, INTERVAL @is_start_time_reachedtomorrow DAY), NEW.TimeIn);
SET NEW.TimeStampOut = CONCAT_DATETIME(ADDDATE(NEW.`Date`, INTERVAL IS_TIMERANGE_REACHTOMORROW(NEW.TimeIn, NEW.TimeOut) DAY), NEW.TimeOut);

IF NEW.TimeStampIn IS NOT NULL THEN SELECT INSUPD_timeentrylog(NEW.OrganizationID,EmployeeID,NEW.TimeStampIn,1) FROM employee WHERE RowID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID INTO anyint; END IF;

IF NEW.TimeStampOut IS NOT NULL THEN SELECT INSUPD_timeentrylog(NEW.OrganizationID,EmployeeID,NEW.TimeStampOut,1) FROM employee WHERE RowID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID INTO anyint; END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
