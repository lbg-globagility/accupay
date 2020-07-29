/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `employeedisciplinaryaction` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `Created` datetime NOT NULL DEFAULT current_timestamp(),
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `LastUpdBy` int(10) DEFAULT NULL,
  `EmployeeID` int(10) NOT NULL,
  `DateFrom` date NOT NULL,
  `DateTo` date NOT NULL,
  `FindingID` int(10) DEFAULT NULL,
  `FindingDescription` varchar(2000) DEFAULT NULL,
  `Action` varchar(100) DEFAULT NULL,
  `Penalty` varchar(100) DEFAULT NULL,
  `Comments` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_BaseTables_organization` (`OrganizationID`),
  KEY `FK_BaseTables_user` (`CreatedBy`),
  KEY `FK_BaseTables_user_2` (`LastUpdBy`),
  KEY `FK_employeemedicalrecord_employee` (`EmployeeID`),
  KEY `FK_employeedisciplinaryaction_product` (`FindingID`),
  CONSTRAINT `FK_employeedisciplinaryaction_product` FOREIGN KEY (`FindingID`) REFERENCES `product` (`RowID`),
  CONSTRAINT `employeedisciplinaryaction_ibfk_1` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `employeedisciplinaryaction_ibfk_3` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `employeedisciplinaryaction_ibfk_4` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `employeedisciplinaryaction_ibfk_5` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
