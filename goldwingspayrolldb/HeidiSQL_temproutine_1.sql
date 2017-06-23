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

-- Dumping structure for procedure goldwingspayrolldb.HeidiSQL_temproutine_1
DROP PROCEDURE IF EXISTS `HeidiSQL_temproutine_1`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `HeidiSQL_temproutine_1`(IN `ps_OrganizationID` INT, IN `ps_PayPeriodID1` INT, IN `ps_PayPeriodID2` INT, IN `psi_undeclared` CHAR(1), IN `strSalaryDistrib` VARCHAR(50))
    DETERMINISTIC
SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
