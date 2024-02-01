/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP PROCEDURE IF EXISTS `GetLeaveTransaction`;
DELIMITER //
CREATE PROCEDURE `GetLeaveTransaction`(
	IN `orgId` INT,
	IN `periodId` INT
)
BEGIN

SET @firstDatefrom='';
SET @datefrom='2022-01-21'; SET @dateto='2022-02-05';
SET @orgId = orgId;
SET @ppId = periodId;
SET @_year = 2022;
SET @_count=0;

SELECT
COUNT(ps.RowID),
#NULL,
MIN(pp1.PayFromDate),
ps.PayFromDate, ps.PayToDate,
pp.Year
FROM paystub ps
INNER JOIN payperiod pp ON pp.RowID=ps.PayPeriodID AND pp.RowID=@ppId
INNER JOIN payperiod pp1 ON pp1.OrganizationID=pp.OrganizationID AND pp1.Year=pp.Year AND pp1.TotalGrossSalary=pp.TotalGrossSalary AND pp1.OrdinalValue=1
INTO @_count, @firstDatefrom, @datefrom, @dateto, @_year
;

SELECT
DATE_FORMAT(lr.StartPeriodDate, CONCAT(@_year, '-%m-%d')) `StartPeriod`
FROM leavereset lr
WHERE lr.OrganizationId=orgId
AND lr.EffectiveDate <= @datefrom
ORDER BY lr.EffectiveDate DESC
LIMIT 1
INTO @firstDatefrom
;

SET @vacationLtIds='';
SET @sickLtIds='';
SET @othersLtIds='';
SET @parentalLtIds='';

SET @iter=0;
SET @eIds='';

WHILE @iter < @_count DO
	SET @eId=0;
	
	SELECT
	ps.EmployeeID
	FROM paystub ps
	INNER JOIN employee e ON e.RowID=ps.EmployeeID
	WHERE ps.OrganizationID=@orgId
	AND ps.PayPeriodID=@ppId
	AND FIND_IN_SET(ps.EmployeeID, @eIds) = 0
	ORDER BY CONCAT(e.LastName, e.FirstName)
	LIMIT 1
	INTO @eId
	;
	
	SET @eIds=CONCAT_WS(',', @eIds, @eId);
	
	SET @vacLtIds='';
	SELECT
#	GROUP_CONCAT(lt.RowID)
	lt.RowID
	FROM leavetransaction lt
	INNER JOIN leaveledger ll ON ll.RowID=lt.LeaveLedgerID
	INNER JOIN product p ON p.RowID=ll.ProductID AND p.PartNo='Vacation leave'
	INNER JOIN category c ON c.RowID=p.CategoryID AND c.CategoryName='Leave Type'
	WHERE lt.EmployeeID=@eId
	AND lt.TransactionDate BETWEEN @firstDatefrom AND @dateto
	ORDER BY lt.TransactionDate DESC
	LIMIT 1
	INTO @vacLtIds
	;
	SET @vacationLtIds=CONCAT_WS(',', @vacationLtIds, @vacLtIds);
	
	SET @sicLtIds='';
	SELECT
#	GROUP_CONCAT(lt.RowID)
	lt.RowID
	FROM leavetransaction lt
	INNER JOIN leaveledger ll ON ll.RowID=lt.LeaveLedgerID
	INNER JOIN product p ON p.RowID=ll.ProductID AND p.PartNo='Sick leave'
	INNER JOIN category c ON c.RowID=p.CategoryID AND c.CategoryName='Leave Type'
	WHERE lt.EmployeeID=@eId
	AND lt.TransactionDate BETWEEN @firstDatefrom AND @dateto
	ORDER BY lt.TransactionDate DESC
	LIMIT 1
	INTO @sicLtIds
	;
	SET @sickLtIds=CONCAT_WS(',', @sickLtIds, @sicLtIds);
	
	SET @othLtIds='';
	SELECT
#	GROUP_CONCAT(lt.RowID)
	lt.RowID
	FROM leavetransaction lt
	INNER JOIN leaveledger ll ON ll.RowID=lt.LeaveLedgerID
	INNER JOIN product p ON p.RowID=ll.ProductID AND p.PartNo='Others'
	INNER JOIN category c ON c.RowID=p.CategoryID AND c.CategoryName='Leave Type'
	WHERE lt.EmployeeID=@eId
	AND lt.TransactionDate BETWEEN @firstDatefrom AND @dateto
	ORDER BY lt.TransactionDate DESC
	LIMIT 1
	INTO @othLtIds
	;
	SET @othersLtIds=CONCAT_WS(',', @othersLtIds, @othLtIds);
	
	SET @parLtIds='';
	SELECT
