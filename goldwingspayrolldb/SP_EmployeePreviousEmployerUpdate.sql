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

-- Dumping structure for procedure SP_EmployeePreviousEmployerUpdate
DROP PROCEDURE IF EXISTS `SP_EmployeePreviousEmployerUpdate`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_EmployeePreviousEmployerUpdate`(IN `I_Name` VARCHAR(100), IN `I_TradeName` VARCHAR(100), IN `I_MainPhone` VARCHAR(50), IN `I_FaxNumber` VARCHAR(50), IN `I_JobTitle` VARCHAR(50), IN `I_ExperienceFromTo` VARCHAR(50), IN `I_BusinessAddress` VARCHAR(1000), IN `I_ContactName` VARCHAR(200), IN `I_EmailAddress` VARCHAR(50), IN `I_AltEmailAddress` VARCHAR(50), IN `I_AltPhone` VARCHAR(50), IN `I_URL` VARCHAR(50), IN `I_TINNo` VARCHAR(50), IN `I_JobFunction` VARCHAR(2000), IN `I_OrganizationType` VARCHAR(50), IN `I_RowID` INT(10))
    DETERMINISTIC
BEGIN
UPDATE employeepreviousemployer
SET

    Name = I_Name,
    TradeName = I_TradeName,
    MainPhone = I_MainPhone,
    FaxNumber = I_FaxNumber,
    JobTitle = I_JobTitle,
    ExperienceFromTo = I_ExperienceFromTo,
    BusinessAddress = I_BusinessAddress,
    ContactName = I_ContactName,
    EmailAddress = I_EmailAddress,
    AltEmailAddress = I_AltEmailAddress,
    AltPhone = I_AltPhone,
    URL = I_URL,
    TINNo = I_TINNo,
    JobFunction = I_JobFunction,
    OrganizationType = I_OrganizationType
WHERE RowID = I_RowID;
END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
