/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `prodinvlocinventory` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL,
  `CreatedBy` int(11) NOT NULL DEFAULT '0',
  `OrganizationID` int(11) NOT NULL DEFAULT '0' COMMENT 'Internal Company',
  `LastShippedToLocDate` date DEFAULT NULL COMMENT 'When this item from this inventory location was last shipped to',
  `LastCycleCountDate` date DEFAULT NULL COMMENT 'last cycle count on this rack/shelf',
  `LastUpdBy` int(11) NOT NULL DEFAULT '0',
  `ProdInvLocID` int(10) NOT NULL DEFAULT '0' COMMENT 'links to Product Inventory Location.  ',
  `AvailableQty` int(10) unsigned zerofill DEFAULT '0000000000' COMMENT 'Qty available of this product in this location',
  `DistributedQty` int(10) DEFAULT '0' COMMENT 'Qty distributed to each rack/shelf/column when receiving',
  `ReservedQty` int(10) DEFAULT '0' COMMENT 'qty Reserved of this product in this location',
  `SupplierProblemQty` int(10) DEFAULT '0' COMMENT 'only used by main warehouse, this is the quantity that needs to be fixed by supplier or waiting parts from supplier to be fixed',
  `DamagedQty` int(10) NOT NULL DEFAULT '0' COMMENT 'This is used for damaged goods',
  `InRepairQty` int(10) DEFAULT '0' COMMENT 'This is used to track items that are in still in Repair Center (Status of Repair is Open)',
  `RackNo` varchar(50) DEFAULT '0' COMMENT 'the rack number',
  `ShelfNo` varchar(50) DEFAULT '0' COMMENT 'shelf number',
  `ColumnNo` varchar(50) DEFAULT '0' COMMENT 'column number',
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 6` (`RackNo`,`OrganizationID`,`ShelfNo`,`ColumnNo`,`ProdInvLocID`),
  KEY `FK__productinventorylocation` (`ProdInvLocID`),
  KEY `FK_prodinvlocinventory_organization` (`OrganizationID`),
  KEY `FK__user` (`CreatedBy`),
  KEY `FK__user_2` (`LastUpdBy`),
  CONSTRAINT `FK__productinventorylocation` FOREIGN KEY (`ProdInvLocID`) REFERENCES `productinventorylocation` (`RowID`),
  CONSTRAINT `FK__user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK__user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_prodinvlocinventory_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Inventory Location''s inventory of products (including shelf location and quantities)';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
