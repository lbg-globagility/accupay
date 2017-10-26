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

-- Dumping structure for procedure SP_filingstatus
DROP PROCEDURE IF EXISTS `SP_filingstatus`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_filingstatus`(IN `I_Created` DATETIME, IN `I_LastUpd` DATETIME, IN `I_CreatedBy` INT(10), IN `I_LastUpdBy` INT(10), IN `I_FilingStatus` VARCHAR(50), IN `I_MaritalStatus` VARCHAR(50), IN `I_Dependent` INT(11))
    DETERMINISTIC
BEGIN
INSERT INTO filingstatus
(
Created,
LastUpd,
CreatedBy,
LastUpdBy,
FilingStatus,
MaritalStatus,
Dependent

)
VALUES
(
I_Created,
I_LastUpd,
I_CreatedBy,
I_LastUpdBy,
I_FilingStatus,
I_MaritalStatus,
I_Dependent
);
END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
