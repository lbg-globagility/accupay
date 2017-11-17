/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `inventorylocation` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL DEFAULT '0',
  `OrganizationID` int(11) NOT NULL DEFAULT '0' COMMENT 'Company FK to Organization table',
  `AddressID` int(11) DEFAULT NULL COMMENT 'FK to Address Table',
  `Status` varchar(50) DEFAULT NULL COMMENT 'Active, Inactive',
  `MainPhone` varchar(50) DEFAULT NULL,
  `MobilePhone` varchar(50) DEFAULT NULL,
  `FaxNumber` varchar(50) DEFAULT NULL,
  `PrimaryContactID` int(11) DEFAULT NULL COMMENT 'contact associated to this location. (for example, contact person in the branch). links to contact table',
  `CreatedBy` int(11) NOT NULL DEFAULT '0',
  `LastUpdBy` int(11) NOT NULL DEFAULT '0',
  `Type` varchar(50) DEFAULT NULL COMMENT 'Branch,Warehouse,Office,Retail Store',
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `MainBranch` char(1) DEFAULT NULL COMMENT 'used to flag if this is the main warehouse/branch or not.  Y means main warehouse, N means not.',
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 7` (`Name`,`OrganizationID`),
  KEY `FK_branch_address` (`AddressID`),
  KEY `FK_inventorylocation_contact` (`PrimaryContactID`),
  KEY `FK_inventorylocation_organization` (`OrganizationID`),
  KEY `FK_inventorylocation_user` (`CreatedBy`),
  KEY `FK_inventorylocation_user_2` (`LastUpdBy`),
  CONSTRAINT `FK_branch_address` FOREIGN KEY (`AddressID`) REFERENCES `address` (`RowID`),
  CONSTRAINT `FK_inventorylocation_contact` FOREIGN KEY (`PrimaryContactID`) REFERENCES `contact` (`RowID`),
  CONSTRAINT `FK_inventorylocation_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_inventorylocation_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_inventorylocation_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Company Locations';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
