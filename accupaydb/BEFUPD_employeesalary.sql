/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFUPD_employeesalary`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFUPD_employeesalary` BEFORE UPDATE ON `employeesalary` FOR EACH ROW BEGIN

DECLARE e_status VARCHAR(50);

DECLARE e_type VARCHAR(50);

DECLARE e_payfreqID INT(11);

DECLARE e_workdayyear INT(11);

DECLARE hasadditionalamount CHAR(1);

DECLARE e_agencyID INT(11);

DECLARE pay_freq_type VARCHAR(50);


SET NEW.FilingStatusID = IFNULL(NEW.FilingStatusID, 1);

SELECT
    e.EmploymentStatus,
    e.EmployeeType,
    e.PayFrequencyID,
    e.WorkDaysPerYear,
    e.AgencyID,
    pf.PayFrequencyType
FROM employee e
INNER JOIN payfrequency pf
ON pf.RowID = e.PayFrequencyID
WHERE e.RowID = NEW.EmployeeID
INTO
    e_status,
    e_type,
    e_payfreqID,
    e_workdayyear,
    e_agencyID,
    pay_freq_type;

SET NEW.BasicPay = NEW.Salary / IF(LOCATE(e_type,CONCAT(pay_freq_type,'Fixed')) > 0, PAYFREQUENCY_DIVISOR(pay_freq_type), PAYFREQUENCY_DIVISOR(e_type));

IF NEW.PhilHealthDeduction IS NULL THEN
	SET NEW.PhilHealthDeduction = 0;
END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
