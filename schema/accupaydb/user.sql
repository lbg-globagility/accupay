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
  `Created` datetime DEFAULT CURRENT_TIMESTAMP,
  `LastUpdBy` int(11) DEFAULT NULL,
  `CreatedBy` int(11) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Status` varchar(10) DEFAULT NULL,
  `EmailAddress` varchar(50) DEFAULT NULL,
  `AllowLimitedAccess` char(1) DEFAULT NULL,
  `Column 16` char(1) DEFAULT NULL,
  `InSession` char(1) DEFAULT '0',
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Uniqu_KEY` (`UserID`,`OrganizationID`),
  KEY `FK_user_position` (`PositionID`),
  KEY `FK_user_organization` (`OrganizationID`),
  KEY `FK_user_user` (`CreatedBy`),
  KEY `FK_user_user_2` (`LastUpdBy`),
  CONSTRAINT `FK_user_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_user_position` FOREIGN KEY (`PositionID`) REFERENCES `position` (`RowID`),
  CONSTRAINT `FK_user_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_user_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
