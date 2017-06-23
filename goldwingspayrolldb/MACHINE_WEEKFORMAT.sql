-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.11-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.0.0.4396
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for procedure goldwingspayrolldb.MACHINE_WEEKFORMAT
DROP PROCEDURE IF EXISTS `MACHINE_WEEKFORMAT`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `MACHINE_WEEKFORMAT`(IN `iFirstDayOfWeek` CHAR(1))
    DETERMINISTIC
BEGIN

SET @startdayofweek = CURDATE();

SELECT ii.WeekVariable,ii.`DayName`,ii.DayFullName
FROM (SELECT (@startdayofweek := d.DateValue) AS WeekVariable,UPPER(SUBSTRING(DAYNAME(d.DateValue),1,3)) AS `DayName`,DAYNAME(d.DateValue) AS DayFullName FROM dates d WHERE DATE_FORMAT(d.DateValue,'%w')=iFirstDayOfWeek LIMIT 1) ii
UNION
SELECT (@startdayofweek := d.DateValue) AS WeekVariable,UPPER(SUBSTRING(DAYNAME(d.DateValue),1,3)) AS `DayName`,DAYNAME(d.DateValue) AS DayFullName FROM dates d WHERE d.DateValue > @startdayofweek LIMIT 7;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
