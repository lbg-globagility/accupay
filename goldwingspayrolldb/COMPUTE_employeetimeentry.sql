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

-- Dumping structure for function COMPUTE_employeetimeentry
DROP FUNCTION IF EXISTS `COMPUTE_employeetimeentry`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `COMPUTE_employeetimeentry`(`etent_EmployeeID` INT, `etent_OrganizationID` INT, `etent_Date` DATE, `etent_CreatedBy` INT, `etent_LastUpdBy` INT, `EmployeeStartDate` DATE) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE etentID INT(11);

SELECT GENERATE_employeetimeentry(etent_EmployeeID,etent_OrganizationID,etent_Date,etent_CreatedBy) INTO etentID;

RETURN etentID;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
