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

-- Dumping structure for function goldwingspayrolldb.GRACE_PERIOD
DROP FUNCTION IF EXISTS `GRACE_PERIOD`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GRACE_PERIOD`(`Time_IN` TIME, `ShiftTimeFrom` TIME, `GracePeriodValue` INT) RETURNS time
    DETERMINISTIC
BEGIN

DECLARE returnval TIME;

DECLARE mindec_11 DECIMAL(10,2) DEFAULT 0;

DECLARE mindec_12 DECIMAL(10,2) DEFAULT 0;
IF GracePeriodValue IS NULL THEN SET GracePeriodValue = 0; END IF;

IF Time_IN BETWEEN ADDDATE(ShiftTimeFrom, INTERVAL -8 HOUR) AND ADDDATE(ShiftTimeFrom, INTERVAL GracePeriodValue MINUTE) THEN

	SET returnval = TIME_FORMAT(ShiftTimeFrom, '%H:%i:%s');

ELSE

	SET returnval = TIME_FORMAT(Time_IN, '%H:%i:%s');



END IF;


RETURN returnval;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
