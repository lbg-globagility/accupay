/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `leaveledger` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) DEFAULT NULL,
  `Created` timestamp NULL DEFAULT current_timestamp(),
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` timestamp NULL DEFAULT NULL ON UPDATE current_timestamp(),
  `LastUpdBy` int(10) DEFAULT NULL,
  `EmployeeID` int(10) DEFAULT NULL,
  `ProductID` int(10) DEFAULT NULL,
  `LastTransactionID` int(10) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Idx_ledger_per_employee` (`EmployeeID`,`ProductID`),
  KEY `FK_leaveledger_product` (`ProductID`),
  KEY `FK_leaveledger_organization` (`OrganizationID`),
  KEY `FK_leaveledger_user` (`CreatedBy`),
  KEY `FK_leaveledger_user_2` (`LastUpdBy`),
  KEY `FK_leaveledger_leavetransaction` (`LastTransactionID`),
  CONSTRAINT `FK_leaveledger_employee` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`),
  CONSTRAINT `FK_leaveledger_leavetransaction` FOREIGN KEY (`LastTransactionID`) REFERENCES `leavetransaction` (`RowID`) ON DELETE SET NULL,
  CONSTRAINT `FK_leaveledger_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_leaveledger_product` FOREIGN KEY (`ProductID`) REFERENCES `product` (`RowID`),
  CONSTRAINT `FK_leaveledger_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_leaveledger_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
