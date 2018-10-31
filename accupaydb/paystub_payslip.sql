/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `paystub_payslip`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `paystub_payslip`(IN `OrganizID` INT, IN `PayPeriodRowID` INT, IN `IsActualFlag` CHAR(1)
)
    DETERMINISTIC
BEGIN

DECLARE paydate_from DATE;
DECLARE paydat_to DATE;

DECLARE sec_per_hour INT(11) DEFAULT 3600;

SELECT
    pp.PayFromDate,
    pp.PayToDate
FROM payperiod pp
WHERE pp.RowID = PayPeriodRowID
INTO
    paydate_from,
    paydat_to;

SELECT
    e.RowID,
    CONCAT(e.EmployeeID, ' / ', e.LastName, ', ', e.FirstName, ' ', LEFT(e.MiddleName, 1)) AS `COL1`,
    CONCAT_WS(', ', e.LastName, e.FirstName, e.MiddleName) AS `COL69`,
    ROUND(GetBasicPay(e.RowID, paydate_from, paydat_to, IsActualFlag, ete.TotalExpectedHours), 2) AS `COL70`,
    (
        ROUND(IF(
            e.EmployeeType IN ('Fixed', 'Monthly'),
            IF(
                e.PayFrequencyID = 2,
                e.WorkDaysPerYear / 12 * 8, -- monthly pay frequency
                e.WorkDaysPerYear / 12 / 2 * 8 -- semi-monthly pay frequency
            ),
            ete.TotalExpectedHours
        ), 2)
    ) `COL71`,
    IFNULL(ete.AbsentHours +
        IF(
            e.EmployeeType = 'Monthly',
            (IFNULL(ete.VacationLeaveHours, 0) + IFNULL(ete.SickLeaveHours, 0) + IFNULL(ete.MaternityLeaveHours, 0) + IFNULL(ete.OtherLeaveHours, 0)),
            0
        ),
        0
    ) `COL72`,
    IF(IsActualFlag = 1, es.TrueSalary, es.Salary) `COL80`,
    0 AS `COL2`, -- Deprecated
    ROUND(
        IF(
            e.EmployeeType IN ('Fixed', 'Monthly'),
            (es.BasicPay * IF(
                IsActualFlag = 0,
                1,
                (es.TrueSalary / es.Salary)
            )),
            IFNULL(ete.RegularHoursAmount, 0)
        ),
        2
    ) AS `COL3`, -- Deprecated
    IFNULL(ete.UndertimeHours, 0) AS `COL4`, -- Deprecated
    IFNULL(ete.Absent + IF(e.EmployeeType = 'Monthly', ete.Leavepayment, 0), 0) AS `COL5`,
    IFNULL(ete.HoursLate, 0) AS `COL6`,
    IFNULL(ps.LateDeduction, 0) AS `COL7`,
    IFNULL(ete.UndertimeHours, 0) AS `COL8`,
    IFNULL(ps.UndertimeDeduction, 0) AS `COL9`,
    0 AS `COL10`, -- Deprecated
    0 AS `COL11`, -- Deprecated
    (
        IFNULL(ete.OvertimeHoursWorked, 0)
     ) AS `COL12`,
    (
        IFNULL(ps.OvertimePay, 0) +
        IFNULL(ps.NightDiffOvertimePay, 0) +
        IFNULL(ps.RestDayOTPay, 0) +
        IFNULL(ps.SpecialHolidayOTPay, 0) +
        IFNULL(ps.RegularHolidayOTPay, 0)
     ) AS `COL13`,
    IFNULL(ete.NightDifferentialHours, 0) AS `COL14`,
    IFNULL(ps.NightDiffPay, 0) AS `COL15`,
    0 AS `COL16`, -- Deprecated
    IFNULL(ete.HolidayPayAmount, 0) + IFNULL(ete.RestDayAmount, 0) AS `COL17`, -- Deprecated
    (ps.TotalAllowance - IFNULL(psiECOLA.PayAmount, 0)) AS `COL18`,
    ps.TotalAdjustments `COL19`,
    (ps.TotalGrossSalary + ps.TotalAdjustments) AS `COL20`,
    ps.TotalEmpSSS AS `COL21`,
    ps.TotalEmpPhilhealth AS `COL22`,
    ps.TotalEmpHDMF AS `COL23`,
    ps.TotalTaxableSalary AS `COL24`, -- Deprecated
    ps.TotalEmpWithholdingTax AS `COL25`,
    ps.TotalLoans AS `COL26`,
    ps.TotalNetSalary AS `COL27`,
    allowances.`Names` AS `COL28`, -- Deprecated
    allowances.PayAmounts AS `COL29`, -- Deprecated
    ps.TotalAllowance AS `COL30`, -- Deprecated
    payStubLoans.`Names` AS `COL31`,
    payStubLoans.PayAmounts AS `COL32`,
    payStubLoans.TotalBalanceLeft AS `COl36`,
    ps.TotalLoans AS `COL33`, -- Deprecated
    0 AS `COL34`, -- Deprecated
    0 AS `COL35`, -- Deprecated
    adjustments.`Names` AS `COL37`, -- Deprecated
    adjustments.PayAmounts AS `COL38`,-- Deprecated
    (IFNULL(ete.VacationLeaveHours,0) + IFNULL(ete.SickLeaveHours,0) + IFNULL(ete.MaternityLeaveHours,0) + IFNULL(ete.OtherLeaveHours,0)) AS `COL40`,
    IFNULL(ete.Leavepayment,0) AS `COL41`,
	IFNULL(ps.HolidayPay, 0) + IFNULL(ps.RestDayPay, 0) AS `COL42`,
    IFNULL(psiECOLA.PayAmount,0) AS `COL43`,
    psiLeave.`Names` AS `COL44`,
    psiLeave.Availed AS `COl45`,
    psiLeave.Balance AS `COL46`,
    IFNULL(adjustments.`Names`, '') `COL90`,
    IFNULL(adjustments.`PayAmounts`, '') `COL91`
