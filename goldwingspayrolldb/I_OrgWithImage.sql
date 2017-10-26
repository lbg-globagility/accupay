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

-- Dumping structure for procedure I_OrgWithImage
DROP PROCEDURE IF EXISTS `I_OrgWithImage`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_OrgWithImage`(IN `I_Name` VARCHAR(100), IN `I_TradeName` VARCHAR(100), IN `I_PrimaryAddressID` INT(10), IN `I_PrimaryContactID` INT(10), IN `I_MainPhone` VARCHAR(50), IN `I_FaxNumber` VARCHAR(50), IN `I_EmailAddress` VARCHAR(50), IN `I_AltEmailAddress` VARCHAR(50), IN `I_AltPhone` VARCHAR(50), IN `I_URL` VARCHAR(50), IN `I_Created` DATETIME, IN `I_CreatedBy` INT(11), IN `I_LastUpd` DATETIME, IN `I_LastUpdBy` INT(11)
, IN `I_TINNo` VARCHAR(50), IN `I_OrganizationType` VARCHAR(50), IN `I_Image` LONGBLOB, IN `I_VacationLeaveDays` DECIMAL(11,3), IN `I_SickLeaveDays` DECIMAL(11,3), IN `I_MaternityLeaveDays` DECIMAL(11,3), IN `I_OthersLeaveDays` DECIMAL(11,3), IN `I_NightDifferentialTimeFrom` TIME, IN `I_NightDifferentialTimeTo` TIME, IN `I_NightShiftTimeFrom` TIME, IN `I_NightShiftTimeTo` TIME, IN `I_PhilhealthDeductionSchedule` VARCHAR(50), IN `I_SSSDeductionSchedule` VARCHAR(50), IN `I_PagIbigDeductionSchedule` VARCHAR(50), IN `I_PayFrequencyID` INT, IN `I_WorkDaysPerYear` INT, IN `I_RDOCode` CHAR(50), IN `I_ZIPCode` CHAR(50), IN `I_WithholdingDeductionSchedule` CHAR(50))
    DETERMINISTIC
BEGIN
INSERT INTO organization
    (
    Name,
    TradeName,
    PrimaryAddressID,
    PrimaryContactID,
    MainPhone,
    FaxNumber,
    EmailAddress,
    AltEmailAddress,
    AltPhone,
    URL,
    Created,
    CreatedBy,
    LastUpd,
    LastUpdBy,
    TINNo,
    OrganizationType,
    VacationLeaveDays,
    SickLeaveDays,
    MaternityLeaveDays,
    OthersLeaveDays,
    NightDifferentialTimeFrom,
    NightDifferentialTimeTo,
    NightShiftTimeFrom,
    NightShiftTimeTo,
    PhilhealthDeductionSchedule,
    SSSDeductionSchedule,
    PagIbigDeductionSchedule,
    PayFrequencyID,
    Image,
    WorkDaysPerYear,
    RDOCode,
    ZIPCode,
    WithholdingDeductionSchedule
    )
VALUES
    (
    I_Name,
    I_TradeName,
    IF(I_PrimaryAddressID=0,NULL,I_PrimaryAddressID),
    IF(I_PrimaryContactID=0,NULL,I_PrimaryContactID),
    I_MainPhone,
    I_FaxNumber,
    I_EmailAddress,
    I_AltEmailAddress,
    I_AltPhone,
    I_URL,
    I_Created,
    I_CreatedBy,
    I_LastUpd,
    I_LastUpdBy,
    I_TINNo,
    I_OrganizationType,
    I_VacationLeaveDays,
    I_SickLeaveDays,
    I_MaternityLeaveDays,
    I_OthersLeaveDays,
    I_NightDifferentialTimeFrom,
    I_NightDifferentialTimeTo,
    I_NightShiftTimeFrom,
    I_NightShiftTimeTo,
    I_PhilhealthDeductionSchedule,
    I_SSSDeductionSchedule,
    I_PagIbigDeductionSchedule,
    I_PayFrequencyID,
    I_Image,
    IF(I_WorkDaysPerYear=0, IF(DAY(LAST_DAY(ADDDATE(MAKEDATE(YEAR(CURDATE()),1), INTERVAL 1 MONTH))) <= 28, 260, 261), I_WorkDaysPerYear),
    I_RDOCode,
    I_ZIPCode,
    I_WithholdingDeductionSchedule
    ); END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
