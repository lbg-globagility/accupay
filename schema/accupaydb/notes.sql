/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `notes` (
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
  `Note` varchar(5000) DEFAULT NULL,
  `InvoiceID` int(10) DEFAULT NULL,
  `PaymentID` int(10) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_Notes_account` (`AccountID`),
  KEY `FK_Notes_quote` (`QuoteID`),
  KEY `FK_Notes_order` (`OrderID`),
  KEY `FK_Notes_user` (`CreatedBy`),
  KEY `FK_Notes_user_2` (`LastUpdBy`),
  KEY `FK_Notes_contract` (`ContractID`),
  KEY `FK_notes_invoice` (`InvoiceID`),
  KEY `FK_notes_payment` (`PaymentID`),
  CONSTRAINT `FK_Notes_account` FOREIGN KEY (`AccountID`) REFERENCES `account` (`RowID`),
  CONSTRAINT `FK_Notes_contract` FOREIGN KEY (`ContractID`) REFERENCES `contract` (`RowID`),
  CONSTRAINT `FK_Notes_order` FOREIGN KEY (`OrderID`) REFERENCES `order` (`RowID`),
  CONSTRAINT `FK_Notes_quote` FOREIGN KEY (`QuoteID`) REFERENCES `quote` (`RowID`),
  CONSTRAINT `FK_Notes_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_Notes_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_notes_invoice` FOREIGN KEY (`InvoiceID`) REFERENCES `invoice` (`RowID`),
  CONSTRAINT `FK_notes_payment` FOREIGN KEY (`PaymentID`) REFERENCES `payment` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Notes table used to add remarks.';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
