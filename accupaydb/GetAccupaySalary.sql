/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP PROCEDURE IF EXISTS `GetAccupaySalary`;
DELIMITER //
CREATE PROCEDURE `GetAccupaySalary`(
	IN `OrgId` INT,
	IN `PeriodDateFrom` DATE,
	IN `PeriodDateTo` DATE
)
BEGIN

SET @orgId=OrgId;
SET @datefrom=PeriodDateFrom; SET @dateto=PeriodDateTo;

DROP TEMPORARY TABLE IF EXISTS `accupaysalarytable`;
CREATE TEMPORARY TABLE IF NOT EXISTS `accupaysalarytable`
SELECT
es.*
FROM employeesalary es
WHERE es.OrganizationID=@orgId
ORDER BY es.EmployeeID, es.EffectiveDateFrom
;

SET @_count=0;
SET @_curdate='1775-01-01';
SET @_isNewDay=FALSE;
SET @_eId=0;
SET @_isNewEmployee=FALSE;

DROP TEMPORARY TABLE IF EXISTS `accupaysalary`;
CREATE TEMPORARY TABLE IF NOT EXISTS `accupaysalary`
SELECT
i.*,
@_isNewEmployee:=i.EmployeeID != @_eId `IsNewEmployee`,
IF(@_isNewEmployee, @_eId:=i.EmployeeID, @_eId) `SetEmployeeId`,
IF(@_isNewEmployee, @_count:=0, @_count) `ResetCounter`,
@_isNewDay:=i.EffectiveDateFrom != IFNULL(@_curdate, '1775-01-01') `IsNewDay`,
@_curdate:=IF(@_isNewDay, i.EffectiveDateFrom, NULL) `SetCurDate`,

@_count:=IF(@_curdate IS NOT NULL, @_count+1, 0) `SetNewDay`

FROM `accupaysalarytable` i
;

END//
DELIMITER ;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
