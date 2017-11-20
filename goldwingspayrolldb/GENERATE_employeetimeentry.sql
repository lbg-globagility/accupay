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

DECLARE DAYTYPE_REGULAR_WORKING_DAY VARCHAR(50) DEFAULT 'Regular Working Day';
DECLARE DAYTYPE_SPECIAL_NON_WORKING_HOLIDAY VARCHAR(50) DEFAULT 'Special Non-Working Holiday';
DECLARE DAYTYPE_REGULAR_HOLIDAY VARCHAR(50) DEFAULT 'Regular Holiday';

DECLARE MAX_REGULAR_HOURS INT(10) DEFAULT 8;

DECLARE BASIC_RATE INT(10) DEFAULT 1;

DECLARE returnvalue INT(11);

DECLARE pr_PayType TEXT;

DECLARE isRestDay TEXT;
DECLARe isWorkingDay BOOLEAN;

DECLARE hasTimeLogs TEXT;

DECLARE yester_TotDayPay DECIMAL(11,2);
DECLARE yester_TotHrsWorkd DECIMAL(11,2);

DECLARE ete_RegHrsWorkd DECIMAL(11,6);
DECLARE ete_OvertimeHrs DECIMAL(11,6);
DECLARE ete_NDiffHrs DECIMAL(11,6);
DECLARE ete_NDiffOTHrs DECIMAL(11,6);
DECLARE ete_HrsLate DECIMAL(11,6);
DECLARE ete_HrsUnder DECIMAL(11,6);

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

DECLARE divisorToDailyRate INT(11) DEFAULT 0;

DECLARE sh1 TIME DEFAULT NULL;
DECLARE sh2 TIME DEFAULT NULL;

DECLARE dateToday DATE;
DECLARE dateTomorrow DATE;
DECLARE dateYesterday DATE;

DECLARE fullTimeIn DATETIME;
DECLARE fullTimeOut DATETIME;
DECLARE shiftStart DATETIME;
DECLARE shiftEnd DATETIME;
DECLARE breaktimeStart DATETIME;
DECLARE breaktimeEnd DATETIME;
DECLARE hasBreaktime BOOLEAN;

DECLARE isRegularDay BOOLEAN;
DECLARE isSpecialNonWorkingHoliday BOOLEAN;
DECLARE isRegularHoliday BOOLEAN;
DECLARE isHoliday BOOLEAN;

DECLARE dutyStart DATETIME;
DECLARE dutyEnd DATETIME;

DECLARE regularHoursBeforeBreak DECIMAL(11, 6) DEFAULT 0.0;
DECLARE regularHoursAfterBreak DECIMAL(11, 6) DEFAULT 0.0;
DECLARE regularHours DECIMAL(11, 6) DEFAULT 0.0;
DECLARE regularAmount DECIMAL(11, 6) DEFAULT 0.0;

DECLARE isNightShift BOOLEAN;
DECLARE isEntitledToNightDifferential BOOLEAN;

DECLARE nightDiffTimeFrom TIME;
DECLARE nightDiffTimeTo TIME;
DECLARE nightDiffRangeStart DATETIME;
DECLARE nightDiffRangeEnd DATETIME;
DECLARE dawnNightDiffRangeStart DATETIME;
DECLARE dawnNightDiffRangeEnd DATETIME;

DECLARE nightDiffHours DECIMAL(11, 6);
DECLARE nightDiffAmount DECIMAL(11, 6) DEFAULT 0.0;
DECLARE isDutyOverlappedWithNightDifferential BOOLEAN;
DECLARE shouldCalculateNightDifferential BOOLEAN;

DECLARE overtimeStart DATETIME;
DECLARE overtimeEnd DATETIME;
DECLARE overtimeDate DATE;
DECLARE hasOvertime BOOLEAN;

DECLARE overtimeDutyStart DATETIME;
DECLARE overtimeDutyEnd DATETIME;
DECLARE overtimeHours DECIMAL(12, 6);
DECLARE overtimeAmount DECIMAL(11, 6) DEFAULT 0.0;

