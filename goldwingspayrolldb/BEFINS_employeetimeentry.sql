/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFINS_employeetimeentry`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFINS_employeetimeentry` BEFORE INSERT ON `employeetimeentry` FOR EACH ROW BEGIN

DECLARE isRest_day CHAR(1);

DECLARE hasShift BOOLEAN;



DECLARE absent_amount DECIMAL(11,6);

DECLARE isRegularDay CHAR(1);

DECLARE TaxableDailyAllowanceAmount DECIMAL(11,6);

DECLARE rate_this_date DECIMAL(11,6);
DECLARE hourly_rate DECIMAL(11,6);
DECLARE isSpecialHoliday CHAR(1);
DECLARE isPresentInWorkingDaysPriorToThisDate CHAR(1) DEFAULT '0';
DECLARE payrate_this_date DECIMAL(11,2);

DECLARE e_rateperday DECIMAL(12,6) DEFAULT 0;

DECLARE emp_type VARCHAR(50);

DECLARE default_workhours_everyday DECIMAL(11,6) DEFAULT 8;

DECLARE nightDiffTimeFrom TIME DEFAULT '22:00:00';
DECLARE nightDiffTimeTo TIME DEFAULT '06:00:00';

DECLARE nightDiffRangeFrom DATETIME;
DECLARE nightDiffRangeTo DATETIME;

DECLARE leaveHours DECIMAL(15, 4);

DECLARE shiftStart DATETIME;
DECLARE shiftEnd DATETIME;

DECLARE dateToday DATE;
DECLARE dateTomorrow DATE;

SET @e_rateperday = 0.0;

SELECT IF(
    e.EmployeeType = 'Daily',
    es.BasicPay,
    (es.Salary / (e.WorkDaysPerYear / 24))
)
FROM employeesalary es
INNER JOIN employee e
ON e.RowID = es.EmployeeID AND
    e.OrganizationID = es.OrganizationID
WHERE es.RowID = NEW.EmployeeSalaryID
INTO @e_rateperday;

SET e_rateperday = IFNULL(@e_rateperday,0);

















SELECT
    (PayType = 'Regular Day'),
    (LOCATE('Special',PayType) > 0)
FROM payrate
WHERE RowID = NEW.PayRateID
INTO
    isRegularDay,
    isSpecialHoliday;

SET @myperfectshifthrs = 0.0;
SELECT
    `PayRate`,
    GET_employeerateperday(NEW.EmployeeID, NEW.OrganizationID, NEW.`Date`)
FROM payrate pr
WHERE pr.RowID=NEW.PayRateID
INTO
    payrate_this_date,
    rate_this_date;


SET @myperfectshifthrs = 1;

SET NEW.VacationLeaveHours = IFNULL(NEW.VacationLeaveHours,0);
SET NEW.SickLeaveHours = IFNULL(NEW.SickLeaveHours,0);
SET NEW.MaternityLeaveHours = IFNULL(NEW.MaternityLeaveHours,0);
SET NEW.OtherLeaveHours = IFNULL(NEW.OtherLeaveHours,0);

SET @myperfectshifthrs = 0;

SELECT (e.DayOfRest = DAYOFWEEK(NEW.`Date`))
FROM employee e
WHERE e.RowID=NEW.EmployeeID
INTO isRest_day;


SET NEW.RegularHoursWorked = IFNULL(NEW.RegularHoursWorked,0);
SET NEW.RegularHoursAmount = IFNULL(NEW.RegularHoursAmount,0);
SET NEW.TotalHoursWorked = IFNULL(NEW.TotalHoursWorked,0);
SET NEW.OvertimeHoursWorked = IFNULL(NEW.OvertimeHoursWorked,0);
SET NEW.OvertimeHoursAmount = IFNULL(NEW.OvertimeHoursAmount,0);
SET NEW.UndertimeHours = IFNULL(NEW.UndertimeHours,0);
SET NEW.UndertimeHoursAmount = IFNULL(NEW.UndertimeHoursAmount,0);
SET NEW.NightDifferentialHours = IFNULL(NEW.NightDifferentialHours,0);
SET NEW.NightDiffHoursAmount = IFNULL(NEW.NightDiffHoursAmount,0);
SET NEW.NightDifferentialOTHours = IFNULL(NEW.NightDifferentialOTHours,0);
SET NEW.NightDiffOTHoursAmount = IFNULL(NEW.NightDiffOTHoursAmount,0);
SET NEW.HoursLate = IFNULL(NEW.HoursLate,0);
SET NEW.HoursLateAmount = IFNULL(NEW.HoursLateAmount,0);

