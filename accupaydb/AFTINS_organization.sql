/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTINS_organization`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_organization` AFTER INSERT ON `organization` FOR EACH ROW BEGIN

DECLARE INS_ParentDivision_ID INT(11);

INSERT INTO category
(
    CategoryID
    ,CategoryName
    ,OrganizationID
    ,`Catalog`
    ,CatalogID
)   SELECT
    CategoryID
    ,CategoryName
    ,NEW.RowID
    ,`Catalog`
    ,CatalogID
    FROM category
    GROUP BY CategoryName
ON
DUPLICATE
KEY
UPDATE
    OrganizationID=NEW.RowID;


INSERT INTO product
(
    Name
    ,OrganizationID
    ,Description
    ,PartNo
    ,Created
    ,LastUpd
    ,LastArrivedQty
    ,CreatedBy
    ,LastUpdBy
    ,`Category`
    ,CategoryID
    ,AccountingAccountID
    ,`Catalog`
    ,Comments
    ,`Status`
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
    ,ActiveData
)  SELECT
    p.Name
    ,NEW.RowID
    ,p.Description
    ,p.PartNo
    ,CURRENT_TIMESTAMP()
    ,CURRENT_TIMESTAMP()
    ,p.LastArrivedQty
    ,NEW.CreatedBy
    ,NEW.LastUpdBy
    ,p.`Category`
    ,(SELECT RowID FROM category WHERE OrganizationID=NEW.RowID AND CategoryName=p.Category)
    ,p.AccountingAccountID
    ,p.`Catalog`
    ,p.Comments
    ,p.`Status`
    ,p.UnitPrice
    ,p.VATPercent
    ,p.FirstBillFlag
    ,p.SecondBillFlag
    ,p.ThirdBillFlag
    ,p.PDCFlag
    ,p.MonthlyBIllFlag
    ,p.PenaltyFlag
    ,p.WithholdingTaxPercent
    ,p.CostPrice
    ,p.UnitOfMeasure
    ,p.SKU
    ,p.Image
    ,p.LeadTime
    ,p.BarCode
    ,p.BusinessUnitID
    ,p.LastRcvdFromShipmentDate
    ,p.LastRcvdFromShipmentCount
    ,p.TotalShipmentCount
    ,p.BookPageNo
    ,p.BrandName
    ,p.LastPurchaseDate
    ,p.LastSoldDate
    ,p.LastSoldCount
    ,p.ReOrderPoint
    ,p.AllocateBelowSafetyFlag
    ,p.Strength
    ,p.UnitsBackordered
    ,p.UnitsBackorderAsOf
    ,p.DateLastInventoryCount
    ,p.TaxVAT
    ,p.WithholdingTax
    ,p.ActiveData
FROM product p
LEFT JOIN category c ON c.CategoryName=p.Category
GROUP BY p.PartNo
ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP();

INSERT INTO `division` (Name,TradeName,OrganizationID,MainPhone,FaxNumber,BusinessAddress,ContactName,EmailAddress,AltEmailAddress,AltPhone,URL,TINNo,Created,CreatedBy,DivisionType,GracePeriod,WorkDaysPerYear,PhHealthDeductSched,HDMFDeductSched,SSSDeductSched,WTaxDeductSched,DefaultVacationLeave,DefaultSickLeave,DefaultMaternityLeave,DefaultPaternityLeave,DefaultOtherLeave,PayFrequencyID,PhHealthDeductSchedAgency,HDMFDeductSchedAgency,SSSDeductSchedAgency,WTaxDeductSchedAgency,DivisionUniqueID) VALUES ( 'Default Location', '', NEW.RowID, '', '', '', '', '', '', '', '', '', CURRENT_TIMESTAMP(), NEW.CreatedBy, 'Department', 15.00, 313, 'Per pay period', 'Per pay period', 'Per pay period', 'Per pay period', 40.00, 40.00, 40.00, 40.00, 40.00, 1, 'Per pay period', 'Per pay period', 'Per pay period', 'Per pay period',2);
SELECT @@Identity AS ID INTO INS_ParentDivision_ID;

INSERT INTO `division` (Name,TradeName,OrganizationID,MainPhone,FaxNumber,BusinessAddress,ContactName,EmailAddress,AltEmailAddress,AltPhone,URL,TINNo,Created,CreatedBy,DivisionType,GracePeriod,WorkDaysPerYear,PhHealthDeductSched,HDMFDeductSched,SSSDeductSched,WTaxDeductSched,DefaultVacationLeave,DefaultSickLeave,DefaultMaternityLeave,DefaultPaternityLeave,DefaultOtherLeave,PayFrequencyID,PhHealthDeductSchedAgency,HDMFDeductSchedAgency,SSSDeductSchedAgency,WTaxDeductSchedAgency,DivisionUniqueID,ParentDivisionID) SELECT 'Default Division', '', NEW.RowID, '', '', '', '', '', '', '', '', '', CURRENT_TIMESTAMP(), NEW.CreatedBy, 'Department', 15.00, 313, 'Per pay period', 'Per pay period', 'Per pay period', 'Per pay period', 40.00, 40.00, 40.00, 40.00, 40.00, 1, 'Per pay period', 'Per pay period', 'Per pay period', 'Per pay period',2,INS_ParentDivision_ID;
	
	
INSERT INTO `position`
(
	PositionName
	,Created
	,CreatedBy
	,OrganizationID
	,DivisionId
) SELECT
	pos.PositionName
	,CURRENT_TIMESTAMP()
	,NEW.CreatedBy
	,NEW.RowID
	,dd.RowID
	FROM `position` pos
	# LEFT JOIN `division` d ON d.RowID=pos.DivisionId AND d.OrganizationID=pos.OrganizationID
	# LEFT JOIN `division` dd ON dd.Name=d.Name AND dd.OrganizationID=NEW.RowID
	LEFT JOIN `division` dd ON dd.Name='Default division' AND dd.OrganizationID=NEW.RowID
	GROUP BY pos.PositionName
ON
DUPLICATE
KEY
UPDATE
	LastUpd=CURRENT_TIMESTAMP()
	,LastUpdBy=NEW.CreatedBy;


UPDATE product p LEFT JOIN product pp ON pp.PartNo=SUBSTRING_INDEX(p.PartNo,' ',1) AND pp.OrganizationID=NEW.RowID SET
p.LastSoldCount=pp.RowID
,p.LastUpd=CURRENT_TIMESTAMP()
,p.LastUpdBy=NEW.CreatedBy
WHERE p.OrganizationID=NEW.RowID
AND p.Category='Loan Interest';

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
