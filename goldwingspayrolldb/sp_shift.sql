/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `sp_shift`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_shift`(IN `I_OrganizationID` INT(10), IN `I_Created` DATETIME, IN `I_CreatedBy` INT(10), IN `I_LastUpd` DATETIME, IN `I_LastUpdBy` INT(10), IN `I_TimeFrom` TIME, IN `I_TimeTo` TIME)
    DETERMINISTIC
BEGIN
INSERT INTO  `shift`
(
    OrganizationID,
    Created,
    CreatedBy,
    LastUpd,
    LastUpdBy,
    TimeFrom,
    TimeTo
)
VALUES
(
    I_OrganizationID,
    I_Created,
    I_CreatedBy,
    I_LastUpd,
    I_LastUpdBy,
    I_TimeFrom,
    I_TimeTo
);
END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
