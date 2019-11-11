/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `SEARCH_employee_paystub`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `SEARCH_employee_paystub`(
	IN `$organizationId` INT,
	IN `$searchTerm` VARCHAR(50),
	IN `$payperiodId` INT,
	IN `$offset` INT,
	IN `$limit` INT
)
LANGUAGE SQL
DETERMINISTIC
CONTAINS SQL
SQL SECURITY DEFINER
COMMENT ''
BEGIN

DECLARE $skipSearch TINYINT(1) DEFAULT FALSE;

SET $skipSearch = NOT(IFNULL($searchTerm, '') > '');

SELECT
    e.RowID
	,e.EmployeeID
	,e.FirstName
	,e.MiddleName
	,e.LastName
	,e.EmployeeType
	,pos.PositionName
	,d.Name AS 'DivisionName'
FROM paystub p
INNER JOIN employee e
ON e.RowID = p.EmployeeID
LEFT JOIN `position` pos
ON e.PositionID=pos.RowID
LEFT JOIN `division` d
ON pos.DivisionId=d.RowID
WHERE p.OrganizationID = $organizationId AND
    p.PayPeriodID = $payperiodId AND
    ($skipSearch OR (
        e.FirstName = $searchTerm OR
        e.LastName = $searchTerm OR
        e.EmployeeID = $searchTerm
    ))
ORDER BY e.LastName, e.FirstName
LIMIT $offset, $limit;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
