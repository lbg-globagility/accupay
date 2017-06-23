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

-- Dumping structure for procedure goldwingspayrolldb.U_Listofvalue
DROP PROCEDURE IF EXISTS `U_Listofvalue`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `U_Listofvalue`(IN `U_RowID` INT, IN `U_DisplayValue` VARCHAR(50), IN `U_LIC` VARCHAR(50), IN `U_Type` VARCHAR(50), IN `U_ParentLIC` VARCHAR(50), IN `U_Active` VARCHAR(50), IN `U_Description` VARCHAR(500), IN `U_Created` DATETIME, IN `U_CreatedBy` VARCHAR(50), IN `U_LastUpd` DATETIME, IN `U_OrderBy` INT, IN `U_LastUpdBy` VARCHAR(50)
)
    DETERMINISTIC
BEGIN
UPDATE listofval 
	Set
RowID = U_RowID,
 DisplayValue = U_DisplayValue,
 LIC = U_LIC,
 Type = U_Type,
 ParentLIC = U_ParentLIC,
 Active = U_Active,
 Description = U_Description,
 Created = U_Created,
 CreatedBy = U_CreatedBy,
 LastUpd = U_LastUpd,
 OrderBy = U_OrderBy,
 LastUpdBy = U_LastUpdBy
 
 WHERE RowID = U_RowID;END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
