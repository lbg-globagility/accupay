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

-- Dumping structure for function goldwingspayrolldb.string_chopchop
DROP FUNCTION IF EXISTS `string_chopchop`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `string_chopchop`(`EmpPrimKey` INT(11), `_str` VARCHAR(50)) RETURNS char(1) CHARSET latin1
    DETERMINISTIC
BEGIN

DECLARE i INT(11) DEFAULT 0;

DECLARE indx INT(11) DEFAULT 1;

SET i = @@ft_min_word_len;
	
WHILE indx <= (@@ft_min_word_len) DO
	
	SET @curr_str = SUBSTRING(_str,indx,i);
	
	IF LENGTH(@curr_str) > 0 AND IFNULL(EmpPrimKey,0) > 0 THEN
		INSERT INTO employeesearchstring (EmpPrimaKey,searchstring) VALUES (EmpPrimKey,@curr_str) ON DUPLICATE KEY UPDATE searchstring=@curr_str;
	
		SET indx = indx + 1;
		
	ELSE
		
		SET indx = @@ft_min_word_len + 1;
		
	END IF;
		
END WHILE;

RETURN '0';

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
