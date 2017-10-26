/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `I_PRSorder`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_PRSorder`(IN `I_Created` DATETIME, IN `I_OrganizationID` INT(11), IN `I_OrderNumber` VARCHAR(10), IN `I_OrderDate` DATETIME, IN `I_Type` VARCHAR(50), IN `I_Status` VARCHAR(50), IN `I_StatusAsOf` VARCHAR(50), IN `I_InvoiceNo` INT(11), IN `I_LastUpd` DATETIME, IN `I_reatedBy` INT(11), IN `I_LastUpdBy` INT(11), IN `I_InventoryLocationID` INT(11), IN `I_RelatedPRSId` INT(11), IN `I_RelatedRRId` INT(11)
)
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
    InvoiceNo,
    LastUpd,
    reatedBy,
    LastUpdBy,
    InventoryLocationID,
    RelatedPRSId,
    RelatedRRId
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
    I_InvoiceNo,
    I_LastUpd,
    I_reatedBy,
    I_LastUpdBy,
    I_InventoryLocationID,
    I_RelatedPRSId,
    I_RelatedRRId
);END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
