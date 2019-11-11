/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `returns` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `DeductedFromInventory` char(1) NOT NULL DEFAULT '0',
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `CreatedBy` int(10) NOT NULL,
  `LastUpdBy` int(10) NOT NULL,
  `OrganizationID` int(10) NOT NULL,
  `InvoiceID` int(10) NOT NULL,
  `OrderID` int(10) NOT NULL,
  `Quantity` int(10) NOT NULL,
  `Type` varchar(50) NOT NULL COMMENT 'Exchange, Refund',
  `TotalQty` int(10) NOT NULL,
  `TotalAmount` decimal(10,2) NOT NULL,
  `Amount` decimal(10,2) NOT NULL,
  `ProductID` int(10) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_Returns_user` (`CreatedBy`),
  KEY `FK_Returns_user_2` (`LastUpdBy`),
  KEY `FK_Returns_organization` (`OrganizationID`),
  KEY `FK_Returns_invoice` (`InvoiceID`),
  KEY `FK_Returns_order` (`OrderID`),
  KEY `FK_Returns_product` (`ProductID`),
  CONSTRAINT `FK_Returns_invoice` FOREIGN KEY (`InvoiceID`) REFERENCES `invoice` (`RowID`),
  CONSTRAINT `FK_Returns_order` FOREIGN KEY (`OrderID`) REFERENCES `order` (`RowID`),
  CONSTRAINT `FK_Returns_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_Returns_product` FOREIGN KEY (`ProductID`) REFERENCES `product` (`RowID`),
  CONSTRAINT `FK_Returns_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_Returns_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Table to store returns refunds/exchanges';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
