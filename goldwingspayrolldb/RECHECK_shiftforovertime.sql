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

-- Dumping structure for procedure RECHECK_shiftforovertime
DROP PROCEDURE IF EXISTS `RECHECK_shiftforovertime`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RECHECK_shiftforovertime`(IN `OrganizID` INT)
    DETERMINISTIC
BEGIN

DECLARE emp_type TEXT;

DECLARE day_ofrest CHAR(1);

DECLARE is_restday CHAR(1);

DECLARE dutyhourscount DECIMAL(11,2);

DECLARE sh_timefrom TIME;

DECLARE sh_timeto TIME;

DECLARE has_shift CHAR(1);

DECLARE any_int INT(11);

DECLARE eshRowID INT(11);

DECLARE otappliedcount INT(11);




END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