IF isRest_day = '0' THEN

    SELECT EXISTS(
        SELECT RowID
        FROM employeeshift esh
        WHERE esh.EmployeeID = NEW.EmployeeID AND
            esh.OrganizationID = NEW.OrganizationID AND
            esh.RestDay = '0' AND
            NEW.`Date` BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
        LIMIT 1
    )
    INTO hasShift;

    SET @fullshifthrs = 0.00;

    IF hasShift AND isRegularDay = '1' THEN

        SELECT
            COMPUTE_TimeDifference(sh.TimeFrom, sh.TimeTo)
        FROM employeeshift esh
        INNER JOIN shift sh
        ON sh.RowID = esh.ShiftID
        WHERE esh.EmployeeID = NEW.EmployeeID AND
            esh.OrganizationID = NEW.OrganizationID AND
            esh.RestDay = '0' AND
            NEW.`Date` BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
        LIMIT 1
        INTO
            @fullshifthrs;

        SET absent_amount = GET_employeerateperday(NEW.EmployeeID, NEW.OrganizationID, NEW.`Date`);

        
            
        IF (SUBSTRING_INDEX(absent_amount,'.',1) * 1) = (SUBSTRING_INDEX(NEW.HoursLateAmount,'.',1) * 1) THEN
            SET NEW.Absent = 0;
        ELSEIF (SUBSTRING_INDEX(absent_amount,'.',1) * 1) = (SUBSTRING_INDEX(NEW.UndertimeHoursAmount,'.',1) * 1) THEN
            SET NEW.Absent = 0;
        ELSE
            IF NEW.TotalDayPay = 0 THEN
                SET NEW.Absent = absent_amount;
            ELSE
                SET NEW.Absent = 0;
            END IF;
        END IF;

    ELSE

        IF NOT isRegularDay THEN

            SET leaveHours = NEW.VacationLeaveHours + NEW.SickLeaveHours + NEW.MaternityLeaveHours + NEW.OtherLeaveHours;

            SET @calclegalholi = '0';
            SET @calcspecholi = '0';

            SET @daily_pay = 0.00;

            SELECT
                (pr.PayType = 'Regular Holiday' AND e.CalcHoliday = '1' AND e.StartDate <= NEW.`Date`),
                (pr.PayType = 'Special Non-Working Holiday' AND e.CalcSpecialHoliday = '1' AND e.StartDate <= NEW.`Date`),
                e.EmployeeType,
                GET_employeerateperday(NEW.EmployeeID, NEW.OrganizationID, NEW.`Date`) `Result`
            FROM payrate pr
            INNER JOIN employee e
            ON e.RowID=NEW.EmployeeID
            INNER JOIN (
                SELECT *
                FROM employeesalary
                WHERE EmployeeID=NEW.EmployeeID AND
                    OrganizationID=NEW.OrganizationID AND
                    NEW.`Date` BETWEEN EffectiveDateFrom AND IFNULL(EffectiveDateTo, NEW.`Date`)
                LIMIT 1
            ) es
            ON es.RowID > 0
            WHERE pr.RowID=NEW.PayRateID
            INTO
                @calclegalholi,
                @calcspecholi,
                emp_type,
                @daily_pay;

            IF leaveHours > 0 THEN
                SET NEW.Absent = 0.0;
            ELSEIF hasShift AND
                   leaveHours = 0 AND
                   NEW.TotalDayPay = 0 AND
                   @calclegalholi = 0 AND
                   @calcspecholi = 0 THEN

                SET NEW.Absent = @daily_pay;
            ELSEIF hasShift AND
                   leaveHours = 0 AND
                   NEW.TotalDayPay = 0 AND
                   @calclegalholi = 1 THEN

                
                SET NEW.Absent = 0.0;
            ELSEIF hasShift AND
                   leaveHours = 0 AND
                   NEW.TotalDayPay = 0 THEN

                IF @calcspecholi = 1 THEN

                    IF emp_type = 'Daily' THEN

                        SET NEW.TotalDayPay = 0.0;
                        SET NEW.Absent = 0.0;

                    ELSEIF emp_type != 'Daily' THEN

                        SET NEW.TotalDayPay = @daily_pay;
                        SET NEW.Absent = 0.0;

                    END IF;

                ELSE
                    SET NEW.TotalDayPay = 0.0;

                    SET NEW.Absent = @daily_pay;
                END IF;
            ELSE
                SET NEW.Absent = 0.0;
            END IF;
        ELSE
            SET NEW.Absent = IFNULL(NEW.Absent,0);
        END IF;
    END IF;
ELSE
    SET NEW.Absent = 0.0;
END IF;

SET NEW.Absent = IFNULL(NEW.Absent,0);

IF isRest_day = '1' && NEW.EmployeeShiftID IS NOT NULL AND COALESCE(NEW.RegularHoursWorked, 0) = 0 THEN
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
