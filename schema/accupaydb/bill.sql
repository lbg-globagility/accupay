/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `bill` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `BillNo` int(10) DEFAULT NULL,
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpdBy` int(10) DEFAULT NULL,
  `Created` datetime DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `OrganizationID` int(10) DEFAULT NULL,
  `BillDate` date DEFAULT NULL,
  `BillDueDate` date DEFAULT NULL,
  `Status` varchar(50) DEFAULT NULL,
  `BillPeriod` varchar(100) DEFAULT NULL,
  `TotalBillAmount` decimal(10,2) DEFAULT '0.00',
  `TotalDue` decimal(10,2) DEFAULT '0.00',
  `BalanceForward` decimal(10,2) DEFAULT '0.00',
  `BalanceDue` decimal(10,2) DEFAULT '0.00',
  `PaymentAmount` decimal(10,2) DEFAULT '0.00',
  `Comments` varchar(2000) NOT NULL,
  `BillType` varchar(50) DEFAULT NULL,
  `Vendor` varchar(100) DEFAULT NULL,
  `TotalWithholdingTax` decimal(10,2) DEFAULT '0.00',
  `TotalPenalties` decimal(10,2) DEFAULT '0.00',
  `TotalVAT` decimal(10,2) DEFAULT '0.00',
  `TotalGrossAmount` decimal(10,2) DEFAULT '0.00',
  `AccountingPeriodID` int(10) DEFAULT NULL,
  `TaxID` int(10) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 2` (`OrganizationID`,`BillNo`),
  KEY `FK_invoice_user` (`CreatedBy`),
  KEY `FK_invoice_user_2` (`LastUpdBy`),
  KEY `FK_invoice_accountingperiod` (`AccountingPeriodID`),
  KEY `FK_bill_tax` (`TaxID`),
  CONSTRAINT `FK_bill_tax` FOREIGN KEY (`TaxID`) REFERENCES `tax` (`RowID`),
  CONSTRAINT `bill_ibfk_1` FOREIGN KEY (`AccountingPeriodID`) REFERENCES `accountingperiod` (`RowID`),
  CONSTRAINT `bill_ibfk_7` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `bill_ibfk_8` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `bill_ibfk_9` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='This is the billing statement table (Header)';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
