/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `payfrequency` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `CreatedBy` int(10) NOT NULL DEFAULT 0,
  `LastUpdBy` int(10) DEFAULT 0,
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `Created` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `PayFrequencyType` varchar(50) NOT NULL COMMENT 'Daily, Weekly, Monthly, Semi-Monthly, Bi-Weekly',
  `PayFrequencyStartDate` date DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_PayFrequency_user` (`CreatedBy`),
  KEY `FK_PayFrequency_user_2` (`LastUpdBy`),
  CONSTRAINT `FK_PayFrequency_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_PayFrequency_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Payroll Frequency - Daily, Weekly, Monthly, Semi-Monthly, Bi-Weekly';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
