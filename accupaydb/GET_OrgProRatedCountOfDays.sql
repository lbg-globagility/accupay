/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `GET_OrgProRatedCountOfDays`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GET_OrgProRatedCountOfDays`(`Org_WorkDays` INT, `Effective_DateFrom` DATE, `Effective_DateTo` DATE, `Frequency` TEXT) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11) DEFAULT 0;

DECLARE betweenfrom DATE;

DECLARE betweento DATE;

    IF Effective_DateFrom < MAKEDATE(YEAR(CURDATE()), 1) THEN

        SET Effective_DateFrom = MAKEDATE(YEAR(CURDATE()), 1);

    END IF;

    IF Effective_DateTo > LAST_DAY(CONCAT(YEAR(CURDATE()),'-12-01')) THEN

        SET Effective_DateTo = LAST_DAY(CONCAT(YEAR(CURDATE()),'-12-01'));

    END IF;

    IF Frequency = 'Daily' THEN

        IF Org_WorkDays BETWEEN 310 AND 320 THEN

            SELECT COUNT(d.DateValue)
            FROM dates d
            WHERE DAYOFWEEK(d.DateValue)!=7
            AND d.DateValue BETWEEN Effective_DateFrom AND Effective_DateTo
            INTO returnvalue;

        ELSE

            SELECT COUNT(d.DateValue)
            FROM dates d
            WHERE DAYOFWEEK(d.DateValue) NOT IN (6,7)
            AND d.DateValue BETWEEN Effective_DateFrom AND Effective_DateTo
            INTO returnvalue;

        END IF;

    ELSEIF Frequency = 'Monthly' THEN

        SELECT COUNT(DISTINCT(MONTH(d.DateValue)))
        FROM dates d
        WHERE d.DateValue BETWEEN Effective_DateFrom AND Effective_DateTo
        INTO returnvalue;

    END IF;

RETURN returnvalue;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
