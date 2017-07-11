/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTUPD_employeetimeentry`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_employeetimeentry` AFTER UPDATE ON `employeetimeentry` FOR EACH ROW BEGIN


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
DECLARE undeclaredSalary DECIMAL(11, 6);

DECLARE payType VARCHAR(50);

DECLARE isHoliday TINYINT(1);

DECLARE anyint INT(11);

DECLARE agfRowID INT(11);

DECLARE perfecthoursworked DECIMAL(11,6);

DECLARE actualrate DECIMAL(11,5);

DECLARE actualratepercent DECIMAL(11,5);

DECLARE emprateperday DECIMAL(11,6);

DECLARE breaktimeFrom TIME;
DECLARE breaktimeTo TIME;

SELECT
    COMPUTE_TimeDifference(sh.TimeFrom, sh.TimeTo),
    sh.BreakTimeFrom,
    sh.BreakTimeTo
FROM employeeshift esh
INNER JOIN shift sh
    ON sh.RowID=esh.ShiftID
WHERE esh.RowID=NEW.EmployeeShiftID
INTO
    perfecthoursworked,
    breaktimeFrom,
    breaktimeTo;

SELECT
    COALESCE(employeeshift.RestDay, FALSE),
    employee.DayOfRest
FROM employeeshift
INNER JOIN employee
    ON employee.RowID = employeeshift.EmployeeID
WHERE employeeshift.RowID = NEW.EmployeeShiftID
    AND NEW.Date BETWEEN employeeshift.EffectiveFrom AND employeeshift.EffectiveTo
LIMIT 1
INTO
    isShiftRestDay,
    dayOfRest;

IF breaktimeFrom IS NOT NULL AND breaktimeTo IS NOT NULL THEN
    SET perfecthoursworked = perfecthoursworked - COMPUTE_TimeDifference(breaktimeFrom, breaktimeTo);
END IF;

SET isRestDay = (DAYOFWEEK(NEW.Date) = dayOfRest) OR isShiftRestDay;
SET isRestDay = isShiftRestDay;

IF perfecthoursworked IS NULL THEN
    SET perfecthoursworked = 0;
END IF;

-- IF perfecthoursworked NOT IN (4,5) THEN
--    SET perfecthoursworked = perfecthoursworked - 1;
-- END IF;

SELECT
    e.AgencyID,
    e.PositionID,
    p.DivisionId,
    ag.`AgencyFee`
FROM employee e
LEFT JOIN position p
    ON p.RowID=e.PositionID
LEFT JOIN agency ag
    ON ag.RowID=e.AgencyID
WHERE e.RowID=NEW.EmployeeID
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

-- SELECT
--     isRestDay,
--     isHoliday,
--     (
--         (AgencyRowID IS NOT NULL) AND
--         (perfecthoursworked > 0) AND
--         (NOT isRestDay) AND
--         (NOT isHoliday)
--     ),
--     isShiftRestDay
-- INTO OUTFILE 'D:/logs/aaron.txt'
-- FIELDS TERMINATED BY ', ';

IF  (AgencyRowID IS NOT NULL) AND
    (perfecthoursworked > 0) THEN

    SELECT agf.RowID
    FROM agencyfee agf
    WHERE agf.OrganizationID=NEW.OrganizationID
        AND agf.EmployeeID=NEW.EmployeeID
        AND agf.TimeEntryDate=NEW.`Date`
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
        (ag_fee / perfecthoursworked) * NEW.RegularHoursWorked
    )
    INTO anyint;

ELSE

    UPDATE agencyfee af
    SET
        af.DailyFee=0,
        af.LastUpd=CURRENT_TIMESTAMP(),
        af.LastUpdBy=af.CreatedBy
    WHERE af.OrganizationID=NEW.OrganizationID
        AND af.EmployeeID=NEW.EmployeeID
        AND af.TimeEntryDate=NEW.`Date`;

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

-- Deprecate
IF isRestDay THEN
    SET actualRegularHoursAmount = NEW.RegularHoursAmount + (undeclaredSalary / 8 * NEW.RegularHoursWorked);
ELSE
    SET actualRegularHoursAmount = NEW.RegularHoursAmount;
END IF;

