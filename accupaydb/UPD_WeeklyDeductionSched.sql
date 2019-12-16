/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `UPD_WeeklyDeductionSched`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `UPD_WeeklyDeductionSched`(IN `og_rowid` INT, IN `is_employee_under_agency` BOOL, IN `text_benefit_deduction` VARCHAR(50), IN `benefit_weekly_deduction_sched` VARCHAR(50))
    DETERMINISTIC
    COMMENT 'is_employee_under_agency = (TRUE OR FALSE); text_benefit_deduction = (sss OR philhealth OR hdmf OR tax)'
BEGIN

DECLARE weekly_payfreq_id INT(11);


SELECT pf.RowID
FROM payfrequency pf
WHERE pf.PayFrequencyType = 'WEEKLY'
INTO weekly_payfreq_id;


# First week of the month,Second week of the month,Third week of the month,Last week of the month,Last Friday of the month

# text_benefit_deduction = (sss OR philhealth OR hdmf OR tax)


UPDATE payperiod pp
SET
pp.SSSWeeklyContribSched = 0
WHERE pp.TotalGrossSalary = weekly_payfreq_id
AND pp.OrganizationID = og_rowid
AND text_benefit_deduction = 'sss'
AND is_employee_under_agency = FALSE
;
UPDATE payperiod pp
SET
pp.PhHWeeklyContribSched = 0
WHERE pp.TotalGrossSalary = weekly_payfreq_id
AND pp.OrganizationID = og_rowid
AND text_benefit_deduction = 'philhealth'
AND is_employee_under_agency = FALSE
;
UPDATE payperiod pp
SET
pp.HDMFWeeklyContribSched = 0
WHERE pp.TotalGrossSalary = weekly_payfreq_id
AND pp.OrganizationID = og_rowid
AND text_benefit_deduction = 'hdmf'
AND is_employee_under_agency = FALSE
;
UPDATE payperiod pp
SET
pp.WTaxWeeklyContribSched = 0
WHERE pp.TotalGrossSalary = weekly_payfreq_id
AND pp.OrganizationID = og_rowid
AND text_benefit_deduction = 'tax'
AND is_employee_under_agency = FALSE
;

# ############################################################################################################

UPDATE payperiod pp
SET
pp.SSSWeeklyAgentContribSched = 0
WHERE pp.TotalGrossSalary = weekly_payfreq_id
AND pp.OrganizationID = og_rowid
AND text_benefit_deduction = 'sss'
AND is_employee_under_agency = TRUE
;
UPDATE payperiod pp
SET
pp.PhHWeeklyAgentContribSched = 0
WHERE pp.TotalGrossSalary = weekly_payfreq_id
AND pp.OrganizationID = og_rowid
AND text_benefit_deduction = 'philhealth'
AND is_employee_under_agency = TRUE
;
UPDATE payperiod pp
SET
pp.HDMFWeeklyAgentContribSched = 0
WHERE pp.TotalGrossSalary = weekly_payfreq_id
AND pp.OrganizationID = og_rowid
AND text_benefit_deduction = 'hdmf'
AND is_employee_under_agency = TRUE
;
UPDATE payperiod pp
SET
pp.WTaxWeeklyAgentContribSched = 0
WHERE pp.TotalGrossSalary = weekly_payfreq_id
AND pp.OrganizationID = og_rowid
AND text_benefit_deduction = 'tax'
AND is_employee_under_agency = TRUE
;


