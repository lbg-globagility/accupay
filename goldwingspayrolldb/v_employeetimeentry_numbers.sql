-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.12-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for view cinema2k.v_employeetimeentry_numbers
DROP VIEW IF EXISTS `v_employeetimeentry_numbers`;
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `v_employeetimeentry_numbers`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` VIEW `v_employeetimeentry_numbers` AS SELECT et.RowID
	,et.OrganizationID
	,et.`Date`
	,pp.RowID `PayPeriodID`
	,et.EmployeeID
	#,SUM(et.RegularHoursWorked) `RegularHoursWorked`
	#,SUM(sh.DivisorToDailyRate) `DivisorToDailyRate`
	#,AVG((et.RegularHoursWorked / sh.DivisorToDailyRate)) `AttendancePercentage`
	#,COUNT(esh.RowID) `PerfectAttendance`
	
	,et.RegularHoursWorked
	,sh.DivisorToDailyRate
	,(et.RegularHoursWorked / sh.DivisorToDailyRate) `AttendancePercentage`
	
	FROM employeetimeentry et
	INNER JOIN employeeshift esh ON esh.RowID=et.EmployeeShiftID
	INNER JOIN shift sh ON sh.RowID=esh.ShiftID
	INNER JOIN payrate pr ON pr.RowID=et.PayRateID AND pr.PayType = 'Regular Day'
	INNER JOIN employeesalary esa ON esa.RowID=et.EmployeeSalaryID
	INNER JOIN payperiod pp ON pp.OrganizationID=et.OrganizationID AND et.`Date` BETWEEN pp.PayFromDate AND pp.PayToDate
	
UNION
	SELECT et.RowID
	,et.OrganizationID
	,et.`Date`
	,pp.RowID `PayPeriodID`
	,et.EmployeeID
	,et.RegularHoursWorked
	,sh.DivisorToDailyRate
	,0 `AttendancePercentage`
	
	FROM employeetimeentry et
	INNER JOIN employee e ON e.RowID=et.EmployeeID
	INNER JOIN employeeshift esh ON esh.RowID=et.EmployeeShiftID
	INNER JOIN shift sh ON sh.RowID=esh.ShiftID
	INNER JOIN payrate pr ON pr.RowID=et.PayRateID AND pr.PayType = 'Regular Holiday'
	INNER JOIN employeesalary esa ON esa.RowID=et.EmployeeSalaryID
	INNER JOIN payperiod pp ON pp.OrganizationID=et.OrganizationID AND et.`Date` BETWEEN pp.PayFromDate AND pp.PayToDate
	WHERE et.RegularHoursWorked = 0
	      AND et.TotalDayPay = 0

UNION
	SELECT et.RowID
	,et.OrganizationID
	,et.`Date`
	,pp.RowID `PayPeriodID`
	,et.EmployeeID
	,et.RegularHoursWorked
	,sh.DivisorToDailyRate
	,1 `AttendancePercentage`
	
	FROM employeetimeentry et
	INNER JOIN employee e ON e.RowID=et.EmployeeID
	INNER JOIN employeeshift esh ON esh.RowID=et.EmployeeShiftID
	INNER JOIN shift sh ON sh.RowID=esh.ShiftID
	INNER JOIN payrate pr ON pr.RowID=et.PayRateID AND pr.PayType = 'Regular Holiday'
	INNER JOIN employeesalary esa ON esa.RowID=et.EmployeeSalaryID
	INNER JOIN payperiod pp ON pp.OrganizationID=et.OrganizationID AND et.`Date` BETWEEN pp.PayFromDate AND pp.PayToDate
	WHERE et.RegularHoursWorked = 0
	      AND et.TotalDayPay > 0

UNION
	SELECT et.RowID
	,et.OrganizationID
	,et.`Date`
	,pp.RowID `PayPeriodID`
	,et.EmployeeID
	,et.RegularHoursWorked
	,sh.DivisorToDailyRate
	,(et.RegularHoursAmount >= ( IF(e.EmployeeType = 'Daily'
		                             , esa.BasicPay
											  , ROUND((esa.Salary / (e.WorkDaysPerYear / 12)), 2)) )) `AttendancePercentage`
	
	FROM employeetimeentry et
	INNER JOIN employee e ON e.RowID=et.EmployeeID
	INNER JOIN employeeshift esh ON esh.RowID=et.EmployeeShiftID
	INNER JOIN shift sh ON sh.RowID=esh.ShiftID
	INNER JOIN payrate pr ON pr.RowID=et.PayRateID AND pr.PayType = 'Regular Holiday'
	INNER JOIN employeesalary esa ON esa.RowID=et.EmployeeSalaryID
	INNER JOIN payperiod pp ON pp.OrganizationID=et.OrganizationID AND et.`Date` BETWEEN pp.PayFromDate AND pp.PayToDate
	WHERE et.RegularHoursWorked > 0
	      AND et.TotalDayPay > 0

UNION
	SELECT et.RowID
	,et.OrganizationID
	,et.`Date`
	,pp.RowID `PayPeriodID`
	,et.EmployeeID
	,et.RegularHoursWorked
	,sh.DivisorToDailyRate
	,(et.RegularHoursAmount >= ( IF(e.EmployeeType = 'Daily'
		                             , esa.BasicPay
											  , ROUND((esa.Salary / (e.WorkDaysPerYear / 12)), 2)) )) `AttendancePercentage`
	
	FROM employeetimeentry et
	INNER JOIN employee e ON e.RowID=et.EmployeeID AND e.CalcSpecialHoliday = 0
	INNER JOIN employeeshift esh ON esh.RowID=et.EmployeeShiftID
	INNER JOIN shift sh ON sh.RowID=esh.ShiftID
	INNER JOIN payrate pr ON pr.RowID=et.PayRateID AND pr.PayType = 'Special Non-Working Holiday'
	INNER JOIN employeesalary esa ON esa.RowID=et.EmployeeSalaryID
	INNER JOIN payperiod pp ON pp.OrganizationID=et.OrganizationID AND et.`Date` BETWEEN pp.PayFromDate AND pp.PayToDate

UNION
	SELECT et.RowID
	,et.OrganizationID
	,et.`Date`
	,pp.RowID `PayPeriodID`
	,et.EmployeeID
	,et.RegularHoursWorked
	,sh.DivisorToDailyRate
	,((et.RegularHoursAmount / pr.`PayRate`)
	  >= ( IF(e.EmployeeType = 'Daily'
	          , esa.BasicPay
				 , ROUND((esa.Salary / (e.WorkDaysPerYear / 12)), 2)) )) `AttendancePercentage`
	
	FROM employeetimeentry et
	INNER JOIN employee e ON e.RowID=et.EmployeeID AND e.CalcSpecialHoliday = 1
	INNER JOIN employeeshift esh ON esh.RowID=et.EmployeeShiftID
	INNER JOIN shift sh ON sh.RowID=esh.ShiftID
	INNER JOIN payrate pr ON pr.RowID=et.PayRateID AND pr.PayType = 'Special Non-Working Holiday'
	INNER JOIN employeesalary esa ON esa.RowID=et.EmployeeSalaryID
	INNER JOIN payperiod pp ON pp.OrganizationID=et.OrganizationID AND et.`Date` BETWEEN pp.PayFromDate AND pp.PayToDate ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
