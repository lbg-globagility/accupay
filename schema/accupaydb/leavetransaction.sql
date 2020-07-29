/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `leavetransaction` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) DEFAULT NULL,
  `Created` timestamp NULL DEFAULT current_timestamp(),
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` timestamp NULL DEFAULT NULL ON UPDATE current_timestamp(),
  `LastUpdBy` int(10) DEFAULT NULL,
  `EmployeeID` int(10) DEFAULT NULL,
  `ReferenceID` int(10) DEFAULT NULL,
  `LeaveLedgerID` int(10) DEFAULT NULL,
  `PayPeriodID` int(10) DEFAULT NULL,
  `PaystubID` int(10) DEFAULT NULL,
  `TransactionDate` date DEFAULT NULL,
  `Description` varchar(50) DEFAULT NULL,
  `Type` varchar(50) DEFAULT NULL,
  `Balance` decimal(10,2) DEFAULT 0.00,
  `Amount` decimal(10,2) DEFAULT 0.00,
  `Comments` varchar(255) DEFAULT '',
  PRIMARY KEY (`RowID`),
  KEY `FK_leavetransaction_organization` (`OrganizationID`),
  KEY `FK_leavetransaction_user` (`CreatedBy`),
  KEY `FK_leavetransaction_user_2` (`LastUpdBy`),
  KEY `FK_leavetransaction_employee` (`EmployeeID`),
  KEY `FK_leavetransaction_leaveledger` (`LeaveLedgerID`),
  KEY `FK_leavetransaction_employeeleave` (`ReferenceID`),
  KEY `FK_leavetransaction_payperiod` (`PayPeriodID`),
  KEY `TransactionDate` (`TransactionDate`),
  KEY `FK_leavetransaction_paystub_PaystubID` (`PaystubID`),
  CONSTRAINT `FK_leavetransaction_employee_EmployeeID` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`),
  CONSTRAINT `FK_leavetransaction_employeeleave_ReferenceID` FOREIGN KEY (`ReferenceID`) REFERENCES `employeeleave` (`RowID`) ON DELETE SET NULL,
  CONSTRAINT `FK_leavetransaction_leaveledger_LeaveLedgerID` FOREIGN KEY (`LeaveLedgerID`) REFERENCES `leaveledger` (`RowID`),
  CONSTRAINT `FK_leavetransaction_organization_OrganizationID` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_leavetransaction_payperiod_PayPeriodID` FOREIGN KEY (`PayPeriodID`) REFERENCES `payperiod` (`RowID`),
  CONSTRAINT `FK_leavetransaction_paystub_PaystubID` FOREIGN KEY (`PaystubID`) REFERENCES `paystub` (`RowID`),
  CONSTRAINT `FK_leavetransaction_user_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_leavetransaction_user_LastUpdBy` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
