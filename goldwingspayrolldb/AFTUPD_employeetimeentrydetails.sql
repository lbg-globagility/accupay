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

-- Dumping structure for trigger goldwingspayrolldb.AFTUPD_employeetimeentrydetails
DROP TRIGGER IF EXISTS `AFTUPD_employeetimeentrydetails`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_employeetimeentrydetails` AFTER UPDATE ON `employeetimeentrydetails` FOR EACH ROW BEGIN

DECLARE emp_group_name VARCHAR(50);

DECLARE anyint INT(11);
DECLARE sh_timefrom TIME;
DECLARE sh_timeto TIME;
DECLARE today_timefrom DATETIME;
DECLARE today_timeto DATETIME;
DECLARE tomorrow_timefrom DATETIME DEFAULT NULL;
DECLARE tomorrow_timeto DATETIME DEFAULT NULL;
DECLARE isShiftRestDay CHAR(1);
DECLARE today_timein DATETIME;
DECLARE today_timeout DATETIME;
DECLARE empOTRowID INT(11);

DECLARE anycomment VARCHAR(200);

DECLARE au_ViewID INT(11);

SELECT d.AutomaticOvertimeFiling
FROM employee e
INNER JOIN position p ON p.RowID=e.PositionID
INNER JOIN `division` d ON d.RowID=p.DivisionId AND d.ParentDivisionID IS NOT NULL
WHERE e.RowID=NEW.EmployeeID
INTO emp_group_name;

SET today_timein = ADDTIME(TIMESTAMP(NEW.`Date`), NEW.TimeIn);

SET today_timeout = ADDTIME(IF(NEW.TimeOut > NEW.TimeIn AND TIME_FORMAT(NEW.TimeIn,'%p') != TIME_FORMAT(NEW.TimeOut,'%p'), TIMESTAMP(NEW.`Date`), TIMESTAMP(ADDDATE(NEW.`Date`, INTERVAL 1 DAY))), NEW.TimeOut);

IF emp_group_name = '1' THEN
	SET @min_ot_time = CURTIME();
	SELECT sh.TimeFrom
	,sh.TimeTo
	,esh.RestDay
	,ADDTIME(TIMESTAMP(NEW.`Date`), sh.TimeFrom)
	,ADDTIME(IF(sh.TimeTo > sh.TimeFrom, TIMESTAMP(NEW.`Date`), TIMESTAMP(ADDDATE(NEW.`Date`, INTERVAL 1 DAY))), sh.TimeTo)
	,IFNULL(e.MinimumOvertime, CAST(0 AS TIME))
	FROM employeeshift esh
	INNER JOIN employee e ON e.RowID=esh.EmployeeID AND e.OrganizationID=esh.OrganizationID
	INNER JOIN shift sh ON sh.RowID=esh.ShiftID
	WHERE esh.EmployeeID=NEW.EmployeeID
	AND esh.OrganizationID=NEW.OrganizationID
	AND NEW.`Date` BETWEEN esh.EffectiveFrom AND esh.EffectiveTo LIMIT 1
	INTO sh_timefrom
			,sh_timeto
			,isShiftRestDay,today_timefrom,today_timeto, @min_ot_time;
	
	SET tomorrow_timefrom = TIMESTAMP(TIMESTAMPADD(HOUR,24,today_timefrom));
	SET tomorrow_timeto = TIMESTAMP(TIMESTAMPADD(HOUR,24,today_timeto));
	IF isShiftRestDay IS NOT NULL THEN
	
		IF isShiftRestDay = '0' THEN
		
			SELECT RowID
			FROM employeeovertime
			WHERE EmployeeID=NEW.EmployeeID
			AND OrganizationID=NEW.OrganizationID
			AND OTStatus='Approved'
			AND NEW.`Date` BETWEEN OTEndDate AND OTEndDate
			LIMIT 1
			INTO empOTRowID;
			
			IF TIMESTAMP(today_timeout) BETWEEN ADDTIME(TIMESTAMP(today_timeto), @min_ot_time) AND TIMESTAMP(TIMESTAMPADD(SECOND, -1, tomorrow_timefrom)) THEN
			
				SET anycomment = 'checking the employees time out here if it is in at least 15 minutes';
			
				IF empOTRowID IS NOT NULL THEN
					
					UPDATE employeeovertime
					SET OTStatus='Approved', OTStartTime = ADDTIME(sh_timeto,'00:00:01'), OTEndTime = NEW.TimeOut
					,Comments=''
					,LastUpd=CURRENT_TIMESTAMP()
					,LastUpdBy=NEW.LastUpdBy
					WHERE RowID=empOTRowID
					AND OTStatus='Approved';
				
				ELSE
				
					SELECT INSUPD_employeeOT(NULL,NEW.OrganizationID,NEW.CreatedBy,NEW.CreatedBy,NEW.EmployeeID,'Overtime',ADDTIME(sh_timeto,'00:00:01'),NEW.TimeOut,NEW.`Date`,NEW.`Date`,'Approved','',today_timeout,NULL) INTO anyint;
					
				END IF;
					
			ELSEIF TIMESTAMP(today_timeout) BETWEEN TIMESTAMP(SUBTIME(today_timeto,'00:15:59')) AND TIMESTAMP(today_timeto) THEN
				SET anycomment = 'employees time out here seems an under time';
				
				IF empOTRowID IS NOT NULL THEN
					
					UPDATE employeeovertime
					SET OTStatus='Pending'
					,LastUpd=CURRENT_TIMESTAMP()
					,LastUpdBy=NEW.LastUpdBy
					WHERE RowID=empOTRowID
					AND OTStatus='Approved';
				
				END IF;
				
			END IF;
			
			
			
			IF TIME_FORMAT(sh_timeto,'%p')='PM' THEN
				
				IF NEW.TimeStampIn < ADDTIME(TIMESTAMP(NEW.`Date`), SUBTIME(sh_timefrom, @min_ot_time)) THEN
					
					SELECT 0 INTO anyint;
				
				ELSE
				
					DELETE FROM employeeovertime WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND (NEW.`Date` BETWEEN OTStartDate AND OTEndDate) AND (OTStartTime BETWEEN TIME_FORMAT(NEW.TimeStampIn,@@time_format) AND SUBTIME(sh_timefrom,'00:00:01'));
				END IF;
			
			
			END IF;
				
			
			
		END IF;
		
	END IF;
	
	UPDATE employeeovertime SET OTStartTime = ADDTIME(sh_timeto,'00:00:01'), OTEndTime = NEW.TimeOut, LastUpd=IFNULL(ADDTIME(LastUpd, '00:00:01'),CURRENT_TIMESTAMP()), LastUpdBy=NEW.CreatedBy WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND NEW.`Date` BETWEEN OTStartDate AND OTEndDate AND sh_timeto IS NOT NULL;
		
END IF;
	
	SELECT RowID
	FROM `view`
	WHERE ViewName='Employee Time Entry Logs'
	AND OrganizationID=NEW.OrganizationID
	LIMIT 1
	INTO au_ViewID;

	SELECT INS_audittrail_RETRowID(NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,au_ViewID,'TimeIn',NEW.RowID,OLD.TimeIn,NEW.TimeIn,'Update') INTO anyint;

	SELECT INS_audittrail_RETRowID(NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,au_ViewID,'TimeOut',NEW.RowID,OLD.TimeOut,NEW.TimeOut,'Update') INTO anyint;
	
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
