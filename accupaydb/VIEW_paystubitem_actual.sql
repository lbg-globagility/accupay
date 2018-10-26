/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_paystubitem_actual`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `VIEW_paystubitem_actual`(
	IN `OrganizID` INT,
	IN `EmpRowID` INT,
	IN `pay_date_from` DATE,
	IN `pay_date_to` DATE
)
    DETERMINISTIC
BEGIN

SELECT
    psa.RowID,
    psa.PayPeriodID,
    pstb.RegularHours,
    psa.RegularPay,
    ete.OvertimeHoursWorked `OvertimeHours`,
    psa.OvertimePay `OvertimePay`,
    psa.OvertimePay `OvertimeHoursAmount`,
    pstb.NightDiffHours,
    psa.NightDiffPay,
    pstb.NightDiffOvertimeHours,
    psa.NightDiffOvertimePay,
    pstb.RestDayHours,
    psa.RestDayPay,
    pstb.LateHours,
    psa.LateDeduction,
    pstb.UndertimeHours,
    psa.UndertimeDeduction,
    psa.TotalGrossSalary,
    psa.TotalNetSalary,
    psa.TotalTaxableSalary,
    psa.TotalEmpSSS,
    psa.TotalEmpWithholdingTax,
    psa.TotalCompSSS,
    psa.TotalEmpPhilhealth,
    psa.TotalCompPhilhealth,
    psa.TotalEmpHDMF,
    psa.TotalCompHDMF,
    psa.TotalVacationDaysLeft,
    psa.RestDayPay,
    psa.TotalLoans,
    psa.TotalBonus,
    psa.TotalAllowance,
    psa.TotalAdjustments,
    psa.ThirteenthMonthInclusion,
    psa.FirstTimeSalary,
    psa.PayPeriodID,
    psa.PayFromDate,
    psa.PayToDate,
    psa.TotalGrossSalary,
    psa.TotalNetSalary,
    psa.TotalTaxableSalary,
    psa.TotalEmpSSS,
    psa.TotalEmpWithholdingTax,
    psa.TotalCompSSS,
    psa.TotalEmpPhilhealth,
    psa.TotalCompPhilhealth,
    psa.TotalEmpHDMF,
    psa.TotalCompHDMF,
    psa.TotalVacationDaysLeft,
    psa.TotalLoans,
    psa.TotalBonus,
    psa.TotalAllowance,
    psa.TotalAdjustments,
    psa.ThirteenthMonthInclusion,
    psa.FirstTimeSalary,
    psa.HolidayPay,
    es.BasicPay * (es.TrueSalary / es.Salary) AS `BasicPay`,
    es.TrueSalary,
    IF(e.EmployeeType='Daily',PAYFREQUENCY_DIVISOR(e.EmployeeType),PAYFREQUENCY_DIVISOR(pf.PayFrequencyType)) AS PAYFREQUENCYDIVISOR,
    ete.*,
    e.EmployeeType,
    (e.StartDate BETWEEN pay_date_from AND pay_date_to) AS FirstTimeSalary,
    SUM(IFNULL(ete.Leavepayment,0)) `PaidLeaveAmount`,
    thirteenthmonthpay.Amount AS 'ThirteenthMonthPay',
    psa.SpecialHolidayOTPay,
    psa.RegularHolidayOTPay,
    psa.RegularHolidayPay
FROM paystubactual psa
INNER JOIN paystub pstb
ON pstb.OrganizationID = psa.OrganizationID AND
    pstb.EmployeeID = psa.EmployeeID AND
    pstb.PayPeriodID = psa.PayPeriodID
INNER JOIN employee e
ON e.RowID = psa.EmployeeID AND
    e.OrganizationID = psa.OrganizationID
INNER JOIN payfrequency pf
ON pf.RowID = e.PayFrequencyID
INNER JOIN employeesalary es
ON es.EmployeeID = psa.EmployeeID AND
    es.OrganizationID = psa.OrganizationID AND
    (es.EffectiveDateFrom >= psa.PayFromDate OR IFNULL(es.EffectiveDateTo, CURDATE()) >= psa.PayFromDate) AND
    (es.EffectiveDateFrom <= psa.PayToDate OR IFNULL(es.EffectiveDateTo, CURDATE()) <= psa.PayToDate)
LEFT JOIN thirteenthmonthpay
ON thirteenthmonthpay.PaystubID = psa.RowID
LEFT JOIN (
    SELECT
        etea.RowID `eteRowID`,
        SUM(etea.RegularHoursWorked) `RegularHoursWorked`,
        SUM(etea.RegularHoursAmount) `RegularHoursAmount`,
        SUM(etea.TotalHoursWorked) `TotalHoursWorked`,
        SUM(etea.OvertimeHoursWorked) `OvertimeHoursWorked`,
        SUM(etea.OvertimeHoursAmount) `OvertimeHoursAmount`,
        SUM(etea.UndertimeHours) `UndertimeHours`,
        SUM(etea.UndertimeHoursAmount) `UndertimeHoursAmount`,
        SUM(etea.NightDifferentialHours) `NightDifferentialHours`,
        SUM(etea.NightDiffHoursAmount) `NightDiffHoursAmount`,
        SUM(etea.NightDifferentialOTHours) `NightDifferentialOTHours`,
        SUM(etea.NightDiffOTHoursAmount) `NightDiffOTHoursAmount`,
        SUM(etea.HoursLate) `HoursLate`,
        SUM(etea.HoursLateAmount) `HoursLateAmount`,
        SUM(etea.VacationLeaveHours) `VacationLeaveHours`,
        SUM(etea.SickLeaveHours) `SickLeaveHours`,
        SUM(etea.MaternityLeaveHours) `MaternityLeaveHours`,
        SUM(etea.OtherLeaveHours) `OtherLeaveHours`,
        SUM(etea.TotalDayPay) `TotalDayPay`,
        SUM(etea.Absent) `Absent`,
        SUM(etea.Leavepayment) `Leavepayment`,
        SUM(agencyfee.DailyFee) `TotalAgencyFee`,
        SUM(etea.SpecialHolidayPay) `SpecialHolidayPay`,
        SUM(etea.SpecialHolidayOTPay) `SpecialHolidayOTPay`,
        SUM(etea.RestDayOTPay) `RestDayOTPay`,
        SUM(et.RegularHolidayOTHours) `RegularHolidayOTHours`,
        SUM(et.SpecialHolidayOTHours) `SpecialHolidayOTHours`,
        SUM(et.RestDayOTHours) `RestDayOTHours`,
        SUM(IFNULL(et.RegularHolidayHours, 0)) `RegularHolidayHours`,
        SUM(IFNULL(et.SpecialHolidayHours, 0)) `SpecialHolidayHours`,
        SUM(IFNULL(et.RestDayHours, 0)) `RestDayHours`
    FROM employeetimeentryactual etea
    INNER JOIN employeetimeentry et
    ON et.EmployeeID = etea.EmployeeID AND
        et.OrganizationID = etea.OrganizationID AND
        et.`Date` = etea.`Date`
    LEFT JOIN agencyfee
    ON agencyfee.EmployeeID = etea.EmployeeID AND
        agencyfee.TimeEntryDate = etea.`Date`
    WHERE etea.EmployeeID = EmpRowID AND
        etea.OrganizationID=OrganizID AND
        etea.`Date` BETWEEN pay_date_from AND pay_date_to
) ete
ON ete.eteRowID IS NOT NULL
WHERE psa.EmployeeID = EmpRowID AND
    psa.OrganizationID = OrganizID AND
    psa.PayFromDate = pay_date_from AND
    psa.PayToDate = pay_date_to
ORDER BY es.EffectiveDateFrom DESC
LIMIT 1;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
