/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `SET_govtcontribsched_payperiod`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `SET_govtcontribsched_payperiod`(IN `OrganizID` INT)
    DETERMINISTIC
BEGIN

DECLARE i INT(11) DEFAULT 1;

DECLARE initdate DATE DEFAULT '2015-01-07';

myloop : LOOP

    IF i > 0 THEN


        UPDATE payperiod
        SET SSSContribSched = '1'
        WHERE OrganizationID=OrganizID AND TotalGrossSalary=4 AND initdate BETWEEN PayFromDate AND PayToDate;

        SET initdate = ADDDATE(initdate, INTERVAL 4 WEEK);

        SELECT EXISTS(
        SELECT RowID
        FROM payperiod
        WHERE OrganizationID=OrganizID
        AND TotalGrossSalary=4
        AND initdate BETWEEN PayFromDate
                         AND PayToDate LIMIT 1) INTO i;

    ELSE
        LEAVE myloop;
    END IF;

END LOOP myloop;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
