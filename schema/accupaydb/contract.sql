/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `contract` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `Location_UnitCode` varchar(50) DEFAULT NULL COMMENT 'for Royal MP, defines which location and unit the lessee is interested in',
  `OrganizationID` int(11) DEFAULT NULL COMMENT 'Internal Company',
  `ContractNo` int(11) NOT NULL,
  `Area` decimal(10,2) DEFAULT NULL COMMENT 'area of the location/unit code in sq. m.',
  `LeaseTerm` varchar(50) DEFAULT NULL COMMENT 'lease term',
  `LeaseYears` int(11) DEFAULT NULL COMMENT 'lease in number of years',
  `LeaseMonths` int(11) DEFAULT NULL COMMENT 'lease in number of months',
  `QuoteID` int(11) DEFAULT NULL COMMENT 'before becoming a contract, a quote is created. links contract to quote',
  `CUSA` decimal(10,2) DEFAULT NULL COMMENT 'Common Use Service Area fee',
  `CUSAperSqM` decimal(10,2) DEFAULT NULL,
  `Aircon` varchar(50) DEFAULT NULL COMMENT 'LOV Type = "LEASE_RESP"- To be provided by Lessee or To be provided by Lessor',
  `Utilities` varchar(50) DEFAULT NULL COMMENT 'LOV Type = "LEASE_UTIL" - ',
  `DocumentaryStamp` varchar(50) DEFAULT NULL,
  `Escalated` char(1) DEFAULT NULL COMMENT 'Flag to determine if Contract has had renewal escalations already',
  `NextEscalationDate` date DEFAULT NULL COMMENT 'Date when the next Renewal Escalation will happen.',
  `Escalation` decimal(10,0) DEFAULT NULL COMMENT 'In percentage, escalation is the increase in rent (by %) annually',
  `Created` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `InterestPercent` decimal(10,2) DEFAULT NULL,
  `PenaltyPercent` decimal(10,2) DEFAULT NULL,
  `CreatedBy` int(11) DEFAULT NULL,
  `LastUpdBy` int(11) DEFAULT NULL,
  `AdvanceRent` int(11) DEFAULT NULL COMMENT '# of months in advance rent collected',
  `SecurityDeposit` int(11) DEFAULT NULL COMMENT 'security deposit (in months) to be collected',
  `ElectricalFeederline` varchar(200) DEFAULT NULL,
  `ConstructionBond` int(11) DEFAULT NULL COMMENT 'bond (in months) nneds to be collected for contruction. refundable deposit',
  `ConstructionPeriod` int(11) DEFAULT NULL COMMENT 'number of days allowed for contruction after commencement date',
  `LeasingManagerID` int(11) DEFAULT NULL COMMENT 'Leasing Manager from Organization. Linked to Contact ID.  To get this, link to OrganizationID, and from Organization, link to Contact',
  `Signage` varchar(100) DEFAULT NULL COMMENT 'Signage to be collected',
  `Parking` varchar(100) DEFAULT NULL COMMENT 'Parking fee to be collected',
  `RentalEffectiveDate` date DEFAULT NULL COMMENT 'Effective Date of Rent. when to start collecting rent.',
  `PermittedUse` varchar(100) DEFAULT NULL COMMENT 'Permitted Use (nature of Business)',
  `RentalRate` decimal(10,2) DEFAULT NULL COMMENT 'Gross Rental Rate per month',
  `RentalRatePerSqm` decimal(10,2) DEFAULT NULL COMMENT 'renta per sq. meter',
  `BalanceAmount` decimal(10,2) DEFAULT '0.00',
  `LesseeID` int(10) DEFAULT '0' COMMENT 'who is going to rent the unit.  Links to Account, and from Account, the Contact associated to the account',
  `VAT` decimal(10,2) DEFAULT '0.00',
  `CommencementDate` date DEFAULT NULL COMMENT 'when the contract is to begin',
  `TerminationDate` date DEFAULT NULL,
  `AccountID` int(10) DEFAULT NULL COMMENT 'Tenant associated to this contract.',
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 9` (`OrganizationID`,`ContractNo`),
  KEY `FK_contract_user` (`CreatedBy`),
  KEY `FK_contract_user_2` (`LastUpdBy`),
  KEY `FK_contract_contact` (`LeasingManagerID`),
  KEY `FK_contract_contact_2` (`LesseeID`),
  KEY `FK_contract_leasequote` (`QuoteID`),
  KEY `FK_contract_account` (`AccountID`),
  CONSTRAINT `FK_contract_account` FOREIGN KEY (`AccountID`) REFERENCES `account` (`RowID`),
  CONSTRAINT `FK_contract_contact` FOREIGN KEY (`LeasingManagerID`) REFERENCES `contact` (`RowID`),
  CONSTRAINT `FK_contract_contact_2` FOREIGN KEY (`LesseeID`) REFERENCES `contact` (`RowID`),
  CONSTRAINT `FK_contract_leasequote` FOREIGN KEY (`QuoteID`) REFERENCES `quote` (`RowID`),
  CONSTRAINT `FK_contract_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_contract_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_contract_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Contract table';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
