/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_AttendanceDeduction`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_AttendanceDeduction`(IN `og_rowid` INT, IN `date_from` DATE, IN `date_to` DATE, IN `emp_rowid` INT)
    DETERMINISTIC
BEGIN

DECLARE mins_per_hour INT(11) DEFAULT 60;

SELECT
e.EmployeeType `DatCol1`
,dv.Name `DatCol2`

,(@mid_init := LEFT(e.MiddleName, 1)) `MiddleInit`

,CONCAT_WS(', ', e.LastName, e.FirstName, IF(LENGTH(@mid_init) = 0, NULL, CONCAT(@mid_init, '.'))) `DatCol3`

, ROUND(SUM(et.Absent), 2) `DatCol5`
, ROUND(SUM(et.HoursLateAmount), 2) `DatCol7`
, ROUND(SUM(et.UndertimeHoursAmount), 2) `DatCol9`
 
, SUM( (et.Absent > 0) ) `DatCol4`
, ROUND(SUM( (et.HoursLate * mins_per_hour) ), 2) `DatCol6`
, ROUND(SUM(et.UndertimeHours), 2) `DatCol8`
 
FROM employeetimeentry et
INNER JOIN employee e
        ON e.RowID=et.EmployeeID
		     AND e.OrganizationID=et.OrganizationID
		     AND FIND_IN_SET(e.EmploymentStatus, UNEMPLOYEMENT_STATUSES()) = 0
			  AND e.EmploymentStatus = 'Regular'
			  AND e.AgencyID IS NULL
INNER JOIN `position` pos
        ON pos.RowID=e.PositionID
           AND pos.OrganizationID=e.OrganizationID
INNER JOIN division dv
        ON dv.RowID=pos.DivisionId
WHERE et.OrganizationID=og_rowid
AND et.`Date` BETWEEN date_from AND date_to
AND (et.Absent + et.HoursLateAmount + et.UndertimeHoursAmount) > 0

GROUP BY e.RowID

ORDER BY FIELD(e.EmployeeType, 'Daily', 'Monthly', 'Fixed')
         ,dv.Name
         ,CONCAT(e.LastName, e.FirstName, e.MiddleName)
;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
