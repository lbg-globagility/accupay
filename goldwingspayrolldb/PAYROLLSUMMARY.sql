/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `PAYROLLSUMMARY`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `PAYROLLSUMMARY`(IN `ps_OrganizationID` INT, IN `ps_PayPeriodID1` INT, IN `ps_PayPeriodID2` INT, IN `psi_undeclared` CHAR(1), IN `strSalaryDistrib` VARCHAR(50))
    DETERMINISTIC
BEGIN

DECLARE paypdatefrom DATE;

DECLARE paypdateto DATE;

DECLARE payfreq_rowid INT(11);


SELECT PayFromDate, TotalGrossSalary FROM payperiod WHERE RowID=ps_PayPeriodID1 INTO paypdatefrom, payfreq_rowid;

SELECT PayToDate FROM payperiod WHERE RowID=IFNULL(ps_PayPeriodID2,ps_PayPeriodID1) INTO paypdateto;



SELECT
    IFNULL(
        (
            SELECT
                IF(
                    e.EmployeeType='Fixed',
                    IFNULL(es.BasicPay, 0),
                    IF(
                        e.EmployeeType='Daily',
                        SUM(IFNULL(i.RegularHoursAmount, 0)),
                        (IFNULL(es.BasicPay, 0) - (SUM(IFNULL(i.HoursLateAmount, 0)) + SUM(IFNULL(i.UndertimeHoursAmount, 0)) + SUM(IFNULL(i.Absent, 0)) + SUM(IFNULL(HolidayPayAmount, 0))))
                    )
                )
            FROM (
                SELECT *
                FROM v_uni_employeetimeentry i
                WHERE i.`AsActual` = psi_undeclared
                    AND i.OrganizationID = ps_OrganizationID
            ) i
            LEFT JOIN employeesalary es
                ON es.RowID = i.EmployeeSalaryID
            WHERE i.EmployeeID = ps.EmployeeID
                AND i.`Date` BETWEEN paypdatefrom AND paypdateto
        ),
        0
    ) `DatCol21`,
    ps.TotalGrossSalary `DatCol22`,
    ps.TotalNetSalary `DatCol23`,
    IF(
        (GET_employeerateperday(ps.EmployeeID, ps_OrganizationID, ps.PayFromDate) <= d.MinimumWageAmount),
        0,
        ps.TotalTaxableSalary
    ) `DatCol24`
    ,(ps.TotalEmpSSS) `DatCol25`
    ,(IFNULL(pst9.PayAmount,0)) `DatCol26`
    ,(ps.TotalEmpPhilhealth) `DatCol27`
    ,(ps.TotalEmpHDMF) `DatCol28`
    ,(ps.TotalLoans) `DatCol29`
    ,(ps.TotalBonus) `DatCol30`
    ,(ps.TotalAllowance) `DatCol31`
    ,e.EmployeeID `DatCol2`
    ,CONCAT_WS(', ', e.LastName, e.FirstName, INITIALS(e.MiddleName,'. ','1')) `DatCol3`
    ,UCASE(e.FirstName) 'FirstName'
    ,INITIALS(e.MiddleName,'. ','1') 'MiddleName'
    ,UCASE(e.LastName) 'LastName'
    ,UCASE(e.Surname) 'Surname'
    ,UCASE(p.PositionName) 'PositionName'
    ,UCASE(d.Name) `DatCol1`
    ,e.RowID 'EmployeeRowID'
    ,(IFNULL(pst.PayAmount,0)) `DatCol33`
    ,(IFNULL(pst1.PayAmount,0)) `DatCol34`
    ,(IFNULL(pst2.PayAmount,0)) `DatCol35`
    ,(IFNULL(pst3.PayAmount,0)) `DatCol36`
    ,(IFNULL(pst4.PayAmount,0)) `DatCol37`
    ,(IFNULL(pst5.PayAmount,0)) `DatCol38`
    ,IFNULL(pst10.PayAmount,0) AS DatCol32
    ,(GET_employeerateperday(ps.EmployeeID,ps_OrganizationID,PayFromDate) <= d.MinimumWageAmount) AS IsMinimum
    ,IFNULL(agf.DailyFee,0) `DatCol39`
    ,IFNULL(tmp.Amount,0) `DatCol40`
    ,CONCAT_WS(' to ', DATE_FORMAT(ps.PayFromDate, IF(YEAR(ps.PayFromDate) = YEAR(ps.PayToDate), '%c/%e', '%c/%e/%Y')), DATE_FORMAT(ps.PayToDate,'%c/%e/%Y')) `DatCol20`
    ,timeEntrySummary.RegularHoursWorked AS `DatCol41`
    ,(IFNULL(ps.TotalNetSalary, 0) + IFNULL(tmp.Amount, 0) + IFNULL(agf.DailyFee, 0)) AS `DatCol42`
FROM v_uni_paystub ps
LEFT JOIN employee e ON e.RowID=ps.EmployeeID
LEFT JOIN `position` p ON p.RowID=e.PositionID
LEFT JOIN division d ON d.RowID=p.DivisionId
INNER JOIN product pd ON pd.OrganizationID=ps_OrganizationID AND pd.PartNo='Tardiness'
LEFT JOIN paystubitem pst ON pst.PayStubID=ps.RowID AND pst.ProductID=pd.RowID AND pst.`Undeclared`=psi_undeclared

