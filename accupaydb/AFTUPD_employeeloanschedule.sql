/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTUPD_employeeloanschedule`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_employeeloanschedule` AFTER UPDATE ON `employeeloanschedule` FOR EACH ROW BEGIN

# SET @is_uncharge_tobonus = (OLD.BonusID IS NOT NULL AND NEW.BonusID IS NULL);
SET @is_uncharge_tobonus = (NEW.BonusID IS NULL);

IF @is_charge_tobonust = TRUE THEN

   UPDATE employeebonus eb
	SET
	# eb.RemainingBalance = (eb.RemainingBalance + NEW.DeductionAmount)
	eb.RemainingBalance = (eb.RemainingBalance + (NEW.DeductionAmount * NEW.LoanPayPeriodLeft))
	,eb.LastUpdBy = IFNULL(eb.LastUpdBy, eb.CreatedBy)
	,eb.LastUpd = CURRENT_TIMESTAMP()
	WHERE eb.RowID = OLD.BonusID
	# AND NEW.BonusPotentialPaymentForLoan = 0
	;

	/*UPDATE employeebonus eb
	SET
	eb.RemainingBalance = (eb.RemainingBalance + (NEW.DeductionAmount * NEW.LoanPayPeriodLeft))
	,eb.LastUpdBy = IFNULL(eb.LastUpdBy, eb.CreatedBy)
	,eb.LastUpd = CURRENT_TIMESTAMP()
	WHERE eb.RowID = OLD.BonusID
	AND NEW.BonusPotentialPaymentForLoan = 1;*/
	
END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
