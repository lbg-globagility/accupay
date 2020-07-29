/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `employeetimeattendancelog` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT current_timestamp(),
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `LastUpdBy` int(10) DEFAULT NULL,
  `EmployeeID` int(10) NOT NULL,
  `TimeStamp` date NOT NULL,
  `IsTimeIn` tinyint(1) DEFAULT NULL,
  `WorkDay` date NOT NULL,
  `ImportNumber` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_employeetimeattendancelog_organization_OrganizationID` (`OrganizationID`),
  KEY `FK_employeetimeattendancelog_user_CreatedBy` (`CreatedBy`),
  KEY `FK_employeetimeattendancelog_user_LastUpdBy` (`LastUpdBy`),
  KEY `FK_employeetimeattendancelog_employee_EmployeeID` (`EmployeeID`),
  CONSTRAINT `FK_employeetimeattendancelog_employee_EmployeeID` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `FK_employeetimeattendancelog_organization_OrganizationID` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_employeetimeattendancelog_user_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_employeetimeattendancelog_user_LastUpdBy` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
