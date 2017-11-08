/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `employeetimeentry` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastUpdBy` int(10) DEFAULT NULL,
  `Date` date NOT NULL COMMENT 'time entry date',
  `EmployeeShiftID` int(10) DEFAULT NULL,
  `EmployeeID` int(10) DEFAULT NULL,
  `EmployeeSalaryID` int(10) DEFAULT NULL,
  `EmployeeFixedSalaryFlag` char(1) DEFAULT NULL COMMENT 'Flag is derived from EmployeeSalary table. flag to indicate if employee is on fixed salary, thus no overtime pay is calculated. TotalDayPay is fixed regardless of hours worked.',
  `RegularHoursWorked` decimal(10,2) DEFAULT NULL,
  `RegularHoursAmount` decimal(11,6) DEFAULT NULL,
  `TotalHoursWorked` decimal(10,2) DEFAULT NULL,
  `OvertimeHoursWorked` decimal(10,2) DEFAULT NULL,
  `OvertimeHoursAmount` decimal(11,6) DEFAULT NULL,
  `UndertimeHours` decimal(10,2) DEFAULT NULL,
  `UndertimeHoursAmount` decimal(11,6) DEFAULT NULL,
  `NightDifferentialHours` decimal(10,2) DEFAULT NULL,
  `NightDiffHoursAmount` decimal(11,6) DEFAULT NULL,
  `NightDifferentialOTHours` decimal(10,2) DEFAULT NULL,
  `NightDiffOTHoursAmount` decimal(11,6) DEFAULT NULL,
  `HoursLate` decimal(10,2) DEFAULT NULL,
  `HoursLateAmount` decimal(11,6) DEFAULT NULL,
  `LateFlag` varchar(50) DEFAULT NULL,
  `PayRateID` int(11) DEFAULT NULL COMMENT 'Defines if the day is a Holiday, Special Holiday, Regular Day, HolidayType',
  `VacationLeaveHours` decimal(10,2) DEFAULT NULL,
  `SickLeaveHours` decimal(10,2) DEFAULT NULL,
  `MaternityLeaveHours` decimal(10,2) DEFAULT NULL,
  `OtherLeaveHours` decimal(10,2) DEFAULT NULL,
  `TotalDayPay` decimal(11,6) DEFAULT NULL COMMENT 'calculated based on PayRate, EmployeeSalary,Hours Worked, and EmployeeShift',
  `Absent` decimal(10,2) DEFAULT NULL,
  `ChargeToDivisionID` int(11) DEFAULT NULL,
  `TaxableDailyAllowance` decimal(11,6) DEFAULT '0.000000',
  `HolidayPayAmount` decimal(11,6) DEFAULT '0.000000',
  `TaxableDailyBonus` decimal(11,6) DEFAULT '0.000000',
  `NonTaxableDailyBonus` decimal(11,6) DEFAULT '0.000000',
  `Leavepayment` decimal(11,6) DEFAULT '0.000000',
  `BasicDayPay` decimal(11,6) DEFAULT '0.000000',
  `RestDayHours` decimal(11,6) DEFAULT '0.000000',
  `RestDayAmount` decimal(11,6) DEFAULT '0.000000',
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 8` (`Date`,`EmployeeID`,`OrganizationID`),
  KEY `FK_BaseTables_organization` (`OrganizationID`),
  KEY `FK_BaseTables_user` (`CreatedBy`),
  KEY `FK_BaseTables_user_2` (`LastUpdBy`),
  KEY `FK_employeetimeentry_employee` (`EmployeeID`),
  KEY `FK_employeetimeentry_employeesalary` (`EmployeeSalaryID`),
  KEY `FK_employeetimeentry_payrate` (`PayRateID`),
  KEY `FK_employeetimeentry_employeeshift` (`EmployeeShiftID`),
  CONSTRAINT `FK_employeetimeentry_employee` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`),
  CONSTRAINT `FK_employeetimeentry_employeesalary` FOREIGN KEY (`EmployeeSalaryID`) REFERENCES `employeesalary` (`RowID`),
  CONSTRAINT `FK_employeetimeentry_employeeshift` FOREIGN KEY (`EmployeeShiftID`) REFERENCES `employeeshift` (`RowID`),
  CONSTRAINT `FK_employeetimeentry_payrate` FOREIGN KEY (`PayRateID`) REFERENCES `payrate` (`RowID`),
  CONSTRAINT `employeetimeentry_ibfk_1` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `employeetimeentry_ibfk_2` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `employeetimeentry_ibfk_3` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB AUTO_INCREMENT=18001 DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
