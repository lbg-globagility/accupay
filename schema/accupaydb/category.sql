/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `category` (
  `RowID` int(11) NOT NULL AUTO_INCREMENT,
  `CategoryID` int(11) DEFAULT NULL,
  `CategoryName` varchar(50) DEFAULT NULL COMMENT 'Indoor Lights, Outdoor Lights (LOV="CATEGORY_NAME")',
  `OrganizationID` int(10) NOT NULL,
  `Catalog` varchar(50) DEFAULT NULL,
  `CatalogID` int(11) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `CategoryName` (`CategoryName`,`OrganizationID`),
  KEY `FK_category_organization` (`OrganizationID`),
  KEY `FK_category_catalog` (`CatalogID`),
  CONSTRAINT `FK_category_catalog` FOREIGN KEY (`CatalogID`) REFERENCES `catalog` (`RowID`),
  CONSTRAINT `FK_category_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
