/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `paystub_payslip`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `paystub_payslip`(
	IN `OrganizID` INT,
	IN `PayPeriodRowID` INT,
	IN `IsActualFlag` CHAR(1)
)
    DETERMINISTIC
BEGIN

DECLARE paydate_from DATE;
DECLARE paydat_to DATE;

SELECT pp.PayFromDate,pp.PayToDate FROM payperiod pp WHERE pp.RowID=PayPeriodRowID INTO paydate_from,paydat_to;

SET @daily_rate = GET_employeerateperday(9,2,paydat_to);

SELECT i.*
FROM (
    SELECT
        e.RowID,
        e.EmployeeID AS `COL1`,
        CONCAT_WS(', ',e.LastName,e.FirstName,e.MiddleName) AS `COL69`,
        IF(IsActualFlag=0, ete.RegularHoursAmount, ROUND(ete.RegularHoursAmount, 2)) AS `COL70`,
        IF(IsActualFlag = 1, es.TrueSalary, es.Salary) `COL80`,
        0 AS `COL2`,
        ROUND( IF(e.EmployeeType IN ('Fixed','Monthly'), (es.BasicPay * IF(IsActualFlag=0, 1, (es.TrueSalary / es.Salary))), IFNULL(ete.RegularHoursAmount,0)) , 2) AS `COL3`,
        IFNULL(ete.UndertimeHours, 0) AS `COL4`,
        IFNULL(ete.Absent, 0) AS `COL5`,
        IFNULL(ete.HoursLate, 0) AS `COL6`,
        IFNULL(ete.HoursLateAmount, 0) AS `COL7`,
        IFNULL(ete.UndertimeHours, 0) AS `COL8`,
        IFNULL(ete.UndertimeHoursAmount,0) AS `COL9`,
        0 AS `COL10`,
        0 AS `COL11`,
        IFNULL(ete.OvertimeHoursWorked, 0) AS `COL12`,
        IFNULL(ete.OvertimeHoursAmount, 0) AS `COL13`,
        IFNULL(ete.NightDifferentialHours, 0) AS `COL14`,
        IFNULL(ete.NightDiffHoursAmount, 0) AS `COL15`,
        0 AS `COL16`,
        IFNULL(ete.HolidayPayAmount, 0) AS `COL17`,
        (ps.TotalAllowance - IFNULL(psiECOLA.PayAmount, 0)) AS `COL18`,
        ps.TotalAdjustments `COL19`,
        ps.TotalGrossSalary AS `COL20`,
        ps.TotalEmpSSS AS `COL21`,
        ps.TotalEmpPhilhealth AS `COL22`,
        ps.TotalEmpHDMF AS `COL23`,
        ps.TotalTaxableSalary AS `COL24`,
        ps.TotalEmpWithholdingTax AS `COL25`,
        ps.TotalLoans AS `COL26`,
        ps.TotalNetSalary AS `COL27`,
        allowances.Names AS `COL28`,
        allowances.PayAmounts AS `COL29`,
        ps.TotalAllowance AS `COL30`,
        payStubLoans.Names AS `COL31`,
        payStubLoans.PayAmounts AS `COL32`,
        payStubLoans.TotalBalanceLeft AS `COl36`,
        ps.TotalLoans AS `COL33`,
        0 AS `COL34`,
        0 AS `COL35`,
        adjustments.Names AS `COL37`,
        adjustments.PayAmounts AS `COL38`,
        (IFNULL(ete.VacationLeaveHours,0) + IFNULL(ete.SickLeaveHours,0) + IFNULL(ete.MaternityLeaveHours,0) + IFNULL(ete.OtherLeaveHours,0)) AS `COL40`,
        IFNULL(ete.Leavepayment,0) AS `COL41`,
        IFNULL(psiHoli.PayAmount,0) AS `COL42`,
        IFNULL(psiECOLA.PayAmount,0) AS `COL43`,
        psiLeave.Names AS `COL44`
    FROM (
            SELECT
                RowID,
                OrganizationID,
                PayPeriodID,
                EmployeeID,
                TimeEntryID,
                PayFromDate,
                PayToDate,
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
            RowID,
            EmployeeID,
            SUM(RegularHoursWorked) AS RegularHoursWorked,
            SUM(RegularHoursAmount) AS RegularHoursAmount,
            SUM(TotalHoursWorked) AS TotalHoursWorked,
            SUM(OvertimeHoursWorked) AS OvertimeHoursWorked,
            SUM(OvertimeHoursAmount) AS OvertimeHoursAmount,
            SUM(UndertimeHours) AS UndertimeHours,
            SUM(UndertimeHoursAmount) AS UndertimeHoursAmount,
            SUM(NightDifferentialHours) AS NightDifferentialHours,
            SUM(NightDiffHoursAmount) AS NightDiffHoursAmount,
            SUM(NightDifferentialOTHours) AS NightDifferentialOTHours,
            SUM(NightDiffOTHoursAmount) AS NightDiffOTHoursAmount,
            SUM(HoursLate) AS HoursLate,
            SUM(HoursLateAmount) AS HoursLateAmount,
            SUM(VacationLeaveHours) AS VacationLeaveHours,
            SUM(SickLeaveHours) AS SickLeaveHours,
            SUM(MaternityLeaveHours) AS MaternityLeaveHours,
            SUM(OtherLeaveHours) AS OtherLeaveHours,
            SUM(TotalDayPay) AS TotalDayPay,
            SUM(Absent) AS Absent,
            SUM(TaxableDailyAllowance) AS TaxableDailyAllowance,
            SUM(HolidayPayAmount) AS HolidayPayAmount,
            SUM(TaxableDailyBonus) AS TaxableDailyBonus,
            SUM(NonTaxableDailyBonus) AS NonTaxableDailyBonus,
            SUM(Leavepayment) AS Leavepayment
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
                    TaxableDailyAllowance,
                    HolidayPayAmount,
                    TaxableDailyBonus,
                    NonTaxableDailyBonus,
                    Leavepayment
                FROM employeetimeentry
                WHERE OrganizationID=OrganizID AND
                    IsActualFlag=0 AND
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
                    TaxableDailyAllowance,
                    HolidayPayAmount,
                    TaxableDailyBonus,
                    NonTaxableDailyBonus,
                    Leavepayment
                FROM employeetimeentryactual
                WHERE OrganizationID=OrganizID AND
                    IsActualFlag=1 AND
                    `Date` BETWEEN paydate_from AND paydat_to
        ) i
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
            REPLACE(GROUP_CONCAT(IFNULL(scheduledloansperpayperiod.DeductionAmount, '')), ',', '\n') 'PayAmounts',
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
            REPLACE(GROUP_CONCAT(IFNULL(p.PartNo, '')), ',', '\n') `Names`
        FROM paystubitem psi
        INNER JOIN product p
        ON p.RowID=psi.ProductID AND
            p.OrganizationID=psi.OrganizationID AND
            p.`Category`='Leave Type'
        WHERE psi.OrganizationID=OrganizID AND
            psi.PayAmount!=0
        GROUP BY psi.PayStubID
    ) psiLeave
    ON psiLeave.PayStubID=ps.RowID
    LEFT JOIN (
        SELECT
            psi.*,
            p.PartNo AS ItemName
        FROM paystubitem psi
        INNER JOIN product p
        ON p.RowID=psi.ProductID AND
            p.OrganizationID=psi.OrganizationID AND
            p.PartNo='Holiday pay'
        WHERE psi.Undeclared=IsActualFlag AND
            psi.OrganizationID=OrganizID AND
            psi.PayAmount!=0
    ) psiHoli
    ON psiHoli.PayStubID=ps.RowID
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
            REPLACE(GROUP_CONCAT(IFNULL(adjustment.PayAmount, '')), ',', '\n') `PayAmounts`,
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
) i;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
