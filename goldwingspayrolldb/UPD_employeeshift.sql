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

-- Dumping structure for procedure UPD_employeeshift
DROP PROCEDURE IF EXISTS `UPD_employeeshift`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `UPD_employeeshift`(IN `esh_RowID` INT, IN `UserRowID` INT, IN `esh_DateFrom` DATE, IN `esh_DateTo` DATE, IN `esh_ShiftID` INT, IN `esh_NShift` TINYINT, IN `esh_IsRestDay` TINYINT)
    DETERMINISTIC
BEGIN

UPDATE employeeshift
SET
lastupd = CURRENT_TIMESTAMP()
, lastupdby = UserRowID
, EffectiveFrom = esh_DateFrom
, EffectiveTo = esh_DateTo
, ShiftID = IF(IFNULL(esh_ShiftID,0)=0, NULL, esh_ShiftID)
, NightShift = esh_NShift
, RestDay = esh_IsRestDay
WHERE RowID = esh_RowID;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
