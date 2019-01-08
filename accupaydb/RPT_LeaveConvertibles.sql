/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_LeaveConvertibles`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_LeaveConvertibles`(
	IN `orgId` INT,
	IN `leaveTypeId` INT,
	IN `payPeriodFromId` INT,
	IN `payPeriodToId` INT,
	IN `employeePrimId` INT




)
BEGIN

DECLARE defaultWorkHour INT(11) DEFAULT 8;

SET SESSION group_concat_max_len = 2048;

SET @ids = NULL;

SELECT
	GROUP_CONCAT(pp.RowID) `Ids`
FROM payperiod pp
	INNER JOIN payperiod ppf ON ppf.RowID=payPeriodFromId
	INNER JOIN payperiod ppt ON ppt.RowID=payPeriodToId
WHERE (pp.PayFromDate >= ppf.PayFromDate OR pp.PayToDate >= ppf.PayFromDate)
		AND (pp.PayFromDate <= ppt.PayToDate OR pp.PayToDate <= ppt.PayToDate)
		AND pp.OrganizationID=orgId
ORDER BY pp.`Year`, pp.OrdinalValue
INTO @ids
;

SET @dailyRate = 0.00;

IF employeePrimId IS NULL THEN

	IF leaveTypeId IS NULL THEN

		SELECT lll.*
		, i.`EmployeeNo`
		, CONCAT_WS(', ', i.LastName, i.FirstName) `FullName`
		, (@dailyRate := GetActualDailyRate(lll.EmployeeID, orgId, lll.PayToDate)) `ActualDailyRate`
		, (@dailyRate / defaultWorkHour) `HourlyRate`
		FROM latestleaveledger lll
		INNER JOIN (SELECT ll.*
						
						#, p.PartNo `LeaveType`
						
						, lt.TransactionDate
						, lt.ReferenceID
						, lt.Balance
						, lt.Amount
						, lt.PayPeriodID
						, pp.PayFromDate, pp.PayToDate, pp.OrdinalValue
						
						, e.EmployeeID `EmployeeNo`
						, e.LastName
						, e.FirstName
						, e.MiddleName
						
						, MAX(pp.PayToDate) `MaxPayToDate`
						FROM leaveledger ll
						INNER JOIN employee e ON e.RowID=ll.EmployeeID
						INNER JOIN product p
						        ON p.RowID=ll.ProductID
								     AND p.PartNo IN ('Sick Leave', 'Vacation Leave')
								     AND p.RowID = leaveTypeId
						INNER JOIN leavetransaction lt
						        ON lt.LeaveLedgerID=ll.RowID
						INNER JOIN payperiod pp
						        ON pp.RowID=lt.PayPeriodID
						AND FIND_IN_SET(pp.RowID, @ids) > 0
						GROUP BY ll.EmployeeID, ll.ProductID
						) i
		
		WHERE lll.EmployeeID=i.EmployeeID
		AND lll.PayToDate=i.`MaxPayToDate`
		ORDER BY CONCAT(i.LastName, i.FirstName, i.MiddleName)
		;

	ELSE 

		SELECT lll.*
		, i.`EmployeeNo`
		, CONCAT_WS(', ', i.LastName, i.FirstName) `FullName`
		, (@dailyRate := GetActualDailyRate(lll.EmployeeID, orgId, lll.PayToDate)) `ActualDailyRate`
		, (@dailyRate / defaultWorkHour) `HourlyRate`
		FROM latestleaveledger lll
		INNER JOIN (SELECT ll.*
						
						#, p.PartNo `LeaveType`
						
						, lt.TransactionDate
						, lt.ReferenceID
						, lt.Balance
						, lt.Amount
						, lt.PayPeriodID
						, pp.PayFromDate, pp.PayToDate, pp.OrdinalValue
						
						, e.EmployeeID `EmployeeNo`
						, e.LastName
						, e.FirstName
						, e.MiddleName
						
						, MAX(pp.PayToDate) `MaxPayToDate`
						FROM leaveledger ll
						INNER JOIN employee e ON e.RowID=ll.EmployeeID
						INNER JOIN product p
						        ON p.RowID=ll.ProductID
								     AND p.PartNo IN ('Sick Leave', 'Vacation Leave')
								     AND p.RowID = leaveTypeId
						INNER JOIN leavetransaction lt
						        ON lt.LeaveLedgerID=ll.RowID
						INNER JOIN payperiod pp
						        ON pp.RowID=lt.PayPeriodID
						AND FIND_IN_SET(pp.RowID, @ids) > 0
						GROUP BY ll.EmployeeID, ll.ProductID
						) i
		
		WHERE lll.EmployeeID=i.EmployeeID
		AND lll.PayToDate=i.`MaxPayToDate`
		AND lll.ProductID=leaveTypeId
		ORDER BY CONCAT(i.LastName, i.FirstName, i.MiddleName)
		;

	END IF;

