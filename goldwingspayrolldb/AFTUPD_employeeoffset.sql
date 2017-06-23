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

-- Dumping structure for trigger goldwingspayrolldb.AFTUPD_employeeoffset
DROP TRIGGER IF EXISTS `AFTUPD_employeeoffset`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_employeeoffset` AFTER UPDATE ON `employeeoffset` FOR EACH ROW BEGIN

DECLARE tothoursoffset DECIMAL(11,6);



IF OLD.`Status` != 'Approved' AND NEW.`Status` = 'Approved' THEN

	SET tothoursoffset = COMPUTE_TimeDifference(NEW.StartTime,NEW.EndTime);
	
	UPDATE employee e
	SET e.OffsetBalance = IFNULL(e.OffsetBalance,0) + tothoursoffset
	,e.LastUpdBy=NEW.CreatedBy
	WHERE e.RowID = NEW.EmployeeID;
	
END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
