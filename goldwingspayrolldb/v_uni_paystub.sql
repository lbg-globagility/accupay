/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP VIEW IF EXISTS `v_uni_paystub`;
DROP TABLE IF EXISTS `v_uni_paystub`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` VIEW `v_uni_paystub` AS SELECT RowID,OrganizationID,PayPeriodID,EmployeeID,TimeEntryID,PayFromDate,PayToDate,TotalGrossSalary,TotalNetSalary,TotalTaxableSalary,TotalEmpSSS,TotalEmpWithholdingTax,TotalCompSSS,TotalEmpPhilhealth,TotalCompPhilhealth,TotalEmpHDMF,TotalCompHDMF,TotalVacationDaysLeft,TotalLoans,TotalBonus,TotalAllowance,TotalAdjustments,ThirteenthMonthInclusion,FirstTimeSalary,0 `AsActual` FROM paystub
UNION
SELECT RowID,OrganizationID,PayPeriodID,EmployeeID,TimeEntryID,PayFromDate,PayToDate,TotalGrossSalary,TotalNetSalary,TotalTaxableSalary,TotalEmpSSS,TotalEmpWithholdingTax,TotalCompSSS,TotalEmpPhilhealth,TotalCompPhilhealth,TotalEmpHDMF,TotalCompHDMF,TotalVacationDaysLeft,TotalLoans,TotalBonus,TotalAllowance,TotalAdjustments,ThirteenthMonthInclusion,FirstTimeSalary,1 `AsActual` FROM paystubactual ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
