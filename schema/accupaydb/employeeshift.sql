/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `employeeshift` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastUpdBy` int(10) DEFAULT NULL,
  `EmployeeID` int(10) DEFAULT NULL,
  `ShiftID` int(10) DEFAULT NULL,
  `EffectiveFrom` date DEFAULT NULL,
  `EffectiveTo` date DEFAULT NULL,
  `NightShift` char(1) DEFAULT NULL,
  `RestDay` char(1) DEFAULT NULL,
  `IsEncodedByDay` char(1) DEFAULT '0',
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 6` (`EmployeeID`,`EffectiveFrom`,`EffectiveTo`,`OrganizationID`),
  KEY `FK_BaseTables_organization` (`OrganizationID`),
  KEY `FK_BaseTables_user` (`CreatedBy`),
  KEY `FK_BaseTables_user_2` (`LastUpdBy`),
  KEY `FK_employeeshift_employee` (`EmployeeID`),
  KEY `FK_employeeshift_shift` (`ShiftID`),
  CONSTRAINT `FK_employeeshift_employee` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`),
  CONSTRAINT `FK_employeeshift_shift` FOREIGN KEY (`ShiftID`) REFERENCES `shift` (`RowID`),
  CONSTRAINT `employeeshift_ibfk_1` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `employeeshift_ibfk_2` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `employeeshift_ibfk_3` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB AUTO_INCREMENT=6245 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
