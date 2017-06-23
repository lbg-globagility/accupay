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

-- Dumping structure for procedure goldwingspayrolldb.SP_employeesalary
DROP PROCEDURE IF EXISTS `SP_employeesalary`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_employeesalary`(IN `I_EmployeeID` INT(10), IN `I_Created` DATETIME, IN `I_CreatedBy` INT(10), IN `I_LastUpd` DATETIME, IN `I_LastUpdBy` INT(10), IN `I_OrganizationID` INT(10), IN `I_FilingStatusID` INT(10), IN `I_PaySocialSecurityID` INT(10), IN `I_PayPhilhealthID` INT(10), IN `I_HDMFAmount` DECIMAL(10,2), IN `I_BasicPay` DECIMAL(10,2), IN `I_NoofDependents` INT(10), IN `I_MaritalStatus` VARCHAR(50), IN `I_EffectiveDateFrom` DATE, IN `I_EffectiveDateTo` DATE)
    DETERMINISTIC
BEGIN
INSERT INTO employeesalary
(
	EmployeeID,
	Created,
	CreatedBy,
	LastUpd,
	LastUpdBy,
	OrganizationID,
	FilingStatusID,
	PaySocialSecurityID,
	PayPhilhealthID,
	HDMFAmount,
	BasicPay,
	NoofDependents,
	MaritalStatus,
	EffectiveDateFrom,
	EffectiveDateTo
)
VALUES
(
	I_EmployeeID,
	I_Created,
	I_CreatedBy,
	I_LastUpd,
	I_LastUpdBy,
	I_OrganizationID,
	I_FilingStatusID,
	I_PaySocialSecurityID,
	I_PayPhilhealthID,
	I_HDMFAmount,
	I_BasicPay,
	I_NoofDependents,
	I_MaritalStatus,
	I_EffectiveDateFrom,
	I_EffectiveDateTo
);
END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
