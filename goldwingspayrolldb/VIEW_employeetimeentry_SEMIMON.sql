-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.11-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.0.0.4396
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for procedure goldwingspayrolldb.VIEW_employeetimeentry_SEMIMON
DROP PROCEDURE IF EXISTS `VIEW_employeetimeentry_SEMIMON`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeetimeentry_SEMIMON`(IN `etent_OrganizationID` INT, IN `etent_EmployeeID` INT, IN `etent_Date` DATE)
    DETERMINISTIC
BEGIN

DECLARE paypFrom DATE;

DECLARE paypTo DATE;

DECLARE salRowID INT(11);

DECLARE day_ofrest CHAR(1);

DECLARE payrateRowID INT(11);
DECLARE commonrate DECIMAL(10,2);
DECLARE otrate DECIMAL(10,2);
DECLARE ndiffrate DECIMAL(10,2);
DECLARE ndiffotrate DECIMAL(10,2);
DECLARE restday_rate DECIMAL(10,2);
DECLARE restdayot_rate DECIMAL(10,2);
DECLARE pr_DayBefore DATE;
DECLARE pr_PayType VARCHAR(50);

DECLARE custom_dateformat VARCHAR(8) DEFAULT '%c/%e/%Y';
DECLARE custom_timeformat VARCHAR(8) DEFAULT '%h:%i %p';

SELECT e.DayOfRest,pp.PayFromDate,pp.PayToDate FROM employee e INNER JOIN payperiod pp ON pp.OrganizationID=e.OrganizationID AND pp.TotalGrossSalary=e.PayFrequencyID AND pp.`Month` = MONTH(etent_Date) AND pp.`Year` = YEAR(etent_Date) AND pp.Half = (DATE_FORMAT(etent_Date, '%d') <= 15) WHERE e.RowID=etent_EmployeeID INTO day_ofrest, paypFrom, paypTo;

SELECT RowID FROM employeesalary WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND etent_Date BETWEEN DATE(COALESCE(EffectiveDateFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveDateTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) AND DATEDIFF(etent_Date,EffectiveDateFrom) >= 0 ORDER BY DATEDIFF(DATE_FORMAT(etent_Date,'%Y-%m-%d'),EffectiveDateFrom) LIMIT 1 INTO salRowID;

	SELECT DATE(CONCAT(YEAR(etent_Date),'-',MONTH(etent_Date),'-',IF(DAY(etent_Date) <= 15,1,16))) INTO paypFrom;

	SELECT DATE(CONCAT(YEAR(etent_Date),'-',MONTH(etent_Date),'-',IF(DAY(etent_Date) <= 15,15,DAY(LAST_DAY(DATE(etent_Date)))))) INTO paypTo;
	
	
	
SELECT 
etent.RowID
,etent.OrganizationID
,etent.Created
,etent.CreatedBy
,IFNULL(etent.LastUpd,'') 'LastUpd'
,IFNULL(etent.LastUpdBy,'') 'LastUpdBy'
,IFNULL(DATE(etent.`Date`),'') 'Date'
,IFNULL(etent.EmployeeShiftID,'') 'EmployeeShiftID'
,etent.EmployeeID
,IFNULL(etent.EmployeeSalaryID,salRowID) 'EmployeeSalaryID'
,IFNULL(etent.EmployeeFixedSalaryFlag,0) 'EmployeeFixedSalaryFlag'
,IFNULL(etent.TotalHoursWorked,0) 'TotalHoursWorked'
,IFNULL(etent.RegularHoursWorked,0) 'RegularHoursWorked'
,IFNULL(etent.RegularHoursAmount,0) 'RegularHoursAmount'
,IFNULL(etent.OvertimeHoursWorked,0) 'OvertimeHoursWorked'
,IFNULL(etent.OvertimeHoursAmount,0) 'OvertimeHoursAmount'
,IFNULL(etent.UndertimeHours,0) 'UndertimeHours'
,IFNULL(etent.UndertimeHoursAmount,0) * IFNULL(prt.CommonRate,1) 'UndertimeHoursAmount'
,IFNULL(etent.NightDifferentialHours,0) 'NightDifferentialHours'
,IFNULL(etent.NightDiffHoursAmount,0) 'NightDiffHoursAmount'
,IFNULL(etent.NightDifferentialOTHours,0) 'NightDifferentialOTHours'
,IFNULL(etent.NightDiffOTHoursAmount,0) 'NightDiffOTHoursAmount'
,IFNULL(etent.HoursLate,0) 'HoursLate'
,IFNULL(etent.HoursLateAmount,0) * IFNULL(prt.CommonRate,1) 'HoursLateAmount'
,IFNULL(etent.LateFlag,0) 'LateFlag'
,IFNULL(etent.PayRateID,0) 'PayRateID'
,IFNULL(etent.VacationLeaveHours,0) 'VacationLeaveHours'
,IFNULL(etent.SickLeaveHours,0) 'SickLeaveHours'
,IFNULL(etent.MaternityLeaveHours,0) 'MaternityLeaveHours'
,IFNULL(etent.OtherLeaveHours,0) 'OtherLeaveHours'
,IFNULL(etent.TotalDayPay,0) 'TotalDayPay'
,etent.Absent
,IFNULL(TIME_FORMAT(sh.TimeFrom, custom_timeformat),	IFNULL(TIME_FORMAT(emsh.TimeFrom, custom_timeformat),'')) AS DutyStartTime
,IFNULL(TIME_FORMAT(sh.TimeTo, custom_timeformat),	IFNULL(TIME_FORMAT(emsh.TimeTo, custom_timeformat),'')) AS DutyEndTime
,IFNULL(DATE_FORMAT(esh.EffectiveFrom, custom_dateformat),	IFNULL(DATE_FORMAT(ems.EffectiveFrom, custom_dateformat),'')) AS ShiftEffectiveDateFrom
,IFNULL(DATE_FORMAT(esh.EffectiveTo, custom_dateformat),	IFNULL(DATE_FORMAT(ems.EffectiveTo, custom_dateformat),'')) AS ShiftEffectiveDateTo
,IFNULL(esh.RestDay,(DAYOFWEEK(etent.`Date`) = prt.DayOfRest)) AS IsDayOfRest
,IFNULL(es.BasicPay,0) AS BasicPay
,IFNULL(TIME_FORMAT(det.TimeIn, custom_timeformat), '') AS TimeIn
,IFNULL(TIME_FORMAT(det.TimeOut, custom_timeformat), '') AS TimeOut

FROM employeetimeentry etent

INNER JOIN employee e ON e.RowID=etent.EmployeeID

LEFT JOIN employeesalary es ON es.RowID=etent.EmployeeSalaryID

LEFT JOIN employeetimeentrydetails det ON det.RowID = (
		SELECT MAX(RowID)
		FROM employeetimeentrydetails
		WHERE `Date` = etent.`Date`
		AND EmployeeID = etent.EmployeeID
		ORDER BY Created DESC
		LIMIT 1
	)
LEFT JOIN (
	SELECT
		pr.RowID
		,IF(pr.PayType = 'Special Non-Working Holiday', IF(e.CalcSpecialHoliday = '1', pr.`PayRate`, 1), IF(pr.PayType = 'Regular Holiday', IF(e.CalcHoliday = '1', pr.`PayRate`, 1), pr.`PayRate`)) AS CommonRate
		,IF(e.OvertimeOverride = '1',pr.OvertimeRate,1) AS OverTimeRate
		,IF(e.CalcNightDiff = '1', pr.NightDifferentialRate, 1) AS NDiffRate
		,IF(e.CalcNightDiffOT = '1', pr.NightDifferentialOTRate, 1) AS NDiffOTRate
		,IF(e.CalcRestDay = '1', pr.RestDayRate, 1) AS RestDayRate
		,IF(e.CalcRestDayOT = '1', pr.RestDayOvertimeRate, 1) AS RestDayOTRate
		,e.DayOfRest
	FROM payrate pr
	INNER JOIN employee e ON e.RowID=etent_EmployeeID AND e.OrganizationID=etent_OrganizationID
	WHERE pr.OrganizationID=etent_OrganizationID
	AND pr.`Date` BETWEEN paypFrom AND paypTo) prt ON prt.RowID=etent.PayRateID
	
LEFT JOIN employeeshift esh ON esh.RowID=etent.EmployeeShiftID 

LEFT JOIN shift sh ON sh.RowID=esh.ShiftID

LEFT OUTER JOIN employeeshift ems ON ems.EmployeeID=etent.EmployeeID AND ems.OrganizationID=etent.OrganizationID AND etent.`Date` BETWEEN ems.EffectiveFrom AND ems.EffectiveTo AND e.DayOfRest = DAYOFWEEK(etent.`Date`) AND etent.EmployeeShiftID IS NULL

LEFT JOIN shift emsh ON emsh.RowID=ems.ShiftID

WHERE etent.OrganizationID=etent_OrganizationID
AND etent.EmployeeID=etent_EmployeeID
AND etent.`Date` BETWEEN paypFrom AND paypTo;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
