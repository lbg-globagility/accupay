/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFUPD_employeetimeentry`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFUPD_employeetimeentry` BEFORE UPDATE ON `employeetimeentry` FOR EACH ROW BEGIN

DECLARE isRestDay BOOLEAN;
DECLARE hasShift BOOLEAN;

DECLARE dailyRate DECIMAL(11,6);

DECLARE TaxableDailyAllowanceAmount DECIMAL(11,6);

DECLARE rate_this_date DECIMAL(11,6);
DECLARE isPresentInWorkingDaysPriorToThisDate CHAR(1) DEFAULT '0';
DECLARE payrate_this_date DECIMAL(11,2);

DECLARE nightDiffTimeFrom TIME DEFAULT '22:00:00';
DECLARE nightDiffTimeTo TIME DEFAULT '06:00:00';

DECLARE nightDiffRangeFrom DATETIME;
DECLARE nightDiffRangeTo DATETIME;

DECLARE leaveHours DECIMAL(15, 4);

DECLARE shiftStart DATETIME;
DECLARE shiftEnd DATETIME;

DECLARE dateToday DATE;
DECLARE dateTomorrow DATE;

DECLARE isRegularHoliday BOOLEAN;
DECLARE isSpecialNonWorkingHoliday BOOLEAN;
DECLARE isHoliday BOOLEAN;

DECLARE isDefaultRestDay BOOLEAN;
DECLARE isShiftRestDay BOOLEAN;

DECLARE hasWorked BOOLEAN;
DECLARE hasLeave BOOLEAN;

SELECT
    `PayRate`,
    GET_employeerateperday(NEW.EmployeeID, NEW.OrganizationID, NEW.`Date`)
FROM payrate pr
WHERE pr.RowID=NEW.PayRateID
INTO
    payrate_this_date,
    rate_this_date;

SET NEW.VacationLeaveHours = IFNULL(NEW.VacationLeaveHours,0);
SET NEW.SickLeaveHours = IFNULL(NEW.SickLeaveHours,0);
SET NEW.MaternityLeaveHours = IFNULL(NEW.MaternityLeaveHours,0);
SET NEW.OtherLeaveHours = IFNULL(NEW.OtherLeaveHours,0);

SELECT COMPUTE_TimeDifference(sh.TimeFrom, sh.TimeTo)
FROM employeeshift esh
INNER JOIN shift sh
ON sh.RowID = esh.ShiftID
WHERE esh.EmployeeID = NEW.EmployeeID AND
    esh.OrganizationID = NEW.OrganizationID AND
    esh.RestDay = '0' AND
    NEW.`Date` BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
LIMIT 1
INTO @fullshifthrs;

SET NEW.RegularHoursWorked = IFNULL(NEW.RegularHoursWorked, 0);
SET NEW.RegularHoursAmount = IFNULL(NEW.RegularHoursAmount, 0);
SET NEW.TotalHoursWorked = IFNULL(NEW.TotalHoursWorked, 0);
SET NEW.OvertimeHoursWorked = IFNULL(NEW.OvertimeHoursWorked, 0);
SET NEW.OvertimeHoursAmount = IFNULL(NEW.OvertimeHoursAmount, 0);
SET NEW.UndertimeHours = IFNULL(NEW.UndertimeHours, 0);
SET NEW.UndertimeHoursAmount = IFNULL(NEW.UndertimeHoursAmount, 0);
SET NEW.NightDifferentialHours = IFNULL(NEW.NightDifferentialHours, 0);
SET NEW.NightDiffHoursAmount = IFNULL(NEW.NightDiffHoursAmount, 0);
SET NEW.NightDifferentialOTHours = IFNULL(NEW.NightDifferentialOTHours, 0);
SET NEW.NightDiffOTHoursAmount = IFNULL(NEW.NightDiffOTHoursAmount, 0);
SET NEW.HoursLate = IFNULL(NEW.HoursLate, 0);
SET NEW.HoursLateAmount = IFNULL(NEW.HoursLateAmount, 0);

SET hasWorked = NEW.RegularHoursWorked > 0;

SELECT GET_employeerateperday(NEW.EmployeeID, NEW.OrganizationID, NEW.`Date`)
INTO dailyRate;

SELECT
    (pr.PayType = 'Regular Holiday' AND e.CalcHoliday = '1' AND e.StartDate <= NEW.`Date`),
    (pr.PayType = 'Special Non-Working Holiday' AND e.CalcSpecialHoliday = '1' AND e.StartDate <= NEW.`Date`)
FROM payrate pr
INNER JOIN employee e
ON e.RowID = NEW.EmployeeID
INNER JOIN (
    SELECT RowID
    FROM employeesalary
    WHERE EmployeeID = NEW.EmployeeID AND
        OrganizationID = NEW.OrganizationID AND
        NEW.`Date` BETWEEN EffectiveDateFrom AND IFNULL(EffectiveDateTo, NEW.`Date`)
    LIMIT 1
) es
ON es.RowID > 0
WHERE pr.RowID=NEW.PayRateID
INTO
    isRegularHoliday,
    isSpecialNonWorkingHoliday;

SET isHoliday = isRegularHoliday OR isSpecialNonWorkingHoliday;

SET leaveHours = NEW.VacationLeaveHours + NEW.SickLeaveHours + NEW.MaternityLeaveHours + NEW.OtherLeaveHours;
SET hasLeave = leaveHours > 0;

