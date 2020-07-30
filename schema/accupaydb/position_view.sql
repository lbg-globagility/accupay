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
  `Created` datetime NOT NULL DEFAULT current_timestamp(),
  `CreatedBy` int(11) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `LastUpdBy` int(11) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `AK_position_view_PositionID_ViewID_OrganizationID` (`PositionID`,`ViewID`,`OrganizationID`),
  KEY `FK_position_view_ViewID` (`ViewID`),
  KEY `FK_position_view_organization_OrganizationID` (`OrganizationID`),
  KEY `FK_position_view_user_CreatedBy` (`CreatedBy`),
  KEY `FK_position_view_user_LastUpdBy` (`LastUpdBy`),
  CONSTRAINT `FK_position_view_organization_OrganizationID` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_position_view_position_PositionID` FOREIGN KEY (`PositionID`) REFERENCES `position` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `FK_position_view_user_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_position_view_user_LastUpdBy` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_position_view_view_ViewID` FOREIGN KEY (`ViewID`) REFERENCES `view` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
