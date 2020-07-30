/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `user` (
  `RowID` int(11) NOT NULL AUTO_INCREMENT,
  `LastName` varchar(50) DEFAULT NULL,
  `FirstName` varchar(50) DEFAULT NULL,
  `MiddleName` varchar(50) DEFAULT NULL,
  `UserID` varchar(50) DEFAULT NULL,
  `Password` varchar(50) DEFAULT NULL,
  `OrganizationID` int(11) NOT NULL,
  `PositionID` int(11) NOT NULL,
  `Created` datetime DEFAULT current_timestamp(),
  `LastUpdBy` int(11) DEFAULT NULL,
  `CreatedBy` int(11) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `Status` varchar(10) DEFAULT NULL,
  `EmailAddress` varchar(50) DEFAULT NULL,
  `AllowLimitedAccess` char(1) DEFAULT NULL,
  `Column 16` char(1) DEFAULT NULL,
  `InSession` char(1) DEFAULT '0',
  `UserLevel` smallint(1) NOT NULL DEFAULT 0,
  `AspNetUserId` binary(16) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Uniqu_KEY` (`UserID`,`OrganizationID`),
  KEY `FK_user_position` (`PositionID`),
  KEY `FK_user_organization` (`OrganizationID`),
  KEY `FK_user_user` (`CreatedBy`),
  KEY `FK_user_user_2` (`LastUpdBy`),
  KEY `FK_user_aspnetusers_AspNetUserId` (`AspNetUserId`),
  CONSTRAINT `FK_user_aspnetusers_AspNetUserId` FOREIGN KEY (`AspNetUserId`) REFERENCES `aspnetusers` (`Id`),
  CONSTRAINT `FK_user_organization_OrganizationID` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_user_position_PositionID` FOREIGN KEY (`PositionID`) REFERENCES `position` (`RowID`),
  CONSTRAINT `FK_user_user_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_user_user_LastUpdBy` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
