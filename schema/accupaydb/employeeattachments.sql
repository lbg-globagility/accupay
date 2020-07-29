/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `employeeattachments` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `CreatedBy` int(11) NOT NULL,
  `LastUpdBy` int(11) DEFAULT NULL,
  `EmployeeID` int(11) NOT NULL,
  `Type` varchar(50) DEFAULT NULL COMMENT 'Account, Quote, Order, ListofVal Type = "NOTE_TYPE"',
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `Created` timestamp NULL DEFAULT current_timestamp(),
  `AttachedFile` mediumblob DEFAULT NULL,
  `FileType` varchar(10) DEFAULT NULL,
  `FileName` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_employeeattachments_employee` (`EmployeeID`),
  KEY `FK_employeeattachments_user_2` (`LastUpdBy`),
  KEY `FK_employeeattachments_user` (`CreatedBy`),
  CONSTRAINT `FK_employeeattachments_employee` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `FK_employeeattachments_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_employeeattachments_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Notes table used to add remarks.';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
