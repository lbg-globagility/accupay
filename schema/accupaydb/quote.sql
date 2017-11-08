/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `quote` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `Location_UnitCode` varchar(50) DEFAULT NULL COMMENT 'see Contract table for definition.',
  `OrganizationID` int(11) DEFAULT NULL,
  `Area` decimal(10,2) DEFAULT NULL,
  `LeaseTerm` varchar(50) DEFAULT NULL,
  `LeaseYears` int(11) NOT NULL,
  `LeaseMonths` int(11) DEFAULT NULL,
  `Status` varchar(50) DEFAULT NULL COMMENT 'ListOfVal Type = "QUOTE_TYPE"',
  `CUSA` decimal(10,2) DEFAULT NULL,
  `CUSAperSqM` decimal(10,2) DEFAULT NULL,
  `Aircon` varchar(50) DEFAULT NULL,
  `Utilities` varchar(50) DEFAULT NULL,
  `Escalation` decimal(10,0) DEFAULT NULL,
  `Created` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `CreatedBy` int(11) DEFAULT NULL,
  `LastUpdBy` int(11) DEFAULT NULL,
  `AdvanceRent` int(11) DEFAULT NULL,
  `SecurityDeposit` int(11) DEFAULT NULL,
  `ElectricalFeederline` varchar(100) DEFAULT NULL,
  `WaterDeposit` varchar(100) DEFAULT NULL,
  `ConstructionBond` int(11) DEFAULT NULL,
  `ConstructionPeriod` int(11) DEFAULT NULL,
  `LeasingManagerID` int(11) DEFAULT NULL,
  `PermittedUse` varchar(50) DEFAULT NULL,
  `RelatedQuoteID` int(11) DEFAULT NULL,
  `RentalRate` decimal(10,2) DEFAULT NULL,
  `RentalRatePerSqm` decimal(10,2) DEFAULT NULL,
  `LesseeID` int(10) NOT NULL DEFAULT '0',
  `VAT` decimal(10,0) NOT NULL DEFAULT '0',
  `Parking` varchar(100) DEFAULT NULL,
  `Signage` varchar(100) DEFAULT NULL,
  `AccountID` int(10) DEFAULT NULL,
  `QuoteNo` int(10) NOT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 9` (`OrganizationID`,`QuoteNo`),
  KEY `FK_leasequote_contact` (`LesseeID`),
  KEY `FK_leasequote_contact_2` (`LeasingManagerID`),
  KEY `FK_leasequote_user` (`CreatedBy`),
  KEY `FK_leasequote_user_2` (`LastUpdBy`),
  KEY `FK_quote_account` (`AccountID`),
  KEY `FK_quote_quote` (`RelatedQuoteID`),
  CONSTRAINT `FK__organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_leasequote_contact` FOREIGN KEY (`LesseeID`) REFERENCES `contact` (`RowID`),
  CONSTRAINT `FK_leasequote_contact_2` FOREIGN KEY (`LeasingManagerID`) REFERENCES `contact` (`RowID`),
  CONSTRAINT `FK_leasequote_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_leasequote_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_quote_account` FOREIGN KEY (`AccountID`) REFERENCES `account` (`RowID`),
  CONSTRAINT `FK_quote_quote` FOREIGN KEY (`RelatedQuoteID`) REFERENCES `quote` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Quotes before becoming an Order/Contract. see Contract for definition.';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
