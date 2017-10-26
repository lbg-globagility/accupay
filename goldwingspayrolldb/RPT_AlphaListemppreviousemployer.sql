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

-- Dumping structure for procedure RPT_AlphaListemppreviousemployer
DROP PROCEDURE IF EXISTS `RPT_AlphaListemppreviousemployer`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_AlphaListemppreviousemployer`(IN `OrganizID` INT)
    DETERMINISTIC
BEGIN



SELECT
epe.EmployeeID
,epe.TINNo
,epe.Name
,epe.BusinessAddress
FROM employeepreviousemployer epe
WHERE epe.OrganizationID=OrganizID
GROUP BY epe.EmployeeID
ORDER BY DATE(SUBSTRING_INDEX(epe.ExperienceFromTo,'@',1)) DESC,DATE(SUBSTRING_INDEX(epe.ExperienceFromTo,'@',-1)) DESC;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
