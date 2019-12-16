/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_employeebonus`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_employeebonus`(`bon_RowID` INT, `bon_OrganizationID` INT, `bon_CreatedBy` INT, `bon_LastUpdBy` INT, `bon_EmployeeID` INT, `bon_AllowanceFrequency` VARCHAR(50), `bon_EffectiveStartDate` DATE, `bon_EffectiveEndDate` DATE, `bon_ProductID` INT, `bon_BonusAmount` DECIMAL(11,6)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE bon_ID INT(11);

INSERT INTO employeebonus
(
    RowID
    ,OrganizationID
    ,Created
    ,CreatedBy
    ,EmployeeID
    ,ProductID
    ,EffectiveStartDate
    ,AllowanceFrequency
    ,EffectiveEndDate
    ,TaxableFlag
    ,BonusAmount
) VALUES (
    bon_RowID
    ,bon_OrganizationID
    ,CURRENT_TIMESTAMP()
    ,bon_CreatedBy
    ,bon_EmployeeID
    ,bon_ProductID
    ,bon_EffectiveStartDate
    ,bon_AllowanceFrequency
    ,bon_EffectiveEndDate
    ,(SELECT `Status` FROM product WHERE RowID=bon_ProductID)
    ,bon_BonusAmount
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=bon_LastUpdBy
    ,ProductID=bon_ProductID
    ,EffectiveStartDate=bon_EffectiveStartDate
    ,AllowanceFrequency=bon_AllowanceFrequency
    ,EffectiveEndDate=bon_EffectiveEndDate
    ,TaxableFlag=(SELECT `Status` FROM product WHERE RowID=bon_ProductID)
    ,BonusAmount=bon_BonusAmount;SELECT @@Identity AS id INTO bon_ID;

RETURN bon_ID;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
