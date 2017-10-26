/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `IS_TIMERANGE_REACHTOMORROW`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `IS_TIMERANGE_REACHTOMORROW`(`time_range_from` TIME, `time_range_to` TIME) RETURNS tinyint(1)
    DETERMINISTIC
BEGIN

DECLARE return_val BOOL DEFAULT FALSE;

IF time_range_from IS NOT NULL
	AND time_range_to IS NOT NULL THEN
	
	SET return_val = (TIME_FORMAT(time_range_from, '%p') = 'PM'
							AND TIME_FORMAT(time_range_to, '%p') = 'AM')
							
						  # OR (TIME_FORMAT(time_range_from, '%p') = 'AM' AND TIME_FORMAT(time_range_to, '%p') = 'AM'
						  OR (TIME_FORMAT(time_range_from, '%p') = TIME_FORMAT(time_range_to, '%p')
						      AND HOUR(time_range_from) > HOUR(time_range_to))
								
						  OR (TIME_FORMAT(time_range_from, '%p') = TIME_FORMAT(time_range_to, '%p')
						      AND SUBDATE(TIMESTAMP(TIME(0)), INTERVAL 1 SECOND) = SUBDATE(TIMESTAMP(MAKETIME(HOUR(time_range_from),0,0)), INTERVAL 1 SECOND));

END IF;
	
RETURN return_val;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