FROM (
        SELECT
            RowID,
            OrganizationID,
            PayPeriodID,
            EmployeeID,
            TimeEntryID,
            PayFromDate,
            PayToDate,
            HolidayPay,
            OvertimePay,
            NightDiffPay,
            NightDiffOvertimePay,
            RestDayOTPay,
            SpecialHolidayOTPay,
            RegularHolidayOTPay,
            LateDeduction,
            UndertimeDeduction,
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
            FirstTimeSalary,
            RestDayPay
        FROM paystub
        WHERE IsActualFlag = 0 AND
            OrganizationID = OrganizID
    UNION
        SELECT
            RowID,
            OrganizationID,
            PayPeriodID,
            EmployeeID,
            TimeEntryID,
            PayFromDate,
            PayToDate,
            HolidayPay,
            OvertimePay,
            NightDiffPay,
            NightDiffOvertimePay,
            RestDayOTPay,
            SpecialHolidayOTPay,
            RegularHolidayOTPay,
            LateDeduction,
            UndertimeDeduction,
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
            FirstTimeSalary,
            RestDayPay
        FROM paystubactual
        WHERE IsActualFlag = 1 AND
            OrganizationID = OrganizID
) ps
INNER JOIN employee e
ON e.RowID = ps.EmployeeID AND
    e.OrganizationID = ps.OrganizationID
INNER JOIN employeesalary es
ON es.EmployeeID = ps.EmployeeID AND
    es.OrganizationID=ps.OrganizationID AND
    (es.EffectiveDateFrom >= ps.PayFromDate OR IFNULL(es.EffectiveDateTo,ps.PayToDate) >= ps.PayFromDate) AND
    (es.EffectiveDateFrom <= ps.PayToDate OR IFNULL(es.EffectiveDateTo,ps.PayToDate) <= ps.PayToDate)
