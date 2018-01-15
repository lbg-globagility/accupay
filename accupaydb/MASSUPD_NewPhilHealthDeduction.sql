/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `MASSUPD_NewPhilHealthDeduction`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `MASSUPD_NewPhilHealthDeduction`(IN `org_rowid` INT)
    DETERMINISTIC
BEGIN

DECLARE row_ids VARCHAR(255);

DECLARE year_ofeffect INT(11) DEFAULT 2018;

DECLARE phh_rate DECIMAL(11, 4) DEFAULT 0.0275;

DECLARE default_phh_contrib DECIMAL(11, 4) DEFAULT 275;

DECLARE min_phh_contrib
        ,max_phh_contrib DECIMAL(11, 4);

DECLARE effect_fromdate
        , effect_todate
		  , effect_to_date
		  , _lastdate
		  , other_date DATE;

DECLARE _rowid
        , _count
		  , _index
		  , other_count
		  , emp_rowid INT(11);

SET min_phh_contrib = default_phh_contrib;

SET max_phh_contrib = 1100;

SET @is_exists = FALSE;

SELECT
EXISTS(SELECT lv.RowID
		 FROM listofval lv
		 WHERE lv.`Type`='PhilHealth'
		 AND lv.LIC IN ('DeductionType','MaximumContribution','MinimumContribution','Rate')
       )
INTO @is_exists;

IF @is_exists THEN

	SET phh_rate = (SELECT (lv.DisplayValue * 0.01) FROM listofval lv WHERE lv.`Type`='PhilHealth' AND lv.LIC = 'Rate' LIMIT 1);

	SET min_phh_contrib = (SELECT lv.DisplayValue FROM listofval lv WHERE lv.`Type`='PhilHealth' AND lv.LIC = 'MinimumContribution' LIMIT 1);

	SET max_phh_contrib = (SELECT lv.DisplayValue FROM listofval lv WHERE lv.`Type`='PhilHealth' AND lv.LIC = 'MaximumContribution' LIMIT 1);

END IF;

SELECT pp.PayFromDate, pp.PayToDate
FROM payperiod pp
WHERE pp.OrganizationID = org_rowid
AND pp.TotalGrossSalary = 1
AND pp.`Year` = year_ofeffect
AND pp.OrdinalValue = 1
LIMIT 1
INTO effect_fromdate
     ,effect_todate;

SET _count = 0;

SELECT COUNT(esa.RowID)
,GROUP_CONCAT(esa.RowID)
FROM employeesalary esa
INNER JOIN employee e ON e.RowID=esa.EmployeeID AND e.OrganizationID=esa.OrganizationID
INNER JOIN (SELECT pp.RowID
            , pp.PayFromDate, pp.PayToDate
            , pp.TotalGrossSalary
            FROM payperiod pp
				WHERE pp.OrganizationID = org_rowid
				AND pp.`Year` = year_ofeffect
				AND pp.OrdinalValue = 1) pp ON pp.TotalGrossSalary = e.PayFrequencyID
WHERE esa.OrganizationID=org_rowid
AND pp.PayFromDate BETWEEN esa.EffectiveDateFrom AND esa.EffectiveDateTo
INTO _count
     ,row_ids
;

SET _index = 0;

