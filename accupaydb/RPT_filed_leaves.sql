/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_filed_leaves`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_filed_leaves`(
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

SELECT
et.RowID
, DATE_FORMAT(et.`Date`, custom_dateformat) `Date`
,et.EmployeeShiftID
# ,et.EmployeeID
,e.EmployeeID
,et.EmployeeSalaryID
,et.EmployeeFixedSalaryFlag
,et.RegularHoursWorked
,et.RegularHoursAmount
,et.TotalHoursWorked
,et.OvertimeHoursWorked
,et.OvertimeHoursAmount
,et.UndertimeHours
,et.UndertimeHoursAmount
,et.NightDifferentialHours
,et.NightDiffHoursAmount
,et.NightDifferentialOTHours
,et.NightDiffOTHoursAmount
,et.HoursLate
,et.HoursLateAmount
,et.LateFlag
,et.PayRateID
,et.VacationLeaveHours
,et.SickLeaveHours
,et.MaternityLeaveHours
,et.OtherLeaveHours
,et.AdditionalVLHours
,et.TotalDayPay
,et.Absent
,et.TaxableDailyAllowance
,et.HolidayPayAmount
,et.TaxableDailyBonus
,et.NonTaxableDailyBonus
,et.IsValidForHolidayPayment

, e.EmployeeID
, PROPERCASE(CONCAT_WS(', ', e.LastName, e.FirstName)) `FullName`
, e.LeaveAllowance
, e.SickLeaveAllowance
, e.AdditionalVLAllowance

, (e.LeaveAllowance
   + e.SickLeaveAllowance
   + e.AdditionalVLAllowance) `TotalLeave`

, remarks `Remarks`
FROM employeetimeentry et

INNER JOIN employee e
        ON e.RowID=et.EmployeeID
		     AND e.RowID = IFNULL(emp_rowid, e.RowID)
		     AND e.OrganizationID = et.OrganizationID
		     AND FIND_IN_SET(e.EmploymentStatus, UNEMPLOYEMENT_STATUSES()) = 0
			  AND (e.DateRegularized BETWEEN date_from AND date_to
				    OR (e.DateRegularized <= date_from
					     OR e.DateRegularized <= date_to)
					 )

INNER JOIN payperiod pp ON pp.TotalGrossSalary=e.PayFrequencyID AND pp.OrganizationID=e.OrganizationID AND pp.`Year` = yeartofollow AND et.`Date` BETWEEN pp.PayFromDate AND pp.PayToDate

INNER JOIN paystub ps ON ps.OrganizationID=e.OrganizationID AND ps.EmployeeID=e.RowID AND ps.PayPeriodID=pp.RowID
INNER JOIN paystubitem psi ON psi.PayStubID = ps.RowID AND FIND_IN_SET(psi.ProductID, leave_prodids) > 0
WHERE et.OrganizationID = org_rowid
AND et.`Date` BETWEEN date_from AND date_to
# AND (et.VacationLeaveHours + et.SickLeaveHours + et.OtherLeaveHours + et.AdditionalVLHours) > 0
AND (et.VacationLeaveHours + et.SickLeaveHours + et.AdditionalVLHours) > 0
GROUP BY et.RowID
ORDER BY CONCAT(e.LastName, e.FirstName), et.`Date`
;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
