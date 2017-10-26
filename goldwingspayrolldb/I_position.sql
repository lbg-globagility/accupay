/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `I_position`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_position`(IN `I_PositionName` VARCHAR(50), IN `I_LastUpd` DATETIME, IN `I_Created` DATETIME, IN `I_CreatedBy` INT(11), IN `I_OrganizationID` INT(11), IN `I_LastUpdBy` INT(11)
    )
BEGIN
    INSERT INTO position
    (
    PositionName,
    LastUpd,
    Created,
    CreatedBy,
    OrganizationID,
    LastUpdBy
    )
    VALUES
    (
    I_PositionName,
    I_LastUpd,
    I_Created,
    I_CreatedBy,
    I_OrganizationID,
    I_LastUpdBy
    );END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
