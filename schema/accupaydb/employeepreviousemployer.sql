/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `employeepreviousemployer` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) DEFAULT NULL,
  `TradeName` varchar(100) DEFAULT NULL,
  `OrganizationID` int(10) NOT NULL,
  `EmployeeID` int(10) NOT NULL,
  `MainPhone` varchar(50) DEFAULT NULL,
  `FaxNumber` varchar(50) DEFAULT NULL,
  `JobTitle` varchar(50) DEFAULT NULL,
  `ExperienceFromTo` date NOT NULL,
  `ExperienceTo` date NOT NULL,
  `BusinessAddress` varchar(1000) DEFAULT NULL,
  `ContactName` varchar(200) DEFAULT NULL,
  `EmailAddress` varchar(50) DEFAULT NULL,
  `AltEmailAddress` varchar(50) DEFAULT NULL,
  `AltPhone` varchar(50) DEFAULT NULL,
  `URL` varchar(50) DEFAULT NULL,
  `TINNo` varchar(50) DEFAULT NULL,
  `JobFunction` varchar(2000) DEFAULT NULL,
  `Created` timestamp NULL DEFAULT current_timestamp(),
  `CreatedBy` int(11) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `LastUpdBy` int(11) DEFAULT NULL,
  `OrganizationType` varchar(50) DEFAULT NULL COMMENT 'Commercial Center, Office Building',
  PRIMARY KEY (`RowID`),
  KEY `FK_organization_user` (`CreatedBy`),
  KEY `FK_organization_user_2` (`LastUpdBy`),
  KEY `FK_employeepreviousemployer_organization` (`OrganizationID`),
  KEY `FK_employeepreviousemployer_employee` (`EmployeeID`),
  CONSTRAINT `FK_employeepreviousemployer_employee` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `FK_employeepreviousemployer_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_employeepreviousemployer_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_employeepreviousemployer_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `employeepreviousemployer_ibfk_3` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `employeepreviousemployer_ibfk_4` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='This is the internal Company';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
