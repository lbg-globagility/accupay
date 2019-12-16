/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `FREQUENT_leave`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `FREQUENT_leave`(IN `OrganizID` INT)
    DETERMINISTIC
BEGIN

DECLARE min_datebound DATE;

DECLARE max_datebound DATE;

SELECT pp.PayToDate FROM payperiod pp WHERE pp.OrganizationID=OrganizID AND pp.TotalGrossSalary=1 AND CONCAT(pp.`Month`,pp.`Year`)=DATE_FORMAT(SUBDATE(CURDATE(),INTERVAL 1 MONTH),'%c%Y') AND pp.`Half`='1' LIMIT 1 INTO min_datebound;

SELECT pp.PayToDate FROM payperiod pp WHERE pp.OrganizationID=OrganizID AND pp.TotalGrossSalary=1 AND CONCAT(pp.`Month`,pp.`Year`)=DATE_FORMAT(CURDATE(),'%c%Y') AND pp.`Half`=(CURDATE() BETWEEN pp.PayFromDate AND pp.PayToDate) LIMIT 1 INTO max_datebound;

SELECT
e.EmployeeID
,CONCAT(e.LastName,',',e.FirstName, IF(e.MiddleName='','',','),INITIALS(e.MiddleName,'. ','1')) AS Fullname
,COUNT(ete.RowID) AS Frequent
,IF(MONTH(pp.PayToDate) > MONTH(pp.PayFromDate), CONCAT(DATE_FORMAT(pp.PayFromDate,'%c/%e/%Y'),' to ',DATE_FORMAT(pp.PayToDate,'%c/%e/%Y')), CONCAT(DATE_FORMAT(pp.PayFromDate,'%c/%e'),' to ',DATE_FORMAT(pp.PayToDate,'%c/%e/%Y'))) AS CutOff
FROM employeetimeentry ete
INNER JOIN employee e ON e.RowID=ete.EmployeeID AND e.OrganizationID=ete.OrganizationID
INNER JOIN payperiod pp ON pp.OrganizationID=ete.OrganizationID AND pp.TotalGrossSalary=e.PayFrequencyID AND ete.`Date` BETWEEN pp.PayFromDate AND pp.PayToDate
WHERE ete.OrganizationID=OrganizID
AND (ete.VacationLeaveHours + ete.SickLeaveHours + ete.MaternityLeaveHours + ete.OtherLeaveHours) > 0
GROUP BY pp.PayFromDate DESC,pp.PayToDate DESC,e.LastName,e.FirstName;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
