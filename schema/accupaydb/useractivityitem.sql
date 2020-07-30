/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `useractivityitem` (
  `RowID` int(11) NOT NULL AUTO_INCREMENT,
  `UserActivityId` int(11) NOT NULL,
  `EntityId` int(11) NOT NULL,
  `Description` varchar(2000) NOT NULL,
  `Created` datetime NOT NULL DEFAULT current_timestamp(),
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  PRIMARY KEY (`RowID`),
  KEY `FK_useractivityitem_useractivity_UserActivityId` (`UserActivityId`),
  CONSTRAINT `FK_useractivityitem_useractivity_UserActivityId` FOREIGN KEY (`UserActivityId`) REFERENCES `useractivity` (`RowID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