ELSE

	IF leaveTypeId IS NULL THEN

		SELECT lll.*
		, i.`EmployeeNo`
		, CONCAT_WS(', ', i.LastName, i.FirstName) `FullName`
		, (@dailyRate := GetActualDailyRate(lll.EmployeeID, orgId, lll.PayToDate)) `ActualDailyRate`
		, (@dailyRate / defaultWorkHour) `HourlyRate`
		FROM latestleaveledger lll
		INNER JOIN (SELECT ll.*
						
						#, p.PartNo `LeaveType`
						
						, lt.TransactionDate
						, lt.ReferenceID
						, lt.Balance
						, lt.Amount
						, lt.PayPeriodID
						, pp.PayFromDate, pp.PayToDate, pp.OrdinalValue
						
						, e.EmployeeID `EmployeeNo`
						, e.LastName
						, e.FirstName
						, e.MiddleName
						
						, MAX(pp.PayToDate) `MaxPayToDate`
						FROM leaveledger ll
						INNER JOIN product p
						        ON p.RowID=ll.ProductID
								     AND p.PartNo IN ('Sick Leave', 'Vacation Leave')
								     AND p.RowID = leaveTypeId
						INNER JOIN leavetransaction lt
						        ON lt.LeaveLedgerID=ll.RowID
						INNER JOIN payperiod pp
						        ON pp.RowID=lt.PayPeriodID
						AND FIND_IN_SET(pp.RowID, @ids) > 0
						WHERE ll.EmployeeID = employeePrimId
						GROUP BY ll.EmployeeID, ll.ProductID
						) i
		
		WHERE lll.EmployeeID=i.EmployeeID
		AND lll.PayToDate=i.`MaxPayToDate`
		ORDER BY CONCAT(i.LastName, i.FirstName, i.MiddleName)
		;

	ELSE

		SELECT lll.*
		, i.`EmployeeNo`
		, CONCAT_WS(', ', i.LastName, i.FirstName) `FullName`
		, (@dailyRate := GetActualDailyRate(lll.EmployeeID, orgId, lll.PayToDate)) `ActualDailyRate`
		, (@dailyRate / defaultWorkHour) `HourlyRate`
		FROM latestleaveledger lll
		INNER JOIN (SELECT ll.*
						
						#, p.PartNo `LeaveType`
						
						, lt.TransactionDate
						, lt.ReferenceID
						, lt.Balance
						, lt.Amount
						, lt.PayPeriodID
						, pp.PayFromDate, pp.PayToDate, pp.OrdinalValue
						
						, e.EmployeeID `EmployeeNo`
						, e.LastName
						, e.FirstName
						, e.MiddleName
						
						, MAX(pp.PayToDate) `MaxPayToDate`
						FROM leaveledger ll
						INNER JOIN product p
						        ON p.RowID=ll.ProductID
								     AND p.PartNo IN ('Sick Leave', 'Vacation Leave')
								     AND p.RowID = leaveTypeId
						INNER JOIN leavetransaction lt
						        ON lt.LeaveLedgerID=ll.RowID
						INNER JOIN payperiod pp
						        ON pp.RowID=lt.PayPeriodID
						AND FIND_IN_SET(pp.RowID, @ids) > 0
						WHERE ll.EmployeeID = employeePrimId
						GROUP BY ll.EmployeeID, ll.ProductID
						) i
		
		WHERE lll.EmployeeID=i.EmployeeID
		AND lll.PayToDate=i.`MaxPayToDate`
		AND lll.ProductID=leaveTypeId
		ORDER BY CONCAT(i.LastName, i.FirstName, i.MiddleName)
		;

	END IF;
		
END IF;
	
END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
