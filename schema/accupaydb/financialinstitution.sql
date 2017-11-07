/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `financialinstitution` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL COMMENT 'BDO, Metrobank, Security Bank, etc',
  `Branch` varchar(50) NOT NULL COMMENT 'Ortigas,Quezon City, Shaw,etc.',
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Type` varchar(50) DEFAULT NULL COMMENT 'Bank, Credit Card, etc (LOV Type="FININST_TYPE"',
  `FaxNo` varchar(50) DEFAULT NULL,
  `EmailAddress` varchar(50) DEFAULT NULL,
  `OrganizationID` int(11) NOT NULL,
  `ContactID` int(11) DEFAULT NULL,
  `CreatedBy` int(11) NOT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastUpdBy` int(11) NOT NULL,
  `MainPhone` varchar(50) DEFAULT NULL,
  `AddressID` int(11) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK__address` (`AddressID`),
  KEY `FK_bank_user` (`CreatedBy`),
  KEY `FK_bank_user_2` (`LastUpdBy`),
  KEY `FK_bank_organization` (`OrganizationID`),
  KEY `FK_bank_contact` (`ContactID`),
  CONSTRAINT `FK__address` FOREIGN KEY (`AddressID`) REFERENCES `address` (`RowID`),
  CONSTRAINT `FK_bank_contact` FOREIGN KEY (`ContactID`) REFERENCES `contact` (`RowID`),
  CONSTRAINT `FK_bank_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_bank_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_bank_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='List of Banks and their Information';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
