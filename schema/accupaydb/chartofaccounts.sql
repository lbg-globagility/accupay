/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `chartofaccounts` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` int(10) DEFAULT NULL,
  `AccountingPeriodID` int(10) NOT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastUpdBy` int(10) DEFAULT NULL,
  `CategoryID` int(10) NOT NULL,
  `DebitAmount` decimal(10,2) unsigned DEFAULT NULL,
  `CreditAmount` decimal(10,2) DEFAULT NULL,
  `Name` varchar(50) NOT NULL,
  `AllCOAID` int(10) NOT NULL,
  `PaymentAccount` char(1) DEFAULT NULL,
  `SystemAccountFlg` char(1) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 5` (`Name`,`AccountingPeriodID`,`OrganizationID`),
  KEY `FK_BaseTables_organization` (`OrganizationID`),
  KEY `FK_BaseTables_user` (`CreatedBy`),
  KEY `FK_BaseTables_user_2` (`LastUpdBy`),
  KEY `FK_chartofaccounts_accountingperiod` (`AccountingPeriodID`),
  KEY `FK_chartofaccounts_allchartofaccounts` (`AllCOAID`),
  CONSTRAINT `FK_chartofaccounts_accountingperiod` FOREIGN KEY (`AccountingPeriodID`) REFERENCES `accountingperiod` (`RowID`),
  CONSTRAINT `FK_chartofaccounts_allchartofaccounts` FOREIGN KEY (`AllCOAID`) REFERENCES `allchartofaccounts` (`RowID`),
  CONSTRAINT `chartofaccounts_ibfk_1` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `chartofaccounts_ibfk_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `chartofaccounts_ibfk_3` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
