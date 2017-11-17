/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `scheduledloansperpayperiod` (
  `RowID` int(11) NOT NULL AUTO_INCREMENT,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` int(11) DEFAULT NULL,
  `LastUpd` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `LastUpdBy` int(11) DEFAULT NULL,
  `OrganizationID` int(11) DEFAULT NULL,
  `PayPeriodID` int(11) DEFAULT NULL,
  `EmployeeID` int(11) DEFAULT NULL,
  `EmployeeLoanRecordID` int(11) DEFAULT NULL,
  `PayStubID` int(11) DEFAULT NULL,
  `LoanPayPeriodLeft` int(11) DEFAULT '0',
  `TotalBalanceLeft` decimal(20,6) DEFAULT '0.000000',
  `DeductionAmount` decimal(20,6) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `UniqueKeys` (`OrganizationID`,`PayPeriodID`,`EmployeeLoanRecordID`),
  KEY `FK_scheduledloansperpayperiod_organization` (`OrganizationID`),
  KEY `FK_scheduledloansperpayperiod_payperiod` (`PayPeriodID`),
  KEY `FK_scheduledloansperpayperiod_employee` (`EmployeeID`),
  KEY `FK_scheduledloansperpayperiod_employeeloanschedule` (`EmployeeLoanRecordID`),
  CONSTRAINT `FK_scheduledloansperpayperiod_employee` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`),
  CONSTRAINT `FK_scheduledloansperpayperiod_employeeloanschedule` FOREIGN KEY (`EmployeeLoanRecordID`) REFERENCES `employeeloanschedule` (`RowID`),
  CONSTRAINT `FK_scheduledloansperpayperiod_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_scheduledloansperpayperiod_payperiod` FOREIGN KEY (`PayPeriodID`) REFERENCES `payperiod` (`RowID`)
) ENGINE=InnoDB AUTO_INCREMENT=1295 DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
