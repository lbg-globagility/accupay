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

-- Dumping structure for procedure VIEW_employeeoffset
DROP PROCEDURE IF EXISTS `VIEW_employeeoffset`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeeoffset`(IN `OrganizID` INT, IN `EmpRowID` INT, IN `pagenumber` INT)
BEGIN

SELECT eos.RowID
,eos.`Type`
,IFNULL(TIME_FORMAT(eos.StartTime,'%h:%i %p'),'')
,IFNULL(TIME_FORMAT(eos.EndTime,'%h:%i %p'),'')
,IFNULL(eos.StartDate,'')
,IFNULL(eos.EndDate,'')
,IFNULL(eos.`Status`,'')
,eos.Reason
,eos.Comments
FROM employeeoffset eos
WHERE eos.EmployeeID=EmpRowID
AND eos.OrganizationID=OrganizID
ORDER BY eos.StartDate, eos.EndDate
LIMIT pagenumber, 20;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
