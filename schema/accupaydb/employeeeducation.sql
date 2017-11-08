/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `employeeeducation` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) DEFAULT NULL,
  `DateFrom` varchar(100) DEFAULT NULL,
  `OrganizationID` int(10) NOT NULL,
  `EmployeeID` int(10) NOT NULL,
  `DateTo` varchar(50) DEFAULT NULL,
  `School` varchar(100) DEFAULT NULL,
  `Degree` varchar(100) DEFAULT NULL,
  `Course` varchar(100) DEFAULT NULL,
  `Minor` varchar(100) DEFAULT NULL,
  `EducationType` varchar(100) DEFAULT NULL COMMENT 'School, Certiication',
  `Remarks` varchar(1000) DEFAULT NULL,
  `Created` datetime DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` int(11) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastUpdBy` int(11) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  KEY `FK_organization_user` (`CreatedBy`),
  KEY `FK_organization_user_2` (`LastUpdBy`),
  KEY `FK_employeepreviousemployer_organization` (`OrganizationID`),
  KEY `FK_employeepreviousemployer_employee` (`EmployeeID`),
  CONSTRAINT `employeeeducation_ibfk_1` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`),
  CONSTRAINT `employeeeducation_ibfk_2` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `employeeeducation_ibfk_3` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `employeeeducation_ibfk_4` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `employeeeducation_ibfk_5` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `employeeeducation_ibfk_6` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='This is the internal Company';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
