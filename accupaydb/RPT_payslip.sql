/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_payslip`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_payslip`(IN `og_rowid` INT, IN `pperiod_id` INT, IN `is_actual` BOOL, IN `emp_rowid` INT)
    DETERMINISTIC
BEGIN

DECLARE custom_dateformat VARCHAR(50) DEFAULT '%c/%e/%Y';

DECLARE date_from
        ,date_to
		  ,min_date_thisyear
		  ,max_date_thisyear DATE;

DECLARE is_endofmonth BOOL DEFAULT FALSE;

DECLARE max_dependent INT(11);

DECLARE text_cutoff_ordinal VARCHAR(50);

DECLARE giveAllowanceForHoliday BOOL DEFAULT FALSE;

DECLARE month_per_year
        , default_min_workhour INT(11);

DECLARE leave_transac_rowids TINYTEXT;

SET month_per_year = 12; SET default_min_workhour = 8;



SELECT LeaveTransactionRowIdsWithinCutOff(og_rowid, pperiod_id) INTO leave_transac_rowids;

SELECT
EXISTS(SELECT l.RowID
       FROM listofval l
		 WHERE l.`Type` = 'Payroll Policy' AND
		 l.LIC = 'allowances.holiday'
		 AND l.DisplayValue = '1'
		 LIMIT 1)
INTO giveAllowanceForHoliday;

SELECT MAX(fs.Dependent)
FROM filingstatus fs
INTO max_dependent;

SELECT pp.PayFromDate
,pp.PayToDate
,(pp.Half = 0) `is_endofmonth`
,MIN(pyp.PayToDate)
,MAX(pyp.PayToDate)
,CONCAT_WS('-', pp.`Year`, pp.OrdinalValue)
FROM payperiod pp
INNER JOIN payperiod pyp ON pyp.OrganizationID=pp.OrganizationID AND pyp.`Year`=pp.`Year` AND pyp.TotalGrossSalary=pp.TotalGrossSalary
WHERE pp.RowID=pperiod_id
INTO date_from
     ,date_to
	  ,is_endofmonth
	  ,min_date_thisyear
	  ,max_date_thisyear
	  ,text_cutoff_ordinal;

SELECT ps.RowID
,e.EmployeeID `COL1`
,CONCAT_WS(', ', e.LastName, e.FirstName) `COL69`


              




,esa.Salary `COL70`
,(@basic_sal := esa.BasicPay) `BasicPay`

,(@act_regular := IF(e.EmployeeType = 'Fixed'
                    , @basic_sal
						  , IF(e.EmployeeType = 'Monthly'
                         , (@basic_sal - (ps.LateDeduction + ps.UndertimeDeduction + ps.AbsenceDeduction))
                         , IFNULL(ps.RegularPay, 0)))
						  ) `ActualRegular`
/*,FORMAT(@act_regular, 2) `COL3`
,IFNULL(FORMAT(et.RegularHoursWorked, 2), 0) `COL2`*/
, IF(LCASE(e.EmployeeType)='daily', ps.BasicHours, ps.RegularHours) `COL2`
, ROUND(GetBasicPay(e.RowID,
							ps.PayFromDate,
							ps.PayToDate,
							is_actual,
							ps.BasicHours), 2) `COL3`

#,IFNULL(FORMAT(et.Absent, 2), 0) `COL5`
, ps.AbsentHours `COL4`
, ps.AbsenceDeduction `COL5`

,IFNULL(FORMAT(et.HoursLate, 2), 0) `COL6`
,IFNULL(FORMAT(et.HoursLateAmount, 2), 0) `COL7`

,IFNULL(FORMAT(et.UndertimeHours, 2), 0) `COL8`
,IFNULL(FORMAT(et.UndertimeHoursAmount, 2), 0) `COL9`

,IFNULL(FORMAT(et.RestDayHours, 2), 0) `COL41`
,IFNULL(FORMAT(et.RestDayAmount, 2), 0) `COL42`

