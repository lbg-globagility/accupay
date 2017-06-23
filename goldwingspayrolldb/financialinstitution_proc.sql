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

-- Dumping structure for procedure goldwingspayrolldb.financialinstitution_proc
DROP PROCEDURE IF EXISTS `financialinstitution_proc`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `financialinstitution_proc`(IN `I_Name` VARCHAR(50), IN `I_Branch` VARCHAR(50), IN `I_Created` DATETIME, IN `I_Type` VARCHAR(50), IN `I_FaxNo` VARCHAR(50), IN `I_EmailAddress` VARCHAR(50), IN `I_OrganizationID` INT(11), IN `I_ContactID` INT(11), IN `I_CreatedBy` INT(11), IN `I_LastUpd` DATETIME, IN `I_LastUpdBy` INT(11), IN `I_MainPhone` VARCHAR(50))
    DETERMINISTIC
BEGIN
INSERT INTO financialinstitution
(
	Name,
	Branch,
	Created,
	`Type`,
	FaxNo,
	EmailAddress,
	OrganizationID,
	ContactID,
	CreatedBy,
	LastUpd,
	LastUpdBy,
	MainPhone
)
VALUES
(	
	I_Name,
	I_Branch,
	I_Created,
	I_Type,
	I_FaxNo,
	I_EmailAddress,
	I_OrganizationID,
	I_ContactID,
	I_CreatedBy,
	I_LastUpd,
	I_LastUpdBy,
	I_MainPhone
);
END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
