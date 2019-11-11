/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `attachments` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `CreatedBy` int(11) DEFAULT NULL,
  `LastUpdBy` int(11) DEFAULT NULL,
  `AccountID` int(11) DEFAULT NULL,
  `QuoteID` int(11) DEFAULT NULL,
  `OrderID` int(11) DEFAULT NULL,
  `ContractID` int(11) DEFAULT NULL,
  `Type` varchar(50) DEFAULT NULL COMMENT 'Account, Quote, Order, ListofVal Type = "NOTE_TYPE"',
  `LastUpd` datetime DEFAULT CURRENT_TIMESTAMP,
  `Created` timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `AttachedFile` mediumblob,
  `FileType` varchar(10) DEFAULT NULL,
  `FileName` varchar(100) DEFAULT NULL,
  `InvoiceID` int(10) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_attachments_account` (`AccountID`),
  KEY `FK_attachments_quote` (`QuoteID`),
  KEY `FK_attachments_order` (`OrderID`),
  KEY `FK_attachments_contract` (`ContractID`),
  KEY `InvoiceID` (`InvoiceID`),
  CONSTRAINT `FK_attachments_account` FOREIGN KEY (`AccountID`) REFERENCES `account` (`RowID`),
  CONSTRAINT `FK_attachments_contract` FOREIGN KEY (`ContractID`) REFERENCES `contract` (`RowID`),
  CONSTRAINT `FK_attachments_invoice` FOREIGN KEY (`InvoiceID`) REFERENCES `invoice` (`RowID`),
  CONSTRAINT `FK_attachments_order` FOREIGN KEY (`OrderID`) REFERENCES `order` (`RowID`),
  CONSTRAINT `FK_attachments_quote` FOREIGN KEY (`QuoteID`) REFERENCES `quote` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Notes table used to add remarks.';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
