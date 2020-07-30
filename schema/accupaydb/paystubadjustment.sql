/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `paystubadjustment` (
  `RowID` bigint(20) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `Created` datetime NOT NULL DEFAULT current_timestamp(),
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `LastUpdBy` int(10) DEFAULT NULL,
  `PayStubID` int(10) DEFAULT NULL,
  `ProductID` int(10) NOT NULL,
  `PayAmount` decimal(10,2) DEFAULT NULL,
  `Comment` varchar(200) DEFAULT NULL,
  `IsActual` tinyint(1) DEFAULT 0,
  PRIMARY KEY (`RowID`),
  KEY `FK_BaseTables_organization` (`OrganizationID`),
  KEY `FK_BaseTables_user` (`CreatedBy`),
  KEY `FK_BaseTables_user_2` (`LastUpdBy`),
  KEY `FK_paystubitem_product` (`ProductID`),
  KEY `FK_paystubadjustment_paystub` (`PayStubID`),
  CONSTRAINT `FK_paystubadjustment_paystub` FOREIGN KEY (`PayStubID`) REFERENCES `paystub` (`RowID`),
  CONSTRAINT `FK_paystubadjustment_product` FOREIGN KEY (`ProductID`) REFERENCES `product` (`RowID`),
  CONSTRAINT `paystubadjustment_ibfk_3` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `paystubadjustment_ibfk_4` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `paystubadjustment_ibfk_5` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
