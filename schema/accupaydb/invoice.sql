/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `invoice` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `InvoiceNo` int(10) DEFAULT NULL,
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpdBy` int(10) DEFAULT NULL,
  `Created` datetime DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `OrganizationID` int(10) DEFAULT NULL,
  `InvoiceDate` date DEFAULT NULL,
  `Status` varchar(50) DEFAULT NULL,
  `BillPeriod` varchar(100) DEFAULT NULL,
  `TotalInvoiceAmount` decimal(10,2) DEFAULT NULL,
  `TotalDue` decimal(10,2) DEFAULT NULL,
  `BalanceForward` decimal(10,2) DEFAULT NULL,
  `BalanceDue` decimal(10,2) DEFAULT NULL,
  `PaymentAmount` decimal(10,2) DEFAULT NULL,
  `BillToAccountID` int(10) DEFAULT NULL,
  `LatePaymentFlag` char(1) DEFAULT NULL,
  `BillPeriodID` int(11) DEFAULT NULL,
  `OrderID` int(10) DEFAULT NULL,
  `InvoiceDueDate` date DEFAULT NULL,
  `Comments` varchar(2000) DEFAULT NULL,
  `InvoiceType` varchar(50) DEFAULT NULL,
  `WaivePenalty` char(1) DEFAULT NULL,
  `BillPeriodStartDate` date DEFAULT NULL,
  `BillPeriodEndDate` date DEFAULT NULL,
  `PaymentTerms` varchar(50) DEFAULT NULL,
  `ContractID` int(10) DEFAULT NULL,
  `PayableToAccountID` int(10) DEFAULT NULL,
  `AvailableCredit` decimal(10,2) DEFAULT NULL,
  `DepositAmount` decimal(10,2) DEFAULT NULL,
  `DepositAmountReceived` decimal(10,2) DEFAULT NULL,
  `DepositDate` date DEFAULT NULL,
  `DepositDueDate` date DEFAULT NULL,
  `TotalTax` decimal(10,2) DEFAULT NULL,
  `TotalPenalties` decimal(10,2) DEFAULT NULL,
  `TotalVAT` decimal(10,2) DEFAULT NULL,
  `DiscountPercent` decimal(10,2) DEFAULT NULL,
  `DiscountAmount` decimal(10,2) DEFAULT NULL,
  `TotalDiscounts` decimal(10,2) DEFAULT NULL,
  `TotalGrossAmount` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 2` (`OrganizationID`,`InvoiceNo`),
  KEY `FK_invoice_user` (`CreatedBy`),
  KEY `FK_invoice_user_2` (`LastUpdBy`),
  KEY `FK_invoice_account` (`BillToAccountID`),
  KEY `FK_invoice_billingperiod` (`BillPeriodID`),
  KEY `FK_invoice_order` (`OrderID`),
  KEY `FK_invoice_contract` (`ContractID`),
  KEY `FK_invoice_account_2` (`PayableToAccountID`),
  CONSTRAINT `FK_invoice_account` FOREIGN KEY (`BillToAccountID`) REFERENCES `account` (`RowID`),
  CONSTRAINT `FK_invoice_account_2` FOREIGN KEY (`PayableToAccountID`) REFERENCES `account` (`RowID`),
  CONSTRAINT `FK_invoice_billingperiod` FOREIGN KEY (`BillPeriodID`) REFERENCES `billingperiod` (`RowID`),
  CONSTRAINT `FK_invoice_contract` FOREIGN KEY (`ContractID`) REFERENCES `contract` (`RowID`),
  CONSTRAINT `FK_invoice_order` FOREIGN KEY (`OrderID`) REFERENCES `order` (`RowID`),
  CONSTRAINT `FK_invoice_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_invoice_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_invoice_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='This is the billing statement table (Header)';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
