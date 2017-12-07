/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `GET_empPerfectMonthCount`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GET_empPerfectMonthCount`(`OrganizID` INT, `EmpRowID` INT, `FirstDate` DATE, `LastDate` DATE) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11) DEFAULT 0;

DECLARE orgMinWorkDays INT(11);

DECLARE og_MinDaysPerMonth DECIMAL(11,5) DEFAULT 0;

SELECT GET_empworkdaysperyear(EmpRowID) INTO orgMinWorkDays;


SET og_MinDaysPerMonth = orgMinWorkDays / 12;

SELECT GET_empPerfectDayCount(OrganizID, EmpRowID, FirstDate, LastDate) INTO returnvalue;

SET returnvalue = returnvalue / og_MinDaysPerMonth;

RETURN ROUND(returnvalue);

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
