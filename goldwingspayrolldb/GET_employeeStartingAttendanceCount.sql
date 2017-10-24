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

-- Dumping structure for function GET_employeeStartingAttendanceCount
DROP FUNCTION IF EXISTS `GET_employeeStartingAttendanceCount`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `GET_employeeStartingAttendanceCount`(`EmpRowID` INT, `Date_From` DATE, `Date_To` DATE) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11);

SELECT COUNT(ete.RowID)
FROM employeetimeentry ete
INNER JOIN employee e ON e.RowID=EmpRowID
WHERE ete.TotalDayPay!=0
AND ete.EmployeeID=ete.EmployeeID
AND ete.OrganizationID=e.OrganizationID
AND ete.`Date` BETWEEN IF(e.StartDate < Date_From, Date_From, e.StartDate) AND Date_To
INTO returnvalue;

IF returnvalue IS NULL THEN
    SET returnvalue = 0;
END IF;

RETURN returnvalue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
