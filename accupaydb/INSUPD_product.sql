/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_product`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_product`(`p_RowID` INT(11), `p_Name` VARCHAR(50), `p_OrganizationID` INT, `p_PartNo` VARCHAR(50), `p_CreatedBy` INT, `p_LastUpdBy` INT, `p_Category` VARCHAR(50), `p_CategoryID` INT, `p_Status` VARCHAR(50), `p_UnitPrice` DECIMAL(10,2), `p_UnitOfMeasure` VARCHAR(50), `p_IsFixed` TINYINT, `p_IsIncludedIn13th` TINYINT) RETURNS int(11)
    DETERMINISTIC
    COMMENT 'will insert a row and return its RowID if ''prod_RowID'' don''t exist in product table or else will update the table base on ''prod_RowID'''
BEGIN

DECLARE prod_RowID INT(11);

DECLARE cat_RowID INT(11);

SELECT COALESCE(RowID,NULL) FROM category WHERE OrganizationID=p_OrganizationID AND CategoryName=p_Category ORDER BY RowID LIMIT 1 INTO cat_RowID;

INSERT INTO product
(
    RowID
    ,Name
    ,OrganizationID
    ,PartNo
    ,Created
    ,CreatedBy
    ,LastUpdBy
    ,`Category`
    ,CategoryID
    ,`Status`
    ,UnitPrice
    ,UnitOfMeasure
    ,`Fixed`
    ,AllocateBelowSafetyFlag
) VALUES (
    p_RowID
    ,p_Name
    ,p_OrganizationID
    ,p_PartNo
    ,CURRENT_TIMESTAMP()
    ,p_CreatedBy
    ,p_LastUpdBy
    ,p_Category
    ,cat_RowID
    ,p_Status
    ,p_UnitPrice
    ,p_UnitOfMeasure
    ,p_IsFixed
    ,p_IsIncludedIn13th
) ON
DUPLICATE
KEY
UPDATE
    Name=p_Name
    ,PartNo=p_PartNo
    ,LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=p_LastUpdBy
    ,`Status`=p_Status
    ,`Fixed`=p_IsFixed
    ,AllocateBelowSafetyFlag=p_IsIncludedIn13th;SELECT @@Identity AS id INTO prod_RowID;

    IF IFNULL(prod_RowID,0) = 0 THEN
        SELECT RowID FROM product WHERE PartNo=p_PartNo AND OrganizationID=p_OrganizationID AND `Category`=p_Category LIMIT 1 INTO prod_RowID;
    END IF;

    IF p_RowID IS NULL AND cat_RowID = 40 THEN

        SELECT COALESCE(RowID,NULL) FROM category WHERE OrganizationID=p_OrganizationID AND CategoryName='Loan Interest' ORDER BY RowID LIMIT 1 INTO cat_RowID;

        INSERT INTO product
        (
            RowID
            ,Name
            ,OrganizationID
            ,PartNo
            ,Created
            ,CreatedBy
            ,LastUpdBy
            ,`Category`
            ,CategoryID
            ,`Status`
            ,UnitPrice
            ,UnitOfMeasure
        ) VALUES (
            p_RowID
            ,TRIM(CONCAT(p_Name,' Interest'))
            ,p_OrganizationID
            ,TRIM(CONCAT(p_PartNo,' Interest'))
            ,CURRENT_TIMESTAMP()
            ,p_CreatedBy
            ,p_LastUpdBy
            ,'Loan Interest'
            ,cat_RowID
            ,p_Status
            ,p_UnitPrice
            ,p_UnitOfMeasure
        );

    END IF;

    IF p_RowID IS NULL THEN

        INSERT INTO product
        (
            Name
            ,OrganizationID
            ,PartNo
            ,Created
            ,CreatedBy
            ,LastUpdBy
            ,`Category`
            ,CategoryID
            ,`Status`
            ,UnitPrice
            ,UnitOfMeasure
            ,`Fixed`
        ) SELECT
            p_Name
            ,og.RowID
            ,p_PartNo
            ,CURRENT_TIMESTAMP()
            ,p_CreatedBy
            ,p_LastUpdBy
            ,p_Category
            ,ct.RowID
            ,p_Status
            ,p_UnitPrice
            ,p_UnitOfMeasure
            ,p_IsFixed
            FROM organization og
            INNER JOIN category ct ON ct.OrganizationID=og.RowID
            WHERE og.RowID!=p_OrganizationID
            AND ct.CategoryName=p_Category
        ON
        DUPLICATE
        KEY
        UPDATE
            Name=p_Name
            ,LastUpd=CURRENT_TIMESTAMP()
            ,LastUpdBy=p_LastUpdBy
            ,`Fixed`=p_IsFixed;

    END IF;


RETURN prod_RowID;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
