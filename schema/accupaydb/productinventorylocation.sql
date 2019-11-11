/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `productinventorylocation` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `CreatedBy` int(11) NOT NULL,
  `OrganizationID` int(11) NOT NULL COMMENT 'Internal Company',
  `LastUpdBy` int(11) NOT NULL,
  `ProductID` int(10) NOT NULL DEFAULT '0' COMMENT 'the Product.  Links to Product table.',
  `RunningTotalQty` int(10) DEFAULT '0' COMMENT 'Running Total Quantity of this product in that particular branch.  (keeps summing item).  Total overall quantity since beginning of time.',
  `InventoryLocationID` int(10) NOT NULL DEFAULT '0' COMMENT 'the ID of the Branch.',
  `TotalAvailbleItemQty` int(10) DEFAULT '0' COMMENT 'Total Available Quantity of this product in the branch',
  `TotalReservedItemQty` int(10) DEFAULT '0' COMMENT 'Total Reserved Quantity of this product in the branch',
  `UnitPrice` decimal(10,2) unsigned DEFAULT '0.00' COMMENT 'Unit Price per branch. some branches have different price of the item',
  `TotalDamagedQty` int(10) DEFAULT '0' COMMENT 'Total Damaged Quantity of this product in this location',
  `TotalSupplierProblemQty` int(10) DEFAULT '0' COMMENT 'only used by Main Warehouse.  Quantity that needs to be returned to supplier or needs to be repaired and waiting for parts',
  `TotalInRepairQty` int(10) DEFAULT '0',
  `TotalToReceiveQty` int(10) DEFAULT '0',
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 7` (`ProductID`,`OrganizationID`,`InventoryLocationID`),
  KEY `FK__inventorylocation` (`InventoryLocationID`),
  KEY `FK_productinventorylocation_organization` (`OrganizationID`),
  KEY `FK_productinventorylocation_user` (`CreatedBy`),
  KEY `FK_productinventorylocation_user_2` (`LastUpdBy`),
  CONSTRAINT `FK__inventorylocation` FOREIGN KEY (`InventoryLocationID`) REFERENCES `inventorylocation` (`RowID`),
  CONSTRAINT `FK__product` FOREIGN KEY (`ProductID`) REFERENCES `product` (`RowID`),
  CONSTRAINT `FK_productinventorylocation_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_productinventorylocation_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_productinventorylocation_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Table that contains what products the location have (the quantity and shelf locations are in ProdInvLocInventory table)';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
