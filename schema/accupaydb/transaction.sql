/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `transaction` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastUpdBy` int(10) DEFAULT NULL,
  `AccountingPeriodID` int(10) NOT NULL,
  `COAId` int(10) NOT NULL,
  `CategoryCOAID` int(10) NOT NULL,
  `Description` varchar(300) DEFAULT NULL,
  `Category` varchar(300) DEFAULT NULL COMMENT 'Text only derived from COAId',
  `Amount` decimal(10,2) NOT NULL,
  `JournalID` int(11) DEFAULT NULL,
  `BillID` int(11) DEFAULT NULL,
  `BillPaymentDate` date DEFAULT NULL,
  `InvoiceID` int(11) DEFAULT NULL,
  `Verify` varchar(1) DEFAULT NULL,
  `TransactionDate` date NOT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_BaseTables_organization` (`OrganizationID`),
  KEY `FK_BaseTables_user` (`CreatedBy`),
  KEY `FK_BaseTables_user_2` (`LastUpdBy`),
  KEY `FK_journalitem_accountingperiod` (`AccountingPeriodID`),
  KEY `FK_journalitem_chartofaccounts` (`COAId`),
  KEY `FK_journalitem_journal` (`JournalID`),
  KEY `FK_transaction_chartofaccounts` (`CategoryCOAID`),
  CONSTRAINT `FK_transaction_chartofaccounts` FOREIGN KEY (`CategoryCOAID`) REFERENCES `chartofaccounts` (`RowID`),
  CONSTRAINT `transaction_ibfk_1` FOREIGN KEY (`AccountingPeriodID`) REFERENCES `accountingperiod` (`RowID`),
  CONSTRAINT `transaction_ibfk_2` FOREIGN KEY (`COAId`) REFERENCES `chartofaccounts` (`RowID`),
  CONSTRAINT `transaction_ibfk_3` FOREIGN KEY (`JournalID`) REFERENCES `journal` (`RowID`),
  CONSTRAINT `transaction_ibfk_4` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `transaction_ibfk_5` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `transaction_ibfk_6` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
