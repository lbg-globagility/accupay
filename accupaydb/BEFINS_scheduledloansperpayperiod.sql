-- Dumping structure for trigger accupaydb_benchmark.BEFINS_scheduledloansperpayperiod
DROP TRIGGER IF EXISTS `BEFINS_scheduledloansperpayperiod`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFINS_scheduledloansperpayperiod` BEFORE INSERT ON `scheduledloansperpayperiod` FOR EACH ROW BEGIN

IF NEW.PaystubID IS NULL THEN
	SET NEW.PaystubID = (SELECT RowID
								FROM paystub
								WHERE PayPeriodID = NEW.PayPeriodID
								AND EmployeeID = NEW.EmployeeID);
END IF
;

END//
DELIMITER ;