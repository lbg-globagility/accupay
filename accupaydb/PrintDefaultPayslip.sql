/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `PrintDefaultPayslip`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `PrintDefaultPayslip`(
	IN `OrganizID` INT,
	IN `PayPeriodRowID` INT,
	IN `IsActualFlag` TINYINT

)
    DETERMINISTIC
BEGIN

DECLARE paydate_from DATE;
DECLARE paydat_to DATE;

DECLARE v_hours_per_day INT(2) DEFAULT 8;
DECLARE sec_per_hour INT(11) DEFAULT 3600;

SET @ppIds = (SELECT GROUP_CONCAT(pp.RowID)
					#, SUBDATE(ppd.PayToDate, INTERVAL 12 MONTH) #2018-01-05
					
					FROM payperiod pp
					INNER JOIN payperiod ppd ON ppd.RowID = PayPeriodRowID
					WHERE pp.OrganizationID=ppd.OrganizationID
					AND pp.TotalGrossSalary=ppd.TotalGrossSalary
					#AND SUBDATE(ppd.PayToDate, INTERVAL 12 MONTH) BETWEEN pp.PayFromDate AND pp.PayToDate
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

SELECT
    e.RowID,
    CONCAT(e.EmployeeID, ' / ', e.LastName, ', ', e.FirstName, ' ', LEFT(e.MiddleName, 1)) `COL1`,
    CONCAT_WS(', ', e.LastName, e.FirstName, e.MiddleName) `COL69`,
    ROUND(GetBasicPay(e.RowID, paydate_from, paydat_to, IsActualFlag, ps.BasicHours), 2) `COL70`,
    ps.BasicHours `COL71`,
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
    (ps.TotalAllowance - IFNULL(psiECOLA.PayAmount, 0)) `COL18`,
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
    IFNULL(adjustments.`PayAmounts`, '') `COL91`
FROM paystub ps
INNER JOIN paystubactual psa
ON psa.EmployeeID = ps.EmployeeID AND
    psa.PayFromDate = ps.PayFromDate
INNER JOIN employee e
ON e.RowID = ps.EmployeeID AND
    e.OrganizationID = ps.OrganizationID
INNER JOIN employeesalary es
ON es.EmployeeID = ps.EmployeeID AND
    es.OrganizationID=ps.OrganizationID AND
    (es.EffectiveDateFrom >= ps.PayFromDate OR IFNULL(es.EffectiveDateTo,ps.PayToDate) >= ps.PayFromDate) AND
    (es.EffectiveDateFrom <= ps.PayToDate OR IFNULL(es.EffectiveDateTo,ps.PayToDate) <= ps.PayToDate)
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
        paystub.RowID 'PayStubID'
    FROM scheduledloansperpayperiod
    INNER JOIN employeeloanschedule
    ON employeeloanschedule.RowID = scheduledloansperpayperiod.EmployeeLoanRecordID
    INNER JOIN paystub
    ON paystub.PayPeriodID = scheduledloansperpayperiod.PayPeriodID AND
        paystub.EmployeeID = scheduledloansperpayperiod.EmployeeID
    INNER JOIN product
    ON product.RowID = employeeloanschedule.LoanTypeID
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
#ON psiLeave.PayStubID = ps.RowID
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
            WHERE IsActualFlag = 0 AND
                paystubadjustment.OrganizationID = OrganizID AND
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

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
