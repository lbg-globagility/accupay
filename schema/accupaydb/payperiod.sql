/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `payperiod` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) DEFAULT NULL,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastUpdBy` int(10) DEFAULT NULL,
  `PayFromDate` date DEFAULT NULL,
  `PayToDate` date DEFAULT NULL,
  `TotalGrossSalary` decimal(10,2) DEFAULT NULL,
  `TotalNetSalary` decimal(10,2) DEFAULT NULL,
  `TotalEmpSSS` decimal(10,2) DEFAULT NULL,
  `TotalEmpWithholdingTax` decimal(10,2) DEFAULT NULL,
  `TotalCompSSS` decimal(10,2) DEFAULT NULL,
  `TotalEmpPhilhealth` decimal(10,2) DEFAULT NULL,
  `TotalCompPhilhealth` decimal(10,2) DEFAULT NULL,
  `TotalEmpHDMF` decimal(10,2) DEFAULT NULL,
  `TotalCompHDMF` decimal(10,2) DEFAULT NULL,
  `Month` int(11) DEFAULT NULL,
  `Year` char(4) DEFAULT NULL,
  `Half` char(1) DEFAULT NULL,
  `SSSContribSched` char(1) DEFAULT '0' COMMENT 'This flag only applies to ''Weekly'' employees,a sched to when the govt contrib be deducted',
  `PhHContribSched` char(1) DEFAULT '0' COMMENT 'This flag only applies to ''Weekly'' employees,a sched to when the govt contrib be deducted',
  `HDMFContribSched` char(1) DEFAULT '0' COMMENT 'This flag only applies to ''Weekly'' employees,a sched to when the govt contrib be deducted',
  `OrdinalValue` int(11) DEFAULT '0',
  `MinWageValue` decimal(10,2) DEFAULT '481.00',
  `WeekOridnalValue` tinyint(1) DEFAULT '0',
  `IsLastFridayOfMonthFallsHere` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 5` (`OrganizationID`,`PayFromDate`,`PayToDate`),
  KEY `FK_BaseTables_organization` (`OrganizationID`),
  KEY `FK_BaseTables_user` (`CreatedBy`),
  KEY `FK_BaseTables_user_2` (`LastUpdBy`),
  CONSTRAINT `payperiod_ibfk_1` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `payperiod_ibfk_2` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `payperiod_ibfk_3` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB AUTO_INCREMENT=4857 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
