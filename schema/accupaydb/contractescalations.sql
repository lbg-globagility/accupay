/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `contractescalations` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `CreatedBy` int(10) NOT NULL,
  `LastUpdBy` int(10) NOT NULL,
  `ContractID` int(10) NOT NULL COMMENT 'Contract ID',
  `EscalationEffectiveDateFrom` date NOT NULL COMMENT 'when the escalation will take effect, usually the same date as the termination date',
  `EscalationEffectiveDateTo` date NOT NULL,
  `EscalationAmount` decimal(10,2) NOT NULL COMMENT 'basic rent*escalationPercent',
  `EscalationPercent` decimal(10,2) NOT NULL COMMENT 'from contract''s renewal escalation',
  `TotalBasicRent` decimal(10,2) NOT NULL COMMENT 'Escalated Basic Rent (from Contract table) + escalationamount',
  PRIMARY KEY (`RowID`),
  KEY `FK_ContractEscalations_organization` (`OrganizationID`),
  KEY `FK_ContractEscalations_user` (`CreatedBy`),
  KEY `FK_ContractEscalations_user_2` (`LastUpdBy`),
  KEY `FK_ContractEscalations_contract` (`ContractID`),
  CONSTRAINT `FK_ContractEscalations_contract` FOREIGN KEY (`ContractID`) REFERENCES `contract` (`RowID`),
  CONSTRAINT `FK_ContractEscalations_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_ContractEscalations_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_ContractEscalations_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Contract can have more than 1 renewal escalations. Table to store Renewal Escalations for contracts. If contract is still active, and the contract termination date is less than today''s date, then auto create a renewal escalation record.  Everytime a record gets generated, the Contract''s table EscalatedAmount gets updated with the TotalBasicRent; Contract''s EscalationEffectiveDate will be changed to the EscalationDate; New escalation Date';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
