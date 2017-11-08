/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `audittrail` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastUpdBy` int(10) NOT NULL,
  `OrganizationID` int(10) NOT NULL COMMENT 'Internal Company',
  `ViewID` int(10) DEFAULT NULL COMMENT 'The view that the modification is being done',
  `FieldChanged` varchar(100) NOT NULL DEFAULT '0' COMMENT 'What field was changed',
  `ChangedRowID` int(10) NOT NULL,
  `OldValue` varchar(200) DEFAULT '0' COMMENT 'old value of field',
  `NewValue` varchar(200) DEFAULT '0' COMMENT 'new value of field',
  `ActionPerformed` varchar(50) NOT NULL DEFAULT '0' COMMENT 'New Record, Modify Record, Delete Record',
  PRIMARY KEY (`RowID`),
  KEY `FK_audittrail_user` (`CreatedBy`),
  KEY `FK_audittrail_organization` (`OrganizationID`),
  KEY `FK_audittrail_user_2` (`LastUpdBy`),
  CONSTRAINT `audittrail_ibfk_1` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `audittrail_ibfk_2` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `audittrail_ibfk_3` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB AUTO_INCREMENT=477509 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Log of changes to the system';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
