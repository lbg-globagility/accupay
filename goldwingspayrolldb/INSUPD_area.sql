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

-- Dumping structure for function goldwingspayrolldb.INSUPD_area
DROP FUNCTION IF EXISTS `INSUPD_area`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `INSUPD_area`(`arRowID` INT, `OrganizID` INT, `arName` VARCHAR(100), `arBusiID` INT, `UserRowID` INT) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11);

INSERT INTO `area`
(
	RowID
	,OrganizationID
	,Created
	,CreatedBy
	,Name
	,BusinessID
) VALUES (
	arRowID
	,OrganizID
	,CURRENT_TIMESTAMP()
	,UserRowID
	,arName
	,arBusiID
) ON
DUPLICATE
KEY
UPDATE
	LastUpd=CURRENT_TIMESTAMP()
	,LastUpdBy=UserRowID
	,Name=arName
	,BusinessID=arBusiID;SELECT @@Identity AS ID INTO returnvalue;
IF IFNULL(returnvalue,0) = 0 THEN SELECT RowID FROM `area` WHERE Name=arName AND OrganizationID=OrganizID INTO returnvalue; SET returnvalue = IFNULL(returnvalue,0); END IF;
RETURN returnvalue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
