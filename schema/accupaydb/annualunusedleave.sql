/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `annualunusedleave` (
  `RowID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` timestamp NULL DEFAULT NULL,
  `LastUpdBy` int(10) DEFAULT NULL,
  `EmployeeID` int(10) NOT NULL,
  `TotalLeave` decimal(15,4) DEFAULT NULL,
  `Year` int(10) NOT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `only_one_per_employee_per_year` (`EmployeeID`,`Year`),
  KEY `FK_annualunusedleave_user` (`OrganizationID`),
  KEY `FK_annualunusedleave_user_2` (`CreatedBy`),
  KEY `FK_annualunusedleave_user_3` (`LastUpdBy`),
  CONSTRAINT `FK_annualunusedleave_user` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `FK_annualunusedleave_user_2` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_annualunusedleave_user_3` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB AUTO_INCREMENT=1573 DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
