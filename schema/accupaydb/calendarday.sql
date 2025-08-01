/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `calendarday` (
  `RowID` int(11) NOT NULL AUTO_INCREMENT,
  `Created` timestamp NOT NULL DEFAULT current_timestamp(),
  `CreatedBy` int(11) DEFAULT NULL,
  `Updated` timestamp NULL DEFAULT NULL ON UPDATE current_timestamp(),
  `UpdatedBy` int(11) DEFAULT NULL,
  `CalendarID` int(11) NOT NULL,
  `DayTypeID` int(11) NOT NULL,
  `Date` date NOT NULL,
  `Description` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `CalendarID` (`CalendarID`,`Date`),
  KEY `FK_calendarday_daytype` (`DayTypeID`),
  CONSTRAINT `FK_calendarday_calendar_CalendarID` FOREIGN KEY (`CalendarID`) REFERENCES `calendar` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `FK_calendarday_daytype_DayTypeID` FOREIGN KEY (`DayTypeID`) REFERENCES `daytype` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
