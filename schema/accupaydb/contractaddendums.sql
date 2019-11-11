/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `contractaddendums` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `ContractID` int(10) NOT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpdBy` int(10) DEFAULT NULL,
  `CreatedBy` int(10) NOT NULL,
  `OrganizationID` int(10) NOT NULL,
  `ProductID` int(10) NOT NULL,
  `EffectiveFromDate` date NOT NULL,
  `AdditionalLeasingSqM` decimal(10,2) DEFAULT NULL,
  `TotalAmount` decimal(10,2) NOT NULL,
  `Comments` varchar(2000) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_ContractAddendums_contract` (`ContractID`),
  KEY `FK_ContractAddendums_user` (`LastUpdBy`),
  KEY `FK_ContractAddendums_user_2` (`CreatedBy`),
  KEY `FK_ContractAddendums_organization` (`OrganizationID`),
  KEY `FK_ContractAddendums_product` (`ProductID`),
  CONSTRAINT `FK_ContractAddendums_contract` FOREIGN KEY (`ContractID`) REFERENCES `contract` (`RowID`),
  CONSTRAINT `FK_ContractAddendums_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_ContractAddendums_product` FOREIGN KEY (`ProductID`) REFERENCES `product` (`RowID`),
  CONSTRAINT `FK_ContractAddendums_user` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_ContractAddendums_user_2` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='additional items after contract has been signed';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
