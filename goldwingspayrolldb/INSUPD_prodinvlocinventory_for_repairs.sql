/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_prodinvlocinventory_for_repairs`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_prodinvlocinventory_for_repairs`(`pili_RowID` INT, `pili_OrganizationID` INT, `pili_ProdInvLocID` INT, `pili_CreatedBy` INT, `pili_LastUpdBy` INT, `pili_AvailableQty` INT, `pili_RackNo` VARCHAR(50), `pili_ShelfNo` VARCHAR(50), `pili_ColumnNo` VARCHAR(50)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE piliRowID INT (11);

    INSERT INTO prodinvlocinventory
    (
        RowID
        ,Created
        ,CreatedBy
        ,LastUpdBy
        ,OrganizationID
        ,ProdInvLocID
        ,AvailableQty
        ,DamagedQty
        ,RackNo
        ,ShelfNo
        ,ColumnNo
    ) VALUES (
        pili_RowID
        ,CURRENT_TIMESTAMP()
        ,pili_CreatedBy
        ,pili_CreatedBy
        ,pili_OrganizationID
        ,pili_ProdInvLocID
        ,pili_AvailableQty
        ,0
        ,pili_RackNo
        ,pili_ShelfNo
        ,pili_ColumnNo
    ) ON
    DUPLICATE
    KEY
    UPDATE
        LastUpd=CURRENT_TIMESTAMP()
        ,LastUpdBy=pili_LastUpdBy
        ,AvailableQty=pili_AvailableQty;



SELECT @@Identity AS id INTO piliRowID;

RETURN piliRowID;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