DECLARE nightDiffOTDutyStart DATETIME;
DECLARE nightDiffOTDutyEnd DATETIME;
DECLARE nightDiffOTHours DECIMAL(11, 6);
DECLARE nightDiffOTAmount DECIMAL(11, 6) DEFAULT 0.0;
DECLARE isOvertimeOverlappedNightDifferential BOOLEAN;
DECLARE shouldCalculateNightDifferentialOvertime BOOLEAN;

DECLARE isDefaultRestDay BOOLEAN;
DECLARE isShiftRestDay BOOLEAN;
DECLARE restDayHours DECIMAL(15, 4);
DECLARE restDayAmount DECIMAL(15, 4) DEFAULT 0.0;

DECLARE holidayPay DECIMAL(15, 4) DEFAULT 0.0;

DECLARE lateHoursBeforeBreak DECIMAL(11, 6) DEFAULT 0.0;
DECLARE lateHoursAfterBreak DECIMAL(11, 6) DEFAULT 0.0;
DECLARE lateHours DECIMAL(11, 6) DEFAULT 0.0;
DECLARE lateAmount DECIMAL(15, 4) DEFAULT 0.0;

DECLARE undertimeHoursBeforeBreak DECIMAL(11, 6) DEFAULT 0.0;
DECLARE undertimeHoursAfterBreak DECIMAL(11, 6) DEFAULT 0.0;
DECLARE undertimeHours DECIMAL(11, 6) DEFAULT 0.0;
DECLARE undertimeAmount DECIMAL(15, 4) DEFAULT 0.0;

DECLARE hasLeave BOOLEAN DEFAULT FALSE;
DECLARE leaveStartTime TIME;
DECLARE leaveEndTime TIME;
DECLARE leaveStart DATETIME;
DECLARE leaveEnd DATETIME;
DECLARE leaveType VARCHAR(50);

DECLARE leaveHoursBeforeBreak DECIMAL(15, 4) DEFAULT 0.0;
DECLARE leaveHoursAfterBreak DECIMAL(15, 4) DEFAULT 0.0;
DECLARE leaveHours DECIMAL(15, 4);
DECLARE leavePay DECIMAL(15, 4) DEFAULT 0.0;

DECLARE basicDayPay DECIMAL(12, 4);

DECLARE hasWorkedLastWorkingDay BOOLEAN;

DECLARE applicableRegularRate DECIMAL(11, 6);
DECLARE applicableOvertimeRate DECIMAL(11, 6);
DECLARE applicableHolidayRate DECIMAL(11, 6);

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
    e.CalcHoliday,
    e.CalcSpecialHoliday,
    e.CalcNightDiff,
    e.CalcNightDiffOT,
    e.CalcRestDay,
    e.CalcRestDayOT,
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
    calc_Holiday,
    e_CalcSpecialHoliday,
    e_CalcNightDiff,
    e_CalcNightDiffOT,
    e_CalcRestDay,
    e_CalcRestDayOT,
    e_LateGracePeriod,
    e_PositionID,
    nightDiffTimeFrom,
    nightDiffTimeTo;

SELECT
    RowID,
    IF(
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
    ),
    IF(e_OTOverride = '1', OvertimeRate, 1),
    IF(e_CalcNightDiff = '1', NightDifferentialRate, 1),
    IF(e_CalcNightDiffOT = '1', NightDifferentialOTRate, 1),
    IF(e_CalcRestDay = '1', RestDayRate, 1),
    IF(e_CalcRestDayOT = '1', RestDayOvertimeRate, 1),
    PayType
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
    pr_PayType;

SELECT
    IFNULL((NightShift = '1'), FALSE)
FROM employeeshift
WHERE EmployeeID = ete_EmpRowID AND
    OrganizationID = ete_OrganizID AND
    ete_Date BETWEEN EffectiveFrom AND EffectiveTo AND
    DATEDIFF(ete_Date, EffectiveFrom) >= 0
