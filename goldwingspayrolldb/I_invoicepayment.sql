-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.11-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.0.0.4396
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for procedure goldwingspayrolldb.I_invoicepayment
DROP PROCEDURE IF EXISTS `I_invoicepayment`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_invoicepayment`(IN `I_CreatedBy` INT(10), IN `I_OrderID` INT(10), IN `I_LastUpdBy` INT(10), IN `I_InvoiceID` INT(10), IN `I_PaymentID` INT(10), IN `I_LastUpd` DATETIME, IN `I_Created` DATETIME, IN `I_AppliedAmount` DECIMAL(10,2)
, IN `I_OrganizationID` INT(10))
BEGIN 
INSERT INTO invoicepayment
(
	CreatedBy,
	OrderID,
	LastUpdBy,
	InvoiceID,
	PaymentID,
	LastUpd,
	Created,
	AppliedAmount,
	OrganizationID
)	
VALUES
(
	I_CreatedBy,
	I_OrderID,
	I_LastUpdBy,
	I_InvoiceID,
	I_PaymentID,
	I_LastUpd,
	I_Created,
	I_AppliedAmount,
	I_OrganizationID
);END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
