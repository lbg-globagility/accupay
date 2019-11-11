/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `accountbillingperiod` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `CreatedBy` int(10) NOT NULL,
  `LastUpdBy` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `AccountID` int(10) DEFAULT NULL,
  `BillingPeriodID` int(10) DEFAULT NULL,
  `ContractID` int(10) DEFAULT NULL,
  `TotalUtilityReading` int(10) DEFAULT '0' COMMENT 'this is the sum of utility reading for that particular utility type',
  `TotalUtilityUsage` int(10) DEFAULT '0' COMMENT 'this is the sum of utility usage (total utility reading * multiplier)',
  `GrandTotalUtilityAmount` decimal(10,2) DEFAULT '0.00' COMMENT 'calculated value based on Billing Period values inclusive of VAT and other fees',
  `TotalUtilityAmount` decimal(10,2) DEFAULT '0.00' COMMENT 'calculated value based on Billing Period values exclusive of VAT and other fees',
  `TotalVAT` decimal(10,2) DEFAULT '0.00' COMMENT 'calculated VAT value based on Billing Period',
  `UtilityType` varchar(50) NOT NULL COMMENT 'Water, Electricity',
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 2` (`AccountID`,`OrganizationID`,`UtilityType`,`ContractID`,`BillingPeriodID`),
  KEY `FK_accountbillingperiod_organization` (`OrganizationID`),
  KEY `FK_accountbillingperiod_user` (`LastUpdBy`),
  KEY `FK_accountbillingperiod_user_2` (`CreatedBy`),
  KEY `FK_accountbillingperiod_billingperiod` (`BillingPeriodID`),
  KEY `FK_accountbillingperiod_contract` (`ContractID`),
  CONSTRAINT `FK_accountbillingperiod_account` FOREIGN KEY (`AccountID`) REFERENCES `account` (`RowID`),
  CONSTRAINT `FK_accountbillingperiod_billingperiod` FOREIGN KEY (`BillingPeriodID`) REFERENCES `billingperiod` (`RowID`),
  CONSTRAINT `FK_accountbillingperiod_contract` FOREIGN KEY (`ContractID`) REFERENCES `contract` (`RowID`),
  CONSTRAINT `FK_accountbillingperiod_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_accountbillingperiod_user` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_accountbillingperiod_user_2` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='defines the Account''s billing period.   1 account/tenant can have more than 1 billing period, and 1 billing period can have more than 1 tenant.';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