ORDER BY DATEDIFF(ete_Date, EffectiveFrom)
LIMIT 1
INTO
    isNightShift;

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
    COALESCE(esh.RestDay, TRUE)
FROM employeeshift esh
INNER JOIN shift sh
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
    isShiftRestDay;

SET isRestDay = isShiftRestDay;

IF OTCount = 1 THEN

    SELECT
        OTStartTime,
        OTEndTime,
        RowID
    FROM employeeovertime
    WHERE EmployeeID = ete_EmpRowID
        AND OrganizationID = ete_OrganizID
        AND OTStartTime >= shifttimeto
        AND OTStatus = 'Approved'
        AND (ete_Date BETWEEN OTStartDate AND OTEndDate)
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
    etd_TimeIn,
    etd_TimeOut;

SET dateToday = ete_Date;
SET dateTomorrow = DATE_ADD(dateToday, INTERVAL 1 DAY);
SET dateYesterday = DATE_SUB(dateToday, INTERVAL 1 DAY);

SELECT GRACE_PERIOD(etd_TimeIn, shifttimefrom, e_LateGracePeriod)
INTO etd_TimeIn;

SET fullTimeIn = TIMESTAMP(dateToday, etd_TimeIn);
SET fullTimeOut = TIMESTAMP(IF(etd_TimeOut > etd_TimeIn, dateToday, dateTomorrow), etd_TimeOut);

SET shiftStart = TIMESTAMP(dateToday, shifttimefrom);
SET shiftEnd = TIMESTAMP(IF(shifttimeto > shifttimefrom, dateToday, dateTomorrow), shifttimeto);

SET breaktimeStart = TIMESTAMP(IF(@sh_brktimeFr > shifttimefrom, dateToday, dateTomorrow), @sh_brktimeFr);
SET breaktimeEnd = TIMESTAMP(IF(@sh_brktimeTo > shifttimefrom, dateToday, dateTomorrow), @sh_brktimeTo);

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

SET hasBreaktime = (@sh_brktimeFr IS NOT NULL) AND (@sh_brktimeTo IS NOT NULL);

/* Calculate the regular hours worked for the day. */
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
    /* If there is no breaktime, just compute the time spanning from the duty start and end. */
    SET regularHours = COMPUTE_TimeDifference(TIME(dutyStart), TIME(dutyEnd));
END IF;

/* Make sure the regular hours doesn't go above the standard 8-hour workday. */
SET regularHours = LEAST(regularHours, MAX_REGULAR_HOURS);

SET nightDiffRangeStart = TIMESTAMP(dateToday, nightDiffTimeFrom);
SET nightDiffRangeEnd = TIMESTAMP(IF(nightDiffTimeTo > nightDiffTimeFrom, ete_Date, dateTomorrow), nightDiffTimeTo);

SET dawnNightDiffRangeStart = TIMESTAMP(dateYesterday, nightDiffTimeFrom);
SET dawnNightDiffRangeEnd = TIMESTAMP(IF(nightDiffTimeTo > nightDiffTimeFrom, dateYesterday, dateToday), nightDiffTimeTo);

SET isEntitledToNightDifferential = (e_CalcNightDiff = 1);

SET shouldCalculateNightDifferential = (
    isNightShift AND
    isEntitledToNightDifferential
);

IF shouldCalculateNightDifferential THEN
    SET nightDiffHours = IFNULL(ComputeNightDiffHours(dutyStart, dutyEnd, nightDiffRangeStart, nightDiffRangeEnd), 0);
    SET nightDiffHours = nightDiffHours + IFNULL(ComputeNightDiffHours(dutyStart, dutyEnd, dawnNightDiffRangeStart, dawnNightDiffRangeEnd), 0);
END IF;

SET hasOvertime = (otstartingtime IS NOT NULL) AND (otendingtime IS NOT NULL);

