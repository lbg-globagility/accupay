-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.12-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for procedure SP_Division
DROP PROCEDURE IF EXISTS `SP_Division`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_Division`(IN `I_Name` VARCHAR(100), IN `I_TradeName` VARCHAR(100), IN `I_OrganizationID` INT(10), IN `I_MainPhone` VARCHAR(50), IN `I_FaxNumber` VARCHAR(50), IN `I_BusinessAddress` VARCHAR(1000), IN `I_ContactName` VARCHAR(200), IN `I_EmailAddress` VARCHAR(50), IN `I_AltEmailAddress` VARCHAR(50), IN `I_AltPhone` VARCHAR(50), IN `I_URL` VARCHAR(50), IN `I_TINNo` VARCHAR(50), IN `I_Created` DATETIME, IN `I_CreatedBy` INT(11), IN `I_LastUpd` DATETIME, IN `I_LastUpdBy` INT(11), IN `I_DivisionType` VARCHAR(50), IN `I_GracePeriod` DECIMAL(11,2), IN `I_WorkDaysPerYear` INT(11), IN `I_PhHealthDeductSched` VARCHAR(100), IN `I_HDMFDeductSched` VARCHAR(100), IN `I_SSSDeductSched` VARCHAR(100), IN `I_WTaxDeductSched` VARCHAR(100), IN `I_DefaultVacationLeave` DECIMAL(11,2), IN `I_DefaultSickLeave` DECIMAL(11,2), IN `I_DefaultMaternityLeave` DECIMAL(11,2), IN `I_DefaultPaternityLeave` DECIMAL(11,2), IN `I_DefaultOtherLeave` DECIMAL(11,2), IN `I_PayFrequencyID` INT, IN `I_PhHealthDeductSchedNoAgent` VARCHAR(50), IN `I_HDMFDeductSchedNoAgent` VARCHAR(50), IN `I_SSSDeductSchedNoAgent` VARCHAR(50), IN `I_WTaxDeductSchedNoAgent` VARCHAR(50), IN `I_MinimumWageAmount` DECIMAL(11,2), IN `I_AutomaticOT` CHAR(1))
    DETERMINISTIC
BEGIN

DECLARE divisionRowID INT(11);

INSERT INTO `division`
(
    Name,
    TradeName,
    OrganizationID,
    MainPhone,
    FaxNumber,
    BusinessAddress,
    ContactName,
    EmailAddress,
    AltEmailAddress,
    AltPhone,
    URL,
    TINNo,
    Created,
    CreatedBy,
    LastUpd,
    LastUpdBy,
    DivisionType,
    GracePeriod,
    WorkDaysPerYear,
    PhHealthDeductSched,
    HDMFDeductSched,
    SSSDeductSched,
    WTaxDeductSched,
    DefaultVacationLeave,
    DefaultSickLeave,
    DefaultMaternityLeave,
    DefaultPaternityLeave,
    DefaultOtherLeave,
    PayFrequencyID,
    PhHealthDeductSchedAgency,
    HDMFDeductSchedAgency,
    SSSDeductSchedAgency,
    WTaxDeductSchedAgency,
    MinimumWageAmount,
    AutomaticOvertimeFiling
) VALUES (
    I_Name,
    I_TradeName,
    I_OrganizationID,
    I_MainPhone,
    I_FaxNumber,
    I_BusinessAddress,
    I_ContactName,
    I_EmailAddress,
    I_AltEmailAddress,
    I_AltPhone,
    I_URL,
    I_TINNo,
    I_Created,
    I_CreatedBy,
    I_LastUpd,
   I_LastUpdBy,
    I_DivisionType,
    I_GracePeriod,
    I_WorkDaysPerYear,
    I_PhHealthDeductSched,
    I_HDMFDeductSched,
    I_SSSDeductSched,
    I_WTaxDeductSched,
    I_DefaultVacationLeave,
    I_DefaultSickLeave,
    I_DefaultMaternityLeave,
    I_DefaultPaternityLeave,
    I_DefaultOtherLeave,
    I_PayFrequencyID,
    I_PhHealthDeductSchedNoAgent,
    I_HDMFDeductSchedNoAgent,
    I_SSSDeductSchedNoAgent,
    I_WTaxDeductSchedNoAgent,
    I_MinimumWageAmount,
    I_AutomaticOT);

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
