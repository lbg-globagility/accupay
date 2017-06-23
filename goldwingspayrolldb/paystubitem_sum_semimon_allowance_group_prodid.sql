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

-- Dumping structure for view goldwingspayrolldb.paystubitem_sum_semimon_allowance_group_prodid
DROP VIEW IF EXISTS `paystubitem_sum_semimon_allowance_group_prodid`;
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `paystubitem_sum_semimon_allowance_group_prodid`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`127.0.0.1` VIEW `goldwingspayrolldb`.`paystubitem_sum_semimon_allowance_group_prodid` AS SELECT et.RowID AS `etRowID`
		,(ea.ProductID) AS ProductID
		,et.EmployeeID,et.OrganizationID,et.`Date`
		,0 AS Column1
		,GET_employeerateperday(et.EmployeeID,et.OrganizationID,et.`Date`) AS Column2
		,((IF(pr.PayType='Regular Day'
				, IF(et.TotalDayPay > et.RegularHoursAmount AND (et.VacationLeaveHours + et.SickLeaveHours + et.MaternityLeaveHours + et.OtherLeaveHours) > 0, IF(et.RegularHoursAmount = 0, et.TotalDayPay, et.RegularHoursAmount), IF(et.RegularHoursAmount > GET_employeerateperday(et.EmployeeID,et.OrganizationID,et.`Date`), GET_employeerateperday(et.EmployeeID,et.OrganizationID,et.`Date`), et.RegularHoursAmount))
				, IF(pr.PayType='Special Non-Working Holiday' AND e.CalcSpecialHoliday = '1'
					, IF(e.EmployeeType = 'Daily', (et.RegularHoursAmount / pr.`PayRate`), et.HolidayPayAmount)
					, IF(pr.PayType='Special Non-Working Holiday' AND e.CalcSpecialHoliday = '0'
						, IF(e.EmployeeType = 'Daily', et.RegularHoursAmount, et.HolidayPayAmount)
						, IF(pr.PayType='Regular Holiday' AND e.CalcHoliday = '1'
							, et.HolidayPayAmount + ((et.VacationLeaveHours + et.SickLeaveHours + et.MaternityLeaveHours + et.OtherLeaveHours) * (GET_employeerateperday(et.EmployeeID,et.OrganizationID,et.`Date`)
							/ IFNULL(COMPUTE_TimeDifference(sh.TimeFrom, sh.TimeTo) - COMPUTE_TimeDifference(sh.BreakTimeFrom, sh.BreakTimeTo),0)))
							
							, 0)))) / GET_employeerateperday(et.EmployeeID,et.OrganizationID,et.`Date`)) * (ea.AllowanceAmount / (e.WorkDaysPerYear / (PAYFREQUENCY_DIVISOR(pf.PayFrequencyType) * 12))) ) AS TotalAllowanceAmt
		,ea.TaxableFlag
		,(et.HoursLate + et.UndertimeHours) AS HoursToLess
		,ea.AllowanceAmount
		,e.WorkDaysPerYear
		,p.`Fixed`
		,PAYFREQUENCY_DIVISOR(pf.PayFrequencyType) AS PAYFREQDIV
		FROM employeetimeentry et
		INNER JOIN employee e ON e.OrganizationID=et.OrganizationID AND e.RowID=et.EmployeeID AND e.EmploymentStatus NOT IN ('Resigned','Terminated') AND e.EmployeeType!='Fixed'
		INNER JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID
		LEFT JOIN employeeshift es ON es.RowID=et.EmployeeShiftID
		LEFT JOIN shift sh ON sh.RowID=es.ShiftID
		INNER JOIN employeeallowance ea ON ea.AllowanceFrequency='Semi-monthly' AND ea.EmployeeID=e.RowID AND ea.OrganizationID=e.OrganizationID AND et.`Date` BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate
		INNER JOIN product p ON p.RowID=ea.ProductID
		INNER JOIN payrate pr ON pr.RowID=et.PayRateID
UNION ALL
SELECT et.RowID AS `etRowID`
		,(ea.ProductID) AS ProductID
		,et.EmployeeID,et.OrganizationID,et.`Date`
		,0 AS Column1
		,GET_employeerateperday(et.EmployeeID,et.OrganizationID,et.`Date`) AS Column2
		,ROUND((ea.AllowanceAmount / (e.WorkDaysPerYear / (PAYFREQUENCY_DIVISOR(pf.PayFrequencyType) * 12))),2)
		- IFNULL(((et.HoursLate + et.UndertimeHours)
			* (ROUND((ea.AllowanceAmount / (e.WorkDaysPerYear / (PAYFREQUENCY_DIVISOR(pf.PayFrequencyType) * 12))),2)
				/ IFNULL(sh.DivisorToDailyRate,0))), 0) AS TotalAllowanceAmt
		,ea.TaxableFlag
		,(et.HoursLate + et.UndertimeHours) AS HoursToLess
		,ea.AllowanceAmount
		,e.WorkDaysPerYear
		,p.`Fixed`
		,PAYFREQUENCY_DIVISOR(pf.PayFrequencyType) AS PAYFREQDIV
		FROM employeetimeentry et
		INNER JOIN employee e ON e.OrganizationID=et.OrganizationID AND e.RowID=et.EmployeeID AND e.EmploymentStatus NOT IN ('Resigned','Terminated') AND e.EmployeeType='Fixed'
		INNER JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID
		INNER JOIN employeeshift es ON es.RowID=et.EmployeeShiftID
		INNER JOIN shift sh ON sh.RowID=es.ShiftID
		INNER JOIN employeeallowance ea ON ea.AllowanceFrequency='Semi-monthly' AND ea.EmployeeID=e.RowID AND ea.OrganizationID=e.OrganizationID AND et.`Date` BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate
		INNER JOIN product p ON p.RowID=ea.ProductID
		INNER JOIN payrate pr ON pr.RowID=et.PayRateID ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
