/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `IsEndOfMonth_PayPeriod`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `IsEndOfMonth_PayPeriod`(`payperiodRowID` INT, `selectyear` INT) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnval INT(11);

SELECT EXISTS(
                    SELECT p.RowID
                    FROM payperiod p
                    INNER JOIN (SELECT *, SUBDATE(PayToDate, INTERVAL 1 DAY) AS PayFrDate
                                    FROM payperiod
                                    WHERE 28 BETWEEN DAY(PayFromDate) AND DAY(PayToDate)
                                    AND selectyear IN (YEAR(PayFromDate),YEAR(PayToDate))
                                    AND DATEDIFF(PayToDate,PayFromDate) != 4
                                    ) p2 ON IF(DAY(p.PayFromDate) > DAY(p.PayToDate), p.PayFromDate, p.PayToDate) BETWEEN p2.PayFrDate AND p2.PayToDate
                    WHERE p.RowID=payperiodRowID
                    ) INTO returnval;


RETURN returnval;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
