/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `allowanceitem` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT current_timestamp(),
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `LastUpdBy` int(10) DEFAULT NULL,
  `PaystubID` int(10) NOT NULL,
  `AllowanceID` int(10) DEFAULT NULL,
  `PayPeriodID` int(10) DEFAULT NULL,
  `Amount` decimal(15,4) DEFAULT 0.0000,
  PRIMARY KEY (`RowID`),
  KEY `FK_BaseTables_organization` (`OrganizationID`),
  KEY `FK_BaseTables_user` (`CreatedBy`),
  KEY `FK_BaseTables_user_2` (`LastUpdBy`),
  KEY `FK_allowanceitem_paystub` (`PaystubID`),
  KEY `FK_allowanceitem_payperiod` (`PayPeriodID`),
  KEY `FK_allowanceitem_product` (`AllowanceID`),
  CONSTRAINT `FK_allowanceitem_payperiod` FOREIGN KEY (`PayPeriodID`) REFERENCES `payperiod` (`RowID`),
  CONSTRAINT `FK_allowanceitem_paystub` FOREIGN KEY (`PaystubID`) REFERENCES `paystub` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `FK_allowanceitem_product_AllowanceID` FOREIGN KEY (`AllowanceID`) REFERENCES `employeeallowance` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `allowanceitem_ibfk_1` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `allowanceitem_ibfk_2` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `allowanceitem_ibfk_3` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
