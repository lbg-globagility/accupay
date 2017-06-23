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

-- Dumping structure for view goldwingspayrolldb.v_emptimeenetryholidaypay
DROP VIEW IF EXISTS `v_emptimeenetryholidaypay`;
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `v_emptimeenetryholidaypay`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` VIEW `goldwingspayrolldb`.`v_emptimeenetryholidaypay` AS SELECT ete.*
,pr.PayType
,pr.`PayRate`
,pr.OvertimeRate
,pr.NightDifferentialRate
,pr.NightDifferentialOTRate
,pr.RestDayRate
,pr.RestDayOvertimeRate
,ete.TotalDayPay - (ete.TotalDayPay / pr.`PayRate`) AS HolidayPay
FROM employeetimeentry ete
INNER JOIN payrate pr ON pr.OrganizationID=ete.OrganizationID AND pr.`Date`=ete.`Date` ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
