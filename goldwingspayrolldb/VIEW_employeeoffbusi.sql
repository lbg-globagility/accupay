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

-- Dumping structure for procedure VIEW_employeeoffbusi
DROP PROCEDURE IF EXISTS `VIEW_employeeoffbusi`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeeoffbusi`(IN `obf_EmployeeID` INT, IN `obf_OrganizationID` INT)
    DETERMINISTIC
BEGIN



SELECT
obf.RowID
,COALESCE(obf.OffBusType,'') 'LeaveType'
,IF(obf.OffBusStartTime IS NULL,'',CONCAT(SUBSTRING_INDEX(TIME_FORMAT(obf.OffBusStartTime,'%h:%i:%s'),':',2),RIGHT(TIME_FORMAT(obf.OffBusStartTime,'%r'),3))) 'OffBusStartTime'
,IF(obf.OffBusEndTime IS NULL,'',CONCAT(SUBSTRING_INDEX(TIME_FORMAT(obf.OffBusEndTime,'%h:%i:%s'),':',2),RIGHT(TIME_FORMAT(obf.OffBusEndTime,'%r'),3))) 'OffBusEndTime'
,COALESCE(DATE_FORMAT(obf.OffBusStartDate,'%m/%d/%Y'),'') 'OTStartDate'
,COALESCE(DATE_FORMAT(obf.OffBusEndDate,'%m/%d/%Y'),'') 'OTEndDate'
,COALESCE(OffBusStatus,'') 'OffBusStatus'
,COALESCE(obf.Reason,'') 'Reason'
,COALESCE(obf.Comments,'') 'Comments'
,COALESCE(obf.Image,'') 'Image'
,'view this'
,COALESCE((SELECT FileName FROM employeeattachments WHERE EmployeeID=obf.EmployeeID AND Type=CONCAT('Official Business@',obf.RowID)),'') 'FileName'
,COALESCE((SELECT FileType FROM employeeattachments WHERE EmployeeID=obf.EmployeeID AND Type=CONCAT('Official Business@',obf.RowID)),'') 'FileExtens'
FROM employeeofficialbusiness obf
WHERE obf.OrganizationID=obf_OrganizationID
AND obf.EmployeeID=obf_EmployeeID;


END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
