/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `EmployeeShiftsImportToShiftSchedules`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `EmployeeShiftsImportToShiftSchedules`()
BEGIN

DECLARE minutesPerHour INT(11) DEFAULT 3600;

SET @startBreak = NULL;
SET @endBreak = NULL;
SET @breakDuration = 0;

INSERT INTO `shiftschedules` (`OrganizationID`, `Created`, `LastUpd`, `EmployeeID`, `Date`, `StartTime`, `EndTime`, `BreakStartTime`, `BreakLength`, `IsRestDay`, `ShiftHours`, `WorkHours`)
SELECT
ii.OrganizationID
, CURRENT_TIMESTAMP()
, CURRENT_TIMESTAMP()
, ii.EmployeeID
, ii.DateValue
, ii.StartTime
, ii.EndTime
, ii.BreakStartTime
, IFNULL(ii.BreakLength, 0)
, ii.IsRestDay
, IFNULL(ii.ShiftHours, 0)
, IFNULL(ii.WorkHours, 0)
FROM (SELECT
		i.OrganizationID
		, i.EmployeeID
		, d.DateValue
		, i.TimeFrom `StartTime`
		, i.TimeTo `EndTime`
		, i.BreakTimeFrom #`BreakStartTime`
		
		, i.RestDay `IsRestDay`
		, i.ShiftHours
		
		, @startBreak := ADDTIME(TIMESTAMP(d.DateValue), i.BreakTimeFrom) `Result`
		, @endBreak := GetNextStartDateTime(ADDTIME(TIMESTAMP(d.DateValue), i.BreakTimeFrom), i.BreakTimeTo) `Result1`
		, @breakDuration := 
		  IF(i.BreakTimeFrom IS NULL
		     , 0
			  , (TIMESTAMPDIFF(SECOND, @startBreak, @endBreak) / minutesPerHour)) `BreakLength`
		
		, IF(@breakDuration = 0, NULL, i.BreakTimeFrom) `BreakStartTime`
		, i.WorkHours
		FROM (SELECT esh.*
				, sh.TimeFrom, sh.TimeTo, sh.BreakTimeFrom, sh.BreakTimeTo, sh.ShiftHours, sh.WorkHours
				FROM employeeshift esh
				LEFT JOIN shift sh ON sh.RowID=esh.ShiftID
				) i
		INNER JOIN dates d ON d.DateValue BETWEEN i.EffectiveFrom AND i.EffectiveTo
		ORDER BY i.EmployeeID, d.DateValue
		) ii

WHERE NOT (ii.StartTime IS NULL AND ii.IsRestDay = FALSE)
ON DUPLICATE KEY UPDATE LastUpd = CURRENT_TIMESTAMP()
, `StartTime` = ii.StartTime
, `EndTime` = ii.EndTime
, `BreakStartTime` = ii.BreakStartTime
, `BreakLength` = IFNULL(ii.BreakLength, 0)
, `IsRestDay` = ii.IsRestDay
, `ShiftHours` = IFNULL(ii.ShiftHours, 0)
, `WorkHours` = IFNULL(ii.WorkHours, 0)
;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
