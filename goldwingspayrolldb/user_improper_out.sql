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

-- Dumping structure for function goldwingspayrolldb.user_improper_out
DROP FUNCTION IF EXISTS `user_improper_out`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `user_improper_out`(`organizid` INT, `user_name` VARCHAR(50), `pass_word` VARCHAR(50)) RETURNS tinyint(4)
    DETERMINISTIC
BEGIN

DECLARE return_val TINYINT;

SELECT EXISTS(SELECT RowID FROM `user` WHERE UserID=user_name AND `Password`=pass_word AND InSession=1) INTO return_val;

RETURN return_val;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
