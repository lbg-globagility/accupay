/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `PRINT_employee_profiles`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `PRINT_employee_profiles`(IN `og_rowid` INT)
    DETERMINISTIC
BEGIN

DECLARE pp_rowid
        ,result_count INT(11);

DECLARE preferred_dateformat VARCHAR(50) DEFAULT '%c/%e/%Y';

DROP TEMPORARY TABLE if EXISTS `daynameconvert`;
CREATE TEMPORARY TABLE `daynameconvert`
SELECT 1 `Index`, 'Sunday' `DayName`
UNION SELECT 2,  'Monday'
UNION SELECT 3,  'Tuesday'
UNION SELECT 4,  'Wednesday'
UNION SELECT 5,  'Thursday'
UNION SELECT 6,  'Friday'
UNION SELECT 7,  'Saturday'
;

SELECT
	e.Salutation,
	e.FirstName,
	e.MiddleName,
	e.LastName,
	e.EmployeeID,
	e.TINNo,
	e.SSSNo,
	e.HDMFNo,
	e.PhilHealthNo,
	e.EmploymentStatus,
	e.EmailAddress,
	e.WorkPhone,
	e.HomePhone,
	e.MobilePhone,
	e.HomeAddress,
	e.Nickname,
	IF(e.Gender='F', 'Female', 'Male') `Gender`,
	e.EmployeeType,
	e.MaritalStatus,
	DATE_FORMAT(e.Birthdate, preferred_dateformat) `Birthdate`,
	DATE_FORMAT(e.StartDate, preferred_dateformat) `StartDate`,
	DATE_FORMAT(e.TerminationDate, preferred_dateformat) `TerminationDate`,
	pos.PositionName, # PositionID
	pf.PayFrequencyType, # PayFrequencyID
#	e.NoOfDependents,
	e.LeaveBalance,
	e.SickLeaveBalance,
	e.MaternityLeaveBalance,
	e.OtherLeaveBalance,
	e.LeaveAllowance,
	e.SickLeaveAllowance,
	e.MaternityLeaveAllowance,
	e.OtherLeaveAllowance,
	e.LeavePerPayPeriod,
	e.SickLeavePerPayPeriod,
	e.MaternityLeavePerPayPeriod,
	e.OtherLeavePerPayPeriod,
	e.WorkDaysPerYear,	
	dc.`DayName` `DayOfRest`,
	e.ATMNo,
	e.BankName,
	IF(e.CalcHoliday, 'Yes', 'No') `CalcHoliday`,
	IF(e.CalcSpecialHoliday, 'Yes', 'No') `CalcSpecialHoliday`,
	IF(e.CalcNightDiff, 'Yes', 'No') `CalcNightDiff`,
	IF(e.CalcNightDiffOT, 'Yes', 'No') `CalcNightDiffOT`,
	IF(e.CalcRestDay, 'Yes', 'No') `CalcRestDay`,
	IF(e.CalcRestDayOT, 'Yes', 'No') `CalcRestDayOT`,
	DATE_FORMAT(e.DateRegularized, preferred_dateformat) `DateRegularized`,
	DATE_FORMAT(e.DateEvaluated, preferred_dateformat) `DateEvaluated`,
	e.LateGracePeriod,
	ag.AgencyName, # AgencyID
	e.OffsetBalance,
	b.BranchName, # BranchID
	TIME_FORMAT(e.MinimumOvertime, '%H:%i') `MinimumOvertime`, # MinimumOvertime
	e.AdvancementPoints,
	e.BPIInsurance,
	ep.`Name` `EmploymentPolicyName` # EmploymentPolicyId
FROM employee e

INNER JOIN organization og ON og.RowID=e.OrganizationID

LEFT JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID

LEFT JOIN branch b ON b.RowID=e.BranchID

LEFT JOIN `position` pos ON pos.RowID=e.PositionID

LEFT JOIN agency ag ON ag.RowID=e.AgencyID

LEFT JOIN employmentpolicy ep ON ep.Id=e.EmploymentPolicyId

LEFT JOIN `daynameconvert` dc ON dc.`Index`=e.DayOfRest

WHERE e.OrganizationID=og_rowid
AND e.EmploymentStatus NOT IN ('Resigned', 'Terminated')
AND e.RevealInPayroll IN ('1', 'Y')

ORDER BY CONCAT(e.LastName,e.FirstName,e.MiddleName)
;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