IF hasOvertime THEN

    SET overtimeDate = DATE(shiftEnd);

    SET overtimeStart = TIMESTAMP(overtimeDate, otstartingtime);
    SET overtimeEnd = TIMESTAMP(IF(otendingtime > otstartingtime, overtimeDate, dateTomorrow), otendingtime);

    /*
     * Start by figuring out the overtimeDutyStart (the time considered the employee has started working overtime)
     * and the overtimeDutyEnd (the time considered the employee has worked overtime until).
     */
    IF (overtimeStart > shiftStart) THEN

        SET overtimeDutyStart = LEAST(
            GREATEST(overtimeStart, fullTimeIn, shiftEnd),
            fullTimeOut
        );
        SET overtimeDutyEnd = LEAST(overtimeEnd, fullTimeOut);

    ELSEIF (overtimeStart < shiftStart) THEN

        SET overtimeDutyStart = GREATEST(overtimeStart, fullTimeIn);
        SET overtimeDutyEnd = LEAST(overtimeEnd, fullTimeOut, shiftStart);

    END IF;

    SET overtimeHours = COMPUTE_TimeDifference(TIME(overtimeDutyStart), TIME(overtimeDutyEnd));

END IF;

SET isOvertimeOverlappedNightDifferential = (
    (overtimeDutyStart < nightDiffRangeEnd) AND
    (overtimeDutyEnd > nightDiffRangeStart)
);

SET shouldCalculateNightDifferentialOvertime = (
    isNightShift AND
    isEntitledToNightDifferential AND
    isOvertimeOverlappedNightDifferential AND
    hasOvertime
);

IF shouldCalculateNightDifferentialOvertime THEN
    SET nightDiffOTDutyStart = GREATEST(overtimeDutyStart, nightDiffRangeStart);
    SET nightDiffOTDutyEnd = LEAST(overtimeDutyEnd, nightDiffRangeEnd);

    SET nightDiffOTHours = COMPUTE_TimeDifference(TIME(nightDiffOTDutyStart), TIME(nightDiffOTDutyEnd));
END IF;

/* First check if the duty start is above shift start to check if the employee is late. */
IF dutyStart > shiftStart THEN

    IF hasBreaktime THEN

        IF shiftStart < breaktimeStart THEN
            /*
             * Make sure that the late period doesn't include part of the breaktime since that is not
             * part of the required work hours.
             */
            SET @latePeriodEndBeforeBreaktime = LEAST(dutyStart, breaktimeStart);

            SET lateHoursBeforeBreak = COMPUTE_TimeDifference(TIME(shiftStart), TIME(@latePeriodEndBeforeBreaktime));
        END IF;

        IF dutyStart > breaktimeEnd THEN
            SET lateHoursAfterBreak = COMPUTE_TimeDifference(TIME(breaktimeEnd), TIME(dutyStart));
        END IF;

        SET lateHours = lateHoursBeforeBreak + lateHoursAfterBreak;
    ELSE
        SET lateHours = COMPUTE_TimeDifference(TIME(shiftStart), TIME(dutyStart));
    END IF;

END IF;

/* First check if the duty ends before the shift ends to check if the employee committed undertime. */
IF dutyEnd < shiftEnd THEN

    IF hasBreaktime THEN

        IF dutyEnd < breaktimeStart THEN
            SET undertimeHoursBeforeBreak = COMPUTE_TimeDifference(TIME(dutyEnd), TIME(breaktimeStart));
        END IF;

        /*
         * Calculate the remaining undertime that happened after breaktime.
         */
        SET @undertimePeriodStartAfterBreaktime = GREATEST(dutyEnd, breaktimeEnd);

        SET undertimeHoursAfterBreak = COMPUTE_TimeDifference(TIME(@undertimePeriodStartAfterBreaktime), TIME(shiftEnd));

        SET undertimeHours = undertimeHoursBeforeBreak + undertimeHoursAfterBreak;
    ELSE
        SET undertimeHours = COMPUTE_TimeDifference(TIME(dutyEnd), TIME(shiftEnd));
    END IF;

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
    ete_Date BETWEEN elv.LeaveStartDate AND elv.LeaveEndDate
