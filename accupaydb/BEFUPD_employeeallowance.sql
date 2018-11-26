/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFUPD_employeeallowance`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFUPD_employeeallowance` BEFORE UPDATE ON `employeeallowance` FOR EACH ROW BEGIN

DECLARE win_form_datetimepicker_mindate DATE DEFAULT STR_TO_DATE('1753-01-01', @@date_format);

IF NEW.EffectiveStartDate < win_form_datetimepicker_mindate THEN
	SET NEW.EffectiveStartDate = win_form_datetimepicker_mindate;
END IF;

IF NEW.EffectiveEndDate < win_form_datetimepicker_mindate THEN
	SET NEW.EffectiveEndDate = win_form_datetimepicker_mindate;
END IF;

/* If allowance is one time, ensure end date is the same as start date */
IF NEW.AllowanceFrequency = 'One time' THEN
	SET NEW.EffectiveEndDate = NEW.EffectiveStartDate;
END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
