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

-- Dumping structure for procedure goldwingspayrolldb.VIEW_area
DROP PROCEDURE IF EXISTS `VIEW_area`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `VIEW_area`(IN `OrganizID` INT, IN `PageNumber` INT, IN `ar_RowID` INT)
    DETERMINISTIC
BEGIN

IF IFNULL(ar_RowID,0) = 0 THEN

	SELECT
	aa.RowID
	,aa.Name
	FROM `area` aa
	WHERE aa.OrganizationID=OrganizID
	LIMIT PageNumber, 10;

ELSE

	SELECT
	aa.RowID
	,aa.Name
	FROM `area` aa
	WHERE aa.OrganizationID=OrganizID
	AND LOCATE(ar_RowID,aa.RowID) > 0
	LIMIT PageNumber, 10;

END IF;
	
END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
