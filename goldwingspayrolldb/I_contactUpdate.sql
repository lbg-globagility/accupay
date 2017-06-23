-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.11-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.0.0.4396
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for procedure goldwingspayrolldb.I_contactUpdate
DROP PROCEDURE IF EXISTS `I_contactUpdate`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_contactUpdate`(IN `I_Status` VARCHAR(50), IN `I_MainPhone` VARCHAR(50), IN `I_LastName` VARCHAR(50), IN `I_FirstName` VARCHAR(50), IN `I_MiddleName` VARCHAR(50), IN `I_MobilePhone` VARCHAR(50), IN `I_WorkPhone` VARCHAR(50), IN `I_Gender` VARCHAR(50), IN `I_JobTitle` VARCHAR(50), IN `I_EmailAddress` VARCHAR(50), IN `I_AlternatePhone` VARCHAR(50), IN `I_FaxNumber` VARCHAR(50), IN `I_LastUpd` DATETIME, IN `I_LastUpdBy` INT(11), IN `I_Personaltitle` VARCHAR(50), IN `I_Type` VARCHAR(50), IN `I_Suffix` VARCHAR(50), IN `I_AddressID` INT(11), IN `I_TINNumber` VARCHAR(50), IN `I_RowID` INT(11))
    DETERMINISTIC
BEGIN
UPDATE contact 
SET
	Status = I_Status,
	MainPhone = I_MainPhone,
	LastName = I_LastName,
	FirstName = I_FirstName,
	MiddleName = I_MiddleName,
	MobilePhone = I_MobilePhone,
	WorkPhone = I_WorkPhone,
	Gender = I_Gender,
	JobTitle = I_JobTitle,
	EmailAddress = I_EmailAddress,
	AlternatePhone = I_AlternatePhone,
	FaxNumber = I_FaxNumber,
	LastUpd = I_LastUpd,
	LastUpdBy = I_LastUpdBy,
	PersonalTitle = I_PersonalTitle,
	`Type` = I_Type,
	Suffix = I_Suffix,
	AddressID = IF(COALESCE(I_AddressID,0) = 0, NULL, I_AddressID),
	TINNumber = I_TINNumber
WHERE RowID = I_RowID;
END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
