/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `filingstatus` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `Created` datetime NOT NULL DEFAULT current_timestamp(),
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpdBy` int(10) DEFAULT NULL,
  `FilingStatus` varchar(50) DEFAULT NULL COMMENT 'Z, S/ME, ME1, ME2, ME3, ME4',
  `MaritalStatus` varchar(50) DEFAULT NULL,
  `Dependent` int(11) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 4` (`MaritalStatus`,`Dependent`),
  KEY `FK_FilingStatus_user` (`CreatedBy`),
  KEY `FK_FilingStatus_user_2` (`LastUpdBy`),
  CONSTRAINT `FK_FilingStatus_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_FilingStatus_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Filing Status for Employee';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
