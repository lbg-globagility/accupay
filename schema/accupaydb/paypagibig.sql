/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `paypagibig` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `Created` timestamp NOT NULL DEFAULT current_timestamp(),
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `CreatedBy` int(10) NOT NULL,
  `LastUpdBy` int(10) DEFAULT NULL,
  `SalaryRangeFrom` decimal(10,2) DEFAULT NULL,
  `SalaryRangeTo` decimal(10,2) DEFAULT NULL,
  `EmployeeShare` decimal(10,2) DEFAULT NULL,
  `EmployerShare` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_user_user` (`CreatedBy`),
  KEY `FK_user_user_2` (`LastUpdBy`),
  CONSTRAINT `paypagibig_ibfk_1` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `paypagibig_ibfk_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Philhealth table';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
