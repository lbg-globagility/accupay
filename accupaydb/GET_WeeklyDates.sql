/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `GET_WeeklyDates`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `GET_WeeklyDates`(IN `param_PayPeriodID` INT)
    DETERMINISTIC
BEGIN

SELECT
ADDDATE(pyp.PayFromDate, INTERVAL g.n DAY) AS WkDate
FROM generator_16 g
INNER JOIN payperiod pp ON pp.RowID=param_PayPeriodID
INNER JOIN payperiod pyp ON pyp.OrganizationID=pp.OrganizationID AND CONCAT(pyp.`Year`,pyp.`Month`)=CONCAT(pp.`Year`,pp.`Month`) AND pyp.TotalGrossSalary=pp.TotalGrossSalary AND pyp.PayFromDate < pp.PayFromDate AND pyp.PayToDate < pp.PayToDate
WHERE g.n <= DATEDIFF(pyp.PayToDate,pyp.PayFromDate)
ORDER BY ADDDATE(pyp.PayFromDate, INTERVAL g.n DAY);

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
