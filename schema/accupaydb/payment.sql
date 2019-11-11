/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `payment` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `CreatedBy` int(10) NOT NULL,
  `LastUpdBy` int(10) NOT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `PaymentType` varchar(50) DEFAULT NULL,
  `PDCAmount` decimal(10,2) DEFAULT '0.00',
  `Amount` decimal(10,2) DEFAULT '0.00',
  `FinancialInstitutionID` int(10) DEFAULT NULL,
  `BankAccountNumber` varchar(10) DEFAULT NULL,
  `BankCheckNumber` varchar(10) DEFAULT NULL,
  `BankRoutingNumber` varchar(10) DEFAULT NULL,
  `CardHolder` varchar(100) DEFAULT NULL,
  `CardNumber` varchar(10) DEFAULT NULL,
  `CreditMemoNumber` varchar(10) DEFAULT NULL,
  `Description` varchar(2000) DEFAULT NULL,
  `ExpirationDate` varchar(50) DEFAULT NULL,
  `ContactID` int(10) DEFAULT NULL,
  `ContractID` int(10) DEFAULT NULL,
  `RequestedAmount` decimal(10,2) DEFAULT '0.00',
  `RemainingBalance` decimal(10,2) DEFAULT '0.00',
  `PaymentNo` int(10) DEFAULT NULL,
  `AccountID` int(10) DEFAULT NULL,
  `AccountingPeriodID` int(10) DEFAULT NULL,
  `COAID` int(10) DEFAULT NULL,
  `PaymentDate` date DEFAULT NULL,
  `PaymentMethod` varchar(50) DEFAULT NULL,
  `ReferenceNumber` varchar(50) DEFAULT NULL,
  `PDCDateUsed` date DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_payment_organization` (`OrganizationID`),
  KEY `FK_payment_user` (`CreatedBy`),
  KEY `FK_payment_user_2` (`LastUpdBy`),
  KEY `FK_payment_financialinstitution` (`FinancialInstitutionID`),
  KEY `FK_payment_contact` (`ContactID`),
  KEY `FK_payment_account` (`AccountID`),
  KEY `FK_payment_contract` (`ContractID`),
  KEY `FK_payment_chartofaccounts` (`COAID`),
  KEY `FK_payment_accountingperiod` (`AccountingPeriodID`),
  CONSTRAINT `FK_payment_account` FOREIGN KEY (`AccountID`) REFERENCES `account` (`RowID`),
  CONSTRAINT `FK_payment_accountingperiod` FOREIGN KEY (`AccountingPeriodID`) REFERENCES `accountingperiod` (`RowID`),
  CONSTRAINT `FK_payment_chartofaccounts` FOREIGN KEY (`COAID`) REFERENCES `chartofaccounts` (`RowID`),
  CONSTRAINT `FK_payment_contact` FOREIGN KEY (`ContactID`) REFERENCES `contact` (`RowID`),
  CONSTRAINT `FK_payment_contract` FOREIGN KEY (`ContractID`) REFERENCES `contract` (`RowID`),
  CONSTRAINT `FK_payment_financialinstitution` FOREIGN KEY (`FinancialInstitutionID`) REFERENCES `financialinstitution` (`RowID`),
  CONSTRAINT `FK_payment_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_payment_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_payment_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
