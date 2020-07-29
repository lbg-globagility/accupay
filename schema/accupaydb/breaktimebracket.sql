/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `breaktimebracket` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `DivisionID` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT current_timestamp(),
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `LastUpdBy` int(10) DEFAULT NULL,
  `ShiftDuration` decimal(10,2) NOT NULL COMMENT 'In hours',
  `BreakDuration` decimal(10,2) NOT NULL COMMENT 'In hours',
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `AK_breaktimebracket_DivisionID_ShiftDuration` (`DivisionID`,`ShiftDuration`),
  KEY `FK_breaktimebracket_user_CreatedBy` (`CreatedBy`),
  KEY `FK_breaktimebracket_user_LastUpdBy` (`LastUpdBy`),
  KEY `FK_breaktimebracket_division_DivisionID` (`DivisionID`),
  CONSTRAINT `FK_breaktimebracket_division_DivisionID` FOREIGN KEY (`DivisionID`) REFERENCES `division` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `FK_breaktimebracket_user_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_breaktimebracket_user_LastUpdBy` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
