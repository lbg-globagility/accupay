/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `GET_paytodatepayperiod`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GET_paytodatepayperiod`(`OrganizID` INT, `anydate` DATE, `EmpPayFreqID` INT) RETURNS date
    DETERMINISTIC
BEGIN

DECLARE returnvalue DATE;

DECLARE pp_month CHAR(5);

DECLARE pp_year CHAR(5);

SELECT pp.`Month`
,pp.`Year`
FROM payperiod pp
WHERE pp.OrganizationID=OrganizID
AND anydate
BETWEEN pp.PayFromDate
AND pp.PayToDate
AND pp.TotalGrossSalary=EmpPayFreqID
LIMIT 1
INTO pp_month
      ,pp_year;

SELECT
pp.PayToDate
FROM payperiod pp
WHERE pp.OrganizationID=OrganizID
AND pp.`Year`=pp_year
AND pp.`Month`=pp_month
AND pp.TotalGrossSalary=EmpPayFreqID
ORDER BY pp.PayFromDate DESC,pp.PayToDate DESC
LIMIT 1
INTO returnvalue;

RETURN returnvalue;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
