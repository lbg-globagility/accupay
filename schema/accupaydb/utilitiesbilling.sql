/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `utilitiesbilling` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `CreatedBy` int(10) NOT NULL,
  `LastUpdBy` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `AccountBillingPeriodID` int(10) NOT NULL,
  `PreviousUtilityReading` int(10) DEFAULT NULL,
  `MeterNo` varchar(50) DEFAULT NULL,
  `PresentUtilityReading` int(10) DEFAULT NULL,
  `UtilityReadingDifference` int(10) DEFAULT NULL,
  `UtilityAmount` decimal(10,2) DEFAULT NULL,
  `UtilityType` varchar(50) NOT NULL COMMENT 'LOV=UTILITY_TYPE, Water, Electricity',
  `UtilityBillStartDate` date DEFAULT NULL,
  `UtilityBillEndDate` date DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 2` (`UtilityType`,`MeterNo`,`AccountBillingPeriodID`),
  KEY `FK_utilitiesbilling_organization` (`OrganizationID`),
  KEY `FK_utilitiesbilling_user` (`CreatedBy`),
  KEY `FK_utilitiesbilling_user_2` (`LastUpdBy`),
  KEY `FK_utilitiesbilling_accountbillingperiod` (`AccountBillingPeriodID`),
  CONSTRAINT `FK_utilitiesbilling_accountbillingperiod` FOREIGN KEY (`AccountBillingPeriodID`) REFERENCES `accountbillingperiod` (`RowID`),
  CONSTRAINT `FK_utilitiesbilling_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_utilitiesbilling_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_utilitiesbilling_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='defines the Account''s billing period.   1 account/tenant can have more than 1 billing period, and 1 billing period can have more than 1 tenant.';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
