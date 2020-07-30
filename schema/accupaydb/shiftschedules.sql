/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `shiftschedules` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) DEFAULT NULL,
  `Created` timestamp NOT NULL DEFAULT current_timestamp(),
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `LastUpdBy` int(10) DEFAULT NULL,
  `EmployeeID` int(10) DEFAULT NULL,
  `Date` date NOT NULL,
  `StartTime` time DEFAULT NULL,
  `EndTime` time DEFAULT NULL,
  `BreakStartTime` time DEFAULT NULL,
  `BreakLength` decimal(10,2) DEFAULT 0.00,
  `IsRestDay` tinyint(1) NOT NULL DEFAULT 0,
  `ShiftHours` decimal(10,2) DEFAULT 0.00,
  `WorkHours` decimal(10,2) DEFAULT 0.00,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `AK_EmployeeID_Date` (`EmployeeID`,`Date`),
  KEY `FK_shiftschedules_organization_OrganizationID` (`OrganizationID`),
  KEY `FK_shiftschedules_user_CreatedBy` (`CreatedBy`),
  KEY `FK_shiftschedules_user_LastUpdBy` (`LastUpdBy`),
  CONSTRAINT `FK_shiftschedules_organization_OrganizationID` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`) ON DELETE SET NULL,
  CONSTRAINT `FK_shiftschedules_user_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`) ON DELETE SET NULL,
  CONSTRAINT `FK_shiftschedules_user_LastUpdBy` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`) ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
