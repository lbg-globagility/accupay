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

-- Dumping structure for procedure goldwingspayrolldb.I_ClientName
DROP PROCEDURE IF EXISTS `I_ClientName`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_ClientName`(IN `I_Created` DATETIME, IN `I_OrganizationID` INT(11), IN `I_LastName` VARCHAR(50), IN `I_FirstName` VARCHAR(50), IN `I_MiddleName` VARCHAR(50), IN `I_LastUpd` DATETIME, IN `I_CreatedBy` INT(11), IN `I_LastUpdBy` INT(11)
)
BEGIN 
INSERT INTO contact
(
	Created,
	OrganizationID,
	LastName,
	FirstName,
	MiddleName,
	LastUpd,
	CreatedBy,
	LastUpdBy
)
VALUES
(
	I_Created,
	I_OrganizationID,
	I_LastName,
	I_FirstName,
	I_MiddleName,
	I_LastUpd,
	I_CreatedBy,
	I_LastUpdBy
);END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