,IFNULL(FORMAT(et.NightDifferentialOTHours, 2), 0) `COL43`
,IFNULL(FORMAT(et.NightDiffOTHoursAmount, 2), 0) `COL44`

,IFNULL(FORMAT(et.RestDayOTHours, 2), 0) `COL10`
,IFNULL(FORMAT(et.RestDayOTPay, 2), 0) `COL11`

,IFNULL(FORMAT(et.OvertimeHoursWorked, 2), 0) `COL12`
,IFNULL(FORMAT(et.OvertimeHoursAmount, 2), 0) `COL13`

,IFNULL(FORMAT(et.NightDifferentialHours, 2), 0) `COL14`
,IFNULL(FORMAT(et.NightDiffHoursAmount, 2), 0) `COL15`

,IFNULL(FORMAT(ps.RegularHolidayHours+ps.SpecialHolidayHours, 2), 0) `COL16`
,IFNULL(FORMAT(ps.RegularHolidayPay+ps.SpecialHolidayPay, 2), 0) `COL17`

,(ps.TotalAllowance + ps.TotalBonus) `COL18`
,ps.TotalAdjustments `COL19`

,ps.TotalGrossSalary `COL20`

,ps.TotalEmpSSS `COL21`
,ps.TotalEmpPhilhealth `COL22`
,ps.TotalEmpHDMF `COL23`

,ps.TotalTaxableSalary `COL24`
,ps.TotalEmpWithholdingTax `COL25`

,ps.TotalLoans `COL26`

,ps.TotalNetSalary `COL27`

,CONCAT_WS('\n'
           , IF(plusadj.`AdjustmentName` IS NULL, '', REPLACE(plusadj.`AdjustmentName`, ',', '\n'))
			  , IF(lessadj.`AdjustmentName` IS NULL, '', REPLACE(lessadj.`AdjustmentName`, ',', '\n')))
`COL37`
,CONCAT_WS('\n'
           , IF(plusadj.`PayAmount` IS NULL, '', REPLACE(plusadj.`PayAmount`, ',', '\n'))
			  , IF(lessadj.`PayAmount` IS NULL, '', REPLACE(lessadj.`PayAmount`, ',', '\n')))
`COL38`

,ps.TotalAdjustments `COL39`

,CONCAT_WS('\n'
           , IF(once_allow.`AllowanceNameList` IS NULL, '-', REPLACE(once_allow.`AllowanceNameList`, ',', '\n'))
			  , IF(day_allow.`AllowanceNameList` IS NULL, '-', REPLACE(day_allow.`AllowanceNameList`, ',', '\n'))
			  , IF(semimonth_allow.`AllowanceNameList` IS NULL, '-', REPLACE(semimonth_allow.`AllowanceNameList`, ',', '\n'))) `COL28`
,CONCAT_WS('\n'
           , IF(once_allow.`AllowanceAmountList` IS NULL, '-', REPLACE(once_allow.`AllowanceAmountList`, ',', '\n'))
           , IF(day_allow.`AllowanceAmountList` IS NULL, '-', REPLACE(day_allow.`AllowanceAmountList`, ',', '\n'))
           , IF(semimonth_allow.`AllowanceAmountList` IS NULL, '-', REPLACE(semimonth_allow.`AllowanceAmountList`, ',', '\n'))) `COL29`

,ps.TotalAllowance `COL30`

,IF(slp.`LoanNameList` IS NULL, '', REPLACE(slp.`LoanNameList`, ',', '\n')) `COL31`
,IF(slp.`LoanDeductList` IS NULL, '', REPLACE(slp.`LoanDeductList`, ',', '\n')) `COL32`
,IF(slp.`LoanBalance` IS NULL, '', REPLACE(slp.`LoanBalance`, ',', '\n')) `COL40`
,IFNULL(slp.`TotalLoanBal`, 0) `COL50`