#	GROUP_CONCAT(lt.RowID)
	lt.RowID
	FROM leavetransaction lt
	INNER JOIN leaveledger ll ON ll.RowID=lt.LeaveLedgerID
	INNER JOIN product p ON p.RowID=ll.ProductID AND p.PartNo='Maternity/paternity leave'
	INNER JOIN category c ON c.RowID=p.CategoryID AND c.CategoryName='Leave Type'
	WHERE lt.EmployeeID=@eId
	AND lt.TransactionDate BETWEEN @firstDatefrom AND @dateto
	ORDER BY lt.TransactionDate DESC
	LIMIT 1
	INTO @parLtIds
	;
	SET @parentalLtIds=CONCAT_WS(',', @parentalLtIds, @parLtIds);
	
	SET @iter = @iter + 1;
END WHILE;

SET @_ltIds=CONCAT_WS(',', @vacationLtIds, @sickLtIds, @othersLtIds, @parentalLtIds);

DROP TEMPORARY TABLE IF EXISTS `currenttimeentry`;
CREATE TEMPORARY TABLE IF NOT EXISTS `currenttimeentry`
SELECT
et.EmployeeID,
SUM(et.VacationLeaveHours) `VacationLeaveHours`,
SUM(et.SickLeaveHours) `SickLeaveHours`,
SUM(et.OtherLeaveHours) `OtherLeaveHours`,
SUM(et.MaternityLeaveHours) `MaternityLeaveHours`
FROM employeetimeentry et
WHERE et.OrganizationID=@orgId
AND FIND_IN_SET(et.EmployeeID, @eIds)
AND (et.`Date` BETWEEN @datefrom AND @dateto)
#AND (et.`Date` BETWEEN @firstDatefrom AND @dateto)
AND (et.VacationLeaveHours + et.SickLeaveHours + et.OtherLeaveHours + et.MaternityLeaveHours) > 0
GROUP BY et.EmployeeID
;

DROP TEMPORARY TABLE IF EXISTS `currentleavetransaction`;
CREATE TEMPORARY TABLE IF NOT EXISTS `currentleavetransaction`
SELECT
#@_ltIds `Result`,
#@_count, @firstDatefrom, @datefrom, @dateto, @orgId, @eIds,
#lt.RowID,lt.OrganizationID,lt.Created,lt.CreatedBy,lt.LastUpd,lt.LastUpdBy,lt.EmployeeID,lt.ReferenceID,lt.LeaveLedgerID,lt.PayPeriodID,lt.PaystubID,lt.TransactionDate,lt.Description,lt.`Type`,ROUND(lt.Balance / 8, 2) `Balance`,lt.Amount,lt.Comments,
lt.`RowID`, lt.`OrganizationID`, lt.`Created`, lt.`CreatedBy`, lt.`LastUpd`, lt.`LastUpdBy`, lt.`EmployeeID`, lt.`ReferenceID`, lt.`LeaveLedgerID`, lt.`PayPeriodID`, lt.`PaystubID`, lt.`TransactionDate`, lt.`Description`, lt.`Type`, lt.`Balance`, lt.`Amount`, lt.`Comments`,
p.PartNo `Names`,
p.PartNo = 'Vacation leave' `IsVacation`,
IFNULL(i.VacationLeaveHours, 0) `VacationLeaveHours`,
p.PartNo = 'Sick leave' `IsSick`,
IFNULL(i.SickLeaveHours, 0) `SickLeaveHours`,
p.PartNo = 'Others' `IsOthers`,
IFNULL(i.OtherLeaveHours, 0) `OtherLeaveHours`,
p.PartNo = 'Maternity/paternity leave' `IsParental`,
IFNULL(i.MaternityLeaveHours, 0) `MaternityLeaveHours`

FROM leavetransaction lt
INNER JOIN leaveledger ll ON ll.RowID=lt.LeaveLedgerID
INNER JOIN product p ON p.RowID=ll.ProductID

LEFT JOIN `currenttimeentry` i ON i.EmployeeID=lt.EmployeeID

WHERE FIND_IN_SET(lt.RowID, @_ltIds)
GROUP BY lt.EmployeeID, ll.ProductID
;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
