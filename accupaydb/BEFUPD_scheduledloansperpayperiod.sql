-- Dumping structure for trigger accupaydb_benchmark.BEFUPD_scheduledloansperpayperiod
DROP TRIGGER IF EXISTS `BEFUPD_scheduledloansperpayperiod`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFUPD_scheduledloansperpayperiod` BEFORE UPDATE ON `scheduledloansperpayperiod` FOR EACH ROW BEGIN

IF NEW.PaystubID IS NULL THEN
	SET NEW.PaystubID = (SELECT RowID
								FROM paystub
								WHERE PayPeriodID = NEW.PayPeriodID
								AND EmployeeID = NEW.EmployeeID);
END IF
;

END//
DELIMITER ;