-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.12-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for procedure HyundaiPayslip
DROP PROCEDURE IF EXISTS `HyundaiPayslip`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `HyundaiPayslip`(IN `og_rowid` INT, IN `pperiod_id` INT, IN `is_actual` BOOL, IN `emp_rowid` INT)
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
,CONCAT_WS(', ', e.LastName, e.FirstName) `COL2`

,CONCAT_WS('-', DATE_FORMAT(ADDDATE(date_from, INTERVAL 5 DAY), custom_dateformat)
              , DATE_FORMAT(IF(is_endofmonth = TRUE
				                   , LAST_DAY(date_to)
										 , ADDDATE(date_to, INTERVAL 5 DAY)), custom_dateformat)) `COL3`
              
,CONCAT_WS('-', DATE_FORMAT(date_from, custom_dateformat)
              , DATE_FORMAT(date_to, custom_dateformat)) `COL4`

,fs.`FilingStatus` `COL5`

,(@basic_sal := esa.BasicPay) `EmployeeBasicSalary`

,FORMAT(ps.RegularHours, 2) `COL6`
,(@act_regular := IF(e.EmployeeType = 'Fixed'
                    , @basic_sal
						  , IF(e.EmployeeType = 'Monthly'
                         , (@basic_sal - (ps.LateDeduction + ps.UndertimeDeduction + ps.AbsenceDeduction))
                         , IFNULL(et.`RegularHoursAmount`, 0)))
						  ) `ActualRegular`
,FORMAT(@act_regular, 2) `COL7`

,(@de_minimis := IFNULL(ROUND(((@act_regular * (esa.TrueSalary / esa.Salary))
                               - @act_regular), 2), 0)
  ) `DeMinimis`
,IF(@de_minimis <= 0, 0.00, FORMAT(ps.RegularHours, 2)) `COL8`
,FORMAT(@de_minimis, 2) `COL9`

,ps.TotalEmpSSS `COL10`
,ps.TotalEmpPhilhealth `COL11`
,ps.TotalEmpHDMF `COL12`

,IF(slp.`LoanNameList` IS NULL, '', REPLACE(slp.`LoanNameList`, ',', '\n')) `COL13`
,IF(slp.`LoanDeductList` IS NULL, '', REPLACE(slp.`LoanDeductList`, ',', '\n')) `COL15`





,CONCAT_WS('\n'
           , IF(once_allow.`AllowanceNameList` IS NULL, '', REPLACE(once_allow.`AllowanceNameList`, ',', '\n'))
			  , IF(day_allow.`AllowanceNameList` IS NULL, '', REPLACE(day_allow.`AllowanceNameList`, ',', '\n'))
			  , IF(semimonth_allow.`AllowanceNameList` IS NULL, '', REPLACE(semimonth_allow.`AllowanceNameList`, ',', '\n'))) `COL16`
,CONCAT_WS('\n'
           , IF(once_allow.`AllowanceAmountList` IS NULL, '', REPLACE(once_allow.`AllowanceAmountList`, ',', '\n'))
           , IF(day_allow.`AllowanceAmountList` IS NULL, '', REPLACE(day_allow.`AllowanceAmountList`, ',', '\n'))
           , IF(semimonth_allow.`AllowanceAmountList` IS NULL, '', REPLACE(semimonth_allow.`AllowanceAmountList`, ',', '\n'))) `COL18`





,IF(plusadj.`AdjustmentName` IS NULL, '', REPLACE(plusadj.`AdjustmentName`, ',', '\n')) `COL19`
,IF(plusadj.PayAmount IS NULL, '', REPLACE(plusadj.PayAmount, ',', '\n')) `COL20`

,IF(lessadj.`AdjustmentName` IS NULL, '', REPLACE(lessadj.`AdjustmentName`, ',', '\n')) `COL21`
,IF(lessadj.PayAmount IS NULL, '', REPLACE(lessadj.PayAmount, ',', '\n')) `COL22`

,psa.TotalGrossSalary `COL23`
,(ps.TotalLoans
  + ps.TotalEmpSSS
  + ps.TotalEmpPhilhealth
  + ps.TotalEmpHDMF
  + ps.TotalEmpWithholdingTax
  + IFNULL((lessadj.`TotalNegativeAdjustment`), 0)) `COL24`
,psa.TotalNetSalary `COL25`

,(e.LeaveAllowance - IFNULL(etlv.VacationLeaveHours, 0)) `COL26`
,(e.SickLeaveAllowance - IFNULL(etlv.SickLeaveHours, 0)) `COL27`

,ps.TotalEmpWithholdingTax `COL32`

