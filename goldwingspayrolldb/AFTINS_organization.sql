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

-- Dumping structure for trigger goldwingspayrolldb_global_e_pc.AFTINS_organization
DROP TRIGGER IF EXISTS `AFTINS_organization`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_organization` AFTER INSERT ON `organization` FOR EACH ROW BEGIN



DECLARE indx INT(11) DEFAULT 0;

DECLARE view_count INT(11);

DECLARE view_name VARCHAR(50);

DECLARE view_ID INT(11);

DECLARE userPositionID INT(11);

DECLARE INS_audit_ID INT(11);

DECLARE view_RowID INT(11);

DECLARE orgIDOfCreator INT(11);


SELECT p.RowID FROM user u LEFT JOIN position p ON p.RowID=u.PositionID WHERE u.RowID=NEW.CreatedBy LIMIT 1 INTO userPositionID;



INSERT INTO `view`
(
    ViewName
    ,OrganizationID
) SELECT v.ViewName
    ,NEW.RowID
    FROM `view` v
    GROUP BY v.ViewName
    ORDER BY v.ViewName;

INSERT INTO payperiod
(
    OrganizationID
    ,Created
    ,CreatedBy
    ,PayFromDate
    ,PayToDate
    ,TotalGrossSalary
    ,`Year`
    ,`Month`
    ,`Half`
    ,OrdinalValue
) SELECT
    NEW.RowID
    ,CURRENT_TIMESTAMP()
    ,NEW.CreatedBy
    ,PayFromDate
    ,PayToDate
    ,TotalGrossSalary
    ,`Year`
    ,`Month`
    ,`Half`
    ,OrdinalValue
FROM payperiod
GROUP BY PayFromDate,PayToDate
ORDER BY PayFromDate,PayToDate
ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP();

INSERT INTO payrate
(
    OrganizationID
    ,Created
    ,CreatedBy
    ,DayBefore
    ,`Date`
    ,PayType
    ,Description
    ,`PayRate`
    ,OvertimeRate
    ,NightDifferentialRate
    ,NightDifferentialOTRate
    ,RestDayRate
    ,RestDayOvertimeRate
) SELECT
    NEW.RowID
    ,CURRENT_TIMESTAMP()
    ,NEW.CreatedBy
    ,DayBefore
    ,`Date`
    ,PayType
    ,Description
    ,`PayRate`
    ,OvertimeRate
    ,NightDifferentialRate
    ,NightDifferentialOTRate
    ,RestDayRate
    ,RestDayOvertimeRate
FROM payrate
GROUP BY `Date`
ORDER BY `Date`
ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP();

    SELECT OrganizationID FROM user WHERE RowID=NEW.CreatedBy LIMIT 1 INTO orgIDOfCreator;


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
    ,COAId
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
    ,p.COAId
    ,p.ActiveData
FROM product p
LEFT JOIN category c ON c.CategoryName=p.Category
GROUP BY p.PartNo
ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP();


INSERT INTO shift
(
    OrganizationID
    ,CreatedBy
    ,Created
    ,TimeFrom
    ,TimeTo
) VALUES (
    NEW.RowID
    ,NEW.CreatedBy
    ,CURRENT_TIMESTAMP()
    ,NEW.NightShiftTimeFrom
    ,NEW.NightShiftTimeTo
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP();

	
	
	INSERT INTO `division` (Name,TradeName,OrganizationID,MainPhone,FaxNumber,BusinessAddress,ContactName,EmailAddress,AltEmailAddress,AltPhone,URL,TINNo,Created,CreatedBy,DivisionType,GracePeriod,WorkDaysPerYear,PhHealthDeductSched,HDMFDeductSched,SSSDeductSched,WTaxDeductSched,DefaultVacationLeave,DefaultSickLeave,DefaultMaternityLeave,DefaultPaternityLeave,DefaultOtherLeave,PayFrequencyID,PhHealthDeductSchedAgency,HDMFDeductSchedAgency,SSSDeductSchedAgency,WTaxDeductSchedAgency,DivisionUniqueID) VALUES ( 'Any location', '', NEW.RowID, '', '', '', '', '', '', '', '', '', CURRENT_TIMESTAMP(), NEW.CreatedBy, 'Department', 15.00, 313, 'Per pay period', 'Per pay period', 'Per pay period', 'Per pay period', 40.00, 40.00, 40.00, 40.00, 40.00, 1, 'Per pay period', 'Per pay period', 'Per pay period', 'Per pay period',2);SELECT @@Identity AS ID INTO INS_audit_ID;
	
	INSERT INTO `division` (Name,TradeName,OrganizationID,MainPhone,FaxNumber,BusinessAddress,ContactName,EmailAddress,AltEmailAddress,AltPhone,URL,TINNo,Created,CreatedBy,DivisionType,GracePeriod,WorkDaysPerYear,PhHealthDeductSched,HDMFDeductSched,SSSDeductSched,WTaxDeductSched,DefaultVacationLeave,DefaultSickLeave,DefaultMaternityLeave,DefaultPaternityLeave,DefaultOtherLeave,PayFrequencyID,PhHealthDeductSchedAgency,HDMFDeductSchedAgency,SSSDeductSchedAgency,WTaxDeductSchedAgency,DivisionUniqueID,ParentDivisionID) SELECT 'Default division', '', NEW.RowID, '', '', '', '', '', '', '', '', '', CURRENT_TIMESTAMP(), NEW.CreatedBy, 'Department', 15.00, 313, 'Per pay period', 'Per pay period', 'Per pay period', 'Per pay period', 40.00, 40.00, 40.00, 40.00, 40.00, 1, 'Per pay period', 'Per pay period', 'Per pay period', 'Per pay period',2,INS_audit_ID;
	
	
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

INSERT INTO position_view
(
    PositionID
    ,ViewID
    ,Creates
    ,OrganizationID
    ,ReadOnly
    ,Updates
    ,Deleting
    ,Created
    ,CreatedBy
    ,LastUpdBy
) SELECT
    pos.RowID
    ,v.RowID
    ,'N'
    ,NEW.RowID
    ,'Y'
    ,'N'
    ,'N'
    ,CURRENT_TIMESTAMP()
    ,NEW.CreatedBy
    ,NEW.CreatedBy
    FROM `view` v
    LEFT JOIN (SELECT * FROM position GROUP BY PositionName) pos ON pos.RowID > 0 AND pos.RowID != IFNULL(userPositionID,0)
    WHERE v.OrganizationID=NEW.RowID
UNION
    SELECT
    pos.RowID
    ,v.RowID
    ,'Y'
    ,NEW.RowID
    ,'N'
    ,'Y'
    ,'Y'
    ,CURRENT_TIMESTAMP()
    ,NEW.CreatedBy
    ,NEW.CreatedBy
    FROM `view` v
    INNER JOIN position pos ON pos.RowID = IFNULL(userPositionID,0)
    WHERE v.OrganizationID=NEW.RowID
ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP();

    SELECT RowID FROM `view` WHERE ViewName='Organization' AND OrganizationID=orgIDOfCreator INTO view_RowID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'Name',NEW.RowID,'',NEW.Name,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'TradeName',NEW.RowID,'',NEW.TradeName,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'PrimaryAddressID',NEW.RowID,'',NEW.PrimaryAddressID,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'PrimaryContactID',NEW.RowID,'',NEW.PrimaryContactID,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'PremiseAddressID',NEW.RowID,'',NEW.PremiseAddressID,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'MainPhone',NEW.RowID,'',NEW.MainPhone,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'FaxNumber',NEW.RowID,'',NEW.FaxNumber,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'EmailAddress',NEW.RowID,'',NEW.EmailAddress,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'AltEmailAddress',NEW.RowID,'',NEW.AltEmailAddress,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'AltPhone',NEW.RowID,'',NEW.AltPhone,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'URL',NEW.RowID,'',NEW.URL,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'TINNo',NEW.RowID,'',NEW.TINNo,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'BankAccountNo',NEW.RowID,'',NEW.BankAccountNo,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'BankName',NEW.RowID,'',NEW.BankName,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'OrganizationType',NEW.RowID,'',NEW.OrganizationType,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'TotalFloorArea',NEW.RowID,'',NEW.TotalFloorArea,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'MinimumWater',NEW.RowID,'',NEW.MinimumWater,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'VacationLeaveDays',NEW.RowID,'',NEW.VacationLeaveDays,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'SickLeaveDays',NEW.RowID,'',NEW.SickLeaveDays,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'MaternityLeaveDays',NEW.RowID,'',NEW.MaternityLeaveDays,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'OthersLeaveDays',NEW.RowID,'',NEW.OthersLeaveDays,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'STPFlag',NEW.RowID,'',NEW.STPFlag,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'PayFrequencyID',NEW.RowID,'',NEW.PayFrequencyID,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'PhilhealthDeductionSchedule',NEW.RowID,'',NEW.PhilhealthDeductionSchedule,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'SSSDeductionSchedule',NEW.RowID,'',NEW.SSSDeductionSchedule,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'PagIbigDeductionSchedule',NEW.RowID,'',NEW.PagIbigDeductionSchedule,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'ReportText',NEW.RowID,'',NEW.ReportText,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'NightDifferentialTimeFrom',NEW.RowID,'',NEW.NightDifferentialTimeFrom,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'NightDifferentialTimeTo',NEW.RowID,'',NEW.NightDifferentialTimeTo,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'NightShiftTimeFrom',NEW.RowID,'',NEW.NightShiftTimeFrom,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'NightShiftTimeTo',NEW.RowID,'',NEW.NightShiftTimeTo,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'AllowNegativeLeaves',NEW.RowID,'',NEW.AllowNegativeLeaves,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'LimitedAccess',NEW.RowID,'',NEW.LimitedAccess,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'WorkDaysPerYear',NEW.RowID,'',NEW.WorkDaysPerYear,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'RDOCode',NEW.RowID,'',NEW.RDOCode,'Insert') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,orgIDOfCreator,view_RowID,'ZIPCode',NEW.RowID,'',NEW.ZIPCode,'Insert') INTO INS_audit_ID;

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
