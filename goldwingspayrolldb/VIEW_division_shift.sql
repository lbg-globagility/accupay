-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.12-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for procedure VIEW_division_shift
DROP PROCEDURE IF EXISTS `VIEW_division_shift`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `VIEW_division_shift`(IN `OrganizID` INT, IN `DivisionRowID` INT)
    DETERMINISTIC
BEGIN

DECLARE shift_startdate_thisyear DATE;

DECLARE shift_enddate_thisyear DATE;

DECLARE anyintiger INT(11) DEFAULT 0;

SELECT DateValue
FROM dates
WHERE YEAR(DateValue)=YEAR(CURDATE())
AND DAYOFWEEK(DateValue)=(@@default_week_format + 1)
ORDER BY DateValue
LIMIT 1 INTO
shift_startdate_thisyear;

SELECT CAST(@@default_week_format AS INT) INTO anyintiger;

SELECT IF(LAST_DAY(DATE_FORMAT(CURDATE(),'%Y-12-01')) > d.DateValue
           ,ADDDATE(d.DateValue, INTERVAL 1 WEEK)
           ,LAST_DAY(DATE_FORMAT(CURDATE(),'%Y-12-01'))) AS EndingDate
FROM dates d
WHERE YEAR(d.DateValue) <= YEAR(CURDATE())
AND DAYOFWEEK(d.DateValue) = IF(anyintiger - 1 < 0, 7, anyintiger)
AND WEEKOFYEAR(d.DateValue) > 50
ORDER BY d.DateValue DESC
LIMIT 1
INTO shift_enddate_thisyear;

SELECT sh.RowID AS shRowID
,TIME_FORMAT(sh.TimeFrom, '%l:%i %p') AS TimeFrom
,TIME_FORMAT(sh.TimeTo, '%l:%i %p') AS TimeTo
FROM employeeshiftbyday esd
LEFT JOIN shift sh ON sh.RowID=esd.ShiftID AND sh.OrganizationID=esd.OrganizationID
INNER JOIN (SELECT e.*
                FROM employee e
                INNER JOIN position pos ON pos.DivisionId=DivisionRowID AND pos.OrganizationID=e.OrganizationID AND pos.RowID=e.PositionID
                WHERE e.OrganizationID=OrganizID) e ON e.RowID = esd.EmployeeID
WHERE esd.OrganizationID=OrganizID
AND esd.SampleDate BETWEEN shift_startdate_thisyear AND shift_enddate_thisyear
GROUP BY esd.NameOfDay,esd.ShiftID
ORDER BY esd.OrderByValue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