,ps.TotalLoans `COL33`

,IF(lt.`LeaveTypes` IS NULL, '', REPLACE(lt.`LeaveTypes`, ',', '\n')) `COL34`
,IF(lt.`BalanceLeave` IS NULL, '', REPLACE(lt.`BalanceLeave`, ',', '\n')) `COL35`

, ps.LeaveHours `COL80`
, ps.LeavePay `COL81`

, e.RowID `EmployeeRowID`

FROM paystub ps

INNER JOIN employee e
        ON e.RowID=ps.EmployeeID
		     AND e.OrganizationID=ps.OrganizationID
		     AND FIND_IN_SET(e.EmploymentStatus, UNEMPLOYEMENT_STATUSES()) = 0
INNER JOIN `position` pos
        ON pos.RowID=e.PositionID
		     AND pos.OrganizationID=e.OrganizationID
INNER JOIN division dv
        ON dv.RowID=pos.DivisionId

INNER JOIN filingstatus fs
        ON fs.MaritalStatus=e.MaritalStatus
           AND fs.Dependent=IF(max_dependent < e.NoOfDependents, max_dependent, e.NoOfDependents)

INNER JOIN paystubactual psa
        ON psa.RowID=ps.RowID

LEFT JOIN (SELECT ete.EmployeeID
           ,SUM(ete.RegularHoursWorked) `RegularHoursWorked`
           ,SUM(ete.RegularHoursAmount) `RegularHoursAmount`
			  ,SUM(ete.HoursLate) `HoursLate`
			  ,SUM(ete.HoursLateAmount) `HoursLateAmount`
			  ,SUM(ete.UndertimeHours) `UndertimeHours`
			  ,SUM(ete.UndertimeHoursAmount) `UndertimeHoursAmount`
			  ,SUM(ete.OvertimeHoursWorked) `OvertimeHoursWorked`
			  ,SUM(ete.OvertimeHoursAmount) `OvertimeHoursAmount`
			  ,SUM(ete.NightDifferentialHours) `NightDifferentialHours`
           ,SUM(ete.NightDiffHoursAmount) `NightDiffHoursAmount`
           ,SUM(ete.HolidayPayAmount) `HolidayPayAmount`
           ,SUM(ete.Absent) `Absent`
           
			  ,SUM(ete.RestDayHours) `RestDayHours`
			  ,SUM(ete.RestDayAmount) `RestDayAmount`
			  ,SUM(ete.RestDayOTHours) `RestDayOTHours`
			  ,SUM(ete.RestDayOTPay) `RestDayOTPay`
			  
			  ,SUM(ete.NightDifferentialOTHours) `NightDifferentialOTHours`
			  ,SUM(ete.NightDiffOTHoursAmount) `NightDiffOTHoursAmount`
           FROM v_uni_employeetimeentry ete
           WHERE ete.OrganizationID = og_rowid
           AND ete.`Date` BETWEEN date_from AND date_to
           AND ete.AsActual = FALSE
			  GROUP BY ete.EmployeeID
           ) et
       ON et.EmployeeID=ps.EmployeeID

INNER JOIN employeesalary esa
        ON esa.RowID = (SELECT esa.RowID 
		                  FROM employeesalary esa
								WHERE esa.EmployeeID=ps.EmployeeID
								AND esa.OrganizationID=og_rowid
								AND (esa.EffectiveDateFrom >= date_from OR IFNULL(esa.EffectiveDateTo, max_date_thisyear) >= date_from)
								AND (esa.EffectiveDateFrom <= date_to OR IFNULL(esa.EffectiveDateTo, max_date_thisyear) <= date_to)
								ORDER BY esa.EffectiveDateFrom DESC
								LIMIT 1)

