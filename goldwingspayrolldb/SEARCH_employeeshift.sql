/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `SEARCH_employeeshift`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `SEARCH_employeeshift`(IN `OrganizID` INT, IN `DivisionRowID` INT, IN `EmployeeIDString` VARCHAR(50), IN `EmployeeLastName` VARCHAR(50))
    DETERMINISTIC
BEGIN

IF DivisionRowID > 0 THEN

    SELECT
    CONCAT(e.LastName,',',e.FirstName,',',INITIALS(e.MiddleName,'.','1')) AS name
    , e.EmployeeID
    , e.RowID
    ,(esh.esdRowID IS NOT NULL) AS IsByDayEncoding
    FROM (SELECT * FROM employee WHERE OrganizationID=OrganizID AND EmployeeID  =EmployeeIDString   AND LENGTH(EmployeeIDString) > 0
        UNION
            SELECT * FROM employee WHERE OrganizationID=OrganizID AND LastName  =EmployeeLastName   AND LENGTH(EmployeeLastName) > 0
        UNION
            SELECT * FROM employee WHERE OrganizationID=OrganizID AND LENGTH(TRIM(EmployeeIDString))=0 AND LENGTH(TRIM(EmployeeLastName))=0
            ) e
    LEFT JOIN (SELECT RowID AS esdRowID,EmployeeID FROM employeeshiftbyday WHERE OrganizationID=OrganizID GROUP BY EmployeeID) esh ON esh.EmployeeID=e.RowID
    INNER JOIN (SELECT * FROM `position` WHERE OrganizationID=OrganizID AND DivisionID=DivisionRowID) pos ON pos.RowID=e.PositionID

    ORDER BY e.RowID DESC;

ELSE

    SELECT
    CONCAT(e.LastName,',',e.FirstName,',',INITIALS(e.MiddleName,'.','1')) AS name
    , e.EmployeeID
    , e.RowID
    ,(esh.esdRowID IS NOT NULL) AS IsByDayEncoding
    FROM (SELECT * FROM employee WHERE OrganizationID=OrganizID AND EmployeeID  =EmployeeIDString   AND LENGTH(EmployeeIDString) > 0
        UNION
            SELECT * FROM employee WHERE OrganizationID=OrganizID AND LastName  =EmployeeLastName   AND LENGTH(EmployeeLastName) > 0
        UNION
            SELECT * FROM employee WHERE OrganizationID=OrganizID AND LENGTH(TRIM(EmployeeIDString))=0 AND LENGTH(TRIM(EmployeeLastName))=0
            ) e
    LEFT JOIN (SELECT RowID AS esdRowID,EmployeeID FROM employeeshiftbyday WHERE OrganizationID=OrganizID GROUP BY EmployeeID) esh ON esh.EmployeeID=e.RowID

    ORDER BY e.RowID DESC;

END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
