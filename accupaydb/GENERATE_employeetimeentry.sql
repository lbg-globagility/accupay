/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `GENERATE_employeetimeentry`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GENERATE_employeetimeentry`(`ete_EmpRowID` INT, `ete_OrganizID` INT, `ete_Date` DATE, `ete_UserRowID` INT
) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE DAYTYPE_REGULAR_WORKING_DAY VARCHAR(50) DEFAULT 'Regular Working Day';
DECLARE DAYTYPE_SPECIAL_NON_WORKING_HOLIDAY VARCHAR(50) DEFAULT 'Special Non-Working Holiday';
DECLARE DAYTYPE_REGULAR_HOLIDAY VARCHAR(50) DEFAULT 'Regular Holiday';

DECLARE STANDARD_WORKING_HOURS INT(10) DEFAULT 8;

DECLARE SECONDS_PER_HOUR INT(11) DEFAULT 3600;

/*
 * The standard rate is 100% or a multiplier of 1.0.
 */
DECLARE BASIC_RATE DECIMAL(15, 4) DEFAULT 1.0;

DECLARE returnvalue INT(11);

DECLARE isRestDay TEXT;
DECLARe isWorkingDay BOOLEAN;

DECLARE yester_TotDayPay DECIMAL(11,2);
DECLARE yester_TotHrsWorkd DECIMAL(11,2);

DECLARE e_EmpStatus TEXT;

DECLARE e_EmpType TEXT;

DECLARE e_MaritStatus TEXT;

DECLARE e_StartDate DATE;

DECLARE e_PayFreqID INT(11);

DECLARE e_NumDependent INT(11);

DECLARE e_UTOverride CHAR(1);

DECLARE e_OTOverride CHAR(1);

DECLARE e_DaysPerYear INT(11);

DECLARE isEntitledToNightDiff BOOLEAN;
DECLARE isEntitledToNightDiffOvertime BOOLEAN;
DECLARE isEntitledToRegularHoliday BOOLEAN;
DECLARE isEntitledToSpecialNonWorkingHoliday BOOLEAN;
DECLARE isEntitledToHoliday BOOLEAN;
DECLARE isEntitledToRestDay BOOLEAN;
DECLARE isEntitledToRestDayOvertime BOOLEAN;
DECLARE isCalculatingRegularHoliday BOOLEAN DEFAULT FALSE;
DECLARE isCalculatingSpecialNonWorkingHoliday BOOLEAN DEFAULT FALSE;

DECLARE isDayMatchRestDay BOOLEAN DEFAULT FALSE;

DECLARE yes_true CHAR(1) DEFAULT '0';

DECLARE anytime TIME;

DECLARE timeEntryID INT(11);

DECLARE hourlyRate DECIMAL(11,6);
DECLARE dailyRate DECIMAL(11,6);

DECLARE commonrate DECIMAL(11,6);

DECLARE otrate DECIMAL(11,6);

DECLARE ndiffrate DECIMAL(11,6);

DECLARE ndiffotrate DECIMAL(11,6);

DECLARE restday_rate DECIMAL(11,6);

DECLARE restdayot_rate DECIMAL(11,6);


DECLARE employeeShiftID INT(11);

DECLARE esalRowID INT(11);

DECLARE payrateRowID INT(11);

DECLARE ete_TotalDayPay DECIMAL(11,6);

DECLARE OTCount INT(11) DEFAULT 0;

DECLARE aftershiftOTRowID INT(11) DEFAULT 0;

DECLARE anotherOTHours DECIMAL(11,6);

DECLARE e_LateGracePeriod DECIMAL(11,2);

DECLARE e_PositionID INT(11);
DECLARE shiftID INT(11);

DECLARE workingHours DECIMAL(11,6) DEFAULT 0;#INT(10)

DECLARE requiredToWorkLastWorkingDay BOOLEAN DEFAULT FALSE;
DECLARE allowAbsenceOnHoliday BOOLEAN DEFAULT FALSE;

DECLARE dateToday DATE;
DECLARE dateTomorrow DATE;
DECLARE dateYesterday DATE;

DECLARE etd_TimeIn TIME;
DECLARE etd_TimeOut TIME;
DECLARE actualTimeIn TIME;
DECLARE fullTimeIn DATETIME;
DECLARE fullTimeOut DATETIME;
DECLARE hasTimeLogs BOOLEAN DEFAULT FALSE;

DECLARE officialBusStartTime TIME;
DECLARE officialBusEndTime TIME;

DECLARE shifttimefrom TIME;
DECLARE shifttimeto TIME;
DECLARE shiftStart DATETIME;
DECLARE shiftEnd DATETIME;
DECLARE shiftHours DECIMAL(15, 4) DEFAULT 0.0;
DECLARE hasShift BOOLEAN DEFAULT FALSE;
DECLARE breaktimeStart DATETIME;
DECLARE breaktimeEnd DATETIME;
DECLARE hasBreaktime BOOLEAN DEFAULT FALSE;
DECLARE isNightShift BOOLEAN DEFAULT FALSE;

DECLARE isRegularDay BOOLEAN DEFAULT FALSE;
DECLARE isSpecialNonWorkingHoliday BOOLEAN DEFAULT FALSE;
DECLARE isRegularHoliday BOOLEAN DEFAULT FALSE;
DECLARE isHoliday BOOLEAN DEFAULT FALSE;

DECLARE dutyStart DATETIME;
DECLARE dutyEnd DATETIME;

DECLARE regularHoursBeforeBreak DECIMAL(11, 6) DEFAULT 0.0;
DECLARE regularHoursAfterBreak DECIMAL(11, 6) DEFAULT 0.0;
DECLARE regularHours DECIMAL(11, 6) DEFAULT 0.0;
DECLARE regularAmount DECIMAL(11, 6) DEFAULT 0.0;
DECLARE hasWorked BOOLEAN DEFAULT FALSE;

DECLARE nightDiffTimeFrom TIME;
DECLARE nightDiffTimeTo TIME;
DECLARE nightDiffRangeStart DATETIME;
DECLARE nightDiffRangeEnd DATETIME;
DECLARE dawnNightDiffRangeStart DATETIME;
DECLARE dawnNightDiffRangeEnd DATETIME;

DECLARE nightDiffHours DECIMAL(11, 6) DEFAULT 0.0;
DECLARE nightDiffAmount DECIMAL(11, 6) DEFAULT 0.0;
DECLARE isDutyOverlappedWithNightDifferential BOOLEAN DEFAULT FALSE;
DECLARE shouldCalculateNightDifferential BOOLEAN DEFAULT FALSE;

DECLARE otstartingtime TIME DEFAULT NULL;
DECLARE otendingtime TIME DEFAULT NULL;
DECLARE overtimeStart DATETIME;
DECLARE overtimeEnd DATETIME;
DECLARE overtimeDate DATE;
DECLARE hasOvertime BOOLEAN DEFAULT FALSE;

DECLARE overtimeDutyStart DATETIME;
DECLARE overtimeDutyEnd DATETIME;
DECLARE overtimeHours DECIMAL(12, 6) DEFAULT 0.0;
DECLARE overtimeAmount DECIMAL(11, 6) DEFAULT 0.0;
DECLARE restDayOvertimeHours DECIMAL(15, 4) DEFAULT 0.0;

DECLARE nightDiffOTDutyStart DATETIME;
DECLARE nightDiffOTDutyEnd DATETIME;
DECLARE nightDiffOTHours DECIMAL(11, 6) DEFAULT 0.0;
DECLARE nightDiffOTAmount DECIMAL(11, 6) DEFAULT 0.0;
DECLARE isOvertimeOverlappedNightDifferential BOOLEAN DEFAULT FALSE;
DECLARE shouldCalculateNightDifferentialOvertime BOOLEAN DEFAULT FALSE;

DECLARE isDefaultRestDay BOOLEAN DEFAULT FALSE;
DECLARE isShiftRestDay BOOLEAN DEFAULT FALSE;
DECLARE restDayHours DECIMAL(15, 4) DEFAULT 0.0;
DECLARE restDayAmount DECIMAL(15, 4) DEFAULT 0.0;

DECLARE holidayPay DECIMAL(15, 4) DEFAULT 0.0;
DECLARE isExemptForHoliday BOOLEAN DEFAULT FALSE;

DECLARE lateHoursBeforeBreak DECIMAL(11, 6) DEFAULT 0.0;
DECLARE lateHoursAfterBreak DECIMAL(11, 6) DEFAULT 0.0;
DECLARE lateHours DECIMAL(11, 2) DEFAULT 0.0;
DECLARE lateAmount DECIMAL(15, 4) DEFAULT 0.0;

DECLARE undertimeHoursBeforeBreak DECIMAL(11, 6) DEFAULT 0.0;
DECLARE undertimeHoursAfterBreak DECIMAL(11, 6) DEFAULT 0.0;
DECLARE undertimeHours DECIMAL(11, 6) DEFAULT 0.0;
DECLARE undertimeAmount DECIMAL(15, 4) DEFAULT 0.0;

DECLARE absentHours DECIMAL(15, 4) DEFAULT 0.0;
DECLARE absentAmount DECIMAL(15, 4) DEFAULT 0.0;

DECLARE hasLeave BOOLEAN DEFAULT FALSE;
DECLARE leaveStartTime TIME;
DECLARE leaveEndTime TIME;
DECLARE leaveStart DATETIME;
DECLARE leaveEnd DATETIME;
DECLARE leaveType VARCHAR(50);

DECLARE leaveHoursBeforeBreak DECIMAL(15, 4) DEFAULT 0.0;
DECLARE leaveHoursAfterBreak DECIMAL(15, 4) DEFAULT 0.0;
DECLARE leaveHours DECIMAL(15, 4) DEFAULT 0.0;
DECLARE leavePay DECIMAL(15, 4) DEFAULT 0.0;

DECLARE basicDayPay DECIMAL(15, 4) DEFAULT 0.0;

DECLARE hasWorkedLastWorkingDay BOOLEAN DEFAULT FALSE;

DECLARE applicableHolidayRate DECIMAL(11, 6) DEFAULT 0.0;

DECLARE isRestDayInclusive BOOLEAN DEFAULT FALSE;

SELECT
    e.EmploymentStatus,
    e.EmployeeType,
    e.MaritalStatus,
    e.StartDate,
    e.PayFrequencyID,
    e.NoOfDependents,
    e.UndertimeOverride,
    e.OvertimeOverride,
    e.WorkDaysPerYear,
    (e.CalcHoliday = 1),
    (e.CalcSpecialHoliday = 1),
    (e.CalcNightDiff = 1),
    (e.CalcNightDiffOT = 1),
    (e.CalcRestDay = 1),
    (e.CalcRestDayOT = 1),
    e.LateGracePeriod,
    e.PositionID,
    og.NightDifferentialTimeFrom,
    og.NightDifferentialTimeTo
FROM employee e
INNER JOIN organization og
ON og.RowID = e.OrganizationID
WHERE e.RowID = ete_EmpRowID
INTO
    e_EmpStatus,
    e_EmpType,
    e_MaritStatus,
    e_StartDate,
    e_PayFreqID,
    e_NumDependent,
    e_UTOverride,
    e_OTOverride,
    e_DaysPerYear,
    isEntitledToRegularHoliday,
    isEntitledToSpecialNonWorkingHoliday,
    isEntitledToNightDiff,
    isEntitledToNightDiffOvertime,
    isEntitledToRestDay,
    isEntitledToRestDayOvertime,
    e_LateGracePeriod,
    e_PositionID,
    nightDiffTimeFrom,
    nightDiffTimeTo;

SET requiredToWorkLastWorkingDay = GetListOfValueOrDefault(
    'Payroll Policy', 'HolidayLastWorkingDayOrAbsent', FALSE
);

SET isRestDayInclusive = GetListOfValueOrDefault(
    'Payroll Policy', 'restday.inclusiveofbasicpay', FALSE
);

SET allowAbsenceOnHoliday = GetListOfValueOrDefault(
    'Payroll Policy', 'holiday.allowabsence', FALSE
);

SELECT
    RowID,
    IF(
        PayType = 'Special Non-Working Holiday',
        IF(
            isEntitledToSpecialNonWorkingHoliday,
            PayRate,
            BASIC_RATE
        ),
        IF(
            PayType = 'Regular Holiday',
            IF(
                isEntitledToRegularHoliday,
                `PayRate`,
                BASIC_RATE
            ),
            `PayRate`
        )
    ),
    IF(e_OTOverride = '1', OvertimeRate, 1),
    IF(isEntitledToNightDiff, NightDifferentialRate, 1),
    IF(isEntitledToNightDiffOvertime, NightDifferentialOTRate, 1),
    IF(isEntitledToRestDay, RestDayRate, 1),
    IF(isEntitledToRestDayOvertime, RestDayOvertimeRate, OvertimeRate),
    (PayType = 'Regular Holiday'),
    (PayType = 'Special Non-Working Holiday')
FROM payrate
WHERE `Date` = ete_Date
    AND OrganizationID = ete_OrganizID
INTO
    payrateRowID,
    commonrate,
    otrate,
    ndiffrate,
    ndiffotrate,
    restday_rate,
    restdayot_rate,
    isRegularHoliday,
    isSpecialNonWorkingHoliday;

SET isCalculatingRegularHoliday = isRegularHoliday AND isEntitledToRegularHoliday;
SET isCalculatingSpecialNonWorkingHoliday = isSpecialNonWorkingHoliday AND isEntitledToSpecialNonWorkingHoliday;
SET isHoliday = isRegularHoliday OR isSpecialNonWorkingHoliday;
SET isRegularDay = NOT isHoliday;

SELECT COUNT(RowID)
FROM employeeovertime
WHERE EmployeeID = ete_EmpRowID AND
    OrganizationID = ete_OrganizID AND
    ete_Date BETWEEN OTStartDate AND OTEndDate AND
    OTStatus = 'Approved'
INTO OTCount;

SELECT COALESCE(DAYOFWEEK(ete_Date) = e.DayOfRest, FALSE)
FROM employee e
WHERE e.RowID = ete_EmpRowID
INTO isDefaultRestDay;

SELECT
    esh.RowID,
    sh.RowID,
    sh.TimeFrom,
    sh.TimeTo,
    sh.BreakTimeFrom,
    sh.BreakTimeTo,
    IFNULL(esh.NightShift = '1', FALSE),
    COALESCE(esh.RestDay, TRUE),
    sh.DivisorToDailyRate
FROM employeeshift esh
LEFT JOIN shift sh
ON sh.RowID = esh.ShiftID
WHERE esh.EmployeeID = ete_EmpRowID AND
    esh.OrganizationID = ete_OrganizID AND
    ete_Date BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
ORDER BY DATEDIFF(ete_Date, esh.EffectiveFrom)
LIMIT 1
INTO
    employeeShiftID,
    shiftID,
    shifttimefrom,
    shifttimeto,
    @sh_brktimeFr,
    @sh_brktimeTo,
    isNightShift,
    isShiftRestDay,
    workingHours;

SET hasShift = (shifttimefrom IS NOT NULL) AND (shifttimeto IS NOT NULL);
SET isRestDay = isShiftRestDay;
SET isWorkingDay = NOT isRestDay;

SET workingHours = IF(hasShift, workingHours, STANDARD_WORKING_HOURS);

IF OTCount = 1 THEN

    SELECT
        OTStartTime,
        OTEndTime,
        RowID
    FROM employeeovertime
    WHERE EmployeeID = ete_EmpRowID AND
        OrganizationID = ete_OrganizID AND
        -- OTStartTime >= shifttimeto AND
        OTStatus = 'Approved' AND
        (ete_Date BETWEEN OTStartDate AND OTEndDate)
    ORDER BY OTStartTime DESC
    LIMIT 1
    INTO
        otstartingtime,
        otendingtime,
        aftershiftOTRowID;

ELSE

    SELECT
        OTStartTime,
        OTEndTime,
        RowID
    FROM employeeovertime
    WHERE EmployeeID = ete_EmpRowID
        AND OrganizationID = ete_OrganizID
        AND ete_Date BETWEEN OTStartDate AND COALESCE(OTEndDate,OTStartDate)
        AND OTStatus = 'Approved'
    ORDER BY OTStartTime DESC
    LIMIT 1
    INTO
        otstartingtime,
        otendingtime,
        aftershiftOTRowID;

END IF;

SELECT
    etd.TimeIn,
    IF(e_UTOverride = 1, etd.TimeOut, IFNULL(sh.TimeTo, etd.TimeOut))
FROM employeetimeentrydetails etd
INNER JOIN employee e ON e.RowID=etd.EmployeeID
LEFT JOIN employeeshift esh
ON esh.OrganizationID = etd.OrganizationID AND
    esh.EmployeeID = etd.EmployeeID AND
    etd.`Date` BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
LEFT JOIN shift sh
ON sh.RowID = esh.ShiftID
WHERE etd.EmployeeID = ete_EmpRowID AND
    etd.OrganizationID = ete_OrganizID AND
    etd.`Date` = ete_Date
ORDER BY IFNULL(etd.LastUpd, etd.Created) DESC
LIMIT 1
INTO
    actualTimeIn,
    etd_TimeOut;

SET dateToday = ete_Date;
SET dateTomorrow = DATE_ADD(dateToday, INTERVAL 1 DAY);
SET dateYesterday = DATE_SUB(dateToday, INTERVAL 1 DAY);

SELECT
    MIN(ofb.OffBusStartTime),
    MAX(ofb.OffBusEndTime)
FROM employeeofficialbusiness ofb
WHERE ofb.OffBusStartDate = dateToday AND
    ofb.EmployeeID = ete_EmpRowID
INTO
    officialBusStartTime,
    officialBusEndTime;

SET actualTimeIn = IF(officialBusStartTime IS NULL, actualTimeIn, LEAST(actualTimeIn, officialBusStartTime));
SET etd_TimeOut = IF(officialBusEndTime IS NULL, etd_TimeOut, GREATEST(etd_TimeOut, officialBusEndTime));

SELECT GRACE_PERIOD(actualTimeIn, shifttimefrom, e_LateGracePeriod)
INTO etd_TimeIn;

SET fullTimeIn = TIMESTAMP(dateToday, etd_TimeIn);
SET fullTimeOut = TIMESTAMP(IF(etd_TimeOut > etd_TimeIn, dateToday, dateTomorrow), etd_TimeOut);

SET shiftStart = TIMESTAMP(dateToday, shifttimefrom);
SET shiftEnd = TIMESTAMP(IF(shifttimeto > shifttimefrom, dateToday, dateTomorrow), shifttimeto);

SET breaktimeStart = TIMESTAMP(IF(@sh_brktimeFr > shifttimefrom, dateToday, dateTomorrow), @sh_brktimeFr);
SET breaktimeEnd = TIMESTAMP(IF(@sh_brktimeTo > shifttimefrom, dateToday, dateTomorrow), @sh_brktimeTo);

SET hasBreaktime = (@sh_brktimeFr IS NOT NULL) AND (@sh_brktimeTo IS NOT NULL);

IF hasBreaktime THEN
    SET shiftHours =
        COMPUTE_TimeDifference(TIME(shiftStart), TIME(breaktimeStart)) +
        COMPUTE_TimeDifference(TIME(breaktimeEnd), TIME(shiftEnd));
ELSE
    SET shiftHours = COMPUTE_TimeDifference(TIME(shiftStart), TIME(shiftEnd));
END IF;

SELECT
    COUNT(elv.RowID) > 0,
    elv.LeaveStartTime,
    elv.LeaveEndTime,
    elv.LeaveType
FROM employeeleave elv
WHERE elv.EmployeeID = ete_EmpRowID AND
    elv.`Status` = 'Approved' AND
    elv.OrganizationID = ete_OrganizID AND
    ete_Date BETWEEN elv.LeaveStartDate AND elv.LeaveEndDate AND
    elv.LeaveType != 'Leave w/o Pay'
LIMIT 1
INTO
    hasLeave,
    leaveStartTime,
    leaveEndTime,
    leaveType;

SET leaveStart = TIMESTAMP(dateToday, leaveStartTime);
SET leaveEnd = TIMESTAMP(IF(leaveEndTime > leaveStartTime, dateToday, dateTomorrow), leaveEndTime);

SET @coveredStart = IF(leaveStart IS NULL, dutyStart, LEAST(dutyStart, leaveStart));
SET @coveredEnd = IF(leaveEnd IS NULL, dutyEnd, GREATEST(dutyEnd, leaveEnd));

/*
 * The official work start is the time that is considered the employee has started working.
 * In this case, the work start is the time in, unless the employee went in early, then it should
 * just be the start of the shift.
 */
SET dutyStart = GREATEST(fullTimeIn, shiftStart);
/*
 * The official work end is the time that is considered the employee has stopped working.
 * It should be the end of the shift, unless the employee timed out early, then it should be the
 * time out.
 */
SET dutyEnd = LEAST(fullTimeOut, shiftEnd);

/******************************************************************************
 ******************************************************************************
 * Compute the Regular hours
 ******************************************************************************
 ******************************************************************************/

/*
 * Calculate the regular hours worked for the day.
 */
IF hasBreaktime THEN
    /*
     * If there is a breaktime, split the computation between the work done before breaktime,
     * and the work done after breaktime.
     */
    IF dutyStart < breaktimeStart THEN
        SET @lastWorkBeforeBreaktime = LEAST(dutyEnd, breaktimeStart);

        SET regularHoursBeforeBreak = COMPUTE_TimeDifference(TIME(dutyStart), TIME(@lastWorkBeforeBreaktime));
    END IF;

    IF dutyEnd > breaktimeEnd THEN
        /*
         * Let's make sure that we calculate the correct work hours after breaktime by ensuring that we don't choose the
         * breaktime's end when the employee started work after breaktime.
         */
        SET @workStartAfterBreaktime = GREATEST(breaktimeEnd, dutyStart);

        SET regularHoursAfterBreak = COMPUTE_TimeDifference(TIME(@workStartAfterBreaktime), TIME(dutyEnd));
    END IF;

    SET regularHours = regularHoursBeforeBreak + regularHoursAfterBreak;
ELSE
    /*
     * If there is no breaktime, just compute the time spanning from the duty start and end.
     */
    SET regularHours = COMPUTE_TimeDifference(TIME(dutyStart), TIME(dutyEnd));
END IF;

SET hasWorked = regularHours > 0;

/******************************************************************************
 ******************************************************************************
 * Compute the Late hours
 ******************************************************************************
 ******************************************************************************/

/*
 * First check if the duty start is above shift start to check if the employee is late.
 */
IF @coveredStart > shiftStart THEN

    IF hasBreaktime THEN

        IF shiftStart < breaktimeStart THEN
            /*
             * Make sure that the late period doesn't include part of the breaktime since that is not
             * part of the required work hours.
             */
            SET @latePeriodEndBeforeBreaktime = LEAST(@coveredStart, breaktimeStart);

            SET lateHoursBeforeBreak = COMPUTE_TimeDifference(TIME(shiftStart), TIME(@latePeriodEndBeforeBreaktime));
        END IF;

        IF @coveredStart > breaktimeEnd THEN
            SET lateHoursAfterBreak = COMPUTE_TimeDifference(TIME(breaktimeEnd), TIME(@coveredStart));
        END IF;

        SET lateHours = lateHoursBeforeBreak + lateHoursAfterBreak;
    ELSE
        SET lateHours = COMPUTE_TimeDifference(TIME(shiftStart), TIME(@coveredStart));
    END IF;

END IF;

/******************************************************************************
 ******************************************************************************
 * Compute the Undertime hours
 ******************************************************************************
 ******************************************************************************/

/*
 * First check if the duty ends before the shift ends to check if the employee committed undertime.
 */
IF @coveredEnd < shiftEnd THEN

    IF hasBreaktime THEN

        IF @coveredEnd < breaktimeStart THEN
            SET undertimeHoursBeforeBreak = COMPUTE_TimeDifference(TIME(@coveredEnd), TIME(breaktimeStart));
        END IF;

        /*
         * Calculate the remaining undertime that happened after breaktime.
         */
        SET @undertimePeriodStartAfterBreaktime = GREATEST(@coveredEnd, breaktimeEnd);

        SET undertimeHoursAfterBreak = COMPUTE_TimeDifference(TIME(@undertimePeriodStartAfterBreaktime), TIME(shiftEnd));

        SET undertimeHours = undertimeHoursBeforeBreak + undertimeHoursAfterBreak;
    ELSE
        SET undertimeHours = COMPUTE_TimeDifference(TIME(@coveredEnd), TIME(shiftEnd));
    END IF;

END IF;

/******************************************************************************
 ******************************************************************************
 * Compute the Night Differential hours
 ******************************************************************************
 ******************************************************************************/
SET nightDiffRangeStart = TIMESTAMP(dateToday, nightDiffTimeFrom);
SET nightDiffRangeEnd = TIMESTAMP(IF(nightDiffTimeTo > nightDiffTimeFrom, ete_Date, dateTomorrow), nightDiffTimeTo);

SET dawnNightDiffRangeStart = TIMESTAMP(dateYesterday, nightDiffTimeFrom);
SET dawnNightDiffRangeEnd = TIMESTAMP(IF(nightDiffTimeTo > nightDiffTimeFrom, dateYesterday, dateToday), nightDiffTimeTo);

SET shouldCalculateNightDifferential = (
    isNightShift AND
    isEntitledToNightDiff
);

IF shouldCalculateNightDifferential THEN
    SET nightDiffHours =
        ComputeNightDiffHours(dutyStart, dutyEnd, nightDiffRangeStart, nightDiffRangeEnd) +
        ComputeNightDiffHours(dutyStart, dutyEnd, dawnNightDiffRangeStart, dawnNightDiffRangeEnd);
END IF;

/******************************************************************************
 ******************************************************************************
 * Compute the Overtime and Night Differential Overtime hours
 ******************************************************************************
 ******************************************************************************/
SET hasOvertime = (otstartingtime IS NOT NULL) AND (otendingtime IS NOT NULL);

IF hasOvertime THEN

    SET overtimeDate = DATE(shiftEnd);

    SET overtimeStart = TIMESTAMP(overtimeDate, otstartingtime);
    SET overtimeEnd = TIMESTAMP(IF(otendingtime > otstartingtime, overtimeDate, dateTomorrow), otendingtime);

    SET @preShiftOvertimeHours = 0;

    /*
     * Compute the overtime hours for pre-shift work.
     */
    IF overtimeStart < shiftStart THEN

        /*
         * Ensure that the overtime hours are not calculated until the employee has clocked in.
         */
        SET overtimeDutyStart = GREATEST(overtimeStart, fullTimeIn);

        /*
         * Ensure that the overtime hours stop computing when either the overtime has ended,
         * the employee has clocked out, or the regular shift has already started.
         */
        SET overtimeDutyEnd = LEAST(overtimeEnd, fullTimeOut, shiftStart);

        SET @preShiftOvertimeHours = COMPUTE_TimeDifference(TIME(overtimeDutyStart), TIME(overtimeDutyEnd));

        IF shouldCalculateNightDifferential THEN
            SET nightDiffOTHours =
                ComputeNightDiffHours(overtimeDutyStart, overtimeDutyEnd, nightDiffRangeStart, nightDiffRangeEnd) +
                ComputeNightDiffHours(overtimeDutyStart, overtimeDutyEnd, dawnNightDiffRangeStart, dawnNightDiffRangeEnd);
        END IF;

    END IF;

    SET @postShiftOvertimeHours = 0;

    /*
     * Compute the overtime hours for post-shift work.
     */
    IF overtimeEnd > shiftEnd THEN

        SET overtimeDutyStart = LEAST(
            GREATEST(overtimeStart, fullTimeIn, shiftEnd),
            fullTimeOut
        );
        SET overtimeDutyEnd = LEAST(overtimeEnd, fullTimeOut);

        SET @postShiftOvertimeHours = COMPUTE_TimeDifference(TIME(overtimeDutyStart), TIME(overtimeDutyEnd));

        IF shouldCalculateNightDifferential THEN
            SET nightDiffOTHours = nightDiffOTHours +
                ComputeNightDiffHours(overtimeDutyStart, overtimeDutyEnd, nightDiffRangeStart, nightDiffRangeEnd) +
                ComputeNightDiffHours(overtimeDutyStart, overtimeDutyEnd, dawnNightDiffRangeStart, dawnNightDiffRangeEnd);
        END IF;

    END IF;

END IF;

/*
 * If the hours worked is in excess of the working hours, put that extra hours into
 * overtime.
 */
IF isRestDay AND (regularHours > workingHours) THEN
    SET @excessHours = regularHours - workingHours;
    SET regularHours = workingHours;

    SET overtimeHours = @excessHours;
END IF;

IF isDefaultRestDay AND (NOT hasWorked) THEN
    SET employeeShiftID = NULL;
    SET hasShift = FALSE;
END IF;

IF hasLeave THEN
    IF hasBreaktime THEN
        IF leaveStart < breaktimeStart THEN
            SET @leavePeriodEndBeforeBreaktime = LEAST(leaveEnd, breaktimeStart);

            SET leaveHoursBeforeBreak = COMPUTE_TimeDifference(TIME(leaveStart), TIME(@leavePeriodEndBeforeBreaktime));

        END IF;

        IF leaveEnd > breaktimeEnd THEN
            SET @leavePeriodStartAfterBreaktime = GREATEST(breaktimeEnd, leaveStart);

            SET leaveHoursAfterBreak = COMPUTE_TimeDifference(TIME(@leavePeriodStartAfterBreaktime), TIME(leaveEnd));

        END IF;

        SET leaveHours = leaveHoursBeforeBreak + leaveHoursAfterBreak;
    ELSE
        # SET leaveHours = COMPUTE_TimeDifference(TIME(leaveStart), TIME(leaveEnd));
        SET leaveHours = TIMESTAMPDIFF(SECOND
		                                 , CONCAT_DATETIME(ete_Date, leaveStartTime)
													, CONCAT_DATETIME(ADDDATE(ete_Date
											                                , INTERVAL IS_TIMERANGE_REACHTOMORROW(leaveStartTime, leaveEndTime) DAY)
																			, leaveEndTime)) / SECONDS_PER_HOUR;

    END IF;
END IF;

/******************************************************************************
 ******************************************************************************
 * Compute the Absent hours
 ******************************************************************************
 ******************************************************************************/
SET hasWorkedLastWorkingDay = HasWorkedLastWorkingDay(ete_EmpRowID, dateToday);

SET isExemptForHoliday = (
    (
        (isHoliday AND (NOT requiredToWorkLastWorkingDay)) OR
        (isHoliday AND hasWorkedLastWorkingDay)
    ) AND
    (
        isCalculatingRegularHoliday OR
        isCalculatingSpecialNonWorkingHoliday OR
        (NOT allowAbsenceOnHoliday)
    )
);

IF (NOT hasShift) OR hasWorked OR isRestDay OR isExemptForHoliday OR hasLeave THEN
    SET absentHours = 0;
ELSE
    SET absentHours = shiftHours;
END IF;

/*
 * If the employee filed a leave for a day for which the leave hours is not enough,
 * file the unaccounted hours as absent hours.
 */
IF hasLeave AND (leaveHours + regularHours) < shiftHours THEN
    SET absentHours = shiftHours - (leaveHours + regularHours);
END IF;

SET regularHours = IFNULL(regularHours, 0);
SET overtimeHours = GetOvertimeHours(ete_OrganizID, ete_EmpRowID, ete_Date);
SET nightDiffHours = IFNULL(nightDiffHours, 0);
SET nightDiffOTHours = IFNULL(nightDiffOTHours, 0);
SET lateHours = IFNULL(lateHours, 0);
SET undertimeHours = IFNULL(undertimeHours, 0);

SELECT RowID
FROM employeetimeentry
WHERE EmployeeID = ete_EmpRowID AND
    OrganizationID = ete_OrganizID AND
    `Date` = ete_Date
LIMIT 1
INTO timeEntryID;

SELECT RowID
FROM employeesalary
WHERE EmployeeID = ete_EmpRowID AND
    OrganizationID = ete_OrganizID AND
    ete_Date BETWEEN DATE(COALESCE(EffectiveDateFrom, DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveDateTo, ete_Date)) AND
    DATEDIFF(ete_Date,EffectiveDateFrom) >= 0
ORDER BY DATEDIFF(DATE_FORMAT(ete_Date,'%Y-%m-%d'),EffectiveDateFrom)
LIMIT 1
INTO esalRowID;

/******************************************************************************
 ******************************************************************************
 * COMPUTE PAY
 ******************************************************************************
 ******************************************************************************/
SET ndiffrate = ndiffrate MOD 1;
SET ndiffotrate = otrate * ndiffrate;

SET dailyRate = GET_employeerateperday(ete_EmpRowID, ete_OrganizID, dateToday);
SET hourlyRate = dailyRate / workingHours;
SET basicDayPay = regularHours * hourlyRate;

SET absentAmount = absentHours * hourlyRate;

IF hasLeave AND isWorkingDay THEN
    SET leavePay = IFNULL(leaveHours * hourlyRate, 0);
END IF;

-- b. If current day is before employment hiring date.
IF ete_Date < e_StartDate THEN

    SET ete_TotalDayPay = 0.0;

    SELECT INSUPD_employeetimeentries(
        timeEntryID,
        ete_OrganizID,
        ete_UserRowID,
        ete_UserRowID,
        ete_Date,
        employeeShiftID,
        ete_EmpRowID,
        esalRowID,
        '0',
        regularHours,
        overtimeHours,
        undertimeHours,
        nightDiffHours,
        nightDiffOTHours,
        lateHours,
        payrateRowID,
        ete_TotalDayPay,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        1,
        NULL,
        0,
        0,
        0,
        0
    )
    INTO timeEntryID;

ELSEIF isRegularDay THEN

    IF isWorkingDay THEN
        SET regularAmount = (regularHours * hourlyRate) * commonrate;
        SET overtimeAmount = (overtimeHours * hourlyRate) * otrate;

        SET lateAmount = lateHours * hourlyRate;
        SET undertimeAmount = undertimeHours * hourlyRate;
    ELSEIF isRestDay THEN

        IF isRestDayInclusive AND e_EmpType = 'Monthly' THEN
            SET restDayAmount = (regularHours * hourlyRate) * (restday_rate - 1);
        ELSE
            SET restDayAmount = (regularHours * hourlyRate) * restday_rate;
        END IF;

        SET overtimeAmount = (overtimeHours * hourlyRate) * restdayot_rate;

        SET lateHours = 0.0;
        SET undertimeHours = 0.0;
    END IF;

    SET nightDiffAmount = (nightDiffHours * hourlyRate) * ndiffrate;
    SET nightDiffOTAmount = (nightDiffOTHours * hourlyRate) * ndiffotrate;

    SET ete_TotalDayPay = regularAmount + overtimeAmount +
                          nightDiffAmount + nightDiffOTAmount +
                          restDayAmount + leavePay;

    SELECT INSUPD_employeetimeentries(
        timeEntryID,
        ete_OrganizID,
        ete_UserRowID,
        ete_UserRowID,
        ete_Date,
        employeeShiftID,
        ete_EmpRowID,
        esalRowID,
        '0',
        regularHours,
        overtimeHours,
        undertimeHours,
        nightDiffHours,
        nightDiffOTHours,
        lateHours,
        payrateRowID,
        ete_TotalDayPay,
        regularHours + overtimeHours,
        regularAmount,
        overtimeAmount,
        undertimeAmount,
        nightDiffAmount,
        nightDiffOTAmount,
        lateAmount,
        restDayHours,
        restDayAmount,
        0,
        basicDayPay,
        5,
        leaveType,
        leaveHours,
        leavePay,
        absentHours,
        absentAmount
    )
    INTO timeEntryID;

ELSEIF isHoliday THEN

    IF isRestDay THEN
        SET regularAmount = (regularHours * hourlyRate);
        SET overtimeAmount = (overtimeHours * hourlyRate) * restdayot_rate;

        SET applicableHolidayRate = restday_rate;
    ELSE
        SET regularAmount = (regularHours * hourlyRate);
        SET overtimeAmount = (overtimeHours * hourlyRate) * otrate;

        SET applicableHolidayRate = commonrate;
    END IF;

    IF isCalculatingRegularHoliday OR
       isCalculatingSpecialNonWorkingHoliday THEN

        SET lateHours = 0.0;
        SET undertimeHours = 0.0;
    END IF;

    SET nightDiffAmount = (nightDiffHours * hourlyRate) * ndiffrate;
    SET nightDiffOTAmount = (nightDiffOTHours * hourlyRate) * ndiffotrate;

    IF isRegularHoliday THEN

        IF e_EmpType = 'Daily' THEN
            SET holidayPay = IF(hasWorkedLastWorkingDay, dailyRate, 0);
        ELSEIF e_EmpType = 'Monthly' THEN
            SET holidayPay = regularAmount * (applicableHolidayRate - 1);
        END IF;

    ELSEIF isSpecialNonWorkingHoliday THEN
        SET holidayPay = regularAmount * (applicableHolidayRate - 1);
    ELSE
        SET holidayPay = 0;
    END IF;

    SET ete_TotalDayPay = COALESCE(regularAmount, 0) +
                          COALESCE(overtimeAmount, 0) +
                          COALESCE(nightDiffAmount, 0) +
                          COALESCE(nightDiffOTAmount, 0) +
                          COALESCE(holidayPay, 0) +
                          COALESCE(leavePay, 0);

    SELECT INSUPD_employeetimeentries(
        timeEntryID,
        ete_OrganizID,
        ete_UserRowID,
        ete_UserRowID,
        ete_Date,
        employeeShiftID,
        ete_EmpRowID,
        esalRowID,
        '0',
        regularHours,
        overtimeHours,
        undertimeHours,
        nightDiffHours,
        nightDiffOTHours,
        lateHours,
        payrateRowID,
        ete_TotalDayPay,
        regularHours + overtimeHours,
        regularAmount,
        overtimeAmount,
        (undertimeHours * hourlyRate),
        nightDiffAmount,
        nightDiffOTAmount,
        (lateHours * hourlyRate),
        0,
        0,
        holidayPay,
        basicDayPay,
        7,
        leaveType,
        leaveHours,
        leavePay,
        absentHours,
        absentAmount
    )
    INTO timeEntryID;

END IF;


SET returnvalue = timeEntryID;

RETURN yes_true;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
