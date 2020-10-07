/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTINS_paystub_then_paystubitem`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_paystub_then_paystubitem` AFTER INSERT ON `paystub` FOR EACH ROW BEGIN

DECLARE $vacationLeaveHours DECIMAL(15, 4);
DECLARE $sickLeaveHours DECIMAL(15, 4);
DECLARE $otherLeaveHours DECIMAL(15, 4);
DECLARE $maternityLeaveHours DECIMAL(15, 4);

SELECT
    IFNULL(SUM(t.VacationLeaveHours), 0),
    IFNULL(SUM(t.SickLeaveHours), 0),
    IFNULL(SUM(t.MaternityLeaveHours), 0),
    IFNULL(SUM(t.OtherLeaveHours), 0)
FROM employeetimeentryactual t
WHERE t.OrganizationID = NEW.OrganizationID AND
    t.EmployeeID = NEW.EmployeeID AND
    t.`Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate
INTO
    $vacationLeaveHours,
    $sickLeaveHours,
    $maternityLeaveHours,
    $otherLeaveHours;

UPDATE employee e
SET e.LeaveBalance = GREATEST(e.LeaveBalance - $vacationLeaveHours, 0),
    e.SickLeaveBalance = GREATEST(e.SickLeaveBalance - $sickLeaveHours, 0),
    e.MaternityLeaveBalance = GREATEST(e.MaternityLeaveBalance - $maternityLeaveHours, 0),
    e.OtherLeaveBalance = GREATEST(e.OtherLeaveBalance - $otherLeaveHours, 0)
WHERE e.RowID = NEW.EmployeeID;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
