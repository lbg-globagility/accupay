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

-- Dumping structure for procedure goldwingspayrolldb.VIEW_employeetimeentry_SUM
DROP PROCEDURE IF EXISTS `VIEW_employeetimeentry_SUM`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeetimeentry_SUM`(IN `etent_OrganizationID` INT, IN `etent_EmployeeID` INT, IN `etent_Date` DATE, IN `etent_DateTo` DATE)
    DETERMINISTIC
BEGIN

DECLARE paypFrom DATE;

DECLARE paypTo DATE;

DECLARE salRowID INT(11);

DECLARE EmpPayFreqID INT(11);

DECLARE employee_datehire DATE;

SELECT PayFrequencyID,StartDate FROM employee WHERE RowID=etent_EmployeeID INTO EmpPayFreqID,employee_datehire;



SELECT RowID FROM employeesalary WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND etent_Date BETWEEN DATE(COALESCE(EffectiveDateFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveDateTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) AND DATEDIFF(etent_Date,EffectiveDateFrom) >= 0 ORDER BY DATEDIFF(DATE_FORMAT(etent_Date,'%Y-%m-%d'),EffectiveDateFrom) LIMIT 1 INTO salRowID;


IF EmpPayFreqID = 1 THEN

	
	SET paypFrom = etent_Date;

	IF employee_datehire IS NOT NULL THEN
		
		SET paypFrom = IF(employee_datehire > etent_Date, employee_datehire, etent_Date);

	END IF;
	
	SET paypTo = etent_DateTo;

ELSEIF EmpPayFreqID = 2 THEN

	SELECT DATE(CONCAT(YEAR(etent_Date),'-',MONTH(etent_Date),'-01')) INTO paypFrom;

	SELECT DATE(CONCAT(YEAR(etent_Date),'-',MONTH(etent_Date),'-',IF(DAY(etent_Date) <= 15,15,DAY(LAST_DAY(DATE(etent_Date)))))) INTO paypTo;

ELSEIF EmpPayFreqID = 3 THEN

	SELECT DATE(CONCAT(YEAR(etent_Date),'-',MONTH(etent_Date),'-',IF(DAY(etent_Date) <= 15,15,DAY(LAST_DAY(DATE(etent_Date)))))) INTO paypTo;

	SET paypFrom = paypTo;

ELSEIF EmpPayFreqID = 4 THEN


	SET paypFrom = etent_Date;

	IF employee_datehire IS NOT NULL THEN
		
		SET paypFrom = IF(employee_datehire > etent_Date, employee_datehire, etent_Date);

	END IF;
	
	SET paypTo = etent_DateTo;

END IF;







SELECT 
COALESCE(etent.RowID,'') 'RowID'
,COALESCE(DATE(COALESCE(etent.Date,'')),'') 'Date'
,COALESCE(etent.EmployeeShiftID,(SELECT RowID FROM employeeshift WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND DATE(etent_Date) BETWEEN COALESCE(EffectiveFrom,DATE_ADD(DATE(etent_Date), INTERVAL -1 MONTH)) AND COALESCE(EffectiveTo,DATE_ADD(DATE(etent_Date), INTERVAL 1 MONTH)) LIMIT 1)) 'EmployeeShiftID'
,COALESCE(etent.EmployeeID,'') 'EmployeeID'
,COALESCE(etent.EmployeeSalaryID,salRowID) 'EmployeeSalaryID'
,COALESCE(etent.EmployeeFixedSalaryFlag,0) 'EmployeeFixedSalaryFlag'
,IFNULL(SUM(IFNULL(etent.TotalHoursWorked,0)) - SUM(IFNULL(etent.OvertimeHoursWorked,0)), 0) 'TotalHoursWorked'
,COALESCE(SUM(COALESCE(etent.RegularHoursWorked,0)),0) 'RegularHoursWorked'
,SUM(COALESCE(etent.TotalDayPay,0)) - SUM(COALESCE(etent.OvertimeHoursAmount,0)) - SUM(COALESCE(etent.NightDiffHoursAmount,0)) - SUM(COALESCE(etent.NightDiffOTHoursAmount,0)) + SUM(IFNULL(etent.HoursLateAmount,0)) + SUM(IFNULL(etent.UndertimeHoursAmount,0)) 'RegularHoursAmount'
,COALESCE(SUM(COALESCE(etent.OvertimeHoursWorked,0)),0) 'OvertimeHoursWorked'
,COALESCE(SUM(COALESCE(etent.OvertimeHoursAmount,0)),0) 'OvertimeHoursAmount'
,COALESCE(SUM(COALESCE(etent.UndertimeHours,0)),0) 'UndertimeHours'
,COALESCE(SUM(COALESCE(etent.UndertimeHoursAmount,0)),0) 'UndertimeHoursAmount'
,COALESCE(SUM(COALESCE(etent.NightDifferentialHours,0)),0) 'NightDifferentialHours'
,COALESCE(SUM(COALESCE(etent.NightDiffHoursAmount,0)),0) 'NightDiffHoursAmount'
,COALESCE(SUM(COALESCE(etent.NightDifferentialOTHours,0)),0) 'NightDifferentialOTHours'
,COALESCE(SUM(COALESCE(etent.NightDiffOTHoursAmount,0)),0) 'NightDiffOTHoursAmount'
,COALESCE(SUM(COALESCE(etent.HoursLate,0)),0) 'HoursLate'
,COALESCE(SUM(COALESCE(etent.HoursLateAmount,0)),0) 'HoursLateAmount'
,COALESCE(etent.LateFlag,0) 'LateFlag'
,COALESCE(etent.PayRateID,0) 'PayRateID'
,COALESCE(SUM(COALESCE(etent.VacationLeaveHours,0)),0) 'VacationLeaveHours'
,COALESCE(SUM(COALESCE(etent.SickLeaveHours,0)),0) 'SickLeaveHours'
,COALESCE(SUM(COALESCE(etent.TotalDayPay,0)),0) 'TotalDayPay'
FROM employeetimeentry etent
WHERE etent.OrganizationID=etent_OrganizationID
AND etent.EmployeeID=etent_EmployeeID
AND Date BETWEEN paypFrom AND paypTo;




END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
