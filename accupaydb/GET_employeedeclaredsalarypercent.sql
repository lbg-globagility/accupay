/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `GET_employeedeclaredsalarypercent`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GET_employeedeclaredsalarypercent`(`EmpRowID` INT, `OrganizID` INT, `PayPFrom` DATE, `PayPTo` DATE) RETURNS decimal(11,6)
    DETERMINISTIC
BEGIN

DECLARE returnvalue DECIMAL(11,6) DEFAULT 0;

SELECT es.Salary / es.TrueSalary AS UndeclaredPercent
FROM employeesalary es
WHERE es.EmployeeID=EmpRowID
AND es.OrganizationID=OrganizID
AND (es.EffectiveDateFrom >= PayPFrom OR IFNULL(es.EffectiveDateTo,PayPTo) >= PayPFrom)
AND (es.EffectiveDateFrom <= PayPTo OR IFNULL(es.EffectiveDateTo,PayPTo) <= PayPTo)
LIMIT 1
INTO returnvalue;

SET returnvalue = IFNULL(returnvalue,0.0);

RETURN returnvalue;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
