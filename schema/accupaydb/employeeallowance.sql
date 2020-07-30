/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `employeeallowance` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT current_timestamp(),
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `LastUpdBy` int(10) DEFAULT NULL,
  `EmployeeID` int(10) DEFAULT NULL,
  `ProductID` int(10) DEFAULT NULL,
  `EffectiveStartDate` date DEFAULT NULL,
  `AllowanceFrequency` varchar(50) DEFAULT NULL COMMENT 'Daily, Monthly, One Time',
  `EffectiveEndDate` date DEFAULT NULL,
  `TaxableFlag` char(50) DEFAULT NULL,
  `AllowanceAmount` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_BaseTables_organization` (`OrganizationID`),
  KEY `FK_BaseTables_user` (`CreatedBy`),
  KEY `FK_BaseTables_user_2` (`LastUpdBy`),
  KEY `FK_basetables_employee` (`EmployeeID`),
  KEY `FK_employeeallowance_product` (`ProductID`),
  CONSTRAINT `FK_employeeallowance_product` FOREIGN KEY (`ProductID`) REFERENCES `product` (`RowID`),
  CONSTRAINT `employeeallowance_ibfk_1` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `employeeallowance_ibfk_2` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `employeeallowance_ibfk_3` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `employeeallowance_ibfk_4` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
