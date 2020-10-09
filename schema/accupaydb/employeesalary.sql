/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `employeesalary` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `EmployeeID` int(10) NOT NULL,
  `Created` datetime NOT NULL DEFAULT current_timestamp(),
  `CreatedBy` int(10) NOT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `LastUpdBy` int(10) DEFAULT NULL,
  `OrganizationID` int(10) NOT NULL,
  `PhilHealthDeduction` decimal(15,4) DEFAULT 0.0000,
  `HDMFAmount` decimal(11,2) DEFAULT NULL,
  `TrueSalary` decimal(11,2) DEFAULT 0.00,
  `BasicPay` decimal(11,2) NOT NULL DEFAULT 0.00,
  `Salary` decimal(11,2) DEFAULT NULL,
  `UndeclaredSalary` decimal(11,2) DEFAULT 0.00,
  `BasicDailyPay` decimal(11,2) DEFAULT NULL,
  `BasicHourlyPay` decimal(11,2) DEFAULT NULL,
  `NoofDependents` int(11) DEFAULT NULL,
  `MaritalStatus` varchar(50) DEFAULT NULL,
  `PositionID` int(11) DEFAULT NULL,
  `EffectiveDateFrom` date DEFAULT NULL,
  `OverrideDiscardSSSContrib` tinyint(3) DEFAULT 0,
  `OverrideDiscardPhilHealthContrib` tinyint(3) DEFAULT 0,
  `DoPaySSSContribution` tinyint(1) NOT NULL DEFAULT 1,
  `AutoComputePhilHealthContribution` tinyint(1) NOT NULL DEFAULT 1,
  `AutoComputeHDMFContribution` tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `EmployeeID` (`EmployeeID`,`OrganizationID`,`EffectiveDateFrom`),
  KEY `FK_EmployeeSalary_employee` (`EmployeeID`),
  KEY `FK_EmployeeSalary_user` (`CreatedBy`),
  KEY `FK_EmployeeSalary_user_2` (`LastUpdBy`),
  KEY `FK_EmployeeSalary_organization` (`OrganizationID`),
  CONSTRAINT `FK_EmployeeSalary_employee` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `FK_EmployeeSalary_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_EmployeeSalary_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_EmployeeSalary_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
