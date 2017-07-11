/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `I_invoice`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_invoice`(IN `I_InvoiceNo` INT(10), IN `I_CreatedBy` INT(10), IN `I_LastUpdBy` INT(10), IN `I_Created` DATETIME, IN `I_LastUpd` DATETIME, IN `I_OrganizationID` INT(10), IN `I_InvoiceDate` DATE, IN `I_Status` VARCHAR(50), IN `I_TotalInvoiceAmount` DECIMAL(10,2), IN `I_PaymentAmount` DECIMAL(10,2), IN `I_TotalDue` DECIMAL(10,2), IN `I_OrderID` INT(10), IN `I_InvoiceDueDate` DATE, IN `I_Comments` VARCHAR(2000), IN `I_BalanceDue` DECIMAL(10,2), IN `I_TotalTax` DECIMAL(10,2), IN `I_TotalVAT` DECIMAL(10,2)
)
BEGIN
INSERT INTO invoice
(
    InvoiceNo,
    CreatedBy,
    LastUpdBy,
    Created,
    LastUpd,
    OrganizationID,
    InvoiceDate,
    Status,
    TotalInvoiceAmount,
    PaymentAmount,
    TotalDue,
    OrderID,
    InvoiceDueDate,
    Comments,
    BalanceDue,
    TotalTax,
    TotalVAT
)
VALUES
(
    I_InvoiceNo,
    I_CreatedBy,
    I_LastUpdBy,
    I_Created,
    I_LastUpd,
    I_OrganizationID,
    I_InvoiceDate,
    I_Status,
    I_TotalInvoiceAmount,
    I_PaymentAmount,
    I_TotalDue,
    I_OrderID,
    I_InvoiceDueDate,
    I_Comments,
    I_BalanceDue,
    I_TotalTax,
    I_TotalVAT
);END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
