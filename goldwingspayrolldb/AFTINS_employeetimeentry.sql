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

DECLARE ag_fee DECIMAL(11,6);

DECLARE isShiftRestDay TINYINT(1);
DECLARE dayOfRest INT(11);
DECLARE actualRegularHoursAmount DECIMAL(11, 6);
DECLARE isRestDay TINYINT(1);
DECLARE undeclaredSalary DECIMAL(20, 6);

DECLARE payType VARCHAR(50);

DECLARE isHoliday TINYINT(1);

DECLARE anyint INT(11);

DECLARE agfRowID INT(11);

DECLARE divisorToHourlyRate DECIMAL(11, 6);

DECLARE actualrate DECIMAL(11,5);

DECLARE actualratepercent DECIMAL(11,5);

DECLARE emprateperday DECIMAL(11,6);

DECLARE breaktimeFrom TIME;
DECLARE breaktimeTo TIME;

SELECT
    sh.BreakTimeFrom,
    sh.BreakTimeTo,
    sh.DivisorToDailyRate
FROM employeeshift esh
INNER JOIN shift sh
ON sh.RowID = esh.ShiftID
WHERE esh.RowID = NEW.EmployeeShiftID
INTO
    breaktimeFrom,
    breaktimeTo,
    divisorToHourlyRate;

SELECT
    COALESCE(employeeshift.RestDay, FALSE),
    employee.DayOfRest
FROM employeeshift
INNER JOIN employee
ON employee.RowID = employeeshift.EmployeeID
WHERE employeeshift.RowID = NEW.EmployeeShiftID AND
    NEW.Date BETWEEN employeeshift.EffectiveFrom AND employeeshift.EffectiveTo
LIMIT 1
INTO
    isShiftRestDay,
    dayOfRest;

SET isRestDay = (DAYOFWEEK(NEW.Date) = dayOfRest) OR isShiftRestDay;
SET isRestDay = isShiftRestDay;

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
    ag_fee;

SELECT payrate.PayType
FROM payrate
WHERE payrate.RowID = NEW.PayRateID
INTO payType;

SET isHoliday = (payType = 'Regular Holiday') OR (payType = 'Special Non-Working Holiday');

IF  (AgencyRowID IS NOT NULL) AND
    (NEW.RegularHoursWorked > 0) THEN

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
        (ag_fee / divisorToHourlyRate) * NEW.RegularHoursWorked
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
    (es.UndeclaredSalary / es.Salary) AS ActualPercent,
    es.UndeclaredSalary
FROM employeesalary es
WHERE es.EmployeeID=NEW.EmployeeID
AND es.OrganizationID=NEW.OrganizationID
AND NEW.`Date` BETWEEN es.EffectiveDateFrom AND IFNULL(es.EffectiveDateTo,NEW.`Date`)
LIMIT 1
INTO
    actualrate,
    actualratepercent,
    undeclaredSalary;

SET actualrate = IFNULL(actualrate, 0);

SELECT GET_employeerateperday(NEW.EmployeeID, NEW.OrganizationID, NEW.`Date`)
INTO emprateperday;

SELECT `GET_employeeundeclaredsalarypercent`(
    NEW.EmployeeID,
    NEW.OrganizationID,
    NEW.`Date`,
    NEW.`Date`
)
INTO actualratepercent;


IF isRestDay THEN
    SET actualRegularHoursAmount = NEW.RegularHoursAmount + (undeclaredSalary / divisorToHourlyRate * NEW.RegularHoursWorked);
ELSE
    SET actualRegularHoursAmount = NEW.RegularHoursAmount;
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
    HolidayPayAmount,
    BasicDayPay,
    RestDayHours,
    RestDayAmount
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
    NEW.OvertimeHoursAmount + (NEW.OvertimeHoursAmount * actualrate),
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
    NEW.HolidayPayAmount + (NEW.HolidayPayAmount * actualrate),
    NEW.BasicDayPay + (NEW.BasicDayPay * actualrate),
    NEW.RestDayHours,
    New.RestDayAmount + (NEW.RestDayAmount * actualrate)
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
    OvertimeHoursAmount = NEW.OvertimeHoursAmount + (NEW.OvertimeHoursAmount * actualrate),
    UndertimeHours = NEW.UndertimeHours,
    UndertimeHoursAmount = NEW.UndertimeHoursAmount + (NEW.UndertimeHoursAmount * actualrate),
    NightDifferentialHours = NEW.NightDifferentialHours,
    NightDiffHoursAmount = NEW.NightDiffHoursAmount + (NEW.NightDiffHoursAmount * actualrate),
    NightDifferentialOTHours = NEW.NightDifferentialOTHours,
    NightDiffOTHoursAmount = NEW.NightDiffOTHoursAmount + (NEW.NightDiffOTHoursAmount * actualrate),
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
    HolidayPayAmount = NEW.HolidayPayAmount + (NEW.HolidayPayAmount * actualrate),
    BasicDayPay = NEW.BasicDayPay + (NEW.BasicDayPay * actualrate),
    RestDayHours = NEW.RestDayHours,
    RestDayAmount = NEW.RestDayAmount + (NEW.RestDayAmount * actualrate);

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
