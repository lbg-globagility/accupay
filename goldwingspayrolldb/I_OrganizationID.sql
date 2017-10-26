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

-- Dumping structure for procedure I_OrganizationID
DROP PROCEDURE IF EXISTS `I_OrganizationID`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_OrganizationID`(IN `I_RowID` INT(10), IN `I_Name` VARCHAR(100), IN `I_PrimaryAddressID` INT(10), IN `I_PrimaryContactID` INT(10), IN `I_MainPhone` VARCHAR(50), IN `I_FaxNumber` VARCHAR(50), IN `I_EmailAddress` VARCHAR(50), IN `I_AltEmailAddress` VARCHAR(50), IN `I_AltPhone` VARCHAR(50), IN `I_URL` VARCHAR(50), IN `I_Created` DATETIME, IN `I_CreatedBy` INT(11), IN `I_LastUpd` DATETIME, IN `I_LastUpdBy` INT(11)
, IN `I_VacationLeaveDays` DECIMAL(11,3), IN `I_SickLeaveDays` DECIMAL(11,3), IN `I_MaternityLeaveDays` DECIMAL(11,3), IN `I_PayFrequencyID` INT)
    DETERMINISTIC
BEGIN
INSERT INTO organization
    (
    RowID,
    Name,
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
    VacationLeaveDays,
    SickLeaveDays,
    MaternityLeaveDays,
    PayFrequencyID
    )
VALUES
    (
    I_RowID,
    I_Name,
    I_PrimaryAddressID,
    I_PrimaryContactID,
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
    I_VacationLeaveDays,
    I_SickLeaveDays,
    I_MaternityLeaveDays,
    I_PayFrequencyID
    );END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
