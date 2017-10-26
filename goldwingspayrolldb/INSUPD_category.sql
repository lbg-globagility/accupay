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

-- Dumping structure for function INSUPD_category
DROP FUNCTION IF EXISTS `INSUPD_category`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_category`(`cat_RowID` INT, `cat_CategoryName` VARCHAR(50), `cat_OrganizationID` INT, `cat_CatalogID` INT) RETURNS int(11)
    DETERMINISTIC
    COMMENT 'will insert a row and return its RowID if ''cat_RowID'' don''t exist in category table or else will update the table base on ''cat_RowID'''
BEGIN

DECLARE cat_ID INT(11);

DECLARE newCatID INT(11);

SELECT COALESCE(MAX(CategoryID),0) + 1 FROM category WHERE OrganizationID=cat_OrganizationID AND CategoryName=cat_CategoryName INTO newCatID;

INSERT INTO category
(
    RowID
    ,CategoryID
    ,CategoryName
    ,OrganizationID
    ,CatalogID
    ,LastUpd
) SELECT
    cat_RowID
    ,newCatID
    ,cat_CategoryName
    ,og.RowID
    ,NULL
    ,CURRENT_TIMESTAMP()
    FROM organization og WHERE og.RowID > 0
ON
DUPLICATE
KEY
UPDATE LastUpd=CURRENT_TIMESTAMP();SELECT @@Identity AS id INTO cat_ID;





RETURN cat_ID;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
