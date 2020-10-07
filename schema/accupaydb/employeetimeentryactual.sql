/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `employeetimeentryactual` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) DEFAULT NULL,
  `Date` date NOT NULL DEFAULT '1900-01-01' COMMENT 'time entry date',
  `EmployeeShiftID` int(10) DEFAULT NULL,
  `EmployeeID` int(10) DEFAULT NULL,
  `EmployeeFixedSalaryFlag` char(1) DEFAULT '0' COMMENT 'Flag is derived from EmployeeSalary table. flag to indicate if employee is on fixed salary, thus no overtime pay is calculated. TotalDayPay is fixed regardless of hours worked.',
  `ShiftHours` decimal(10,2) NOT NULL DEFAULT 0.00,
  `WorkHours` decimal(10,2) NOT NULL DEFAULT 0.00,
  `RegularHoursWorked` decimal(10,2) NOT NULL DEFAULT 0.00,
  `RegularHoursAmount` decimal(12,6) NOT NULL DEFAULT 0.000000,
  `TotalHoursWorked` decimal(10,2) NOT NULL DEFAULT 0.00,
  `OvertimeHoursWorked` decimal(10,2) NOT NULL DEFAULT 0.00,
  `OvertimeHoursAmount` decimal(12,6) NOT NULL DEFAULT 0.000000,
  `UndertimeHours` decimal(10,2) NOT NULL DEFAULT 0.00,
  `UndertimeHoursAmount` decimal(12,6) NOT NULL DEFAULT 0.000000,
  `NightDifferentialHours` decimal(10,2) NOT NULL DEFAULT 0.00,
  `NightDiffHoursAmount` decimal(12,6) NOT NULL DEFAULT 0.000000,
  `NightDifferentialOTHours` decimal(10,2) NOT NULL DEFAULT 0.00,
  `NightDiffOTHoursAmount` decimal(12,6) NOT NULL DEFAULT 0.000000,
  `HoursLate` decimal(10,2) NOT NULL DEFAULT 0.00,
  `HoursLateAmount` decimal(12,6) NOT NULL DEFAULT 0.000000,
  `LateFlag` varchar(50) DEFAULT '0',
  `PayRateID` int(11) DEFAULT NULL COMMENT 'Defines if the day is a Holiday, Special Holiday, Regular Day, HolidayType',
  `VacationLeaveHours` decimal(10,2) NOT NULL DEFAULT 0.00,
  `SickLeaveHours` decimal(10,2) NOT NULL DEFAULT 0.00,
  `MaternityLeaveHours` decimal(10,2) NOT NULL DEFAULT 0.00,
  `OtherLeaveHours` decimal(10,2) NOT NULL DEFAULT 0.00,
  `TotalDayPay` decimal(12,6) NOT NULL DEFAULT 0.000000,
  `Absent` decimal(12,6) NOT NULL DEFAULT 0.000000,
  `ChargeToDivisionID` int(11) DEFAULT NULL,
  `TaxableDailyAllowance` decimal(12,6) NOT NULL DEFAULT 0.000000,
  `SpecialHolidayPay` decimal(12,6) NOT NULL DEFAULT 0.000000,
  `SpecialHolidayOTHours` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `SpecialHolidayOTPay` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `RegularHolidayPay` decimal(12,6) NOT NULL DEFAULT 0.000000,
  `RegularHolidayOTHours` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `RegularHolidayOTPay` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `HolidayPayAmount` decimal(12,6) NOT NULL DEFAULT 0.000000,
  `TaxableDailyBonus` decimal(12,6) NOT NULL DEFAULT 0.000000,
  `NonTaxableDailyBonus` decimal(12,6) NOT NULL DEFAULT 0.000000,
  `Leavepayment` decimal(12,6) NOT NULL DEFAULT 0.000000,
  `BasicDayPay` decimal(12,6) NOT NULL DEFAULT 0.000000,
  `RestDayHours` decimal(12,6) NOT NULL DEFAULT 0.000000,
  `RestDayAmount` decimal(12,6) NOT NULL DEFAULT 0.000000,
  `RestDayOTHours` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `RestDayOTPay` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `AbsentHours` decimal(12,6) NOT NULL DEFAULT 0.000000,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Unique_Key` (`OrganizationID`,`EmployeeID`,`Date`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
