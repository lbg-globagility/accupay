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

-- Dumping structure for function PAYMENT_SCHED_TO_CHAR
DROP FUNCTION IF EXISTS `PAYMENT_SCHED_TO_CHAR`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `PAYMENT_SCHED_TO_CHAR`(`StringSchedule` VARCHAR(50)) RETURNS char(1) CHARSET utf8
    DETERMINISTIC
BEGIN

DECLARE returnvalue CHAR(1);

IF StringSchedule = 'End of the month' THEN
    SET returnvalue = '0';
ELSEIF StringSchedule = 'First half' THEN
    SET returnvalue = '1';
ELSEIF StringSchedule = 'Per pay period' THEN
    SET returnvalue = '2';
END IF;

RETURN returnvalue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