LEFT JOIN (
    SELECT
        i.RowID,
        i.EmployeeID,
        SUM(i.RegularHoursWorked) AS RegularHoursWorked,
        SUM(i.RegularHoursAmount) AS RegularHoursAmount,
        SUM(i.TotalHoursWorked) AS TotalHoursWorked,
        SUM(i.OvertimeHoursWorked) AS OvertimeHoursWorked,
        SUM(i.UndertimeHours) AS UndertimeHours,
        SUM(i.UndertimeHoursAmount) AS UndertimeHoursAmount,
        SUM(i.NightDifferentialHours) AS NightDifferentialHours,
        SUM(i.NightDifferentialOTHours) AS NightDifferentialOTHours,
        SUM(i.HoursLate) AS HoursLate,
        SUM(i.VacationLeaveHours) AS VacationLeaveHours,
        SUM(i.SickLeaveHours) AS SickLeaveHours,
        SUM(i.MaternityLeaveHours) AS MaternityLeaveHours,
        SUM(i.OtherLeaveHours) AS OtherLeaveHours,
        SUM(i.TotalDayPay) AS TotalDayPay,
        SUM(i.Absent) AS Absent,
        SUM(i.TaxableDailyAllowance) AS TaxableDailyAllowance,
        SUM(i.HolidayPayAmount) AS HolidayPayAmount,
        SUM(i.TaxableDailyBonus) AS TaxableDailyBonus,
        SUM(i.NonTaxableDailyBonus) AS NonTaxableDailyBonus,
        SUM(i.Leavepayment) AS Leavepayment,
        SUM(i.RestDayAmount) AS RestDayAmount,
        SUM(
            IF(
                (
                    (pyr.PayType = 'Regular Holiday') OR
                    (pyr.PayType = 'Special Non-Working Holiday') OR
                    (i.Leavepayment > 0) OR
                    es.RestDay = TRUE
                ),
                0,
                sh.DivisorToDailyRate
            )
		) `TotalExpectedHours`,
        SUM(IF(i.Absent > 0, sh.DivisorToDailyRate, 0)) AS AbsentHours
    FROM (
            SELECT
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
                UndertimeHours,
                UndertimeHoursAmount,
                NightDifferentialHours,
                NightDiffHoursAmount,
                NightDifferentialOTHours,
                HoursLate,
                LateFlag,
                PayRateID,
                VacationLeaveHours,
                SickLeaveHours,
                MaternityLeaveHours,
                OtherLeaveHours,
                TotalDayPay,
                Absent,
                ChargeToDivisionID,
                TaxableDailyAllowance,
                HolidayPayAmount,
                TaxableDailyBonus,
                NonTaxableDailyBonus,
                Leavepayment,
                RestDayAmount
            FROM employeetimeentry
            WHERE OrganizationID = OrganizID AND
                IsActualFlag = 0 AND
                `Date` BETWEEN paydate_from AND paydat_to
        UNION
            SELECT
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
                UndertimeHours,
                UndertimeHoursAmount,
                NightDifferentialHours,
                NightDiffHoursAmount,
                NightDifferentialOTHours,
                HoursLate,
                LateFlag,
                PayRateID,
                VacationLeaveHours,
                SickLeaveHours,
                MaternityLeaveHours,
                OtherLeaveHours,
                TotalDayPay,
                Absent,
                ChargeToDivisionID,
                TaxableDailyAllowance,
                HolidayPayAmount,
                TaxableDailyBonus,
                NonTaxableDailyBonus,
                Leavepayment,
                RestDayAmount
            FROM employeetimeentryactual
            WHERE OrganizationID = OrganizID AND
                IsActualFlag = 1 AND
                `Date` BETWEEN paydate_from AND paydat_to
    ) i
    INNER JOIN employeeshift es
    ON es.RowID = i.EmployeeShiftID
    INNER JOIN shift sh
    ON sh.RowID = es.ShiftID
    INNER JOIN payrate pyr
    ON pyr.Date = i.Date AND
        pyr.OrganizationID = OrganizID
    GROUP BY i.EmployeeID
) ete
ON ete.RowID IS NOT NULL AND
    ete.EmployeeID=ps.EmployeeID
