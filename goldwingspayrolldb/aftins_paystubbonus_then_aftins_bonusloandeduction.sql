/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `aftins_paystubbonus_then_aftins_bonusloandeduction`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `aftins_paystubbonus_then_aftins_bonusloandeduction`(IN `OrganizID` INT, IN `UserRowID` INT, IN `EmpRowID` INT, IN `psb_PayPeriodID` INT, IN `min_date_ofparamdate` DATE, IN `max_date_ofparamdate` DATE)
    DETERMINISTIC
BEGIN

/* Disable first the functionalities in AFTINS_paystubbonus trigger */

DECLARE IsFirstHalfOfMonth
        ,is_bonus_loan_deduct_exists BOOL;

DECLARE pay_datefrom
        ,pay_dateto DATE;

SELECT EXISTS(SELECT bld.RowID
              FROM bonusloandeduction bld
              INNER JOIN employeeloanschedule els
                      ON els.RowID=bld.LoanSchedID
                         AND els.OrganizationID=bld.OrganizationID
                         AND els.EmployeeID=EmpRowID
              WHERE bld.PayPeriodID=psb_PayPeriodID
              AND bld.OrganizationID=OrganizID
              ) `Result`
INTO is_bonus_loan_deduct_exists;

SELECT (pp.`Half` = 1) `Result`
,pp.PayFromDate
,pp.PayToDate
FROM payperiod pp
WHERE pp.RowID=psb_PayPeriodID
INTO IsFirstHalfOfMonth
     ,pay_datefrom
	  ,pay_dateto;

INSERT INTO bonusloandeduction
(
  OrganizationID
  ,CreatedBy
  ,LoanSchedID
  ,PayPeriodID
  ,DeductionLoanAmount
) SELECT
  OrganizID
  ,UserRowID
  ,i.RowID
  ,psb_PayPeriodID
  ,(@els_deduct_amt := i.`DeductionAmount`)
  
  FROM (SELECT
        els.RowID
        ,IF(els.LoanPayPeriodLeft = 0
		      , ( els.DeductionAmount + (els.TotalLoanAmount - (els.DeductionAmount * els.NoOfPayPeriod)) )
	         , els.DeductionAmount) `DeductionAmount`
        FROM employeeloanschedule els
		  INNER JOIN employeebonus eb
		          ON eb.EmployeeID = EmpRowID
					    AND eb.OrganizationID = OrganizID
						 AND (eb.EffectiveStartDate >= pay_datefrom OR eb.EffectiveEndDate >= pay_datefrom)
						 AND (eb.EffectiveStartDate <= pay_dateto OR eb.EffectiveEndDate <= pay_dateto)
						 AND els.BonusPotentialPaymentForLoan = 0
						 AND eb.RowID = els.BonusID
        WHERE els.EmployeeID=EmpRowID
		  AND els.OrganizationID=OrganizID
		  AND els.`Status` IN ('In progress', 'Complete')
		  AND els.BonusID IS NOT NULL
		  AND (els.DedEffectiveDateFrom >= min_date_ofparamdate OR els.DedEffectiveDateTo >= min_date_ofparamdate)
		  AND (els.DedEffectiveDateFrom <= max_date_ofparamdate OR els.DedEffectiveDateTo <= max_date_ofparamdate)
        
      UNION
        SELECT
        els.RowID
        ,(els.TotalLoanAmount - ((els.NoOfPayPeriod - els.LoanPayPeriodLeftForBonus) * els.DeductionAmount)) `DeductionAmount`
        FROM employeeloanschedule els
		  INNER JOIN employeebonus eb
		          ON eb.EmployeeID = EmpRowID
					    AND eb.OrganizationID = OrganizID
						 AND (eb.EffectiveStartDate >= pay_datefrom OR eb.EffectiveEndDate >= pay_datefrom)
						 AND (eb.EffectiveStartDate <= pay_dateto OR eb.EffectiveEndDate <= pay_dateto)
						 AND els.BonusPotentialPaymentForLoan = 1
						 AND eb.RowID = els.BonusID
        WHERE els.EmployeeID=EmpRowID
		  AND els.OrganizationID=OrganizID
		  AND els.`Status` IN ('In progress', 'Complete')
		  AND els.BonusID IS NOT NULL
		  AND (els.DedEffectiveDateFrom >= min_date_ofparamdate OR els.DedEffectiveDateTo >= min_date_ofparamdate)
		  AND (els.DedEffectiveDateFrom <= max_date_ofparamdate OR els.DedEffectiveDateTo <= max_date_ofparamdate)
        
        ) i
