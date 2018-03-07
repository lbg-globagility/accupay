/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTINS_employeetimeentry`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_employeetimeentry` AFTER INSERT ON `employeetimeentry` FOR EACH ROW BEGIN

DECLARE auditRowID INT(11);

DECLARE viewID INT(11);

DECLARE AgencyRowID INT(11);

DECLARE EmpPositionRowID INT(11);

DECLARE DivisionRowID INT(11);

DECLARE _agencyFeeAmount DECIMAL(11, 6);

DECLARE anyint INT(11);

DECLARE agfRowID INT(11);

DECLARE divisorToHourlyRate DECIMAL(11, 6);

DECLARE actualrate DECIMAL(11,5);
DECLARE actualratepercent DECIMAL(11,5);

DECLARE _isAllowancePayableForOvertime BOOLEAN DEFAULT FALSE;
DECLARE _isAllowancePayableForNightDiff BOOLEAN DEFAULT FALSE;
DECLARE _isAllowancePayableForNightDiffOT BOOLEAN DEFAULT FALSE;
DECLARE _isAllowancePayableForRestDay BOOLEAN DEFAULT FALSE;
DECLARE _isAllowancePayableForHoliday BOOLEAN DEFAULT FALSE;

DECLARE hoursWorked DECIMAL(12, 6) DEFAULT 0.0;
DECLARE overtimeHoursAmount DECIMAL(15, 4) DEFAULT 0.0;
DECLARE nightDiffAmount DECIMAL(15, 4) DEFAULT 0.0;
DECLARE nightDiffOvertimeAmount DECIMAL(15, 4) DEFAULT 0.0;
DECLARE restDayAmount DECIMAL(15, 4) DEFAULT 0.0;
DECLARE _specialHolidayPay DECIMAL(15, 4) DEFAULT 0.0;
DECLARE _specialHolidayOTPay DECIMAL(15, 4) DEFAULT 0.0;
DECLARE _regularHolidayPay DECIMAL(15, 4) DEFAULT 0.0;
DECLARE _regularHolidayOTPay DECIMAL(15, 4) DEFAULT 0.0;
DECLARE _holidayPay DECIMAL(12, 6); /* Deprecated */

SELECT
    sh.DivisorToDailyRate
FROM employeeshift esh
INNER JOIN shift sh
ON sh.RowID = esh.ShiftID
WHERE esh.RowID = NEW.EmployeeShiftID
INTO
    divisorToHourlyRate;

SELECT
    e.AgencyID,
    e.PositionID,
    p.DivisionId,
    ag.`AgencyFee`
FROM employee e
LEFT JOIN position p
ON p.RowID = e.PositionID
LEFT JOIN agency ag
ON ag.RowID = e.AgencyID
WHERE e.RowID = NEW.EmployeeID
INTO
    AgencyRowID,
    EmpPositionRowID,
    DivisionRowID,
    _agencyFeeAmount;

SET hoursWorked = NEW.RegularHoursWorked + NEW.RestDayHours + NEW.SpecialHolidayHours + NEW.RegularHolidayHours;

IF  (AgencyRowID IS NOT NULL) AND
    (hoursWorked > 0) THEN

    SELECT agf.RowID
    FROM agencyfee agf
    WHERE agf.OrganizationID = NEW.OrganizationID AND
        agf.EmployeeID = NEW.EmployeeID AND
        agf.TimeEntryDate = NEW.`Date`
    ORDER BY DATEDIFF(DATE(DATE_FORMAT(agf.Created,'%Y-%m-%d')), NEW.`Date`)
    LIMIT 1
    INTO agfRowID;

    SELECT INSUPD_agencyfee(
        agfRowID,
        NEW.OrganizationID,
        NEW.CreatedBy,
        AgencyRowID,
        NEW.EmployeeID,
        EmpPositionRowID,
        DivisionRowID,
        NEW.RowID,
        NEW.`Date`,
        (_agencyFeeAmount / divisorToHourlyRate) * hoursWorked
    )
    INTO anyint;

ELSE

    UPDATE agencyfee af
    SET
        af.DailyFee = 0,
        af.LastUpd = CURRENT_TIMESTAMP(),
        af.LastUpdBy = af.CreatedBy
    WHERE af.OrganizationID = NEW.OrganizationID AND
        af.EmployeeID = NEW.EmployeeID AND
        af.TimeEntryDate = NEW.`Date`;

END IF;

SELECT
    (es.UndeclaredSalary / es.Salary) AS UndeclaredPercent,
    (es.UndeclaredSalary / es.Salary) AS ActualPercent
FROM employeesalary es
WHERE es.EmployeeID=NEW.EmployeeID
AND es.OrganizationID=NEW.OrganizationID
AND NEW.`Date` BETWEEN es.EffectiveDateFrom AND IFNULL(es.EffectiveDateTo,NEW.`Date`)
LIMIT 1
INTO
    actualrate,
    actualratepercent;

