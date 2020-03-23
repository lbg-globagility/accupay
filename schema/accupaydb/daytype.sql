/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `daytype` (
  `RowID` int(11) NOT NULL AUTO_INCREMENT,
  `Created` timestamp NOT NULL DEFAULT current_timestamp(),
  `CreatedBy` int(11) DEFAULT NULL,
  `Updated` timestamp NULL DEFAULT NULL ON UPDATE current_timestamp(),
  `UpdatedBy` int(11) DEFAULT NULL,
  `Name` varchar(100) NOT NULL,
  `DayConsideredAs` varchar(50) DEFAULT NULL,
  `RegularRate` decimal(10,4) NOT NULL,
  `OvertimeRate` decimal(10,4) NOT NULL,
  `NightDiffRate` decimal(10,4) NOT NULL,
  `NightDiffOTRate` decimal(10,4) NOT NULL,
  `RestDayRate` decimal(10,4) NOT NULL,
  `RestDayOTRate` decimal(10,4) NOT NULL,
  `RestDayNDRate` decimal(10,4) NOT NULL,
  `RestDayNDOTRate` decimal(10,4) NOT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Name` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
