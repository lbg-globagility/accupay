/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `employeeoffset` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `EmployeeID` int(10) DEFAULT NULL,
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpdBy` int(10) DEFAULT NULL,
  `Created` timestamp NOT NULL DEFAULT current_timestamp(),
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `Type` varchar(50) DEFAULT NULL COMMENT 'Overtime',
  `StartTime` time DEFAULT NULL,
  `EndTime` time DEFAULT NULL,
  `StartDate` date DEFAULT NULL,
  `EndDate` date DEFAULT NULL,
  `Status` varchar(50) DEFAULT NULL COMMENT 'Pending, Approved',
  `Reason` varchar(500) DEFAULT NULL,
  `Comments` varchar(2000) DEFAULT NULL,
  `Image` mediumblob DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_BaseTables_organization` (`OrganizationID`),
  KEY `FK_BaseTables_user` (`CreatedBy`),
  KEY `FK_BaseTables_user_2` (`LastUpdBy`),
  KEY `FK_employeetimeentrydetails_employee` (`EmployeeID`),
  CONSTRAINT `employeeoffset_ibfk_1` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `employeeoffset_ibfk_2` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `employeeoffset_ibfk_3` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `employeeoffset_ibfk_4` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