SET actualrate = IFNULL(actualrate, 0);

SELECT `GET_employeeundeclaredsalarypercent`(
    NEW.EmployeeID,
    NEW.OrganizationID,
    NEW.`Date`,
    NEW.`Date`
)
INTO actualratepercent;

/* TODO: Make this faster by selecting the payroll policy settings only once for all employees. */

/*
 * Calculate the allowance salary for overtime work.
 */
SET _isAllowancePayableForOvertime = GetListOfValueOrDefault(
    'Payroll Policy', 'PaySalaryAllowanceForOvertime', TRUE);

SET overtimeHoursAmount = NEW.OvertimeHoursAmount;
IF _isAllowancePayableForOvertime THEN
    SET overtimeHoursAmount = overtimeHoursAmount + (overtimeHoursAmount * actualrate);
END IF;

/*
 * Calculate the allowance salary for night differential work.
 */
SET _isAllowancePayableForNightDiff = GetListOfValueOrDefault(
    'Payroll Policy', 'PaySalaryAllowanceForNightDifferential', TRUE);

SET nightDiffAmount = NEW.NightDiffHoursAmount;
IF _isAllowancePayableForOvertime THEN
    SET nightDiffAmount = nightDiffAmount + (nightDiffAmount * actualrate);
END IF;

/*
 * Calculate the allowance salary for night differential overtime work.
 */
SET _isAllowancePayableForNightDiffOT = GetListOfValueOrDefault(
    'Payroll Policy', 'PaySalaryAllowanceForNightDifferentialOvertime', TRUE);

SET nightDiffOvertimeAmount = NEW.NightDiffOTHoursAmount;
IF _isAllowancePayableForOvertime THEN
    SET nightDiffOvertimeAmount = nightDiffOvertimeAmount + (nightDiffOvertimeAmount * actualrate);
END IF;

/*
 * Calculate the allowance salary for rest day work.
 */
SET _isAllowancePayableForRestDay = GetListOfValueOrDefault(
    'Payroll Policy', 'PaySalaryAllowanceForRestDay', TRUE);

SET restDayAmount = NEW.RestDayAmount;
IF _isAllowancePayableForRestDay THEN
    SET restDayAmount = restDayAmount + (restDayAmount * actualrate);
END IF;

/*
 * Calculate the allowance salary for holiday work (both for regular and special non-working holidays).
 */
SET _isAllowancePayableForHoliday = GetListOfValueOrDefault(
    'Payroll Policy', 'PaySalaryAllowanceForHolidayPay', TRUE);

SET _specialHolidayPay = NEW.SpecialHolidayPay;
SET _specialHolidayOTPay = NEW.SpecialHolidayOTPay;
SET _regularHolidayPay = NEW.RegularHolidayPay;
SET _regularHolidayOTPay = NEW.RegularHolidayOTPay;
SET _holidayPay = NEW.HolidayPayAmount; /* Deprecated */

IF _isAllowancePayableForHoliday THEN
    SET _holidayPay = _holidayPay + (_holidayPay * actualrate); /* Deprecated */

    SET _specialHolidayPay = _specialHolidayPay + (_specialHolidayPay * actualrate);
    SET _regularHolidayPay = _regularHolidayPay + (_regularHolidayPay * actualrate);

    SET _specialHolidayOTPay = _specialHolidayOTPay + (_specialHolidayOTPay * actualrate);
    SET _regularHolidayOTPay = _regularHolidayOTPay + (_regularHolidayOTPay * actualrate);
END IF;

