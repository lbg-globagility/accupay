/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_shift`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_shift`(`sh_RowID` INT, `sh_OrganizationID` INT, `sh_CreatedBy` INT, `sh_LastUpdBy` INT, `sh_TimeFrom` TIME, `sh_TimeTo` TIME) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE shiftRowID INT(11);



INSERT INTO shift
(
    RowID
    ,OrganizationID
    ,Created
    ,CreatedBy
    ,TimeFrom
    ,TimeTo
) VALUES (
    sh_RowID
    ,sh_OrganizationID
    ,CURRENT_TIMESTAMP()
    ,sh_CreatedBy
    ,sh_TimeFrom
    ,sh_TimeTo
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=sh_LastUpdBy
    ,TimeFrom=sh_TimeFrom
    ,TimeTo=sh_TimeTo;SELECT @@Identity AS id INTO shiftRowID;

RETURN shiftRowID;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
