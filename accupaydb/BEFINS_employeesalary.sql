/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFINS_employeesalary`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFINS_employeesalary` BEFORE INSERT ON `employeesalary` FOR EACH ROW BEGIN

DECLARE e_type VARCHAR(50);

DECLARE e_workdayyear INT(11);

SELECT e.EmployeeType,e.WorkDaysPerYear FROM employee e WHERE e.RowID=NEW.EmployeeID AND e.OrganizationID=NEW.OrganizationID INTO e_type,e_workdayyear;

IF e_type = 'Daily' THEN

    SET NEW.PaySocialSecurityID = (SELECT RowID FROM paysocialsecurity WHERE ((NEW.BasicPay * e_workdayyear) / 12.00) BETWEEN RangeFromAmount AND RangeToAmount AND NEW.OverrideDiscardSSSContrib = 0 LIMIT 1);

    SET NEW.PayPhilhealthID = (SELECT RowID FROM payphilhealth WHERE ((NEW.BasicPay * e_workdayyear) / 12.00) BETWEEN SalaryRangeFrom AND SalaryRangeTo AND NEW.OverrideDiscardPhilHealthContrib = 0 LIMIT 1);

END IF;

IF IFNULL(NEW.UndeclaredSalary,0) = 0 THEN
    SET NEW.UndeclaredSalary = IFNULL(NEW.TrueSalary - NEW.`Salary`,0);
END IF;

IF NEW.PhilHealthDeduction IS NULL THEN
	SET NEW.PhilHealthDeduction = 0;
END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
