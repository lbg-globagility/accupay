/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `scheduledloansperpayperiod` (
  `RowID` int(11) NOT NULL AUTO_INCREMENT,
  `Created` timestamp NOT NULL DEFAULT current_timestamp(),
  `CreatedBy` int(11) DEFAULT NULL,
  `LastUpd` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `LastUpdBy` int(11) DEFAULT NULL,
  `OrganizationID` int(11) DEFAULT NULL,
  `PayPeriodID` int(11) DEFAULT NULL,
  `EmployeeID` int(11) DEFAULT NULL,
  `EmployeeLoanRecordID` int(11) DEFAULT NULL,
  `PaystubID` int(11) DEFAULT NULL,
  `LoanPayPeriodLeft` int(11) DEFAULT 0,
  `TotalBalanceLeft` decimal(20,6) DEFAULT 0.000000,
  `DeductionAmount` decimal(20,6) DEFAULT 0.000000,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `AK_scheduledloansperpayperiod_OrganizationID_PyPrdD_EmplyLnRcrdD` (`OrganizationID`,`PayPeriodID`,`EmployeeLoanRecordID`),
  UNIQUE KEY `AK_scheduledloansperpayperiod_EmployeeLoanRecordID_PaystubID` (`EmployeeLoanRecordID`,`PaystubID`),
  KEY `FK_scheduledloansperpayperiod_organization` (`OrganizationID`),
  KEY `FK_scheduledloansperpayperiod_payperiod` (`PayPeriodID`),
  KEY `FK_scheduledloansperpayperiod_employee` (`EmployeeID`),
  KEY `FK_scheduledloansperpayperiod_employeeloanschedule` (`EmployeeLoanRecordID`),
  KEY `FK_scheduledloansperpayperiod_paystub` (`PaystubID`),
  CONSTRAINT `FK_scheduledloansperpayperiod_employee` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `FK_scheduledloansperpayperiod_employeeloanschedule` FOREIGN KEY (`EmployeeLoanRecordID`) REFERENCES `employeeloanschedule` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `FK_scheduledloansperpayperiod_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_scheduledloansperpayperiod_payperiod` FOREIGN KEY (`PayPeriodID`) REFERENCES `payperiod` (`RowID`),
  CONSTRAINT `FK_scheduledloansperpayperiod_paystub` FOREIGN KEY (`PaystubID`) REFERENCES `paystub` (`RowID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
