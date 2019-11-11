/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `productmovementhistory` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) DEFAULT NULL,
  `Created` datetime DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` int(11) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastUpdBy` int(11) DEFAULT NULL,
  `ProductID` int(10) DEFAULT NULL,
  `Comments` varchar(2000) DEFAULT NULL,
  `TransactionType` varchar(50) NOT NULL COMMENT 'Stock Adjustment, Stock Transfer, Receiving, Shipping,etc.',
  `OrderID` int(11) DEFAULT NULL,
  `OriginatingInvLocationID` int(11) DEFAULT NULL,
  `DestinationInvLocationID` int(11) DEFAULT NULL,
  `CurrentQty` int(11) DEFAULT NULL,
  `NewQty` int(11) DEFAULT NULL,
  `OrigRackNo` varchar(50) DEFAULT NULL,
  `OrigShelfNo` varchar(50) DEFAULT NULL,
  `OrigColumnNo` varchar(50) DEFAULT NULL,
  `DestinationRackNo` varchar(50) DEFAULT NULL,
  `DestinationhelfNo` varchar(50) DEFAULT NULL,
  `DestinationColumnNo` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_productmovementhistory_organization` (`OrganizationID`),
  KEY `FK_productmovementhistory_product` (`ProductID`),
  KEY `FK_productmovementhistory_order` (`OrderID`),
  KEY `FK_productmovementhistory_user` (`LastUpdBy`),
  KEY `FK_productmovementhistory_user_2` (`CreatedBy`),
  CONSTRAINT `FK_productmovementhistory_order` FOREIGN KEY (`OrderID`) REFERENCES `order` (`RowID`),
  CONSTRAINT `FK_productmovementhistory_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_productmovementhistory_product` FOREIGN KEY (`ProductID`) REFERENCES `product` (`RowID`),
  CONSTRAINT `FK_productmovementhistory_user` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_productmovementhistory_user_2` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Tracks the movement history of a particular product every time the product moves (whether it be Purchase Order, Warehouse Receivng, Customer Order, Customer Fulfillment, Stock Transfer, Stock Adjustment, Returns, etc)';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
