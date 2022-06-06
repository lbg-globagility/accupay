/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP PROCEDURE IF EXISTS `GetMultipleSalaries`;
DELIMITER //
CREATE PROCEDURE `GetMultipleSalaries`(
	IN `orgId` INT,
	IN `periodDateFrom` DATE,
	IN `periodDateTo` DATE
)
BEGIN

DECLARE employeeCount, employeeSalaryCount,
	salaryCount,
	empIndex,
	employeeSalaryIndex,
	salIndex INT DEFAULT 0;

DROP TEMPORARY TABLE IF EXISTS `temposalary`;
CREATE TEMPORARY TABLE IF NOT EXISTS `temposalary`
SELECT 0 `RowID`, 1 `OrganizationID`, CURDATE() `Created`, 0 `CreatedBy`, CURRENT_TIMESTAMP() `LastUpd`, 0 `LastUpdBy`, 0 `EmployeeID`, 1100.0000 `PhilHealthDeduction`, 100.00 `HDMFAmount`, 120000.00 `TrueSalary`, 25000.00 `BasicPay`, 50000.00 `Salary`, 700000.00 `UndeclaredSalary`, 100000.00 `BasicDailyPay`, 0.00 `BasicHourlyPay`, 0 `NoofDependents`, '' `MaritalStatus`, 0 `PositionID`, CURDATE() `EffectiveDateFrom`, CURDATE() `EffectiveDateTo`, 0 `OverrideDiscardSSSContrib`, 0 `OverrideDiscardPhilHealthContrib`, 1 `DoPaySSSContribution`, 1 `AutoComputePhilHealthContribution`, 1 `AutoComputeHDMFContribution`, 0 `IsMinimumWage`
;

SELECT
COUNT(e.RowID)
FROM employee e
INNER JOIN organization og ON og.RowID=e.OrganizationID AND og.NoPurpose=FALSE
WHERE e.OrganizationID=orgId
INTO employeeCount;

SELECT
COUNT(es.RowID)
FROM employeesalary es
INNER JOIN organization og ON og.RowID=es.OrganizationID AND og.NoPurpose=FALSE
INNER JOIN employee e ON e.RowID=es.EmployeeID
WHERE es.OrganizationID=orgId
INTO salaryCount;

SET @eId=0;

WHILE empIndex < employeeCount DO
	SELECT
	e.RowID
	FROM employee e
	INNER JOIN organization og ON og.RowID=e.OrganizationID AND og.NoPurpose=FALSE
	WHERE e.OrganizationID=orgId
	ORDER BY e.LastName, e.FirstName
	LIMIT empIndex, 1
	INTO @eId;
	
	SELECT
	COUNT(es.RowID)
	FROM employeesalary es
	INNER JOIN organization og ON og.RowID=es.OrganizationID AND og.NoPurpose=FALSE
	INNER JOIN employee e ON e.RowID=es.EmployeeID
	WHERE es.OrganizationID=orgId
	AND es.EmployeeID=@eId
	INTO employeeSalaryCount
	;
	
	SET @previousEffectiveDateFrom=NULL;
	SET employeeSalaryIndex=0;
	
	WHILE employeeSalaryIndex < employeeSalaryCount DO
		
		INSERT INTO `temposalary` (`RowID`, `OrganizationID`, `Created`, `CreatedBy`, `LastUpd`, `LastUpdBy`, `EmployeeID`, `PhilHealthDeduction`, `HDMFAmount`, `TrueSalary`, `BasicPay`, `Salary`, `UndeclaredSalary`, `BasicDailyPay`, `BasicHourlyPay`, `NoofDependents`, `MaritalStatus`, `PositionID`, `EffectiveDateFrom`, `OverrideDiscardSSSContrib`, `OverrideDiscardPhilHealthContrib`, `DoPaySSSContribution`, `AutoComputePhilHealthContribution`, `AutoComputeHDMFContribution`, `IsMinimumWage`, `EffectiveDateTo`)
		SELECT
		es.`RowID`, es.`OrganizationID`, es.`Created`, es.`CreatedBy`, CURRENT_TIMESTAMP() `LastUpd`, 0 `LastUpdBy`, es.`EmployeeID`, es.`PhilHealthDeduction`, es.`HDMFAmount`, es.`TrueSalary`, es.`BasicPay`, es.`Salary`, es.`UndeclaredSalary`, es.`BasicDailyPay`, es.`BasicHourlyPay`, 0 `NoofDependents`, '' `MaritalStatus`, 0 `PositionID`, es.`EffectiveDateFrom`, es.`OverrideDiscardSSSContrib`, es.`OverrideDiscardPhilHealthContrib`, es.`DoPaySSSContribution`, es.`AutoComputePhilHealthContribution`, es.`AutoComputeHDMFContribution`, es.`IsMinimumWage`,
		IF(@previousEffectiveDateFrom IS NULL, GREATEST(es.`EffectiveDateFrom`, periodDateTo), SUBDATE(@previousEffectiveDateFrom, INTERVAL 1 DAY))
		FROM employeesalary es
		WHERE es.OrganizationID=orgId
		AND es.EmployeeID=@eId
		ORDER BY es.EffectiveDateFrom DESC
		LIMIT employeeSalaryIndex, 1
		;
		
		SELECT
		es.EffectiveDateFrom
		FROM employeesalary es
		WHERE es.OrganizationID=orgId
		AND es.EmployeeID=@eId
		ORDER BY es.EffectiveDateFrom DESC
		LIMIT employeeSalaryIndex, 1
		INTO @previousEffectiveDateFrom;
		
		SET employeeSalaryIndex = employeeSalaryIndex + 1;
	END WHILE;
	
	SET empIndex = empIndex + 1;
END WHILE;

/*SELECT
*
FROM `temposalary`
WHERE ((@datefrom BETWEEN EffectiveDateFrom AND EffectiveDateTo) OR
	@dateto BETWEEN EffectiveDateFrom AND EffectiveDateTo);*/

END//
DELIMITER ;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
