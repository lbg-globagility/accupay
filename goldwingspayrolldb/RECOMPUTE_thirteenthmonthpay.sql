/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RECOMPUTE_thirteenthmonthpay`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RECOMPUTE_thirteenthmonthpay`(
	IN `OrganizID` INT,
	IN `PayPRowID` INT,
	IN `UserRowID` INT


)
    DETERMINISTIC
BEGIN

DECLARE ispayperiodendofmonth TEXT;

DECLARE newvalue DECIMAL(11,6);

DECLARE payp_month TEXT;

DECLARE payp_year INT;

DECLARE emppayfreqID INT(11);

DECLARE paypmonthlyID VARCHAR(50);

DECLARE last_date DATE;

DECLARE month_firstdate DATE;

DECLARE pf_div INT;

DECLARE overtimeRate DECIMAL(10, 2);

DECLARE HOURS_IN_A_WORKDAY INT DEFAULT 8;

SELECT
    pyp.`Half`,
    pyp.`Month`,
    pyp.`Year`,
    pyp.TotalGrossSalary,
    pyp.PayFromDate,
    pyp.PayToDate,
    MONTH( SUBDATE(MAKEDATE(YEAR(CURDATE()), 1), INTERVAL 1 DAY) )
FROM payperiod pyp
INNER JOIN payfrequency pf ON pf.RowID=pyp.TotalGrossSalary
WHERE pyp.RowID=PayPRowID
INTO    ispayperiodendofmonth
        ,payp_month
        ,payp_year
        ,emppayfreqID
        ,month_firstdate
        ,last_date
        ,pf_div;

SELECT payrate.OvertimeRate
FROM payrate
WHERE payrate.Date = last_date
    AND payrate.OrganizationID = OrganizID
INTO overtimeRate;

SELECT GROUP_CONCAT(RowID)
FROM payperiod pp
WHERE pp.OrganizationID=OrganizID
    AND pp.TotalGrossSalary=1
    AND pp.`Year`=payp_year
    AND pp.`Month`=payp_month
ORDER BY
    pp.PayFromDate DESC,
    pp.PayToDate DESC
INTO paypmonthlyID;