IF is_employee_under_agency THEN
	SET is_employee_under_agency = TRUE;
	
	IF benefit_weekly_deduction_sched = 'First week of the month' THEN
	
		IF text_benefit_deduction = 'sss' THEN
			
			UPDATE payperiod pp
			SET
			pp.SSSWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 1
			;
			
		ELSEIF text_benefit_deduction = 'philhealth' THEN
		
			UPDATE payperiod pp
			SET
			pp.PhHWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 1
			;
			
		ELSEIF text_benefit_deduction = 'hdmf' THEN
		
			UPDATE payperiod pp
			SET
			pp.HDMFWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 1
			;
			
		ELSEIF text_benefit_deduction = 'tax' THEN
		
			UPDATE payperiod pp
			SET
			pp.WTaxWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 1
			;
			
		END IF;
		
	ELSEIF benefit_weekly_deduction_sched = 'Second week of the month' THEN
	
		IF text_benefit_deduction = 'sss' THEN
			
			UPDATE payperiod pp
			SET
			pp.SSSWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 2
			;
			
		ELSEIF text_benefit_deduction = 'philhealth' THEN
		
			UPDATE payperiod pp
			SET
			pp.PhHWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 2
			;
			
		ELSEIF text_benefit_deduction = 'hdmf' THEN
		
			UPDATE payperiod pp
			SET
			pp.HDMFWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 2
			;
			
		ELSEIF text_benefit_deduction = 'tax' THEN
		
			UPDATE payperiod pp
			SET
			pp.WTaxWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 2
			;
			
		END IF;
		
	ELSEIF benefit_weekly_deduction_sched = 'Third week of the month' THEN
		
		IF text_benefit_deduction = 'sss' THEN
			
			UPDATE payperiod pp
			SET
			pp.SSSWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 3
			;
			
		ELSEIF text_benefit_deduction = 'philhealth' THEN
		
			UPDATE payperiod pp
			SET
			pp.PhHWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 3
			;
			
		ELSEIF text_benefit_deduction = 'hdmf' THEN
		
			UPDATE payperiod pp
			SET
			pp.HDMFWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 3
			;
			
		ELSEIF text_benefit_deduction = 'tax' THEN
		
			UPDATE payperiod pp
			SET
			pp.WTaxWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 3
			;
			
		END IF;
		
	ELSEIF benefit_weekly_deduction_sched = 'Last week of the month' THEN
	
		SET SESSION group_concat_max_len = 1024000;
		
		SET @row_ids = NULL;
		
		SELECT GROUP_CONCAT(pp.RowID) `Result`
		# SELECT pp.*
		FROM payperiod pp
		INNER JOIN (SELECT pp.*
						, MAX(pp.WeekOridnalValue) `MaxWeekOridnalValue`
						FROM payperiod pp
						WHERE pp.TotalGrossSalary = weekly_payfreq_id
						AND pp.OrganizationID = og_rowid
						GROUP BY pp.`Year`, pp.`Month`
						) i
						ON i.OrganizationID = pp.OrganizationID
						   AND i.`Year` = pp.`Year`
						   AND i.`Month` = pp.`Month`
						   AND i.`MaxWeekOridnalValue` = pp.WeekOridnalValue
		WHERE pp.TotalGrossSalary = weekly_payfreq_id
		AND pp.OrganizationID = og_rowid
		INTO @row_ids
		;

		IF text_benefit_deduction = 'sss' THEN
		
			UPDATE payperiod pp
			SET
			pp.SSSWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE FIND_IN_SET(pp.RowID, @row_ids) > 0
			;
			
		ELSEIF text_benefit_deduction = 'philhealth' THEN
		
			UPDATE payperiod pp
			SET
			pp.PhHWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE FIND_IN_SET(pp.RowID, @row_ids) > 0
			;
			
		ELSEIF text_benefit_deduction = 'hdmf' THEN
		
			UPDATE payperiod pp
			SET
			pp.HDMFWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE FIND_IN_SET(pp.RowID, @row_ids) > 0
			;
			
		ELSEIF text_benefit_deduction = 'tax' THEN
		
			UPDATE payperiod pp
			SET
			pp.WTaxWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE FIND_IN_SET(pp.RowID, @row_ids) > 0
			;
			
		END IF;
		
	ELSEIF benefit_weekly_deduction_sched = 'Last Friday of the month' THEN
		
		IF text_benefit_deduction = 'sss' THEN
		
			UPDATE payperiod pp
			SET
			pp.SSSWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.IsLastFridayOfMonthFallsHere = 1
			;
			
		ELSEIF text_benefit_deduction = 'philhealth' THEN
		
			UPDATE payperiod pp
			SET
			pp.PhHWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.IsLastFridayOfMonthFallsHere = 1
			;
			
		ELSEIF text_benefit_deduction = 'hdmf' THEN
		
			UPDATE payperiod pp
			SET
			pp.HDMFWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.IsLastFridayOfMonthFallsHere = 1
			;
			
		ELSEIF text_benefit_deduction = 'tax' THEN
		
			UPDATE payperiod pp
			SET
			pp.WTaxWeeklyAgentContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.IsLastFridayOfMonthFallsHere = 1
			;
			
		END IF;
		
	END IF;
	
	
	
