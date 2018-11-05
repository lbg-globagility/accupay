/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_leave_summary`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_leave_summary`(
	IN `org_rowid` INT,
	IN `emp_rowid` INT,
	IN `date_from` DATE,
	IN `date_to` DATE

)
BEGIN

DECLARE leave_prodids TEXT;

DECLARE yeartofollow INT(11);

DECLARE remarks TEXT;

DECLARE custom_dateformat TEXT DEFAULT '%c/%e/%Y';

SELECT GROUP_CONCAT(p.RowID)
FROM product p
WHERE p.`Category`='Leave type'
AND p.OrganizationID=org_rowid
AND p.PartNo IN ('Vacation leave', 'Sick leave', 'Additional VL')
INTO leave_prodids;

SELECT MAX(pp.`Year`)
FROM payperiod pp
INNER JOIN dates d ON d.DateValue BETWEEN date_from AND date_to
WHERE pp.OrganizationID = org_rowid
AND d.DateValue BETWEEN pp.PayFromDate AND pp.PayToDate
AND pp.TotalGrossSalary = 1
INTO yeartofollow;

SELECT CONCAT('BALANCE AS OF ', DATE_FORMAT(MAX(pp.PayToDate), custom_dateformat)) `Remarks`
FROM payperiod pp
WHERE pp.OrganizationID = org_rowid
AND pp.TotalGrossSalary = 1
AND date_to BETWEEN pp.PayFromDate AND pp.PayToDate
INTO remarks;

SELECT i.*

/**/ , (i.LeaveAllowance - SUM(i.VacationLeaveHrs)) `VacationLeaveHours`
, (i.SickLeaveAllowance - SUM(i.SickLeaveHrs)) `SickLeaveHours`
# ,i.MaternityLeaveHours `MaternityLeaveHours`
# ,i.OtherLeaveHours `OtherLeaveHours`
, (i.AdditionalVLAllowance - SUM(i.AdditionalVLHrs)) `AdditionalVLHours`

FROM (SELECT ii.*
      FROM (SELECT
				e.RowID `EmployeeRowId`
				, et.RowID
				, DATE_FORMAT(et.`Date`, custom_dateformat) `Date`
				
				,et.VacationLeaveHours `VacationLeaveHrs`
				,et.SickLeaveHours `SickLeaveHrs`
				,et.MaternityLeaveHours `MaternityLeaveHrs`
				,et.OtherLeaveHours `OtherLeaveHrs`
				,et.AdditionalVLHours `AdditionalVLHrs`
				
				, e.EmployeeID
				, PROPERCASE(CONCAT_WS(', ', e.LastName, e.FirstName)) `FullName`
				, e.LeaveAllowance
				, e.SickLeaveAllowance
				, e.AdditionalVLAllowance
				
				, (e.LeaveAllowance
				   + e.SickLeaveAllowance
				   + e.AdditionalVLAllowance) `TotalLeave`
				
				, remarks `Remarks`
				FROM employee e
				
				LEFT JOIN employeetimeentry et
				        ON et.OrganizationID = org_rowid
						     AND et.`Date` BETWEEN date_from AND date_to
							  # AND (et.VacationLeaveHours + et.SickLeaveHours + et.OtherLeaveHours + et.AdditionalVLHours) > 0
							  AND (et.VacationLeaveHours + et.SickLeaveHours + et.AdditionalVLHours) > 0
							  AND et.EmployeeID=e.RowID
				
				INNER JOIN employeeleave elv ON elv.EmployeeID=et.EmployeeID AND elv.`Status`='Approved' AND et.`Date` BETWEEN elv.LeaveStartDate AND elv.LeaveEndDate
				
				LEFT JOIN payperiod pp ON pp.TotalGrossSalary=e.PayFrequencyID AND pp.OrganizationID=e.OrganizationID AND pp.`Year` = yeartofollow AND et.`Date` BETWEEN pp.PayFromDate AND pp.PayToDate
				
				LEFT JOIN paystub ps ON ps.OrganizationID=e.OrganizationID AND ps.EmployeeID=e.RowID AND ps.PayPeriodID=pp.RowID
				LEFT JOIN paystubitem psi ON psi.PayStubID = ps.RowID AND FIND_IN_SET(psi.ProductID, leave_prodids) > 0
				
				WHERE e.OrganizationID = et.OrganizationID
				AND FIND_IN_SET(e.EmploymentStatus, UNEMPLOYEMENT_STATUSES()) = 0
				AND e.RowID = IFNULL(emp_rowid, e.RowID)
				AND (e.DateRegularized BETWEEN date_from AND date_to
				     OR (e.DateRegularized <= date_from
					      OR e.DateRegularized <= date_to)
					  )
				
				GROUP BY et.RowID
				ORDER BY CONCAT(e.LastName, e.FirstName), et.`Date`
				) ii
					
			/**/ UNION
				SELECT
				e.RowID `EmployeeRowId`
				, NULL `RowID`
				, NULL `Date`
				
				,0 `VacationLeaveHrs`
				,0 `SickLeaveHrs`
				,0 `MaternityLeaveHrs`
				,0 `OtherLeaveHrs`
				,0 `AdditionalVLHrs`
				
				, e.EmployeeID
				, PROPERCASE(CONCAT_WS(', ', e.LastName, e.FirstName)) `FullName`
				, e.LeaveAllowance
				, e.SickLeaveAllowance
				, e.AdditionalVLAllowance
				
				, (e.LeaveAllowance
				   + e.SickLeaveAllowance
				   + e.AdditionalVLAllowance) `TotalLeave`
				
				, remarks `Remarks`
				FROM employee e
				WHERE e.OrganizationID=org_rowid
				AND e.RowID=IFNULL(emp_rowid, e.RowID)
				AND FIND_IN_SET(e.EmploymentStatus, UNEMPLOYEMENT_STATUSES()) = 0
				AND (e.LeaveAllowance
				     + e.SickLeaveAllowance
				     + e.AdditionalVLAllowance) > 0
				AND (e.DateRegularized BETWEEN date_from AND date_to
				     OR (e.DateRegularized <= date_from
					      OR e.DateRegularized <= date_to)
					  )
) i
GROUP BY i.EmployeeRowId
ORDER BY i.`FullName`
;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
