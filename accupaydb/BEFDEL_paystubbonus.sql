/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFDEL_paystubbonus`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFDEL_paystubbonus` BEFORE DELETE ON `paystubbonus` FOR EACH ROW BEGIN

DECLARE bld_rowid
        ,loan_rowid
        ,v_index
		  ,v_count INT(11);

SET v_index = 0;

SELECT COUNT(bld.RowID)
FROM bonusloandeduction bld
INNER JOIN employeeloanschedule els
        ON els.EmployeeID=OLD.EmployeeID
           AND els.RowID=bld.LoanSchedID
WHERE bld.PayPeriodID=OLD.PayPeriodID
INTO v_count;

WHILE v_index < v_count DO
	
	SELECT bld.RowID
	,els.RowID
	FROM bonusloandeduction bld
	INNER JOIN employeeloanschedule els
	        ON els.EmployeeID=OLD.EmployeeID
	           AND els.RowID=bld.LoanSchedID
	WHERE bld.PayPeriodID=OLD.PayPeriodID
	LIMIT 1
	INTO bld_rowid
	     ,loan_rowid;
	
	DELETE FROM bonusloandeduction WHERE RowID=bld_rowid;
	
	UPDATE employeeloanschedule els
	SET
	els.LoanPayPeriodLeft = (els.LoanPayPeriodLeft + 1)
	,els.TotalBalanceLeft = (els.TotalBalanceLeft + els.DeductionAmount)
	WHERE els.RowID=loan_rowid;

	SET v_index = (v_index + 1);

END WHILE;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
