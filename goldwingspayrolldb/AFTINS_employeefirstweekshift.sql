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

-- Dumping structure for trigger AFTINS_employeefirstweekshift
DROP TRIGGER IF EXISTS `AFTINS_employeefirstweekshift`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_employeefirstweekshift` AFTER INSERT ON `employeefirstweekshift` FOR EACH ROW BEGIN

DECLARE anyintiger INT(11) DEFAULT 0;

DECLARE date_diff INT(11);

DECLARE lastdateof_default_week_format DATE;

DECLARE EndingDate DATE DEFAULT LAST_DAY(DATE_FORMAT(CURDATE(),'%Y-12-01'));

SET date_diff = DATEDIFF(NEW.EffectiveTo,NEW.EffectiveFrom);

SELECT @@default_week_format INTO anyintiger;

SELECT IF(EndingDate > d.DateValue, ADDDATE(d.DateValue, INTERVAL 1 WEEK), EndingDate) FROM dates d WHERE YEAR(d.DateValue)=YEAR(CURDATE()) AND DAYOFWEEK(d.DateValue)=IF(anyintiger - 1 < 0, 7, anyintiger) ORDER BY d.DateValue DESC LIMIT 1 INTO lastdateof_default_week_format;

INSERT INTO employeeshift
(
    OrganizationID
    ,CreatedBy
    ,EmployeeID
    ,ShiftID
    ,EffectiveFrom
    ,EffectiveTo
    ,NightShift
    ,RestDay
    ,IsEncodedByDay
) SELECT NEW.OrganizationID
    ,NEW.CreatedBy
    ,NEW.EmployeeID
    ,NEW.ShiftID
    ,d.DateValue
    ,ADDDATE(d.DateValue, INTERVAL date_diff DAY)
    ,NEW.NightShift
    ,NEW.RestDay
    ,NEW.IsEncodedByDay
    FROM dates d
    WHERE DAYOFWEEK(d.DateValue)=DAYOFWEEK(NEW.EffectiveFrom) AND d.DateValue BETWEEN NEW.EffectiveFrom AND lastdateof_default_week_format
    ORDER BY d.DateValue
ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=NEW.CreatedBy
    ,ShiftID=NEW.ShiftID;



END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
