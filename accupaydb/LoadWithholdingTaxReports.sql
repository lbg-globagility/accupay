/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `LoadWithholdingTaxReports`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `LoadWithholdingTaxReports`(IN `OrganizationID` INT, IN `BatchID` INT)
    DETERMINISTIC
BEGIN

    SELECT
        w.RowID,
        w.EmployeeID,
        w.OrganizationID,
        w.`Category`,
        w.PreviousTaxableIncome,
        w.PremiumPaidOnHealth,
        w.PreviousTaxWithheld,
        w.HazardPay,
        w.DeMinimisBenefits,
        w.SalariesAndOtherCompensation,
        w.Representation,
        w.Transportation,
        w.CostOfLivingAllowance,
        w.FixedHousingAllowance,
        w.OthersAName,
        w.OthersAAmount,
        w.OthersBName,
        w.OthersBAmount,
        w.Commission,
        w.ProfitSharing,
        w.FeesInclDirectorsFee,
        w.Taxable13thMonthPay,
        w.TaxableHazardPay,
        w.TaxableOvertimePay,
        w.SupplementaryAAmount,
        w.SupplementaryBAmount
    FROM withholdingtaxreport AS w
    WHERE w.OrganizationID = OrganizationID
        AND w.BatchID = BatchID;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
