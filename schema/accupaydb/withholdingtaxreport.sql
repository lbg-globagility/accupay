/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `withholdingtaxreport` (
  `RowID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `BatchID` int(10) unsigned NOT NULL,
  `EmployeeID` int(10) unsigned NOT NULL,
  `OrganizationID` int(10) unsigned DEFAULT NULL,
  `Created` timestamp NULL DEFAULT current_timestamp(),
  `CreatedBy` int(10) unsigned DEFAULT NULL,
  `Updated` timestamp NULL DEFAULT NULL ON UPDATE current_timestamp(),
  `UpdatedBy` int(10) unsigned DEFAULT NULL,
  `Category` varchar(50) DEFAULT NULL,
  `IsMinimumWageEarner` tinyint(1) unsigned DEFAULT NULL,
  `MinimumWagePerDay` decimal(10,0) DEFAULT NULL,
  `MinimumWagePerMonth` decimal(10,0) DEFAULT NULL,
  `PreviousTaxableIncome` decimal(16,6) DEFAULT NULL,
  `PremiumPaidOnHealth` decimal(16,6) DEFAULT NULL,
  `PreviousTaxWithheld` decimal(16,6) DEFAULT NULL,
  `HazardPay` decimal(16,6) DEFAULT NULL,
  `DeMinimisBenefits` decimal(16,6) DEFAULT NULL,
  `SalariesAndOtherCompensation` decimal(16,6) DEFAULT NULL,
  `Representation` decimal(16,6) DEFAULT NULL,
  `Transportation` decimal(16,6) DEFAULT NULL,
  `CostOfLivingAllowance` decimal(16,6) DEFAULT NULL,
  `FixedHousingAllowance` decimal(16,6) DEFAULT NULL,
  `OthersAAmount` decimal(16,6) DEFAULT NULL,
  `OthersBAmount` decimal(16,6) DEFAULT NULL,
  `OthersAName` varchar(50) DEFAULT NULL,
  `OthersBName` varchar(50) DEFAULT NULL,
  `Commission` decimal(16,6) DEFAULT NULL,
  `ProfitSharing` decimal(16,6) DEFAULT NULL,
  `FeesInclDirectorsFee` decimal(16,6) DEFAULT NULL,
  `Taxable13thMonthPay` decimal(16,6) DEFAULT NULL,
  `TaxableHazardPay` decimal(16,6) DEFAULT NULL,
  `TaxableOvertimePay` decimal(16,6) DEFAULT NULL,
  `SupplementaryAName` varchar(50) DEFAULT NULL,
  `SupplementaryAAmount` decimal(16,6) DEFAULT NULL,
  `SupplementaryBName` varchar(50) DEFAULT NULL,
  `SupplementaryBAmount` decimal(16,6) DEFAULT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `employee_per_batch` (`BatchID`,`EmployeeID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
