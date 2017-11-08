/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `contact` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `AddressID` int(10) DEFAULT NULL,
  `Status` varchar(50) DEFAULT '0' COMMENT 'On Leave,Prospect,Active,Deceased,,Inactive etc.',
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Type` varchar(50) DEFAULT NULL COMMENT 'Partners',
  `AccountID` int(11) DEFAULT NULL COMMENT 'Links to the Account.  A Company can have multiple contacts.',
  `EmployeeFlg` varchar(1) DEFAULT NULL COMMENT 'Y if contact is employee',
  `OrganizationID` int(11) DEFAULT NULL COMMENT 'Internal Company',
  `MainPhone` varchar(50) DEFAULT NULL,
  `PersonalTitle` varchar(50) DEFAULT NULL COMMENT 'Mr, Mrs, Dr, etc.',
  `TINNumber` varchar(50) DEFAULT NULL,
  `LastName` varchar(50) DEFAULT NULL,
  `FirstName` varchar(50) DEFAULT NULL,
  `MiddleName` varchar(50) DEFAULT NULL,
  `MobilePhone` varchar(50) DEFAULT NULL,
  `WorkPhone` varchar(50) DEFAULT NULL,
  `Suffix` varchar(50) DEFAULT NULL,
  `Gender` varchar(50) DEFAULT NULL,
  `Nickname` varchar(50) DEFAULT NULL,
  `JobTitle` varchar(50) DEFAULT NULL,
  `EmailAddress` varchar(50) DEFAULT NULL,
  `AlternatePhone` varchar(50) DEFAULT NULL,
  `FaxNumber` varchar(50) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `CreatedBy` int(11) DEFAULT NULL,
  `LastUpdBy` int(11) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_contact_address` (`AddressID`),
  KEY `FK_contact_organization` (`OrganizationID`),
  KEY `FK_contact_user` (`LastUpdBy`),
  KEY `FK_contact_user_2` (`CreatedBy`),
  KEY `FK_contact_contact` (`AccountID`),
  CONSTRAINT `FK_contact_address` FOREIGN KEY (`AddressID`) REFERENCES `address` (`RowID`),
  CONSTRAINT `FK_contact_contact` FOREIGN KEY (`AccountID`) REFERENCES `contact` (`RowID`),
  CONSTRAINT `FK_contact_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_contact_user` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_contact_user_2` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Contacts';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