ELSE # ####################################################################################################################



	SET is_employee_under_agency = FALSE;
	
	IF benefit_weekly_deduction_sched = 'First week of the month' THEN
	
		IF text_benefit_deduction = 'sss' THEN
			
			UPDATE payperiod pp
			SET
			pp.SSSWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 1
			;
			
		ELSEIF text_benefit_deduction = 'philhealth' THEN
		
			UPDATE payperiod pp
			SET
			pp.PhHWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 1
			;
			
		ELSEIF text_benefit_deduction = 'hdmf' THEN
		
			UPDATE payperiod pp
			SET
			pp.HDMFWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 1
			;
			
		ELSEIF text_benefit_deduction = 'tax' THEN
		
			UPDATE payperiod pp
			SET
			pp.WTaxWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 1
			;
			
		END IF;
		
	ELSEIF benefit_weekly_deduction_sched = 'Second week of the month' THEN
	
		IF text_benefit_deduction = 'sss' THEN
			
			UPDATE payperiod pp
			SET
			pp.SSSWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 2
			;
			
		ELSEIF text_benefit_deduction = 'philhealth' THEN
		
			UPDATE payperiod pp
			SET
			pp.PhHWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 2
			;
			
		ELSEIF text_benefit_deduction = 'hdmf' THEN
		
			UPDATE payperiod pp
			SET
			pp.HDMFWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 2
			;
			
		ELSEIF text_benefit_deduction = 'tax' THEN
		
			UPDATE payperiod pp
			SET
			pp.WTaxWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 2
			;
			
		END IF;
		
	ELSEIF benefit_weekly_deduction_sched = 'Third week of the month' THEN
		
		IF text_benefit_deduction = 'sss' THEN
			
			UPDATE payperiod pp
			SET
			pp.SSSWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 3
			;
			
		ELSEIF text_benefit_deduction = 'philhealth' THEN
		
			UPDATE payperiod pp
			SET
			pp.PhHWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 3
			;
			
		ELSEIF text_benefit_deduction = 'hdmf' THEN
		
			UPDATE payperiod pp
			SET
			pp.HDMFWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 3
			;
			
		ELSEIF text_benefit_deduction = 'tax' THEN
		
			UPDATE payperiod pp
			SET
			pp.WTaxWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.WeekOridnalValue = 3
			;
			
		END IF;
		
	ELSEIF benefit_weekly_deduction_sched = 'Last week of the month' THEN
	
		SET SESSION group_concat_max_len = 1024000;
		
		SET @row_ids = NULL;
		
		SELECT GROUP_CONCAT(pp.RowID) `Result`
		# SELECT pp.*
		FROM payperiod pp
		INNER JOIN (SELECT pp.*
						, MAX(pp.WeekOridnalValue) `MaxWeekOridnalValue`
						FROM payperiod pp
						WHERE pp.TotalGrossSalary = weekly_payfreq_id
						AND pp.OrganizationID = og_rowid
						GROUP BY pp.`Year`, pp.`Month`
						) i
						ON i.OrganizationID = pp.OrganizationID
						   AND i.`Year` = pp.`Year`
						   AND i.`Month` = pp.`Month`
						   AND i.`MaxWeekOridnalValue` = pp.WeekOridnalValue
		WHERE pp.TotalGrossSalary = weekly_payfreq_id
		AND pp.OrganizationID = og_rowid
		INTO @row_ids
		;

		IF text_benefit_deduction = 'sss' THEN
		
			UPDATE payperiod pp
			SET
			pp.SSSWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE FIND_IN_SET(pp.RowID, @row_ids) > 0
			;
			
		ELSEIF text_benefit_deduction = 'philhealth' THEN
		
			UPDATE payperiod pp
			SET
			pp.PhHWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE FIND_IN_SET(pp.RowID, @row_ids) > 0
			;
			
		ELSEIF text_benefit_deduction = 'hdmf' THEN
		
			UPDATE payperiod pp
			SET
			pp.HDMFWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE FIND_IN_SET(pp.RowID, @row_ids) > 0
			;
			
		ELSEIF text_benefit_deduction = 'tax' THEN
		
			UPDATE payperiod pp
			SET
			pp.WTaxWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE FIND_IN_SET(pp.RowID, @row_ids) > 0
			;
			
		END IF;
		
	ELSEIF benefit_weekly_deduction_sched = 'Last Friday of the month' THEN
	
		IF text_benefit_deduction = 'sss' THEN
		
			UPDATE payperiod pp
			SET
			pp.SSSWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.IsLastFridayOfMonthFallsHere = 1
			;
			
		ELSEIF text_benefit_deduction = 'philhealth' THEN
		
			UPDATE payperiod pp
			SET
			pp.PhHWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.IsLastFridayOfMonthFallsHere = 1
			;
			
		ELSEIF text_benefit_deduction = 'hdmf' THEN
		
			UPDATE payperiod pp
			SET
			pp.HDMFWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.IsLastFridayOfMonthFallsHere = 1
			;
			
		ELSEIF text_benefit_deduction = 'tax' THEN
		
			UPDATE payperiod pp
			SET
			pp.WTaxWeeklyContribSched = 1
			, pp.LastUpd = CURRENT_TIMESTAMP()
			, pp.LastUpdBy = IFNULL(pp.LastUpdBy, pp.CreatedBy)
			WHERE pp.TotalGrossSalary = weekly_payfreq_id
			AND pp.OrganizationID = og_rowid
			AND pp.IsLastFridayOfMonthFallsHere = 1
			;
			
		END IF;
		
	END IF;
	
END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
