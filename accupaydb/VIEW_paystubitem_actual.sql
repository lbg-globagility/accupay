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
    psa.PayFromDate,
    psa.PayToDate,

    ps.RegularHours,
    psa.RegularPay,
    ps.OvertimeHours,
    psa.OvertimePay,
    ps.NightDiffHours,
    psa.NightDiffPay,
    ps.NightDiffOvertimeHours,
    psa.NightDiffOvertimePay,
    ps.RestDayHours,
    psa.RestDayPay,
    ps.RestDayOTHours,
    psa.RestDayOTPay,
    ps.LeaveHours,
    psa.LeavePay,

    ps.SpecialHolidayHours,
    psa.SpecialHolidayPay,
    ps.SpecialHolidayOTHours,
    psa.SpecialHolidayOTPay,
    ps.RegularHolidayHours,
    psa.RegularHolidayPay,
    ps.RegularHolidayOTHours,
    psa.RegularHolidayOTPay,

    ps.LateHours,
    psa.LateDeduction,
    ps.UndertimeHours,
    psa.UndertimeDeduction,
    ps.AbsentHours,
    psa.AbsenceDeduction,

    ps.TotalBonus,
    psa.TotalAllowance,
    psa.TotalTaxableAllowance,

    psa.TotalGrossSalary,

    ps.TotalTaxableSalary,
    ps.TotalEmpSSS,
    ps.TotalEmpWithholdingTax,
    ps.TotalCompSSS,
    ps.TotalEmpPhilhealth,
    ps.TotalCompPhilhealth,
    ps.TotalEmpHDMF,
    ps.TotalCompHDMF,
    ps.TotalLoans,
    
    psa.TotalNetSalary,
    
    psa.TotalVacationDaysLeft,
    
    psa.TotalAdjustments,
    psa.FirstTimeSalary,
   
    psa.HolidayPay,
    (es.Salary + es.UndeclaredSalary) / IF(e.EmployeeType = 'Daily', PAYFREQUENCY_DIVISOR(e.EmployeeType), PAYFREQUENCY_DIVISOR(pf.PayFrequencyType)) AS 'BasicPay',
    es.TrueSalary,
    IF(e.EmployeeType='Daily',PAYFREQUENCY_DIVISOR(e.EmployeeType),PAYFREQUENCY_DIVISOR(pf.PayFrequencyType)) AS PAYFREQUENCYDIVISOR,
    ete.*,
    e.EmployeeType,
    (e.StartDate BETWEEN pay_date_from AND pay_date_to) AS FirstTimeSalary,
    thirteenthmonthpay.Amount AS 'ThirteenthMonthPay'
FROM paystubactual psa
INNER JOIN paystub ps
ON ps.OrganizationID = psa.OrganizationID AND
    ps.EmployeeID = psa.EmployeeID AND
    ps.PayPeriodID = psa.PayPeriodID
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
        SUM(etea.TotalDayPay) `TotalDayPay`,
        SUM(agencyfee.DailyFee) `TotalAgencyFee`
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
