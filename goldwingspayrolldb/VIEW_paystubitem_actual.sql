/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_paystubitem_actual`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `VIEW_paystubitem_actual`(IN `OrganizID` INT, IN `EmpRowID` INT, IN `pay_date_from` DATE, IN `pay_date_to` DATE)
    DETERMINISTIC
BEGIN

DECLARE themonth INT(11);

DECLARE theyear INT(11);

DECLARE startdate_ofpreviousmonth DATE;

DECLARE enddate_ofpreviousmonth DATE;

SELECT pp.`Month`,pp.`Year`
FROM payperiod pp
INNER JOIN employee e ON e.RowID=EmpRowID AND e.OrganizationID=OrganizID
WHERE pp.OrganizationID=OrganizID
AND pp.TotalGrossSalary=e.PayFrequencyID
AND pp.PayFromDate=pay_date_from
AND pp.PayToDate=pay_date_to
INTO themonth
        ,theyear;

SELECT pp.PayFromDate
,pp.PayToDate
FROM payperiod pp
INNER JOIN employee e ON e.RowID=EmpRowID AND e.OrganizationID=OrganizID
WHERE pp.OrganizationID=OrganizID
AND pp.TotalGrossSalary=e.PayFrequencyID
AND pp.`Month`=MONTH(SUBDATE(DATE(CONCAT('2016-',themonth,'-01')), INTERVAL 1 DAY))
AND pp.`Year`=theyear
ORDER BY pp.PayFromDate DESC,pp.PayToDate DESC
LIMIT 1
INTO startdate_ofpreviousmonth
        ,enddate_ofpreviousmonth;



SELECT
psa.RowID
,psa.PayPeriodID
,psa.TotalGrossSalary
,psa.TotalNetSalary
,psa.TotalTaxableSalary
,psa.TotalEmpSSS
,psa.TotalEmpWithholdingTax
,psa.TotalCompSSS
,psa.TotalEmpPhilhealth
,psa.TotalCompPhilhealth
,psa.TotalEmpHDMF
,psa.TotalCompHDMF
,psa.TotalVacationDaysLeft
,psa.TotalLoans
,psa.TotalBonus
,psa.TotalAllowance
,psa.TotalAdjustments
,psa.ThirteenthMonthInclusion
,psa.FirstTimeSalary
,psa.PayPeriodID
,psa.PayFromDate
,psa.PayToDate
,psa.TotalGrossSalary
,psa.TotalNetSalary
,psa.TotalTaxableSalary
,psa.TotalEmpSSS
,psa.TotalEmpWithholdingTax
,psa.TotalCompSSS
,psa.TotalEmpPhilhealth
,psa.TotalCompPhilhealth
,psa.TotalEmpHDMF
,psa.TotalCompHDMF
,psa.TotalVacationDaysLeft
,psa.TotalLoans
,psa.TotalBonus
,psa.TotalAllowance
,psa.TotalAdjustments
,psa.ThirteenthMonthInclusion
,psa.FirstTimeSalary

,es.BasicPay * (es.TrueSalary / es.Salary) AS `BasicPay`
,es.TrueSalary
,IF(e.EmployeeType='Daily',PAYFREQUENCY_DIVISOR(e.EmployeeType),PAYFREQUENCY_DIVISOR(pf.PayFrequencyType)) AS PAYFREQUENCYDIVISOR
,ete.*
,e.EmployeeType
,(e.StartDate BETWEEN pay_date_from AND pay_date_to) AS FirstTimeSalary
,SUM(IFNULL(ete.Leavepayment,0)) `PaidLeaveAmount`
,thirteenthmonthpay.Amount AS 'ThirteenthMonthPay'
FROM paystubactual psa
INNER JOIN employee e ON e.RowID=psa.EmployeeID AND e.OrganizationID=psa.OrganizationID
INNER JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID
INNER JOIN employeesalary es ON es.EmployeeID=psa.EmployeeID AND es.OrganizationID=psa.OrganizationID AND (es.EffectiveDateFrom >= psa.PayFromDate OR IFNULL(es.EffectiveDateTo,CURDATE()) >= psa.PayFromDate) AND (es.EffectiveDateFrom <= psa.PayToDate OR IFNULL(es.EffectiveDateTo,CURDATE()) <= psa.PayToDate)
LEFT JOIN thirteenthmonthpay
    ON thirteenthmonthpay.PaystubID = psa.RowID
LEFT JOIN (SELECT etea.RowID AS eteRowID
                , SUM(etea.RegularHoursWorked) AS RegularHoursWorked
                , SUM(etea.RegularHoursAmount / pr.`PayRate`) AS RegularHoursAmount
                , SUM(etea.TotalHoursWorked) AS TotalHoursWorked
                , SUM(etea.OvertimeHoursWorked) AS OvertimeHoursWorked
                , SUM(etea.OvertimeHoursAmount) AS OvertimeHoursAmount
                , SUM(etea.UndertimeHours) AS UndertimeHours
                , SUM(etea.UndertimeHoursAmount) AS UndertimeHoursAmount
                , SUM(etea.NightDifferentialHours) AS NightDifferentialHours
                , SUM(etea.NightDiffHoursAmount) AS NightDiffHoursAmount
                , SUM(etea.NightDifferentialOTHours) AS NightDifferentialOTHours
                , SUM(etea.NightDiffOTHoursAmount) AS NightDiffOTHoursAmount
                , SUM(etea.HoursLate) AS HoursLate
                , SUM(etea.HoursLateAmount) AS HoursLateAmount
                , SUM(etea.VacationLeaveHours) AS VacationLeaveHours
                , SUM(etea.SickLeaveHours) AS SickLeaveHours
                , SUM(etea.MaternityLeaveHours) AS MaternityLeaveHours
                , SUM(etea.OtherLeaveHours) AS OtherLeaveHours
                , SUM(etea.TotalDayPay) AS TotalDayPay
                , SUM(etea.Absent) AS Absent
                , SUM(etea.Leavepayment) AS Leavepayment
                , IFNULL(i.`PayAmount`,0) `HolidayPayment`
                , SUM(agencyfee.DailyFee) `TotalAgencyFee`
                FROM employeetimeentryactual etea
                INNER JOIN payrate pr ON pr.RowID=etea.PayRateID
                LEFT JOIN agencyfee
                    ON agencyfee.EmployeeID = etea.EmployeeID
                    AND agencyfee.TimeEntryDate = etea.Date
                LEFT JOIN (SELECT SUM(psi.PayAmount) `PayAmount` FROM paystubitem psi INNER JOIN product p ON p.RowID=psi.ProductID AND p.PartNo='Holiday pay' AND psi.Undeclared=1 INNER JOIN paystub ps ON ps.EmployeeID=EmpRowID AND ps.OrganizationID=OrganizID AND ps.PayFromDate=pay_date_from AND ps.PayToDate=pay_date_to AND ps.RowID=psi.PayStubID INNER JOIN employee e ON e.RowID=ps.EmployeeID AND e.OrganizationID=ps.OrganizationID AND e.EmployeeType IN ('Daily','Monthly')) i ON i.PayAmount IS NULL OR i.PayAmount IS NOT NULL

                WHERE etea.EmployeeID=EmpRowID
                AND etea.OrganizationID=OrganizID
                AND etea.`Date` BETWEEN pay_date_from AND pay_date_to) ete ON ete.eteRowID IS NOT NULL
WHERE psa.EmployeeID=EmpRowID
AND psa.OrganizationID=OrganizID
AND psa.PayFromDate=pay_date_from
AND psa.PayToDate=pay_date_to
ORDER BY es.EffectiveDateFrom DESC
LIMIT 1;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
