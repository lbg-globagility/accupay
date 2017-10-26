-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.12-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for procedure sp_employeeshiftentry
DROP PROCEDURE IF EXISTS `sp_employeeshiftentry`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employeeshiftentry`(IN `I_OrganizationID` INT(10), IN `I_Created` DATETIME, IN `I_CreatedBy` INT(10), IN `I_LastUpd` DATETIME, IN `I_LastUpdby` INT(10), IN `I_EmployeeID` INT(10), IN `I_ShiftID` INT(10), IN `I_EffectiveFrom` DATE, IN `I_EffectiveTo` DATE, IN `I_NightShift` CHAR(50), IN `I_RestDay` CHAR(50))
    DETERMINISTIC
BEGIN
INSERT INTO `employeeshift`
(
    OrganizationID,
    Created,
    CreatedBy,
    LastUpd,
    LastUpdBy,
    EmployeeID,
    ShiftID,
    EffectiveFrom,
    EffectiveTo,
    NightShift,
    RestDay
)
VALUES
(
    I_OrganizationID,
    I_Created,
    I_CreatedBy,
    I_LastUpd,
    I_LastUpdBy,
    I_EmployeeID,
    I_ShiftID,
    I_EffectiveFrom,
    I_EffectiveTo,
    I_NightShift,
    I_RestDay
);
END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
