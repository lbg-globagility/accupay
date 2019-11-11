/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `SP_UpdatePaystubAdjustment`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_UpdatePaystubAdjustment`(IN `pa_EmployeeID` VARCHAR(50), IN `pa_PayPeriodID` INT, IN `User_RowID` INT, IN `Og_RowID` INT)
    DETERMINISTIC
BEGIN


DECLARE SumPayStubAdjustments DECIMAL(11,6);
DECLARE ps_TotalUndeclaredSalary DECIMAL(11,6);
DECLARE taxableadjustmentamount DECIMAL(11,6);

DECLARE nontaxableadjustmentamount DECIMAL(11,6);

DECLARE ogRowID INT(11);

DECLARE p_PaystubID INT;
DECLARE p_TotalAdjustments DECIMAL(10, 2);

DECLARE psiRowID INT(11);

DECLARE paydatefrom DATE;

DECLARE paydateto DATE;

DECLARE empRowID INT(11);

DECLARE monthlytaxableincome DECIMAL(11,6);

DECLARE numofdependent INT(11);

DECLARE maritstat TEXT;

DECLARE payfreqRowID INT(11);

DECLARE ExemptAmt DECIMAL(11,6);

DECLARE TaxabIncomeFromAmt DECIMAL(11,6);

DECLARE ExemptInExcessAmt DECIMAL(11,6);

DECLARE emp_taxabsal DECIMAL(11,6);

DECLARE themonth VARCHAR(2);

DECLARE theyear INT(11);

DECLARE thehalf CHAR(1);

DECLARE prod_rowID INT(11);

DECLARE endofmonthpaypRowID INT(11);

DECLARE prev_adjamt DECIMAL(11,6) DEFAULT 0.0;

DECLARE viewRowID INT(11);

DECLARE taxableIncomeProductID INT(11);

DECLARE oldPayStubAdjustments DECIMAL(11, 6);


SET p_PaystubID = (SELECT FN_GetPayStubIDByEmployeeIDAndPayPeriodID(pa_EmployeeID, pa_PayPeriodID, OrganizationID)
                   FROM payperiod
                   WHERE RowID=pa_PayPeriodID);

SELECT ps.OrganizationID,
       pyp.PayFromDate,
       pyp.PayToDate,
       ps.EmployeeID,
       e.NoOfDependents,
       e.MaritalStatus,
       e.PayFrequencyID,
       pyp.`Month`,
       pyp.`Year`,
       pyp.`Half`
FROM paystub ps
INNER JOIN payperiod pyp
        ON pyp.RowID=ps.PayPeriodID
       AND pyp.OrganizationID=ps.OrganizationID
INNER JOIN employee e
        ON e.RowID=ps.EmployeeID
       AND e.OrganizationID=ps.OrganizationID
WHERE ps.RowID=p_PaystubID
LIMIT 1
INTO ogRowID,
     paydatefrom,
     paydateto,
     empRowID,
     numofdependent,
     maritstat,
     payfreqRowID,
     themonth,
     theyear,
     thehalf;



SELECT paystub.TotalAdjustments
FROM paystub
WHERE paystub.RowID = p_PaystubID
INTO oldPayStubAdjustments;

SELECT GET_paystubadjustmenttaxabornontaxab(p_PaystubID,'1')
INTO taxableadjustmentamount;

SELECT GET_paystubadjustmenttaxabornontaxab(p_PaystubID,'0')
INTO nontaxableadjustmentamount;

SET p_TotalAdjustments = IFNULL(taxableadjustmentamount, 0)
                         + IFNULL(nontaxableadjustmentamount, 0);

SELECT v.RowID
FROM `view` v
INNER JOIN paystub ps
        ON ps.RowID=p_PaystubID
WHERE v.OrganizationID=ps.OrganizationID
  AND v.ViewName='Employee Pay Slip'
INTO viewRowID;

SELECT RowID
FROM product p
WHERE p.PartNo='Taxable Income'
  AND p.OrganizationID=ogRowID
INTO taxableIncomeProductID;

SELECT aut.OldValue
FROM audittrail aut
WHERE aut.ViewID=viewRowID
  AND aut.FieldChanged='TotalAdjustments'
  AND aut.ActionPerformed='Update'
  AND aut.ChangedRowID=p_PaystubID
ORDER BY Created DESC
LIMIT 1
INTO prev_adjamt;

IF prev_adjamt IS NULL THEN
    SET prev_adjamt = 0.0;
END IF;

UPDATE paystub
SET paystub.TotalAdjustments = p_TotalAdjustments,
    paystub.TotalNetSalary = (paystub.TotalNetSalary - oldPayStubAdjustments)
                             + p_TotalAdjustments
WHERE paystub.RowID = p_PaystubID;






SELECT NULL INTO taxableadjustmentamount;

IF taxableadjustmentamount IS NOT NULL THEN
    UPDATE paystub ps
    INNER JOIN paystubitem psi
            ON psi.ProductID=taxableIncomeProductID
           AND psi.OrganizationID=ogRowID
           AND psi.PayStubID=p_PaystubID
    SET ps.TotalTaxableSalary=psi.PayAmount
    WHERE ps.RowID=p_PaystubID;

    SELECT `Half`
    ,`Month`
    ,`Year`
    ,OrganizationID
    FROM payperiod
    WHERE RowID = pa_PayPeriodID
    INTO thehalf
            ,themonth
            ,theyear
            ,ogRowID;

    IF thehalf = '0' THEN
        SELECT PayFromDate
        FROM payperiod
        WHERE OrganizationID=ogRowID AND `Month`=themonth AND `Year`=theyear AND TotalGrossSalary=payfreqRowID
        ORDER BY PayFromDate,PayToDate
        LIMIT 1 INTO paydatefrom;

        SELECT PayFromDate,
               RowID
        FROM payperiod
        WHERE OrganizationID=ogRowID
          AND `Month`=themonth
          AND `Year`=theyear
          AND TotalGrossSalary=payfreqRowID
        ORDER BY PayFromDate DESC,
                 PayToDate DESC
        LIMIT 1
        INTO paydateto,
             endofmonthpaypRowID;

        SELECT SUM(IFNULL(ps.TotalTaxableSalary, 0)),
               e.RowID
        FROM paystub ps
        INNER JOIN employee e
                ON e.EmployeeID=pa_EmployeeID
               AND e.OrganizationID=ogRowID
        WHERE (ps.PayFromDate >= paydatefrom OR ps.PayToDate >= paydatefrom)
          AND (ps.PayFromDate <= paydateto OR ps.PayToDate <= paydateto)
          AND ps.EmployeeID=e.RowID
          AND ps.OrganizationID=ogRowID
        INTO monthlytaxableincome,
             empRowID;

        SELECT pwt.ExemptionAmount,
               pwt.TaxableIncomeFromAmount,
               pwt.ExemptionInExcessAmount
        FROM paywithholdingtax pwt
        INNER JOIN employee e ON e.RowID=empRowID
        INNER JOIN filingstatus fs ON fs.MaritalStatus=e.MaritalStatus AND fs.Dependent=e.NoOfDependents
        WHERE pwt.FilingStatusID=fs.RowID
          AND pwt.PayFrequencyID=2
          AND (monthlytaxableincome BETWEEN pwt.TaxableIncomeFromAmount AND pwt.TaxableIncomeToAmount)
        LIMIT 1
        INTO ExemptAmt,
             TaxabIncomeFromAmt,
             ExemptInExcessAmt;

        SET emp_taxabsal = (ExemptAmt + ((monthlytaxableincome - TaxabIncomeFromAmt) * ExemptInExcessAmt));

        SELECT RowID
        FROM product
        WHERE PartNo='Withholding Tax'
        AND OrganizationID=ogRowID
        INTO prod_rowID;

        SELECT RowID
        FROM paystub
        WHERE EmployeeID=empRowID
        AND OrganizationID=ogRowID
        AND PayPeriodID=pa_PayPeriodID
        INTO p_PaystubID;

        UPDATE paystubitem
        SET PayAmount=emp_taxabsal
        WHERE PayStubID=p_PaystubID
        AND OrganizationID=ogRowID
        AND ProductID=prod_rowID;

        UPDATE paystub ps
        SET ps.TotalEmpWithholdingTax = emp_taxabsal
        WHERE ps.RowID=p_PaystubID;
    END IF;
END IF;

SELECT SUM(psa.PayAmount), ps.RowID
FROM paystubadjustment psa
INNER JOIN paystub ps
        ON ps.RowID=psa.PayStubID
       AND ps.OrganizationID=ogRowID
       AND ps.EmployeeID=empRowID
       AND ps.PayPeriodID=pa_PayPeriodID
INTO SumPayStubAdjustments, psiRowID;

SET SumPayStubAdjustments = IFNULL(SumPayStubAdjustments,0);

SELECT GET_employeeundeclaredsalarypercent(empRowID,ogRowID,pp.PayFromDate,pp.PayToDate)
FROM payperiod pp
WHERE pp.RowID=pa_PayPeriodID
INTO ps_TotalUndeclaredSalary;

SET ps_TotalUndeclaredSalary = IFNULL(ps_TotalUndeclaredSalary,0);

IF ps_TotalUndeclaredSalary < 1.0 THEN
    SET ps_TotalUndeclaredSalary = ps_TotalUndeclaredSalary + 1.000000;
ELSEIF ps_TotalUndeclaredSalary > 1.0 THEN
    SET ps_TotalUndeclaredSalary = ps_TotalUndeclaredSalary - 1.000000;
END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
