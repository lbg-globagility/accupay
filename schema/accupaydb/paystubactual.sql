/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `paystubactual` (
  `RowID` int(10) NOT NULL,
  `OrganizationID` int(10) DEFAULT NULL,
  `PayPeriodID` int(10) DEFAULT NULL,
  `EmployeeID` int(10) DEFAULT NULL,
  `TimeEntryID` int(10) DEFAULT NULL,
  `PayFromDate` date DEFAULT NULL,
  `PayToDate` date DEFAULT NULL,
  `RegularPay` decimal(20,6) DEFAULT NULL,
  `OvertimePay` decimal(20,6) DEFAULT NULL,
  `NightDiffPay` decimal(20,6) DEFAULT NULL,
  `NightDiffOvertimePay` decimal(20,6) DEFAULT NULL,
  `RestDayPay` decimal(20,6) DEFAULT NULL,
  `LeavePay` decimal(20,6) DEFAULT NULL,
  `HolidayPay` decimal(20,6) DEFAULT NULL,
  `LateDeduction` decimal(20,6) DEFAULT NULL,
  `UndertimeDeduction` decimal(20,6) DEFAULT NULL,
  `AbsenceDeduction` decimal(20,6) DEFAULT NULL,
  `PaySubtotal` decimal(20,6) DEFAULT NULL,
  `DeductionSubtotal` decimal(20,6) DEFAULT NULL,
  `TotalGrossSalary` decimal(20,6) DEFAULT NULL,
  `TotalNetSalary` decimal(20,6) DEFAULT '0.000000',
  `TotalTaxableSalary` decimal(20,6) DEFAULT '0.000000',
  `TotalEmpSSS` decimal(10,2) DEFAULT '0.00',
  `TotalEmpWithholdingTax` decimal(20,6) DEFAULT '0.000000',
  `TotalCompSSS` decimal(10,2) DEFAULT '0.00',
  `TotalEmpPhilhealth` decimal(10,2) DEFAULT '0.00',
  `TotalCompPhilhealth` decimal(10,2) DEFAULT '0.00',
  `TotalEmpHDMF` decimal(10,2) DEFAULT '0.00',
  `TotalCompHDMF` decimal(10,2) DEFAULT '0.00',
  `TotalVacationDaysLeft` decimal(10,2) DEFAULT '0.00',
  `TotalLoans` decimal(10,2) DEFAULT '0.00',
  `TotalBonus` decimal(20,6) DEFAULT '0.000000',
  `TotalAllowance` decimal(20,6) DEFAULT '0.000000',
  `TotalAdjustments` decimal(20,6) DEFAULT '0.000000',
  `ThirteenthMonthInclusion` char(1) DEFAULT '0',
  `FirstTimeSalary` char(1) DEFAULT '0',
  PRIMARY KEY (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