SELECT (e.DayOfRest = DAYOFWEEK(NEW.`Date`))
FROM employee e
WHERE e.RowID = NEW.EmployeeID
INTO isDefaultRestDay;

SELECT
    esh.RowID IS NOT NULL,
    COALESCE(esh.RestDay, FALSE)
FROM employeeshift esh
WHERE esh.EmployeeID = NEW.EmployeeID AND
    esh.OrganizationID = NEW.OrganizationID AND
    NEW.`Date` BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
LIMIT 1
INTO
    hasShift,
    isShiftRestDay;

-- If there is no shift set for the day, assume that it's a rest day.
SET isShiftRestDay = IF(hasShift, isShiftRestDay, TRUE);

SET isRestDay = isShiftRestDay OR isDefaultRestDay;

IF hasWorked OR isRestDay OR isHoliday OR hasLeave THEN
    SET NEW.Absent = 0;
ELSE
    SET NEW.Absent = dailyRate;
END IF;

SET NEW.Absent = IFNULL(NEW.Absent, 0);

IF isDefaultRestDay = '1' && NEW.EmployeeShiftID IS NOT NULL AND COALESCE(NEW.RegularHoursWorked, 0) = 0 THEN
    SET NEW.EmployeeShiftID = NULL;
END IF;

SELECT GET_employeerateperday(NEW.EmployeeID, NEW.OrganizationID, NEW.`Date`)
INTO rate_this_date;


SELECT SUM(ea.AllowanceAmount)
FROM employeeallowance ea
WHERE ea.AllowanceFrequency='Daily'
    AND ea.TaxableFlag='1'
    AND ea.EmployeeID=NEW.EmployeeID
    AND ea.OrganizationID=NEW.OrganizationID
    AND NEW.`Date` BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate
INTO TaxableDailyAllowanceAmount;

SET rate_this_date = IFNULL(
    (
        SELECT sh.DivisorToDailyRate - COMPUTE_TimeDifference(sh.BreakTimeFrom,sh.BreakTimeTo)
        FROM employeeshift esh
        INNER JOIN shift sh
            ON sh.RowID=esh.ShiftID
        WHERE esh.RowID=NEW.EmployeeShiftID
    ),
    0
);

SET @daily_salary = GET_employeerateperday(NEW.EmployeeID, NEW.OrganizationID, NEW.`Date`);
SET @leave_hrs = (NEW.VacationLeaveHours + NEW.SickLeaveHours + NEW.MaternityLeaveHours + NEW.OtherLeaveHours);

SET NEW.TaxableDailyAllowance = (
    SELECT (
        IF(
            pr.PayType='Regular Day',
            IF(
                NEW.TotalDayPay > NEW.RegularHoursAmount AND @leave_hrs > 0,
                IF(
                    NEW.RegularHoursAmount=0,
                    NEW.TotalDayPay,
                    NEW.RegularHoursAmount
                ),
                IF(
                    NEW.RegularHoursAmount > @daily_salary,
                    @daily_salary,
                    NEW.RegularHoursAmount
                )
            ),
            IF(
                pr.PayType='Special Non-Working Holiday' AND e.CalcSpecialHoliday = '1',
                IF(
                    e.EmployeeType = 'Daily',
                    (NEW.RegularHoursAmount / pr.`PayRate`),
                    NEW.HolidayPayAmount
                ),
                IF(
                    pr.PayType='Special Non-Working Holiday' AND e.CalcSpecialHoliday = '0',
                    IF(
                        e.EmployeeType = 'Daily',
                        NEW.RegularHoursAmount,
                        NEW.HolidayPayAmount
                    ),
                    IF(
                        pr.PayType='Regular Holiday' AND e.CalcHoliday = '1',
                        NEW.HolidayPayAmount + ((NEW.VacationLeaveHours + NEW.SickLeaveHours + NEW.MaternityLeaveHours + NEW.OtherLeaveHours) * (@daily_salary / rate_this_date)),
                        0
                    )
                )
            )
        ) / @daily_salary
    ) * TaxableDailyAllowanceAmount
    FROM employee e
    INNER JOIN payrate pr
        ON pr.RowID=NEW.PayRateID
    WHERE e.RowID=NEW.EmployeeID
        AND e.OrganizationID=NEW.OrganizationID
);

IF NEW.TaxableDailyAllowance IS NULL THEN
    SET NEW.TaxableDailyAllowance = 0;
END IF;

IF NEW.TaxableDailyBonus IS NULL THEN
    SET NEW.TaxableDailyBonus = 0;
END IF;

IF NEW.NonTaxableDailyBonus IS NULL THEN
    SET NEW.NonTaxableDailyBonus = 0;
END IF;

IF NEW.LastUpd != '1900-01-01 00:00:01' THEN

    SET NEW.EmployeeSalaryID = (
        SELECT RowID
        FROM employeesalary
        WHERE EmployeeID=NEW.EmployeeID
            AND OrganizationID=NEW.OrganizationID
            AND NEW.`Date` BETWEEN EffectiveDateFrom AND IFNULL(EffectiveDateTo,NEW.`Date`)
        LIMIT 1
    );

END IF;

IF NEW.Leavepayment < 0 THEN
    SET NEW.Leavepayment = 0;
    SET NEW.TotalDayPay = 0;
    SET NEW.Absent = NEW.Leavepayment * -1;
ELSEIF NEW.Leavepayment > 0 THEN
    SET NEW.Absent = 0;
END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
