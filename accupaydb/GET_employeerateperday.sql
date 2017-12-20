/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `GET_employeerateperday`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GET_employeerateperday`(`EmpID` INT, `OrgID` INT, `paramDate` DATE) RETURNS DECIMAL(15, 4)
    DETERMINISTIC
BEGIN

DECLARE returnvalue DECIMAL(15, 4);

DECLARE hoursofduty DECIMAL(15, 4);

DECLARE empBasicPay DECIMAL(15, 4);

DECLARE dailyrate DECIMAL(15, 4);

DECLARE rateperhour DECIMAL(15, 4);

DECLARE numofweekthisyear INT(11) DEFAULT 53;

DECLARE shiftRowID INT(11);

DECLARE PayFreqID INT(11);

DECLARE minnumday DECIMAL(11,7);

DECLARE emptype VARCHAR(100);

DECLARE timedifference TIME;

DECLARE org_workdaysofyear INT(11);

DECLARE emp_sal DECIMAL(11,6);

DECLARE month_count_peryear INT(11) DEFAULT 12;

SELECT ShiftID
FROM employeeshift
WHERE EmployeeID = EmpID AND
      OrganizationID = OrgID AND
      paramDate BETWEEN DATE(COALESCE(EffectiveFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 3 MONTH))) AND
      DATEDIFF(paramDate,EffectiveFrom) >= 0 AND
      COALESCE(RestDay, 0) = 0
ORDER BY DATEDIFF(DATE_FORMAT(paramDate, '%Y-%m-%d'), EffectiveFrom)
LIMIT 1
INTO shiftRowID;

SELECT SUBSTRING_INDEX(TIMEDIFF(TimeFrom, IF(TimeFrom > TimeTo, ADDTIME(TimeTo, '24:00:00'), TimeTo)), '-', -1)
FROM shift
WHERE RowID = shiftRowID
INTO timedifference;

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
    Salary * 1.0
FROM employeesalary
WHERE EmployeeID = EmpID AND
    OrganizationID = OrgID AND
    paramDate BETWEEN EffectiveDateFrom AND IFNULL(EffectiveDateTo,paramDate) AND
    DATEDIFF(paramDate,EffectiveDateFrom) >= 0
ORDER BY DATEDIFF(DATE_FORMAT(paramDate,'%Y-%m-%d'),EffectiveDateFrom)
LIMIT 1
INTO
    empBasicPay,
    emp_sal;

SET empBasicPay = COALESCE(empBasicPay,0);

SELECT PayFrequencyID, EmployeeType
FROM employee
WHERE RowID=EmpID
INTO PayFreqID, emptype;

SELECT
    PayFrequencyID,
    WEEKOFYEAR(LAST_DAY(CONCAT(YEAR(paramDate),'-12-01')))
FROM organization
WHERE RowID=OrgID
INTO
    PayFreqID,
    numofweekthisyear;

IF DAY(paramDate) <= 15 THEN
    SET minnumday = 15;
ELSE
    SET minnumday = DAY(LAST_DAY(paramDate)) - 15;
END IF;

SELECT GET_empworkdaysperyear(EmpID) * 1.0
INTO org_workdaysofyear;

IF emptype IN ('Fixed','Monthly') THEN

    IF PayFreqID = 1 THEN

        SET dailyrate = org_workdaysofyear / 12.0;
        SET dailyrate = dailyrate / 2.0;
        SET dailyrate = empBasicPay / dailyrate;
        SET dailyrate = dailyrate * 1.0;
        SET dailyrate = ROUND(dailyrate, 1);

    ELSEIF PayFreqID = 2 THEN

        SELECT (emp_sal / (e.WorkDaysPerYear / month_count_peryear)) `Result`
		  FROM employee e
		  WHERE e.RowID=EmpID
		  INTO dailyrate;

    ELSEIF PayFreqID = 3 THEN

        SET dailyrate = empBasicPay;

    ELSEIF PayFreqID = 4 THEN

        SET dailyrate = IF(
            DAY(
                LAST_DAY(
                    ADDDATE(
                        MAKEDATE(YEAR(paramDate), 1),
                        INTERVAL 1 MONTH
                    )
                )
            ) <= 28,
            (empBasicPay * numofweekthisyear) / org_workdaysofyear,
            (empBasicPay * numofweekthisyear) / (org_workdaysofyear + 1)
        );

    END IF;

ELSEIF emptype = 'Daily' THEN

    SET dailyrate = empBasicPay;

ELSEIF emptype = 'Hourly' THEN

    SET rateperhour = empBasicPay * hoursofduty;
    SET dailyrate = rateperhour;

END IF;

RETURN dailyrate;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
