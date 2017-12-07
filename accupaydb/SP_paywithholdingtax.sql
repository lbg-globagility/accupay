/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `SP_paywithholdingtax`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_paywithholdingtax`(IN `I_Created` DATETIME, IN `I_LastUpd` DATETIME, IN `I_CreatedBy` INT(10), IN `I_LastUpdBy` INT(10), IN `I_PayFrequencyID` INT(11), IN `I_FilingStatusID` INT(11), IN `I_EffectiveDateFrom` DATE, IN `I_EffectiveDateTo` DATE, IN `I_ExemptionAmount` DECIMAL(10,2), IN `I_ExemptionInExcessAmount` DECIMAL(10,2), IN `I_TaxableIncomeFromAmount` DECIMAL(10,2), IN `I_TaxableIncomeToAmount` DECIMAL(10,2))
    DETERMINISTIC
BEGIN
INSERT INTO paywithholdingtax
(
    Created,
    LastUpd,
    CreatedBy,
    LastUpdBy,
    PayFrequencyID,
    FilingStatusID,
    EffectiveDateFrom,
    EffectiveDateTo,
    ExemptionAmount,
    ExemptionInExcessAmount,
    TaxableIncomeFromAmount,
    TaxableIncomeToAmount
)
VALUES
(
    I_Created,
    I_LastUpd,
    I_CreatedBy,
    I_LastUpdBy,
    I_PayFrequencyID,
    I_FilingStatusID,
    I_EffectiveDateFrom,
    I_EffectiveDateTo,
    I_ExemptionAmount,
    I_ExemptionInExcessAmount,
    I_TaxableIncomeFromAmount,
    I_TaxableIncomeToAmount
);
END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
