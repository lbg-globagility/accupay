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

-- Dumping structure for procedure RPT_Tardiness
DROP PROCEDURE IF EXISTS `RPT_Tardiness`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_Tardiness`(IN `OrganizID` INT, IN `DateLateFrom` DATE, IN `DateLateTo` DATE)
    DETERMINISTIC
BEGIN

DECLARE diffdate INT(11);

SELECT DATEDIFF(DateLateTo, DateLateFrom) INTO diffdate;

IF ISNULL(diffdate) = '0' THEN

    SET diffdate = diffdate + 1;

ELSE

    SET diffdate = 0;

END IF;

SELECT
e.EmployeeID
,CONCAT(e.LastName,',',e.FirstName, IF(e.MiddleName='','',','),INITIALS(e.MiddleName,'. ','1')) AS Fullname
,SUM(ete.HoursLate) TotalHrsLate
,COUNT(ete.`Date`) 'DateCount'
,diffdate
FROM employeetimeentry ete
INNER JOIN employee e ON e.RowID=ete.EmployeeID
WHERE ete.OrganizationID=OrganizID
AND ete.`Date` BETWEEN DateLateFrom AND DateLateTo
AND ete.HoursLate > 0.25
GROUP BY ete.EmployeeID;



END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
