/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `U_ProductInvLocation`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `U_ProductInvLocation`(IN `I_RowID` INT(10), IN `I_Created` DATETIME, IN `I_LastUpd` DATETIME, IN `I_CreatedBy` INT(11), IN `I_LastUpdBy` INT(11), IN `I_RunningTotalQty` INT(10), IN `I_TotalAvailbleItemQty` INT(10), IN `I_TotalReservedItemQty` INT(10)
    )
    DETERMINISTIC
BEGIN
UPDATE productinventorylocation SET
    Created = I_Created,
    LastUpd = I_LastUpd,
    CreatedBy = I_CreatedBy,
    LastUpdBy = I_LastUpdBy,
    RunningTotalQty = I_RunningTotalQty,
    TotalAvailbleItemQty = I_TotalAvailbleItemQty,
    TotalReservedItemQty = I_TotalReservedItemQty
WHERE RowID = I_RowID;END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
