/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `COUNT_pay_period`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `COUNT_pay_period`(`paramDateFrom` DATE, `paramDateTo` DATE) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnval INT(11);

DECLARE indx INT(11) DEFAULT 1;

DECLARE cnt INT(11) DEFAULT 0;

DECLARE dateloop DATE;

SET dateloop = paramDateFrom;

    date_loop : LOOP

        IF dateloop < paramDateTo THEN

            IF DATE_ADD(paramDateFrom,INTERVAL indx MONTH) > LAST_DAY(dateloop) THEN

                IF DAY(dateloop) < 16 THEN

                    SET cnt = cnt + 2;

                END IF;

            END IF;

            SELECT DATE_ADD(paramDateFrom,INTERVAL indx MONTH) INTO dateloop;

            SET indx = indx + 1;

        ELSE

            LEAVE date_loop;

        END IF;


    END LOOP;

    IF DAY(paramDateTo) <= 15 THEN

        SET cnt = cnt - 1;

    ELSEIF DATE_ADD(CONCAT(YEAR(NOW()),'-01-01'), INTERVAL 0 DAY) = paramDateFrom
        AND DATE_ADD(CONCAT(YEAR(NOW()),'-12-31'), INTERVAL 0 DAY) = paramDateTo THEN

        SET cnt = cnt + 2;

    END IF;

    SET returnval = cnt;

RETURN returnval;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
