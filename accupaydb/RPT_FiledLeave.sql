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

DECLARE defaultWorkHour INT(11) DEFAULT 8;

SET @leaveHours = 0.00;

SELECT emp.RowID AS `DatCol1`,
       emp.EmployeeID AS `DatCol2`,
       CONCAT_WS(', ', emp.LastName, emp.FirstName, SUBSTR(emp.MiddleName, 1, 1)) AS `DatCol3`,
       DAYNAME(ete.`Date`) AS `DatCol11`,
       DATE_FORMAT(ete.`Date`, '%m/%d/%Y') AS `DatCol12`,
       lev.LeaveType AS `DatCol13`,
       @leaveHours := (IFNULL(ete.SickLeaveHours, 0)
		                 + IFNULL(ete.VacationLeaveHours, 0)
		                 + IFNULL(ete.OtherLeaveHours, 0)) AS `DatCol14`,
       (@leaveHours / defaultWorkHour) `DatCol15`,
       lev.Reason AS `DatCol16`
FROM employee emp
INNER JOIN employeeleave lev
        ON lev.EmployeeID = emp.RowID
INNER JOIN employeetimeentry ete
        ON ete.`Date` BETWEEN lev.LeaveStartDate AND lev.LeaveEndDate
           AND ete.EmployeeID = lev.EmployeeID
INNER JOIN employeeshift esh
        ON esh.RowID=ete.EmployeeShiftID
INNER JOIN shift sh
        ON sh.RowID=esh.ShiftID
WHERE emp.OrganizationID = OrganizationID AND
      lev.LeaveStartDate BETWEEN PayDateFrom AND PayDateTo
		# AND FIND_IN_SET(emp.EmploymentStatus, UNEMPLOYEMENT_STATUSES()) = 0
		AND (IFNULL(ete.SickLeaveHours, 0)
		     + IFNULL(ete.VacationLeaveHours, 0)
			  + IFNULL(ete.OtherLeaveHours, 0)) != 0
ORDER BY CONCAT(emp.LastName,emp.FirstName), ete.`Date`
;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
