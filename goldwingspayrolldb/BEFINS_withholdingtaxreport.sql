/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFINS_withholdingtaxreport`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFINS_withholdingtaxreport` BEFORE INSERT ON `withholdingtaxreport` FOR EACH ROW BEGIN

SET NEW.MinimumWagePerDay=IFNULL(NEW.MinimumWagePerDay,0);
SET NEW.MinimumWagePerMonth=IFNULL(NEW.MinimumWagePerMonth,0);
SET NEW.PreviousTaxableIncome=IFNULL(NEW.PreviousTaxableIncome,0);
SET NEW.PremiumPaidOnHealth=IFNULL(NEW.PremiumPaidOnHealth,0);
SET NEW.PreviousTaxWithheld=IFNULL(NEW.PreviousTaxWithheld,0);
SET NEW.HazardPay=IFNULL(NEW.HazardPay,0);
SET NEW.DeMinimisBenefits=IFNULL(NEW.DeMinimisBenefits,0);
SET NEW.SalariesAndOtherCompensation=IFNULL(NEW.SalariesAndOtherCompensation,0);
SET NEW.Representation=IFNULL(NEW.Representation,0);
SET NEW.Transportation=IFNULL(NEW.Transportation,0);
SET NEW.CostOfLivingAllowance=IFNULL(NEW.CostOfLivingAllowance,0);
SET NEW.FixedHousingAllowance=IFNULL(NEW.FixedHousingAllowance,0);
SET NEW.OthersAAmount=IFNULL(NEW.OthersAAmount,0);
SET NEW.OthersBAmount=IFNULL(NEW.OthersBAmount,0);
SET NEW.OthersAName=IFNULL(NEW.OthersAName,'');
SET NEW.OthersBName=IFNULL(NEW.OthersBName,'');
SET NEW.Commission=IFNULL(NEW.Commission,0);
SET NEW.ProfitSharing=IFNULL(NEW.ProfitSharing,0);
SET NEW.FeesInclDirectorsFee=IFNULL(NEW.FeesInclDirectorsFee,0);
SET NEW.Taxable13thMonthPay=IFNULL(NEW.Taxable13thMonthPay,0);
SET NEW.TaxableHazardPay=IFNULL(NEW.TaxableHazardPay,0);
SET NEW.TaxableOvertimePay=IFNULL(NEW.TaxableOvertimePay,0);
SET NEW.SupplementaryAName=IFNULL(NEW.SupplementaryAName,'');
SET NEW.SupplementaryAAmount=IFNULL(NEW.SupplementaryAAmount,0);
SET NEW.SupplementaryBName=IFNULL(NEW.SupplementaryBName,'');
SET NEW.SupplementaryBAmount=IFNULL(NEW.SupplementaryBAmount,0);

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
