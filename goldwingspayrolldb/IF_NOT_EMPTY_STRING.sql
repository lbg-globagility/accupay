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

-- Dumping structure for function IF_NOT_EMPTY_STRING
DROP FUNCTION IF EXISTS `IF_NOT_EMPTY_STRING`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `IF_NOT_EMPTY_STRING`(`Param1` TEXT, `Param2` TEXT) RETURNS text CHARSET latin1
    DETERMINISTIC
BEGIN

DECLARE returnvalue TEXT;

IF IFNULL(Param1,'') != '' THEN

    SET returnvalue = IFNULL(Param2,'');

ELSE

    SET returnvalue = '';

END IF;

RETURN returnvalue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