LIMIT 1
INTO
    hasLeave,
    leaveStartTime,
    leaveEndTime,
    leaveType;

IF hasLeave THEN
    SET leaveStart = TIMESTAMP(dateToday, leaveStartTime);
    SET leaveEnd = TIMESTAMP(IF(leaveEndTime > leaveStartTime, dateToday, dateTomorrow), leaveEndTime);

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
        SET leaveHours = COMPUTE_TimeDifference(TIME(leaveStart), TIME(leaveEnd));
    END IF;
END IF;

SET ete_RegHrsWorkd = regularHours;
SET ete_OvertimeHrs = overtimeHours;
SET ete_NDiffHrs = nightDiffHours;
SET ete_NDiffOTHrs = nightDiffOTHours;
SET ete_HrsLate = lateHours;
SET ete_HrsUnder = undertimeHours;

SET ndiffrate = ndiffrate MOD 1;
SET ndiffotrate = otrate * ndiffrate;

SELECT GET_employeerateperday(ete_EmpRowID, ete_OrganizID, ete_Date)
INTO dailyRate;

SELECT shift.DivisorToDailyRate
FROM shift
WHERE shift.RowID = shiftID
INTO divisorToDailyRate;

SET hourlyRate = dailyRate / divisorToDailyRate;

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

SET ete_RegHrsWorkd = IFNULL(ete_RegHrsWorkd, 0);
SET ete_OvertimeHrs = IFNULL(ete_OvertimeHrs, 0);
SET ete_NDiffHrs = IFNULL(ete_NDiffHrs, 0);
SET ete_NDiffOTHrs = IFNULL(ete_NDiffOTHrs, 0);
SET ete_HrsLate = IFNULL(ete_HrsLate, 0);
SEt ete_HrsUnder = IFNULL(ete_HrsUnder, 0);

IF IFNULL(OTCount,0) > 1 THEN

    SELECT COMPUTE_TimeDifference(OTStartTime, OTEndTime)
    FROM employeeovertime
    WHERE EmployeeID = ete_EmpRowID AND
        OrganizationID = ete_OrganizID AND
        ete_Date BETWEEN OTStartDate AND COALESCE(OTEndDate, OTStartDate) AND
        OTStatus = 'Approved' AND
        RowID != aftershiftOTRowID
    LIMIT 1
    INTO anotherOTHours;

    IF anotherOTHours IS NULL THEN
        SET anotherOTHours = 0.0;
    END IF;

    SET ete_OvertimeHrs = ete_OvertimeHrs + anotherOTHours;

ELSEIF IFNULL(OTCount,0) = 1 && ete_OvertimeHrs = 0 THEN

    SELECT COMPUTE_TimeDifference(OTStartTime, OTEndTime)
    FROM employeeovertime
    WHERE EmployeeID = ete_EmpRowID AND
        OrganizationID = ete_OrganizID AND
        OTStatus = 'Approved' AND
        ete_Date BETWEEN OTStartDate AND OTStartDate AND
        RowID != aftershiftOTRowID
    LIMIT 1
    INTO anotherOTHours;

    IF anotherOTHours IS NULL THEN
        SET anotherOTHours = 0.0;
    END IF;

    SET ete_OvertimeHrs = ete_OvertimeHrs + anotherOTHours;

END IF;

SET basicDayPay = ete_RegHrsWorkd * hourlyRate;

SET isSpecialNonWorkingHoliday = pr_PayType = 'Special Non-Working Holiday';
SET isRegularHoliday = pr_PayType = 'Regular Holiday';

SET isHoliday = isSpecialNonWorkingHoliday OR isRegularHoliday;
SET isRegularDay = NOT isHoliday;

SET isWorkingDay = NOT isRestDay;

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
        ete_RegHrsWorkd,
        ete_OvertimeHrs,
        ete_HrsUnder,
        ete_NDiffHrs,
        ete_NDiffOTHrs,
        ete_HrsLate,
        payrateRowID,
        ete_TotalDayPay,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        NULL,
        NULL,
        NULL,
        NULL,
        1,
        NULL,
        NULL,
        NULL
    )
    INTO timeEntryID;

