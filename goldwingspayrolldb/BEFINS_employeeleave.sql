/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFINS_employeeleave`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFINS_employeeleave` BEFORE INSERT ON `employeeleave` FOR EACH ROW BEGIN

DECLARE selected_leavebal DECIMAL(11,2) DEFAULT 0;

/*********************************************************
START METHOD `SET_OfficialValidHours_AND_OfficialValidDays`
*********************************************************/
IF NEW.`Status` = 'Approved' THEN
	
	SET @offcl_validdays = TIMESTAMPDIFF(DAY, NEW.LeaveStartDate, NEW.LeaveEndDate);
	
	IF @offcl_validdays <= 0 THEN
		SET @offcl_validdays = 1;
		
	END IF;
	
	SET NEW.OfficialValidDays = @offcl_validdays;

	SELECT i.`LeaveBalance`
	FROM (SELECT e.RowID,e.LeaveBalance FROM employee e WHERE e.RowID=NEW.EmployeeID AND NEW.LeaveType='Vacation leave'
			UNION
			SELECT e.RowID,e.SickLeaveBalance `LeaveBalance` FROM employee e WHERE e.RowID=NEW.EmployeeID AND NEW.LeaveType='Sick leave'
			UNION
			SELECT e.RowID,e.OtherLeaveBalance `LeaveBalance` FROM employee e WHERE e.RowID=NEW.EmployeeID AND NEW.LeaveType='Others leave'
			UNION
			SELECT e.RowID,e.MaternityLeaveBalance `LeaveBalance` FROM employee e WHERE e.RowID=NEW.EmployeeID AND LOCATE('aternity', NEW.LeaveType) > 0
			) i
	INTO selected_leavebal;
	
	SET @offcl_validhrs = 0.0;
	
	SET @min_perhour = 60; SET @sec_permin = 60;
	
	SET @appropriate_end_date = CURDATE();
	
	IF IS_TIMERANGE_REACHTOMORROW(NEW.LeaveStartTime, NEW.LeaveEndTime) = TRUE THEN
		SET @appropriate_end_date = ADDDATE(NEW.LeaveStartDate, INTERVAL 1 DAY);
		
	ELSE
		SET @appropriate_end_date = NEW.LeaveStartDate;
		
	END IF;

	SET @offcl_validhrs =	(TIMESTAMPDIFF(SECOND
														,CONCAT_DATETIME(NEW.LeaveStartDate, NEW.LeaveStartTime)
														,CONCAT_DATETIME(@appropriate_end_date, NEW.LeaveEndTime))
									/ (@min_perhour * @sec_permin));
	
	SET @shift_rowid = NULL;
	
	IF @offcl_validdays > 1 THEN
	
		SELECT sh.RowID
		FROM employeeshift esh
		INNER JOIN shift sh ON sh.RowID=esh.ShiftID
		WHERE esh.EmployeeID=NEW.EmployeeID
		AND esh.OrganizationID=NEW.OrganizationID
		AND (esh.EffectiveFrom <= NEW.LeaveStartDate OR esh.EffectiveFrom <= NEW.LeaveEndDate)
		AND (NEW.LeaveStartDate <= esh.EffectiveTo OR NEW.LeaveEndDate <= esh.EffectiveTo)
		LIMIT 1
		INTO @shift_rowid;
	
	ELSEIF @offcl_validdays = 1 THEN
	
		SELECT sh.RowID
		FROM employeeshift esh
		INNER JOIN shift sh ON sh.RowID=esh.ShiftID
		WHERE esh.EmployeeID=NEW.EmployeeID
		AND esh.OrganizationID=NEW.OrganizationID
		AND NEW.LeaveStartDate BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
		INTO @shift_rowid;
	
	END IF;
	
	SET @break_hrs = NULL;
	
	SELECT IF(IS_TIMERANGE_REACHTOMORROW(sh.BreakTimeFrom, sh.BreakTimeTo)
				, (TIMESTAMPDIFF(SECOND
										,CONCAT_DATETIME(CURDATE(), sh.BreakTimeFrom)
										,CONCAT_DATETIME(ADDDATE(CURDATE(), INTERVAL 1 DAY), sh.BreakTimeTo))
					/ (@min_perhour * @sec_permin))
				, COMPUTE_TimeDifference(sh.BreakTimeFrom, sh.BreakTimeTo)) `Result`
	FROM shift sh
	WHERE sh.RowID=@shift_rowid
	AND sh.TimeFrom=NEW.LeaveStartTime
	AND sh.TimeTo	=NEW.LeaveEndTime
	INTO @break_hrs;
	
	SET @break_hrs = IFNULL(@break_hrs, 0);
	
	SET @validhrs_multip_validdays = (@offcl_validhrs - @break_hrs) * @offcl_validdays;
	
	IF (selected_leavebal - @validhrs_multip_validdays) < 0 THEN
		SET @validhrs_multip_validdays = @validhrs_multip_validdays + (selected_leavebal - @validhrs_multip_validdays);
		
	END IF;
	
	# SET NEW.OfficialValidHours = @validhrs_multip_validdays;
	SET NEW.OfficialValidHours = (@offcl_validhrs - @break_hrs);
	
ELSE

	SET NEW.OfficialValidHours = 0;

	SET NEW.OfficialValidDays = 0;

END IF;
/*********************************************************
END METHOD `SET_OfficialValidHours_AND_OfficialValidDays`
*********************************************************/

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
