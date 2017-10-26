/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `I_paymentproc`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_paymentproc`(IN `I_OrganizationID` INT(10), IN `I_CreatedBy` INT(10), IN `I_LastUpdBy` INT(10), IN `I_Created` DATETIME, IN `I_LastUpd` DATETIME, IN `I_PaymentType` VARCHAR(50), IN `I_Amount` DECIMAL(10,2), IN `I_FinancialInstitutionID` INT(10), IN `I_BankAccountNumber` VARCHAR(10), IN `I_BankCheckNumber` VARCHAR(10), IN `I_BankRoutingNumber` VARCHAR(10), IN `I_CardHolder` VARCHAR(100), IN `I_CardNumber` VARCHAR(10), IN `I_CreditMemoNumber` VARCHAR(10), IN `I_RequestedAmount` DECIMAL(10,2), IN `I_RemainingBalance` DECIMAL(10,2), IN `I_PaymentNo` INT(10), IN `I_PaymentDate` DATE, IN `I_PaymentMethod` VARCHAR(50))
BEGIN
INSERT INTO payment
(
    OrganizationID,
    CreatedBy,
    LastUpdBy,
    Created,
    LastUpd,
    PaymentType,
    Amount,
    FinancialInstitutionID,
    BankAccountNumber,
    BankCheckNumber,
    BankRoutingNumber,
    CardHolder,
    CardNumber,
    CreditMemoNumber,
    RequestedAmount,
    RemainingBalance,
    PaymentNo,
    PaymentDate,
    PaymentMethod
)
VALUES
(
    I_OrganizationID,
    I_CreatedBy,
    I_LastUpdBy,
    I_Created,
    I_LastUpd,
    I_PaymentType,
    I_Amount,
    I_FinancialInstitutionID,
    I_BankAccountNumber,
    I_BankCheckNumber,
    I_BankRoutingNumber,
    I_CardHolder,
    I_CardNumber,
    I_CreditMemoNumber,
    I_RequestedAmount,
    I_RemainingBalance,
    I_PaymentNo,
    I_PaymentDate,
    I_PaymentMethod
);END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
