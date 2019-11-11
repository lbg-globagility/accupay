/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `U_PositionView`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `U_PositionView`(IN `I_RowID` INT(11), IN `I_Creates` CHAR(1), IN `I_ReadOnly` CHAR(1), IN `I_Updates` CHAR(1), IN `I_Deleting` CHAR(1), IN `I_Created` DATETIME, IN `I_CreatedBy` INT(11), IN `I_LastUpd` DATETIME, IN `I_LastUpdBy` INT(11)
)
BEGIN
UPDATE position_view SET
    Creates = I_Creates,
    ReadOnly = I_ReadOnly,
    Updates = I_Updates,
    Deleting = I_Deleting,
    Created = I_Created,
    CreatedBy = I_CreatedBy,
    LastUpd = I_LastUpd,
    LastUpdBy = I_LastUpdBy
    WHERE RowID = I_RowID;END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
