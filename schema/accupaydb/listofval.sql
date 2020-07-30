/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `listofval` (
  `RowID` int(11) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(11) DEFAULT NULL,
  `DisplayValue` varchar(50) DEFAULT NULL,
  `LIC` varchar(50) DEFAULT NULL COMMENT 'Language Independent Code',
  `Type` varchar(50) DEFAULT NULL,
  `ParentLIC` varchar(50) DEFAULT NULL,
  `Active` varchar(50) DEFAULT NULL,
  `Description` varchar(500) DEFAULT NULL,
  `Created` datetime NOT NULL,
  `CreatedBy` int(11) NOT NULL,
  `LastUpd` datetime NOT NULL,
  `OrderBy` int(22) DEFAULT NULL,
  `LastUpdBy` int(11) NOT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Unique` (`LIC`,`ParentLIC`,`Type`),
  KEY `FK_listofval_user` (`LastUpdBy`),
  KEY `FK_listofval_user_2` (`CreatedBy`),
  CONSTRAINT `FK_listofval_user` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_listofval_user_2` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
