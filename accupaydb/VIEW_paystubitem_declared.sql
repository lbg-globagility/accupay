/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_paystubitem_declared`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `VIEW_paystubitem_declared`(
	IN `OrganizID` INT,
	IN `EmpRowID` INT,
	IN `pay_date_from` DATE,
	IN `pay_date_to` DATE




)
    DETERMINISTIC
BEGIN

DECLARE themonth INT(11);
DECLARE theyear INT(11);

DECLARE startdate_ofpreviousmonth DATE;
DECLARE enddate_ofpreviousmonth DATE;

SELECT
    pp.`Month`,
    pp.`Year`
FROM payperiod pp
INNER JOIN employee e
ON e.RowID = EmpRowID AND
    e.OrganizationID = OrganizID
WHERE pp.OrganizationID = OrganizID AND
    pp.TotalGrossSalary = e.PayFrequencyID AND
    pp.PayFromDate = pay_date_from AND
    pp.PayToDate = pay_date_to
INTO
    themonth,
    theyear;

SELECT
    pp.PayFromDate,
    pp.PayToDate
FROM payperiod pp
INNER JOIN employee e
ON e.RowID = EmpRowID AND
    e.OrganizationID = OrganizID
WHERE pp.OrganizationID = OrganizID AND
    pp.TotalGrossSalary = e.PayFrequencyID AND
    pp.`Month` = MONTH(SUBDATE(DATE(CONCAT('2016-', themonth, '-01')), INTERVAL 1 DAY)) AND
    pp.`Year` = theyear
ORDER BY pp.PayFromDate DESC,
    pp.PayToDate DESC
LIMIT 1
INTO
    startdate_ofpreviousmonth,
    enddate_ofpreviousmonth;

SELECT
    psa.RowID,
    psa.PayPeriodID,
    psa.RegularHours,
    psa.RegularPay,
    psa.OvertimeHours,
    /*(psa.OvertimePay
	  + IFNULL(ete.SpecialHolidayOTPay, 0)
	  + IFNULL(ete.RestDayOTPay, 0)
	  ) `OvertimePay`,*/
	  psa.OvertimePay,
    psa.NightDiffHours,
    psa.NightDiffPay,
    psa.NightDiffOvertimeHours,
    psa.NightDiffOvertimePay,
    psa.RestDayHours,
    psa.RestDayPay,
    psa.LateHours,
    psa.LateDeduction,
    psa.UndertimeHours,
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
    psa.TotalUndeclaredSalary,
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
    IF(
        e.AgencyID IS NULL,
        IF(BINARY `PAYMENT_SCHED_TO_CHAR`(d.WTaxDeductSched) = BINARY pp.`Half`, psa.TotalTaxableSalary, 0),
        IF(BINARY `PAYMENT_SCHED_TO_CHAR`(d.WTaxDeductSchedAgency) = BINARY pp.`Half`, psa.TotalTaxableSalary, 0)
    ) AS TotalTaxableSalary,
    psa.TotalEmpSSS,
    psa.TotalEmpWithholdingTax,
    psa.TotalCompSSS,
    psa.TotalEmpPhilhealth,
    psa.TotalCompPhilhealth,
    psa.TotalEmpHDMF,
    psa.TotalCompHDMF,
    psa.TotalVacationDaysLeft,
    psa.TotalUndeclaredSalary,
    psa.TotalLoans,
    psa.TotalBonus,
    psa.TotalAllowance,
    psa.TotalAdjustments,
    psa.ThirteenthMonthInclusion,
    psa.FirstTimeSalary,
    es.BasicPay,
    es.Salary AS TrueSalary,
    IF(
        e.EmployeeType = 'Daily',
        PAYFREQUENCY_DIVISOR(e.EmployeeType),
        PAYFREQUENCY_DIVISOR(pf.PayFrequencyType)
    ) AS PAYFREQUENCYDIVISOR,
    ete.*,
    e.EmployeeType,
    (e.StartDate BETWEEN pay_date_from AND pay_date_to) AS FirstTimeSalary,
    SUM(IFNULL(ete.Leavepayment,0)) `PaidLeaveAmount`,
    thirteenthmonthpay.Amount AS 'ThirteenthMonthPay',
    psa.HolidayPay
FROM paystub psa
INNER JOIN employee e
ON e.RowID = psa.EmployeeID AND
    e.OrganizationID = psa.OrganizationID
INNER JOIN payperiod pp
ON pp.RowID = psa.PayPeriodID
LEFT JOIN position pos
ON pos.RowID = e.PositionID
LEFT JOIN `division` d
ON d.RowID = pos.DivisionId
INNER JOIN payfrequency pf
ON pf.RowID = e.PayFrequencyID
INNER JOIN employeesalary es
ON es.EmployeeID = psa.EmployeeID AND
    es.OrganizationID = psa.OrganizationID AND
    (es.EffectiveDateFrom >= psa.PayFromDate OR IFNULL(es.EffectiveDateTo,CURDATE()) >= psa.PayFromDate) AND
    (es.EffectiveDateFrom <= psa.PayToDate OR IFNULL(es.EffectiveDateTo,CURDATE()) <= psa.PayToDate)
LEFT JOIN thirteenthmonthpay
ON thirteenthmonthpay.PaystubID = psa.RowID
LEFT JOIN (
    SELECT
        etea.RowID AS eteRowID,
        SUM(etea.TotalHoursWorked) AS TotalHoursWorked,
        SUM(etea.VacationLeaveHours) AS VacationLeaveHours,
        SUM(etea.SickLeaveHours) AS SickLeaveHours,
        SUM(etea.MaternityLeaveHours) AS MaternityLeaveHours,
        SUM(etea.OtherLeaveHours) AS OtherLeaveHours,
        SUM(etea.TotalDayPay) AS TotalDayPay,
        SUM(etea.Absent) AS Absent,
        SUM(etea.Leavepayment) AS Leavepayment,
        SUM(agencyfee.DailyFee) AS TotalAgencyFee,
        SUM(etea.SpecialHolidayPay) `SpecialHolidayPay`,
		  SUM(etea.SpecialHolidayOTPay) `SpecialHolidayOTPay`,
		  SUM(etea.RestDayOTPay) `RestDayOTPay`
    FROM employeetimeentry etea
    INNER JOIN payrate pr
    ON pr.RowID = etea.PayRateID
    LEFT JOIN agencyfee
    ON agencyfee.EmployeeID = etea.EmployeeID AND
        agencyfee.TimeEntryDate = etea.Date
    WHERE etea.EmployeeID = EmpRowID AND
        etea.OrganizationID = OrganizID AND
        etea.`Date` BETWEEN pay_date_from AND pay_date_to
) ete
ON ete.eteRowID > 0
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
