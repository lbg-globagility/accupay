/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_productForOtherOrganiz`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_productForOtherOrganiz`(`p_SupplierID` INT, `p_Name` VARCHAR(200), `p_OrganizationID` INT, `p_Description` VARCHAR(2000), `p_PartNo` VARCHAR(200), `p_LastArrivedQty` INT, `p_CreatedBy` INT, `p_Category` VARCHAR(50), `p_AccountingAccountID` INT, `p_Catalog` VARCHAR(50), `p_Comments` VARCHAR(2000), `p_Status` VARCHAR(50), `p_UnitPrice` DECIMAL(10,2), `p_VATPercent` DECIMAL(10,2), `p_FirstBillFlag` CHAR(50), `p_SecondBillFlag` CHAR(50), `p_ThirdBillFlag` CHAR(50), `p_PDCFlag` CHAR(50), `p_MonthlyBIllFlag` CHAR(50), `p_PenaltyFlag` CHAR(50), `p_WithholdingTaxPercent` DECIMAL(10,2), `p_CostPrice` DECIMAL(10,2), `p_UnitOfMeasure` VARCHAR(50), `p_SKU` VARCHAR(50), `p_LeadTime` INT, `p_BarCode` VARCHAR(50), `p_BusinessUnitID` INT, `p_LastRcvdFromShipmentDate` DATE, `p_LastRcvdFromShipmentCount` INT, `p_TotalShipmentCount` INT, `p_BookPageNo` VARCHAR(10), `p_BrandName` VARCHAR(50), `p_LastPurchaseDate` DATE, `p_LastSoldDate` DATE, `p_LastSoldCount` INT, `p_ReOrderPoint` INT, `p_AllocateBelowSafetyFlag` CHAR(50), `p_Strength` VARCHAR(30), `p_UnitsBackordered` INT, `p_UnitsBackorderAsOf` DATETIME, `p_DateLastInventoryCount` DATETIME, `p_TaxVAT` DECIMAL(10,2), `p_WithholdingTax` DECIMAL(10,2), `p_COAId` INT) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11) DEFAULT 0;

INSERT INTO product
(
    SupplierID
    ,Name
    ,OrganizationID
    ,Description
    ,PartNo
    ,Created
    ,LastUpd
    ,LastArrivedQty
    ,CreatedBy
    ,LastUpdBy
    ,Category
    ,CategoryID
    ,AccountingAccountID
    ,Catalog
    ,Comments
    ,Status
    ,UnitPrice
    ,VATPercent
    ,FirstBillFlag
    ,SecondBillFlag
    ,ThirdBillFlag
    ,PDCFlag
    ,MonthlyBIllFlag
    ,PenaltyFlag
    ,WithholdingTaxPercent
    ,CostPrice
    ,UnitOfMeasure
    ,SKU
    ,Image
    ,LeadTime
    ,BarCode
    ,BusinessUnitID
    ,LastRcvdFromShipmentDate
    ,LastRcvdFromShipmentCount
    ,TotalShipmentCount
    ,BookPageNo
    ,BrandName
    ,LastPurchaseDate
    ,LastSoldDate
    ,LastSoldCount
    ,ReOrderPoint
    ,AllocateBelowSafetyFlag
    ,Strength
    ,UnitsBackordered
    ,UnitsBackorderAsOf
    ,DateLastInventoryCount
    ,TaxVAT
    ,WithholdingTax
    ,COAId
) SELECT
    p_SupplierID
    ,p_Name
    ,og.RowID
    ,p_Description
    ,p_PartNo
    ,CURRENT_TIMESTAMP()
    ,CURRENT_TIMESTAMP()
    ,p_LastArrivedQty
    ,p_CreatedBy
    ,p_CreatedBy
    ,p_Category
    ,ct.RowID
    ,p_AccountingAccountID
    ,p_Catalog
    ,p_Comments
    ,p_Status
    ,p_UnitPrice
    ,p_VATPercent
    ,p_FirstBillFlag
    ,p_SecondBillFlag
    ,p_ThirdBillFlag
    ,p_PDCFlag
    ,p_MonthlyBIllFlag
    ,p_PenaltyFlag
    ,p_WithholdingTaxPercent
    ,p_CostPrice
    ,p_UnitOfMeasure
    ,p_SKU
    ,NULL
    ,p_LeadTime
    ,p_BarCode
    ,p_BusinessUnitID
    ,p_LastRcvdFromShipmentDate
    ,p_LastRcvdFromShipmentCount
    ,p_TotalShipmentCount
    ,p_BookPageNo
    ,p_BrandName
    ,p_LastPurchaseDate
    ,p_LastSoldDate
    ,p_LastSoldCount
    ,p_ReOrderPoint
    ,p_AllocateBelowSafetyFlag
    ,p_Strength
    ,p_UnitsBackordered
    ,p_UnitsBackorderAsOf
    ,p_DateLastInventoryCount
    ,p_TaxVAT
    ,p_WithholdingTax
    ,p_COAId
FROM organization og
INNER JOIN category ct ON ct.OrganizationID=og.RowID
WHERE ct.CategoryName=p_Category
AND og.RowID!=p_OrganizationID
ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP();SELECT @@Identity AS ID INTO returnvalue;



RETURN returnvalue;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
