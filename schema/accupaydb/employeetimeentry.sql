/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `employeetimeentry` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT current_timestamp(),
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `LastUpdBy` int(10) DEFAULT NULL,
  `Date` date NOT NULL COMMENT 'time entry date',
  `EmployeeID` int(10) DEFAULT NULL,
  `BranchID` int(10) DEFAULT NULL,
  `EmployeeFixedSalaryFlag` char(1) DEFAULT NULL COMMENT 'Flag is derived from EmployeeSalary table. flag to indicate if employee is on fixed salary, thus no overtime pay is calculated. TotalDayPay is fixed regardless of hours worked.',
  `ShiftHours` decimal(10,2) NOT NULL DEFAULT 0.00,
  `WorkHours` decimal(10,2) NOT NULL DEFAULT 0.00,
  `IsRestDay` tinyint(1) NOT NULL DEFAULT 0,
  `HasShift` tinyint(1) NOT NULL DEFAULT 0,
  `RegularHoursWorked` decimal(10,2) NOT NULL DEFAULT 0.00,
  `RegularHoursAmount` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `TotalHoursWorked` decimal(10,2) NOT NULL DEFAULT 0.00,
  `OvertimeHoursWorked` decimal(10,2) NOT NULL DEFAULT 0.00,
  `OvertimeHoursAmount` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `UndertimeHours` decimal(10,2) NOT NULL DEFAULT 0.00,
  `UndertimeHoursAmount` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `NightDifferentialHours` decimal(10,2) NOT NULL DEFAULT 0.00,
  `NightDiffHoursAmount` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `NightDifferentialOTHours` decimal(10,2) NOT NULL DEFAULT 0.00,
  `NightDiffOTHoursAmount` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `HoursLate` decimal(11,2) NOT NULL DEFAULT 0.00,
  `HoursLateAmount` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `LateFlag` varchar(50) DEFAULT NULL,
  `PayRateID` int(11) DEFAULT NULL COMMENT 'Defines if the day is a Holiday, Special Holiday, Regular Day, HolidayType',
  `VacationLeaveHours` decimal(10,2) NOT NULL DEFAULT 0.00,
  `SickLeaveHours` decimal(10,2) NOT NULL DEFAULT 0.00,
  `MaternityLeaveHours` decimal(10,2) NOT NULL DEFAULT 0.00,
  `OtherLeaveHours` decimal(10,2) NOT NULL DEFAULT 0.00,
  `TotalDayPay` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `Absent` decimal(10,2) NOT NULL DEFAULT 0.00,
  `ChargeToDivisionID` int(11) DEFAULT NULL,
  `TaxableDailyAllowance` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `SpecialHolidayHours` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `SpecialHolidayPay` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `SpecialHolidayOTHours` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `SpecialHolidayOTPay` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `RegularHolidayHours` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `RegularHolidayPay` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `RegularHolidayOTHours` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `RegularHolidayOTPay` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `HolidayPayAmount` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `TaxableDailyBonus` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `NonTaxableDailyBonus` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `Leavepayment` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `BasicDayPay` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `RestDayHours` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `RestDayAmount` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `RestDayOTHours` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `RestDayOTPay` decimal(11,6) NOT NULL DEFAULT 0.000000,
  `AbsentHours` decimal(11,6) NOT NULL DEFAULT 0.000000,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 8` (`Date`,`EmployeeID`,`OrganizationID`),
  KEY `FK_BaseTables_organization` (`OrganizationID`),
  KEY `FK_BaseTables_user` (`CreatedBy`),
  KEY `FK_BaseTables_user_2` (`LastUpdBy`),
  KEY `FK_employeetimeentry_employee` (`EmployeeID`),
  KEY `FK_employeetimeentry_payrate` (`PayRateID`),
  KEY `FK_employeetimeentry_branch_BranchID` (`BranchID`),
  CONSTRAINT `FK_employeetimeentry_branch_BranchID` FOREIGN KEY (`BranchID`) REFERENCES `branch` (`RowID`),
  CONSTRAINT `FK_employeetimeentry_employee_EmployeeID` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `FK_employeetimeentry_payrate_PayRateID` FOREIGN KEY (`PayRateID`) REFERENCES `payrate` (`RowID`),
  CONSTRAINT `FK_employeetimeentry_user_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_employeetimeentry_user_LastUpdBy` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `employeetimeentry_ibfk_1` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
