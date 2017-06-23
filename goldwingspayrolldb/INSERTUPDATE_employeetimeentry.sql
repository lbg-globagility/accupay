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

-- Dumping structure for function goldwingspayrolldb.INSERTUPDATE_employeetimeentry
DROP FUNCTION IF EXISTS `INSERTUPDATE_employeetimeentry`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `INSERTUPDATE_employeetimeentry`(`eteRowID` INT
, `eteOrganizID` INT
, `UserRowID` INT
, `eteDate` DATE
, `eteEmployeeShiftID` INT
, `eteEmployeeID` INT
, `eteEmployeeSalaryID` INT
, `eteEmployeeFixedSalaryFlag` CHAR(1)
, `eteRegularHoursWorked` DECIMAL(11,2)
, `eteRegularHoursAmount` DECIMAL(11,2)
, `eteTotalHoursWorked` DECIMAL(11,2)
, `eteOvertimeHoursWorked` DECIMAL(11,2)
, `eteOvertimeHoursAmount` DECIMAL(11,2)
, `eteUndertimeHours` DECIMAL(11,2)
, `eteUndertimeHoursAmount` DECIMAL(11,2)
, `eteNightDifferentialHours` DECIMAL(11,2)
, `eteNightDiffHoursAmount` DECIMAL(11,2)
, `eteNightDifferentialOTHours` DECIMAL(11,2)
, `eteNightDiffOTHoursAmount` DECIMAL(11,2)
, `eteHoursLate` DECIMAL(11,2)
, `eteHoursLateAmount` DECIMAL(11,2)
, `eteLateFlag` CHAR(1)
, `etePayRateID` INT
, `eteVacationLeaveHours` DECIMAL(11,2)
, `eteSickLeaveHours` DECIMAL(11,2)
, `eteMaternityLeaveHours` DECIMAL(11,2)
, `eteOtherLeaveHours` DECIMAL(11,2)
, `eteTotalDayPay` DECIMAL(11,2)
, `eteAbsent` DECIMAL(11,2)
, `eteChargeToDivisionID` INT) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11);

INSERT INTO employeetimeentry
(
	RowID
	,OrganizationID
	,Created
	,CreatedBy
	,Date
	,EmployeeShiftID
	,EmployeeID
	,EmployeeSalaryID
	,EmployeeFixedSalaryFlag
	,RegularHoursWorked
	,RegularHoursAmount
	,TotalHoursWorked
	,OvertimeHoursWorked
	,OvertimeHoursAmount
	,UndertimeHours
	,UndertimeHoursAmount
	,NightDifferentialHours
	,NightDiffHoursAmount
	,NightDifferentialOTHours
	,NightDiffOTHoursAmount
	,HoursLate
	,HoursLateAmount
	,LateFlag
	,PayRateID
	,VacationLeaveHours
	,SickLeaveHours
	,MaternityLeaveHours
	,OtherLeaveHours
	,TotalDayPay
	,Absent
	,ChargeToDivisionID
) VALUES (
	eteRowID
	,eteOrganizID
	,CURRENT_TIMESTAMP()
	,UserRowID
	,eteDate
	,eteEmployeeShiftID
	,eteEmployeeID
	,eteEmployeeSalaryID
	,eteEmployeeFixedSalaryFlag
	,eteRegularHoursWorked
	,eteRegularHoursAmount
	,eteTotalHoursWorked
	,eteOvertimeHoursWorked
	,eteOvertimeHoursAmount
	,eteUndertimeHours
	,eteUndertimeHoursAmount
	,eteNightDifferentialHours
	,eteNightDiffHoursAmount
	,eteNightDifferentialOTHours
	,eteNightDiffOTHoursAmount
	,eteHoursLate
	,eteHoursLateAmount
	,eteLateFlag
	,etePayRateID
	,eteVacationLeaveHours
	,eteSickLeaveHours
	,eteMaternityLeaveHours
	,eteOtherLeaveHours
	,eteTotalDayPay
	,eteAbsent
	,eteChargeToDivisionID
) ON
DUPLICATE
KEY
UPDATE
	LastUpd=CURRENT_TIMESTAMP()
	,LastUpdBy=UserRowID
	,EmployeeShiftID=eteEmployeeShiftID
	,EmployeeSalaryID=eteEmployeeSalaryID
	,EmployeeFixedSalaryFlag=eteEmployeeFixedSalaryFlag
	,RegularHoursWorked=eteRegularHoursWorked
	,RegularHoursAmount=eteRegularHoursAmount
	,TotalHoursWorked=eteTotalHoursWorked
	,OvertimeHoursWorked=eteOvertimeHoursWorked
	,OvertimeHoursAmount=eteOvertimeHoursAmount
	,UndertimeHours=eteUndertimeHours
	,UndertimeHoursAmount=eteUndertimeHoursAmount
	,NightDifferentialHours=eteNightDifferentialHours
	,NightDiffHoursAmount=eteNightDiffHoursAmount
	,NightDifferentialOTHours=eteNightDifferentialOTHours
	,NightDiffOTHoursAmount=eteNightDiffOTHoursAmount
	,HoursLate=eteHoursLate
	,HoursLateAmount=eteHoursLateAmount
	,LateFlag=eteLateFlag
	,PayRateID=etePayRateID
	,VacationLeaveHours=eteVacationLeaveHours
	,SickLeaveHours=eteSickLeaveHours
	,MaternityLeaveHours=eteMaternityLeaveHours
	,OtherLeaveHours=eteOtherLeaveHours
	,TotalDayPay=eteTotalDayPay
	,Absent=eteAbsent
	,ChargeToDivisionID=eteChargeToDivisionID;
	


RETURN returnvalue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
