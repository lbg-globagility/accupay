/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_employee_for_payroll`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `VIEW_employee_for_payroll`(IN `OrganizID` INT, IN `Page_Number` INT, IN `SearchString` VARCHAR(20))
    DETERMINISTIC
BEGIN

SELECT
e.RowID
,e.EmployeeID AS `Employee ID`
,e.FirstName AS `First Name`
,e.MiddleName AS `Middle Name`
,e.LastName AS `Last Name`
,e.TINNo AS `TIN No.`
,e.SSSNo AS `SSS No.`
,e.HDMFNo AS `HDMF No.`
,e.PhilHealthNo AS `PhilHealth No.`
,e.EmploymentStatus AS `Employment status`
,pos.PositionName AS `Position name`








,e.EmployeeType AS `Employee Type`










































FROM (
    SELECT *
    FROM employee
    WHERE LENGTH(SearchString) = 0 AND OrganizationID=OrganizID
UNION
    SELECT e.*
    FROM employee e
    INNER JOIN employeesearchstring ess ON ess.EmpPrimaKey=e.RowID AND LOCATE(SearchString,ess.searchstring) > 0
    WHERE LENGTH(SearchString) > 0 AND e.OrganizationID=OrganizID

) e
LEFT JOIN position pos ON pos.RowID=e.PositionID
ORDER BY RowID DESC
LIMIT Page_Number,20;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
