/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `utilities` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `ActiveFlag` char(1) NOT NULL,
  `OrganizationID` int(10) NOT NULL,
  `AccountID` int(10) DEFAULT NULL,
  `ContractID` int(10) DEFAULT NULL,
  `CreatedBy` int(10) NOT NULL,
  `LastUpdBy` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `MeterNo` varchar(50) DEFAULT NULL,
  `UsageArea` decimal(10,2) DEFAULT NULL COMMENT 'If there is no readings, Usage Area is used to calculate.  Usage Area should be the same as the lot area the Account/Tenant is occupying based on the contract.',
  `UtilityType` varchar(50) DEFAULT NULL COMMENT 'LOV=UTILITY_TYPE, Water, Electricity',
  `PreviousMeterReading` int(10) DEFAULT NULL,
  `ParentUtilityMeter` int(10) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 6` (`UtilityType`,`OrganizationID`,`MeterNo`),
  KEY `FK_utilities_organization` (`OrganizationID`),
  KEY `FK_utilities_account` (`AccountID`),
  KEY `FK_utilities_user` (`CreatedBy`),
  KEY `FK_utilities_user_2` (`LastUpdBy`),
  KEY `FK_utilities_contract` (`ContractID`),
  CONSTRAINT `FK_utilities_account` FOREIGN KEY (`AccountID`) REFERENCES `account` (`RowID`),
  CONSTRAINT `FK_utilities_contract` FOREIGN KEY (`ContractID`) REFERENCES `contract` (`RowID`),
  CONSTRAINT `FK_utilities_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_utilities_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_utilities_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Definition of Utilities for each company';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
