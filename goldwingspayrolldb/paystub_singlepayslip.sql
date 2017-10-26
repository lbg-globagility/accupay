/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `paystub_singlepayslip`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `paystub_singlepayslip`(
    IN `OrganizID` INT,
    IN `PayPeriodRowID` INT,
    IN `IsActualFlag` CHAR(1),
    IN `EmployeeRowID` INT




)
    DETERMINISTIC
BEGIN

DECLARE paydate_from DATE;

DECLARE paydat_to DATE;

SELECT pp.PayFromDate,pp.PayToDate FROM payperiod pp WHERE pp.RowID=PayPeriodRowID INTO paydate_from,paydat_to;


SET @daily_rate = GET_employeerateperday(9,2,paydat_to);



SELECT i.*
FROM (SELECT
e.RowID,e.EmployeeID AS `COL1`
,CONCAT_WS(', ',e.LastName,e.FirstName,e.MiddleName) AS `COL69`

,IF(IsActualFlag=0, ete.RegularHoursAmount, ROUND(ete.RegularHoursAmount, 2)) AS `COL70`
,IF(IsActualFlag = 1, es.TrueSalary, es.Salary) `COL80`
,0 AS `COL2`
,ROUND( IF(e.EmployeeType IN ('Fixed','Monthly'), (es.BasicPay * IF(IsActualFlag=0, 1, (es.TrueSalary / es.Salary))), IFNULL(ete.RegularHoursAmount,0)) , 2) AS `COL3`
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
,(ps.TotalAllowance - IFNULL(psiECOLA.PayAmount,0)) AS `COL18`
,ps.TotalAdjustments `COL19`
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
,REPLACE(GROUP_CONCAT(IFNULL(psiLoan.PayAmount,'')),',','\n') AS `COL32`
,REPLACE(GROUP_CONCAT(IFNULL(slp.TotBalLeft,'')),',','\n') AS `COL36`

,ps.TotalLoans AS `COL33`
,0 AS `COL34`
,0 AS `COL35`

,REPLACE(GROUP_CONCAT(IFNULL(psa.ItemName,'')),',','\n') AS `COL37`
,REPLACE(GROUP_CONCAT(IFNULL(psa.PayAmount,0)),',','\n') AS `COL38`

,(IFNULL(ete.VacationLeaveHours,0) + IFNULL(ete.SickLeaveHours,0) + IFNULL(ete.MaternityLeaveHours,0) + IFNULL(ete.OtherLeaveHours,0)) AS `COL40`
,IFNULL(ete.Leavepayment,0) AS `COL41`

,IFNULL(psiHoli.PayAmount,0) AS `COL42`

,IFNULL(psiECOLA.PayAmount,0) AS `COL43`

,REPLACE(GROUP_CONCAT(IFNULL(psiLeave.ItemName,'')),',','\n') AS `COL44`


FROM (SELECT RowID,OrganizationID,PayPeriodID,EmployeeID,TimeEntryID,PayFromDate,PayToDate,TotalGrossSalary,TotalNetSalary,TotalTaxableSalary,TotalEmpSSS,TotalEmpWithholdingTax,TotalCompSSS,TotalEmpPhilhealth,TotalCompPhilhealth,TotalEmpHDMF,TotalCompHDMF,TotalVacationDaysLeft,TotalLoans,TotalBonus,TotalAllowance,TotalAdjustments,ThirteenthMonthInclusion,FirstTimeSalary FROM paystub WHERE IsActualFlag=0 AND OrganizationID=OrganizID
        UNION
        SELECT RowID,OrganizationID,PayPeriodID,EmployeeID,TimeEntryID,PayFromDate,PayToDate,TotalGrossSalary,TotalNetSalary,TotalTaxableSalary,TotalEmpSSS,TotalEmpWithholdingTax,TotalCompSSS,TotalEmpPhilhealth,TotalCompPhilhealth,TotalEmpHDMF,TotalCompHDMF,TotalVacationDaysLeft,TotalLoans,TotalBonus,TotalAllowance,TotalAdjustments,ThirteenthMonthInclusion,FirstTimeSalary FROM paystubactual WHERE IsActualFlag=1 AND OrganizationID=OrganizID
        ) ps

INNER JOIN employee e ON e.RowID=ps.EmployeeID AND e.OrganizationID=ps.OrganizationID AND e.EmployeeType='Daily'

INNER JOIN employeesalary es ON es.EmployeeID=ps.EmployeeID AND es.OrganizationID=ps.OrganizationID AND (es.EffectiveDateFrom >= ps.PayFromDate OR IFNULL(es.EffectiveDateTo,ps.PayToDate) >= ps.PayFromDate) AND (es.EffectiveDateFrom <= ps.PayToDate OR IFNULL(es.EffectiveDateTo,ps.PayToDate) <= ps.PayToDate)

LEFT JOIN (SELECT RowID,EmployeeID,SUM(RegularHoursWorked) AS RegularHoursWorked,SUM(RegularHoursAmount) AS RegularHoursAmount,SUM(TotalHoursWorked) AS TotalHoursWorked,SUM(OvertimeHoursWorked) AS OvertimeHoursWorked,SUM(OvertimeHoursAmount) AS OvertimeHoursAmount,SUM(UndertimeHours) AS UndertimeHours,SUM(UndertimeHoursAmount) AS UndertimeHoursAmount,SUM(NightDifferentialHours) AS NightDifferentialHours,SUM(NightDiffHoursAmount) AS NightDiffHoursAmount,SUM(NightDifferentialOTHours) AS NightDifferentialOTHours,SUM(NightDiffOTHoursAmount) AS NightDiffOTHoursAmount,SUM(HoursLate) AS HoursLate,SUM(HoursLateAmount) AS HoursLateAmount,SUM(VacationLeaveHours) AS VacationLeaveHours,SUM(SickLeaveHours) AS SickLeaveHours,SUM(MaternityLeaveHours) AS MaternityLeaveHours,SUM(OtherLeaveHours) AS OtherLeaveHours,SUM(TotalDayPay) AS TotalDayPay,SUM(Absent) AS Absent,SUM(TaxableDailyAllowance) AS TaxableDailyAllowance,SUM(HolidayPayAmount) AS HolidayPayAmount,SUM(TaxableDailyBonus) AS TaxableDailyBonus,SUM(NonTaxableDailyBonus) AS NonTaxableDailyBonus,SUM(Leavepayment) AS Leavepayment
                FROM (SELECT RowID,OrganizationID,`Date`,EmployeeShiftID,EmployeeID,EmployeeSalaryID,EmployeeFixedSalaryFlag,RegularHoursWorked,RegularHoursAmount,TotalHoursWorked,OvertimeHoursWorked,OvertimeHoursAmount,UndertimeHours,UndertimeHoursAmount,NightDifferentialHours,NightDiffHoursAmount,NightDifferentialOTHours,NightDiffOTHoursAmount,HoursLate,HoursLateAmount,LateFlag,PayRateID,VacationLeaveHours,SickLeaveHours,MaternityLeaveHours,OtherLeaveHours,TotalDayPay,Absent,ChargeToDivisionID,TaxableDailyAllowance,HolidayPayAmount,TaxableDailyBonus,NonTaxableDailyBonus,Leavepayment FROM employeetimeentry WHERE OrganizationID=OrganizID AND IsActualFlag=0 AND `Date` BETWEEN paydate_from AND paydat_to
                        UNION
                        SELECT RowID,OrganizationID,`Date`,EmployeeShiftID,EmployeeID,EmployeeSalaryID,EmployeeFixedSalaryFlag,RegularHoursWorked,RegularHoursAmount,TotalHoursWorked,OvertimeHoursWorked,OvertimeHoursAmount,UndertimeHours,UndertimeHoursAmount,NightDifferentialHours,NightDiffHoursAmount,NightDifferentialOTHours,NightDiffOTHoursAmount,HoursLate,HoursLateAmount,LateFlag,PayRateID,VacationLeaveHours,SickLeaveHours,MaternityLeaveHours,OtherLeaveHours,TotalDayPay,Absent,ChargeToDivisionID,TaxableDailyAllowance,HolidayPayAmount,TaxableDailyBonus,NonTaxableDailyBonus,Leavepayment FROM employeetimeentryactual WHERE OrganizationID=OrganizID AND IsActualFlag=1 AND `Date` BETWEEN paydate_from AND paydat_to
                        ) i GROUP BY i.EmployeeID
                ) ete ON ete.RowID IS NOT NULL AND ete.EmployeeID=ps.EmployeeID




LEFT JOIN (SELECT psi.*,p.PartNo AS ItemName FROM paystubitem psi INNER JOIN product p ON p.RowID=psi.ProductID AND p.OrganizationID=psi.OrganizationID AND p.`Category`='Allowance Type' WHERE psi.OrganizationID=OrganizID AND psi.PayAmount!=0) psiAllwnc ON psiAllwnc.PayStubID=ps.RowID

LEFT JOIN (SELECT psi.*,p.PartNo AS ItemName FROM paystubitem psi INNER JOIN product p ON p.RowID=psi.ProductID AND p.OrganizationID=psi.OrganizationID AND p.`Category`='Loan Type' WHERE psi.OrganizationID=OrganizID AND psi.PayAmount!=0) psiLoan ON psiLoan.PayStubID=ps.RowID

LEFT JOIN (SELECT a.*,b.LoanTypeID,ROUND(a.TotalBalanceLeft,2) `TotBalLeft` FROM scheduledloansperpayperiod a INNER JOIN employeeloanschedule b ON b.RowID=a.EmployeeLoanRecordID) slp ON slp.OrganizationID=ps.OrganizationID AND slp.PayPeriodID=ps.PayPeriodID AND slp.EmployeeID=ps.EmployeeID AND slp.LoanTypeID=psiLoan.ProductID AND psiLoan.ProductID IS NOT NULL

LEFT JOIN (SELECT psi.*,p.PartNo AS ItemName FROM paystubitem psi INNER JOIN product p ON p.RowID=psi.ProductID AND p.OrganizationID=psi.OrganizationID AND p.`Category`='Leave Type' WHERE psi.OrganizationID=OrganizID AND psi.PayAmount!=0) psiLeave ON psiLeave.PayStubID=ps.RowID

LEFT JOIN (SELECT psi.*,p.PartNo AS ItemName FROM paystubitem psi INNER JOIN product p ON p.RowID=psi.ProductID AND p.OrganizationID=psi.OrganizationID AND p.PartNo='Holiday pay' WHERE psi.Undeclared=IsActualFlag AND psi.OrganizationID=OrganizID AND psi.PayAmount!=0) psiHoli ON psiHoli.PayStubID=ps.RowID

LEFT JOIN (SELECT psi.*,p.PartNo AS ItemName FROM paystubitem psi INNER JOIN product p ON p.RowID=psi.ProductID AND p.OrganizationID=psi.OrganizationID AND p.PartNo='Ecola' WHERE psi.Undeclared=0 AND psi.OrganizationID=OrganizID AND psi.PayAmount!=0) psiECOLA ON psiECOLA.PayStubID=ps.RowID

LEFT JOIN (SELECT psa.RowID,psa.PayStubID,psa.PayAmount,p.PartNo AS ItemName FROM paystubadjustment psa INNER JOIN product p ON p.RowID=psa.ProductID WHERE IsActualFlag=0 AND psa.OrganizationID=OrganizID AND psa.PayAmount!=0
                UNION
                SELECT psa.RowID,psa.PayStubID,psa.PayAmount,p.PartNo AS ItemName FROM paystubadjustmentactual psa INNER JOIN product p ON p.RowID=psa.ProductID WHERE IsActualFlag=1 AND psa.OrganizationID=OrganizID AND psa.PayAmount!=0
                ) psa ON psa.PayStubID=ps.RowID

WHERE ps.OrganizationID=OrganizID
AND ps.PayFromDate=paydate_from
AND ps.PayToDate=paydat_to AND ps.EmployeeID=EmployeeRowID
GROUP BY ps.EmployeeID) i

