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

-- Dumping structure for procedure U_ProdProc
DROP PROCEDURE IF EXISTS `U_ProdProc`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `U_ProdProc`(IN `I_RowID` INT(10), IN `I_Name` VARCHAR(200), IN `I_Description` VARCHAR(500), IN `I_PartNo` VARCHAR(200), IN `I_Created` DATETIME, IN `I_LastUpd` DATETIME, IN `I_CreatedBy` INT(11), IN `I_LastUpdBy` INT(11), IN `I_Category` VARCHAR(50), IN `I_Catalog` VARCHAR(50), IN `I_Comments` VARCHAR(2000), IN `I_Status` VARCHAR(50), IN `I_UnitPrice` DECIMAL(10,2), IN `I_CostPrice` DECIMAL(10,2), IN `I_UnitOfMeasure` VARCHAR(50), IN `I_SKU` VARCHAR(50), IN `I_LeadTime` INT(10), IN `I_BarCode` VARCHAR(50), IN `I_BusinessUnitID` INT(11), IN `I_LastRcvdFromShipmentDate` DATE, IN `I_LastRcvdFromShipmentCount` INT(11), IN `I_BookPageNo` VARCHAR(10), IN `I_ReOrderPoint` INT(11), IN `I_AllocateBelowSafetyFlag` CHAR(1), IN `I_Strength` VARCHAR(30), IN `I_UnitsBackordered` INT(10), IN `I_UnitsBackorderAsOf` DATETIME, IN `I_DateLastInventoryCount` DATETIME
)
    DETERMINISTIC
BEGIN
UPDATE product SET
    Name = I_Name,
    Description = I_Description,
    PartNo = I_PartNo,
    Created = I_Created,
    LastUpd = I_LastUpd,
    CreatedBy = I_CreatedBy,
    LastUpdBy = I_LastUpdBy,
    Category = I_Category,
    Catalog = I_Catalog,
    Comments = I_Comments,
    Status = I_Status,
    UnitPrice = I_UnitPrice,
    CostPrice = I_CostPrice,
    UnitOfMeasure = I_UnitOfMeasure,
    SKU = I_SKU,
    LeadTime = I_LeadTime,
    BarCode = I_BarCode,
    BusinessUnitID = I_BusinessUnitID,
    LastRcvdFromShipmentDate = I_LastRcvdFromShipmentDate,
    LastRcvdFromShipmentCount = I_LastRcvdFromShipmentCount,
    BookPageNo = I_BookPageNo,
    ReOrderPoint = I_ReOrderPoint,
    AllocateBelowSafetyFlag = I_AllocateBelowSafetyFlag,
    Strength = I_Strength,
    UnitsBackordered = I_UnitsBackordered,
    UnitsBackorderAsOf = I_UnitsBackorderAsOf,
    DateLastInventoryCount = I_DateLastInventoryCount
WHERE RowID = I_RowID;END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
