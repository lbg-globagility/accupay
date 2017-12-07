/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `I_Products`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_Products`(IN `I_Name` VARCHAR(200), IN `I_OrganizationID` INT(11), IN `I_Description` VARCHAR(500), IN `I_PartNo` VARCHAR(200), IN `I_Created` DATETIME, IN `I_LastUpd` DATETIME, IN `I_CreatedBy` INT(11), IN `I_LastUpdBy` INT(11), IN `I_Category` VARCHAR(50), IN `I_Catalog` VARCHAR(50), IN `I_Comments` VARCHAR(2000), IN `I_Status` VARCHAR(50), IN `I_UnitPrice` DECIMAL(10,2), IN `I_CostPrice` DECIMAL(10,2), IN `I_UnitOfMeasure` VARCHAR(50), IN `I_SKU` VARCHAR(50), IN `I_Image` TEXT, IN `I_LeadTime` INT(10), IN `I_BarCode` VARCHAR(50), IN `I_BusinessUnitID` INT(11), IN `I_LastRcvdFromShipmentDate` DATE, IN `I_BookPageNo` VARCHAR(10), IN `I_ReOrderPoint` INT(10), IN `I_AllocateBelowSafetyFlag` CHAR(1), IN `I_Strength` VARCHAR(30), IN `I_UnitsBackordered` INT(10), IN `I_UnitsBackorderAsOf` DATETIME, IN `I_DateLastInventoryCount` DATETIME
, IN `I_SupplierID` INT(10))
    DETERMINISTIC
BEGIN
INSERT INTO product
    (
    Name,
    OrganizationID,
    Description,
    PartNo,
    Created,
    LastUpd,
    CreatedBy,
    LastUpdBy,
    Category,
    Catalog,
    Comments,
    Status,
    UnitPrice,
    CostPrice,
    UnitOfMeasure,
    SKU,
    Image,
    LeadTime,
    BarCode,
    BusinessUnitID,
    LastRcvdFromShipmentDate,
    BookPageNo,
    ReOrderPoint,
    AllocateBelowSafetyFlag,
    Strength,
    UnitsBackordered,
    UnitsBackorderAsOf,
    DateLastInventoryCount,
    SupplierID
    )
    VALUES
    (
    I_Name,
    I_OrganizationID,
    I_Description,
    I_PartNo,
    I_Created,
    I_LastUpd,
    I_CreatedBy,
    I_LastUpdBy,
    I_Category,
    I_Catalog,
    I_Comments,
    I_Status,
    I_UnitPrice,
    I_CostPrice,
    I_UnitOfMeasure,
    I_SKU,
    I_Image,
    I_LeadTime,
    I_BarCode,
    I_BusinessUnitID,
    I_LastRcvdFromShipmentDate,
    I_BookPageNo,
    I_ReOrderPoint,
    I_AllocateBelowSafetyFlag,
    I_Strength,
    I_UnitsBackordered,
    I_UnitsBackorderAsOf,
    I_DateLastInventoryCount,
    I_SupplierID
    );END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
