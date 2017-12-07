/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_dates_weekly`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `VIEW_dates_weekly`(IN `_year` INT)
    DETERMINISTIC
BEGIN

DECLARE daynameoffirstdateofyear CHAR(2);

DECLARE first_date DATE;

SELECT DAYOFWEEK(MAKEDATE(_year,1)) INTO daynameoffirstdateofyear;

SET @firstdate = MAKEDATE(_year,1);


    SELECT
    IF(daynameoffirstdateofyear=1,(SELECT @firstdate := MAKEDATE(_year,1)),ADDDATE(@firstdate, INTERVAL (1-daynameoffirstdateofyear) DAY)) AS Sun
    ,IF(daynameoffirstdateofyear=2,(SELECT @firstdate := MAKEDATE(_year,1)),ADDDATE(@firstdate, INTERVAL (2-daynameoffirstdateofyear) DAY)) AS Mon
    ,IF(daynameoffirstdateofyear=3,(SELECT @firstdate := MAKEDATE(_year,1)),ADDDATE(@firstdate, INTERVAL (3-daynameoffirstdateofyear) DAY)) AS Tue
    ,IF(daynameoffirstdateofyear=4,(SELECT @firstdate := MAKEDATE(_year,1)),ADDDATE(@firstdate, INTERVAL (4-daynameoffirstdateofyear) DAY)) AS Wed
    ,IF(daynameoffirstdateofyear=5,(SELECT @firstdate := MAKEDATE(_year,1)),ADDDATE(@firstdate, INTERVAL (5-daynameoffirstdateofyear) DAY)) AS Thu
    ,IF(daynameoffirstdateofyear=6,(SELECT @firstdate := MAKEDATE(_year,1)),ADDDATE(@firstdate, INTERVAL (6-daynameoffirstdateofyear) DAY)) AS Fri
    ,IF(daynameoffirstdateofyear=7,(SELECT @weeklastdate := MAKEDATE(_year,1)),(SELECT @weeklastdate := ADDDATE(@firstdate, INTERVAL (7-daynameoffirstdateofyear) DAY))) AS Sat
UNION
    SELECT d.DateValue AS Sun
    ,dd.DateValue AS Mon
    ,ddd.DateValue AS Tue
    ,d4.DateValue AS Wed
    ,dv.DateValue AS Thu
    ,dvi.DateValue AS Fri
    ,dvii.DateValue AS Sat
    FROM dates d
    LEFT JOIN dates dd ON ADDDATE(d.DateValue, INTERVAL 1 DAY)=dd.DateValue
    LEFT JOIN dates ddd ON ADDDATE(dd.DateValue, INTERVAL 1 DAY)=ddd.DateValue
    LEFT JOIN dates d4 ON ADDDATE(ddd.DateValue, INTERVAL 1 DAY)=d4.DateValue
    LEFT JOIN dates dv ON ADDDATE(d4.DateValue, INTERVAL 1 DAY)=dv.DateValue
    LEFT JOIN dates dvi ON ADDDATE(dv.DateValue, INTERVAL 1 DAY)=dvi.DateValue
    LEFT JOIN dates dvii ON ADDDATE(dvi.DateValue, INTERVAL 1 DAY)=dvii.DateValue
    WHERE d.DateValue > @weeklastdate
    AND YEAR(d.DateValue)=_year
    AND DAYOFWEEK(d.DateValue)=1;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
