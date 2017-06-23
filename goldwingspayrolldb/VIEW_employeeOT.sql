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

-- Dumping structure for procedure goldwingspayrolldb.VIEW_employeeOT
DROP PROCEDURE IF EXISTS `VIEW_employeeOT`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeeOT`(IN `eot_EmployeeID` INT, IN `eot_OrganizationID` INT, IN `pagenumber` INT)
    DETERMINISTIC
BEGIN

SELECT
eot.RowID
,COALESCE(eot.OTType,'') 'LeaveType'
,IF(eot.OTStartTime IS NULL,'',CONCAT(SUBSTRING_INDEX(TIME_FORMAT(eot.OTStartTime,'%h:%i:%s'),':',2),RIGHT(TIME_FORMAT(eot.OTStartTime,'%r'),3))) 'OTStartTime'
,IF(eot.OTEndTime IS NULL,'',CONCAT(SUBSTRING_INDEX(TIME_FORMAT(eot.OTEndTime,'%h:%i:%s'),':',2),RIGHT(TIME_FORMAT(eot.OTEndTime,'%r'),3))) 'OTEndTime'
,COALESCE(DATE_FORMAT(eot.OTStartDate,'%m/%d/%Y'),'') 'OTStartDate'
,COALESCE(DATE_FORMAT(eot.OTEndDate,'%m/%d/%Y'),'') 'OTEndDate'
,COALESCE(eot.OTStatus,'') 'Status'
,COALESCE(eot.Reason,'') 'Reason'
,COALESCE(eot.Comments,'') 'Comments'
,COALESCE(eot.Image,'') 'Image'
,'view this'
,COALESCE((SELECT FileName FROM employeeattachments WHERE EmployeeID=eot.EmployeeID AND Type=CONCAT('Employee Overtime@',eot.RowID)),'') 'FileName'
,COALESCE((SELECT FileType FROM employeeattachments WHERE EmployeeID=eot.EmployeeID AND Type=CONCAT('Employee Overtime@',eot.RowID)),'') 'FileExtens'
FROM employeeovertime eot 
WHERE eot.OrganizationID=eot_OrganizationID
AND eot.EmployeeID=eot_EmployeeID
ORDER BY eot.OTStartDate,eot.OTEndDate
LIMIT pagenumber, 10;



END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
