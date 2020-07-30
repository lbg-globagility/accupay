/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `employeetimeentrydetails` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT current_timestamp(),
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `LastUpdBy` int(10) DEFAULT NULL,
  `EmployeeID` int(10) DEFAULT NULL,
  `TimeIn` time DEFAULT NULL,
  `TimeOut` time DEFAULT NULL,
  `Date` date DEFAULT NULL,
  `TimeScheduleType` varchar(50) DEFAULT '' COMMENT 'F-Flexible',
  `TimeEntryStatus` varchar(50) DEFAULT '',
  `ChargeToDivisionID` int(11) DEFAULT NULL,
  `TimeStampIn` datetime DEFAULT NULL,
  `TimeStampOut` datetime DEFAULT NULL,
  `TimeentrylogsImportID` varchar(50) DEFAULT '',
  `BranchID` int(11) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_BaseTables_organization` (`OrganizationID`),
  KEY `FK_BaseTables_user` (`CreatedBy`),
  KEY `FK_BaseTables_user_2` (`LastUpdBy`),
  KEY `FK_employeetimeentrydetails_employee` (`EmployeeID`),
  KEY `FK_employeetimeentrydetails_branch_BranchID` (`BranchID`),
  CONSTRAINT `FK_employeetimeentrydetails_branch_BranchID` FOREIGN KEY (`BranchID`) REFERENCES `branch` (`RowID`),
  CONSTRAINT `FK_employeetimeentrydetails_employee_EmployeeID` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `FK_employeetimeentrydetails_organization_OrganizationID` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_employeetimeentrydetails_user_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_employeetimeentrydetails_user_LastUpdBy` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
