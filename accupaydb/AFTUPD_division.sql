/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTUPD_division`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_division` AFTER UPDATE ON `division` FOR EACH ROW BEGIN

DECLARE anyint INT(11);

UPDATE employee e
INNER JOIN `position` ps
ON ps.DivisionId = NEW.RowID
SET # e.PayFrequencyID = NEW.PayFrequencyID,
    e.WorkDaysPerYear = NEW.WorkDaysPerYear,
    e.LateGracePeriod = NEW.GracePeriod,
    e.LastUpdBy = NEW.LastUpdBy
WHERE e.OrganizationID = NEW.OrganizationID
AND e.PositionID = ps.RowID;

IF NEW.AutomaticOvertimeFiling = 1 THEN
     SET anyint = 0;
END IF;

IF OLD.SSSDeductionWeekSchedule != NEW.SSSDeductionWeekSchedule THEN
	CALL UPD_WeeklyDeductionSched(NEW.OrganizationID, FALSE, 'sss', NEW.SSSDeductionWeekSchedule);
END IF;

IF OLD.PhilhealthDeductionWeekSchedule != NEW.PhilhealthDeductionWeekSchedule THEN
	CALL UPD_WeeklyDeductionSched(NEW.OrganizationID, FALSE, 'philhealth', NEW.PhilhealthDeductionWeekSchedule);
END IF;

IF OLD.PagIbigDeductionWeekSchedule != NEW.PagIbigDeductionWeekSchedule THEN
	CALL UPD_WeeklyDeductionSched(NEW.OrganizationID, FALSE, 'hdmf', NEW.PagIbigDeductionWeekSchedule);
END IF;

IF OLD.WithholdingTaxDeductionWeekSchedule != NEW.WithholdingTaxDeductionWeekSchedule THEN
	CALL UPD_WeeklyDeductionSched(NEW.OrganizationID, FALSE, 'tax', NEW.WithholdingTaxDeductionWeekSchedule);
END IF;


IF OLD.SSSDeductionWeekwithAgenSchedule != NEW.SSSDeductionWeekwithAgenSchedule THEN
	CALL UPD_WeeklyDeductionSched(NEW.OrganizationID, TRUE, 'sss', NEW.SSSDeductionWeekwithAgenSchedule);
END IF;

IF OLD.PhilhealthDeductionWeekwithAgenSchedule != NEW.PhilhealthDeductionWeekwithAgenSchedule THEN
	CALL UPD_WeeklyDeductionSched(NEW.OrganizationID, TRUE, 'philhealth', NEW.PhilhealthDeductionWeekwithAgenSchedule);
END IF;

IF OLD.PagIbigDeductionWeekwithAgenSchedule != NEW.PagIbigDeductionWeekwithAgenSchedule THEN
	CALL UPD_WeeklyDeductionSched(NEW.OrganizationID, TRUE, 'hdmf', NEW.PagIbigDeductionWeekwithAgenSchedule);
END IF;

IF OLD.WithholdingTaxDeductionWeekwithAgenSchedule != NEW.WithholdingTaxDeductionWeekwithAgenSchedule THEN
	CALL UPD_WeeklyDeductionSched(NEW.OrganizationID, TRUE, 'tax', NEW.WithholdingTaxDeductionWeekwithAgenSchedule);
END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
