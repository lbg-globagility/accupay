/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `I_orderitem`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_orderitem`(IN `I_ProductID` INT(10), IN `I_ParentOrderID` INT(10), IN `I_ProductName` VARCHAR(50), IN `I_PartNo` VARCHAR(200), IN `I_OrganizationID` INT(11), IN `I_Created` DATETIME, IN `I_LineNum` INT(11), IN `I_QtyRequested` INT(11), IN `I_QtyOrdered` INT(11), IN `I_Status` VARCHAR(10), IN `I_SRP` INT(11), IN `I_CreatedBy` INT(11), IN `I_LastUpd` DATETIME, IN `I_LastUpdBy` INT(11), IN `I_InvoiceNo` INT(10)
, IN `I_MainOfficeEndingInventory` INT(11), IN `I_TotalOverallEndingInventory` INT(11))
BEGIN
INSERT INTO orderitem
(
    ProductID,
    ParentOrderID,
    ProductName,
    PartNo,
    OrganizationID,
    Created,
    LineNum,
    QtyRequested,
    QtyOrdered,
    Status,
    SRP,
    CreatedBy,
    LastUpd,
    LastUpdBy,
    InvoiceNo,
    MainOfficeEndingInventory,
    TotalOverallEndingInventory
)
VALUES
(
    I_ProductID,
    I_ParentOrderID,
    I_ProductName,
    I_PartNo,
    I_OrganizationID,
    I_Created,
    I_LineNum,
    I_QtyRequested,
    I_QtyOrdered,
    I_Status,
    I_SRP,
    I_CreatedBy,
    I_LastUpd,
    I_LastUpdBy,
    I_InvoiceNo,
    I_MainOfficeEndingInventory,
    I_TotalOverallEndingInventory
);END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
