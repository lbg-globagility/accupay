/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `GENERATE_employeetimeentry`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GENERATE_employeetimeentry`(
	`ete_EmpRowID` INT,
	`ete_OrganizID` INT,
	`ete_Date` DATE,
	`ete_UserRowID` INT






















) RETURNS int(11)
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


DECLARE og_ndtimefrom TIME DEFAULT NULL;

DECLARE og_ndtimeto TIME DEFAULT NULL;


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

DECLARE isDayMatchRestDay TINYINT(1);


DECLARE yes_true CHAR(1) DEFAULT '0';

DECLARE anytime TIME;

DECLARE timeEntryID INT(11);


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

DECLARE e_LateGracePeriod DECIMAL(11,2);

DECLARE e_PositionID INT(11);
DECLARE shift_rowid INT(11);

DECLARE is_reg_shift_valid_for_ndiff TINYINT DEFAULT 0;

DECLARE is_OT_valid_for_ndiff TINYINT DEFAULT 0;

DECLARE sec_per_min INT(11) DEFAULT 60;

DECLARE min_per_hour INT(11) DEFAULT 60;

DECLARE divisorToDailyRate INT(11) DEFAULT 0;

DECLARE sh1 TIME DEFAULT NULL;
DECLARE sh2 TIME DEFAULT NULL;

DECLARE officialWorkStart DATETIME;
DECLARE officialWorkEnd DATETIME;

DECLARE tomorrowDate DATE;

DECLARE timeInTimestamp DATETIME;
DECLARE timeOutTimestamp DATETIME;
DECLARE shiftStartTimestamp DATETIME;
DECLARE shiftEndTimestamp DATETIME;
DECLARE overtimeAmount DECIMAL(11, 6);

DECLARE regularAmount DECIMAL(11, 6);

DECLARE breaktimeStartTimestamp DATETIME;
DECLARE breaktimeEndTimestamp DATETIME;

DECLARE holidayPay DECIMAL(11, 6);

DECLARE basicDayPay DECIMAL(12, 4);

DECLARE applicableRegularRate DECIMAL(11,6);
DECLARE applicableOvertimeRate DECIMAL(11,6);


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
,e.LateGracePeriod
,e.PositionID,og.NightDifferentialTimeFrom,og.NightDifferentialTimeTo
FROM employee e
INNER JOIN organization og ON og.RowID=e.OrganizationID
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
        ,e_CalcRestDayOT
        ,e_LateGracePeriod
        ,e_PositionID,og_ndtimefrom,og_ndtimeto;



SELECT
    RowID
    ,IF(
        PayType = 'Special Non-Working Holiday',
        IF(
            e_CalcSpecialHoliday = '1',
            PayRate,
            1
        ),
        IF(
            PayType = 'Regular Holiday',
            IF(
                calc_Holiday = '1',
                `PayRate`,
                1
            ),
            `PayRate`
        )
    )
    ,IF(e_OTOverride = '1', OvertimeRate, 1)
    ,IF(e_CalcNightDiff = '1', NightDifferentialRate, 1)
    ,IF(e_CalcNightDiffOT = '1', NightDifferentialOTRate, 1)
    ,IF(e_CalcRestDay = '1', RestDayRate, 1)
    ,IF(e_CalcRestDayOT = '1', RestDayOvertimeRate, 1)
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


SELECT COUNT(RowID)
FROM employeeovertime
WHERE EmployeeID=ete_EmpRowID
    AND OrganizationID=ete_OrganizID
    AND ete_Date BETWEEN OTStartDate AND OTEndDate
    AND OTStatus='Approved'
INTO OTCount;

SELECT COALESCE(DAYOFWEEK(ete_Date) = e.DayOfRest, FALSE)
FROM employee e
WHERE e.RowID = ete_EmpRowID
INTO isDayMatchRestDay;

SELECT
    sh.TimeFrom,
    sh.TimeTo,
    esh.RowID,
    sh.RowID,
    esh.RestDay
FROM employeeshift esh
INNER JOIN shift sh
    ON sh.RowID=esh.ShiftID
WHERE esh.EmployeeID=ete_EmpRowID
    AND esh.OrganizationID=ete_OrganizID
    AND ete_Date BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
ORDER BY DATEDIFF(ete_Date, esh.EffectiveFrom)
LIMIT 1
INTO
    shifttimefrom,
    shifttimeto,
    eshRowID,
    shift_rowid,
    isRestDay;

-- SELECT shifttimefrom, shifttimeto, isRestDay, ete_Date, ete_EmpRowID, ete_OrganizID
-- INTO OUTFILE 'D:/logs/shift.txt'
-- FIELDS TERMINATED BY ', ';

IF OTCount = 1 THEN

    SELECT
        IF(OTStartTime = shifttimeto, ADDTIME(shifttimeto,'00:00:01'), OTStartTime),
        OTEndTime,
        RowID
    FROM employeeovertime
    WHERE EmployeeID=ete_EmpRowID
        AND OrganizationID=ete_OrganizID
        AND OTStartTime >= shifttimeto
        AND OTStatus='Approved'
        AND (ete_Date BETWEEN OTStartDate AND OTEndDate)
    ORDER BY OTStartTime DESC
    LIMIT 1
    INTO
        otstartingtime,
        otendingtime,
        aftershiftOTRowID;

ELSE

    SELECT
        IF(OTStartTime = shifttimeto, ADDTIME(shifttimeto,'00:00:01'), OTStartTime),
        OTEndTime,
        RowID
    FROM employeeovertime
    WHERE EmployeeID=ete_EmpRowID
        AND OrganizationID=ete_OrganizID
        AND ete_Date BETWEEN OTStartDate AND COALESCE(OTEndDate,OTStartDate)
        AND OTStatus='Approved'
    ORDER BY OTStartTime DESC
    LIMIT 1
    INTO
        otstartingtime,
        otendingtime,
        aftershiftOTRowID;

END IF;

SET @sh_brktimeFr=NULL;
SET @sh_brktimeTo=NULL;

SELECT
    etd.TimeIn,
    IF(e_UTOverride = 1, etd.TimeOut, IFNULL(sh.TimeTo, etd.TimeOut)),
    sh.BreakTimeFrom,
    sh.BreakTimeTo
FROM employeetimeentrydetails etd
LEFT JOIN employeeshift esh
    ON esh.OrganizationID=etd.OrganizationID
    AND esh.EmployeeID=etd.EmployeeID
    AND etd.`Date` BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
LEFT JOIN shift sh
    ON sh.RowID=esh.ShiftID
WHERE etd.EmployeeID=ete_EmpRowID
    AND etd.OrganizationID=ete_OrganizID
    AND etd.`Date`=ete_Date
ORDER BY IFNULL(etd.LastUpd, etd.Created) DESC
LIMIT 1
INTO
    etd_TimeIn,
    etd_TimeOut,
    @sh_brktimeFr,
    @sh_brktimeTo;

SET tomorrowDate = DATE_ADD(ete_Date, INTERVAL 1 DAY);

SELECT GRACE_PERIOD(etd_TimeIn, shifttimefrom, e_LateGracePeriod)
INTO etd_TimeIn;

SET timeInTimestamp = TIMESTAMP(ete_Date, etd_TimeIn);
SET timeOutTimestamp = TIMESTAMP(IF(etd_TimeOut > etd_TimeIn, ete_Date, tomorrowDate), etd_TimeOut);

SET shiftStartTimestamp = TIMESTAMP(ete_Date, shifttimefrom);
SET shiftEndTimestamp = TIMESTAMP(IF(shifttimeto > shifttimefrom, ete_Date, tomorrowDate), shifttimeto);

SET breaktimeStartTimestamp = TIMESTAMP(ete_Date, @sh_brktimeFr);
SET breaktimeEndTimestamp = TIMESTAMP(ete_Date, @sh_brktimeTo);

/* The official work start is the time that is considered the employee has started working.
 * In this case, the work start is the time in, unless the employee went in early, then it should
 * just be the start of the shift.
 */
SET officialWorkStart = GREATEST(timeInTimestamp, shiftStartTimestamp);

/* The official work end is the time that is considered the employee has stopped working.
 * It should be the end of the shift, unless the employee timed out early, then it should be the
 * time out.
 */
SET officialWorkEnd = LEAST(timeOutTimestamp, shiftEndTimestamp);

IF @sh_brktimeFr IS NULL AND @sh_brktimeTo IS NULL THEN
    /* Calculate the regular work hours for the day.
     * If there is no breaktime, just compute the time span from the official work start and the official work end.
     */
    SET ete_RegHrsWorkd = COMPUTE_TimeDifference(TIME(officialWorkStart), TIME(officialWorkEnd));
ELSE
    /* If there is a breaktime, split the computation between the work done before breaktime,
     * and the work done after breaktime.
     */
    IF officialWorkStart < breaktimeStartTimestamp THEN
        SET @workBeforeBreak = COMPUTE_TimeDifference(TIME(officialWorkStart), TIME(breaktimeStartTimestamp));
    ELSE
        SET @workBeforeBreak = 0;
    END IF;

    IF officialWorkEnd < breaktimeEndTimestamp THEN
        SET @workAfterBreak = 0;
    ELSE
        /* Let's make sure that we calculate the correct work hours after breaktime by ensuring that we don't choose the
         * breaktime's end when the employee started work after breaktime.
         */
        SET @workAfterBreakStart = GREATEST(breaktimeEndTimestamp, officialWorkStart);

        SET @workAfterBreak = COMPUTE_TimeDifference(TIME(@workAfterBreakStart), TIME(officialWorkEnd));
    END IF;

    SET ete_RegHrsWorkd = @workBeforeBreak + @workAfterBreak;
END IF;

SET @hasBreaktime = @sh_brktimeFr IS NOT NULL;

/* Calculate the late hours starting the process by specifying the period the employee was late.
 */
IF officialWorkStart > shifttimefrom THEN
    SET @lateStart = shifttimefrom;
    SET @lateEnd = officialWorkStart;

    If @hasBreaktime THEN
        IF @lateStart < @sh_brktimeFr THEN
            SET @lateBeforeBreakStart = LEAST(officialWorkStart, @sh_brktimeFr);

            SET @lateBeforeBreak = COMPUTE_TimeDifference(@lateStart, @lateBeforeBreakStart);
        ELSE
            SET @lateBeforeBreak = 0;
        END IF;

        IF @lateEnd < @sh_brktimeTo THEN
            SET @lateAfterBreak = 0;
        ELSE
            SET @lateAfterBreak = COMPUTE_TimeDifference(@sh_brktimeTo, @lateEnd);
        END IF;

        SET ete_HrsLate = @lateBeforeBreak + @lateAfterBreak;
    ELSE
        SET ete_HrsLate = COMPUTE_TimeDifference(@lateStart, @lateEnd);
    END IF;
END IF;

IF shifttimeto > etd_TimeOut THEN
    SET ete_HrsUnder = (COMPUTE_TimeDifference(shifttimefrom, shifttimeto) - COMPUTE_TimeDifference(@sh_brktimeFr, @sh_brktimeTo)) - ete_RegHrsWorkd;
END IF;

IF otstartingtime IS NULL AND otstartingtime IS NULL THEN
    SET @noop = true;
ELSE
    IF shifttimeto > etd_TimeOut THEN
        SET ete_HrsUnder = (COMPUTE_TimeDifference(shifttimefrom, shifttimeto) - COMPUTE_TimeDifference(@sh_brktimeFr, @sh_brktimeTo)) - ete_RegHrsWorkd;
    END IF;

    SELECT COMPUTE_TimeDifference(otstartingtime, etd_TimeOut)
    INTO ete_OvertimeHrs;

    IF TIME_FORMAT(otstartingtime,'%p') = 'PM'
        AND TIME_FORMAT(otendingtime,'%p') = 'AM'
        AND TIME_FORMAT(etd_TimeOut,'%p') = 'AM' THEN

        IF ADDTIME(etd_TimeOut,'24:00') BETWEEN otstartingtime AND ADDTIME(otendingtime,'24:00') THEN

            SELECT COMPUTE_TimeDifference(otstartingtime, etd_TimeOut)
            INTO ete_OvertimeHrs;

            SET etd_TimeOut = SUBTIME(otstartingtime, '00:00:01');

        ELSEIF etd_TimeOut > otendingtime THEN

            SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
            INTO ete_OvertimeHrs;

            SET etd_TimeOut = SUBTIME(otstartingtime,'00:00:01');

        ELSE

            SELECT COMPUTE_TimeDifference(otstartingtime, etd_TimeOut)
            INTO ete_OvertimeHrs;

            SET ete_OvertimeHrs = ete_OvertimeHrs - COMPUTE_TimeDifference(otendingtime,etd_TimeOut);

            SET etd_TimeOut = SUBTIME(otstartingtime,'00:00:01');

        END IF;

    ELSEIF TIME_FORMAT(otstartingtime,'%p') = 'PM'
             AND TIME_FORMAT(otendingtime,'%p') = 'AM'
             AND TIME_FORMAT(etd_TimeOut,'%p') = 'PM' THEN

        SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
        INTO ete_OvertimeHrs;

        IF etd_TimeOut BETWEEN otstartingtime AND ADDTIME(otendingtime,'24:00') THEN

            SET @false = false;

        ELSEIF etd_TimeOut < shifttimeto THEN

            SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
            INTO ete_OvertimeHrs;

        ELSE

            SET ete_OvertimeHrs = 0;

        END IF;

    ELSEIF TIME_FORMAT(otstartingtime,'%p') = 'AM'
             AND TIME_FORMAT(otendingtime,'%p') = 'PM'
             AND TIME_FORMAT(etd_TimeOut,'%p') = 'PM' THEN

        SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
        INTO ete_OvertimeHrs;

    ELSEIF TIME_FORMAT(otstartingtime,'%p') = 'AM'
             AND TIME_FORMAT(otendingtime,'%p') = 'PM'
             AND TIME_FORMAT(etd_TimeOut,'%p') = 'AM' THEN

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

        IF DATE_FORMAT(etd_TimeOut, '%H') = '00' THEN

            IF (DATE_FORMAT(etd_TimeOut, '24:%i:%s') > otstartingtime AND DATE_FORMAT(etd_TimeOut, '24:%i:%s') > otendingtime) THEN

                SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
                INTO ete_OvertimeHrs;

                SET etd_TimeOut = SUBTIME(otstartingtime,'00:00:01');

            ELSE
                SET ete_OvertimeHrs = 0.0;

            END IF;

        ELSE

            IF (etd_TimeOut > otstartingtime AND etd_TimeOut > otendingtime) THEN

                SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
                INTO ete_OvertimeHrs;

                SET etd_TimeOut = SUBTIME(otstartingtime,'00:00:01');

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

                IF COMPUTE_TimeDifference(otendingtime, etd_TimeOut) > 0 THEN

                    SELECT COMPUTE_TimeDifference(otstartingtime, etd_TimeOut)
                    INTO ete_OvertimeHrs;

                ELSE

                    SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
                    INTO ete_OvertimeHrs;

                END IF;

            END IF;

        ELSE

            IF etd_TimeOut BETWEEN otstartingtime AND otendingtime THEN

                SELECT COMPUTE_TimeDifference(otstartingtime, etd_TimeOut)
                INTO ete_OvertimeHrs;

                SET etd_TimeOut = shifttimeto;

            ELSE

                IF shifttimefrom > otendingtime THEN

                    SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
                    INTO ete_OvertimeHrs;

                ELSEIF etd_TimeOut < otstartingtime THEN

                    SET ete_OvertimeHrs = 0;

                ELSE

                    SELECT COMPUTE_TimeDifference(otstartingtime, otendingtime)
                    INTO ete_OvertimeHrs;

                    SET etd_TimeOut = SUBTIME(otstartingtime, '00:00:01');

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

IF etd_TimeOut < shifttimeto THEN
    SET yes_true = 1;
    SET ete_HrsUnder = COMPUTE_TimeDifference(shifttimefrom,shifttimeto) - 0;
    SET ete_HrsUnder = ete_HrsUnder - (ete_RegHrsWorkd + ete_HrsLate);
END IF;

SET ete_NDiffHrs = 0.0;
SET ete_NDiffOTHrs = 0.0;


SELECT GET_employeerateperday(ete_EmpRowID, ete_OrganizID, ete_Date)
INTO dailypay;

SET rateperhour = COMPUTE_TimeDifference(shifttimefrom, shifttimeto);

SELECT shift.DivisorToDailyRate
FROM shift
WHERE shift.RowID = shift_rowid
INTO divisorToDailyRate;

SET rateperhour = dailypay / divisorToDailyRate;

SELECT RowID
FROM employeetimeentry
WHERE EmployeeID=ete_EmpRowID
    AND OrganizationID=ete_OrganizID
    AND `Date`=ete_Date
LIMIT 1
INTO timeEntryID;


SELECT RowID
FROM employeesalary
WHERE EmployeeID=ete_EmpRowID
    AND OrganizationID=ete_OrganizID
    AND ete_Date BETWEEN DATE(COALESCE(EffectiveDateFrom, DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveDateTo, ete_Date))
    AND DATEDIFF(ete_Date,EffectiveDateFrom) >= 0
ORDER BY DATEDIFF(DATE_FORMAT(ete_Date,'%Y-%m-%d'),EffectiveDateFrom)
LIMIT 1
INTO esalRowID;

IF ete_RegHrsWorkd IS NULL THEN
    SET ete_RegHrsWorkd = 0;
END IF;

SET @break_time_hrs = IFNULL((SELECT COMPUTE_TimeDifference(sh.BreakTimeFrom,sh.BreakTimeTo) FROM shift sh WHERE sh.RowID=shift_rowid),0);

IF ete_HrsLate IS NULL THEN
    SET ete_HrsLate = 0;
END IF;

IF ete_HrsLate > 4 AND COMPUTE_TimeDifference(shifttimefrom, shifttimeto) = 9 THEN

    SET ete_HrsLate = COMPUTE_TimeDifference(shifttimefrom, IF(etd_TimeIn BETWEEN @sh_brktimeFr AND @sh_brktimeFr, @sh_brktimeTo, etd_TimeIn));
ELSEIF ete_HrsLate > 5 AND COMPUTE_TimeDifference(shifttimefrom, shifttimeto) = 10 THEN

    SET ete_HrsLate = COMPUTE_TimeDifference(shifttimefrom, IF(etd_TimeIn BETWEEN @sh_brktimeFr AND @sh_brktimeFr, @sh_brktimeTo, etd_TimeIn));
END IF;


IF ete_HrsUnder IS NULL THEN
    SET ete_HrsUnder = 0;
END IF;

IF ete_HrsUnder > 4 AND COMPUTE_TimeDifference(shifttimefrom, shifttimeto) = 9 THEN
    SET ete_HrsUnder = COMPUTE_TimeDifference(IFNULL(@sh_brktimeTo, SUBTIME(shifttimeto,'04:00')), shifttimeto);
ELSEIF ete_HrsUnder > 5 AND COMPUTE_TimeDifference(shifttimefrom, shifttimeto) = 10 THEN
    SET ete_HrsUnder = COMPUTE_TimeDifference(IFNULL(@sh_brktimeTo, SUBTIME(shifttimeto,'04:00')), shifttimeto);
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

    SELECT COMPUTE_TimeDifference(OTStartTime, OTEndTime)
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

SET basicDayPay = ete_RegHrsWorkd * rateperhour;

-- a. If the current day is a regular working day.
IF pr_DayBefore IS NULL THEN

    SELECT
        IFNULL(TotalDayPay, 0),
        IFNULL(TotalHoursWorked, 0)
    FROM employeetimeentry
    WHERE EmployeeID=ete_EmpRowID
        AND OrganizationID=ete_OrganizID
        AND `Date`=ete_Date
    INTO
        yester_TotDayPay,
        yester_TotHrsWorkd;

    IF yester_TotDayPay IS NULL THEN
        SET yester_TotDayPay = 0;
    END IF;

    -- a. If the current day is a regular working day.
    -- b. If current day is before employment hiring date.
    IF ete_Date < e_StartDate THEN

        SET ete_TotalDayPay = 0.0;

        SELECT INSUPD_employeetimeentries(
                timeEntryID
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
                , NULL
                , NULL
                , NULL
                , NULL
                , 1
        ) INTO timeEntryID;

    -- a. If the current day is a regular working day.
    -- b. If employee was not payed yesterday.
    ELSEIF yester_TotDayPay = 0 THEN

        -- a. If the current day is a regular working day.
        -- b. If employee was not payed yesterday.
        -- c. If it's currently a rest day.
        IF isRestDay = '1' THEN

            SET ete_TotalDayPay = ((ete_RegHrsWorkd * rateperhour) * commonrate)
                                + ((ete_OvertimeHrs * rateperhour) * otrate);

            SELECT INSUPD_employeetimeentries(
                    timeEntryID
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
                    , ((ete_RegHrsWorkd - ete_NDiffHrs) * rateperhour) * ((commonrate + restday_rate) - 1)
                    , (ete_OvertimeHrs * rateperhour) * ((commonrate + restdayot_rate) - 1)
                    , (ete_HrsUnder * rateperhour)
                    , (ete_NDiffHrs * rateperhour) * ndiffrate
                    , (ete_NDiffOTHrs * rateperhour) * ndiffotrate
                    , (ete_HrsLate * rateperhour)
                    , ete_RegHrsWorkd
                    , ((ete_RegHrsWorkd - ete_NDiffHrs) * rateperhour) * ((commonrate + restday_rate) - 1)
                    , 0
                    , basicDayPay
                    , 2
            ) INTO timeEntryID;

        -- a. If the current day is a regular working day.
        -- b. If employee was not payed yesterday.
        -- c. If it's currently NOT a rest day.
        ELSEIF isRestDay = '0' THEN

            IF ete_RegHrsWorkd = 0 THEN
                SET ete_TotalDayPay = 0;
            ELSE
                SET ete_TotalDayPay = ((ete_RegHrsWorkd * rateperhour) * commonrate)
                                         + ((ete_OvertimeHrs * rateperhour) * otrate);
            END IF;

            SELECT INSUPD_employeetimeentries(
                    timeEntryID
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
                    , ((ete_RegHrsWorkd - ete_NDiffHrs) * rateperhour) * commonrate
                    , (ete_OvertimeHrs * rateperhour) * otrate
                    , (ete_HrsUnder * rateperhour)
                    , (ete_NDiffHrs * rateperhour) * ndiffrate
                    , (ete_NDiffOTHrs * rateperhour) * ndiffotrate
                    , (ete_HrsLate * rateperhour)
                    , NULL
                    , NULL
                    , 0
                    , basicDayPay
                    , 3
            ) INTO timeEntryID;

        END IF;

    -- a. If the current day is a regular working day.
    -- b. Employee was payed yesterday AND
    --    current day is after employment hiring date.
    ELSE

        SELECT CAST(
            EXISTS(
                SELECT
                elv.RowID
                FROM employeeleave elv
                WHERE elv.EmployeeID=ete_EmpRowID
                    AND elv.`Status`='Approved'
                    AND elv.OrganizationID=ete_OrganizID
                    AND ete_Date BETWEEN elv.LeaveStartDate AND elv.LeaveEndDate
                LIMIT 1
            ) AS CHAR
        ) 'CharResult'
        INTO hasLeave;

        -- a. If the current day is a regular working day.
        -- b. Employee was payed yesterday AND
        --    current day is after employment hiring date.
        -- c. If no leave was filed today.
        IF hasLeave = '0' THEN

            SELECT EXISTS(
                SELECT RowID
                FROM employeeshift esh
                WHERE esh.RestDay='1'
                    AND esh.EmployeeID=ete_EmpRowID
                    AND esh.OrganizationID=ete_OrganizID
                    AND ete_Date BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
                LIMIT 1
            ) INTO isRestDay;

            -- a. If the current day is a regular working day.
            -- b. Employee was payed yesterday AND
            --    current day is after employment hiring date.
            -- c. If no leave was filed today.
            -- d. If it's a rest day.
            IF isRestDay = '1' THEN

                IF ete_RegHrsWorkd = 140 THEN
                    SET ete_RegHrsWorkd = 8;
                END IF;

                SET ete_TotalDayPay =   (
                    (ete_RegHrsWorkd - ete_NDiffHrs) * rateperhour) * ((commonrate + restday_rate) - 1)
                    + (ete_OvertimeHrs * rateperhour) * restdayot_rate
                    + (ete_NDiffHrs * rateperhour) * ndiffrate;

                SET ete_HrsLate = 0.0;


                SELECT INSUPD_employeetimeentries(
                        timeEntryID
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
                        , ((ete_RegHrsWorkd - ete_NDiffHrs) * rateperhour) * ((commonrate + restday_rate) - 1)
                        , (ete_OvertimeHrs * rateperhour) * restdayot_rate
                        , (ete_HrsUnder * rateperhour)
                        , (ete_NDiffHrs * rateperhour) * ndiffrate
                        , (ete_NDiffOTHrs * rateperhour) * ndiffotrate
                        , (ete_HrsLate * rateperhour)
                        , ete_RegHrsWorkd
                        , ((ete_RegHrsWorkd - ete_NDiffHrs) * rateperhour) * ((commonrate + restday_rate) - 1)
                        , 0
                        , basicDayPay
                        , 4
                ) INTO timeEntryID;

            -- a. If the current day is a regular working day.
            -- b. Employee was payed yesterday AND
            --    current day is after employment hiring date.
            -- c. If no leave was filed today.
            -- d. If it's NOT a rest day.
            ELSE

                SET ete_TotalDayPay = ((ete_RegHrsWorkd * rateperhour) * commonrate)
                                             + ((ete_OvertimeHrs * rateperhour) * otrate);

                SELECT INSUPD_employeetimeentries(
                        timeEntryID
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
                        , ((ete_RegHrsWorkd - ete_NDiffHrs) * rateperhour) * commonrate
                        , (ete_OvertimeHrs * rateperhour) * otrate
                        , (ete_HrsUnder * rateperhour)
                        , (ete_NDiffHrs * rateperhour) * ndiffrate
                        , (ete_NDiffOTHrs * rateperhour) * ndiffotrate
                        , (ete_HrsLate * rateperhour)
                        , NULL
                        , NULL
                        , 0
                        , basicDayPay
                        , 5
                ) INTO timeEntryID;

            END IF;

        -- a. If the current day is a regular working day.
        -- b. Employee was payed yesterday AND
        --    current day is after employment hiring date.
        -- c. If a leave was filed today.
        ELSE

            SET ete_TotalDayPay = IFNULL(
                (
                    SELECT SUM(TotalDayPay)
                    FROM employeetimeentry
                    WHERE EmployeeID=ete_EmpRowID
                        AND OrganizationID=ete_OrganizID
                        AND `Date`=ete_Date
                ),
                0
            );

            SET ete_TotalDayPay = ete_TotalDayPay + ((ete_RegHrsWorkd * rateperhour) * commonrate) + ((ete_OvertimeHrs * rateperhour) * otrate);

            SELECT INSUPD_employeetimeentries(
                timeEntryID,
                ete_OrganizID,
                ete_UserRowID,
                ete_UserRowID,
                ete_Date,
                eshRowID,
                ete_EmpRowID,
                esalRowID,
                '0',
                ete_RegHrsWorkd,
                ete_OvertimeHrs,
                ete_HrsUnder,
                ete_NDiffHrs,
                ete_NDiffOTHrs,
                ete_HrsLate,
                payrateRowID,
                ete_TotalDayPay,
                ete_RegHrsWorkd + ete_OvertimeHrs,
                ((ete_RegHrsWorkd - ete_NDiffHrs) * rateperhour) * commonrate, (ete_OvertimeHrs * rateperhour) * otrate,
                (ete_HrsUnder * rateperhour),
                (ete_NDiffHrs * rateperhour) * ndiffrate,
                (ete_NDiffOTHrs * rateperhour) * ndiffotrate,
                (ete_HrsLate * rateperhour),
                NULL,
                NULL,
                0,
                basicDayPay,
                6
            ) INTO timeEntryID;
        END IF;

    END IF;

-- a. If the current day is a holiday.
ELSE

    SELECT
        IFNULL(et.TotalDayPay,0),
        IFNULL(et.TotalHoursWorked,0)
    FROM employeetimeentry et
    INNER JOIN employee e ON e.RowID=et.EmployeeID
    WHERE et.EmployeeID=ete_EmpRowID
    AND et.OrganizationID=ete_OrganizID
    AND et.`Date`=IF(CHAR_TO_DAYOFWEEK(e.DayOfRest) = DAYNAME(pr_DayBefore), SUBDATE(pr_DayBefore, INTERVAL 1 DAY), pr_DayBefore)
    INTO
        yester_TotDayPay,
        yester_TotHrsWorkd;

    SELECT EXISTS(
        SELECT elv.RowID
        FROM employeeleave elv
        WHERE elv.EmployeeID=ete_EmpRowID
            AND elv.`Status`='Approved'
            AND elv.OrganizationID=ete_OrganizID
            AND ete_Date BETWEEN elv.LeaveStartDate AND elv.LeaveEndDate
        LIMIT 1
    )
    INTO hasLeave;

    IF pr_DayBefore <= SUBDATE(e_StartDate, INTERVAL 1 DAY) THEN
        SET isRestDay = '0';

        SET yester_TotDayPay = 0;
    END IF;

    -- a. If the current day is a holiday.
    -- b. If employee was payed yesterday.
    IF yester_TotDayPay != 0 THEN

        IF isRestDay = '1' THEN
            SET regularAmount = (ete_RegHrsWorkd * rateperhour) * (restday_rate - 1);
            SET overtimeAmount = (ete_OvertimeHrs * rateperhour) * restdayot_rate;

            SET applicableRegularRate = restday_rate;
        ELSE
            SET regularAmount = (ete_RegHrsWorkd * rateperhour);
            SET overtimeAmount = (ete_OvertimeHrs * rateperhour) * otrate;

            SET applicableRegularRate = commonrate;
        END IF;

        IF pr_PayType = 'Regular Holiday' THEN
            SET holidayPay = dailypay;
        ELSEIF pr_PayType = 'Special Non-Working Holiday' THEN
            SET holidayPay = regularAmount * (applicableRegularRate - 1);
        ELSE
            SET holidayPay = 0;
        END IF;

        SET ete_TotalDayPay = regularAmount + overtimeAmount + holidayPay;

        SELECT INSUPD_employeetimeentries(
                timeEntryID,
                ete_OrganizID,
                ete_UserRowID,
                ete_UserRowID,
                ete_Date,
                eshRowID,
                ete_EmpRowID,
                esalRowID,
                '0',
                ete_RegHrsWorkd,
                ete_OvertimeHrs,
                ete_HrsUnder,
                ete_NDiffHrs,
                ete_NDiffOTHrs,
                ete_HrsLate,
                payrateRowID,
                ete_TotalDayPay,
                ete_RegHrsWorkd + ete_OvertimeHrs,
                regularAmount,
                overtimeAmount,
                (ete_HrsUnder * rateperhour),
                (ete_NDiffHrs * rateperhour) * ndiffrate,
                (ete_NDiffOTHrs * rateperhour) * ndiffotrate,
                (ete_HrsLate * rateperhour),
                NULL,
                NULL,
                holidayPay,
                basicDayPay,
                7
        )
        INTO timeEntryID;

    -- a. If the current day is a holiday.
    -- b. If employee was NOT payed yesterday.
    ELSE

        -- a. If the current day is a holiday.
        -- b. If employee was NOT payed yesterday.
        -- c. If today is a rest day.
        IF isRestDay = '1' THEN


            SET ete_TotalDayPay = ((ete_RegHrsWorkd * rateperhour) * commonrate)
                                         + ((ete_OvertimeHrs * rateperhour) * otrate);

            SELECT INSUPD_employeetimeentries(
                    timeEntryID
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
                    , ((ete_RegHrsWorkd - ete_NDiffHrs) * rateperhour) * ((commonrate + restday_rate) - 1)
                    , (ete_OvertimeHrs * rateperhour) * ((commonrate + restdayot_rate) - 1)
                    , (ete_HrsUnder * rateperhour)
                    , (ete_NDiffHrs * rateperhour) * ndiffrate
                    , (ete_NDiffOTHrs * rateperhour) * ndiffotrate
                    , (ete_HrsLate * rateperhour)
                    , NULL
                    , NULL
                    , 0
                    , basicDayPay
                    , 9
            )
            INTO timeEntryID;

        -- a. If the current day is a holiday.
        -- b. If employee was NOT payed yesterday.
        -- c. If today is NOT a rest day.
        ELSEIF isRestDay = '0' THEN

            SELECT (
                IF(
                    CHAR_TO_DAYOFWEEK(e.DayOfRest) = DAYNAME(pr_DayBefore),
                    DAYOFWEEK(SUBDATE(pr_DayBefore, INTERVAL 1 DAY)),
                    DAYOFWEEK(pr_DayBefore)
                ) = e.DayOfRest
            )
            FROM employee e
            WHERE e.RowID = ete_EmpRowID
            INTO isRestDay;

            IF isRestDay IN (0, 1) THEN

                SET ete_TotalDayPay = ((ete_RegHrsWorkd * rateperhour) * commonrate)
                                        + ((ete_OvertimeHrs * rateperhour) * otrate);

                SELECT INSUPD_employeetimeentries(
                        timeEntryID
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
                        , ((ete_RegHrsWorkd - ete_NDiffHrs) * rateperhour) * commonrate
                        , (ete_OvertimeHrs * rateperhour) * otrate
                        , (ete_HrsUnder * rateperhour)
                        , (ete_NDiffHrs * rateperhour) * ndiffrate
                        , (ete_NDiffOTHrs * rateperhour) * ndiffotrate
                        , (ete_HrsLate * rateperhour)
                        , NULL
                        , NULL
                        , 0
                        , basicDayPay
                        , 10
                ) INTO timeEntryID;

            ELSE

                SET ete_TotalDayPay = 0.0;

                -- deprecate

                -- SELECT INSUPD_employeetimeentries(
                --         timeEntryID
                --         , ete_OrganizID
                --         , ete_UserRowID
                --         , ete_UserRowID
                --         , ete_Date
                --         , eshRowID
                --         , ete_EmpRowID
                --         , esalRowID
                --         , '0'
                --         , ete_RegHrsWorkd
                --         , ete_OvertimeHrs
                --         , ete_HrsUnder
                --         , ete_NDiffHrs
                --         , ete_NDiffOTHrs
                --         , ete_HrsLate
                --         , payrateRowID
                --         , ete_TotalDayPay
                --         , 0
                --         , 0
                --         , 0
                --         , 0
                --         , 0
                --         , 0
                --         , 0
                --         , NULL
                --         , NULL
                --         , 0
                --         , NULL
                --         , 11
                -- ) INTO timeEntryID;

            END IF;


        END IF;

    END IF;

END IF;


SET returnvalue = timeEntryID;

RETURN yes_true;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
