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

-- Dumping structure for procedure U_Users
DROP PROCEDURE IF EXISTS `U_Users`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `U_Users`(IN `I_RowID` INT(11), IN `I_LastName` VARCHAR(50), IN `I_FirstName` VARCHAR(50), IN `I_MiddleName` VARCHAR(50), IN `I_PositionID` INT(11), IN `I_Created` DATETIME, IN `I_LastUpdBy` INT(11), IN `I_CreatedBy` INT(11), IN `I_LastUpd` DATETIME, IN `I_Status` VARCHAR(10)
, IN `I_EmailAddress` VARCHAR(50), IN `enc_userid` VARCHAR(50), IN `enc_pword` VARCHAR(50))
    DETERMINISTIC
BEGIN

UPDATE user SET
    LastName = I_LastName,
    FirstName = I_FirstName,
    MiddleName = I_MiddleName,
    PositionID = I_PositionID,
    Created = I_Created,
    LastUpdBy = I_LastUpdBy,
    CreatedBy = I_CreatedBy,
    LastUpd = I_LastUpd,
    `Status` = I_Status,
    EmailAddress = I_EmailAddress
    ,`UserID` = enc_userid
    ,`Password` = enc_pword
WHERE RowID = I_RowID;END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
