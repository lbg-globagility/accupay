/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_paywithholdingtax`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_paywithholdingtax`(`wtx_RowID` INT, `wtx_CreatedBy` INT, `wtx_LastUpdBy` INT, `wtx_PayFrequencyID` INT, `wtx_FilingStatusID` INT, `wtx_EffectiveDateFrom` DATE, `wtx_EffectiveDateTo` DATE, `wtx_ExemptionAmount` DECIMAL(10,2), `wtx_ExemptionInExcessAmount` DECIMAL(10,2), `wtx_TaxableIncomeFromAmount` DECIMAL(10,2), `wtx_TaxableIncomeToAmount` DECIMAL(10,2)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnval INT(11);

INSERT INTO paywithholdingtax
(
    RowID
    ,Created
    ,CreatedBy
    ,PayFrequencyID
    ,FilingStatusID
    ,EffectiveDateFrom
    ,EffectiveDateTo
    ,ExemptionAmount
    ,ExemptionInExcessAmount
    ,TaxableIncomeFromAmount
    ,TaxableIncomeToAmount
) VALUES (
    wtx_RowID
    ,CURRENT_TIMESTAMP()
    ,wtx_CreatedBy
    ,wtx_PayFrequencyID
    ,wtx_FilingStatusID
    ,wtx_EffectiveDateFrom
    ,wtx_EffectiveDateTo
    ,wtx_ExemptionAmount
    ,wtx_ExemptionInExcessAmount
    ,wtx_TaxableIncomeFromAmount
    ,wtx_TaxableIncomeToAmount
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=wtx_LastUpdBy
    ,PayFrequencyID=wtx_PayFrequencyID
    ,FilingStatusID=wtx_FilingStatusID
    ,EffectiveDateFrom=wtx_EffectiveDateFrom
    ,EffectiveDateTo=wtx_EffectiveDateTo
    ,ExemptionAmount=wtx_ExemptionAmount
    ,ExemptionInExcessAmount=wtx_ExemptionInExcessAmount
    ,TaxableIncomeFromAmount=wtx_TaxableIncomeFromAmount
    ,TaxableIncomeToAmount=wtx_TaxableIncomeToAmount;SELECT @@Identity AS id INTO returnval;

RETURN returnval;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
