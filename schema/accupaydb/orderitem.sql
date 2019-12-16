/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `orderitem` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `ProductID` int(10) NOT NULL DEFAULT '0' COMMENT 'Product ID (links to Product table)',
  `ParentOrderID` int(10) NOT NULL DEFAULT '0' COMMENT 'OrderID. Links to Order Table',
  `ProductName` varchar(50) NOT NULL DEFAULT '0' COMMENT 'The Product Name.',
  `PartNo` varchar(200) NOT NULL DEFAULT '0' COMMENT 'The Product Part Number.',
  `OrganizationID` int(11) NOT NULL DEFAULT '0' COMMENT 'Internal Company',
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LineNum` int(11) NOT NULL COMMENT 'sequence per order - 1, 2,3.  Line number of the order line',
  `LastShipmentDate` date DEFAULT NULL COMMENT 'retrieved from the ProductID.  When this item was last received in a shipment',
  `LastShipmentQty` int(11) DEFAULT '0' COMMENT 'retrieved from the ProductID.  how many item was received from last shipment',
  `QtyRequested` int(11) DEFAULT '0' COMMENT 'quantity Requested in this order for this item',
  `QtyOrdered` int(11) DEFAULT '0' COMMENT 'quantity Ordered from Supplier',
  `QtyApproved` int(11) DEFAULT '0' COMMENT 'quantity Approved from Requested',
  `QtyIssued` int(11) DEFAULT '0' COMMENT 'quantity Issued from Warehouse',
  `Status` varchar(10) DEFAULT NULL COMMENT 'Open, Reserved',
  `UnitOfMeasure` varchar(50) DEFAULT NULL,
  `VerifiedBy` int(11) DEFAULT NULL,
  `VerificationDate` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `MainOfficeEndingInventory` int(11) NOT NULL COMMENT 'Ending Inventory of the main warehouse. retrieved from InventoryLocationInv',
  `SRP` int(11) NOT NULL COMMENT 'Suggested retail price',
  `TotalOverallEndingInventory` int(11) NOT NULL COMMENT 'Total Ending Inventory of all branches including main warehouse for this item',
  `SupplierID` int(11) DEFAULT NULL COMMENT 'who the supplier for this item is.',
  `Remarks` varchar(2000) DEFAULT NULL,
  `CreatedBy` int(11) NOT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastUpdBy` int(11) NOT NULL DEFAULT '0',
  `Verify` char(1) DEFAULT NULL COMMENT 'verified accurate by the GM',
  `CartonNo` varchar(50) DEFAULT NULL COMMENT 'carton number of this item',
  `BatchNo` varchar(50) DEFAULT NULL,
  `FinalVerification` char(1) DEFAULT NULL COMMENT 'verified accurate by CEO',
  `InvoiceNo` int(10) DEFAULT NULL,
  `RelatedOrderID` int(10) DEFAULT NULL,
  `RelatedPOID` int(10) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_orderitem_product` (`ProductID`),
  KEY `FK_orderitem_order` (`ParentOrderID`),
  KEY `FK_orderitem_organization` (`OrganizationID`),
  KEY `FK_orderitem_user` (`LastUpdBy`),
  KEY `FK_orderitem_user_2` (`CreatedBy`),
  KEY `FK_orderitem_user_3` (`VerifiedBy`),
  KEY `FK_orderitem_order_2` (`RelatedOrderID`),
  KEY `FK_orderitem_order_3` (`RelatedPOID`),
  CONSTRAINT `FK_orderitem_order` FOREIGN KEY (`ParentOrderID`) REFERENCES `order` (`RowID`),
  CONSTRAINT `FK_orderitem_order_2` FOREIGN KEY (`RelatedOrderID`) REFERENCES `order` (`RowID`),
  CONSTRAINT `FK_orderitem_order_3` FOREIGN KEY (`RelatedPOID`) REFERENCES `order` (`RowID`),
  CONSTRAINT `FK_orderitem_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_orderitem_product` FOREIGN KEY (`ProductID`) REFERENCES `product` (`RowID`),
  CONSTRAINT `FK_orderitem_user` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_orderitem_user_2` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_orderitem_user_3` FOREIGN KEY (`VerifiedBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Items ordered against a Sales, or Purchase order';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