INSERT INTO employeetimeentryactual (
    RowID,
    OrganizationID,
    `Date`,
    EmployeeShiftID,
    EmployeeID,
    EmployeeSalaryID,
    EmployeeFixedSalaryFlag,
    RegularHoursWorked,
    RegularHoursAmount,
    TotalHoursWorked,
    OvertimeHoursWorked,
    OvertimeHoursAmount,
    UndertimeHours,
    UndertimeHoursAmount,
    NightDifferentialHours,
    NightDiffHoursAmount,
    NightDifferentialOTHours,
    NightDiffOTHoursAmount,
    HoursLate,
    HoursLateAmount,
    LateFlag,
    PayRateID,
    VacationLeaveHours,
    SickLeaveHours,
    MaternityLeaveHours,
    OtherLeaveHours,
    TotalDayPay,
    Absent,
    ChargeToDivisionID,
    Leavepayment,
    SpecialHolidayPay,
    SpecialHolidayOTPay,
    RegularHolidayPay,
    RegularHolidayOTPay,
    HolidayPayAmount,
    BasicDayPay,
    RestDayHours,
    RestDayAmount,
    RestDayOTHours,
    RestDayOTPay
)
VALUES (
    NEW.RowID,
    NEW.OrganizationID,
    NEW.`Date`,
    NEW.EmployeeShiftID,
    NEW.EmployeeID,
    NEW.EmployeeSalaryID,
    NEW.EmployeeFixedSalaryFlag,
    NEW.RegularHoursWorked,
    NEW.RegularHoursAmount + (NEW.RegularHoursAmount * actualrate),
    NEW.TotalHoursWorked,
    NEW.OvertimeHoursWorked,
    overtimeHoursAmount,
    NEW.UndertimeHours,
    NEW.UndertimeHoursAmount + (NEW.UndertimeHoursAmount * actualrate),
    NEW.NightDifferentialHours,
    NEW.NightDiffHoursAmount + (NEW.NightDiffHoursAmount * actualrate),
    NEW.NightDifferentialOTHours,
    NEW.NightDiffOTHoursAmount + (NEW.NightDiffOTHoursAmount * actualrate),
    NEW.HoursLate,
    NEW.HoursLateAmount + (NEW.HoursLateAmount * actualrate),
    NEW.LateFlag,
    NEW.PayRateID,
    NEW.VacationLeaveHours,
    NEW.SickLeaveHours,
    NEW.MaternityLeaveHours,
    NEW.OtherLeaveHours,
    NEW.TotalDayPay * actualratepercent,
    NEW.Absent * actualratepercent,
    NEW.ChargeToDivisionID,
    NEW.Leavepayment + (NEW.Leavepayment * actualrate),
    _specialHolidayPay,
    _specialHolidayOTPay,
    _regularHolidayPay,
    _regularHolidayOTPay,
    _holidayPay,
    NEW.BasicDayPay + (NEW.BasicDayPay * actualrate),
    NEW.RestDayHours,
    NEW.RestDayAmount + (NEW.RestDayAmount * actualrate),
    NEW.RestDayOTHours,
    NEW.RestDayOTPay + (NEW.RestDayOTPay * actualrate)
)
ON DUPLICATE KEY
UPDATE
    OrganizationID = NEW.OrganizationID,
    `Date` = NEW.`Date`,
    EmployeeShiftID = NEW.EmployeeShiftID,
    EmployeeID = NEW.EmployeeID,
    EmployeeSalaryID = NEW.EmployeeSalaryID,
    EmployeeFixedSalaryFlag = NEW.EmployeeFixedSalaryFlag,
    RegularHoursWorked = NEW.RegularHoursWorked,
    RegularHoursAmount = NEW.RegularHoursAmount + (NEW.RegularHoursAmount * actualrate),
    TotalHoursWorked = NEW.TotalHoursWorked,
    OvertimeHoursWorked = NEW.OvertimeHoursWorked,
    OvertimeHoursAmount = overtimeHoursAmount,
    UndertimeHours = NEW.UndertimeHours,
    UndertimeHoursAmount = NEW.UndertimeHoursAmount + (NEW.UndertimeHoursAmount * actualrate),
    NightDifferentialHours = NEW.NightDifferentialHours,
    NightDiffHoursAmount = nightDiffAmount,
    NightDifferentialOTHours = NEW.NightDifferentialOTHours,
    NightDiffOTHoursAmount = nightDiffOvertimeAmount,
    HoursLate = NEW.HoursLate,
    HoursLateAmount = NEW.HoursLateAmount + (NEW.HoursLateAmount * actualrate),
    LateFlag = NEW.LateFlag,
    PayRateID = NEW.PayRateID,
    VacationLeaveHours = NEW.VacationLeaveHours,
    SickLeaveHours = NEW.SickLeaveHours,
    MaternityLeaveHours = NEW.MaternityLeaveHours,
    OtherLeaveHours = NEW.OtherLeaveHours,
    TotalDayPay = NEW.TotalDayPay * actualratepercent,
    Absent = NEW.Absent * actualratepercent,
    ChargeToDivisionID = NEW.ChargeToDivisionID,
    Leavepayment = NEW.Leavepayment + (NEW.Leavepayment * actualrate),
    SpecialHolidayPay = _specialHolidayPay,
    SpecialHolidayOTPay = _specialHolidayOTPay,
    RegularHolidayPay = _regularHolidayPay,
    RegularHolidayOTPay = _regularHolidayOTPay,
    HolidayPayAmount = _holidayPay,
    BasicDayPay = NEW.BasicDayPay + (NEW.BasicDayPay * actualrate),
    RestDayHours = NEW.RestDayHours,
    RestDayAmount = restDayAmount,
    RestDayOTHours = NEW.RestDayOTHours,
    RestDayOTPay = NEW.RestDayOTPay + (NEW.RestDayOTPay * actualrate);

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
