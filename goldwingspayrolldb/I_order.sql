/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `I_order`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_order`(IN `I_Created` DATETIME, IN `I_OrganizationID` INT(11), IN `I_OrderDate` DATETIME, IN `I_Type` VARCHAR(50), IN `I_TotalAmount` DECIMAL(10,2), IN `I_TotalDownPayment` DECIMAL(10,2), IN `I_TotalPayment` DECIMAL(10,2), IN `I_TotalBalance` DECIMAL(10,2), IN `I_Status` VARCHAR(50), IN `I_StatusAsOf` VARCHAR(50), IN `I_InvoiceNo` INT(11), IN `I_LastUpd` DATETIME, IN `I_CreatedBy` INT(11), IN `I_LastUpdBy` INT(11), IN `I_InventoryLocationID` INT(11)
    , IN `I_OrderNumber` VARCHAR(10), IN `I_Comments` VARCHAR(2000))
    DETERMINISTIC
BEGIN

    INSERT INTO `order`
    (
    Created,
    OrganizationID,
    OrderDate,
    Type,
    TotalAmount,
    TotalDownPayment,
    TotalPayment,
    TotalBalance,
    Status,
    StatusAsOf,
    InvoiceNo,
    LastUpd,
    CreatedBy,
    LastUpdBy,
    InventoryLocationID,
    OrderNumber,
    Comments
    )
    VALUES
    (
    I_Created,
    I_OrganizationID,
    I_OrderDate,
    I_Type,
    I_TotalAmount,
    I_TotalDownPayment,
    I_TotalPayment,
    I_TotalBalance,
    I_Status,
    I_StatusAsOf,
    I_InvoiceNo,
    I_LastUpd,
    I_CreatedBy,
    I_LastUpdBy,
    I_InventoryLocationID,
    I_OrderNumber,
    I_Comments
    );END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
