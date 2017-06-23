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

-- Dumping structure for procedure goldwingspayrolldb.SP_employeeeducationUpdate
DROP PROCEDURE IF EXISTS `SP_employeeeducationUpdate`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_employeeeducationUpdate`(IN `I_DateFrom` VARCHAR(100), IN `I_DateTo` VARCHAR(50), IN `I_School` VARCHAR(100), IN `I_Degree` VARCHAR(100), IN `I_Course` VARCHAR(100), IN `I_Minor` VARCHAR(100), IN `I_EducationType` VARCHAR(100), IN `I_Remarks` VARCHAR(1000), IN `I_RowID` INT(11))
    DETERMINISTIC
BEGIN
UPDATE `employeeeducation` 
SET
	DateFrom = I_DateFrom,
	DateTo = I_DateTo,
	School = I_School,
	Degree = I_Degree,
	Course = I_Course,
	Minor = I_Minor,
	EducationType = I_EducationType,
	Remarks = I_Remarks
WHERE RowID = I_RowID;
END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