IF ispayperiodendofmonth IS NOT NULL THEN
    INSERT INTO thirteenthmonthpay
    (
        RowID,
        OrganizationID,
        Created,
        CreatedBy,
        PaystubID,
        Amount
    )
    SELECT
        GET_prev13monthRowID(OrganizID, PayPRowID, ii.EmployeeID),
        OrganizID,
        CURRENT_TIMESTAMP(),
        UserRowID,
        ps.RowID,
        (ii.BasicAmount / pf_div)
    FROM
    (
            -- Get the summation of basic pay for the daily salaried employees.
            SELECT
                (
                    timeEntrySummary.BasicAmount +
                    (timeEntrySummary.TotalOvertimeHours * (es.UndeclaredSalary / HOURS_IN_A_WORKDAY) * overtimeRate) +
                    timeEntrySummary.TotalLeavePay
                ) AS BasicAmount,
                e.RowID AS EmployeeID,
                'employeetimeentry' AS SourceOfAmount
            FROM employee e
            INNER JOIN (
                    SELECT
                        ete.EmployeeID AS EmployeeID,
                        SUM(ete.BasicDayPay) AS BasicAmount,
                        SUM(ete.OvertimeHoursWorked) AS TotalOvertimeHours,
                        SUM(ete.Leavepayment) AS TotalLeavePay
                    FROM employeetimeentryactual ete
                    LEFT JOIN employeeshift esh
                        ON esh.RowID = ete.EmployeeShiftID
                        AND esh.RestDay = FALSE
                    WHERE ete.OrganizationID = OrganizID
                        AND `Date` BETWEEN month_firstdate AND last_date
                        AND esh.RestDay = 0
                    GROUP BY ete.EmployeeID
                ) timeEntrySummary
                ON timeEntrySummary.EmployeeID = e.RowID
            INNER JOIN employeesalary es
                ON es.EmployeeID = e.RowID
                AND last_date BETWEEN es.EffectiveDateFrom AND IFNULL(es.EffectiveDateTo, last_date)
            WHERE e.PayFrequencyID = emppayfreqID
                AND e.EmployeeType = 'Daily'
                AND e.OrganizationID = OrganizID
                AND e.EmploymentStatus NOT IN ('Contractual', 'SERVICE CONTRACT')
        UNION
            SELECT
                (
                    timeEntrySummary.BasicAmount +
                    -- (timeEntrySummary.TotalOvertimeHours * (es.UndeclaredSalary / HOURS_IN_A_WORKDAY) * overtimeRate) +
                    timeEntrySummary.TotalLeavePay
                ) AS BasicAmount,
                e.RowID AS EmployeeID,
                'employeetimeentry' AS SourceOfAmount
            FROM employee e
            INNER JOIN (
                    SELECT
                        ete.EmployeeID AS EmployeeID,
                        SUM(ete.BasicDayPay) AS BasicAmount,
                        SUM(ete.OvertimeHoursWorked) AS TotalOvertimeHours,
                        SUM(ete.Leavepayment) AS TotalLeavePay
                    FROM employeetimeentry ete
                    LEFT JOIN employeeshift esh
                        ON esh.RowID = ete.EmployeeShiftID
                        AND esh.RestDay = FALSE
                    WHERE ete.OrganizationID = OrganizID
                        AND `Date` BETWEEN month_firstdate AND last_date
                        AND esh.RestDay = 0
                    GROUP BY ete.EmployeeID
                ) timeEntrySummary
                ON timeEntrySummary.EmployeeID = e.RowID
            INNER JOIN employeesalary es
                ON (
                    es.EmployeeID = e.RowID AND
                    last_date BETWEEN es.EffectiveDateFrom AND IFNULL(es.EffectiveDateTo, last_date)
                )
            WHERE e.PayFrequencyID = emppayfreqID
                AND e.EmployeeType = 'Daily'
                AND e.OrganizationID = OrganizID
                AND e.EmploymentStatus IN ('Contractual', 'SERVICE CONTRACT')
        UNION
            -- Get the summation of basic pay for the monthly and fixed salaried employees.
            SELECT
                (es.TrueSalary - IFNULL(et.LessAmount,0)) AS BasicAmount,
                e.RowID AS EmployeeID,
                'employeeallowance' AS SourceOfAmount
            FROM employee e
            INNER JOIN employeesalary es
                ON es.EmployeeID=e.RowID
                AND last_date BETWEEN es.EffectiveDateFrom AND IFNULL(es.EffectiveDateTo, last_date)
            LEFT JOIN (
                    SELECT
                        EmployeeID,
                        SUM(HoursLateAmount + UndertimeHoursAmount + Absent) AS LessAmount
                    FROM employeetimeentryactual
                    WHERE
                        (HoursLateAmount + UndertimeHoursAmount + Absent) > 0
                        AND OrganizationID=OrganizID
                        AND `Date` BETWEEN month_firstdate AND last_date
                    GROUP BY EmployeeID
                ) et
                ON et.EmployeeID=e.RowID
            WHERE e.PayFrequencyID=emppayfreqID
                AND e.OrganizationID=OrganizID
                AND e.EmployeeType IN ('Monthly','Fixed')
    ) ii
    INNER JOIN paystub ps
        ON ps.OrganizationID=OrganizID
        AND ps.PayPeriodID=PayPRowID
        AND ps.EmployeeID=ii.EmployeeID
    LEFT JOIN (
            SELECT
                psi.PayStubID,
                SUM(psi.PayAmount) AS PayAmount
            FROM paystubitem psi
            INNER JOIN product p
                ON p.RowID=psi.ProductID
                AND p.`Category`='Allowance Type'
                AND p.AllocateBelowSafetyFlag='1'
                AND p.OrganizationID=psi.OrganizationID
            WHERE psi.PayStubID IS NOT NULL
                AND psi.OrganizationID=OrganizID
                AND psi.PayAmount != 0
            GROUP BY psi.PayStubID
        ) ea
        ON ea.PayStubID = ps.RowID
    ON DUPLICATE KEY
    UPDATE
        LastUpd=CURRENT_TIMESTAMP(),
        LastUpdBy=UserRowID,
        Amount=ii.BasicAmount / pf_div;
END IF;

UPDATE employeeloanschedule els
INNER JOIN scheduledloansperpayperiod slp
    ON slp.EmployeeLoanRecordID=els.RowID
    AND slp.PayPeriodID=PayPRowID
    AND slp.OrganizationID=OrganizID
SET
    els.LastUpd = CURRENT_TIMESTAMP(),
    els.LastUpdBy = UserRowID,
    els.LoanPayPeriodLeft = slp.LoanPayPeriodLeft,
    els.TotalBalanceLeft = slp.TotalBalanceLeft;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
