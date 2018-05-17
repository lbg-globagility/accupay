/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFINS_shift`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFINS_shift` BEFORE INSERT ON `shift` FOR EACH ROW BEGIN

DECLARE default_break_hour INT(11) DEFAULT 1;

DECLARE divisor_to_half INT(11) DEFAULT 2;

DECLARE sec_per_hour INT(11) DEFAULT 3600;

DECLARE shift_timestamp_from
        , sh_timestamp_from DATETIME;

DECLARE shift_total_hours INT(11);

DECLARE is_even BOOL DEFAULT FALSE;

DECLARE sh_hrs, work_hrs, br_hrs DECIMAL(10, 2) DEFAULT 0;

DECLARE is_reach_tomorrow BOOL DEFAULT FALSE;

DECLARE custom_timeformat TEXT DEFAULT '%H:%i:00';

SET NEW.TimeFrom = TIME_FORMAT(NEW.TimeFrom, custom_timeformat);
SET NEW.TimeTo = TIME_FORMAT(NEW.TimeTo, custom_timeformat);
SET NEW.BreakTimeFrom = TIME_FORMAT(NEW.BreakTimeFrom, custom_timeformat);
SET NEW.BreakTimeTo = TIME_FORMAT(NEW.BreakTimeTo, custom_timeformat);

SET shift_timestamp_from = CONCAT_DATETIME(CURDATE(), NEW.TimeFrom);

SET shift_total_hours =
TIMESTAMPDIFF(SECOND
              , shift_timestamp_from
				  , CONCAT_DATETIME(ADDDATE(CURDATE(), INTERVAL IS_TIMERANGE_REACHTOMORROW(NEW.TimeFrom, NEW.TimeTo) DAY), NEW.TimeTo)) / sec_per_hour;

SET shift_total_hours = (shift_total_hours / divisor_to_half);

SET is_even = ( (shift_total_hours MOD 2) = 0 );

IF is_even THEN

	SET sh_timestamp_from = ADDDATE(shift_timestamp_from, INTERVAL shift_total_hours HOUR);
ELSE

	SET sh_timestamp_from = ADDDATE(shift_timestamp_from, INTERVAL (shift_total_hours - default_break_hour) HOUR);
END IF;

SET NEW.BreakTimeFrom = TIME(sh_timestamp_from);

SET NEW.BreakTimeTo = TIME( ADDDATE(sh_timestamp_from, INTERVAL default_break_hour HOUR) );

# ##############################################################################

SET is_reach_tomorrow = IS_TIMERANGE_REACHTOMORROW(NEW.TimeFrom, NEW.TimeTo);

SET sh_hrs = TIMESTAMPDIFF(SECOND
                           , CONCAT_DATETIME(CURDATE(), NEW.TimeFrom)
                           , ADDDATE(CONCAT_DATETIME(CURDATE(), NEW.TimeTo), INTERVAL is_reach_tomorrow DAY)) / sec_per_hour;

SET NEW.ShiftHours = IFNULL(sh_hrs, 0);


SET is_reach_tomorrow = IS_TIMERANGE_REACHTOMORROW(NEW.BreakTimeFrom, NEW.BreakTimeTo);

SET br_hrs = TIMESTAMPDIFF(SECOND
                           , CONCAT_DATETIME(CURDATE(), NEW.BreakTimeFrom)
                           , ADDDATE(CONCAT_DATETIME(CURDATE(), NEW.BreakTimeTo), INTERVAL is_reach_tomorrow DAY)) / sec_per_hour;

SET NEW.WorkHours = NEW.ShiftHours - IFNULL(br_hrs, 0);

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
