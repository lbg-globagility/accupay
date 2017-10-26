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

-- Dumping structure for procedure I_InventoryLocation
DROP PROCEDURE IF EXISTS `I_InventoryLocation`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_InventoryLocation`(IN `I_Name` VARCHAR(50), IN `I_OrganizationID` INT(11), IN `I_Status` VARCHAR(50), IN `I_MainPhone` VARCHAR(50), IN `I_MobilePhone` VARCHAR(50), IN `I_FaxNumber` VARCHAR(50), IN `I_CreatedBy` INT(11), IN `I_LastUpdBy` INT(11), IN `I_Type` VARCHAR(50), IN `I_Created` DATETIME, IN `I_LastUpd` DATETIME, IN `I_MainBranch` CHAR(1)
)
    DETERMINISTIC
BEGIN
INSERT INTO InventoryLocation
(
    Name,
    OrganizationID,
    Status,
    MainPhone,
    MobilePhone,
    FaxNumber,
    CreatedBy,
    LastUpdBy,
    Type,
    Created,
    LastUpd,
    MainBranch
)
VALUES
(
    I_Name,
    I_OrganizationID,
    I_Status,
    I_MainPhone,
    I_MobilePhone,
    I_FaxNumber,
    I_CreatedBy,
    I_LastUpdBy,
    I_Type,
    I_Created,
    I_LastUpd,
    I_MainBranch
);END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
