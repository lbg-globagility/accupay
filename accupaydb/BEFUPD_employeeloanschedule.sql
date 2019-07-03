/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFUPD_employeeloanschedule`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFUPD_employeeloanschedule` BEFORE UPDATE ON `employeeloanschedule` FOR EACH ROW BEGIN

/* First update the pay period left based on the remaining balance and how many deductions to
 * get the balance to zero.
 */
SET NEW.LoanPayPeriodLeft = CEIL(NEW.TotalBalanceLeft / NEW.DeductionAmount);

/* If there's remaining balance left change the status to 'In Progress'. This should also only happen
 * if the status was `Complete` and not for any other status.
 */
IF NEW.TotalBalanceLeft > 0 THEN
    IF NEW.`Status` = 'Complete' THEN
        SET NEW.`Status` = 'In Progress';
    END IF;
END IF;

/* If the remaining balance is <= 0, then status should be `Complete`.
 */
IF NEW.TotalBalanceLeft <= 0 THEN
    SET NEW.`Status` = 'Complete';
END IF;

IF NEW.TotalBalanceLeft > NEW.TotalLoanAmount THEN
    SET NEW.TotalBalanceLeft = OLD.TotalLoanAmount;
END IF;

SET @is_charge_tobonust = (OLD.BonusID IS NULL AND NEW.BonusID IS NOT NULL);

IF @is_charge_tobonust = TRUE THEN
    SET NEW.LoanPayPeriodLeftForBonus = NEW.LoanPayPeriodLeft;
END IF;

/* Normalize the casing of the deduction schedule
 */
IF LCASE(NEW.DeductionSchedule) = 'end of the month' THEN
    SET NEW.DeductionSchedule = 'End of the month';
END IF;
IF LCASE(NEW.DeductionSchedule) = 'first half' THEN
    SET NEW.DeductionSchedule = 'First half';
END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
