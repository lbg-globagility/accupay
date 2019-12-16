/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `productshipmenthistory` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `ProductID` int(10) NOT NULL,
  `CreatedBy` int(11) NOT NULL,
  `LastUpdBy` int(11) DEFAULT NULL,
  `OrganizationID` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `ShipmentCount` int(10) DEFAULT NULL,
  `ShipmentDate` date DEFAULT NULL,
  `LotNo` varchar(50) DEFAULT NULL,
  `BatchNo` varchar(50) DEFAULT NULL,
  `CartonNo` varchar(50) DEFAULT NULL,
  `InvoiceNo` int(10) DEFAULT NULL COMMENT 'InvoiceNo from OrderItem (manually entered) - this is the shipment''s/suppliers invoice #',
  PRIMARY KEY (`RowID`),
  KEY `FK_productshipmenthistory_product` (`ProductID`),
  KEY `FK_productshipmenthistory_organization` (`OrganizationID`),
  KEY `FK_productshipmenthistory_user` (`CreatedBy`),
  KEY `FK_productshipmenthistory_user_2` (`LastUpdBy`),
  CONSTRAINT `FK_productshipmenthistory_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_productshipmenthistory_product` FOREIGN KEY (`ProductID`) REFERENCES `product` (`RowID`),
  CONSTRAINT `FK_productshipmenthistory_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_productshipmenthistory_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Contains the history of the shipments from suppliers';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
