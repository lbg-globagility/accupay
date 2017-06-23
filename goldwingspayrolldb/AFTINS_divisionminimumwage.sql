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

-- Dumping structure for trigger goldwingspayrolldb.AFTINS_divisionminimumwage
DROP TRIGGER IF EXISTS `AFTINS_divisionminimumwage`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_divisionminimumwage` AFTER INSERT ON `divisionminimumwage` FOR EACH ROW BEGIN

SELECT COUNT(dmw.RowID) FROM divisionminimumwage dmw WHERE dmw.DivisionID=NEW.DivisionID
INTO @rec_count_minwage;

IF @rec_count_minwage <= 1 THEN

	UPDATE division d
	SET d.MinimumWageAmount=NEW.Amount
	,d.LastUpd=IFNULL(ADDTIME(d.LastUpd, SEC_TO_TIME(1)),CURRENT_TIMESTAMP())
	,d.LastUpdBy=IFNULL(d.LastUpdBy,d.CreatedBy)
	WHERE d.RowID=NEW.DivisionID;

END IF;
	
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
