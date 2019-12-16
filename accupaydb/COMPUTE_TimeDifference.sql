/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `COMPUTE_TimeDifference`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `COMPUTE_TimeDifference`(`TimeOne` TIME, `TimeTwo` TIME) RETURNS decimal(11,6)
    DETERMINISTIC
BEGIN

DECLARE returnvalue DECIMAL(11,6);

IF TimeOne IS NULL
    OR TimeTwo IS NULL THEN

    SET returnvalue = 0;

ELSE

    IF HOUR(TimeOne) >= 24 THEN
        SET TimeOne = TIME_FORMAT(TimeOne,'00:%i:%s');
    END IF;

    IF HOUR(TimeTwo) >= 24 THEN
        SET TimeTwo = TIME_FORMAT(TimeTwo,'00:%i:%s');
    END IF;

    IF DATE_FORMAT(TimeOne,'%p') = 'PM'
        AND DATE_FORMAT(TimeTwo,'%p') = 'AM' THEN

        SET returnvalue = ((TIME_TO_SEC(TIMEDIFF(ADDTIME(TimeTwo,'24:00'), TimeOne)) / 60) / 60);

    ELSEIF DATE_FORMAT(TimeTwo,'%p') = 'PM'
             AND DATE_FORMAT(TimeOne,'%p') = 'AM' THEN

        SET returnvalue = ((TIME_TO_SEC(TIMEDIFF(TimeTwo, TimeOne)) / 60) / 60);

    ELSE

        IF TimeTwo > TimeOne THEN

            SET returnvalue = ((TIME_TO_SEC(TIMEDIFF(TimeTwo, TimeOne)) / 60) / 60);

        ELSE

            SET returnvalue = ((TIME_TO_SEC(TIMEDIFF(TimeOne, TimeTwo)) / 60) / 60);

        END IF;

    END IF;

END IF;


RETURN returnvalue;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
