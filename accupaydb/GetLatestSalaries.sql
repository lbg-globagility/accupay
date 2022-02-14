/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP PROCEDURE IF EXISTS `GetLatestSalaries`;
DELIMITER //
CREATE PROCEDURE `GetLatestSalaries`(
	IN `organizationId` INT
)
BEGIN
DECLARE rowCount INT DEFAULT 0;
DECLARE iterator INT DEFAULT 0;

SET @salaryIds='';
SET @orgId=organizationId;

SELECT
COUNT(i.RowID) `Count`
FROM (SELECT es.RowID
		FROM employeesalary es
		WHERE es.OrganizationID=@orgId
		GROUP BY es.EmployeeID
		) i
INTO rowCount
;

SET @eId=0;

WHILE iterator < rowCount DO
	
	SELECT
	es.EmployeeID
	FROM employeesalary es
	INNER JOIN employee e ON e.RowID=es.EmployeeID
	WHERE es.OrganizationID=@orgId
	ORDER BY CONCAT(e.LastName, e.FirstName), es.EffectiveDateFrom DESC
	LIMIT iterator, 1
	INTO @eId
	;
	
	SET @salaryId=0;
	
	SELECT
	es.RowID
	FROM employeesalary es
	INNER JOIN employee e ON e.RowID=es.EmployeeID
	WHERE es.OrganizationID=@orgId
	AND es.EmployeeID=@eId
	ORDER BY CONCAT(e.LastName, e.FirstName), es.EffectiveDateFrom DESC
	LIMIT 1
	INTO @salaryId
	;

	SET @salaryIds=CONCAT_WS(',', @salaryIds, @salaryId);
	
	SET iterator = iterator + 1;
END WHILE;

DROP TEMPORARY TABLE IF EXISTS `latestsalaries`;
CREATE TEMPORARY TABLE IF NOT EXISTS `latestsalaries`
SELECT
es.*
FROM employeesalary es
WHERE FIND_IN_SET(es.RowID, @salaryIds) > 0
;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
