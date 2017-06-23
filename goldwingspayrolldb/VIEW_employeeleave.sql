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

-- Dumping structure for procedure goldwingspayrolldb.VIEW_employeeleave
DROP PROCEDURE IF EXISTS `VIEW_employeeleave`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeeleave`(IN `elv_EmployeeID` INT, IN `elv_OrganizationID` INT)
    DETERMINISTIC
BEGIN

SELECT
elv.RowID
,COALESCE(elv.LeaveType,'') 'LeaveType'
,IF(elv.LeaveStartTime IS NULL,'',CONCAT(SUBSTRING_INDEX(TIME_FORMAT(elv.LeaveStartTime,'%h:%i:%s'),':',2),RIGHT(TIME_FORMAT(elv.LeaveStartTime,'%r'),3))) 'LeaveStartTime'
,IF(elv.LeaveEndTime IS NULL,'',CONCAT(SUBSTRING_INDEX(TIME_FORMAT(elv.LeaveEndTime,'%h:%i:%s'),':',2),RIGHT(TIME_FORMAT(elv.LeaveEndTime,'%r'),3))) 'LeaveEndTime'
,COALESCE(DATE_FORMAT(elv.LeaveStartDate,'%m/%d/%Y'),'') 'LeaveStartDate'
,COALESCE(DATE_FORMAT(elv.LeaveEndDate,'%m/%d/%Y'),'') 'LeaveEndDate'
,COALESCE(elv.Reason,'') 'Reason'
,COALESCE(elv.Comments,'') 'Comments'
,COALESCE(elv.Image,'') 'Image'
,'view this'
,COALESCE((SELECT FileName FROM employeeattachments WHERE EmployeeID=elv.EmployeeID AND `Type`=CONCAT('Employee Leave@',elv.RowID)),'') 'FileName'
,COALESCE((SELECT FileType FROM employeeattachments WHERE EmployeeID=elv.EmployeeID AND `Type`=CONCAT('Employee Leave@',elv.RowID)),'') 'FileExtens'
,elv.`Status`
FROM employeeleave elv 
WHERE elv.OrganizationID=elv_OrganizationID
AND elv.EmployeeID=elv_EmployeeID;




END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