WHILE _index < _count DO

	SELECT esa.RowID
	, esa.EffectiveDateTo
	, pp.PayFromDate
	, esa.EmployeeID
	FROM employeesalary esa
	INNER JOIN employee e ON e.RowID=esa.EmployeeID AND e.OrganizationID=esa.OrganizationID
	INNER JOIN (SELECT pp.RowID
	            , pp.PayFromDate, pp.PayToDate
	            , pp.TotalGrossSalary
	            FROM payperiod pp
					WHERE pp.OrganizationID = org_rowid
					AND pp.`Year` = year_ofeffect
					AND pp.OrdinalValue = 1) pp ON pp.TotalGrossSalary = e.PayFrequencyID
	WHERE # esa.OrganizationID=org_rowid
	# AND pp.PayFromDate BETWEEN esa.EffectiveDateFrom AND esa.EffectiveDateTo
	# AND DATE(pp.Created) != CURDATE()
	# AND
	FIND_IN_SET(esa.RowID, row_ids) > 0
	LIMIT _index, 1
	INTO _rowid
	     , effect_to_date
	     , _lastdate
	     , emp_rowid
	;
	
	UPDATE employeesalary esa
	SET
	esa.EffectiveDateTo = SUBDATE(_lastdate, INTERVAL 1 DAY)
	, esa.LastUpd = CURRENT_TIMESTAMP()
	, esa.LastUpdBy = IFNULL(esa.LastUpdBy, esa.CreatedBy)
	WHERE esa.RowID = _rowid
	# AND esa.EffectiveDateFrom < SUBDATE(@_lastdate, INTERVAL 1 DAY)
	;
	
	SELECT COUNT(esa.RowID)
	FROM employeesalary esa
	WHERE esa.EmployeeID = emp_rowid
	AND esa.OrganizationID = org_rowid
	AND esa.EffectiveDateFrom >= _lastdate
	INTO other_count;
	
	IF other_count = 0 THEN
	
		INSERT INTO employeesalary (EmployeeID, Created, CreatedBy, OrganizationID, FilingStatusID, PaySocialSecurityID, PayPhilhealthID, PhilHealthDeduction, HDMFAmount, TrueSalary, BasicPay, Salary, UndeclaredSalary, BasicDailyPay, BasicHourlyPay, NoofDependents, MaritalStatus, PositionID, EffectiveDateFrom, EffectiveDateTo, OverrideDiscardSSSContrib, OverrideDiscardPhilHealthContrib)
		SELECT esa.EmployeeID, CURRENT_TIMESTAMP(), esa.CreatedBy, esa.OrganizationID, esa.FilingStatusID, esa.PaySocialSecurityID, esa.PayPhilhealthID, esa.PhilHealthDeduction, esa.HDMFAmount, esa.TrueSalary, esa.BasicPay, esa.Salary, esa.UndeclaredSalary, esa.BasicDailyPay, esa.BasicHourlyPay, esa.NoofDependents, esa.MaritalStatus, esa.PositionID
		, _lastdate, ADDDATE(_lastdate, INTERVAL 10 YEAR)
		, esa.OverrideDiscardSSSContrib, esa.OverrideDiscardPhilHealthContrib
		FROM employeesalary esa
		WHERE esa.RowID = _rowid
		ON DUPLICATE KEY UPDATE
		LastUpd = CURRENT_TIMESTAMP();
		
	ELSE
	
		SELECT MIN(esa.EffectiveDateFrom)
		FROM employeesalary esa
		WHERE esa.EmployeeID = emp_rowid
		AND esa.OrganizationID = org_rowid
		AND esa.EffectiveDateFrom >= _lastdate
		INTO other_date;
		
		IF _lastdate < SUBDATE(other_date, INTERVAL 1 DAY) THEN
		# effect_to_date > other_date
			INSERT INTO employeesalary (EmployeeID, Created, CreatedBy, OrganizationID, FilingStatusID, PaySocialSecurityID, PayPhilhealthID, PhilHealthDeduction, HDMFAmount, TrueSalary, BasicPay, Salary, UndeclaredSalary, BasicDailyPay, BasicHourlyPay, NoofDependents, MaritalStatus, PositionID, EffectiveDateFrom, EffectiveDateTo, OverrideDiscardSSSContrib, OverrideDiscardPhilHealthContrib)
			SELECT esa.EmployeeID, CURRENT_TIMESTAMP(), esa.CreatedBy, esa.OrganizationID, esa.FilingStatusID, esa.PaySocialSecurityID, esa.PayPhilhealthID, esa.PhilHealthDeduction, esa.HDMFAmount, esa.TrueSalary, esa.BasicPay, esa.Salary, esa.UndeclaredSalary, esa.BasicDailyPay, esa.BasicHourlyPay, esa.NoofDependents, esa.MaritalStatus, esa.PositionID
			, _lastdate, SUBDATE(other_date, INTERVAL 1 DAY)
			, esa.OverrideDiscardSSSContrib, esa.OverrideDiscardPhilHealthContrib
			FROM employeesalary esa
			WHERE esa.RowID = _rowid
			ON DUPLICATE KEY UPDATE
			LastUpd = CURRENT_TIMESTAMP();
			
		END IF;
			
	END IF;
	
	SET _index = (_index + 1);
	
END WHILE;

UPDATE employeesalary esa
INNER JOIN (SELECT ii.RowID
            , ii.MonthlySalary
            , ROUND((ii.MonthlySalary * phh_rate), 4) `ComputedPhilHealth`
            FROM employeesalary_withdailyrate ii
            WHERE ii.OrganizationID = org_rowid
            ) i ON i.RowID = esa.RowID
SET
esa.LastUpd = CURRENT_TIMESTAMP()
,esa.LastUpdBy = IFNULL(esa.LastUpdBy, esa.CreatedBy)
,esa.PhilHealthDeduction = IFNULL(min_phh_contrib, default_phh_contrib)
WHERE i.`ComputedPhilHealth` < min_phh_contrib
AND ((effect_fromdate >= esa.EffectiveDateFrom OR effect_fromdate >= esa.EffectiveDateTo)
     OR (effect_todate >= esa.EffectiveDateFrom OR effect_todate >= esa.EffectiveDateTo))
;

UPDATE employeesalary esa
INNER JOIN (SELECT ii.RowID
            , ii.MonthlySalary
            , ROUND((ii.MonthlySalary * phh_rate), 4) `ComputedPhilHealth`
            FROM employeesalary_withdailyrate ii
            WHERE ii.OrganizationID = org_rowid
            ) i ON i.RowID = esa.RowID
SET
esa.LastUpd = CURRENT_TIMESTAMP()
,esa.LastUpdBy = IFNULL(esa.LastUpdBy, esa.CreatedBy)
,esa.PhilHealthDeduction = IFNULL(i.`ComputedPhilHealth`, default_phh_contrib)
WHERE i.`ComputedPhilHealth` BETWEEN min_phh_contrib AND max_phh_contrib
AND ((effect_fromdate >= esa.EffectiveDateFrom OR effect_fromdate >= esa.EffectiveDateTo)
     OR (effect_todate >= esa.EffectiveDateFrom OR effect_todate >= esa.EffectiveDateTo))
;

UPDATE employeesalary esa
INNER JOIN (SELECT ii.RowID
            , ii.MonthlySalary
            , ROUND((ii.MonthlySalary * phh_rate), 4) `ComputedPhilHealth`
            FROM employeesalary_withdailyrate ii
            WHERE ii.OrganizationID = org_rowid
            ) i ON i.RowID = esa.RowID
SET
esa.LastUpd = CURRENT_TIMESTAMP()
,esa.LastUpdBy = IFNULL(esa.LastUpdBy, esa.CreatedBy)
,esa.PhilHealthDeduction = IFNULL(max_phh_contrib, default_phh_contrib)
WHERE i.`ComputedPhilHealth` > max_phh_contrib
AND ((effect_fromdate >= esa.EffectiveDateFrom OR effect_fromdate >= esa.EffectiveDateTo)
     OR (effect_todate >= esa.EffectiveDateFrom OR effect_todate >= esa.EffectiveDateTo))
;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
