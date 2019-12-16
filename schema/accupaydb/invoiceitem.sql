/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `invoiceitem` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL DEFAULT '0',
  `CreatedBy` int(10) NOT NULL DEFAULT '0',
  `LastUpdBy` int(10) NOT NULL DEFAULT '0',
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `ItemCode` varchar(50) DEFAULT '0' COMMENT 'LOV Type = "Invoice Item"',
  `ItemDescription` varchar(1000) DEFAULT '0',
  `AmountDue` decimal(10,2) DEFAULT '0.00',
  `InvoiceDate` date NOT NULL,
  `VAT` decimal(10,2) DEFAULT '0.00',
  `WithholdingTax` decimal(10,2) DEFAULT '0.00',
  `InvoiceID` int(10) NOT NULL DEFAULT '0',
  `AccountID` int(10) DEFAULT NULL,
  `BillingPeriodID` int(10) DEFAULT NULL,
  `AccountBillingPeriodID` int(10) DEFAULT NULL,
  `DiscountAmount` decimal(10,2) DEFAULT NULL,
  `DiscountPercent` decimal(10,2) DEFAULT NULL,
  `TotalDiscount` decimal(10,2) DEFAULT NULL,
  `Comments` varchar(2000) DEFAULT NULL,
  `TotalAmountDue` decimal(10,2) DEFAULT '0.00',
  `LineNumber` int(10) NOT NULL DEFAULT '0',
  `OrderID` int(10) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 2` (`RowID`),
  KEY `FK_invoiceitems_user` (`CreatedBy`),
  KEY `FK_invoiceitems_user_2` (`LastUpdBy`),
  KEY `FK_invoiceitem_billingperiod` (`BillingPeriodID`),
  KEY `FK_invoiceitem_account` (`AccountID`),
  KEY `FK_invoiceitem_order` (`OrderID`),
  KEY `FK_invoiceitem_accountbillingperiod` (`AccountBillingPeriodID`),
  KEY `OrganizationID` (`OrganizationID`),
  KEY `InvoiceID` (`InvoiceID`),
  CONSTRAINT `FK_invoiceitem_account` FOREIGN KEY (`AccountID`) REFERENCES `account` (`RowID`),
  CONSTRAINT `FK_invoiceitem_accountbillingperiod` FOREIGN KEY (`AccountBillingPeriodID`) REFERENCES `accountbillingperiod` (`RowID`),
  CONSTRAINT `FK_invoiceitem_billingperiod` FOREIGN KEY (`BillingPeriodID`) REFERENCES `billingperiod` (`RowID`),
  CONSTRAINT `FK_invoiceitem_order` FOREIGN KEY (`OrderID`) REFERENCES `order` (`RowID`),
  CONSTRAINT `FK_invoiceitems_invoice` FOREIGN KEY (`InvoiceID`) REFERENCES `invoice` (`RowID`),
  CONSTRAINT `FK_invoiceitems_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_invoiceitems_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_invoiceitems_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Items for that particular bill';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
