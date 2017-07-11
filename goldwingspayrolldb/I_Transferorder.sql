/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `I_Transferorder`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_Transferorder`(IN `I_Created` DATETIME, IN `I_OrganizationID` INT(11), IN `I_OrderNumber` VARCHAR(10), IN `I_OrderDate` DATETIME, IN `I_Type` VARCHAR(50), IN `I_Status` VARCHAR(50), IN `I_StatusAsOf` VARCHAR(50), IN `I_LastUpd` DATETIME, IN `I_CreatedBy` INT(11), IN `I_LastUpdBy` INT(11), IN `I_InventoryLocationID` INT(11), IN `I_BranchFromID` INT(11), IN `I_BranchToID` INT(11)
)
    DETERMINISTIC
BEGIN
INSERT INTO `order`
(
    Created,
    OrganizationID,
    OrderNumber,
    OrderDate,
    Type,
    Status,
    StatusAsOf,
    LastUpd,
    CreatedBy,
    LastUpdBy,
    InventoryLocationID,
    BranchFromID,
    BranchToID
)
VALUES
(
    I_Created,
    I_OrganizationID,
    I_OrderNumber,
    I_OrderDate,
    I_Type,
    I_Status,
    I_StatusAsOf,
    I_LastUpd,
    I_CreatedBy,
    I_LastUpdBy,
    I_InventoryLocationID,
    I_BranchFromID,
    I_BranchToID
);END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