LEFT JOIN (SELECT slp.*
           ,GROUP_CONCAT(CONCAT('  ', p.PartNo)) `LoanNameList`
           ,GROUP_CONCAT(ROUND(slp.DeductionAmount, 2)) `LoanDeductList`
           ,GROUP_CONCAT(ROUND(slp.TotalBalanceLeft, 2)) `LoanBalance`
           ,ROUND(SUM(slp.TotalBalanceLeft), 2) `TotalLoanBal`
           
           FROM scheduledloansperpayperiod slp
           INNER JOIN employeeloanschedule els
                   ON els.RowID=slp.EmployeeLoanRecordID
           INNER JOIN product p
                   ON p.RowID=els.LoanTypeID
			  WHERE slp.OrganizationID=og_rowid
			  AND slp.PayPeriodID=pperiod_id
			  GROUP BY slp.EmployeeID) slp
       ON slp.EmployeeID=ps.EmployeeID

LEFT JOIN (SELECT padj.RowID
           ,padj.PayStubID
           ,GROUP_CONCAT(padj.AdjustmentName) `AdjustmentName`
           ,GROUP_CONCAT(ROUND(padj.PayAmount, 2)) `PayAmount`
           FROM paystubadjustwithproductname padj
           INNER JOIN paystub ps ON ps.RowID=padj.PayStubID AND ps.PayPeriodID=pperiod_id
           WHERE padj.PayAmount > 0
			  AND padj.OrganizationID=og_rowid
			  GROUP BY padj.PayStubID) plusadj
       ON plusadj.PayStubID=ps.RowID

LEFT JOIN (SELECT padj.RowID
           ,padj.PayStubID
           ,GROUP_CONCAT(padj.AdjustmentName) `AdjustmentName`
           ,GROUP_CONCAT(ROUND(padj.PayAmount, 2)) `PayAmount`
           ,SUM(ROUND(padj.PayAmount, 2)) `TotalNegativeAdjustment`
           FROM paystubadjustwithproductname padj
           INNER JOIN paystub ps ON ps.RowID=padj.PayStubID AND ps.PayPeriodID=pperiod_id
           WHERE padj.PayAmount < 0
			  AND padj.OrganizationID=og_rowid
			  GROUP BY padj.PayStubID) lessadj
       ON lessadj.PayStubID=ps.RowID

LEFT JOIN (SELECT etlv.RowID
           ,etlv.EmployeeID
           ,SUM(etlv.VacationLeaveHours) `VacationLeaveHours`
			  ,SUM(etlv.SickLeaveHours) `SickLeaveHours`
			  ,SUM(etlv.MaternityLeaveHours) `MaternityLeaveHours`
			  ,SUM(etlv.OtherLeaveHours) `OtherLeaveHours`
           FROM employeetimeentry etlv
			  WHERE etlv.OrganizationID=og_rowid
			  AND (etlv.VacationLeaveHours + etlv.SickLeaveHours + etlv.MaternityLeaveHours + etlv.OtherLeaveHours) > 0
			  AND etlv.`Date` BETWEEN min_date_thisyear AND date_to
			  GROUP BY etlv.EmployeeID) etlv
	    ON etlv.EmployeeID=ps.EmployeeID

LEFT JOIN (SELECT ea.*
           ,GROUP_CONCAT( ea.AllowanceAmount ) `AllowanceAmountList`
           
           ,GROUP_CONCAT( p.PartNo ) `AllowanceNameList`
           FROM employeeallowance ea
           INNER JOIN product p ON p.RowID=ea.ProductID
			  WHERE ea.OrganizationID=og_rowid
			  AND ea.AllowanceAmount != 0
			  AND ea.AllowanceFrequency='One time'
			  AND ea.EffectiveStartDate BETWEEN date_from AND date_to
			  GROUP BY ea.EmployeeID, ea.RowID) once_allow
       ON once_allow.EmployeeID=ps.EmployeeID


