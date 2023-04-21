/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP PROCEDURE IF EXISTS `PrintMorningSunPayslip`;
DELIMITER //
CREATE PROCEDURE `PrintMorningSunPayslip`(
	IN `OrganizID` INT,
	IN `PayPeriodRowID` INT,
	IN `IsActualFlag` TINYINT
)
    DETERMINISTIC
BEGIN

DECLARE paydate_from DATE;
DECLARE paydat_to DATE;

DECLARE v_hours_per_day INT(2) DEFAULT 8;

SET @ppIds = (SELECT GROUP_CONCAT(pp.RowID)
					
					
					FROM payperiod pp
					INNER JOIN payperiod ppd ON ppd.RowID = PayPeriodRowID
					WHERE pp.OrganizationID=ppd.OrganizationID
					AND pp.TotalGrossSalary=ppd.TotalGrossSalary
					
					AND pp.PayFromDate >= SUBDATE(ppd.PayToDate, INTERVAL 12 MONTH)
					AND pp.PayToDate <= ppd.PayToDate);

SELECT
    pp.PayFromDate,
    pp.PayToDate
FROM payperiod pp
WHERE pp.RowID = PayPeriodRowID
INTO
    paydate_from,
    paydat_to;

DROP TEMPORARY TABLE if EXISTS activesalary;
CREATE TEMPORARY TABLE if NOT EXISTS activesalary
SELECT
d.DateValue,
es.*
FROM employeesalary es
INNER JOIN employee e ON e.RowID=es.EmployeeID
INNER JOIN dates d ON d.DateValue BETWEEN es.EffectiveDateFrom AND paydat_to
AND d.DateValue BETWEEN paydate_from AND paydat_to
INNER JOIN payperiod pp ON pp.OrganizationID=es.OrganizationID AND pp.TotalGrossSalary=e.PayFrequencyID AND pp.RowID=PayPeriodRowID
AND pp.PayToDate=d.DateValue
WHERE es.OrganizationID=OrganizID
;

DROP TEMPORARY TABLE if EXISTS latestsalary;
CREATE TEMPORARY TABLE if NOT EXISTS latestsalary
SELECT
EmployeeID
, MIN(DATEDIFF(paydat_to, EffectiveDateFrom)) `Result`
FROM activesalary
WHERE DATEDIFF(paydat_to, EffectiveDateFrom) > -1
GROUP BY EmployeeID
;

SELECT
    e.RowID,
    CONCAT(e.EmployeeID, ' / ', e.LastName, ', ', e.FirstName, ' ', LEFT(e.MiddleName, 1)) `COL1`,
    CONCAT_WS(', ', e.LastName, e.FirstName, e.MiddleName) `COL69`,
    IF(e.EmployeeType='Daily', ps.RegularPay, ROUND(GetBasicPay(e.RowID, paydate_from, paydat_to, IsActualFlag, ps.BasicHours), 2)) `COL70`,
    IF(e.EmployeeType='Daily', ps.RegularHours, ps.BasicHours) `COL71`,
    IFNULL(ps.AbsentHours + IF(e.EmployeeType = 'Monthly', ps.LeaveHours, 0), 0) `COL72`,
    IF(IsActualFlag, es.TrueSalary, es.Salary) `COL80`,
    IF(
        IsActualFlag,
        psa.AbsenceDeduction + IF(e.EmployeeType = 'Monthly', psa.LeavePay, 0),
        ps.AbsenceDeduction + IF(e.EmployeeType = 'Monthly', ps.LeavePay, 0)
    ) AS `COL5`,
    ps.LateHours `COL6`,
    IF(IsActualFlag, psa.LateDeduction, ps.LateDeduction) `COL7`,
    ps.UndertimeHours `COL8`,
    IF(IsActualFlag, psa.UndertimeDeduction, ps.UndertimeDeduction) `COL9`,
    ps.OvertimeHours `COL12`,
    (
        IF(
            IsActualFlag,
            (
                psa.OvertimePay +
                psa.NightDiffOvertimePay
            ),
            (
                ps.OvertimePay +
                ps.NightDiffOvertimePay
            )
        )
    ) `COL13`,
    ps.NightDiffHours `COL14`,
    IF(IsActualFlag, psa.NightDiffPay, ps.NightDiffPay) `COL15`,
    (ps.TotalAllowance + ps.TotalTaxableAllowance - IFNULL(psiECOLA.PayAmount, 0)) `COL18`,
    IF(IsActualFlag = TRUE, psa.TotalAdjustments, ps.TotalAdjustments) `COL19`,
    IF(
        IsActualFlag,
        psa.TotalGrossSalary + psa.TotalAdjustments,
        ps.TotalGrossSalary + ps.TotalAdjustments
    ) `COL20`,
    ps.TotalEmpSSS `COL21`,
    ps.TotalEmpPhilhealth `COL22`,
    ps.TotalEmpHDMF `COL23`,
    ps.TotalEmpWithholdingTax `COL25`,
    ps.TotalLoans `COL26`,
    IF(IsActualFlag, psa.TotalNetSalary, ps.TotalNetSalary) `COL27`,
    allowances.`Names` AS `COL28`, -- Deprecated
    allowances.PayAmounts AS `COL29`, -- Deprecated
    payStubLoans.`Names` AS `COL31`,
    payStubLoans.PayAmounts AS `COL32`,
    payStubLoans.TotalBalanceLeft AS `COl36`,
    adjustments.`Names` AS `COL37`, -- Deprecated
    adjustments.PayAmounts AS `COL38`,-- Deprecated
    ps.LeaveHours `COL40`,
    IF(IsActualFlag, psa.LeavePay, ps.LeavePay) `COL41`,
    
	ps.RegularHolidayHours `COL42`,
	IF(IsActualFlag,
		psa.RegularHolidayPay,
		ps.RegularHolidayPay
		) `COL47`,
    
    ps.RegularHolidayOTHours `COL48`,
    IF(IsActualFlag, psa.RegularHolidayOTPay, ps.RegularHolidayOTPay) `COL49`,
    
    ps.SpecialHolidayHours `COL50`,
    IF(IsActualFlag, psa.SpecialHolidayPay, ps.SpecialHolidayPay) `COL51`,
    
    ps.SpecialHolidayOTHours `COL52`,
    IF(IsActualFlag, psa.SpecialHolidayOTPay, ps.SpecialHolidayOTPay) `COL53`,
    
    ps.RestDayHours `COL54`,
    IF(IsActualFlag, psa.RestDayPay, ps.RestDayPay) `COL55`,
    
    ps.RestDayOTHours `COL56`,
    IF(IsActualFlag, psa.RestDayOTPay, ps.RestDayOTPay) `COL57`,
    
    IFNULL(psiECOLA.PayAmount, 0) `COL43`,
    psiLeave.`Names` `COL44`,
    psiLeave.Availed `COl45`,
    psiLeave.Balance `COL46`,
    IFNULL(adjustments.`Names`, '') `COL90`,
    IFNULL(adjustments.`PayAmounts`, '') `COL91`,

    payStubLoans.`Vale` `COL10`,
    payStubLoans.`AdvVale` `COL11`,
    payStubLoans.`SssSalaryLoan` `COL58`,
    payStubLoans.`SssCalamityLoan` `COL59`,
    payStubLoans.`HdmfSalaryLoan` `COL60`,
    payStubLoans.`HdmfCalamityLoan` `COL61`

