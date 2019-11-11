/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `I_audittrail`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_audittrail`(IN `I_Created` DATETIME, IN `I_CreatedBy` INT(10), IN `I_LastUpd` DATETIME, IN `I_LastUpdBy` INT(10), IN `I_OrganizationID` INT(10), IN `I_ViewID` INT(10), IN `I_FieldChanged` VARCHAR(100), IN `I_ChangedRowID` INT(10), IN `I_OldValue` VARCHAR(200), IN `I_NewValue` VARCHAR(200), IN `I_ActionPerformed` VARCHAR(50)
)
    DETERMINISTIC
BEGIN
INSERT INTO audittrail
(
    Created,
    CreatedBy,
    LastUpd,
    LastUpdBy,
    OrganizationID,
    ViewID,
    FieldChanged,
    ChangedRowID,
    OldValue,
    NewValue,
    ActionPerformed
)
VALUES
(
    I_Created,
    I_CreatedBy,
    I_LastUpd,
    I_LastUpdBy,
    I_OrganizationID,
    I_ViewID,
    I_FieldChanged,
    I_ChangedRowID,
    I_OldValue,
    I_NewValue,
    I_ActionPerformed
);END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
