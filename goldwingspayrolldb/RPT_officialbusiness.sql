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

-- Dumping structure for procedure RPT_officialbusiness
DROP PROCEDURE IF EXISTS `RPT_officialbusiness`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_officialbusiness`(IN `OrganizID` INT, IN `OBDateFrom` DATE, IN `OBDateTo` DATE)
    DETERMINISTIC
BEGIN

SELECT
e.EmployeeID
,CONCAT(e.LastName,',',e.FirstName, IF(e.MiddleName='','',','),INITIALS(e.MiddleName,'. ','1')) AS Fullname
,FORMAT(SUM(IF(ob.TotalOBHrs > 8, (ob.TotalOBHrs - 1), ob.TotalOBHrs)), 2) AS TotalOBHrs
,SUM(IFNULL((DATEDIFF(eob.OffBusEndDate, eob.OffBusStartDate) + 1), 0)) 'TotalOBDays'
,IFNULL(eob.OffBusStatus,'') AS OffBusStatus
FROM employeeofficialbusiness eob
LEFT JOIN (
               SELECT
                IFNULL(IF(TIME_FORMAT(OffBusStartTime,'%p') = 'PM' AND TIME_FORMAT(OffBusEndTime,'%p') = 'AM', ((TIME_TO_SEC(TIMEDIFF(ADDTIME(OffBusEndTime,'24:00'), OffBusStartTime)) / 60) / 60), ((TIME_TO_SEC(TIMEDIFF(OffBusEndTime, OffBusStartTime)) / 60) / 60)),0) AS TotalOBHrs
                ,EmployeeID
                ,OrganizationID
                FROM employeeofficialbusiness
                WHERE OrganizationID=OrganizID
                AND (OffBusStartDate >= OBDateFrom OR OffBusEndDate >= OBDateFrom)
                AND (OffBusStartDate <= OBDateTo OR OffBusEndDate <= OBDateTo)
             ) ob ON ob.EmployeeID=eob.EmployeeID AND ob.OrganizationID=eob.OrganizationID
INNER JOIN employee e ON e.RowID=eob.EmployeeID
WHERE eob.OrganizationID=OrganizID
AND (eob.OffBusStartDate >= OBDateFrom OR eob.OffBusEndDate >= OBDateFrom)
AND (eob.OffBusStartDate <= OBDateTo OR eob.OffBusEndDate <= OBDateTo)
GROUP BY eob.EmployeeID;





END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
