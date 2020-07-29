/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `employeedependents` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `CreatedBy` int(10) DEFAULT NULL,
  `Created` timestamp NULL DEFAULT current_timestamp(),
  `LastUpdBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL,
  `OrganizationID` int(10) DEFAULT NULL,
  `Salutation` varchar(50) DEFAULT NULL,
  `FirstName` varchar(100) DEFAULT NULL,
  `MiddleName` varchar(100) DEFAULT NULL,
  `LastName` varchar(100) DEFAULT NULL,
  `Surname` varchar(50) DEFAULT NULL,
  `ParentEmployeeID` int(11) DEFAULT NULL,
  `TINNo` varchar(50) DEFAULT NULL,
  `SSSNo` varchar(50) DEFAULT NULL,
  `HDMFNo` varchar(50) DEFAULT NULL,
  `PhilHealthNo` varchar(50) DEFAULT NULL,
  `EmailAddress` varchar(50) DEFAULT NULL,
  `WorkPhone` varchar(50) DEFAULT NULL,
  `HomePhone` varchar(50) DEFAULT NULL,
  `MobilePhone` varchar(50) DEFAULT NULL,
  `HomeAddress` varchar(1000) DEFAULT NULL,
  `Nickname` varchar(50) DEFAULT NULL,
  `JobTitle` varchar(50) DEFAULT NULL,
  `Gender` varchar(50) DEFAULT NULL,
  `RelationToEmployee` varchar(50) DEFAULT NULL,
  `ActiveFlag` varchar(1) DEFAULT NULL,
  `Birthdate` date DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `Index 3` (`FirstName`,`MiddleName`,`LastName`,`Salutation`),
  KEY `Index 4` (`TINNo`),
  KEY `Index 5` (`SSSNo`),
  KEY `FK_Employee_user` (`CreatedBy`),
  KEY `FK_Employee_user_2` (`LastUpdBy`),
  KEY `FK_employeedependents_organization` (`OrganizationID`),
  KEY `FK_employeedependents_employee_RowID` (`ParentEmployeeID`),
  CONSTRAINT `FK_employeedependents_employee_RowID` FOREIGN KEY (`ParentEmployeeID`) REFERENCES `employee` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `FK_employeedependents_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `employeedependents_ibfk_4` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `employeedependents_ibfk_5` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Employee Table';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
