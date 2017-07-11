/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `SP_DivisionUpdate`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_DivisionUpdate`(IN `I_Name` VARCHAR(100), IN `I_TradeName` VARCHAR(100), IN `I_MainPhone` VARCHAR(50), IN `I_FaxNumber` VARCHAR(50), IN `I_BusinessAddress` VARCHAR(1000), IN `I_ContactName` VARCHAR(200), IN `I_EmailAddress` VARCHAR(50), IN `I_AltEmailAddress` VARCHAR(50), IN `I_AltPhone` VARCHAR(50), IN `I_URL` VARCHAR(50), IN `I_TINNo` VARCHAR(50), IN `I_LastUpd` DATETIME, IN `I_LastUpdBy` INT(11), IN `I_DivisionType` VARCHAR(50), IN `I_RowID` INT(11), IN `I_GracePeriod` DECIMAL(11,2), IN `I_WorkDaysPerYear` INT(11), IN `I_PhHealthDeductSched` VARCHAR(100), IN `I_HDMFDeductSched` VARCHAR(100), IN `I_SSSDeductSched` VARCHAR(100), IN `I_WTaxDeductSched` VARCHAR(100), IN `I_DefaultVacationLeave` DECIMAL(11,2), IN `I_DefaultSickLeave` DECIMAL(11,2), IN `I_DefaultMaternityLeave` DECIMAL(11,2), IN `I_DefaultPaternityLeave` DECIMAL(11,2), IN `I_DefaultOtherLeave` DECIMAL(11,2), IN `I_PayFrequencyID` INT, IN `I_PhHealthDeductSchedNoAgent` VARCHAR(50), IN `I_HDMFDeductSchedNoAgent` VARCHAR(50), IN `I_SSSDeductSchedNoAgent` VARCHAR(50), IN `I_WTaxDeductSchedNoAgent` VARCHAR(50), IN `I_MinimumWageAmount` DECIMAL(11,2), IN `I_AutomaticOT` CHAR(1))
    DETERMINISTIC
BEGIN
UPDATE division
SET
    Name = I_Name,
    TradeName = I_TradeName,
    MainPhone = I_MainPhone,
    FaxNumber = I_FaxNumber,
    BusinessAddress = I_BusinessAddress,
    ContactName = I_ContactName,
    EmailAddress = I_EmailAddress,
    AltEmailAddress = I_AltEmailAddress,
    AltPhone = I_AltPhone,
    URL = I_URL,
    TINNo = I_TINNo,
    LastUpd = I_LastUpd,
    LastUpdBy = I_LastUpdBy,
    DivisionType = I_DivisionType,
    GracePeriod = I_GracePeriod,
    WorkDaysPerYear = I_WorkDaysPerYear,
    PhHealthDeductSched = I_PhHealthDeductSched,
    HDMFDeductSched = I_HDMFDeductSched,
    SSSDeductSched = I_SSSDeductSched,
    WTaxDeductSched = I_WTaxDeductSched,
    DefaultVacationLeave = I_DefaultVacationLeave,
    DefaultSickLeave = I_DefaultSickLeave,
    DefaultMaternityLeave = I_DefaultMaternityLeave,
    DefaultPaternityLeave = I_DefaultPaternityLeave,
    DefaultOtherLeave = I_DefaultOtherLeave,
    PayFrequencyID = I_PayFrequencyID,
    PhHealthDeductSchedAgency = I_PhHealthDeductSchedNoAgent,
    HDMFDeductSchedAgency = I_HDMFDeductSchedNoAgent,
    SSSDeductSchedAgency = I_SSSDeductSchedNoAgent,
    WTaxDeductSchedAgency = I_WTaxDeductSchedNoAgent,
    MinimumWageAmount = I_MinimumWageAmount,
    AutomaticOvertimeFiling = I_AutomaticOT
WHERE RowID = I_RowID;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
