/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `joblevel` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) DEFAULT NULL,
  `Created` timestamp NULL DEFAULT NULL,
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` timestamp NULL DEFAULT NULL,
  `LastUpdBy` int(10) DEFAULT NULL,
  `JobCategoryID` int(10) DEFAULT NULL,
  `Name` varchar(50) DEFAULT NULL,
  `Points` int(10) NOT NULL DEFAULT 0,
  `SalaryRangeFrom` decimal(15,4) NOT NULL DEFAULT 0.0000,
  `SalaryRangeTo` decimal(15,4) NOT NULL DEFAULT 0.0000,
  PRIMARY KEY (`RowID`),
  KEY `FK_joblevel_organization` (`OrganizationID`),
  KEY `FK_joblevel_jobcategory` (`JobCategoryID`),
  CONSTRAINT `FK_joblevel_jobcategory` FOREIGN KEY (`JobCategoryID`) REFERENCES `jobcategory` (`RowID`),
  CONSTRAINT `FK_joblevel_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
