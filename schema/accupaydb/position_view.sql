/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `position_view` (
  `RowID` int(11) NOT NULL AUTO_INCREMENT,
  `PositionID` int(11) DEFAULT NULL,
  `ViewID` int(11) DEFAULT NULL,
  `Creates` char(1) DEFAULT NULL,
  `OrganizationID` int(11) DEFAULT NULL,
  `ReadOnly` char(1) DEFAULT NULL,
  `Updates` char(1) DEFAULT NULL,
  `Deleting` char(1) DEFAULT NULL,
  `AllowedToAccess` char(1) DEFAULT 'N',
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` int(11) NOT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastUpdBy` int(11) NOT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 7` (`PositionID`,`ViewID`,`OrganizationID`),
  KEY `FK_position_view_view` (`ViewID`),
  KEY `FK_position_view_organization` (`OrganizationID`),
  KEY `FK_position_view_user` (`CreatedBy`),
  KEY `FK_position_view_user_2` (`LastUpdBy`),
  CONSTRAINT `FK_position_view_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_position_view_position` FOREIGN KEY (`PositionID`) REFERENCES `position` (`RowID`),
  CONSTRAINT `FK_position_view_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_position_view_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_position_view_view` FOREIGN KEY (`ViewID`) REFERENCES `view` (`RowID`)
) ENGINE=InnoDB AUTO_INCREMENT=37534 DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
