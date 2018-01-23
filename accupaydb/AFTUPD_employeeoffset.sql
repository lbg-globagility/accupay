/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTUPD_employeeoffset`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_employeeoffset` AFTER UPDATE ON `employeeoffset` FOR EACH ROW BEGIN

DECLARE tothoursoffset DECIMAL(11,6);

DECLARE prev_offset_hours DECIMAL(11,6);

DECLARE sec_per_hour INT(11) DEFAULT 3600; # 60 seconds times 60 minutes

DECLARE bef_num_of_days, aft_num_of_days DECIMAL(11,6);

SET prev_offset_hours = TIMESTAMPDIFF(SECOND
                               , CONCAT_DATETIME(OLD.StartDate, OLD.StartTime)
										 , CONCAT_DATETIME(ADDDATE(OLD.StartDate, INTERVAL IS_TIMERANGE_REACHTOMORROW(OLD.StartTime, OLD.EndTime) DAY), OLD.EndTime)) / sec_per_hour;

SET tothoursoffset = TIMESTAMPDIFF(SECOND
                               , CONCAT_DATETIME(NEW.StartDate, NEW.StartTime)
										 , CONCAT_DATETIME(ADDDATE(NEW.StartDate, INTERVAL IS_TIMERANGE_REACHTOMORROW(NEW.StartTime, NEW.EndTime) DAY), NEW.EndTime)) / sec_per_hour;

SET bef_num_of_days = (DATEDIFF(OLD.EndDate, OLD.StartDate) + 1);

IF bef_num_of_days < 0 THEN SET bef_num_of_days=1; END IF;


SET aft_num_of_days = (DATEDIFF(NEW.EndDate, NEW.StartDate) + 1);

IF aft_num_of_days < 0 THEN SET aft_num_of_days=1; END IF;

UPDATE employee e
SET e.OffsetBalance = ( IFNULL(e.OffsetBalance,0) + ( ((IFNULL(tothoursoffset, 0) * aft_num_of_days) - (IFNULL(prev_offset_hours, 0) * bef_num_of_days)) ) )
,e.LastUpdBy=NEW.CreatedBy
WHERE e.RowID = NEW.EmployeeID
AND NEW.`Status` = 'Approved';

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
