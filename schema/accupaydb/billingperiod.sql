/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `billingperiod` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `CreatedBy` int(10) NOT NULL,
  `LastUpdBy` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `BillPeriod` varchar(100) NOT NULL,
  `BillPeriodStartDate` date NOT NULL,
  `BillPeriodEndDate` date NOT NULL,
  `TotalElectricityDue` decimal(10,2) DEFAULT NULL,
  `TotalElectricityConsumption` decimal(10,2) DEFAULT NULL,
  `CalculatedElectricityRate` decimal(10,2) DEFAULT NULL,
  `TotalWaterDue` decimal(10,2) DEFAULT NULL,
  `TotalWaterConsumption` decimal(10,2) DEFAULT NULL,
  `CalculatedWaterRate` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 5` (`OrganizationID`,`BillPeriodStartDate`,`BillPeriodEndDate`),
  KEY `FK_BillingPeriod_user` (`CreatedBy`),
  KEY `FK_BillingPeriod_user_2` (`LastUpdBy`),
  CONSTRAINT `FK_BillingPeriod_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_BillingPeriod_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_BillingPeriod_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Defines the Billing Period schedule for invoices';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
