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

-- Dumping structure for table hyundaipayrolldb.systemowner
DROP TABLE IF EXISTS `systemowner`;
CREATE TABLE IF NOT EXISTS `systemowner` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastUpdBy` int(10) DEFAULT NULL,
  `Name` varchar(50) DEFAULT '',
  `IsCurrentOwner` enum('0','1') DEFAULT '0',
  PRIMARY KEY (`RowID`),
  KEY `FK_BaseTables_user` (`CreatedBy`),
  KEY `FK_BaseTables_user_2` (`LastUpdBy`),
  CONSTRAINT `systemowner_ibfk_3` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `systemowner_ibfk_4` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

-- Dumping data for table hyundaipayrolldb.systemowner: ~2 rows (approximately)
DELETE FROM `systemowner`;
/*!40000 ALTER TABLE `systemowner` DISABLE KEYS */;
INSERT INTO `systemowner` (`RowID`, `Created`, `CreatedBy`, `LastUpd`, `LastUpdBy`, `Name`, `IsCurrentOwner`) VALUES
	(1, '2017-10-05 09:59:15', NULL, '2017-10-05 10:01:59', NULL, 'Goldwings', '0'),
	(2, '2017-10-05 09:59:15', NULL, '2017-10-05 10:12:56', NULL, 'Hyundai', '1');
/*!40000 ALTER TABLE `systemowner` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
