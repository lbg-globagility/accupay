/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `cyclecount` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `CreatedBy` int(11) NOT NULL,
  `CycleCountNo` int(11) NOT NULL DEFAULT '0',
  `LastUpdBy` int(11) NOT NULL,
  `CycleCountBy` varchar(50) DEFAULT NULL COMMENT 'Location or Product',
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `OrganizationID` int(10) NOT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_Notes_user` (`CreatedBy`),
  KEY `FK_Notes_user_2` (`LastUpdBy`),
  KEY `FK_cyclecount_organization` (`OrganizationID`),
  CONSTRAINT `FK_cyclecount_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_cyclecount_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_cyclecount_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Notes table used to add remarks.';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
