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

-- Dumping structure for procedure SP_EmployeePreviousEmployer
DROP PROCEDURE IF EXISTS `SP_EmployeePreviousEmployer`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_EmployeePreviousEmployer`(IN `I_Name` VARCHAR(100), IN `I_TradeName` VARCHAR(100), IN `I_OrganizationID` INT(10), IN `I_MainPhone` VARCHAR(50), IN `I_FaxNumber` VARCHAR(50), IN `I_JobTitle` VARCHAR(50), IN `I_ExperienceFromTo` VARCHAR(50), IN `I_BusinessAddress` VARCHAR(1000), IN `I_ContactName` VARCHAR(200), IN `I_EmailAddress` VARCHAR(50), IN `I_AltEmailAddress` VARCHAR(50), IN `I_AltPhone` VARCHAR(50), IN `I_URL` VARCHAR(50), IN `I_TINNo` VARCHAR(50), IN `I_JobFunction` VARCHAR(2000), IN `I_Created` DATETIME, IN `I_CreatedBy` INT(10), IN `I_LastUpd` DATETIME, IN `I_LastUpdBy` INT(10), IN `I_OrganizationType` VARCHAR(50), IN `I_EmployeeID` INT(10))
    DETERMINISTIC
BEGIN
INSERT INTO employeepreviousemployer
(
    Name,
    TradeName,
    OrganizationID,
    MainPhone,
    FaxNumber,
    JobTitle,
    ExperienceFromTo,
    BusinessAddress,
    ContactName,
    EmailAddress,
    AltEmailAddress,
    AltPhone,
    URL,
    TINNo,
    JobFunction,
    Created,
    CreatedBy,
    LastUpd,
    LastUpdBy,
    OrganizationType,
    EmployeeID
    )
    VALUES
    (
    I_Name,
    I_TradeName,
    I_OrganizationID,
    I_MainPhone,
    I_FaxNumber,
    I_JobTitle,
    I_ExperienceFromTo,
    I_BusinessAddress,
    I_ContactName,
    I_EmailAddress,
    I_AltEmailAddress,
    I_AltPhone,
    I_URL,
    I_TINNo,
    I_JobFunction,
    I_Created,
    I_CreatedBy,
    I_LastUpd,
    I_LastUpdBy,
    I_OrganizationType,
    I_EmployeeID
    );
END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