UNION



SELECT i.*
FROM (SELECT
e.RowID,e.EmployeeID AS `COL1`
,CONCAT_WS(', ',e.LastName,e.FirstName,e.MiddleName) AS `COL69`

,IF(IsActualFlag=0, ete.RegularHoursAmount, ROUND(ete.RegularHoursAmount, 2)) AS `COL70`
,ROUND( es.BasicPay * IF(IsActualFlag=0, 1, (es.TrueSalary / es.Salary)) , 2) `COL80`
,0 AS `COL2`
,ROUND( IF(e.EmployeeType IN ('Fixed','Monthly'), (es.BasicPay * IF(IsActualFlag=0, 1, (es.TrueSalary / es.Salary))), IFNULL(ete.RegularHoursAmount,0)) , 2) AS `COL3`
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
,(ps.TotalAllowance - IFNULL(psiECOLA.PayAmount,0)) AS `COL18`
,ps.TotalAdjustments `COL19`
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
,REPLACE(GROUP_CONCAT(IFNULL(psiLoan.PayAmount,'')),',','\n') AS `COL32`
,REPLACE(GROUP_CONCAT(IFNULL(slp.TotBalLeft,'')),',','\n') AS `COL36`

,ps.TotalLoans AS `COL33`
,0 AS `COL34`
,0 AS `COL35`

,REPLACE(GROUP_CONCAT(IFNULL(psa.ItemName,'')),',','\n') AS `COL37`
,REPLACE(GROUP_CONCAT(IFNULL(psa.PayAmount,0)),',','\n') AS `COL38`

,(IFNULL(ete.VacationLeaveHours,0) + IFNULL(ete.SickLeaveHours,0) + IFNULL(ete.MaternityLeaveHours,0) + IFNULL(ete.OtherLeaveHours,0)) AS `COL40`
,IFNULL(ete.Leavepayment,0) AS `COL41`

,IFNULL(psiHoli.PayAmount,0) AS `COL42`

,IFNULL(psiECOLA.PayAmount,0) AS `COL43`

,REPLACE(GROUP_CONCAT(IFNULL(psiLeave.ItemName,'')),',','\n') AS `COL44`


FROM (SELECT RowID,OrganizationID,PayPeriodID,EmployeeID,TimeEntryID,PayFromDate,PayToDate,TotalGrossSalary,TotalNetSalary,TotalTaxableSalary,TotalEmpSSS,TotalEmpWithholdingTax,TotalCompSSS,TotalEmpPhilhealth,TotalCompPhilhealth,TotalEmpHDMF,TotalCompHDMF,TotalVacationDaysLeft,TotalLoans,TotalBonus,TotalAllowance,TotalAdjustments,ThirteenthMonthInclusion,FirstTimeSalary FROM paystub WHERE IsActualFlag=0 AND OrganizationID=OrganizID
        UNION
        SELECT RowID,OrganizationID,PayPeriodID,EmployeeID,TimeEntryID,PayFromDate,PayToDate,TotalGrossSalary,TotalNetSalary,TotalTaxableSalary,TotalEmpSSS,TotalEmpWithholdingTax,TotalCompSSS,TotalEmpPhilhealth,TotalCompPhilhealth,TotalEmpHDMF,TotalCompHDMF,TotalVacationDaysLeft,TotalLoans,TotalBonus,TotalAllowance,TotalAdjustments,ThirteenthMonthInclusion,FirstTimeSalary FROM paystubactual WHERE IsActualFlag=1 AND OrganizationID=OrganizID
        ) ps

INNER JOIN employee e ON e.RowID=ps.EmployeeID AND e.OrganizationID=ps.OrganizationID AND e.EmployeeType='Monthly'

INNER JOIN employeesalary es ON es.EmployeeID=ps.EmployeeID AND es.OrganizationID=ps.OrganizationID AND (es.EffectiveDateFrom >= ps.PayFromDate OR IFNULL(es.EffectiveDateTo,ps.PayToDate) >= ps.PayFromDate) AND (es.EffectiveDateFrom <= ps.PayToDate OR IFNULL(es.EffectiveDateTo,ps.PayToDate) <= ps.PayToDate)

LEFT JOIN (SELECT RowID,EmployeeID,SUM(RegularHoursWorked) AS RegularHoursWorked,SUM(RegularHoursAmount) AS RegularHoursAmount,SUM(TotalHoursWorked) AS TotalHoursWorked,SUM(OvertimeHoursWorked) AS OvertimeHoursWorked,SUM(OvertimeHoursAmount) AS OvertimeHoursAmount,SUM(UndertimeHours) AS UndertimeHours,SUM(UndertimeHoursAmount) AS UndertimeHoursAmount,SUM(NightDifferentialHours) AS NightDifferentialHours,SUM(NightDiffHoursAmount) AS NightDiffHoursAmount,SUM(NightDifferentialOTHours) AS NightDifferentialOTHours,SUM(NightDiffOTHoursAmount) AS NightDiffOTHoursAmount,SUM(HoursLate) AS HoursLate,SUM(HoursLateAmount) AS HoursLateAmount,SUM(VacationLeaveHours) AS VacationLeaveHours,SUM(SickLeaveHours) AS SickLeaveHours,SUM(MaternityLeaveHours) AS MaternityLeaveHours,SUM(OtherLeaveHours) AS OtherLeaveHours,SUM(TotalDayPay) AS TotalDayPay,SUM(Absent) AS Absent,SUM(TaxableDailyAllowance) AS TaxableDailyAllowance,SUM(HolidayPayAmount) AS HolidayPayAmount,SUM(TaxableDailyBonus) AS TaxableDailyBonus,SUM(NonTaxableDailyBonus) AS NonTaxableDailyBonus,SUM(Leavepayment) AS Leavepayment
                FROM (SELECT RowID,OrganizationID,`Date`,EmployeeShiftID,EmployeeID,EmployeeSalaryID,EmployeeFixedSalaryFlag,RegularHoursWorked,RegularHoursAmount,TotalHoursWorked,OvertimeHoursWorked,OvertimeHoursAmount,UndertimeHours,UndertimeHoursAmount,NightDifferentialHours,NightDiffHoursAmount,NightDifferentialOTHours,NightDiffOTHoursAmount,HoursLate,HoursLateAmount,LateFlag,PayRateID,VacationLeaveHours,SickLeaveHours,MaternityLeaveHours,OtherLeaveHours,TotalDayPay,Absent,ChargeToDivisionID,TaxableDailyAllowance,HolidayPayAmount,TaxableDailyBonus,NonTaxableDailyBonus,Leavepayment FROM employeetimeentry WHERE OrganizationID=OrganizID AND IsActualFlag=0 AND `Date` BETWEEN paydate_from AND paydat_to
                        UNION
                        SELECT RowID,OrganizationID,`Date`,EmployeeShiftID,EmployeeID,EmployeeSalaryID,EmployeeFixedSalaryFlag,RegularHoursWorked,RegularHoursAmount,TotalHoursWorked,OvertimeHoursWorked,OvertimeHoursAmount,UndertimeHours,UndertimeHoursAmount,NightDifferentialHours,NightDiffHoursAmount,NightDifferentialOTHours,NightDiffOTHoursAmount,HoursLate,HoursLateAmount,LateFlag,PayRateID,VacationLeaveHours,SickLeaveHours,MaternityLeaveHours,OtherLeaveHours,TotalDayPay,Absent,ChargeToDivisionID,TaxableDailyAllowance,HolidayPayAmount,TaxableDailyBonus,NonTaxableDailyBonus,Leavepayment FROM employeetimeentryactual WHERE OrganizationID=OrganizID AND IsActualFlag=1 AND `Date` BETWEEN paydate_from AND paydat_to
                        ) i GROUP BY i.EmployeeID
                ) ete ON ete.RowID IS NOT NULL AND ete.EmployeeID=ps.EmployeeID




LEFT JOIN (SELECT psi.*,p.PartNo AS ItemName FROM paystubitem psi INNER JOIN product p ON p.RowID=psi.ProductID AND p.OrganizationID=psi.OrganizationID AND p.`Category`='Allowance Type' WHERE psi.OrganizationID=OrganizID AND psi.PayAmount!=0) psiAllwnc ON psiAllwnc.PayStubID=ps.RowID

LEFT JOIN (SELECT psi.*,p.PartNo AS ItemName FROM paystubitem psi INNER JOIN product p ON p.RowID=psi.ProductID AND p.OrganizationID=psi.OrganizationID AND p.`Category`='Loan Type' WHERE psi.OrganizationID=OrganizID AND psi.PayAmount!=0) psiLoan ON psiLoan.PayStubID=ps.RowID

LEFT JOIN (SELECT a.*,b.LoanTypeID,ROUND(a.TotalBalanceLeft,2) `TotBalLeft` FROM scheduledloansperpayperiod a INNER JOIN employeeloanschedule b ON b.RowID=a.EmployeeLoanRecordID) slp ON slp.OrganizationID=ps.OrganizationID AND slp.PayPeriodID=ps.PayPeriodID AND slp.EmployeeID=ps.EmployeeID AND slp.LoanTypeID=psiLoan.ProductID AND psiLoan.ProductID IS NOT NULL

LEFT JOIN (SELECT psi.*,p.PartNo AS ItemName FROM paystubitem psi INNER JOIN product p ON p.RowID=psi.ProductID AND p.OrganizationID=psi.OrganizationID AND p.`Category`='Leave Type' WHERE psi.OrganizationID=OrganizID AND psi.PayAmount!=0) psiLeave ON psiLeave.PayStubID=ps.RowID

LEFT JOIN (SELECT psi.*,p.PartNo AS ItemName FROM paystubitem psi INNER JOIN product p ON p.RowID=psi.ProductID AND p.OrganizationID=psi.OrganizationID AND p.PartNo='Holiday pay' WHERE psi.Undeclared=IsActualFlag AND psi.OrganizationID=OrganizID AND psi.PayAmount!=0) psiHoli ON psiHoli.PayStubID=ps.RowID

LEFT JOIN (SELECT psi.*,p.PartNo AS ItemName FROM paystubitem psi INNER JOIN product p ON p.RowID=psi.ProductID AND p.OrganizationID=psi.OrganizationID AND p.PartNo='Ecola' WHERE psi.Undeclared=0 AND psi.OrganizationID=OrganizID AND psi.PayAmount!=0) psiECOLA ON psiECOLA.PayStubID=ps.RowID

LEFT JOIN (SELECT psa.RowID,psa.PayStubID,psa.PayAmount,p.PartNo AS ItemName FROM paystubadjustment psa INNER JOIN product p ON p.RowID=psa.ProductID WHERE IsActualFlag=0 AND psa.OrganizationID=OrganizID AND psa.PayAmount!=0
                UNION
                SELECT psa.RowID,psa.PayStubID,psa.PayAmount,p.PartNo AS ItemName FROM paystubadjustmentactual psa INNER JOIN product p ON p.RowID=psa.ProductID WHERE IsActualFlag=1 AND psa.OrganizationID=OrganizID AND psa.PayAmount!=0
                ) psa ON psa.PayStubID=ps.RowID

WHERE ps.OrganizationID=OrganizID
AND ps.PayFromDate=paydate_from
AND ps.PayToDate=paydat_to AND ps.EmployeeID=EmployeeRowID
GROUP BY ps.EmployeeID) i;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
