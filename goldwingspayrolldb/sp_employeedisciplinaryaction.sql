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

-- Dumping structure for procedure sp_employeedisciplinaryaction
DROP PROCEDURE IF EXISTS `sp_employeedisciplinaryaction`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_employeedisciplinaryaction`(IN `I_OrganizationID` INT(10), IN `I_Created` DATETIME, IN `I_CreatedBy` INT(10), IN `I_LastUpd` DATETIME, IN `I_LastUpdBy` INT(10), IN `I_EmployeeID` INT(10), IN `I_DateFrom` DATE, IN `I_DateTo` DATE, IN `I_FindingID` INT(10), IN `I_FindingDescription` VARCHAR(2000), IN `I_Action` VARCHAR(100), IN `I_Comments` VARCHAR(500))
    DETERMINISTIC
BEGIN
INSERT INTO employeedisciplinaryaction
 (
    OrganizationID,
    Created,
    CreatedBy,
    LastUpd,
    LastUpdBy,
    EmployeeID,
    DateFrom,
    DateTo,
    FindingID,
    FindingDescription,
    `Action`,
    Comments
)
VALUES
(
    I_OrganizationID,
    I_Created,
    I_CreatedBy,
    I_LastUpd,
    I_LastUpdBy,
    I_EmployeeID,
    I_DateFrom,
    I_DateTo,
    I_FindingID,
    I_FindingDescription,
    I_Action,
    I_Comments
);
END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
