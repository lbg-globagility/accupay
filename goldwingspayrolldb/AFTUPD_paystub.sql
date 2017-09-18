/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTUPD_paystub`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_paystub` AFTER UPDATE ON `paystub` FOR EACH ROW BEGIN

DECLARE auditRowID INT(11);

DECLARE viewRowID INT(11);


DECLARE viewID INT(11);

DECLARE IsrbxpayrollFirstHalfOfMonth TEXT;

DECLARE anyint INT(11);

DECLARE product_rowid INT(11);

DECLARE anyamount DECIMAL(11,6);

DECLARE allowancetype_IDs VARCHAR(150);

DECLARE loantype_IDs VARCHAR(150);

DECLARE payperiod_type VARCHAR(50);


DECLARE anyvchar VARCHAR(150);

DECLARE psi_RowID INT(11);


DECLARE e_startdate DATE;

DECLARE e_type VARCHAR(50);

DECLARE IsFirstTimeSalary BOOLEAN;

DECLARE totalWorkAmount DECIMAL(11,6);

DECLARE empsalRowID INT(11);

DECLARE actualrate DECIMAL(11,6);

DECLARE actualgross DECIMAL(11,6);

DECLARE pftype VARCHAR(50);

DECLARE totalallowanceamount VARCHAR(50);

DECLARE MonthCount DECIMAL(11,2) DEFAULT 12.0;

DECLARE regularPay DECIMAL(15, 4);
DECLARE overtimePay DECIMAL(15, 4);
DECLARE nightDiffPay DECIMAL(15, 4);
DECLARE nightDiffOvertimePay DECIMAL(15, 4);
DECLARE restDayPay DECIMAL(15, 4);
DECLARE leavePay DECIMAL(15, 4);
DECLARE holidayPay DECIMAL(15, 4);
DECLARE lateDeduction DECIMAL(15, 4);
DECLARE undertimeDeduction DECIMAL(15, 4);
DECLARE absenceDeduction DECIMAL(15, 4);

SELECT pr.`Half`
FROM payperiod pr
WHERE pr.RowID=NEW.PayPeriodID
INTO IsrbxpayrollFirstHalfOfMonth;

IF IsrbxpayrollFirstHalfOfMonth = '1' THEN
    SET payperiod_type = 'First half';
ELSE
    SET payperiod_type = 'End of the month';
END IF;


























































































































































































































































































































































































































































































































































































































































































































SELECT GET_employeeundeclaredsalarypercent(
    NEW.EmployeeID,
    NEW.OrganizationID,
    NEW.PayFromDate,
    NEW.PayToDate
)
INTO actualrate;

SELECT
    e.StartDate,
    e.EmployeeType,
    pf.PayFrequencyType
FROM employee e
INNER JOIN payfrequency pf
    ON pf.RowID = e.PayFrequencyID
WHERE e.RowID = NEW.EmployeeID
    AND e.OrganizationID = NEW.OrganizationID
INTO
    e_startdate,
    e_type,
    pftype;

SELECT (e_startdate BETWEEN NEW.PayFromDate AND NEW.PayToDate)
INTO IsFirstTimeSalary;

IF e_type IN ('Fixed', 'Monthly') AND IsFirstTimeSalary THEN

    IF e_type = 'Monthly' THEN

        SELECT
            SUM(TotalDayPay),
            EmployeeSalaryID
        FROM employeetimeentryactual
        WHERE OrganizationID = NEW.OrganizationID
            AND EmployeeID = NEW.EmployeeID
            AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate
        INTO
            totalWorkAmount,
            empsalRowID;

        IF totalWorkAmount IS NULL THEN

            SELECT
                SUM(TotalDayPay),
                EmployeeSalaryID
            FROM employeetimeentry
            WHERE OrganizationID = NEW.OrganizationID
                AND EmployeeID = NEW.EmployeeID
                AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate
            INTO
                totalWorkAmount,
                empsalRowID;

            SET totalWorkAmount = IFNULL(totalWorkAmount,0);

            SELECT totalWorkAmount + (totalWorkAmount * actualrate)
            INTO totalWorkAmount;

        END IF;

        SET totalWorkAmount = IFNULL(totalWorkAmount, 0);

    ELSEIF e_type = 'Fixed' THEN

        SELECT es.BasicPay
        FROM employeesalary es
        WHERE es.EmployeeID = NEW.EmployeeID
            AND es.OrganizationID = NEW.OrganizationID
            AND (es.EffectiveDateFrom >= NEW.PayFromDate OR IFNULL(es.EffectiveDateTo,NEW.PayToDate) >= NEW.PayFromDate)
            AND (es.EffectiveDateFrom <= NEW.PayToDate OR IFNULL(es.EffectiveDateTo,NEW.PayToDate) <= NEW.PayToDate)
        ORDER BY es.EffectiveDateFrom DESC
        LIMIT 1
        INTO totalWorkAmount;

        SET totalWorkAmount = IFNULL(totalWorkAmount, 0) * (IF(actualrate < 1, (actualrate + 1), actualrate));

    END IF;

