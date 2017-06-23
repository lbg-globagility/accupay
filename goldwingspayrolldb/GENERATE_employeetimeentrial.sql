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

-- Dumping structure for function goldwingspayrolldb.GENERATE_employeetimeentrial
DROP FUNCTION IF EXISTS `GENERATE_employeetimeentrial`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GENERATE_employeetimeentrial`(`ete_EmpRowID` INT, `ete_OrganizID` INT, `ete_Date` DATE, `ete_UserRowID` INT) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11);


DECLARE pr_DayBefore DATE;

DECLARE pr_PayType TEXT;

DECLARE isRestDay TEXT;

DECLARE hasTimeLogs TEXT;


DECLARE yester_TotDayPay DECIMAL(11,2);

DECLARE yester_TotHrsWorkd DECIMAL(11,2);


DECLARE ete_RegHrsWorkd DECIMAL(11,6);

DECLARE ete_HrsLate DECIMAL(11,6);

DECLARE ete_HrsUnder DECIMAL(11,6);

DECLARE ete_OvertimeHrs DECIMAL(11,6);

DECLARE ete_NDiffHrs DECIMAL(11,6);

DECLARE ete_NDiffOTHrs DECIMAL(11,6);


DECLARE etd_TimeIn TIME;

DECLARE etd_TimeOut TIME;


DECLARE shifttimefrom TIME;

DECLARE shifttimeto TIME;


DECLARE otstartingtime TIME DEFAULT NULL;

DECLARE otendingtime TIME DEFAULT NULL;


DECLARE e_EmpStatus TEXT;

DECLARE e_EmpType TEXT;

DECLARE e_MaritStatus TEXT;

DECLARE e_StartDate DATE;

DECLARE e_PayFreqID INT(11);

DECLARE e_NumDependent INT(11);

DECLARE e_UTOverride CHAR(1);

DECLARE e_OTOverride CHAR(1);

DECLARE e_DaysPerYear INT(11);

DECLARE calc_Holiday CHAR(1);

DECLARE e_CalcSpecialHoliday CHAR(1);

DECLARE e_CalcNightDiff CHAR(1);

DECLARE e_CalcNightDiffOT CHAR(1);

DECLARE e_CalcRestDay CHAR(1);

DECLARE e_CalcRestDayOT CHAR(1);


DECLARE yes_true CHAR(1) DEFAULT '0';

DECLARE anytime TIME;

DECLARE anyINT INT(11);


DECLARE rateperhour DECIMAL(11,6);

DECLARE dailypay DECIMAL(11,6);



DECLARE commonrate DECIMAL(11,6);

DECLARE otrate DECIMAL(11,6);

DECLARE ndiffrate DECIMAL(11,6);

DECLARE ndiffotrate DECIMAL(11,6);

DECLARE restday_rate DECIMAL(11,6);

DECLARE restdayot_rate DECIMAL(11,6);


DECLARE eshRowID INT(11);

DECLARE esalRowID INT(11);

DECLARE payrateRowID INT(11);

DECLARE ete_TotalDayPay DECIMAL(11,6);


DECLARE hasLeave CHAR(1) DEFAULT '0';

DECLARE OTCount INT(11) DEFAULT 0;

DECLARE aftershiftOTRowID INT(11) DEFAULT 0;

DECLARE anotherOTHours DECIMAL(11,6);


SELECT
e.EmploymentStatus
,e.EmployeeType
,e.MaritalStatus
,e.StartDate
,e.PayFrequencyID
,e.NoOfDependents
,e.UndertimeOverride
,e.OvertimeOverride
,e.WorkDaysPerYear
,e.CalcHoliday
,e.CalcSpecialHoliday
,e.CalcNightDiff
,e.CalcNightDiffOT
,e.CalcRestDay
,e.CalcRestDayOT
FROM employee e
WHERE e.RowID=ete_EmpRowID
INTO e_EmpStatus
		,e_EmpType
		,e_MaritStatus
		,e_StartDate
		,e_PayFreqID
		,e_NumDependent
		,e_UTOverride
		,e_OTOverride
		,e_DaysPerYear
		,calc_Holiday
		,e_CalcSpecialHoliday
		,e_CalcNightDiff
		,e_CalcNightDiffOT
		,e_CalcRestDay
		,e_CalcRestDayOT;



SELECT
	RowID
	,IF(PayType = 'Special Non-Working Holiday', IF(e_CalcSpecialHoliday = 'Y', PayRate, 1), IF(PayType = 'Regular Holiday', IF(calc_Holiday = 'Y', PayRate, 1), PayRate))
	,OvertimeRate
	,IF(e_CalcNightDiff = 'Y', NightDifferentialRate, 1)
	,IF(e_CalcNightDiffOT = 'Y', NightDifferentialOTRate, 1)
	,IF(e_CalcRestDay = 'Y', RestDayRate, 1)
	,IF(e_CalcRestDayOT = 'Y', RestDayOvertimeRate, 1)
	,DayBefore
	,PayType
FROM payrate
WHERE `Date`=ete_Date
AND OrganizationID=ete_OrganizID
INTO  payrateRowID
		,commonrate
		,otrate
		,ndiffrate
		,ndiffotrate
		,restday_rate
		,restdayot_rate
		,pr_DayBefore
		,pr_PayType;


SELECT IFNULL(RestDay,'0')
FROM employeeshift
WHERE EmployeeID=ete_EmpRowID
AND OrganizationID=ete_OrganizID
AND ete_Date BETWEEN EffectiveFrom AND EffectiveTo
AND DATEDIFF(ete_Date,EffectiveFrom) >= 0 AND COALESCE(RestDay,0)='1'
ORDER BY DATEDIFF(ete_Date,EffectiveFrom)
LIMIT 1 INTO isRestDay;


SET ete_HrsLate = 0.0;

SET ete_HrsUnder = 0.0;

SET ete_OvertimeHrs = 0.0;

SET ete_NDiffHrs = 0.0;

SET ete_NDiffOTHrs = 0.0;


IF isRestDay IS NULL THEN
	
	SELECT (DAYOFWEEK(ete_Date) = e.DayOfRest)
	FROM employee e
	WHERE e.RowID=ete_EmpRowID
	INTO isRestDay;

END IF;


SELECT
COUNT(RowID)
FROM employeeovertime
WHERE EmployeeID=ete_EmpRowID
AND OrganizationID=ete_OrganizID
AND ete_Date
BETWEEN OTStartDate
AND OTEndDate
AND OTStatus='Approved'
INTO OTCount;





SELECT
sh.TimeFrom
,sh.TimeTo
,sh.RowID
FROM employeeshift esh
INNER JOIN shift sh ON sh.RowID=esh.ShiftID
WHERE esh.EmployeeID=ete_EmpRowID
AND esh.OrganizationID=ete_OrganizID
AND ete_Date BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
ORDER BY DATEDIFF(ete_Date, esh.EffectiveFrom) DESC
LIMIT 1
INTO shifttimefrom
	  ,shifttimeto
	  ,eshRowID;
	  
SET yes_true = '1';

IF OTCount = 1 THEN
	
	SELECT
	OTStartTime
	,OTEndTime
	,RowID
	FROM employeeovertime
	WHERE EmployeeID=ete_EmpRowID
	AND OrganizationID=ete_OrganizID
	AND OTStartTime >= shifttimeto
	AND OTStatus='Approved'
	AND (ete_Date BETWEEN OTStartDate AND OTEndDate)
	ORDER BY OTStartTime DESC
	LIMIT 1
	INTO otstartingtime
		  ,otendingtime
		  ,aftershiftOTRowID;

ELSE
	
	SELECT
	OTStartTime
	,OTEndTime
	,RowID
	FROM employeeovertime
	WHERE EmployeeID=ete_EmpRowID
	AND OrganizationID=ete_OrganizID
	AND ete_Date
	BETWEEN OTStartDate
	AND COALESCE(OTEndDate,OTStartDate)
	AND OTStatus='Approved'
	ORDER BY OTStartTime DESC
	LIMIT 1
	INTO otstartingtime
		  ,otendingtime
		  ,aftershiftOTRowID;


END IF;



IF isRestDay = '1' THEN 
	
	SELECT
	TimeIn
	,TimeOut
	FROM employeetimeentrydetails
	WHERE EmployeeID=ete_EmpRowID
	AND OrganizationID=ete_OrganizID
	AND `Date`=ete_Date
	LIMIT 1
	INTO etd_TimeIn
	     ,etd_TimeOut; 

	SELECT COMPUTE_TimeDifference(etd_TimeIn,etd_TimeOut)
	INTO ete_RegHrsWorkd;
	
	SET ete_HrsLate = 0.0;
	
	SET ete_HrsUnder = 0.0;

	IF otstartingtime IS NOT NULL
		AND otstartingtime IS NOT NULL THEN 
		
		SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
		INTO ete_OvertimeHrs;
		
	ELSE 
		
		SET ete_OvertimeHrs = 0.0;
		
	END IF;
	
	SET ete_NDiffHrs = 0.0;
	
	SET ete_NDiffOTHrs = 0.0;
	
ELSE 
 		
	SELECT
	TimeIn
	,TimeOut
	FROM employeetimeentrydetails
	WHERE EmployeeID=ete_EmpRowID
	AND OrganizationID=ete_OrganizID
	AND `Date`=ete_Date
	LIMIT 1
	INTO etd_TimeIn
	     ,etd_TimeOut;
	
	SELECT GRACE_PERIOD(etd_TimeIn, shifttimefrom)
	INTO etd_TimeIn;
	
	IF otstartingtime IS NULL
		AND otstartingtime IS NULL THEN 
	
		IF IF(HOUR(etd_TimeOut) = 00, ADDTIME(etd_TimeOut,'24:00'), etd_TimeOut) > shifttimeto THEN
	
			SELECT COMPUTE_TimeDifference(etd_TimeIn,shifttimeto)
			INTO ete_RegHrsWorkd;
			
			SET etd_TimeOut = shifttimeto;
			
		ELSE
		
			SELECT COMPUTE_TimeDifference(etd_TimeIn,etd_TimeOut)
			INTO ete_RegHrsWorkd;
			
		END IF;
		
		SET otstartingtime = '12:00:00'; 
		
		SET otendingtime = '12:00:00'; 

		SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
		INTO ete_OvertimeHrs;
			
	ELSE 
	
		SELECT COMPUTE_TimeDifference(otstartingtime, etd_TimeOut)
		INTO ete_OvertimeHrs;

		IF TIME_FORMAT(otstartingtime,'%p') = 'PM'
			AND TIME_FORMAT(otendingtime,'%p') = 'AM'
			AND TIME_FORMAT(etd_TimeOut,'%p') = 'AM' THEN 
	
			SELECT COMPUTE_TimeDifference(etd_TimeIn,shifttimeto)
			INTO ete_RegHrsWorkd;
		
			IF ADDTIME(etd_TimeOut,'24:00') BETWEEN otstartingtime AND ADDTIME(otendingtime,'24:00') THEN
			
				SELECT COMPUTE_TimeDifference(otstartingtime, etd_TimeOut)
				INTO ete_OvertimeHrs;
				
				SET etd_TimeOut = SUBTIME(otstartingtime, '00:01');
				
			ELSEIF etd_TimeOut > otendingtime THEN
			
				SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
				INTO ete_OvertimeHrs;
		
				SET etd_TimeOut = SUBTIME(otstartingtime,'00:01:00');
				
			ELSE
			
				SELECT COMPUTE_TimeDifference(otstartingtime, etd_TimeOut)
				INTO ete_OvertimeHrs;
		
				SET ete_OvertimeHrs = ete_OvertimeHrs - COMPUTE_TimeDifference(otendingtime,etd_TimeOut);
				
				SET etd_TimeOut = SUBTIME(otstartingtime,'00:01:00');
				
			END IF;
		
		ELSEIF TIME_FORMAT(otstartingtime,'%p') = 'PM'
				 AND TIME_FORMAT(otendingtime,'%p') = 'AM'
				 AND TIME_FORMAT(etd_TimeOut,'%p') = 'PM' THEN

			SELECT COMPUTE_TimeDifference(etd_TimeIn,shifttimeto)
			INTO ete_RegHrsWorkd;
			
			IF etd_TimeOut BETWEEN otstartingtime AND ADDTIME(otendingtime,'24:00') THEN
			
				SELECT COMPUTE_TimeDifference(etd_TimeIn,SUBTIME(otstartingtime,'00:01'))
				INTO ete_RegHrsWorkd;
				
			ELSEIF etd_TimeOut < shifttimeto THEN
			
				SELECT COMPUTE_TimeDifference(etd_TimeIn,etd_TimeOut)
				INTO ete_RegHrsWorkd;
				
				SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
				INTO ete_OvertimeHrs;
		
			ELSE
			
				SELECT COMPUTE_TimeDifference(etd_TimeIn,etd_TimeOut)
				INTO ete_RegHrsWorkd;
				
			END IF;
			
			
				
			
			
		ELSEIF TIME_FORMAT(otstartingtime,'%p') = 'AM'
				 AND TIME_FORMAT(otendingtime,'%p') = 'PM'
				 AND TIME_FORMAT(etd_TimeOut,'%p') = 'PM' THEN
		
			SELECT COMPUTE_TimeDifference(etd_TimeIn, IF(etd_TimeOut > shifttimeto, shifttimeto, etd_TimeOut))
			INTO ete_RegHrsWorkd; 
			
			SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
			INTO ete_OvertimeHrs;
	
		ELSEIF TIME_FORMAT(otstartingtime,'%p') = 'AM'
				 AND TIME_FORMAT(otendingtime,'%p') = 'PM'
				 AND TIME_FORMAT(etd_TimeOut,'%p') = 'AM' THEN

			SELECT COMPUTE_TimeDifference(etd_TimeIn, IF(etd_TimeOut > shifttimeto, shifttimeto, etd_TimeOut))
			INTO ete_RegHrsWorkd;
			
			IF (etd_TimeIn < otstartingtime AND etd_TimeIn < otendingtime) THEN
			
				SET ete_OvertimeHrs = 0;
		
			ELSEIF etd_TimeOut BETWEEN shifttimeto AND otendingtime THEN
			
				SELECT COMPUTE_TimeDifference(otstartingtime, etd_TimeOut)
				INTO ete_OvertimeHrs;
		
			ELSEIF etd_TimeOut > otendingtime THEN
		
				SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
				INTO ete_OvertimeHrs;
		
			END IF;
				
		ELSEIF TIME_FORMAT(otstartingtime,'%p') = 'PM'
				 AND TIME_FORMAT(otendingtime,'%p') = 'PM'
				 AND TIME_FORMAT(etd_TimeOut,'%p') = 'AM' THEN

			SELECT COMPUTE_TimeDifference(etd_TimeIn, IF(etd_TimeOut > shifttimeto, shifttimeto, etd_TimeOut))
			INTO ete_RegHrsWorkd;
			
			IF DATE_FORMAT(etd_TimeOut, '%H') = '00' THEN
			
				IF (DATE_FORMAT(etd_TimeOut, '24:%i:%s') > otstartingtime AND DATE_FORMAT(etd_TimeOut, '24:%i:%s') > otendingtime) THEN 
				
					SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
					INTO ete_OvertimeHrs;
			
					SET etd_TimeOut = SUBTIME(otstartingtime,'00:01:00');
							
					SELECT COMPUTE_TimeDifference(etd_TimeIn, etd_TimeOut)
					INTO ete_RegHrsWorkd;
					
				ELSE
					SET ete_OvertimeHrs = 0.0;
					
				END IF;
			
			ELSE
		
				IF (etd_TimeOut > otstartingtime AND etd_TimeOut > otendingtime) THEN 
				
					SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
					INTO ete_OvertimeHrs;
			
					SET etd_TimeOut = SUBTIME(otstartingtime,'00:01:00');
							
					SELECT COMPUTE_TimeDifference(etd_TimeIn, etd_TimeOut)
					INTO ete_RegHrsWorkd;
					
				ELSE
					SET ete_OvertimeHrs = 0.0;
					
					SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
					INTO ete_OvertimeHrs;
			
				END IF;
			
			END IF;
										
		ELSE
		
			IF TIME_FORMAT(otstartingtime,'%p') = 'PM'
					 AND TIME_FORMAT(otendingtime,'%p') = 'AM' THEN
				
				IF etd_TimeOut BETWEEN otstartingtime AND ADDTIME(otendingtime,'24:00') THEN
				
					SELECT COMPUTE_TimeDifference(etd_TimeIn,SUBTIME(otstartingtime,'00:01:00'))
					INTO ete_RegHrsWorkd;
			
					IF COMPUTE_TimeDifference(otendingtime, etd_TimeOut) > 0 THEN
					
						SELECT COMPUTE_TimeDifference(otstartingtime, etd_TimeOut)
						INTO ete_OvertimeHrs;
						
					ELSE
						
						SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
						INTO ete_OvertimeHrs;
						
					END IF;
			
				ELSE
					
					IF etd_TimeOut > shifttimeto THEN
						
						SELECT COMPUTE_TimeDifference(etd_TimeIn,shifttimeto)
						INTO ete_RegHrsWorkd;
						
					ELSE
					
						SELECT COMPUTE_TimeDifference(etd_TimeIn,etd_TimeOut)
						INTO ete_RegHrsWorkd;
						
					END IF;
					
				END IF;
				
			ELSE
			
				IF etd_TimeOut BETWEEN otstartingtime AND otendingtime THEN
				
					SELECT COMPUTE_TimeDifference(etd_TimeIn,SUBTIME(otstartingtime,'00:01:00'))
					INTO ete_RegHrsWorkd;
				
					SELECT COMPUTE_TimeDifference(otstartingtime, etd_TimeOut)
					INTO ete_OvertimeHrs;
				
					SET etd_TimeOut = shifttimeto;
				
				ELSE
				
					IF shifttimefrom > otendingtime THEN
						
						SELECT COMPUTE_TimeDifference(etd_TimeIn,etd_TimeOut)
						INTO ete_RegHrsWorkd; 
						
						SET ete_RegHrsWorkd = ete_RegHrsWorkd - COMPUTE_TimeDifference(shifttimeto,etd_TimeOut);
						
						SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
						INTO ete_OvertimeHrs;
					
					ELSEIF etd_TimeOut < otstartingtime THEN
					
						SELECT COMPUTE_TimeDifference(etd_TimeIn,etd_TimeOut)
						INTO ete_RegHrsWorkd;
						
						SET ete_OvertimeHrs = 0;
						
					ELSE
				
						SELECT COMPUTE_TimeDifference(etd_TimeIn,shifttimeto)
						INTO ete_RegHrsWorkd;
						
						
						SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
						INTO ete_OvertimeHrs;
						
						SET etd_TimeOut = SUBTIME(otstartingtime, '00:01:00');
					
					END IF;
					
				END IF;
				
			END IF;
			
		END IF;
			
	END IF;

	IF etd_TimeIn > shifttimefrom THEN
	
		SELECT COMPUTE_TimeDifference(shifttimefrom, etd_TimeIn)
		INTO ete_HrsLate;

	ELSE
	
		SELECT COMPUTE_TimeDifference(etd_TimeIn, shifttimefrom)
		INTO ete_HrsLate;

	END IF;
		
	SELECT COMPUTE_TimeDifference(etd_TimeOut, shifttimeto)
	INTO ete_HrsUnder;
	
	SET ete_NDiffHrs = 0.0;
	
	SET ete_NDiffOTHrs = 0.0;
	
END IF;


SELECT GET_employeerateperday(ete_EmpRowID, ete_OrganizID, ete_Date)
INTO dailypay;

SET rateperhour = COMPUTE_TimeDifference(shifttimefrom, shifttimeto);

IF rateperhour IS NULL THEN
	SET rateperhour = 9;
	
END IF;

IF rateperhour > 4 THEN
	SET rateperhour = rateperhour - 1;

ELSE 
	SET rateperhour = 8;
	
END IF;

SET rateperhour = dailypay / rateperhour;






SELECT
RowID
FROM employeetimeentry
WHERE EmployeeID=ete_EmpRowID
AND OrganizationID=ete_OrganizID
AND `Date`=ete_Date
LIMIT 1
INTO anyINT;


SELECT
RowID
FROM employeesalary
WHERE EmployeeID=ete_EmpRowID
AND OrganizationID=ete_OrganizID
AND ete_Date BETWEEN DATE(COALESCE(EffectiveDateFrom, DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveDateTo,ete_Date))
AND DATEDIFF(ete_Date,EffectiveDateFrom) >= 0
ORDER BY DATEDIFF(DATE_FORMAT(ete_Date,'%Y-%m-%d'),EffectiveDateFrom)
LIMIT 1
INTO esalRowID;



IF ete_RegHrsWorkd IS NULL THEN
	SET ete_RegHrsWorkd = 0;
END IF;


IF (ete_RegHrsWorkd > 4 AND ete_RegHrsWorkd < 5) AND COMPUTE_TimeDifference(shifttimefrom, shifttimeto) = 9 THEN
	SET ete_RegHrsWorkd = 4;
	
ELSEIF (ete_RegHrsWorkd > 5 AND ete_RegHrsWorkd < 6) AND COMPUTE_TimeDifference(shifttimefrom, shifttimeto) = 10 THEN
	SET ete_RegHrsWorkd = 5;
	
ELSEIF ete_RegHrsWorkd > 4 AND COMPUTE_TimeDifference(shifttimefrom, shifttimeto) = 9 THEN
	SET ete_RegHrsWorkd = ete_RegHrsWorkd - 1;
	
ELSEIF ete_RegHrsWorkd > 5 AND COMPUTE_TimeDifference(shifttimefrom, shifttimeto) = 10 THEN
	SET ete_RegHrsWorkd = ete_RegHrsWorkd - 1;
	
END IF;


IF ete_HrsLate IS NULL THEN
	SET ete_HrsLate = 0;
END IF;


IF ete_HrsLate > 4 AND COMPUTE_TimeDifference(shifttimefrom, shifttimeto) = 9 THEN
	SET ete_HrsLate = COMPUTE_TimeDifference(SUBTIME(shifttimeto,'04:00'), shifttimeto);

ELSEIF ete_HrsLate > 5 AND COMPUTE_TimeDifference(shifttimefrom, shifttimeto) = 10 THEN
	SET ete_HrsLate = COMPUTE_TimeDifference(SUBTIME(shifttimeto,'05:00'), shifttimeto);

END IF;


IF ete_HrsUnder IS NULL THEN
	SET ete_HrsUnder = 0;
END IF;

IF ete_OvertimeHrs IS NULL THEN
	SET ete_OvertimeHrs = 0;
END IF;

IF ete_NDiffHrs IS NULL THEN
	SET ete_NDiffHrs = 0;
END IF;

IF ete_NDiffOTHrs IS NULL THEN
	SET ete_NDiffOTHrs = 0;
END IF;



IF IFNULL(OTCount,0) > 1 THEN
	
	SELECT
	COMPUTE_TimeDifference(OTStartTime, OTEndTime)
	FROM employeeovertime
	WHERE EmployeeID=ete_EmpRowID
	AND OrganizationID=ete_OrganizID
	AND ete_Date
	BETWEEN OTStartDate
	AND COALESCE(OTEndDate,OTStartDate)
	AND OTStatus='Approved'
	AND RowID!=aftershiftOTRowID
	LIMIT 1
	INTO anotherOTHours;
	
	IF anotherOTHours IS NULL THEN
		SET anotherOTHours = 0.0;
		
	END IF;
	
	SET ete_OvertimeHrs = ete_OvertimeHrs + anotherOTHours;
	
ELSEIF IFNULL(OTCount,0) = 1 && ete_OvertimeHrs = 0 THEN

	SELECT
	COMPUTE_TimeDifference(OTStartTime, OTEndTime)
	FROM employeeovertime
	WHERE EmployeeID=ete_EmpRowID
	AND OrganizationID=ete_OrganizID
	AND OTStatus='Approved'
	AND ete_Date BETWEEN OTStartDate AND OTStartDate
	AND RowID!=aftershiftOTRowID
	LIMIT 1
	INTO anotherOTHours;
	
	IF anotherOTHours IS NULL THEN
		SET anotherOTHours = 0.0;
		
	END IF;
	
	SET ete_OvertimeHrs = ete_OvertimeHrs + anotherOTHours;
	
END IF;


IF pr_DayBefore IS NULL THEN 



	SELECT
	IFNULL(TotalDayPay,0)
	,IFNULL(TotalHoursWorked,0)
	FROM employeetimeentry
	WHERE EmployeeID=ete_EmpRowID
	AND OrganizationID=ete_OrganizID
	AND `Date`=ete_Date
	INTO yester_TotDayPay
		  ,yester_TotHrsWorkd; 

	IF yester_TotDayPay IS NULL THEN
		SET yester_TotDayPay = 0;
		
	END IF;
		
	IF ete_Date <= e_StartDate THEN 
		
		
				
		SET ete_TotalDayPay = 0.0;
									 
		SELECT INSUPD_employeetimeentries(
				anyINT
				, ete_OrganizID
				, ete_UserRowID
				, ete_UserRowID
				, ete_Date
				, eshRowID
				, ete_EmpRowID
				, esalRowID
				, '0'
				, ete_RegHrsWorkd
				, ete_OvertimeHrs
				, ete_HrsUnder
				, ete_NDiffHrs
				, ete_NDiffOTHrs
				, ete_HrsLate
				, payrateRowID
				, ete_TotalDayPay
				, 0
				, 0
				, 0
				, 0
				, 0
				, 0
				, 0
		) INTO anyINT;
		

	ELSEIF yester_TotDayPay = 0 THEN
	
		
		
		IF isRestDay = '1' THEN
			
				
			
				
			SET ete_TotalDayPay = ((ete_RegHrsWorkd * rateperhour) * commonrate)
										 + ((ete_OvertimeHrs * rateperhour) * otrate);
										 
			SELECT INSUPD_employeetimeentries(
					anyINT
					, ete_OrganizID
					, ete_UserRowID
					, ete_UserRowID
					, ete_Date
					, eshRowID
					, ete_EmpRowID
					, esalRowID
					, '0'
					, ete_RegHrsWorkd
					, ete_OvertimeHrs
					, ete_HrsUnder
					, ete_NDiffHrs
					, ete_NDiffOTHrs
					, ete_HrsLate
					, payrateRowID
					, ete_TotalDayPay
					, ete_RegHrsWorkd + ete_OvertimeHrs
					, (ete_RegHrsWorkd * rateperhour) * ((commonrate + restday_rate) - 1)
					, (ete_OvertimeHrs * rateperhour) * ((commonrate + restdayot_rate) - 1)
					, (ete_HrsUnder * rateperhour)
					, (ete_NDiffHrs * rateperhour) * ndiffrate
					, (ete_NDiffOTHrs * rateperhour) * ndiffotrate
					, (ete_HrsLate * rateperhour)
			) INTO anyINT;
			
			
		ELSEIF isRestDay = '0' THEN
			
SET yes_true = '0'; 
				
			
				SET ete_TotalDayPay = ((ete_RegHrsWorkd * rateperhour) * commonrate)
											 + ((ete_OvertimeHrs * rateperhour) * otrate);
			
										 
			SELECT INSUPD_employeetimeentries(
					anyINT
					, ete_OrganizID
					, ete_UserRowID
					, ete_UserRowID
					, ete_Date
					, eshRowID
					, ete_EmpRowID
					, esalRowID
					, '0'
					, ete_RegHrsWorkd
					, ete_OvertimeHrs
					, ete_HrsUnder
					, ete_NDiffHrs
					, ete_NDiffOTHrs
					, ete_HrsLate
					, payrateRowID
					, ete_TotalDayPay
					, ete_RegHrsWorkd + ete_OvertimeHrs
					, (ete_RegHrsWorkd * rateperhour) * commonrate
					, (ete_OvertimeHrs * rateperhour) * otrate
					, (ete_HrsUnder * rateperhour)
					, (ete_NDiffHrs * rateperhour) * ndiffrate
					, (ete_NDiffOTHrs * rateperhour) * ndiffotrate
					, (ete_HrsLate * rateperhour)
			) INTO anyINT;
			

		END IF;
	
	ELSE
		
	
		
		SELECT CAST(EXISTS(
		SELECT
		elv.RowID
		FROM employeeleave elv
		WHERE elv.EmployeeID=ete_EmpRowID
		AND elv.OrganizationID=ete_OrganizID
		AND ete_Date BETWEEN elv.LeaveStartDate AND elv.LeaveEndDate
		LIMIT 1) AS CHAR) 'CharResult'
		INTO hasLeave;

		
		IF hasLeave = '0' THEN
	
			
			
			IF isRestDay = '1' THEN
			
				IF ete_RegHrsWorkd > 8 THEN
					SET ete_RegHrsWorkd = 8;
				
				END IF;
			
				SET ete_TotalDayPay = ((ete_RegHrsWorkd * rateperhour) * ((commonrate + restday_rate) - 1))
											 + ((ete_OvertimeHrs * rateperhour) * ((commonrate + restdayot_rate) - 1));
											 
				SET ete_HrsLate = 0.0;
											 
				SELECT INSUPD_employeetimeentries(
						anyINT
						, ete_OrganizID
						, ete_UserRowID
						, ete_UserRowID
						, ete_Date
						, eshRowID
						, ete_EmpRowID
						, esalRowID
						, '0'
						, ete_RegHrsWorkd
						, ete_OvertimeHrs
						, ete_HrsUnder
						, ete_NDiffHrs
						, ete_NDiffOTHrs
						, ete_HrsLate
						, payrateRowID
						, ete_TotalDayPay
						, ete_RegHrsWorkd + ete_OvertimeHrs
						, (ete_RegHrsWorkd * rateperhour) * ((commonrate + restday_rate) - 1)
						, (ete_OvertimeHrs * rateperhour) * ((commonrate + restdayot_rate) - 1)
						, (ete_HrsUnder * rateperhour)
						, (ete_NDiffHrs * rateperhour) * ndiffrate
						, (ete_NDiffOTHrs * rateperhour) * ndiffotrate
						, (ete_HrsLate * rateperhour)
				) INTO anyINT;
				
				
			ELSE
			
				SET ete_TotalDayPay = ((ete_RegHrsWorkd * rateperhour) * commonrate)
											 + ((ete_OvertimeHrs * rateperhour) * otrate);
											 
											 
				SELECT INSUPD_employeetimeentries(
						anyINT
						, ete_OrganizID
						, ete_UserRowID
						, ete_UserRowID
						, ete_Date
						, eshRowID
						, ete_EmpRowID
						, esalRowID
						, '0'
						, ete_RegHrsWorkd
						, ete_OvertimeHrs
						, ete_HrsUnder
						, ete_NDiffHrs
						, ete_NDiffOTHrs
						, ete_HrsLate
						, payrateRowID
						, ete_TotalDayPay
						, ete_RegHrsWorkd + ete_OvertimeHrs
						, (ete_RegHrsWorkd * rateperhour) * commonrate
						, (ete_OvertimeHrs * rateperhour) * otrate
						, (ete_HrsUnder * rateperhour)
						, (ete_NDiffHrs * rateperhour) * ndiffrate
						, (ete_NDiffOTHrs * rateperhour) * ndiffotrate
						, (ete_HrsLate * rateperhour)
				) INTO anyINT;
				
				
			END IF;
				
		
		
		
		
		END IF;

	END IF;

ELSE 
		
	SELECT
	IFNULL(TotalDayPay,0)
	,IFNULL(TotalHoursWorked,0)
	FROM employeetimeentry
	WHERE EmployeeID=ete_EmpRowID
	AND OrganizationID=ete_OrganizID
	AND `Date`=pr_DayBefore
	INTO yester_TotDayPay
		  ,yester_TotHrsWorkd; 

	IF yester_TotDayPay IS NULL THEN
		SET yester_TotDayPay = 0;
		
	END IF;
		
	IF pr_DayBefore <= SUBDATE(e_StartDate, INTERVAL 1 DAY) THEN 
		
		
		SET isRestDay = '0';
		
		SET yester_TotDayPay = 0;
		
	END IF;
	
	IF yester_TotDayPay != 0 THEN
		
		SELECT (DAYOFWEEK(SUBDATE(ete_Date, INTERVAL 1 DAY)) = e.DayOfRest)
		FROM employee e
		WHERE e.RowID=ete_EmpRowID
		INTO isRestDay;
	
		IF isRestDay = '1' THEN

			SET ete_TotalDayPay = ((ete_RegHrsWorkd * rateperhour) * ((commonrate + restday_rate) - 1))
										 + ((ete_OvertimeHrs * rateperhour) * ((commonrate + restdayot_rate) - 1));
										 	 
			SELECT INSUPD_employeetimeentries(
					anyINT
					, ete_OrganizID
					, ete_UserRowID
					, ete_UserRowID
					, ete_Date
					, eshRowID
					, ete_EmpRowID
					, esalRowID
					, '0'
					, ete_RegHrsWorkd
					, ete_OvertimeHrs
					, ete_HrsUnder
					, ete_NDiffHrs
					, ete_NDiffOTHrs
					, ete_HrsLate
					, payrateRowID
					, ete_TotalDayPay
					, ete_RegHrsWorkd + ete_OvertimeHrs
					, (ete_RegHrsWorkd * rateperhour) * ((commonrate + restday_rate) - 1)
					, (ete_OvertimeHrs * rateperhour) * ((commonrate + restdayot_rate) - 1)
					, (ete_HrsUnder * rateperhour)
					, (ete_NDiffHrs * rateperhour) * ndiffrate
					, (ete_NDiffOTHrs * rateperhour) * ndiffotrate
					, (ete_HrsLate * rateperhour)
			) INTO anyINT;
			
		ELSE
		
			SET ete_TotalDayPay = ((ete_RegHrsWorkd * rateperhour) * commonrate)
										 + ((ete_OvertimeHrs * rateperhour) * otrate);
							
			IF ete_TotalDayPay IS NULL 
				OR ete_TotalDayPay = 0 THEN
				
				SET ete_TotalDayPay = dailypay;
				
			END IF;
										 
			SELECT INSUPD_employeetimeentries(
					anyINT
					, ete_OrganizID
					, ete_UserRowID
					, ete_UserRowID
					, ete_Date
					, eshRowID
					, ete_EmpRowID
					, esalRowID
					, '0'
					, ete_RegHrsWorkd
					, ete_OvertimeHrs
					, ete_HrsUnder
					, ete_NDiffHrs
					, ete_NDiffOTHrs
					, ete_HrsLate
					, payrateRowID
					, ete_TotalDayPay
					, ete_RegHrsWorkd + ete_OvertimeHrs
					, (ete_RegHrsWorkd * rateperhour) * commonrate
					, (ete_OvertimeHrs * rateperhour) * otrate
					, (ete_HrsUnder * rateperhour)
					, (ete_NDiffHrs * rateperhour) * ndiffrate
					, (ete_NDiffOTHrs * rateperhour) * ndiffotrate
					, (ete_HrsLate * rateperhour)
			) INTO anyINT;
			
		END IF;
			
	ELSE
	
		IF isRestDay = '1' THEN
		
				
			SET ete_TotalDayPay = ((ete_RegHrsWorkd * rateperhour) * commonrate)
										 + ((ete_OvertimeHrs * rateperhour) * otrate);
										 
			SELECT INSUPD_employeetimeentries(
					anyINT
					, ete_OrganizID
					, ete_UserRowID
					, ete_UserRowID
					, ete_Date
					, eshRowID
					, ete_EmpRowID
					, esalRowID
					, '0'
					, ete_RegHrsWorkd
					, ete_OvertimeHrs
					, ete_HrsUnder
					, ete_NDiffHrs
					, ete_NDiffOTHrs
					, ete_HrsLate
					, payrateRowID
					, ete_TotalDayPay
					, ete_RegHrsWorkd + ete_OvertimeHrs
					, (ete_RegHrsWorkd * rateperhour) * ((commonrate + restday_rate) - 1)
					, (ete_OvertimeHrs * rateperhour) * ((commonrate + restdayot_rate) - 1)
					, (ete_HrsUnder * rateperhour)
					, (ete_NDiffHrs * rateperhour) * ndiffrate
					, (ete_NDiffOTHrs * rateperhour) * ndiffotrate
					, (ete_HrsLate * rateperhour)
			) INTO anyINT;
			
			
		ELSEIF isRestDay = '0' THEN
				
			SELECT (DAYOFWEEK(SUBDATE(ete_Date, INTERVAL 1 DAY)) = e.DayOfRest)
			FROM employee e
			WHERE e.RowID=ete_EmpRowID
			INTO isRestDay;

			IF isRestDay = '1' THEN
	
SET yes_true = '0'; 
					
				SET ete_TotalDayPay = ((ete_RegHrsWorkd * rateperhour) * commonrate)
											 + ((ete_OvertimeHrs * rateperhour) * otrate);
				
				IF IFNULL(ete_TotalDayPay,0) = 0
					AND e_EmpType = 'Daily' THEN
					
					SET ete_TotalDayPay = dailypay;
					
				END IF;
				
				SELECT INSUPD_employeetimeentries(
						anyINT
						, ete_OrganizID
						, ete_UserRowID
						, ete_UserRowID
						, ete_Date
						, eshRowID
						, ete_EmpRowID
						, esalRowID
						, '0'
						, ete_RegHrsWorkd
						, ete_OvertimeHrs
						, ete_HrsUnder
						, ete_NDiffHrs
						, ete_NDiffOTHrs
						, ete_HrsLate
						, payrateRowID
						, ete_TotalDayPay
						, ete_RegHrsWorkd + ete_OvertimeHrs
						, (ete_RegHrsWorkd * rateperhour) * commonrate
						, (ete_OvertimeHrs * rateperhour) * otrate
						, (ete_HrsUnder * rateperhour)
						, (ete_NDiffHrs * rateperhour) * ndiffrate
						, (ete_NDiffOTHrs * rateperhour) * ndiffotrate
						, (ete_HrsLate * rateperhour)
				) INTO anyINT;
				
			ELSE
	
				SET ete_TotalDayPay = 0.0;
											 
				SELECT INSUPD_employeetimeentries(
						anyINT
						, ete_OrganizID
						, ete_UserRowID
						, ete_UserRowID
						, ete_Date
						, eshRowID
						, ete_EmpRowID
						, esalRowID
						, '0'
						, ete_RegHrsWorkd
						, ete_OvertimeHrs
						, ete_HrsUnder
						, ete_NDiffHrs
						, ete_NDiffOTHrs
						, ete_HrsLate
						, payrateRowID
						, ete_TotalDayPay
						, 0
						, 0
						, 0
						, 0
						, 0
						, 0
						, 0
				) INTO anyINT;
				
			END IF;


		END IF;
		
	END IF;
	
END IF;

	
	
	











SET returnvalue = anyint;

RETURN yes_true;



END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
