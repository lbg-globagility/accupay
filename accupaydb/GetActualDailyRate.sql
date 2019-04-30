/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `GetActualDailyRate`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GetActualDailyRate`(`EmpID` INT, `OrgID` INT, `paramDate` DATE) RETURNS decimal(15,4)
    DETERMINISTIC
BEGIN

DECLARE dailyrate DECIMAL(15, 6);

SELECT i.DailyRate
FROM (SELECT esa.*
		 ,ROUND(
		 (esa.TrueSalary
		  / (e.WorkDaysPerYear
		     / 12 # count of months per year
			  )), 6) `DailyRate`
		FROM employeesalary esa
		INNER JOIN employee e ON e.RowID=esa.EmployeeID AND e.EmployeeType IN ('Monthly', 'Fixed') AND e.RowID=EmpID AND e.OrganizationID=OrgID

	UNION
		SELECT esa.*
	   , ROUND(esa.TrueSalary, 6) `DailyRate`
		FROM employeesalary esa
		INNER JOIN employee e ON e.RowID=esa.EmployeeID AND e.EmployeeType = 'Daily' AND e.RowID=EmpID AND e.OrganizationID=OrgID
      ) i
WHERE paramDate BETWEEN i.EffectiveDateFrom AND IFNULL(i.EffectiveDateTo, paramDate)
AND DATEDIFF(paramDate, i.EffectiveDateFrom) >= 0
ORDER BY DATEDIFF(DATE_FORMAT(paramDate, @@date_format), i.EffectiveDateFrom)
LIMIT 1
INTO dailyrate;

RETURN IFNULL(dailyrate, 0);

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