LEFT JOIN (
           SELECT ea.*
		     ,GROUP_CONCAT( ea.`TheAmount` ) `AllowanceAmountList`
		     ,GROUP_CONCAT( ea.PartNo ) `AllowanceNameList`
		FROM (
		      SELECT ai.AllowanceID `RowID`
				, ea.EmployeeID
				, ROUND(ai.Amount, 2) `TheAmount`
				, p.PartNo
				FROM allowanceitem ai
				INNER JOIN employeeallowance ea ON ea.RowID=ai.AllowanceID AND ea.AllowanceFrequency='Daily'
				INNER JOIN product p ON p.RowID=ea.ProductID
				WHERE ai.PayPeriodID = pperiod_id
				AND ai.OrganizationID = og_rowid
				AND ai.Amount > 0
		      ) ea
		 GROUP BY ea.EmployeeID, ea.RowID
			   ) day_allow
       ON day_allow.EmployeeID=ps.EmployeeID

LEFT JOIN (
           SELECT ea.*
		     ,GROUP_CONCAT( ea.`TheAmount` ) `AllowanceAmountList`
		     ,GROUP_CONCAT( ea.PartNo ) `AllowanceNameList`
		FROM (
		     SELECT ea.eaRowID `RowID`, ea.EmployeeID
		     
		     
           , ROUND( ( ea.AllowanceAmount - (SUM(ea.HoursToLess) * ((ea.AllowanceAmount / (ea.WorkDaysPerYear / (ea.PAYFREQDIV * month_per_year))) / default_min_workhour)) + IF(giveAllowanceForHoliday, SUM(ea.HolidayAllowance), 0) ), 2) `TheAmount`
           
		     , p.PartNo
		     FROM paystubitem_sum_semimon_allowance_group_prodid ea
		     INNER JOIN employee e ON e.RowID=ea.EmployeeID AND FIND_IN_SET(e.EmploymentStatus, UNEMPLOYEMENT_STATUSES()) = 0
		     INNER JOIN product p ON p.RowID=ea.ProductID
			  WHERE ea.OrganizationID=og_rowid
			  AND ea.`Date` BETWEEN date_from AND date_to
			  GROUP BY ea.eaRowID
		     ) ea
		GROUP BY ea.EmployeeID, ea.RowID
			  ) semimonth_allow
       ON semimonth_allow.EmployeeID=ps.EmployeeID

INNER JOIN (SELECT ps.RowID
            ,ps.EmployeeID
            ,SUM(ps.TotalTaxableSalary) `TotalTaxableSalary`
            ,SUM(ps.TotalEmpWithholdingTax) `TotalEmpWithholdingTax`
            ,SUM(ps.TotalEmpSSS) `TotalEmpSSS`
            ,SUM(ps.TotalEmpPhilhealth) `TotalEmpPhilhealth`
            ,SUM(ps.TotalEmpHDMF) `TotalEmpHDMF`
            FROM paystubactual ps
            WHERE ps.OrganizationID=og_rowid
            AND (ps.PayFromDate >= min_date_thisyear AND ps.PayToDate <= max_date_thisyear)
            GROUP BY ps.EmployeeID
            ) pstub
        ON pstub.EmployeeID=ps.EmployeeID

LEFT JOIN (SELECT lt.*
			  , GROUP_CONCAT(p.PartNo) `LeaveTypes`
           , GROUP_CONCAT(lt.Balance) `BalanceLeave`
			  FROM leavetransaction lt
			  INNER JOIN leaveledger ll ON ll.LastTransactionID=lt.RowID
			  INNER JOIN product p ON p.RowID=ll.ProductID
			  WHERE FIND_IN_SET(lt.RowID, leave_transac_rowids) > 0
			  AND lt.OrganizationID = og_rowid
			  AND lt.Balance > -1
			  GROUP BY lt.EmployeeID
           ) lt ON lt.EmployeeID = e.RowID

WHERE ps.OrganizationID=og_rowid
		AND ps.PayPeriodID=pperiod_id

GROUP BY ps.RowID
ORDER BY CONCAT(e.LastName, e.FirstName)
;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
