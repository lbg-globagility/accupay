/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP VIEW IF EXISTS `v_uni_employeetimeentry`;
DROP TABLE IF EXISTS `v_uni_employeetimeentry`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_uni_employeetimeentry` AS
SELECT RowID,OrganizationID,`Date`,EmployeeShiftID,EmployeeID,EmployeeSalaryID,EmployeeFixedSalaryFlag,RegularHoursWorked,RegularHoursAmount,TotalHoursWorked,OvertimeHoursWorked,OvertimeHoursAmount,UndertimeHours,UndertimeHoursAmount,NightDifferentialHours,NightDiffHoursAmount,NightDifferentialOTHours,NightDiffOTHoursAmount,HoursLate,HoursLateAmount,LateFlag,PayRateID,VacationLeaveHours,SickLeaveHours,MaternityLeaveHours,OtherLeaveHours,TotalDayPay,Absent,ChargeToDivisionID,HolidayPayAmount,0 `AsActual`,RestDayHours,RestDayAmount,RestDayOTHours,RestDayOTPay FROM employeetimeentry
UNION
SELECT RowID,OrganizationID,`Date`,EmployeeShiftID,EmployeeID,EmployeeSalaryID,EmployeeFixedSalaryFlag,RegularHoursWorked,RegularHoursAmount,TotalHoursWorked,OvertimeHoursWorked,OvertimeHoursAmount,UndertimeHours,UndertimeHoursAmount,NightDifferentialHours,NightDiffHoursAmount,NightDifferentialOTHours,NightDiffOTHoursAmount,HoursLate,HoursLateAmount,LateFlag,PayRateID,VacationLeaveHours,SickLeaveHours,MaternityLeaveHours,OtherLeaveHours,TotalDayPay,Absent,ChargeToDivisionID,HolidayPayAmount,1 `AsActual`,RestDayHours,RestDayAmount,RestDayOTHours,RestDayOTPay FROM employeetimeentryactual ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
