-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.11-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.0.0.4396
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for trigger goldwingspayrolldb.AFTINS_paystubbonus
DROP TRIGGER IF EXISTS `AFTINS_paystubbonus`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_paystubbonus` AFTER INSERT ON `paystubbonus` FOR EACH ROW BEGIN

DECLARE IsFirstHalfOfMonth CHAR(1);

DECLARE function_wrapper INT(11);

SELECT pp.`Half` FROM payperiod pp WHERE pp.RowID=NEW.PayPeriodID INTO IsFirstHalfOfMonth;

IF IsFirstHalfOfMonth = '1' THEN
	
	
	INSERT INTO bonusloandeduction
	(
		OrganizationID
		,CreatedBy
		,LoanSchedID
		,PayPeriodID
		,DeductionLoanAmount
	) SELECT NEW.OrganizationID,NEW.CreatedBy,els.RowID,NEW.PayPeriodID,els.DeductionAmount
		FROM employeeloanschedule els
		INNER JOIN (SELECT *
						FROM employeebonus
						WHERE EmployeeID=NEW.EmployeeID
						AND OrganizationID=NEW.OrganizationID
						AND (EffectiveStartDate >= NEW.PayFromDate OR EffectiveEndDate >= NEW.PayFromDate)
						AND (EffectiveStartDate <= NEW.PayToDate OR EffectiveEndDate <= NEW.PayToDate)
		) eb ON eb.RowID = els.BonusID
		WHERE els.OrganizationID=NEW.OrganizationID
		
		AND els.LoanPayPeriodLeft >= 1
		AND els.`Status`='In Progress'
		AND els.EmployeeID=NEW.EmployeeID
		AND els.DeductionSchedule IN ('First half','Per pay period')
		AND (els.DedEffectiveDateFrom >= NEW.PayFromDate OR els.DedEffectiveDateTo >= NEW.PayFromDate)
		AND (els.DedEffectiveDateFrom <= NEW.PayToDate OR els.DedEffectiveDateTo <= NEW.PayToDate)
	ON
	DUPLICATE
	KEY
	UPDATE
		LastUpdBy=NEW.CreatedBy;
		
	UPDATE employeeloanschedule els
	INNER JOIN (SELECT *
					FROM employeebonus
					WHERE EmployeeID=NEW.EmployeeID
					AND OrganizationID=NEW.OrganizationID
					AND (EffectiveStartDate >= NEW.PayFromDate OR EffectiveEndDate >= NEW.PayFromDate)
					AND (EffectiveStartDate <= NEW.PayToDate OR EffectiveEndDate <= NEW.PayToDate)
	) eb ON eb.RowID = els.BonusID
	SET els.LoanPayPeriodLeft = IF((els.LoanPayPeriodLeft - 1) < 0, 0, (els.LoanPayPeriodLeft - 1))
	,els.TotalBalanceLeft = els.TotalBalanceLeft - els.DeductionAmount
	WHERE els.OrganizationID=NEW.OrganizationID
	
	AND els.LoanPayPeriodLeft >= 1
	AND els.EmployeeID=NEW.EmployeeID
	AND els.DeductionSchedule IN ('First half','Per pay period')
	AND (els.DedEffectiveDateFrom >= NEW.PayFromDate OR els.DedEffectiveDateTo >= NEW.PayFromDate)
	AND (els.DedEffectiveDateFrom <= NEW.PayToDate OR els.DedEffectiveDateTo <= NEW.PayToDate);
	
ELSE
	

	INSERT INTO bonusloandeduction
	(
		OrganizationID
		,CreatedBy
		,LoanSchedID
		,PayPeriodID
		,DeductionLoanAmount
	) SELECT NEW.OrganizationID,NEW.CreatedBy,els.RowID,NEW.PayPeriodID,els.DeductionAmount
		FROM employeeloanschedule els
		INNER JOIN (SELECT *
						FROM employeebonus
						WHERE EmployeeID=NEW.EmployeeID
						AND OrganizationID=NEW.OrganizationID
						AND (EffectiveStartDate >= NEW.PayFromDate OR EffectiveEndDate >= NEW.PayFromDate)
						AND (EffectiveStartDate <= NEW.PayToDate OR EffectiveEndDate <= NEW.PayToDate)
		) eb ON eb.RowID = els.BonusID
		WHERE els.OrganizationID=NEW.OrganizationID
		
		AND els.LoanPayPeriodLeft >= 1
		AND els.EmployeeID=NEW.EmployeeID
		AND els.DeductionSchedule IN ('End of the month','Per pay period')
		AND (els.DedEffectiveDateFrom >= NEW.PayFromDate OR els.DedEffectiveDateTo >= NEW.PayFromDate)
		AND (els.DedEffectiveDateFrom <= NEW.PayToDate OR els.DedEffectiveDateTo <= NEW.PayToDate)
	ON
	DUPLICATE
	KEY
	UPDATE
		LastUpdBy=NEW.CreatedBy;
		
	UPDATE employeeloanschedule els
	INNER JOIN (SELECT *
					FROM employeebonus
					WHERE EmployeeID=NEW.EmployeeID
					AND OrganizationID=NEW.OrganizationID
					AND (EffectiveStartDate >= NEW.PayFromDate OR EffectiveEndDate >= NEW.PayFromDate)
					AND (EffectiveStartDate <= NEW.PayToDate OR EffectiveEndDate <= NEW.PayToDate)
	) eb ON eb.RowID = els.BonusID
	SET els.LoanPayPeriodLeft = IF((els.LoanPayPeriodLeft - 1) < 0, 0, (els.LoanPayPeriodLeft - 1))
	,els.TotalBalanceLeft = els.TotalBalanceLeft - els.DeductionAmount
	WHERE els.OrganizationID=NEW.OrganizationID
	
	AND els.LoanPayPeriodLeft >= 1
	AND els.EmployeeID=NEW.EmployeeID
	AND els.DeductionSchedule IN ('End of the month','Per pay period')
	AND (els.DedEffectiveDateFrom >= NEW.PayFromDate OR els.DedEffectiveDateTo >= NEW.PayFromDate)
	AND (els.DedEffectiveDateFrom <= NEW.PayToDate OR els.DedEffectiveDateTo <= NEW.PayToDate);
	
END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