LEFT JOIN (
    SELECT
        REPLACE(GROUP_CONCAT(IFNULL(product.PartNo, '')), ',', '\n') 'Names',
        REPLACE(GROUP_CONCAT(IFNULL(paystubitem.PayAmount, '')), ',', '\n') 'PayAmounts',
        paystubitem.PayStubID
    FROM paystubitem
    INNER JOIN product
    ON product.RowID = paystubitem.ProductID AND
        product.OrganizationID = paystubitem.OrganizationID AND
        product.`Category` = 'Allowance Type'
    GROUP BY paystubitem.PayStubID
) allowances
ON allowances.PayStubID = ps.RowID
LEFT JOIN (
    SELECT
        REPLACE(GROUP_CONCAT(IFNULL(product.PartNo, '')), ',', '\n') 'Names',
        REPLACE(GROUP_CONCAT(IFNULL(ROUND(scheduledloansperpayperiod.DeductionAmount, 2), '')), ',', '\n') 'PayAmounts',
        REPLACE(GROUP_CONCAT(IFNULL(ROUND(scheduledloansperpayperiod.TotalBalanceLeft, 2), '')), ',', '\n') 'TotalBalanceLeft',
        paystub.RowID 'PayStubID'
    FROM scheduledloansperpayperiod
    INNER JOIN employeeloanschedule
    ON employeeloanschedule.RowID = scheduledloansperpayperiod.EmployeeLoanRecordID
    INNER JOIN paystub
    ON paystub.PayPeriodID = scheduledloansperpayperiod.PayPeriodID AND
        paystub.EmployeeID = scheduledloansperpayperiod.EmployeeID
    INNER JOIN product
    ON product.RowID = employeeloanschedule.LoanTypeID
    GROUP BY paystub.RowID
) payStubLoans
ON payStubLoans.PayStubID = ps.RowID
LEFT JOIN (
    SELECT
        psi.PayStubID,
        REPLACE(GROUP_CONCAT(IFNULL(p.PartNo, '')), ',', '\n') `Names`,
        REPLACE(
            GROUP_CONCAT(
                IFNULL(
                    IF(
                        p.PartNo = 'Sick leave',
                        (FORMAT(ete.SickLeaveHours / 8, 2)),
                        (FORMAT(ete.VacationLeaveHours / 8, 2))
                    ),
                    ''
                )
            ),
            ',',
            '\n'
        ) 'Availed',
        REPLACE(GROUP_CONCAT(IFNULL(FORMAT(psi.PayAmount / 8, 2), 0)), ',', '\n') 'Balance'
    FROM paystubitem psi
    INNER JOIN product p
    ON p.RowID = psi.ProductID AND
        p.OrganizationID = psi.OrganizationID AND
        p.`Category` = 'Leave Type' AND
        p.PartNo = 'Vacation leave'
    INNER JOIN paystub ps
    ON ps.RowID = psi.PayStubID
    INNER JOIN (
        SELECT
            EmployeeID,
            SUM(VacationLeaveHours) `VacationLeaveHours`,
            SUM(SickLeaveHours) `SickLeaveHours`
        FROM employeetimeentry
        WHERE `Date` BETWEEN paydate_from AND paydat_to
        GROUP BY EmployeeID
    ) ete
    ON ete.EmployeeID = ps.EmployeeID
    WHERE psi.OrganizationID = OrganizID
    GROUP BY psi.PayStubID
) psiLeave
ON psiLeave.PayStubID = ps.RowID
LEFT JOIN (
    SELECT
        psi.*,
        p.PartNo AS ItemName
    FROM paystubitem psi
    INNER JOIN product p
    ON p.RowID=psi.ProductID AND
        p.OrganizationID=psi.OrganizationID AND
        p.PartNo='Ecola'
    WHERE psi.Undeclared=0 AND
        psi.OrganizationID=OrganizID AND
        psi.PayAmount!=0
) psiECOLA
ON psiECOLA.PayStubID=ps.RowID
LEFT JOIN (
    SELECT
        REPLACE(GROUP_CONCAT(IFNULL(adjustment.ItemName, '')), ',', '\n') `Names`,
        REPLACE(GROUP_CONCAT(IFNULL(ROUND(adjustment.PayAmount, 2), '')), ',', '\n') `PayAmounts`,
        adjustment.PayStubID
    FROM (
            SELECT
                paystubadjustment.PayStubID,
                paystubadjustment.PayAmount,
                product.PartNo AS ItemName
            FROM paystubadjustment
            INNER JOIN product
            ON product.RowID = paystubadjustment.ProductID
            WHERE IsActualFlag = 0 AND
                paystubadjustment.OrganizationID = OrganizID AND
                paystubadjustment.PayAmount != 0
        UNION
            SELECT
                paystubadjustmentactual.PayStubID,
                paystubadjustmentactual.PayAmount,
                product.PartNo AS ItemName
            FROM paystubadjustmentactual
            INNER JOIN product
            ON product.RowID = paystubadjustmentactual.ProductID
            WHERE IsActualFlag = 1 AND
                paystubadjustmentactual.OrganizationID = OrganizID AND
                paystubadjustmentactual.PayAmount != 0
    ) adjustment
    GROUP BY adjustment.PayStubID
) adjustments
ON adjustments.PayStubID = ps.RowID
WHERE ps.OrganizationID=OrganizID AND
    ps.PayFromDate=paydate_from AND
    ps.PayToDate=paydat_to
GROUP BY ps.EmployeeID
ORDER BY CONCAT(e.LastName, e.FirstName, e.MiddleName);

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
