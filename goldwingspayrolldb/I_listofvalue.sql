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

-- Dumping structure for procedure goldwingspayrolldb.I_listofvalue
DROP PROCEDURE IF EXISTS `I_listofvalue`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_listofvalue`(IN `I_Created` DATETIME, IN `I_CreatedBy` INT(10), IN `I_LastUpd` DATETIME, IN `I_LastUpdBy` INT(10), IN `I_DisplayValue` VARCHAR(100), IN `I_LIC` VARCHAR(100), IN `I_Type` VARCHAR(50), IN `I_ParentLIC` VARCHAR(100), IN `I_Status` VARCHAR(50), IN `I_Description` VARCHAR(2000), IN `I_SystemAccountFlg` CHAR(10), IN `I_DisplayAccountFlg` CHAR(10), IN `I_OrderBy` INT(10))
    DETERMINISTIC
BEGIN
INSERT INTO listofval
(
	Created,
	CreatedBy,
	LastUpd,
	LastUpdBy,
	DisplayValue,
	LIC,
	Type,
	ParentLIC,
	Active,
	Description,
	OrderBy
)
VALUES
(
	I_Created,
	I_CreatedBy,
	I_LastUpd,
	I_LastUpdBy,
 	I_DisplayValue,
 	I_LIC,
 	I_Type,
 	I_ParentLIC,
	I_Status,
	I_Description,
	I_OrderBy
);



END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
