-- --------------------------------------------------------
-- Host:                         localhost
-- Server version:               5.5.5-10.0.12-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win64
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping database structure for accupaydb_4linq
DROP DATABASE IF EXISTS `accupaydb_4linq`;
CREATE DATABASE IF NOT EXISTS `accupaydb_4linq` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `accupaydb_4linq`;


-- Dumping structure for table accupaydb_4linq.machinelog
DROP TABLE IF EXISTS `machinelog`;
CREATE TABLE IF NOT EXISTS `machinelog` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `MachineNumber` int(11) NOT NULL,
  `IndRegID` varchar(50) NOT NULL,
  `DateTimeRecord` text NOT NULL,
  `InOutMode` int(11) NOT NULL DEFAULT '0',
  `DateOnlyRecord` datetime NOT NULL,
  `TimeOnlyRecord` datetime NOT NULL,
  `IsLastInserted` tinyint(4) DEFAULT '0',
  `OrganizationID` int(10) DEFAULT NULL,
  `CreatedBy` int(10) DEFAULT NULL,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`RowID`),
  KEY `CreatedBy` (`CreatedBy`),
  KEY `LastUpdBy` (`LastUpdBy`),
  KEY `OrganizationID` (`OrganizationID`),
  CONSTRAINT `machinelog_ibfk_1` FOREIGN KEY (`CreatedBy`) REFERENCES `account` (`RowID`),
  CONSTRAINT `machinelog_ibfk_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `account` (`RowID`),
  CONSTRAINT `machinelog_ibfk_3` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=latin1;

-- Dumping data for table accupaydb_4linq.machinelog: ~13 rows (approximately)
DELETE FROM `machinelog`;
/*!40000 ALTER TABLE `machinelog` DISABLE KEYS */;
INSERT INTO `machinelog` (`RowID`, `MachineNumber`, `IndRegID`, `DateTimeRecord`, `InOutMode`, `DateOnlyRecord`, `TimeOnlyRecord`, `IsLastInserted`, `OrganizationID`, `CreatedBy`, `Created`, `LastUpdBy`, `LastUpd`) VALUES
	(14, 2, '1', '3/28/2019 4:41:32 PM', 0, '2019-03-28 00:00:00', '2019-04-04 16:41:32', 0, NULL, NULL, '2019-04-04 10:54:10', NULL, '2019-04-04 14:51:27'),
	(15, 2, '1', '3/28/2019 4:44:56 PM', 1, '2019-03-28 00:00:00', '2019-04-04 16:44:56', 0, NULL, NULL, '2019-04-04 10:54:10', NULL, '2019-04-04 14:51:27'),
	(16, 2, '1', '3/28/2019 5:25:24 PM', 1, '2019-03-28 00:00:00', '2019-04-04 17:25:24', 0, NULL, NULL, '2019-04-04 10:54:10', NULL, '2019-04-04 14:51:27'),
	(17, 2, '1', '3/28/2019 5:25:42 PM', 0, '2019-03-28 00:00:00', '2019-04-04 17:25:42', 0, NULL, NULL, '2019-04-04 10:54:10', NULL, '2019-04-04 14:51:27'),
	(18, 2, '1', '4/4/2019 10:54:48 AM', 0, '2019-04-04 00:00:00', '2019-04-04 10:54:48', 0, NULL, NULL, '2019-04-04 14:51:28', NULL, '2019-04-04 15:00:22'),
	(19, 2, '1', '4/4/2019 10:54:58 AM', 1, '2019-04-04 00:00:00', '2019-04-04 10:54:58', 0, NULL, NULL, '2019-04-04 14:51:28', NULL, '2019-04-04 15:00:22'),
	(20, 2, '1', '4/4/2019 2:59:21 PM', 0, '2019-04-04 00:00:00', '2019-04-04 14:59:21', 0, NULL, NULL, '2019-04-04 15:00:22', NULL, '2019-04-04 16:51:30'),
	(21, 2, '1', '4/4/2019 4:12:08 PM', 0, '2019-04-04 00:00:00', '2019-04-04 16:12:08', 0, NULL, NULL, '2019-04-04 16:51:30', NULL, '2019-04-04 17:15:26'),
	(22, 2, '1', '4/4/2019 4:12:20 PM', 1, '2019-04-04 00:00:00', '2019-04-04 16:12:20', 0, NULL, NULL, '2019-04-04 16:51:30', NULL, '2019-04-04 17:15:26'),
	(23, 2, '1', '4/4/2019 4:53:28 PM', 0, '2019-04-04 00:00:00', '2019-04-04 16:53:28', 0, NULL, NULL, '2019-04-04 17:15:26', NULL, '2019-04-04 17:16:13'),
	(24, 2, '1', '4/4/2019 4:53:33 PM', 1, '2019-04-04 00:00:00', '2019-04-04 16:53:33', 0, NULL, NULL, '2019-04-04 17:15:26', NULL, '2019-04-04 17:16:13'),
	(25, 2, '1', '4/4/2019 5:15:30 PM', 0, '2019-04-04 00:00:00', '2019-04-04 17:15:30', 0, NULL, NULL, '2019-04-04 17:16:13', NULL, '2019-04-04 17:28:06'),
	(26, 2, '1', '4/4/2019 5:27:25 PM', 0, '2019-04-04 00:00:00', '2019-04-04 17:27:25', 1, NULL, NULL, '2019-04-04 17:28:06', NULL, NULL);
/*!40000 ALTER TABLE `machinelog` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