INNER JOIN product pd1 ON pd1.OrganizationID=ps_OrganizationID AND pd1.PartNo='Undertime'
LEFT JOIN paystubitem pst1 ON pst1.PayStubID=ps.RowID AND pst1.ProductID=pd1.RowID AND pst1.`Undeclared`=psi_undeclared

INNER JOIN product pd2 ON pd2.OrganizationID=ps_OrganizationID AND pd2.PartNo='Night differential'
LEFT JOIN paystubitem pst2 ON pst2.PayStubID=ps.RowID AND pst2.ProductID=pd2.RowID AND pst2.`Undeclared`=psi_undeclared

INNER JOIN product pd3 ON pd3.OrganizationID=ps_OrganizationID AND pd3.PartNo='Holiday pay'
LEFT JOIN paystubitem pst3 ON pst3.PayStubID=ps.RowID AND pst3.ProductID=pd3.RowID AND pst3.`Undeclared`=psi_undeclared

INNER JOIN product pd4 ON pd4.OrganizationID=ps_OrganizationID AND pd4.PartNo='Overtime'
LEFT JOIN paystubitem pst4 ON pst4.PayStubID=ps.RowID AND pst4.ProductID=pd4.RowID AND pst4.`Undeclared`=psi_undeclared

INNER JOIN product pd5 ON pd5.OrganizationID=ps_OrganizationID AND pd5.PartNo='Night differential OT'
LEFT JOIN paystubitem pst5 ON pst5.PayStubID=ps.RowID AND pst5.ProductID=pd5.RowID AND pst5.`Undeclared`=psi_undeclared

INNER JOIN product pd6 ON pd6.OrganizationID=ps_OrganizationID AND pd6.PartNo='Gross Income'
LEFT JOIN paystubitem pst6 ON pst6.PayStubID=ps.RowID AND pst6.ProductID=pd6.RowID AND pst6.`Undeclared`=psi_undeclared

INNER JOIN product pd7 ON pd7.OrganizationID=ps_OrganizationID AND pd7.PartNo='Net Income'
LEFT JOIN paystubitem pst7 ON pst7.PayStubID=ps.RowID AND pst7.ProductID=pd7.RowID AND pst7.`Undeclared`=psi_undeclared

INNER JOIN product pd8 ON pd8.OrganizationID=ps_OrganizationID AND pd8.PartNo='Taxable Income'
LEFT JOIN paystubitem pst8 ON pst8.PayStubID=ps.RowID AND pst8.ProductID=pd8.RowID AND pst8.`Undeclared`=psi_undeclared

INNER JOIN product pd9 ON pd9.OrganizationID=ps_OrganizationID AND pd9.PartNo='Withholding Tax'
LEFT JOIN paystubitem pst9 ON pst9.PayStubID=ps.RowID AND pst9.ProductID=pd9.RowID AND pst9.`Undeclared`=psi_undeclared

INNER JOIN product pd10 ON pd10.OrganizationID=ps_OrganizationID AND pd10.PartNo='Absent'
LEFT JOIN paystubitem pst10 ON pst10.PayStubID=ps.RowID AND pst10.ProductID=pd10.RowID AND pst10.`Undeclared`=psi_undeclared

LEFT JOIN (
        SELECT
            RowID,
            EmployeeID,
            SUM(DailyFee) AS DailyFee
        FROM agencyfee
        WHERE OrganizationID=ps_OrganizationID
            AND DailyFee > 0
            AND TimeEntryDate BETWEEN paypdatefrom AND paypdateto
        GROUP BY EmployeeID
    ) agf
    ON IFNULL(agf.RowID, 1) > 0
    AND agf.EmployeeID=ps.EmployeeID
LEFT JOIN thirteenthmonthpay tmp
    ON tmp.OrganizationID=ps.OrganizationID
    AND tmp.PaystubID=ps.RowID
LEFT JOIN (
        SELECT EmployeeID, SUM(RegularHoursWorked) AS RegularHoursWorked
        FROM employeetimeentry
        WHERE OrganizationID = ps_OrganizationID
            AND employeetimeentry.`Date` BETWEEN paypdatefrom AND paypdateto
        GROUP BY employeetimeentry.EmployeeID
    ) timeEntrySummary
    ON timeEntrySummary.EmployeeID = ps.EmployeeID
WHERE ps.OrganizationID=ps_OrganizationID
    AND ps.TotalNetSalary > 0
    AND (ps.PayFromDate >= paypdatefrom OR ps.PayToDate >= paypdatefrom)
    AND (ps.PayFromDate <= paypdateto OR ps.PayToDate <= paypdateto)
    AND LENGTH(IFNULL(e.ATMNo,''))=IF(strSalaryDistrib = 'Cash', 0, LENGTH(IFNULL(e.ATMNo,'')))
    AND ps.AsActual = psi_undeclared
ORDER BY d.Name, e.LastName;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
