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

-- Dumping structure for procedure VIEW_employeetimeentry_this_month
DROP PROCEDURE IF EXISTS `VIEW_employeetimeentry_this_month`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `VIEW_employeetimeentry_this_month`(IN `OrganizID` INT, IN `EmpRowID` INT, IN `ParamDate` DATE)
    DETERMINISTIC
BEGIN

DECLARE custom_date TEXT;

DECLARE monthfirstdate DATE;

DECLARE monthlastdate DATE;

SET custom_date = DATE_FORMAT(ParamDate,'%Y%c');


SELECT PayFromDate FROM payperiod WHERE OrganizationID=OrganizID AND CONCAT(`Year`,`Month`)=custom_date ORDER BY PayFromDate,PayToDate LIMIT 1 INTO monthfirstdate;

SELECT PayToDate FROM payperiod WHERE OrganizationID=OrganizID AND CONCAT(`Year`,`Month`)=custom_date ORDER BY PayFromDate DESC,PayToDate DESC LIMIT 1 INTO monthlastdate;

SELECT
PayFromDate
,PayToDate
FROM payperiod
WHERE OrganizationID=OrganizID
AND ParamDate IN (PayFromDate,PayToDate)
ORDER BY PayFromDate,PayToDate
INTO monthfirstdate
        ,monthlastdate;

SELECT ete.*
,DATE_FORMAT(ete.`Date`,'%e') AS DateDay
FROM employeetimeentry ete
WHERE ete.EmployeeID=EmpRowID
AND ete.OrganizationID=OrganizID
AND ete.Absent!=0
AND ete.`Date` BETWEEN monthfirstdate AND monthlastdate
ORDER BY ete.`Date`;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
