/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_paystubbonus`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `INSUPD_paystubbonus`(`OrganizID` INT, `EmpRowID` INT, `UserRowID` INT, `psb_PayPeriodID` INT
, `psb_PayFromDate` DATE
, `psb_PayToDate` DATE
, `psb_TotalGrossSalary` DECIMAL(11,6)
, `psb_TotalNetSalary` DECIMAL(11,6)
, `psb_TotalTaxableSalary` DECIMAL(11,6)
, `psb_TotalEmpSSS` DECIMAL(11,6)
, `psb_TotalEmpWithholdingTax` DECIMAL(11,6)
, `psb_TotalCompSSS` DECIMAL(11,6)
, `psb_TotalEmpPhilhealth` DECIMAL(11,6)
, `psb_TotalCompPhilhealth` DECIMAL(11,6)
, `psb_TotalEmpHDMF` DECIMAL(11,6)
, `psb_TotalCompHDMF` DECIMAL(11,6)
, `psb_TotalVacationDaysLeft` DECIMAL(11,6)
, `psb_TotalUndeclaredSalary` DECIMAL(11,6)
, `psb_TotalLoans` DECIMAL(11,6)
, `psb_TotalBonus` DECIMAL(11,6)
, `psb_TotalAllowance` DECIMAL(11,6)
, `psb_TotalAdjustments` DECIMAL(11,6)
, `psb_ThirteenthMonthInclusion` CHAR(1)
, `psb_FirstTimeSalary` CHAR(1)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11);

DECLARE IsFirstHalfOfMonth BOOL;

DECLARE function_wrapper INT(11);

DECLARE max_date_ofparamdate
       ,min_date_ofparamdate DATE;

SELECT MIN(pyp.PayFromDate)
,MAX(pyp.PayToDate)
FROM payperiod pyp
INNER JOIN payperiod pp
        ON pp.OrganizationID=pyp.OrganizationID
		     AND (pp.PayFromDate >= psb_PayFromDate OR pp.PayToDate >= psb_PayFromDate)
			  AND (pp.PayFromDate <= psb_PayToDate OR pp.PayToDate <= psb_PayToDate)
WHERE pyp.OrganizationID=OrganizID
AND pyp.`Year` = pp.`Year`
INTO min_date_ofparamdate
     ,max_date_ofparamdate;

INSERT INTO paystubbonus
(

    OrganizationID
    ,Created
    ,CreatedBy
    ,PayPeriodID
    ,EmployeeID
    ,PayFromDate
    ,PayToDate
    ,TotalGrossSalary
    ,TotalNetSalary
    ,TotalTaxableSalary
    ,TotalEmpSSS
    ,TotalEmpWithholdingTax
    ,TotalCompSSS
    ,TotalEmpPhilhealth
    ,TotalCompPhilhealth
    ,TotalEmpHDMF
    ,TotalCompHDMF
    ,TotalVacationDaysLeft
    ,TotalUndeclaredSalary
    ,TotalLoans
    ,TotalBonus
    ,TotalAllowance
    ,TotalAdjustments
    ,ThirteenthMonthInclusion
    ,FirstTimeSalary

)   SELECT

    OrganizID
    ,CURRENT_TIMESTAMP()
    ,UserRowID
    ,psb_PayPeriodID
    ,EmpRowID
    ,psb_PayFromDate
    ,psb_PayToDate
    ,psb_TotalGrossSalary
    ,(IFNULL(eb.`SumBonus`,0) - (IFNULL(els.`TotalLoans`, 0) + IFNULL(elss.`TotalLoans`, 0)))
    ,psb_TotalTaxableSalary
    ,psb_TotalEmpSSS
    ,psb_TotalEmpWithholdingTax
    ,psb_TotalCompSSS
    ,psb_TotalEmpPhilhealth
    ,psb_TotalCompPhilhealth
    ,psb_TotalEmpHDMF
    ,psb_TotalCompHDMF
    ,psb_TotalVacationDaysLeft
    ,psb_TotalUndeclaredSalary
    ,(@total_loan := (IFNULL(els.`TotalLoans`, 0) + IFNULL(elss.`TotalLoans`, 0)))# psb_TotalLoans
    ,psb_TotalBonus
    ,psb_TotalAllowance
    ,psb_TotalAdjustments
    ,psb_ThirteenthMonthInclusion
    ,((e.StartDate BETWEEN psb_PayFromDate AND psb_PayToDate) OR (e.StartDate <= psb_PayFromDate)) AND (IFNULL(psb.FirstTimeSalary,0) = '0')
    FROM employee e

    LEFT JOIN (SELECT RowID,FirstTimeSalary FROM paystubbonus WHERE EmployeeID=EmpRowID AND OrganizationID=OrganizID ORDER BY DATEDIFF(PayToDate,CURDATE()) LIMIT 1
	            ) psb
	        ON psb.RowID IS NULL OR psb.RowID IS NOT NULL
    
    LEFT JOIN (SELECT eb.*,SUM(eb.BonusAmount) `SumBonus`
	              FROM employeebonus eb
	              WHERE eb.EmployeeID=EmpRowID
	              AND eb.OrganizationID=OrganizID
	              AND (eb.EffectiveStartDate >= psb_PayFromDate OR eb.EffectiveEndDate >= psb_PayFromDate)
	              AND (eb.EffectiveStartDate <= psb_PayToDate OR eb.EffectiveEndDate <= psb_PayToDate)
               ) eb
			  ON eb.RowID IS NULL OR eb.RowID IS NOT NULL
    
    LEFT JOIN (SELECT els.*
	            ,SUM(IF(els.LoanPayPeriodLeft = 0
					        , ( els.DeductionAmount + (els.TotalLoanAmount - (els.DeductionAmount * els.NoOfPayPeriod)) )
							  , els.DeductionAmount)
						  ) `TotalLoans`
						  
					FROM employeeloanschedule els
               INNER JOIN employeebonus eb
                       ON eb.EmployeeID = EmpRowID
							     AND eb.OrganizationID = OrganizID
								  AND (eb.EffectiveStartDate >= psb_PayFromDate OR eb.EffectiveEndDate >= psb_PayFromDate)
								  AND (eb.EffectiveStartDate <= psb_PayToDate OR eb.EffectiveEndDate <= psb_PayToDate)
								  AND eb.RowID = els.BonusID
								  AND els.BonusPotentialPaymentForLoan = 0
               WHERE els.EmployeeID=EmpRowID
               AND els.OrganizationID=OrganizID
               AND els.`Status` IN ('In progress', 'Complete')
               AND els.BonusID IS NOT NULL
               AND (els.DedEffectiveDateFrom >= min_date_ofparamdate OR els.DedEffectiveDateTo >= min_date_ofparamdate)
               AND (els.DedEffectiveDateFrom <= max_date_ofparamdate OR els.DedEffectiveDateTo <= max_date_ofparamdate)
	            ) els
           ON IFNULL(els.`TotalLoans`, 0) != 0
           
    # ######################################################################
    
    LEFT JOIN (SELECT els.*
	            ,SUM(
	                 (els.TotalLoanAmount - ((els.NoOfPayPeriod - els.LoanPayPeriodLeftForBonus) * els.DeductionAmount))
						  ) `TotalLoans`
						  
					FROM employeeloanschedule els
               INNER JOIN employeebonus eb
                       ON eb.EmployeeID = EmpRowID
							     AND eb.OrganizationID = OrganizID
								  AND (eb.EffectiveStartDate >= psb_PayFromDate OR eb.EffectiveEndDate >= psb_PayFromDate)
								  AND (eb.EffectiveStartDate <= psb_PayToDate OR eb.EffectiveEndDate <= psb_PayToDate)
								  AND eb.RowID = els.BonusID
								  AND els.BonusPotentialPaymentForLoan = 1
               WHERE els.EmployeeID=EmpRowID
               AND els.OrganizationID=OrganizID
               AND els.`Status` IN ('In progress', 'Complete')
               AND els.BonusID IS NOT NULL
               AND (els.DedEffectiveDateFrom >= min_date_ofparamdate OR els.DedEffectiveDateTo >= min_date_ofparamdate)
               AND (els.DedEffectiveDateFrom <= max_date_ofparamdate OR els.DedEffectiveDateTo <= max_date_ofparamdate)
	            ) elss
           ON IFNULL(elss.`TotalLoans`, 0) != 0
           
	 WHERE e.RowID=EmpRowID AND e.OrganizationID=OrganizID
 ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,TotalLoans=IFNULL(@total_loan, 0)
    ,TotalNetSalary=(IFNULL(eb.`SumBonus`,0) - IFNULL(@total_loan, 0))
    ,LastUpdBy=UserRowID;SELECT @@Identity AS ID INTO returnvalue;
 
CALL aftins_paystubbonus_then_aftins_bonusloandeduction(OrganizID, UserRowID, EmpRowID, psb_PayPeriodID, min_date_ofparamdate, max_date_ofparamdate);

RETURN returnvalue;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
