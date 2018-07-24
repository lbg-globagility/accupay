/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_payrate`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_payrate`(
    `prate_RowID` INT,
    `prate_OrganizationID` INT,
    `prate_CreatedBy` INT,
    `prate_LastUpdBy` INT,
    `prate_Date` DATE,
    `prate_PayType` VARCHAR(50),
    `prate_Description` VARCHAR(50),
    `prate_PayRate` DECIMAL(10, 4),
    `prate_OvertimeRate` DECIMAL(10, 4),
    `prate_NightDifferentialRate` DECIMAL(10, 4),
    `prate_NightDifferentialOTRate` DECIMAL(10, 4),
    `prate_RestDayRate` DECIMAL(10, 4),
    `prate_RestDayOvertimeRate` DECIMAL(10, 4)
) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE payrateID INT(11);

DECLARE yester_date DATE;

DECLARE rdayndrate_str
        ,rdayndotrate_str TEXT;

DECLARE rdayndrate
        ,rdayndotrate DECIMAL(11, 4);

DECLARE rdayndrate_default DECIMAL(11, 4) DEFAULT 1.43;
DECLARE rdayndotrate_default DECIMAL(11, 4) DEFAULT 1.859;

SELECT SUBSTRING_INDEX(SUBSTRING_INDEX(lv.DisplayValue, ',', -2), ',', 1) `RDayNDRate`
, SUBSTRING_INDEX(SUBSTRING_INDEX(lv.DisplayValue, ',', -1), ',', 1) `RDayNDOTRate`
# , lv.DisplayValue
FROM listofval lv
WHERE lv.`Type` = 'Pay rate'
AND lv.ParentLIC = prate_PayType
LIMIT 1
INTO rdayndrate_str
     ,rdayndotrate_str;

IF LENGTH(IFNULL(rdayndrate_str, '')) = 0 THEN

	SET rdayndrate = rdayndrate_default;
ELSE

	SET rdayndrate = CONCAT_WS('.'
	                           , LEFT(rdayndrate_str, 1)
	                           , RIGHT(rdayndrate_str, (LENGTH(rdayndrate_str) - 1))
	                           );
END IF;


IF LENGTH(IFNULL(rdayndotrate_str, '')) = 0 THEN

	SET rdayndotrate = rdayndotrate_default;
ELSE

	SET rdayndotrate = CONCAT_WS('.'
	                             , LEFT(rdayndotrate_str, 1)
	                             , RIGHT(rdayndotrate_str, (LENGTH(rdayndotrate_str) - 1))
										  );
END IF;

IF prate_PayType IN ('Regular Holiday', 'Special Non-Working Holiday') THEN
    SET yester_date = SUBDATE(prate_Date, INTERVAL 1 DAY);

ELSE
    SET yester_date = NULL;

END IF;



INSERT INTO payrate
(
    RowID
    ,OrganizationID
    ,Created
    ,CreatedBy
    ,LastUpdBy
    ,`Date`
    ,PayType
    ,Description
    ,`PayRate`
    ,OvertimeRate
    ,NightDifferentialRate
    ,NightDifferentialOTRate
    ,RestDayRate
    ,DayBefore
    ,RestDayOvertimeRate
	 , RestDayNDRate
    , RestDayNDOTRate
) VALUES (
    prate_RowID
    ,prate_OrganizationID
    ,CURRENT_TIMESTAMP()
    ,prate_CreatedBy
    ,prate_LastUpdBy
    ,prate_Date
    ,prate_PayType
    ,prate_Description
    ,prate_PayRate
    ,prate_OvertimeRate
    ,prate_NightDifferentialRate
    ,prate_NightDifferentialOTRate
    ,prate_RestDayRate
    ,yester_date
    ,prate_RestDayOvertimeRate
    , rdayndrate
    , rdayndotrate
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=prate_LastUpdBy
    ,PayType=prate_PayType
    ,Description=prate_Description
    ,`PayRate`=prate_PayRate
    ,OvertimeRate=prate_OvertimeRate
    ,NightDifferentialRate=prate_NightDifferentialRate
    ,NightDifferentialOTRate=prate_NightDifferentialOTRate
    ,RestDayRate=prate_RestDayRate
    ,DayBefore=yester_date
    ,RestDayOvertimeRate=prate_RestDayOvertimeRate
	 , RestDayNDRate = rdayndrate
    , RestDayNDOTRate = rdayndotrate
	 ;SELECT @@Identity AS id INTO payrateID;
	 
RETURN payrateID;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
