/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_solopayslip`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `RPT_solopayslip`(IN `OrganizID` INT, IN `PayDate_From` DATE, IN `PayDate_To` DATE, IN `EmployeeRowID` INT(11), IN `IsActual` CHAR(1))
    DETERMINISTIC
BEGIN

SET @daily_rate = GET_employeerateperday(EmployeeRowID,OrganizID,PayDate_To);

SELECT
e.EmployeeID AS `COL1`
,CONCAT_WS(',',e.LastName,e.FirstName,e.MiddleName) AS `COL69`
,ROUND( (es.BasicPay * IF(IsActual=0, 1, (es.TrueSalary / es.Salary))) , 2) AS `COL70`
,0 AS `COL2`
,ROUND( IF(e.EmployeeType IN ('Fixed','Monthly'), (es.BasicPay * IF(IsActual=0, 1, (es.TrueSalary / es.Salary))), IFNULL(ete.RegularHoursAmount,0)) , 2) AS `COL3`
,IFNULL(ete.UndertimeHours,0) AS `COL4`
,IFNULL(ete.Absent,0) AS `COL5`
,IFNULL(ete.HoursLate,0) AS `COL6`
,IFNULL(ete.HoursLateAmount,0) AS `COL7`
,IFNULL(ete.UndertimeHours,0) AS `COL8`
,IFNULL(ete.UndertimeHoursAmount,0) AS `COL9`
,0 AS `COL10`
,0 AS `COL11`
,IFNULL(ete.OvertimeHoursWorked,0) AS `COL12`
,IFNULL(ete.OvertimeHoursAmount,0) AS `COL13`
,IFNULL(ete.NightDifferentialHours,0) AS `COL14`
,IFNULL(ete.NightDiffHoursAmount,0) AS `COL15`
,0 AS `COL16`
,IFNULL(ete.HolidayPayAmount,0) AS `COL17`
,ps.TotalAllowance AS `COL18`
,ps.TotalAdjustments AS `COL19`
,ps.TotalGrossSalary AS `COL20`
,ps.TotalEmpSSS AS `COL21`
,ps.TotalEmpPhilhealth AS `COL22`
,ps.TotalEmpHDMF AS `COL23`
,ps.TotalTaxableSalary AS `COL24`
,ps.TotalEmpWithholdingTax AS `COL25`
,ps.TotalLoans AS `COL26`
,ps.TotalNetSalary AS `COL27`

,REPLACE(GROUP_CONCAT(IFNULL(psiAllwnc.ItemName,'')),',','\n') AS `COL28`
,REPLACE(GROUP_CONCAT(IFNULL(psiAllwnc.PayAmount,0)),',','\n') AS `COL29`

,ps.TotalAllowance AS `COL30`
,REPLACE(GROUP_CONCAT(IFNULL(psiLoan.ItemName,'')),',','\n') AS `COL31`
,REPLACE(GROUP_CONCAT(IFNULL(psiLoan.PayAmount,0)),',','\n') AS `COL32`

,ps.TotalLoans AS `COL33`
,0 AS `COL34`
,0 AS `COL35`

,REPLACE(GROUP_CONCAT(IFNULL(psa.ItemName,'')),',','\n') AS `COL37`
,REPLACE(GROUP_CONCAT(IFNULL(psa.PayAmount,0)),',','\n') AS `COL38`

,(IFNULL(ete.VacationLeaveHours,0) + IFNULL(ete.SickLeaveHours,0) + IFNULL(ete.MaternityLeaveHours,0) + IFNULL(ete.OtherLeaveHours,0)) AS `COL40`
,IFNULL(psiHoli.PayAmount,0) AS `COL41`
FROM (SELECT RowID,OrganizationID,PayPeriodID,EmployeeID,TimeEntryID,PayFromDate,PayToDate,TotalGrossSalary,TotalNetSalary,TotalTaxableSalary,TotalEmpSSS,TotalEmpWithholdingTax,TotalCompSSS,TotalEmpPhilhealth,TotalCompPhilhealth,TotalEmpHDMF,TotalCompHDMF,TotalVacationDaysLeft,TotalLoans,TotalBonus,TotalAllowance,TotalAdjustments,ThirteenthMonthInclusion,FirstTimeSalary FROM paystub WHERE IsActual=0 AND OrganizationID=OrganizID
        UNION
        SELECT RowID,OrganizationID,PayPeriodID,EmployeeID,TimeEntryID,PayFromDate,PayToDate,TotalGrossSalary,TotalNetSalary,TotalTaxableSalary,TotalEmpSSS,TotalEmpWithholdingTax,TotalCompSSS,TotalEmpPhilhealth,TotalCompPhilhealth,TotalEmpHDMF,TotalCompHDMF,TotalVacationDaysLeft,TotalLoans,TotalBonus,TotalAllowance,TotalAdjustments,ThirteenthMonthInclusion,FirstTimeSalary FROM paystubactual WHERE IsActual=1 AND OrganizationID=OrganizID
        ) ps

INNER JOIN employee e ON e.RowID=ps.EmployeeID AND e.OrganizationID=ps.OrganizationID AND FIND_IN_SET(e.EmploymentStatus, UNEMPLOYEMENT_STATUSES()) = 0

INNER JOIN employeesalary es ON es.EmployeeID=ps.EmployeeID AND es.OrganizationID=ps.OrganizationID AND (es.EffectiveDateFrom >= ps.PayFromDate OR IFNULL(es.EffectiveDateTo,ps.PayToDate) >= ps.PayFromDate) AND (es.EffectiveDateFrom <= ps.PayToDate OR IFNULL(es.EffectiveDateTo,ps.PayToDate) <= ps.PayToDate)

LEFT JOIN (SELECT RowID,SUM(RegularHoursWorked) AS RegularHoursWorked,SUM(RegularHoursAmount) AS RegularHoursAmount,SUM(TotalHoursWorked) AS TotalHoursWorked,SUM(OvertimeHoursWorked) AS OvertimeHoursWorked,SUM(OvertimeHoursAmount) AS OvertimeHoursAmount,SUM(UndertimeHours) AS UndertimeHours,SUM(UndertimeHoursAmount) AS UndertimeHoursAmount,SUM(NightDifferentialHours) AS NightDifferentialHours,SUM(NightDiffHoursAmount) AS NightDiffHoursAmount,SUM(NightDifferentialOTHours) AS NightDifferentialOTHours,SUM(NightDiffOTHoursAmount) AS NightDiffOTHoursAmount,SUM(HoursLate) AS HoursLate,SUM(HoursLateAmount) AS HoursLateAmount,SUM(VacationLeaveHours) AS VacationLeaveHours,SUM(SickLeaveHours) AS SickLeaveHours,SUM(MaternityLeaveHours) AS MaternityLeaveHours,SUM(OtherLeaveHours) AS OtherLeaveHours,SUM(TotalDayPay) AS TotalDayPay,SUM(Absent) AS Absent,SUM(TaxableDailyAllowance) AS TaxableDailyAllowance,SUM(HolidayPayAmount) AS HolidayPayAmount,SUM(TaxableDailyBonus) AS TaxableDailyBonus,SUM(NonTaxableDailyBonus) AS NonTaxableDailyBonus,SUM(Leavepayment) AS Leavepayment
                FROM (SELECT RowID,OrganizationID,`Date`,EmployeeShiftID,EmployeeID,EmployeeSalaryID,EmployeeFixedSalaryFlag,RegularHoursWorked,RegularHoursAmount,TotalHoursWorked,OvertimeHoursWorked,OvertimeHoursAmount,UndertimeHours,UndertimeHoursAmount,NightDifferentialHours,NightDiffHoursAmount,NightDifferentialOTHours,NightDiffOTHoursAmount,HoursLate,HoursLateAmount,LateFlag,PayRateID,VacationLeaveHours,SickLeaveHours,MaternityLeaveHours,OtherLeaveHours,TotalDayPay,Absent,ChargeToDivisionID,TaxableDailyAllowance,HolidayPayAmount,TaxableDailyBonus,NonTaxableDailyBonus,Leavepayment FROM employeetimeentry WHERE OrganizationID=OrganizID AND IsActual=0 AND EmployeeID=EmployeeRowID AND `Date` BETWEEN PayDate_From AND PayDate_To
                        UNION
                        SELECT RowID,OrganizationID,`Date`,EmployeeShiftID,EmployeeID,EmployeeSalaryID,EmployeeFixedSalaryFlag,RegularHoursWorked,RegularHoursAmount,TotalHoursWorked,OvertimeHoursWorked,OvertimeHoursAmount,UndertimeHours,UndertimeHoursAmount,NightDifferentialHours,NightDiffHoursAmount,NightDifferentialOTHours,NightDiffOTHoursAmount,HoursLate,HoursLateAmount,LateFlag,PayRateID,VacationLeaveHours,SickLeaveHours,MaternityLeaveHours,OtherLeaveHours,TotalDayPay,Absent,ChargeToDivisionID,TaxableDailyAllowance,HolidayPayAmount,TaxableDailyBonus,NonTaxableDailyBonus,Leavepayment FROM employeetimeentryactual WHERE OrganizationID=OrganizID AND IsActual=1 AND EmployeeID=EmployeeRowID AND `Date` BETWEEN PayDate_From AND PayDate_To
                        ) i
                ) ete ON ete.RowID IS NOT NULL




LEFT JOIN (SELECT psi.*,p.PartNo AS ItemName FROM paystubitem psi INNER JOIN product p ON p.RowID=psi.ProductID AND p.OrganizationID=psi.OrganizationID AND p.`Category`='Allowance Type' WHERE psi.OrganizationID=OrganizID AND psi.PayAmount!=0) psiAllwnc ON psiAllwnc.PayStubID=ps.RowID

LEFT JOIN (SELECT psi.*,p.PartNo AS ItemName FROM paystubitem psi INNER JOIN product p ON p.RowID=psi.ProductID AND p.OrganizationID=psi.OrganizationID AND p.`Category`='Loan Type' WHERE psi.OrganizationID=OrganizID AND psi.PayAmount!=0) psiLoan ON psiLoan.PayStubID=ps.RowID

LEFT JOIN (SELECT psi.*,p.PartNo AS ItemName FROM paystubitem psi INNER JOIN product p ON p.RowID=psi.ProductID AND p.OrganizationID=psi.OrganizationID AND p.PartNo='Holiday pay' WHERE psi.Undeclared=IsActual AND psi.OrganizationID=OrganizID AND psi.PayAmount!=0) psiHoli ON psiHoli.PayStubID=ps.RowID

LEFT JOIN (SELECT psa.RowID,psa.PayStubID,psa.PayAmount,p.PartNo AS ItemName FROM paystubadjustment psa INNER JOIN product p ON p.RowID=psa.ProductID WHERE IsActual=0 AND psa.OrganizationID=OrganizID AND psa.PayAmount!=0
                UNION
                SELECT psa.RowID,psa.PayStubID,psa.PayAmount,p.PartNo AS ItemName FROM paystubadjustmentactual psa INNER JOIN product p ON p.RowID=psa.ProductID WHERE IsActual=1 AND psa.OrganizationID=OrganizID AND psa.PayAmount!=0
                ) psa ON psa.PayStubID=ps.RowID

WHERE IF(EmployeeRowID IS NULL, TRUE, (ps.EmployeeID=EmployeeRowID))
AND ps.OrganizationID=OrganizID
AND ps.PayFromDate=PayDate_From
AND ps.PayToDate=PayDate_To;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
