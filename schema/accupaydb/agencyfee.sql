/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `agencyfee` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT current_timestamp(),
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `LastUpdBy` int(10) DEFAULT NULL,
  `AgencyID` int(10) DEFAULT NULL,
  `EmployeeID` int(10) DEFAULT NULL,
  `EmpPositionID` int(10) DEFAULT NULL,
  `DivisionID` int(10) DEFAULT NULL,
  `TimeEntryID` int(11) DEFAULT NULL,
  `TimeEntryDate` date DEFAULT NULL,
  `DailyFee` decimal(11,2) DEFAULT 0.00,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `UniqueColumn` (`TimeEntryID`),
  KEY `FK_BaseTables_organization` (`OrganizationID`),
  KEY `FK_BaseTables_user` (`CreatedBy`),
  KEY `FK_BaseTables_user_2` (`LastUpdBy`),
  KEY `FK_basetables_employee` (`EmployeeID`),
  KEY `agencyfee_ibfk_5` (`AgencyID`),
  KEY `agencyfee_ibfk_6` (`DivisionID`),
  CONSTRAINT `agencyfee_ibfk_1` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`),
  CONSTRAINT `agencyfee_ibfk_2` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `agencyfee_ibfk_3` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `agencyfee_ibfk_4` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `agencyfee_ibfk_5` FOREIGN KEY (`AgencyID`) REFERENCES `agency` (`RowID`),
  CONSTRAINT `agencyfee_ibfk_6` FOREIGN KEY (`DivisionID`) REFERENCES `division` (`RowID`),
  CONSTRAINT `agencyfee_ibfk_7` FOREIGN KEY (`TimeEntryID`) REFERENCES `employeetimeentry` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
