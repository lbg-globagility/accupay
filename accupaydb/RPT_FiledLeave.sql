/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_FiledLeave`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_FiledLeave`(
    IN `OrganizationID` INT,
    IN `PayDateFrom` DATE,
    IN `PayDateTo` DATE
)
BEGIN

SELECT emp.RowID AS `DatCol1`,
       emp.EmployeeID AS `DatCol2`,
       CONCAT(emp.LastName, ', ', emp.FirstName, ' ', SUBSTR(emp.MiddleName, 1, 1)) AS `DatCol3`,
       DAYNAME(lev.LeaveStartDate) AS `DatCol11`,
       DATE_FORMAT(lev.LeaveStartDate, '%m/%d/%Y') AS `DatCol12`,
       lev.LeaveType AS `DatCol13`,
       (IFNULL(ete.SickLeaveHours, 0) + IFNULL(ete.VacationLeaveHours, 0)) AS `DatCol14`,
       lev.Reason AS `DatCol16`
FROM employee emp
LEFT JOIN employeeleave lev
       ON lev.EmployeeID = emp.RowID
LEFT JOIN employeetimeentry ete
       ON ete.Date = lev.LeaveStartDate AND
          ete.EmployeeID = lev.EmployeeID
WHERE emp.OrganizationID = OrganizationID AND
      lev.LeaveStartDate BETWEEN PayDateFrom AND PayDateTo;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
