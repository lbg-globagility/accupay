/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `contractrenewescalations` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `ContractID` int(10) NOT NULL,
  `LastUpdBy` int(10) DEFAULT NULL,
  `CreatedBy` int(10) NOT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `EscalationPercent` decimal(10,2) NOT NULL COMMENT 'percent escalation (5% on the 2nd year onwards). The value of this is 5',
  `EscalationYear` int(10) NOT NULL COMMENT 'year escalation (5% on the 2nd year onwards). The value of this is 2',
  `EscalationMonth` int(10) NOT NULL COMMENT 'month escalation',
  PRIMARY KEY (`RowID`),
  KEY `FK_contractrenewescalations_organization` (`OrganizationID`),
  KEY `FK_contractrenewescalations_contract` (`ContractID`),
  KEY `FK_contractrenewescalations_user` (`LastUpdBy`),
  KEY `FK_contractrenewescalations_user_2` (`CreatedBy`),
  CONSTRAINT `FK_contractrenewescalations_contract` FOREIGN KEY (`ContractID`) REFERENCES `contract` (`RowID`),
  CONSTRAINT `FK_contractrenewescalations_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_contractrenewescalations_user` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_contractrenewescalations_user_2` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
