/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `employeeloanschedule` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastUpdBy` int(10) DEFAULT NULL,
  `EmployeeID` int(10) DEFAULT NULL,
  `LoanNumber` varchar(50) DEFAULT NULL,
  `DedEffectiveDateFrom` date DEFAULT NULL,
  `DedEffectiveDateTo` date DEFAULT NULL,
  `TotalLoanAmount` decimal(20,6) DEFAULT NULL,
  `DeductionSchedule` varchar(50) DEFAULT NULL COMMENT 'per payperiod',
  `TotalBalanceLeft` decimal(20,6) DEFAULT NULL,
  `DeductionAmount` decimal(10,2) DEFAULT NULL,
  `Status` varchar(50) DEFAULT NULL COMMENT 'Fully Paid,In Progress,On hold,Cancelled',
  `LoanTypeID` int(11) DEFAULT NULL,
  `DeductionPercentage` decimal(10,2) DEFAULT NULL,
  `NoOfPayPeriod` decimal(10,2) DEFAULT NULL,
  `LoanPayPeriodLeft` decimal(10,2) DEFAULT NULL,
  `Comments` varchar(2000) DEFAULT NULL,
  `BonusID` int(11) DEFAULT NULL,
  `LoanName` varchar(50) DEFAULT '',
  PRIMARY KEY (`RowID`),
  KEY `FK_BaseTables_organization` (`OrganizationID`),
  KEY `FK_BaseTables_user` (`CreatedBy`),
  KEY `FK_BaseTables_user_2` (`LastUpdBy`),
  KEY `FK_employeeloan_employee` (`EmployeeID`),
  KEY `FK_employeeloanschedule_product` (`LoanTypeID`),
  KEY `FK_employee_bonus` (`BonusID`),
  CONSTRAINT `FK_employee_bonus` FOREIGN KEY (`BonusID`) REFERENCES `employeebonus` (`RowID`),
  CONSTRAINT `FK_employeeloan_employee` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`),
  CONSTRAINT `FK_employeeloanschedule_product` FOREIGN KEY (`LoanTypeID`) REFERENCES `product` (`RowID`),
  CONSTRAINT `employeeloanschedule_ibfk_1` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `employeeloanschedule_ibfk_2` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `employeeloanschedule_ibfk_3` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB AUTO_INCREMENT=561 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
