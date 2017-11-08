/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `taxitems` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastUpdBy` int(10) DEFAULT NULL,
  `AccountingPeriodID` int(10) DEFAULT NULL,
  `COAID` int(10) DEFAULT NULL,
  `TaxSource` varchar(50) DEFAULT NULL COMMENT 'Bill, Invoice, Payment',
  `TaxSourceID` int(10) DEFAULT NULL COMMENT 'RowID of the referene TaxSource',
  `TaxAmount` int(10) DEFAULT NULL,
  `IncludeInReport` char(1) DEFAULT NULL,
  `ProductID` int(11) DEFAULT NULL,
  `TaxID` int(11) NOT NULL,
  `TaxDefinitionID` int(11) NOT NULL,
  `TaxType` varchar(50) DEFAULT NULL COMMENT 'In, Out',
  PRIMARY KEY (`RowID`),
  KEY `FK_BaseTables_organization` (`OrganizationID`),
  KEY `FK_BaseTables_user` (`CreatedBy`),
  KEY `FK_BaseTables_user_2` (`LastUpdBy`),
  KEY `FK_basetables_accountingperiod` (`AccountingPeriodID`),
  KEY `FK_basetables_chartofaccounts` (`COAID`),
  KEY `FK_taxitems_product` (`ProductID`),
  KEY `FK_taxitems_taxdefinition` (`TaxDefinitionID`),
  KEY `FK_taxitems_tax` (`TaxID`),
  CONSTRAINT `FK_taxitems_product` FOREIGN KEY (`ProductID`) REFERENCES `product` (`RowID`),
  CONSTRAINT `FK_taxitems_tax` FOREIGN KEY (`TaxID`) REFERENCES `tax` (`RowID`),
  CONSTRAINT `FK_taxitems_taxdefinition` FOREIGN KEY (`TaxDefinitionID`) REFERENCES `taxdefinition` (`RowID`),
  CONSTRAINT `taxitems_ibfk_1` FOREIGN KEY (`AccountingPeriodID`) REFERENCES `accountingperiod` (`RowID`),
  CONSTRAINT `taxitems_ibfk_2` FOREIGN KEY (`COAID`) REFERENCES `chartofaccounts` (`RowID`),
  CONSTRAINT `taxitems_ibfk_3` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `taxitems_ibfk_4` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `taxitems_ibfk_5` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
