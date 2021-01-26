/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `paystubitembonus` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `Created` datetime NOT NULL DEFAULT current_timestamp(),
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `LastUpdBy` int(10) DEFAULT NULL,
  `PayStubBonusID` int(10) DEFAULT NULL,
  `ProductID` int(10) DEFAULT NULL,
  `PayAmount` decimal(11,2) DEFAULT 0.00,
  `Undeclared` char(1) DEFAULT '0',
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 5` (`PayStubBonusID`,`OrganizationID`,`ProductID`,`Undeclared`),
  KEY `FK_BaseTables_organization` (`OrganizationID`),
  KEY `FK_paystubitem_product` (`ProductID`),
  KEY `FK_paystub_RowID` (`PayStubBonusID`),
  KEY `KEY_Undeclared` (`Undeclared`),
  CONSTRAINT `paystubitembonus_ibfk_1` FOREIGN KEY (`PayStubBonusID`) REFERENCES `paystubbonus` (`RowID`),
  CONSTRAINT `paystubitembonus_ibfk_2` FOREIGN KEY (`ProductID`) REFERENCES `product` (`RowID`),
  CONSTRAINT `paystubitembonus_ibfk_3` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
