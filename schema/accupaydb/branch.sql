/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `branch` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `Created` timestamp NOT NULL DEFAULT current_timestamp(),
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `LastUpdBy` int(10) DEFAULT NULL,
  `BranchCode` varchar(100) DEFAULT '',
  `BranchName` varchar(100) DEFAULT '',
  `AddressID` int(10) DEFAULT NULL,
  `CalendarID` int(10) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `AK_branch_BranchCode` (`BranchCode`),
  KEY `FK_BaseTables_user` (`CreatedBy`),
  KEY `FK_BaseTables_user_2` (`LastUpdBy`),
  KEY `agency_ibfk_5` (`AddressID`),
  KEY `FK_branch_calendar_CalendarID` (`CalendarID`),
  CONSTRAINT `FK_branch_address_AddressID` FOREIGN KEY (`AddressID`) REFERENCES `address` (`RowID`),
  CONSTRAINT `FK_branch_calendar_CalendarID` FOREIGN KEY (`CalendarID`) REFERENCES `calendar` (`RowID`),
  CONSTRAINT `FK_branch_user_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_branch_user_LastUpdBy` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
