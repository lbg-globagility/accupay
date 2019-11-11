/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFINS_paystub`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFINS_paystub` BEFORE INSERT ON `paystub` FOR EACH ROW BEGIN

DECLARE phhschced VARCHAR(50);

DECLARE hdmfschced VARCHAR(50);

DECLARE sssschced VARCHAR(50);

DECLARE wtaxschced VARCHAR(50);


SELECT IF(e.AgencyID IS NULL, d.PhHealthDeductSched, d.PhHealthDeductSchedAgency)
,IF(e.AgencyID IS NULL, d.HDMFDeductSched, d.HDMFDeductSchedAgency)
,IF(e.AgencyID IS NULL, d.SSSDeductSched, d.SSSDeductSchedAgency)
,IF(e.AgencyID IS NULL, d.WTaxDeductSched, d.WTaxDeductSchedAgency)
FROM employee e
INNER JOIN position pos ON pos.RowID=e.PositionID AND pos.OrganizationID=e.OrganizationID
INNER JOIN `division` d ON d.RowID=pos.DivisionId AND d.OrganizationID=e.OrganizationID
WHERE e.RowID=NEW.EmployeeID
AND e.OrganizationID=NEW.OrganizationID
INTO phhschced
    ,hdmfschced
    ,sssschced
    ,wtaxschced;

SET NEW.FirstTimeSalary = (SELECT (StartDate BETWEEN NEW.PayFromDate AND NEW.PayToDate) FROM employee WHERE RowID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID);

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