FROM paystub ps
INNER JOIN paystubactual psa
ON psa.EmployeeID = ps.EmployeeID AND
    psa.PayFromDate = ps.PayFromDate
INNER JOIN employee e
ON e.RowID = ps.EmployeeID AND
    e.OrganizationID = ps.OrganizationID
INNER JOIN (SELECT
				ii.*
				FROM latestsalary i
				INNER JOIN activesalary ii ON ii.EmployeeID=i.EmployeeID AND DATEDIFF(paydat_to, EffectiveDateFrom)=i.Result
				) es
ON es.EmployeeID = ps.EmployeeID AND
    es.OrganizationID=ps.OrganizationID
LEFT JOIN (
    SELECT
        REPLACE(GROUP_CONCAT(IFNULL(product.PartNo, '')), ',', '\n') 'Names',
        REPLACE(GROUP_CONCAT(IFNULL(paystubitem.PayAmount, '')), ',', '\n') 'PayAmounts',
        paystubitem.PayStubID
    FROM paystubitem
    INNER JOIN product
    ON product.RowID = paystubitem.ProductID AND
        product.OrganizationID = paystubitem.OrganizationID AND
        product.`Category` = 'Allowance Type'
    GROUP BY paystubitem.PayStubID
) allowances
ON allowances.PayStubID = ps.RowID
LEFT JOIN (
    SELECT
        REPLACE(GROUP_CONCAT(IFNULL(product.PartNo, '')), ',', '\n') 'Names',
        REPLACE(GROUP_CONCAT(IFNULL(ROUND(scheduledloansperpayperiod.DeductionAmount, 2), '')), ',', '\n') 'PayAmounts',
        REPLACE(GROUP_CONCAT(IFNULL(ROUND(scheduledloansperpayperiod.TotalBalanceLeft, 2), '')), ',', '\n') 'TotalBalanceLeft',
        paystub.RowID 'PayStubID',

        IF(FIND_IN_SET('Vale', GROUP_CONCAT(product.PartNo)) > 0, scheduledloansperpayperiod.DeductionAmount, 0) `Vale`,
        IF(FIND_IN_SET('Adv. Vale', GROUP_CONCAT(product.PartNo)) > 0, scheduledloansperpayperiod.DeductionAmount, 0) `AdvVale`,
        IF(FIND_IN_SET('SSS Salary Loan', GROUP_CONCAT(product.PartNo)) > 0, scheduledloansperpayperiod.DeductionAmount, 0) `SssSalaryLoan`,
        IF(FIND_IN_SET('SSS Calamity Loan', GROUP_CONCAT(product.PartNo)) > 0, scheduledloansperpayperiod.DeductionAmount, 0) `SssCalamityLoan`,
        IF(FIND_IN_SET('Pag-ibig Salary Loan', GROUP_CONCAT(product.PartNo)) > 0, scheduledloansperpayperiod.DeductionAmount, 0) `HdmfSalaryLoan`,
        IF(FIND_IN_SET('Pag-ibig Calamity Loan', GROUP_CONCAT(product.PartNo)) > 0, scheduledloansperpayperiod.DeductionAmount, 0) `HdmfCalamityLoan`

    FROM scheduledloansperpayperiod
    INNER JOIN employeeloanschedule
    ON employeeloanschedule.RowID = scheduledloansperpayperiod.EmployeeLoanRecordID
    INNER JOIN paystub
    ON paystub.PayPeriodID = scheduledloansperpayperiod.PayPeriodID AND
        paystub.EmployeeID = scheduledloansperpayperiod.EmployeeID
    INNER JOIN product
    ON product.RowID = employeeloanschedule.LoanTypeID AND product.GovtDeductionType='None'
    GROUP BY paystub.RowID
) payStubLoans
ON payStubLoans.PayStubID = ps.RowID