,ps.OvertimeHours `COL33`
,FORMAT(ps.OvertimePay, 2) `COL34`

,pstub.TotalTaxableSalary `COL41`
,pstub.TotalEmpWithholdingTax `COL42`
,pstub.TotalEmpSSS `COL43`
,pstub.TotalEmpPhilhealth `COL28`
,pstub.TotalEmpHDMF `COL29`

,text_cutoff_ordinal `COL44`

,CONCAT('Dept: ', dv.Name) `COL100`

,is_endofmonth `IsEndOfMonth`

FROM paystub ps

INNER JOIN employee e
        ON e.RowID=ps.EmployeeID
		     AND e.OrganizationID=ps.OrganizationID
		     AND e.EmploymentStatus NOT IN ('Resigned', 'Terminated')
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
           ,SUM(ete.RegularHoursAmount) `RegularHoursAmount`
           FROM v_uni_employeetimeentry ete
           WHERE ete.OrganizationID = og_rowid
           AND ete.`Date` BETWEEN date_from AND date_to
           AND ete.AsActual = FALSE
           ) et
       ON et.EmployeeID=ps.EmployeeID

INNER JOIN employeesalary esa
        ON esa.RowID = (SELECT esa.RowID # ROUND((esa.BasicPay * (esa.TrueSalary / esa.Salary)), 2)
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
           ,GROUP_CONCAT(ea.AllowanceAmount) `AllowanceAmountList`
           ,GROUP_CONCAT(p.PartNo) `AllowanceNameList`
           FROM employeeallowance ea
           INNER JOIN product p ON p.RowID=ea.ProductID
			  WHERE ea.OrganizationID=og_rowid
			  AND ea.AllowanceAmount != 0
			  AND ea.AllowanceFrequency='One time'
			  AND ea.EffectiveStartDate BETWEEN date_from AND date_to
			  GROUP BY ea.EmployeeID) once_allow
       ON once_allow.EmployeeID=ps.EmployeeID

LEFT JOIN (SELECT ea.*
           ,(@perc0 := AVG(IFNULL(etn.`AttendancePercentage`, 0)))
           ,(@counts0 := COUNT(IFNULL(etn.RowID, 0)))
           ,GROUP_CONCAT( ROUND((ea.AllowanceAmount * (IF(p.`Fixed` = 1, 1, @perc0) * @counts0)), 2) ) `AllowanceAmountList`
           ,GROUP_CONCAT(p.PartNo) `AllowanceNameList`
           FROM employeeallowance ea
           INNER JOIN product p ON p.RowID=ea.ProductID
           LEFT JOIN v_employeetimeentry_numbers etn
                  ON etn.EmployeeID=ea.EmployeeID
                     AND etn.OrganizationID=ea.OrganizationID
                     AND etn.PayPeriodID=pperiod_id
			  WHERE ea.OrganizationID=og_rowid
			  AND ea.AllowanceAmount != 0
			  AND ea.AllowanceFrequency='Daily'
			  AND (ea.EffectiveStartDate >= date_from OR ea.EffectiveEndDate >= date_from)
			  AND (ea.EffectiveStartDate <= date_to OR ea.EffectiveEndDate <= date_to)
			  GROUP BY ea.EmployeeID) day_allow
       ON day_allow.EmployeeID=ps.EmployeeID

LEFT JOIN (SELECT ea.*
           ,(@perc1 := AVG(IFNULL(etn.`AttendancePercentage`, 0)))
           ,GROUP_CONCAT( ROUND((ea.AllowanceAmount * IF(p.`Fixed` = 1, 1, @perc1)), 2) ) `AllowanceAmountList`
           ,GROUP_CONCAT(p.PartNo) `AllowanceNameList`
           FROM employeeallowance ea
           INNER JOIN product p ON p.RowID=ea.ProductID
           LEFT JOIN v_employeetimeentry_numbers etn
                  ON etn.EmployeeID=ea.EmployeeID
                     AND etn.OrganizationID=ea.OrganizationID
                     AND etn.PayPeriodID=pperiod_id
			  WHERE ea.OrganizationID=og_rowid
			  AND ea.AllowanceAmount != 0
			  AND ea.AllowanceFrequency='Semi-monthly'
			  AND (ea.EffectiveStartDate >= date_from OR ea.EffectiveEndDate >= date_from)
			  AND (ea.EffectiveStartDate <= date_to OR ea.EffectiveEndDate <= date_to)
			  GROUP BY ea.EmployeeID) semimonth_allow
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

WHERE ps.OrganizationID=og_rowid
		AND ps.PayPeriodID=pperiod_id

ORDER BY CONCAT(e.LastName, e.FirstName)
;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
