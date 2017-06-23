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

-- Dumping structure for procedure goldwingspayrolldb.USER_dropdownposition
DROP PROCEDURE IF EXISTS `USER_dropdownposition`;
DELIMITER //
CREATE DEFINER=`root`@`%` PROCEDURE `USER_dropdownposition`(IN `organizid` INT, IN `userrowid` INT)
    DETERMINISTIC
BEGIN

	SELECT p.RowID,p.PositionName
	FROM `position` p
	INNER JOIN (SELECT p.PositionName FROM `user` u INNER JOIN `position` p ON p.RowID=u.PositionID WHERE u.RowID=userrowid) pp ON lcase(TRIM(p.PositionName))!=lcase(TRIM(pp.PositionName))
	WHERE p.OrganizationID=organizid
UNION
	SELECT p.RowID,p.PositionName
	FROM `user` u INNER JOIN `position` p ON p.RowID=u.PositionID
	WHERE u.RowID=userrowid;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
