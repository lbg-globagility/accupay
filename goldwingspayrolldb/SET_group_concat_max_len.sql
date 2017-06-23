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

-- Dumping structure for procedure goldwingspayrolldb.SET_group_concat_max_len
DROP PROCEDURE IF EXISTS `SET_group_concat_max_len`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `SET_group_concat_max_len`()
    DETERMINISTIC
BEGIN

DECLARE _stmt VARCHAR(1024);
	
	SET @SQL := 'SET group_concat_max_len = 2048;';
	
	PREPARE _stmt FROM @SQL;
	
	EXECUTE _stmt;
	
	DEALLOCATE PREPARE _stmt;
	
	
	SET @SQL := 'SET GLOBAL group_concat_max_len = 2048;';
	
	PREPARE _stmt FROM @SQL;
	
	EXECUTE _stmt;
	
	DEALLOCATE PREPARE _stmt;
	
END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
