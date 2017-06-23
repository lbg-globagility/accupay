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

-- Dumping structure for trigger goldwingspayrolldb.AFTINS_audittrail
DROP TRIGGER IF EXISTS `AFTINS_audittrail`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_audittrail` AFTER INSERT ON `audittrail` FOR EACH ROW BEGIN

DECLARE view_name TEXT;

DECLARE max_date DATE;

DECLARE myprevious_max_date VARCHAR(50);

SELECT ViewName FROM `view` WHERE RowID=NEW.ViewID INTO view_name;





IF view_name = 'Pay rate' THEN

	IF NEW.OldValue != 'Regular Holiday' AND NEW.NewValue = 'Regular Holiday' THEN
	
	
		
		
		SET view_name = 'Pay rate';
		
		
		
	END IF;

END IF;


	
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
