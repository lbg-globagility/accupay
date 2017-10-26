/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `CreateWithholdingTaxReport`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `CreateWithholdingTaxReport`(IN `I_EmployeeID` INT, IN `I_OrganizationID` INT, IN `I_BatchID` INT, IN `I_Category` VARCHAR(50), IN `I_IsMinimumWageEarner` TINYINT(1), IN `I_MinimumWagePerDay` DECIMAL(16,6), IN `I_MinimumWagePerMonth` DECIMAL(16,6), IN `I_PreviousTaxableIncome` DECIMAL(16,6), IN `I_PremiumPaidOnHealth` DECIMAL(16,6), IN `I_PreviousTaxWithheld` DECIMAL(16,6), IN `I_HazardPay` DECIMAL(16,6), IN `I_DeMinimisBenefits` DECIMAL(16,6), IN `I_SalariesAndOtherCompensation` vARCHAR(50), IN `I_Representation` DECIMAL(16,6), IN `I_Transportation` DECIMAL(16,6), IN `I_CostOfLivingAllowance` DECIMAL(16,6), IN `I_FixedHousingAllowance` DECIMAL(16,6), IN `I_OthersAName` VARCHAR(50), IN `I_OthersAAmount` DECIMAL(16,6), IN `I_OthersBName` VARCHAR(50), IN `I_OthersBAmount` DECIMAL(16,6), IN `I_Commission` DECIMAL(16,6), IN `I_ProfitSharing` DECIMAL(16,6), IN `I_FeesInclDirectorsFees` DECIMAL(16,6), IN `I_Taxable13thMonthPay` DECIMAL(16,6), IN `I_TaxableHazardPay` DECIMAL(16,6), IN `I_TaxableOvertimePay` DECIMAL(16,6), IN `I_SupplementaryAName` VARCHAR(50), IN `I_SupplementaryAAmount` DECIMAL(16,6), IN `I_SupplementaryBName` VARCHAR(50), IN `I_SupplementaryBAmount` DECIMAL(16,6))
    DETERMINISTIC
BEGIN

    INSERT INTO withholdingtaxreport
     (
        EmployeeID,
        OrganizationID,
        BatchID,
        CreatedBy,
        UpdatedBy,
        Category,
        IsMinimumWageEarner,
        MinimumWagePerDay,
        MinimumWagePerMonth,
        PreviousTaxableIncome,
        PremiumPaidOnHealth,
        PreviousTaxWithheld,
        HazardPay,
        DeMinimisBenefits,
        SalariesAndOtherCompensation,
        Representation,
        Transportation,
        CostOfLivingAllowance,
        FixedHousingAllowance,
        OthersAName,
        OthersAAmount,
        OthersBName,
        OthersBAmount,
        Commission,
        ProfitSharing,
        FeesInclDirectorsFee,
        Taxable13thMonthPay,
        TaxableHazardPay,
        TaxableOvertimePay,
        SupplementaryAName,
        SupplementaryAAmount,
        SupplementaryBName,
        SupplementaryBAmount
    )
     VALUES
     (
        I_EmployeeID,
        I_OrganizationID,
        I_BatchID,
        NULL,
        NULL,
        I_Category,
        I_IsMinimumWageEarner,
        I_MinimumWagePerDay,
        I_MinimumWagePerMonth,
        I_PreviousTaxableIncome,
        I_PremiumPaidOnHealth,
        I_PreviousTaxWithheld,
        I_HazardPay,
        I_DeMinimisBenefits,
        I_SalariesAndOtherCompensation,
        I_Representation,
        I_Transportation,
        I_CostOfLivingAllowance,
        I_FixedHousingAllowance,
        I_OthersAName,
        I_OthersAAmount,
        I_OthersBName,
        I_OthersBAmount,
        I_Commission,
        I_ProfitSharing,
        I_FeesInclDirectorsFees,
        I_Taxable13thMonthPay,
        I_TaxableHazardPay,
        I_TaxableOvertimePay,
        I_SupplementaryAName,
        I_SupplementaryAAmount,
        I_SupplementaryBName,
        I_SupplementaryBAmount
    )
    ON DUPLICATE KEY
    UPDATE
          Category = I_Category,
          IsMinimumWageEarner = I_IsMinimumWageEarner,
        MinimumWagePerDay = I_MinimumWagePerDay,
        MinimumWagePerMonth = I_MinimumWagePerMonth,
          PreviousTaxableIncome = I_PreviousTaxableIncome,
          PremiumPaidOnHealth = I_PremiumPaidOnHealth,
          PreviousTaxWithheld = I_PreviousTaxWithheld,
          HazardPay = I_HazardPay,
          DeMinimisBenefits = I_DeMinimisBenefits,
          SalariesAndOtherCompensation = I_SalariesAndOtherCompensation,
          Representation = I_Representation,
          Transportation = I_Transportation,
          CostOfLivingAllowance = I_CostOfLivingAllowance,
          FixedHousingAllowance = I_FixedHousingAllowance,
          OthersAName = I_OthersAName,
          OthersAAmount = I_OthersAAmount,
          OthersBName = I_OthersBName,
          OthersBAmount = I_OthersBAmount,
          Commission = I_Commission,
          ProfitSharing = I_ProfitSharing,
          FeesInclDirectorsFee = I_FeesInclDirectorsFees,
          Taxable13thMonthPay = I_Taxable13thMonthPay,
          TaxableHazardPay = I_TaxableHazardPay,
          TaxableOvertimePay = I_TaxableOvertimePay,
          SupplementaryAAmount = I_SupplementaryAAmount,
          SupplementaryBAmount = I_SupplementaryBAmount;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
