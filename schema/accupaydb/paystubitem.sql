/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `paystubitem` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastUpdBy` int(10) DEFAULT NULL,
  `PayStubID` int(10) DEFAULT NULL,
  `ProductID` int(10) DEFAULT NULL,
  `PayAmount` decimal(11,2) DEFAULT NULL,
  `Undeclared` char(1) DEFAULT '0',
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 5` (`PayStubID`,`OrganizationID`,`ProductID`,`Undeclared`),
  KEY `FK_BaseTables_organization` (`OrganizationID`),
  KEY `FK_paystubitem_product` (`ProductID`),
  KEY `FK_paystub_RowID` (`PayStubID`),
  KEY `KEY_Undeclared` (`Undeclared`),
  CONSTRAINT `FK_paystubitem_paystub` FOREIGN KEY (`PayStubID`) REFERENCES `paystub` (`RowID`),
  CONSTRAINT `FK_paystubitem_product` FOREIGN KEY (`ProductID`) REFERENCES `product` (`RowID`),
  CONSTRAINT `paystubitem_ibfk_1` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`)
) ENGINE=InnoDB AUTO_INCREMENT=54147 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
