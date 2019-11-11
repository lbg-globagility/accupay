/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `billitem` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL DEFAULT '0',
  `CreatedBy` int(10) NOT NULL DEFAULT '0',
  `LastUpdBy` int(10) NOT NULL DEFAULT '0',
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `ProductID` int(11) DEFAULT NULL,
  `ProductDescription` varchar(300) DEFAULT NULL,
  `AmountDue` decimal(10,2) DEFAULT '0.00',
  `Qty` decimal(10,2) DEFAULT '0.00',
  `UnitPrice` decimal(10,2) DEFAULT '0.00',
  `BillDate` date NOT NULL,
  `VAT` decimal(10,2) DEFAULT '0.00',
  `WithholdingTax` decimal(10,2) DEFAULT '0.00',
  `VATTaxId` int(10) NOT NULL DEFAULT '0',
  `BillID` int(10) NOT NULL DEFAULT '0',
  `WithholdingTaxID` int(10) NOT NULL DEFAULT '0',
  `Comments` varchar(2000) DEFAULT NULL,
  `TotalAmountDue` decimal(10,2) DEFAULT '0.00',
  `LineNumber` int(10) NOT NULL DEFAULT '0',
  `AccountingPeriodID` int(10) DEFAULT NULL,
  `COAID` int(10) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 2` (`RowID`),
  KEY `FK_invoiceitems_user` (`CreatedBy`),
  KEY `FK_invoiceitems_user_2` (`LastUpdBy`),
  KEY `OrganizationID` (`OrganizationID`),
  KEY `FK_invoiceitem_accountingperiod` (`AccountingPeriodID`),
  KEY `FK_invoiceitem_chartofaccounts` (`COAID`),
  KEY `FK_billitem_product` (`ProductID`),
  KEY `FK_billitem_taxitems` (`VATTaxId`),
  KEY `FK_billitem_taxitems_2` (`WithholdingTaxID`),
  CONSTRAINT `FK_billitem_product` FOREIGN KEY (`ProductID`) REFERENCES `product` (`RowID`),
  CONSTRAINT `FK_billitem_taxitems` FOREIGN KEY (`VATTaxId`) REFERENCES `taxitems` (`RowID`),
  CONSTRAINT `FK_billitem_taxitems_2` FOREIGN KEY (`WithholdingTaxID`) REFERENCES `taxitems` (`RowID`),
  CONSTRAINT `billitem_ibfk_1` FOREIGN KEY (`AccountingPeriodID`) REFERENCES `accountingperiod` (`RowID`),
  CONSTRAINT `billitem_ibfk_2` FOREIGN KEY (`COAID`) REFERENCES `chartofaccounts` (`RowID`),
  CONSTRAINT `billitem_ibfk_4` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `billitem_ibfk_5` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `billitem_ibfk_6` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Items for that particular bill';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
