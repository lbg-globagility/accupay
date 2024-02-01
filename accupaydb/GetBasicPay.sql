/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `GetBasicPay`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GetBasicPay`(`employeeID` INT, `payDateFrom` DATE, `payDateTo` DATE, `isActual` TINYINT(1), `workHours` DECIMAL(10, 4)) RETURNS decimal(15,4)
    DETERMINISTIC
BEGIN

DECLARE PAYFREQUENCY_SEMIMONTHLY INT(11) DEFAULT 1;
DECLARE PAYFREQUENCY_MONTHLY INT(11) DEFAULT 2;
DECLARE PAYFREQUENCY_DAILY INT(11) DEFAULT 3;
DECLARE PAYFREQUENCY_WEEKLY INT(11) DEFAULT 4;

DECLARE MONTHS_IN_YEAR INT(10) DEFAULT 12;
DECLARE SEMIMONTHLY_PAYPERIODS_PER_MONTH INT(10) DEFAULT 2;

DECLARE salary DECIMAL(15, 4) DEFAULT 0.0;
DECLARE workDaysPerYear DECIMAL(10, 4) DEFAULT 0;
DECLARE workDaysPerMonth DECIMAL(10, 4);
DECLARE workDaysPerPayPeriod DECIMAL(10, 4);
DECLARE employeeType VARCHAR(50);
DECLARE payFrequency INT(10) DEFAULT 0;

SELECT
    ee.EmployeeType,
    ee.PayFrequencyID,
    ee.WorkDaysPerYear
FROM employee ee
WHERE ee.RowID = employeeID
INTO
    employeeType,
    payFrequency,
    workDaysPerYear;

SELECT IF(
    isActual,
    es.TrueSalary,
    es.Salary
)
FROM employeesalary es
WHERE es.EmployeeID = employeeID
AND es.EffectiveDateFrom  <= payDateTo
ORDER BY es.EffectiveDateFrom DESC
LIMIT 1
INTO salary;

IF (employeeType = 'Monthly') OR (employeeType = 'Fixed') THEN

    IF payFrequency = PAYFREQUENCY_MONTHLY THEN
        RETURN salary;
    ELSEIF payFrequency = PAYFREQUENCY_SEMIMONTHLY THEN
        RETURN salary / SEMIMONTHLY_PAYPERIODS_PER_MONTH;
    ELSEIF payFrequency = PAYFREQUENCY_WEEKLY THEN
        SIGNAL SQLSTATE '01000'
            SET MESSAGE_TEXT = '`GetBasicPay()` has not been implemented for weekly pay frequency of monthly/fixed employees.';
    ELSEIF payFrequency = PAYFREQUENCY_DAILY THEN
        SIGNAL SQLSTATE '01000'
            SET MESSAGE_TEXT = '`GetBasicPay()` has not been implemented for daily pay frequency of monthly/fixed employees.';
    END IF;

ELSEIF employeeType = 'Daily' THEN

    SET workDaysPerMonth = workDaysPerYear / MONTHS_IN_YEAR;

    IF payFrequency = PAYFREQUENCY_MONTHLY THEN
        RETURN (salary / 8) * workHours;
    ELSEIF payFrequency = PAYFREQUENCY_SEMIMONTHLY THEN
        SET workDaysPerPayPeriod = workDaysPerMonth / SEMIMONTHLY_PAYPERIODS_PER_MONTH;

        RETURN (salary / 8) * workHours;
    ELSEIF payFrequency = PAYFREQUENCY_WEEKLY THEN
	     
        RETURN (salary / 8) * workHours;
        /*SIGNAL SQLSTATE '01000'
            SET MESSAGE_TEXT = '`GetBasicPay()` has not been implemented for weekly pay frequency of daily employees.';*/
    ELSEIF payFrequency = PAYFREQUENCY_DAILY THEN
        SIGNAL SQLSTATE '01000'
            SET MESSAGE_TEXT = '`GetBasicPay()` has not been implemented for daily pay frequency of daily employees.';
    END IF;

END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
