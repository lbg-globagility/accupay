/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `PAYROLLSUMMARY`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `PAYROLLSUMMARY`(
	IN `ps_OrganizationID` INT,
	IN `ps_PayPeriodID1` INT,
	IN `ps_PayPeriodID2` INT,
	IN `psi_undeclared` CHAR(1),
	IN `strSalaryDistrib` VARCHAR(50)
)
BEGIN

DECLARE paypdatefrom DATE;
DECLARE paypdateto DATE;
DECLARE payfreq_rowid INT(11);


SELECT
    PayFromDate,
    TotalGrossSalary
FROM payperiod
WHERE RowID = ps_PayPeriodID1
INTO
    paypdatefrom,
    payfreq_rowid;

SELECT PayToDate
FROM payperiod
WHERE RowID = IFNULL(ps_PayPeriodID2, ps_PayPeriodID1)
INTO paypdateto;

SELECT
    e.RowID 'EmployeeRowID',
    e.EmployeeID `DatCol2`,
    IF(psi_undeclared, paystubactual.RegularPay, paystub.RegularPay) `DatCol21`,
    IF(psi_undeclared, paystubactual.OvertimePay, paystub.OvertimePay) 'DatCol37',
    IF(psi_undeclared, paystubactual.NightDiffPay, paystub.NightDiffPay) 'DatCol35',
    IF(psi_undeclared, paystubactual.NightDiffOvertimePay, paystub.NightDiffOvertimePay) 'DatCol38',
    IF(psi_undeclared, paystubactual.HolidayPay, paystub.HolidayPay) 'DatCol36',
    IF(psi_undeclared, paystubactual.LateDeduction, paystub.LateDeduction) 'DatCol33',
    IF(psi_undeclared, paystubactual.UndertimeDeduction, paystub.UndertimeDeduction) 'DatCol34',
    IF(psi_undeclared, paystubactual.AbsenceDeduction, paystub.AbsenceDeduction) 'DatCol32',
    paystub.TotalBonus `DatCol30`,
    paystub.TotalAllowance `DatCol31`,
    IF(psi_undeclared, paystubactual.TotalGrossSalary, paystub.TotalGrossSalary) `DatCol22`,
    IF(
        psi_undeclared,
        paystubactual.TotalNetSalary + IFNULL(agf.DailyFee, 0),
        paystub.TotalNetSalary + IFNULL(agf.DailyFee, 0)
    ) `DatCol23`,
    paystub.TotalTaxableSalary `DatCol24`,
    paystub.TotalEmpSSS `DatCol25`,
    paystub.TotalEmpPhilhealth `DatCol27`,
    paystub.TotalEmpHDMF `DatCol28`,
    paystub.TotalEmpWithholdingTax `DatCol26`,
    paystub.TotalLoans `DatCol29`,
    CONCAT_WS(', ', e.LastName, e.FirstName, INITIALS(e.MiddleName,'. ','1')) `DatCol3`,
    UCASE(e.FirstName) 'FirstName',
    INITIALS(e.MiddleName,'. ','1') 'MiddleName',
    UCASE(e.LastName) 'LastName',
    UCASE(e.Surname) 'Surname',
    UCASE(p.PositionName) 'PositionName',
    UCASE(d.Name) `DatCol1`,
    IFNULL(agf.DailyFee,0) `DatCol39`,
    IFNULL(thirteenthmonthpay.Amount,0) `DatCol40`,
    CONCAT_WS(
        ' to ',
        DATE_FORMAT(paystub.PayFromDate, IF(YEAR(paystub.PayFromDate) = YEAR(paystub.PayToDate), '%c/%e', '%c/%e/%Y')),
        DATE_FORMAT(paystub.PayToDate,'%c/%e/%Y')
    ) `DatCol20`,
    paystub.RegularHours `DatCol41`,
    (IF(psi_undeclared, paystubactual.TotalNetSalary, paystub.TotalNetSalary) + IFNULL(thirteenthmonthpay.Amount,0) + IFNULL(agf.DailyFee, 0)) `DatCol42`,
    IF(
        psi_undeclared,
        GetActualDailyRate(e.RowID, e.OrganizationID, paystub.PayFromDate),
        GET_employeerateperday(e.RowID, e.OrganizationID, paystub.PayFromDate)
    ) `DatCol43`,
    paystub.OvertimeHours `DatCol44`
FROM paystub
LEFT JOIN paystubactual
ON paystubactual.EmployeeID = paystub.EmployeeID AND
    paystubactual.PayPeriodID = paystub.PayPeriodID
LEFT JOIN employee e
ON e.RowID = paystub.EmployeeID
LEFT JOIN `position` p
ON p.RowID = e.PositionID
LEFT JOIN division d
ON d.RowID = p.DivisionId
LEFT JOIN (
    SELECT
        RowID,
        EmployeeID,
        SUM(DailyFee) AS DailyFee
    FROM agencyfee
    WHERE OrganizationID=ps_OrganizationID AND
        DailyFee > 0 AND
        TimeEntryDate BETWEEN paypdatefrom AND paypdateto
    GROUP BY EmployeeID
) agf
ON IFNULL(agf.RowID, 1) > 0 AND
    agf.EmployeeID=paystub.EmployeeID
LEFT JOIN thirteenthmonthpay
ON thirteenthmonthpay.OrganizationID = paystub.OrganizationID AND
    thirteenthmonthpay.PaystubID = IF(psi_undeclared, paystubactual.RowID, paystub.RowID)
WHERE paystub.OrganizationID = ps_OrganizationID AND
    (paystub.PayFromDate >= paypdatefrom OR paystub.PayToDate >= paypdatefrom) AND
    (paystub.PayFromDate <= paypdateto OR paystub.PayToDate <= paypdateto) AND
    LENGTH(IFNULL(e.ATMNo, '')) = IF(strSalaryDistrib = 'Cash', 0, LENGTH(IFNULL(e.ATMNo, ''))) AND
    -- If employee is paid monthly or daily, employee should have worked for the pay period to appear
    IF(e.EmployeeType IN ('Monthly', 'Daily'), paystub.RegularHours > 0, TRUE)
ORDER BY d.Name, e.LastName;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
