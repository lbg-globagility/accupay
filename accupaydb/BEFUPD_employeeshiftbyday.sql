/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFUPD_employeeshiftbyday`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFUPD_employeeshiftbyday` BEFORE UPDATE ON `employeeshiftbyday` FOR EACH ROW BEGIN

DECLARE StartingDate DATE DEFAULT NULL;
DECLARE empStartingDate DATE DEFAULT NULL;
DECLARE tooOldYearCount INT(11);

DECLARE anytime TIMESTAMP;

SELECT StartDate FROM employee WHERE RowID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID INTO StartingDate;
SELECT d.DateValue FROM dates d WHERE d.DateValue < StartingDate AND CHAR_TO_DAYOFWEEK(@@default_week_format + 1)=DAYNAME(d.DateValue) ORDER BY d.DateValue DESC LIMIT 1 INTO StartingDate;

IF StartingDate IS NULL THEN

    SELECT d.DateValue FROM dates d WHERE CHAR_TO_DAYOFWEEK(@@default_week_format + 1)=DAYNAME(d.DateValue) HAVING MIN(d.DateValue) INTO StartingDate;
    SELECT StartDate FROM employee WHERE RowID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID INTO empStartingDate;
    SELECT i.Column4
    FROM (SELECT *,DAYNAME(DateValue) AS Column1,DAYOFWEEK(DateValue) AS Column2,(DAYOFWEEK(DateValue) = DAYOFWEEK(empStartingDate)) AS Column3, SUBDATE(empStartingDate, INTERVAL (DAYOFWEEK(DateValue) - 1) DAY) AS Column4 FROM dates WHERE DateValue >= empStartingDate ORDER BY DateValue LIMIT 7) i WHERE i.Column3=1 INTO StartingDate;

END IF;

IF TIMESTAMPDIFF(YEAR,StartingDate,CURDATE()) > 1 THEN


    SELECT DateValue FROM dates WHERE YEAR(DateValue)=YEAR(SUBDATE(CURDATE(),INTERVAL 1 YEAR)) AND DAYOFWEEK(DateValue)=(@@default_week_format + 1) ORDER BY DateValue LIMIT 1 INTO StartingDate;

END IF;















SET NEW.OriginDay = NEW.OrderByValue + (SELECT DATEDIFF(StartingDate,StartDate) FROM employee WHERE RowID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID);

SET NEW.SampleDate = ADDDATE(StartingDate, INTERVAL NEW.OrderByValue DAY);



END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
