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

-- Dumping structure for procedure I_ProdInvLocation
DROP PROCEDURE IF EXISTS `I_ProdInvLocation`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_ProdInvLocation`(IN `I_Created` DATETIME, IN `I_LastUpd` DATETIME, IN `I_CreatedBy` INT(11), IN `I_OrganizationID` INT(11), IN `I_LastUpdBy` INT(11), IN `I_ProductID` INT(10), IN `I_RunningTotalQty` INT(10), IN `I_InventoryLocationID` INT(10), IN `I_TotalAvailbleItemQty` INT(10), IN `I_TotalReservedItemQty` INT(10)

    )
    DETERMINISTIC
BEGIN
INSERT INTO productinventorylocation
(
    Created,
    LastUpd,
    CreatedBy,
    OrganizationID,
    LastUpdBy,
    ProductID,
    RunningTotalQty,
    InventoryLocationID,
    TotalAvailbleItemQty,
    TotalReservedItemQty
)
VALUES
(
    I_Created,
    I_LastUpd,
    I_CreatedBy,
    I_OrganizationID,
    I_LastUpdBy,
    I_ProductID,
    I_RunningTotalQty,
    I_InventoryLocationID,
    I_TotalAvailbleItemQty,
    I_TotalReservedItemQty
);END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
