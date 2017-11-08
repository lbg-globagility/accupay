/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `cyclecountitem` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `CreatedBy` int(11) NOT NULL,
  `CycleCountID` int(11) NOT NULL,
  `LastUpdBy` int(11) NOT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `OrganizationID` int(10) NOT NULL,
  `RackNo` varchar(50) DEFAULT '0',
  `ShelfNo` varchar(50) DEFAULT '0',
  `ColumnNo` varchar(50) DEFAULT '0',
  `ProductID` int(10) NOT NULL,
  `OriginalQty` int(10) DEFAULT '0',
  `CycleCount1Qty` int(10) DEFAULT '0',
  `CycleCount2Qty` int(10) DEFAULT '0',
  `CycleCount1ContactID` int(10) DEFAULT NULL,
  `CycleCount2ContactID` int(10) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_Notes_user` (`CreatedBy`),
  KEY `FK_Notes_user_2` (`LastUpdBy`),
  KEY `FK_cyclecount_organization` (`OrganizationID`),
  KEY `FK_cyclecountItem_cyclecount` (`CycleCountID`),
  KEY `FK_cyclecountItem_product` (`ProductID`),
  KEY `FK_cyclecountItem_user` (`CycleCount1ContactID`),
  KEY `FK_cyclecountItem_user_2` (`CycleCount2ContactID`),
  CONSTRAINT `FK_cyclecountItem_cyclecount` FOREIGN KEY (`CycleCountID`) REFERENCES `cyclecount` (`RowID`),
  CONSTRAINT `FK_cyclecountItem_product` FOREIGN KEY (`ProductID`) REFERENCES `product` (`RowID`),
  CONSTRAINT `FK_cyclecountItem_user` FOREIGN KEY (`CycleCount1ContactID`) REFERENCES `contact` (`RowID`),
  CONSTRAINT `FK_cyclecountItem_user_2` FOREIGN KEY (`CycleCount2ContactID`) REFERENCES `contact` (`RowID`),
  CONSTRAINT `cyclecountitem_ibfk_1` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `cyclecountitem_ibfk_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `cyclecountitem_ibfk_3` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='for cycle counts';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