INSERT INTO employeetimeentryactual
(
    RowID
    ,OrganizationID
    ,`Date`
    ,EmployeeShiftID
    ,EmployeeID
    ,EmployeeSalaryID
    ,EmployeeFixedSalaryFlag
    ,RegularHoursWorked
    ,RegularHoursAmount
    ,TotalHoursWorked
    ,OvertimeHoursWorked
    ,OvertimeHoursAmount
    ,UndertimeHours
    ,UndertimeHoursAmount
    ,NightDifferentialHours
    ,NightDiffHoursAmount
    ,NightDifferentialOTHours
    ,NightDiffOTHoursAmount
    ,HoursLate
    ,HoursLateAmount
    ,LateFlag
    ,PayRateID
    ,VacationLeaveHours
    ,SickLeaveHours
    ,MaternityLeaveHours
    ,OtherLeaveHours
    ,TotalDayPay
    ,Absent
    ,ChargeToDivisionID
    ,Leavepayment
    ,HolidayPayAmount
) VALUES(
    NEW.RowID
    ,NEW.OrganizationID
    ,NEW.`Date`
    ,NEW.EmployeeShiftID
    ,NEW.EmployeeID
    ,NEW.EmployeeSalaryID
    ,NEW.EmployeeFixedSalaryFlag
    ,NEW.RegularHoursWorked
    ,NEW.RegularHoursAmount + (NEW.RegularHoursAmount * actualrate)
    ,NEW.TotalHoursWorked
    ,NEW.OvertimeHoursWorked
    ,NEW.OvertimeHoursAmount + (NEW.OvertimeHoursAmount * actualrate)
    ,NEW.UndertimeHours
    ,NEW.UndertimeHoursAmount + (NEW.UndertimeHoursAmount * actualrate)
    ,NEW.NightDifferentialHours
    ,NEW.NightDiffHoursAmount + (NEW.NightDiffHoursAmount * actualrate)
    ,NEW.NightDifferentialOTHours
    ,NEW.NightDiffOTHoursAmount + (NEW.NightDiffOTHoursAmount * actualrate)
    ,NEW.HoursLate
    ,NEW.HoursLateAmount + (NEW.HoursLateAmount * actualrate)
    ,NEW.LateFlag
    ,NEW.PayRateID
    ,NEW.VacationLeaveHours
    ,NEW.SickLeaveHours
    ,NEW.MaternityLeaveHours
    ,NEW.OtherLeaveHours
    ,NEW.TotalDayPay * actualratepercent
    ,NEW.Absent * actualratepercent
    ,NEW.ChargeToDivisionID
    ,NEW.Leavepayment + (NEW.Leavepayment * actualrate)
    ,NEW.HolidayPayAmount + (NEW.HolidayPayAmount * actualrate)
) ON
DUPLICATE
KEY
UPDATE
    OrganizationID=NEW.OrganizationID
    ,`Date`=NEW.`Date`
    ,EmployeeShiftID=NEW.EmployeeShiftID
    ,EmployeeID=NEW.EmployeeID
    ,EmployeeSalaryID=NEW.EmployeeSalaryID
    ,EmployeeFixedSalaryFlag=NEW.EmployeeFixedSalaryFlag
    ,RegularHoursWorked=NEW.RegularHoursWorked
    ,RegularHoursAmount=NEW.RegularHoursAmount + (NEW.RegularHoursAmount * actualrate)
    ,TotalHoursWorked=NEW.TotalHoursWorked
    ,OvertimeHoursWorked=NEW.OvertimeHoursWorked
    ,OvertimeHoursAmount=NEW.OvertimeHoursAmount + (NEW.OvertimeHoursAmount * actualrate)
    ,UndertimeHours=NEW.UndertimeHours
    ,UndertimeHoursAmount=NEW.UndertimeHoursAmount + (NEW.UndertimeHoursAmount * actualrate)
    ,NightDifferentialHours=NEW.NightDifferentialHours
    ,NightDiffHoursAmount=NEW.NightDiffHoursAmount + (NEW.NightDiffHoursAmount * actualrate)
    ,NightDifferentialOTHours=NEW.NightDifferentialOTHours
    ,NightDiffOTHoursAmount=NEW.NightDiffOTHoursAmount + (NEW.NightDiffOTHoursAmount * actualrate)
    ,HoursLate=NEW.HoursLate
    ,HoursLateAmount=NEW.HoursLateAmount + (NEW.HoursLateAmount * actualrate)
    ,LateFlag=NEW.LateFlag
    ,PayRateID=NEW.PayRateID
    ,VacationLeaveHours=NEW.VacationLeaveHours
    ,SickLeaveHours=NEW.SickLeaveHours
    ,MaternityLeaveHours=NEW.MaternityLeaveHours
    ,OtherLeaveHours=NEW.OtherLeaveHours
    ,TotalDayPay=NEW.TotalDayPay * actualratepercent
    ,Absent=NEW.Absent * actualratepercent
    ,ChargeToDivisionID=NEW.ChargeToDivisionID
    ,Leavepayment=NEW.Leavepayment + (NEW.Leavepayment * actualrate)
    ,HolidayPayAmount=NEW.HolidayPayAmount + (NEW.HolidayPayAmount * actualrate);

