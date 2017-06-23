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

-- Dumping structure for procedure goldwingspayrolldb.GETVIEW_employeeTardinessUndertime
DROP PROCEDURE IF EXISTS `GETVIEW_employeeTardinessUndertime`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `GETVIEW_employeeTardinessUndertime`(IN `OrganizID` INT, IN `payp_FromDate` DATE, IN `payp_ToDate` DATE)
    DETERMINISTIC
BEGIN

	SELECT
	ete.EmployeeID
	,SUM(IFNULL(ete.HoursLate,0)) AS HoursLate
	,SUM(IFNULL(ete.HoursLateAmount,0)) AS HoursLateAmount
	,SUM(IFNULL(ete.UndertimeHours,0)) AS UndertimeHours
	,SUM(IFNULL(ete.UndertimeHoursAmount,0)) AS UndertimeHoursAmount
	FROM employeetimeentry ete
	WHERE ete.OrganizationID=OrganizID
	AND ete.Date BETWEEN payp_FromDate AND payp_ToDate
	GROUP BY ete.EmployeeID;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
