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

-- Dumping structure for procedure DBoard_OTPending
DROP PROCEDURE IF EXISTS `DBoard_OTPending`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `DBoard_OTPending`(IN `OrganizID` INT)
    DETERMINISTIC
BEGIN

SELECT
e.EmployeeID
,CONCAT(e.LastName,',',e.FirstName,',',INITIALS(e.MiddleName,'.','1')) 'Employee Fullname'
,FORMAT(IF(TIME_FORMAT(eot.OTStartTime,'%p')='PM' AND TIME_FORMAT(eot.OTEndTime,'%p')='AM'
    ,IFNULL(((TIME_TO_SEC(TIMEDIFF(ADDTIME(eot.OTEndTime,'24:00'), eot.OTStartTime)) / 60) / 60),0)
    ,IFNULL(((TIME_TO_SEC(TIMEDIFF(eot.OTEndTime, eot.OTStartTime)) / 60) / 60),0)), 2) AS OTNumOfHours
,eot.RowID
FROM employeeovertime eot
LEFT JOIN employee e ON e.RowID=eot.EmployeeID
WHERE eot.OrganizationID=OrganizID
AND eot.OTStatus='Pending'
ORDER BY eot.Created;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
