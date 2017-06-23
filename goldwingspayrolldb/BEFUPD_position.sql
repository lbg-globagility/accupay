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

-- Dumping structure for trigger goldwingspayrolldb.BEFUPD_position
DROP TRIGGER IF EXISTS `BEFUPD_position`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFUPD_position` BEFORE UPDATE ON `position` FOR EACH ROW BEGIN

SELECT EXISTS(SELECT RowID FROM `user` WHERE PositionID=NEW.RowID LIMIT 1)
INTO @is_sysuser;

IF NEW.DivisionId IS NULL AND @is_sysuser = 0 THEN
	SET NEW.DivisionId = (SELECT RowID FROM division d WHERE d.OrganizationID=NEW.OrganizationID AND d.ParentDivisionID IS NOT NULL LIMIT 1);
END IF;

SELECT EXISTS(SELECT d.RowID FROM division d WHERE d.OrganizationID=NEW.OrganizationID AND d.RowID=NEW.DivisionId AND d.ParentDivisionID IS NULL AND @is_sysuser = 0 LIMIT 1)
INTO @invalid_divisionid;

IF @invalid_divisionid = 1 THEN
	SET NEW.DivisionId = (SELECT RowID FROM division d WHERE d.OrganizationID=NEW.OrganizationID AND d.ParentDivisionID IS NOT NULL LIMIT 1);
END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
