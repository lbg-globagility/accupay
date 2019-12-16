/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFUPD_payperiod`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFUPD_payperiod` BEFORE UPDATE ON `payperiod` FOR EACH ROW BEGIN

DECLARE payfreq_divisor INT(11);

DECLARE min_date
        ,max_date
		  ,last_friday_date DATE;

IF NEW.TotalGrossSalary = 1 THEN

    SELECT PAYFREQUENCY_DIVISOR(pf.PayFrequencyType) FROM payfrequency pf WHERE pf.RowID=NEW.TotalGrossSalary INTO payfreq_divisor;

    SET NEW.OrdinalValue = (NEW.`Month` * payfreq_divisor) - (NEW.`Half` * 1);

ELSEIF NEW.TotalGrossSalary = 4 THEN
   
   
   
	SET NEW.Half = 2; # set this record as a typical week of the month

	SET @week_ordinal_value = 0; SET @week_indx = 0;
	
	SELECT i.`Result`
	FROM (SELECT pp.RowID
			,(@week_indx := @week_indx + 1) `Result`
			FROM payperiod pp
			INNER JOIN payfrequency pf ON pf.PayFrequencyType='WEEKLY'
			INNER JOIN payperiod pyp ON pyp.RowID=NEW.RowID AND pyp.`Month`=pp.`Month` AND pyp.`Year`=pp.`Year` AND pyp.OrganizationID=pp.OrganizationID AND pyp.TotalGrossSalary=pp.TotalGrossSalary
			WHERE pp.TotalGrossSalary=pf.RowID) i
	WHERE i.`RowID`=NEW.RowID
	INTO @week_ordinal_value;
	
	SET NEW.WeekOrdinalValue = @week_ordinal_value;
		
	SET @rec_count = 0;
	SET @indx_firsthalf = 0;
	SET @indx_secondhalf = 0;

	SELECT COUNT(pp.RowID)
	FROM payperiod pp
	INNER JOIN payfrequency pf ON pf.PayFrequencyType='WEEKLY'
	INNER JOIN payperiod pyp ON pyp.RowID=NEW.RowID AND pyp.`Month`=pp.`Month` AND pyp.`Year`=pp.`Year` AND pyp.OrganizationID=pp.OrganizationID AND pyp.TotalGrossSalary=pp.TotalGrossSalary
	WHERE pp.TotalGrossSalary=pf.RowID
	INTO @rec_count;
	
	SET @half_value = ROUND( (@rec_count / 2), 0);
	SET @pp_rowid = 0;

	SELECT i.RowID
	FROM (SELECT pp.RowID
			,(@indx_firsthalf := @indx_firsthalf + 1) `Result`
			FROM payperiod pp
			INNER JOIN payfrequency pf ON pf.PayFrequencyType='WEEKLY'
			INNER JOIN payperiod pyp ON pyp.RowID=NEW.RowID AND pyp.`Month`=pp.`Month` AND pyp.`Year`=pp.`Year` AND pyp.OrganizationID=pp.OrganizationID AND pyp.TotalGrossSalary=pp.TotalGrossSalary
			WHERE pp.TotalGrossSalary=pf.RowID) i
	WHERE i.`Result`=@half_value
	INTO @pp_rowid;
		
	IF NEW.RowID = @pp_rowid THEN
		# SET NEW.Half = 1; # set this record as first half the month
		SET NEW.Half = 0;
		
	END IF;
	
	SET @pp_rowid = 0;
	
	SELECT i.RowID
	FROM (SELECT pp.RowID
			,(@indx_secondhalf := @indx_secondhalf + 1) `Result`
			FROM payperiod pp
			INNER JOIN payfrequency pf ON pf.PayFrequencyType='WEEKLY'
			INNER JOIN payperiod pyp ON pyp.RowID=NEW.RowID AND pyp.`Month`=pp.`Month` AND pyp.`Year`=pp.`Year` AND pyp.OrganizationID=pp.OrganizationID AND pyp.TotalGrossSalary=pp.TotalGrossSalary
			WHERE pp.TotalGrossSalary=pf.RowID) i
	WHERE i.`Result`=@rec_count
	INTO @pp_rowid;
		
	IF NEW.RowID = @pp_rowid THEN
		# SET NEW.Half = 0; # set this record as end of the month
		SET NEW.Half = 1;
		
	END IF;
	
   
   
   # ###########################################################################
   
	SELECT MIN(d.DateValue), MAX(d.DateValue)
	FROM dates d
	WHERE DATE_FORMAT(d.DateValue, '%Y%c') = CONCAT(NEW.`Year`, NEW.`Month`)
	INTO min_date
			,max_date;
	
	SELECT d.DateValue
	FROM dates d
	WHERE d.DateValue BETWEEN min_date AND max_date
	AND DAYNAME(d.DateValue)='Friday'
	ORDER BY d.DateValue DESC
	# LIMIT 1, 1 -- second to the last Friday of the month
	LIMIT 1 -- last Friday of the month
	INTO last_friday_date;
	
	IF OLD.IsLastFridayOfMonthFallsHere != (last_friday_date BETWEEN NEW.PayFromDate AND NEW.PayToDate) THEN
	
		SET NEW.IsLastFridayOfMonthFallsHere = (last_friday_date BETWEEN NEW.PayFromDate AND NEW.PayToDate);
		SET NEW.IsLastFridayOfMonthFallsHere = IFNULL(NEW.IsLastFridayOfMonthFallsHere, 0);
		
	END IF;
	
	SET NEW.IsLastFridayOfMonthFallsHere = IFNULL(NEW.IsLastFridayOfMonthFallsHere, 0);
		
END IF;




END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