SELECT RowID FROM `view` WHERE ViewName='Employee Time Entry' AND OrganizationID=NEW.OrganizationID LIMIT 1 INTO viewID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EmployeeShiftID',NEW.RowID,OLD.EmployeeShiftID,NEW.EmployeeShiftID,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EmployeeSalaryID',NEW.RowID,OLD.EmployeeSalaryID,NEW.EmployeeSalaryID,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EmployeeFixedSalaryFlag',NEW.RowID,OLD.EmployeeFixedSalaryFlag,NEW.EmployeeFixedSalaryFlag,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'RegularHoursWorked',NEW.RowID,OLD.RegularHoursWorked,NEW.RegularHoursWorked,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'RegularHoursAmount',NEW.RowID,OLD.RegularHoursAmount,NEW.RegularHoursAmount,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TotalHoursWorked',NEW.RowID,OLD.TotalHoursWorked,NEW.TotalHoursWorked,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'OvertimeHoursWorked',NEW.RowID,OLD.OvertimeHoursWorked,NEW.OvertimeHoursWorked,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'OvertimeHoursAmount',NEW.RowID,OLD.OvertimeHoursAmount,NEW.OvertimeHoursAmount,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'UndertimeHours',NEW.RowID,OLD.UndertimeHours,NEW.UndertimeHours,'Insert') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'UndertimeHoursAmount',NEW.RowID,OLD.UndertimeHoursAmount,NEW.UndertimeHoursAmount,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'NightDifferentialHours',NEW.RowID,OLD.NightDifferentialHours,NEW.NightDifferentialHours,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'NightDiffHoursAmount',NEW.RowID,OLD.NightDiffHoursAmount,NEW.NightDiffHoursAmount,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'NightDifferentialOTHours',NEW.RowID,OLD.NightDifferentialOTHours,NEW.NightDifferentialOTHours,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'NightDiffOTHoursAmount',NEW.RowID,OLD.NightDiffOTHoursAmount,NEW.NightDiffOTHoursAmount,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'HoursLate',NEW.RowID,OLD.HoursLate,NEW.HoursLate,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'HoursLateAmount',NEW.RowID,OLD.HoursLateAmount,NEW.HoursLateAmount,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'LateFlag',NEW.RowID,OLD.LateFlag,NEW.LateFlag,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'PayRateID',NEW.RowID,OLD.PayRateID,NEW.PayRateID,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'VacationLeaveHours',NEW.RowID,OLD.VacationLeaveHours,NEW.VacationLeaveHours,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'SickLeaveHours',NEW.RowID,OLD.SickLeaveHours,NEW.SickLeaveHours,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'MaternityLeaveHours',NEW.RowID,OLD.MaternityLeaveHours,NEW.MaternityLeaveHours,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'OtherLeaveHours',NEW.RowID,OLD.OtherLeaveHours,NEW.OtherLeaveHours,'Update') INTO auditRowID;

SELECT INS_audittrail_RETRowID(NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TotalDayPay',NEW.RowID,OLD.TotalDayPay,NEW.TotalDayPay,'Update') INTO auditRowID;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
