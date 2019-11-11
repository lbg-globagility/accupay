/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `paymentfinancialinstitution` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `CreatedBy` int(10) NOT NULL,
  `LastUpdBy` int(10) NOT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `PaymentType` varchar(50) DEFAULT NULL,
  `AmountApplied` decimal(10,2) DEFAULT '0.00',
  `FinancialInstitutionID` int(10) NOT NULL,
  `BankAccountNumber` varchar(10) DEFAULT NULL,
  `BankCheckNumber` varchar(10) DEFAULT NULL,
  `BankRoutingNumber` varchar(10) DEFAULT NULL,
  `CardHolder` varchar(100) DEFAULT NULL,
  `CardNumber` varchar(10) DEFAULT NULL,
  `CreditMemoNumber` varchar(10) DEFAULT NULL,
  `Description` varchar(2000) DEFAULT NULL,
  `ExpirationDate` varchar(50) DEFAULT NULL,
  `ContractID` int(10) DEFAULT NULL,
  `RequestedAmount` decimal(10,2) DEFAULT '0.00',
  `RemainingBalance` decimal(10,2) DEFAULT '0.00',
  `PaymentID` int(10) NOT NULL,
  `AccountID` int(10) DEFAULT NULL,
  `PaymentDate` date DEFAULT NULL,
  `PaymentMethod` varchar(50) DEFAULT NULL,
  `ReferenceNumber` varchar(50) DEFAULT NULL,
  `PDCDateUsed` date DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_payment_organization` (`OrganizationID`),
  KEY `FK_payment_user` (`CreatedBy`),
  KEY `FK_payment_user_2` (`LastUpdBy`),
  KEY `FK_payment_financialinstitution` (`FinancialInstitutionID`),
  KEY `FK_payment_contract` (`ContractID`),
  KEY `FK_paymentfinancialinstitution_payment` (`PaymentID`),
  CONSTRAINT `FK_paymentfinancialinstitution_financialinstitution` FOREIGN KEY (`FinancialInstitutionID`) REFERENCES `financialinstitution` (`RowID`),
  CONSTRAINT `FK_paymentfinancialinstitution_payment` FOREIGN KEY (`PaymentID`) REFERENCES `payment` (`RowID`),
  CONSTRAINT `paymentfinancialinstitution_ibfk_3` FOREIGN KEY (`ContractID`) REFERENCES `contract` (`RowID`),
  CONSTRAINT `paymentfinancialinstitution_ibfk_5` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `paymentfinancialinstitution_ibfk_6` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `paymentfinancialinstitution_ibfk_7` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
