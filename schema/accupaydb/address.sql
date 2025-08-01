/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `address` (
  `RowID` int(11) NOT NULL AUTO_INCREMENT,
  `StreetAddress1` varchar(200) DEFAULT NULL,
  `StreetAddress2` varchar(50) DEFAULT NULL,
  `CityTown` varchar(50) DEFAULT NULL,
  `Country` varchar(50) DEFAULT NULL,
  `State` varchar(50) DEFAULT NULL,
  `CreatedBy` int(11) NOT NULL,
  `LastUpdBy` int(11) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT current_timestamp(),
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `ZipCode` varchar(50) DEFAULT NULL,
  `Barangay` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_address_user` (`CreatedBy`),
  KEY `FK_address_user_2` (`LastUpdBy`),
  CONSTRAINT `FK_address_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_address_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Address Table';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
