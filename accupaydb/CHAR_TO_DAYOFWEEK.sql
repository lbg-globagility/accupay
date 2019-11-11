/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `CHAR_TO_DAYOFWEEK`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `CHAR_TO_DAYOFWEEK`(`param_char` CHAR(50)) RETURNS text CHARSET latin1
    DETERMINISTIC
BEGIN

DECLARE returnvalue TEXT DEFAULT '';

IF param_char = 1 THEN
    SET returnvalue = 'Sunday';
ELSEIF param_char = 2 THEN
    SET returnvalue = 'Monday';
ELSEIF param_char = 3 THEN
    SET returnvalue = 'Tuesday';
ELSEIF param_char = 4 THEN
    SET returnvalue = 'Wednesday';
ELSEIF param_char = 5 THEN
    SET returnvalue = 'Thursday';
ELSEIF param_char = 6 THEN
    SET returnvalue = 'Friday';
ELSEIF param_char = 7 THEN
    SET returnvalue = 'Saturday';
ELSEIF LENGTH(param_char) = 0
       OR param_char = 0 THEN
    SET returnvalue = '';
END IF;

RETURN returnvalue;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
