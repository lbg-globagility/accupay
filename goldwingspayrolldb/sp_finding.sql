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

-- Dumping structure for procedure sp_finding
DROP PROCEDURE IF EXISTS `sp_finding`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_finding`(IN `I_OrganizationID` INT(10), IN `I_Description` VARCHAR(2000), IN `I_PartNo` VARCHAR(200), IN `I_Created` DATETIME, IN `I_LastUpd` DATETIME, IN `I_CreatedBy` INT(10), IN `I_LastUpdBy` INT(10), IN `I_CategoryID` INT(10))
BEGIN
INSERT INTO product
 (
    OrganizationID,
    Description,
    PartNo,
    Created,
    LastUpd,
    CreatedBy,
    LastUpdBy,
    CategoryID,
    Category
)
VALUES
(
    I_OrganizationID,
    I_Description,
    I_PartNo,
    I_Created,
    I_LastUpd,
    I_CreatedBy,
    I_LastUpdBy,
    I_CategoryID,
    (SELECT CategoryName FROM category WHERE RowID=I_CategoryID AND OrganizationID=I_OrganizationID LIMIT 1)
);
END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
