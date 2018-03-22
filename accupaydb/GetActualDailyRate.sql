/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `GetActualDailyRate`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GetActualDailyRate`(`EmpID` INT, `OrgID` INT, `paramDate` DATE) RETURNS decimal(15,4)
    DETERMINISTIC
BEGIN

DECLARE PAYFREQUENCY_SEMIMONTHLY INT(11) DEFAULT 1;
DECLARE PAYFREQUENCY_MONTHLY INT(11) DEFAULT 2;
DECLARE PAYFREQUENCY_DAILY INT(11) DEFAULT 3;
DECLARE PAYFREQUENCY_WEEKLY INT(11) DEFAULT 4;

DECLARE hoursofduty DECIMAL(10,4);

DECLARE empBasicPay DECIMAL(15, 4);
DECLARE dailyrate DECIMAL(15, 4);

DECLARE numofweekthisyear INT(11) DEFAULT 53;

DECLARE shiftRowID INT(11);

DECLARE PayFreqID INT(11);

DECLARE emptype VARCHAR(100);

DECLARE _workDaysPerYear DECIMAL(10, 4);
DECLARE workDaysInMonth DECIMAL(10, 4);

DECLARE basicSalary DECIMAL(15, 4);
DECLARE allowanceSalary DECIMAL(15, 4);
DECLARE fullSalary DECIMAL(15, 4);

DECLARE month_count_peryear INT(11) DEFAULT 12;

SELECT ShiftID
FROM employeeshift
WHERE EmployeeID = EmpID AND
    OrganizationID = OrgID AND
    paramDate BETWEEN DATE(COALESCE(EffectiveFrom, DATE_FORMAT(CURRENT_TIMESTAMP(), '%Y-%m-%d'))) AND DATE(COALESCE(EffectiveTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 3 MONTH))) AND
    DATEDIFF(paramDate, EffectiveFrom) >= 0 AND
    COALESCE(RestDay, 0) = 0
ORDER BY DATEDIFF(DATE_FORMAT(paramDate, '%Y-%m-%d'), EffectiveFrom)
LIMIT 1
INTO shiftRowID;

SELECT COMPUTE_TimeDifference(TimeFrom, TimeTo) * 1.0
FROM shift
WHERE RowID = shiftRowID
INTO hoursofduty;

IF hoursofduty > 8.00 OR hoursofduty IS NULL OR hoursofduty <= 0 THEN
    IF hoursofduty IS NULL THEN
        SET hoursofduty = 8;
    ELSE
        SET hoursofduty = hoursofduty - 1;
    END IF;
END IF;

SELECT
    BasicPay * 1.0,
    Salary * 1.0,
    UndeclaredSalary
FROM employeesalary
WHERE EmployeeID = EmpID AND
    OrganizationID = OrgID AND
    paramDate BETWEEN EffectiveDateFrom AND IFNULL(EffectiveDateTo,paramDate) AND
    DATEDIFF(paramDate,EffectiveDateFrom) >= 0
ORDER BY DATEDIFF(DATE_FORMAT(paramDate,'%Y-%m-%d'), EffectiveDateFrom)
LIMIT 1
INTO
    empBasicPay,
    basicSalary,
    allowanceSalary;

SET fullSalary = basicSalary + allowanceSalary;

SET empBasicPay = COALESCE(empBasicPay, 0);

SELECT
    PayFrequencyID,
    EmployeeType,
    WorkDaysPerYear
FROM employee
WHERE RowID = EmpID
INTO
    PayFreqID,
    emptype,
    _workDaysPerYear;

SELECT
    PayFrequencyID,
    WEEKOFYEAR(LAST_DAY(CONCAT(YEAR(paramDate), '-12-01')))
FROM organization
WHERE RowID = OrgID
INTO
    PayFreqID,
    numofweekthisyear;

IF emptype IN ('Fixed', 'Monthly') THEN

    IF PayFreqID = PAYFREQUENCY_SEMIMONTHLY THEN

        SET workDaysInMonth = _workDaysPerYear / 12.0;

        SET dailyrate = fullSalary / workDaysInMonth;

    ELSEIF PayFreqID = PAYFREQUENCY_MONTHLY THEN

        SELECT (basicSalary / (e.WorkDaysPerYear / month_count_peryear)) `Result`
        FROM employee e
        WHERE e.RowID = EmpID
        INTO dailyrate;

    ELSEIF PayFreqID = PAYFREQUENCY_DAILY THEN

        SET dailyrate = empBasicPay;

    ELSEIF PayFreqID = PAYFREQUENCY_WEEKLY THEN

        SET dailyrate = IF(
            DAY(
                LAST_DAY(
                    ADDDATE(
                        MAKEDATE(YEAR(paramDate), 1),
                        INTERVAL 1 MONTH
                    )
                )
            ) <= 28,
            (empBasicPay * numofweekthisyear) / _workDaysPerYear,
            (empBasicPay * numofweekthisyear) / (_workDaysPerYear + 1)
        );

    END IF;

ELSEIF emptype = 'Daily' THEN

    SET dailyrate = fullSalary;

ELSEIF emptype = 'Hourly' THEN

    SET dailyrate = empBasicPay * hoursofduty;

END IF;

RETURN dailyrate;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