ON
DUPLICATE
KEY
UPDATE
  LastUpd=CURRENT_TIMESTAMP()
  ,LastUpdBy=UserRowID
  ,DeductionLoanAmount=@els_deduct_amt;


# Bonus pays loan deduction amount
UPDATE employeeloanschedule els
INNER JOIN employeebonus eb
        ON eb.EmployeeID = els.EmployeeID
	        AND eb.OrganizationID = els.OrganizationID
			  AND eb.RowID = els.BonusID

LEFT JOIN (SELECT bld.RowID
           ,bld.LoanSchedID
           FROM bonusloandeduction bld
           INNER JOIN employeeloanschedule els
                   ON els.RowID=bld.LoanSchedID
                      AND els.OrganizationID=bld.OrganizationID
                      AND els.EmployeeID=EmpRowID
							 # AND els.BonusPotentialPaymentForLoan = 0
           WHERE bld.PayPeriodID=psb_PayPeriodID
           AND bld.OrganizationID=OrganizID
           ) i
       ON i.LoanSchedID != els.RowID

SET els.LoanPayPeriodLeft = IF((els.LoanPayPeriodLeft - 1) < 0, 0, (els.LoanPayPeriodLeft - 1))
,els.TotalBalanceLeft = (els.TotalBalanceLeft - els.DeductionAmount)
,els.LastUpdBy=UserRowID
WHERE els.EmployeeID=EmpRowID
AND els.OrganizationID=OrganizID
AND els.`Status`='In progress'
AND els.BonusID IS NOT NULL
AND (els.DedEffectiveDateFrom >= min_date_ofparamdate OR els.DedEffectiveDateTo >= min_date_ofparamdate)
AND (els.DedEffectiveDateFrom <= max_date_ofparamdate OR els.DedEffectiveDateTo <= max_date_ofparamdate)
AND els.BonusPotentialPaymentForLoan = 0
# AND is_bonus_loan_deduct_exists = FALSE
;


# Bonus pays full loan balance
UPDATE employeeloanschedule els
INNER JOIN employeebonus eb
        ON eb.EmployeeID = els.EmployeeID
	        AND eb.OrganizationID = els.OrganizationID
			  AND eb.RowID = els.BonusID

LEFT JOIN (SELECT bld.RowID
           ,bld.LoanSchedID
           FROM bonusloandeduction bld
           INNER JOIN employeeloanschedule els
                   ON els.RowID=bld.LoanSchedID
                      AND els.OrganizationID=bld.OrganizationID
                      AND els.EmployeeID=EmpRowID
							 # AND els.BonusPotentialPaymentForLoan = 1
           WHERE bld.PayPeriodID=psb_PayPeriodID
           AND bld.OrganizationID=OrganizID
           ) i
       ON i.LoanSchedID != els.RowID

SET els.LoanPayPeriodLeft = (els.LoanPayPeriodLeft - els.LoanPayPeriodLeftForBonus)
,els.TotalBalanceLeft = (els.TotalBalanceLeft - (els.TotalLoanAmount - ((els.NoOfPayPeriod - els.LoanPayPeriodLeftForBonus) * els.DeductionAmount)))
,els.LastUpdBy=UserRowID
WHERE els.EmployeeID=EmpRowID
AND els.OrganizationID=OrganizID
AND els.`Status`='In progress'
AND els.BonusID IS NOT NULL
AND (els.DedEffectiveDateFrom >= min_date_ofparamdate OR els.DedEffectiveDateTo >= min_date_ofparamdate)
AND (els.DedEffectiveDateFrom <= max_date_ofparamdate OR els.DedEffectiveDateTo <= max_date_ofparamdate)
AND els.BonusPotentialPaymentForLoan = 1
# AND is_bonus_loan_deduct_exists = FALSE
;


END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