ELSEIF e_type IN ('Fixed', 'Monthly') AND NOT IsFirstTimeSalary THEN

    SELECT
        SUM(RegularHoursAmount),
        SUM(OvertimeHoursAmount),
        SUM(NightDiffHoursAmount),
        SUM(NightDiffOTHoursAmount),
        SUM(RestDayAmount),
        SUM(HolidayPayAmount),
        SUM(HoursLateAmount),
        SUM(UndertimeHoursAmount),
        SUM(Absent),
        SUM(TotalDayPay),
        EmployeeSalaryID
    FROM employeetimeentryactual
    WHERE OrganizationID = NEW.OrganizationID AND
        EmployeeID = NEW.EmployeeID AND
        `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate
    INTO
        regularPay,
        overtimePay,
        nightDiffPay,
        nightDiffOvertimePay,
        restDayPay,
        holidayPay,
        lateDeduction,
        undertimeDeduction,
        absenceDeduction,
        totalWorkAmount,
        empsalRowID;

    IF e_type = 'Monthly' THEN

        SELECT (TrueSalary / PAYFREQUENCY_DIVISOR(pftype))
        FROM employeesalary es
        WHERE es.EmployeeID = NEW.EmployeeID AND
            es.OrganizationID = NEW.OrganizationID AND
            (es.EffectiveDateFrom >= NEW.PayFromDate OR IFNULL(es.EffectiveDateTo,CURDATE()) >= NEW.PayFromDate) AND
            (es.EffectiveDateFrom <= NEW.PayToDate OR IFNULL(es.EffectiveDateTo,CURDATE()) <= NEW.PayToDate)
        ORDER BY es.EffectiveDateFrom DESC
        LIMIT 1
        INTO totalWorkAmount;

        SELECT (totalWorkAmount + SUM(HolidayPayAmount) + SUM(RestDayAmount)) - (SUM(HoursLateAmount) + SUM(UndertimeHoursAmount) + SUM(Absent))
        FROM employeetimeentryactual
        WHERE OrganizationID = NEW.OrganizationID AND
            EmployeeID = NEW.EmployeeID AND
            `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate
        INTO totalWorkAmount;

        IF totalWorkAmount IS NULL THEN

            SELECT SUM(HoursLateAmount + UndertimeHoursAmount + Absent)
            FROM employeetimeentry
            WHERE OrganizationID = NEW.OrganizationID AND
                EmployeeID = NEW.EmployeeID AND
                `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate
            INTO totalWorkAmount;

            SET totalWorkAmount = IFNULL(totalWorkAmount, 0);

            SELECT totalWorkAmount + (totalWorkAmount * actualrate)
            INTO totalWorkAmount;

        END IF;

        SET totalWorkAmount = IFNULL(totalWorkAmount, 0);

    ELSEIF e_type = 'Fixed' THEN

        SELECT es.BasicPay
        FROM employeesalary es
        WHERE es.EmployeeID = NEW.EmployeeID AND
            es.OrganizationID = NEW.OrganizationID AND
            (es.EffectiveDateFrom >= NEW.PayFromDate OR IFNULL(es.EffectiveDateTo,NEW.PayToDate) >= NEW.PayFromDate) AND
            (es.EffectiveDateFrom <= NEW.PayToDate OR IFNULL(es.EffectiveDateTo,NEW.PayToDate) <= NEW.PayToDate)
        ORDER BY es.EffectiveDateFrom DESC
        LIMIT 1
        INTO totalWorkAmount;

        SET totalWorkAmount = IFNULL(totalWorkAmount, 0) * (IF(actualrate < 1, (actualrate + 1), actualrate));

    END IF;

ELSE

    SELECT
        SUM(RegularHoursAmount),
        SUM(OvertimeHoursAmount),
        SUM(NightDiffHoursAmount),
        SUM(NightDiffOTHoursAmount),
        SUM(HolidayPayAmount),
        SUM(HoursLateAmount),
        SUM(UndertimeHoursAmount),
        SUM(Absent),
        SUM(TotalDayPay),
        EmployeeSalaryID
    FROM employeetimeentryactual
    WHERE OrganizationID = NEW.OrganizationID AND
        EmployeeID = NEW.EmployeeID AND
        `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate
    INTO
        regularPay,
        overtimePay,
        nightDiffPay,
        nightDiffOvertimePay,
        holidayPay,
        lateDeduction,
        undertimeDeduction,
        absenceDeduction,
        totalWorkAmount,
        empsalRowID;

    IF totalWorkAmount IS NULL THEN

        SELECT
            SUM(TotalDayPay),
            EmployeeSalaryID
        FROM employeetimeentry
        WHERE OrganizationID = NEW.OrganizationID AND
            EmployeeID = NEW.EmployeeID AND
            `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate
        INTO
            totalWorkAmount,
            empsalRowID;

        SET totalWorkAmount = IFNULL(totalWorkAmount, 0);

        SELECT totalWorkAmount + (totalWorkAmount * actualrate)
        INTO totalWorkAmount;

    END IF;

    SET totalWorkAmount = IFNULL(totalWorkAmount, 0);

END IF;

SET actualgross = totalWorkAmount + NEW.TotalAllowance + NEW.TotalBonus;

SET @totaladjust_actual = IFNULL(
    (
        SELECT SUM(pa.PayAmount)
        FROM paystubadjustmentactual pa
        WHERE pa.PayStubID=NEW.RowID
    ),
    0
);

INSERT INTO paystubactual
(
    RowID,
    OrganizationID,
    PayPeriodID,
    EmployeeID,
    TimeEntryID,
    PayFromDate,
    PayToDate,
    RegularPay,
    OvertimePay,
    NightDiffPay,
    NightDiffOvertimePay,
    HolidayPay,
    LateDeduction,
    UndertimeDeduction,
    AbsenceDeduction,
    TotalGrossSalary,
    TotalNetSalary,
    TotalTaxableSalary,
    TotalEmpSSS,
    TotalEmpWithholdingTax,
    TotalCompSSS,
    TotalEmpPhilhealth,
    TotalCompPhilhealth,
    TotalEmpHDMF,
    TotalCompHDMF,
    TotalVacationDaysLeft,
    TotalLoans,
    TotalBonus,
    TotalAllowance,
    TotalAdjustments,
    ThirteenthMonthInclusion,
    FirstTimeSalary
)
VALUES (
    NEW.RowID,
    NEW.OrganizationID,
    NEW.PayPeriodID,
    NEW.EmployeeID,
    NEW.TimeEntryID,
    NEW.PayFromDate,
    NEW.PayToDate,
    regularPay,
    overtimePay,
    nightDiffPay,
    nightDiffOvertimePay,
    holidayPay,
    lateDeduction,
    undertimeDeduction,
    absenceDeduction,
    actualgross,
    (actualgross - (NEW.TotalEmpSSS + NEW.TotalEmpPhilhealth + NEW.TotalEmpHDMF + NEW.TotalEmpWithholdingTax)) - NEW.TotalLoans + (NEW.TotalAdjustments + @totaladjust_actual),
    NEW.TotalTaxableSalary + ((NEW.TotalTaxableSalary + NEW.TotalEmpSSS + NEW.TotalEmpPhilhealth + NEW.TotalEmpHDMF) * actualrate),
    NEW.TotalEmpSSS,
    NEW.TotalEmpWithholdingTax,
    NEW.TotalCompSSS,
    NEW.TotalEmpPhilhealth,
    NEW.TotalCompPhilhealth,
    NEW.TotalEmpHDMF,
    NEW.TotalCompHDMF,
    NEW.TotalVacationDaysLeft,
    NEW.TotalLoans,
    NEW.TotalBonus,
    NEW.TotalAllowance,
    (NEW.TotalAdjustments + @totaladjust_actual),
    NEW.ThirteenthMonthInclusion,
    NEW.FirstTimeSalary
)
ON DUPLICATE KEY
UPDATE
    OrganizationID = NEW.OrganizationID,
    PayPeriodID = NEW.PayPeriodID,
    EmployeeID = NEW.EmployeeID,
    TimeEntryID = NEW.TimeEntryID,
    PayFromDate = NEW.PayFromDate,
    PayToDate = NEW.PayToDate,
    RegularPay = regularPay,
    OvertimePay = overtimePay,
    NightDiffPay = nightDiffPay,
    NightDiffOvertimePay = nightDiffOvertimePay,
    HolidayPay = holidayPay,
    LateDeduction = lateDeduction,
    UndertimeDeduction = undertimeDeduction,
    AbsenceDeduction = absenceDeduction,
    TotalGrossSalary = actualgross,
    TotalNetSalary = (actualgross - (NEW.TotalEmpSSS + NEW.TotalEmpPhilhealth + NEW.TotalEmpHDMF + NEW.TotalEmpWithholdingTax)) - NEW.TotalLoans + (NEW.TotalAdjustments + @totaladjust_actual),
    TotalTaxableSalary = NEW.TotalTaxableSalary,
    TotalEmpSSS = NEW.TotalEmpSSS,
    TotalEmpWithholdingTax = NEW.TotalEmpWithholdingTax,
    TotalCompSSS = NEW.TotalCompSSS,
    TotalEmpPhilhealth = NEW.TotalEmpPhilhealth,
    TotalCompPhilhealth = NEW.TotalCompPhilhealth,
    TotalEmpHDMF = NEW.TotalEmpHDMF,
    TotalCompHDMF = NEW.TotalCompHDMF,
    TotalVacationDaysLeft = NEW.TotalVacationDaysLeft,
    TotalLoans = NEW.TotalLoans,
    TotalBonus = NEW.TotalBonus,
    TotalAllowance = NEW.TotalAllowance,
    TotalAdjustments = (NEW.TotalAdjustments + @totaladjust_actual),
    ThirteenthMonthInclusion = NEW.ThirteenthMonthInclusion,
    FirstTimeSalary = NEW.FirstTimeSalary;



END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
