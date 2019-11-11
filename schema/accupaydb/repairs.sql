/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `repairs` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `CreatedBy` int(10) NOT NULL,
  `LastUpdBy` int(10) NOT NULL,
  `OrganizationID` int(10) NOT NULL,
  `ReferenceNumber` int(10) NOT NULL COMMENT 'either the Pullout Control No or the Packing List number.  If Pullout, that means its a customer returned repair, if PL, that means its a Shipment damage',
  `Quantity` int(10) NOT NULL,
  `Type` varchar(50) NOT NULL COMMENT 'Customer Issue, Shipment Issue',
  `Source` varchar(50) NOT NULL COMMENT 'Pullout or Packing List',
  `RepairNo` int(11) NOT NULL,
  `Status` varchar(50) NOT NULL COMMENT 'Repair Status=Open, Repaired, Non-Repairable',
  `Cause` varchar(2000) DEFAULT NULL COMMENT 'Root cause of the repair issue/item',
  `Resolution` varchar(2000) DEFAULT NULL COMMENT 'How it was resolved',
  `TotalAmount` decimal(10,2) DEFAULT NULL,
  `Amount` decimal(10,2) NOT NULL,
  `ProductID` int(10) NOT NULL,
  `InventoryLocationID` int(10) NOT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_Returns_user` (`CreatedBy`),
  KEY `FK_Returns_user_2` (`LastUpdBy`),
  KEY `FK_Returns_organization` (`OrganizationID`),
  KEY `FK_Returns_product` (`ProductID`),
  KEY `FK_repairs_inventorylocation` (`InventoryLocationID`),
  CONSTRAINT `FK_repairs_inventorylocation` FOREIGN KEY (`InventoryLocationID`) REFERENCES `inventorylocation` (`RowID`),
  CONSTRAINT `FK_repairs_product` FOREIGN KEY (`ProductID`) REFERENCES `product` (`RowID`),
  CONSTRAINT `repairs_ibfk_3` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `repairs_ibfk_5` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `repairs_ibfk_6` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Table to store returns refunds/exchanges';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