LEFT JOIN (SELECT ROUND((lt.Balance / v_hours_per_day), 2) AS 'Balance'
				, lt.EmployeeID
				, p.PartNo `Names`
				, et.`VacationLeaveHours` `Availed`
				
				FROM (SELECT lt.*
						, MAX(lt.TransactionDate) `MaxTransactionDate`
						FROM leavetransaction lt
						WHERE FIND_IN_SET(lt.PayPeriodID, @ppIds) > 0
						AND lt.OrganizationID = OrganizID
						GROUP BY lt.EmployeeID) i
				
				INNER JOIN leavetransaction lt ON lt.EmployeeID=i.EmployeeID AND lt.TransactionDate=i.`MaxTransactionDate`
				INNER JOIN leaveledger ll ON ll.RowID=lt.LeaveLedgerID
				INNER JOIN product p ON p.RowID=ll.ProductID AND p.PartNo='Vacation Leave'
				INNER JOIN category c ON c.RowID=p.CategoryID AND c.CategoryName='Leave Type'
				
				LEFT JOIN (SELECT
								ete.EmployeeID
								, SUM(ete.VacationLeaveHours / 8) `VacationLeaveHours`
								, SUM(ete.SickLeaveHours / 8) `SickLeaveHours`
								FROM employeetimeentry ete
								INNER JOIN payperiod pp ON pp.RowID = PayPeriodRowID
								WHERE ete.OrganizationID = OrganizID
								AND ete.`Date` BETWEEN pp.PayFromDate AND pp.PayToDate
								GROUP BY ete.EmployeeID
								) et ON et.EmployeeID = i.EmployeeID
) psiLeave

ON psiLeave.EmployeeID = ps.EmployeeID

LEFT JOIN (
    SELECT
        psi.*,
        p.PartNo AS ItemName
    FROM paystubitem psi
    INNER JOIN product p
    ON p.RowID=psi.ProductID AND
        p.OrganizationID=psi.OrganizationID AND
        p.PartNo='Ecola'
    WHERE psi.Undeclared=0 AND
        psi.OrganizationID=OrganizID AND
        psi.PayAmount!=0
) psiECOLA
ON psiECOLA.PayStubID=ps.RowID
LEFT JOIN (
    SELECT
        REPLACE(GROUP_CONCAT(IFNULL(adjustment.ItemName, '')), ',', '\n') `Names`,
        REPLACE(GROUP_CONCAT(IFNULL(ROUND(adjustment.PayAmount, 2), '')), ',', '\n') `PayAmounts`,
        adjustment.PayStubID
    FROM (
            SELECT
                paystubadjustment.PayStubID,
                paystubadjustment.PayAmount,
                product.PartNo AS ItemName
            FROM paystubadjustment
            INNER JOIN product
            ON product.RowID = paystubadjustment.ProductID
            WHERE paystubadjustment.OrganizationID = OrganizID AND
                paystubadjustment.PayAmount != 0
        UNION
            SELECT
                paystubadjustmentactual.PayStubID,
                paystubadjustmentactual.PayAmount,
                product.PartNo AS ItemName
            FROM paystubadjustmentactual
            INNER JOIN product
            ON product.RowID = paystubadjustmentactual.ProductID
            WHERE IsActualFlag = 1 AND
                paystubadjustmentactual.OrganizationID = OrganizID AND
                paystubadjustmentactual.PayAmount != 0
    ) adjustment
    GROUP BY adjustment.PayStubID
) adjustments
ON adjustments.PayStubID = ps.RowID




WHERE ps.OrganizationID = OrganizID AND
    ps.PayFromDate = paydate_from AND
    ps.PayToDate = paydat_to
GROUP BY ps.EmployeeID
ORDER BY CONCAT(e.LastName, e.FirstName, e.MiddleName);

END//
DELIMITER ;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
