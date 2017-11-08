/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `position` (
  `RowID` int(11) NOT NULL AUTO_INCREMENT,
  `PositionName` varchar(50) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Created` datetime DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` int(11) DEFAULT NULL,
  `OrganizationID` int(11) NOT NULL,
  `LastUpdBy` int(11) DEFAULT NULL,
  `ParentPositionID` int(10) DEFAULT NULL,
  `DivisionId` int(10) DEFAULT NULL COMMENT 'Department ID',
  `LevelNumber` int(10) DEFAULT '3',
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 5` (`PositionName`,`OrganizationID`),
  KEY `FK_position_organization` (`OrganizationID`),
  KEY `FK_position_user` (`CreatedBy`),
  KEY `FK_position_user_2` (`LastUpdBy`),
  KEY `FK_position_position` (`ParentPositionID`),
  KEY `FK_position_division` (`DivisionId`),
  CONSTRAINT `FK_position_division` FOREIGN KEY (`DivisionId`) REFERENCES `division` (`RowID`),
  CONSTRAINT `FK_position_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_position_position` FOREIGN KEY (`ParentPositionID`) REFERENCES `position` (`RowID`),
  CONSTRAINT `FK_position_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_position_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB AUTO_INCREMENT=1136 DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
