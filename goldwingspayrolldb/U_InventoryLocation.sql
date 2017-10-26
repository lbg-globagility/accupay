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

-- Dumping structure for procedure U_InventoryLocation
DROP PROCEDURE IF EXISTS `U_InventoryLocation`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `U_InventoryLocation`(IN `I_RowID` INT(10), IN `I_Name` VARCHAR(50), IN `I_Status` VARCHAR(50), IN `I_MainPhone` VARCHAR(50), IN `I_MobilePhone` VARCHAR(50), IN `I_FaxNumber` VARCHAR(50), IN `I_CreatedBy` INT(11), IN `I_LastUpdBy` INT(11), IN `I_Type` VARCHAR(50), IN `I_Created` DATETIME, IN `I_LastUpd` DATETIME, IN `I_MainBranch` CHAR(1)
)
BEGIN
UPDATE InventoryLocation SET

    Name = I_Name,
    Status = I_Status,
    MainPhone = I_MainPhone,
    MobilePhone = I_MobilePhone,
    FaxNumber = I_FaxNumber,
    CreatedBy = I_CreatedBy,
    LastUpdBy = I_LastUpdBy,
    Type = I_Type,
    Created = I_Created,
    LastUpd = I_LastUpd,
    MainBranch = I_MainBranch
WHERE   RowID = I_RowID;END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