ELSEIF isRegularDay THEN

    IF hasLeave AND isWorkingDay THEN
        SET leavePay = IFNULL(leaveHours * hourlyRate, 0);
    END IF;

    SET @leave_hrs = IFNULL(leaveHours, 0);

    IF (ete_HrsLate - @leave_hrs) > -1 THEN
        SET ete_HrsLate = (ete_HrsLate - @leave_hrs);
        SET lateHours = ete_HrsLate;
    END IF;

    IF (ete_HrsUnder - @leave_hrs) > -1 THEN
        SET ete_HrsUnder = (ete_HrsUnder - @leave_hrs);
        SET undertimeHours = ete_HrsUnder;
    END IF;

    IF isWorkingDay THEN
        SET regularAmount = (ete_RegHrsWorkd * hourlyRate) * commonrate;
        SET overtimeAmount = (ete_OvertimeHrs * hourlyRate) * otrate;

        SET lateAmount = ete_HrsLate * hourlyRate;
        SET undertimeAmount = ete_HrsUnder * hourlyRate;
    ELSEIF isRestDay THEN
        SET restDayAmount = (ete_RegHrsWorkd * hourlyRate) * restday_rate;
        SET overtimeAmount = (ete_OvertimeHrs * hourlyRate) * restdayot_rate;

        SET lateHours = 0.0;
        SEt undertimeHours = 0.0;
    END IF;

    SET nightDiffAmount = (ete_NDiffHrs * hourlyRate) * ndiffrate;
    SET nightDiffOTAmount = (ete_NDiffOTHrs * hourlyRate) * ndiffotrate;

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
        ete_RegHrsWorkd,
        ete_OvertimeHrs,
        undertimeHours,
        ete_NDiffHrs,
        ete_NDiffOTHrs,
        lateHours,
        payrateRowID,
        ete_TotalDayPay,
        ete_RegHrsWorkd + ete_OvertimeHrs,
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
        leavePay
    )
    INTO timeEntryID;

ELSEIF isHoliday THEN

    SET hasWorkedLastWorkingDay = HasWorkedLastWorkingDay(ete_EmpRowID, ete_Date);

    IF isRestDay THEN
        SET regularAmount = (ete_RegHrsWorkd * hourlyRate);
        SET overtimeAmount = (ete_OvertimeHrs * hourlyRate) * restdayot_rate;

        SET applicableHolidayRate = restday_rate;
    ELSE
        SET regularAmount = (ete_RegHrsWorkd * hourlyRate);
        SET overtimeAmount = (ete_OvertimeHrs * hourlyRate) * otrate;

        SET applicableHolidayRate = commonrate;
    END IF;

    SET ete_HrsLate = 0.0;
    SET ete_HrsUnder = 0.0;

    IF pr_PayType = 'Regular Holiday' THEN

        IF e_EmpType = 'Daily' THEN
            SET holidayPay = IF(hasWorkedLastWorkingDay, dailyRate, 0);
        ELSEIF e_EmpType = 'Monthly' THEN
            SET holidayPay = regularAmount * (applicableHolidayRate - 1);
        END IF;

    ELSEIF pr_PayType = 'Special Non-Working Holiday' THEN
        SET holidayPay = regularAmount * (applicableHolidayRate - 1);
    ELSE
        SET holidayPay = 0;
    END IF;

    SET ete_TotalDayPay = COALESCE(regularAmount, 0) + COALESCE(overtimeAmount, 0) + COALESCE(holidayPay, 0);

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
        (ete_HrsUnder * hourlyRate),
        (ete_NDiffHrs * hourlyRate) * ndiffrate,
        (ete_NDiffOTHrs * hourlyRate) * ndiffotrate,
        (ete_HrsLate * hourlyRate),
        NULL,
        NULL,
        holidayPay,
        basicDayPay,
        7,
        NULL,
        NULL,
        NULL
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
