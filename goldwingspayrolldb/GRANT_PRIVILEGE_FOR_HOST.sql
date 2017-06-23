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

-- Dumping structure for procedure goldwingspayrolldb.GRANT_PRIVILEGE_FOR_HOST
DROP PROCEDURE IF EXISTS `GRANT_PRIVILEGE_FOR_HOST`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `GRANT_PRIVILEGE_FOR_HOST`()
    DETERMINISTIC
    COMMENT 'CREATE USER ''root''@''localhost'' IDENTIFIED BY ''password''; GRANT ALL PRIVILEGES ON *.* TO ''root''@''localhost''; FLUSH PRIVILEGES;'
BEGIN

SELECT 'The Grant privilege SQL command is in the comment section of this \'Procedure\'';



END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
