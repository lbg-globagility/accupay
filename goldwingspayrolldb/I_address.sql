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

-- Dumping structure for procedure goldwingspayrolldb.I_address
DROP PROCEDURE IF EXISTS `I_address`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_address`(IN `I_StreetAddress1` VARCHAR(200), IN `I_StreetAddress2` VARCHAR(50), IN `I_CityTown` VARCHAR(50), IN `I_Country` VARCHAR(50), IN `I_State` VARCHAR(50), IN `I_CreatedBy` INT(11), IN `I_LastUpdBy` INT(11), IN `I_Created` DATETIME, IN `I_LastUpd` DATETIME, IN `I_ZipCode` VARCHAR(50), IN `I_Barangay` VARCHAR(50))
    DETERMINISTIC
BEGIN

INSERT INTO address
(
	StreetAddress1,
	StreetAddress2,
	CityTown,
	Country,
	State,
	CreatedBy,
	LastUpdBy,
	Created,
	LastUpd,
	ZipCode,
	Barangay
)
VALUES
(
	I_StreetAddress1,
	I_StreetAddress2,
	I_CityTown,
	I_Country,
	I_State,
	I_CreatedBy,
	I_LastUpdBy,
	I_Created,
	I_LastUpd,
	I_ZipCode,
	I_Barangay
);

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
