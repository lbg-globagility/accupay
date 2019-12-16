/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `UPD_leavebalance_newlyjoinedemployee`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `UPD_leavebalance_newlyjoinedemployee`(IN `og_rowid` INT, IN `param_date` DATE, IN `e_rowid` INT, IN `user_rowid` INT)
BEGIN

DECLARE leave_type TEXT DEFAULT 'Leave type';

IF e_rowid IS NULL THEN

	/**/
	UPDATE employee e
	INNER JOIN (
	            SELECT
					# e.*
					e.RowID
					, pp.`Year`
					# , (@max_date := STR_TO_DATE(MAX(pyp.PayToDate), @@date_format)) `MaxPayToDate`
					, STR_TO_DATE(MAX(pyp.PayToDate), @@date_format) `MaxPayToDate`
					# , (e.DateRegularized BETWEEN param_date AND STR_TO_DATE(MAX(pyp.PayToDate), @@date_format)) `IsDateRegularizedThisYear`
					, (e.DateRegularized BETWEEN param_date AND MAX(pyp.PayToDate)) `IsDateRegularizedThisYear`
					
					, pp.OrdinalValue
					, pp.PayFromDate, pp.PayToDate

					# , (@_count := COUNT(pyp.RowID)) `CountRecord`
					, (@_count := MAX(pyp.OrdinalValue)) `CountRecord`
					
					, (@_ctr := ((@_count - pp.OrdinalValue) + 1)) `CountRec`
					
					, ROUND((e.LeaveAllowance * ( @_ctr / @_count )), 2) `VLeaveBal`
					, ROUND((e.SickLeaveAllowance * ( @_ctr / @_count )), 2) `SLeaveBal`
					, ROUND((e.OtherLeaveAllowance * ( @_ctr / @_count )), 2) `OLeaveBal`
					, ROUND((e.MaternityLeaveAllowance * ( @_ctr / @_count )), 2) `MLeaveBal`
					
					FROM employee e
					INNER JOIN payperiod pp ON pp.TotalGrossSalary = e.PayFrequencyID AND pp.OrganizationID = e.OrganizationID AND e.DateRegularized BETWEEN pp.PayFromDate AND pp.PayToDate
					
					# INNER JOIN payperiod ppd ON ppd.TotalGrossSalary = e.PayFrequencyID AND ppd.OrganizationID = e.OrganizationID AND param_date BETWEEN ppd.PayFromDate AND ppd.PayToDate
					
					INNER JOIN payperiod pyp ON pyp.OrganizationID = pp.OrganizationID AND pyp.TotalGrossSalary = pp.TotalGrossSalary AND pyp.`Year` = pp.`Year`
					
					WHERE e.DateRegularized IS NOT NULL
					AND e.OrganizationID = og_rowid
					AND YEAR(e.DateRegularized) = YEAR(param_date)
					AND FIND_IN_SET(e.EmploymentStatus, UNEMPLOYEMENT_STATUSES()) = 0
					GROUP BY e.RowID
	        /**/) ee ON ee.RowID = e.RowID #AND ee.`IsDateRegularizedThisYear` = TRUE
	
	SET
	e.LeaveBalance = ee.`VLeaveBal`
	, e.SickLeaveBalance = ee.`SLeaveBal`
	, e.OtherLeaveBalance = ee.`OLeaveBal`
	, e.MaternityLeaveBalance = ee.`MLeaveBal`
	WHERE e.OrganizationID = og_rowid
	;
	
	SET @curr_timestamp = CURRENT_TIMESTAMP();
	
	INSERT INTO leavetransaction(OrganizationID,Created,CreatedBy,EmployeeID,ReferenceID,LeaveLedgerID,PayPeriodID,TransactionDate,`Type`,Balance,Amount,Comments)
	SELECT
	p.OrganizationID
	, @curr_timestamp
	, user_rowid
	, ee.RowID
	, NULL
	, ll.RowID
	, ee.`PayPeriodID`
	, ee.PayFromDate
	, 'Credit'
	, IF(p.PartNo = 'Vacation leave'
	     , ee.`VLeaveBal`
		  , IF(p.PartNo = 'Sick leave'
		       , ee.`SLeaveBal`
				 , IF(p.PartNo = 'Maternity/paternity leave'
				      , ee.`MLeaveBal`
						, IF(p.PartNo = 'Others'
						     , ee.`OLeaveBal`
							  , 0) # 'Leave w/o Pay'
							  )))
	, 0
	, p.PartNo
	
	FROM product p
	
	INNER JOIN (
	            SELECT
					e.RowID
					, pp.`Year`
					, pp.RowID `PayPeriodID`
					, STR_TO_DATE(MAX(pyp.PayToDate), @@date_format) `MaxPayToDate`
					, (e.DateRegularized BETWEEN param_date AND MAX(pyp.PayToDate)) `IsDateRegularizedThisYear`
					
					, pp.OrdinalValue
					, pp.PayFromDate, pp.PayToDate

					, (@_count := MAX(pyp.OrdinalValue)) `CountRecord`
					
					, (@_ctr := ((@_count - pp.OrdinalValue) + 1)) `CountRec`
					
					, ROUND((e.LeaveAllowance * ( @_ctr / @_count )), 2) `VLeaveBal`
					, ROUND((e.SickLeaveAllowance * ( @_ctr / @_count )), 2) `SLeaveBal`
					, ROUND((e.OtherLeaveAllowance * ( @_ctr / @_count )), 2) `OLeaveBal`
					, ROUND((e.MaternityLeaveAllowance * ( @_ctr / @_count )), 2) `MLeaveBal`
					
					FROM employee e
					INNER JOIN payperiod pp ON pp.TotalGrossSalary = e.PayFrequencyID AND pp.OrganizationID = e.OrganizationID AND e.DateRegularized BETWEEN pp.PayFromDate AND pp.PayToDate
					
					INNER JOIN payperiod pyp ON pyp.OrganizationID = pp.OrganizationID AND pyp.TotalGrossSalary = pp.TotalGrossSalary AND pyp.`Year` = pp.`Year`
					
					WHERE e.DateRegularized IS NOT NULL
					AND e.OrganizationID = og_rowid
					AND YEAR(e.DateRegularized) = YEAR(param_date)
					AND FIND_IN_SET(e.EmploymentStatus, UNEMPLOYEMENT_STATUSES()) = 0
					GROUP BY e.RowID
	            ) ee ON ee.RowID > 0 #AND ee.`IsDateRegularizedThisYear` = TRUE
	
	INNER JOIN leaveledger ll
	        ON ll.OrganizationID = p.OrganizationID
			     AND ll.ProductID = p.RowID
			     AND ll.EmployeeID = ee.RowID
	
	WHERE p.OrganizationID=og_rowid
	AND p.`Category`=leave_type
	;
	
	UPDATE leaveledger ll
	INNER JOIN leavetransaction lt
	        ON lt.Created = @curr_timestamp
			     AND lt.OrganizationID=ll.OrganizationID
			     AND lt.EmployeeID = ll.EmployeeID
			     AND lt.LeaveLedgerID = ll.RowID
	SET
	ll.LastTransactionID = lt.RowID
	, ll.LastUpd = @curr_timestamp
	, ll.LastUpdBy = user_rowid
	WHERE ll.OrganizationID = og_rowid
	;
	
	/*
	TO DO : prioritize sick leave balance, always borrow from VL to fill sick leave balance
	*/
	INSERT INTO leavetransaction(OrganizationID,Created,CreatedBy,EmployeeID,ReferenceID,LeaveLedgerID,PayPeriodID,TransactionDate,`Type`,Balance,Amount,Comments)
	SELECT
	p.OrganizationID
	, ADDDATE(@curr_timestamp, INTERVAL 1 SECOND)
	, user_rowid
	, ee.RowID
	, NULL
	, ll.RowID
	, ee.`PayPeriodID`
	, ee.PayFromDate
	, IF(p.PartNo = 'Sick leave', 'Credit', 'Debit')
	, IF(p.PartNo = 'Vacation leave'
	     , ee.`VLeaveBal`
		  , ee.`SLeaveBal`)
	, ABS( (ee.`SLeaveBal` - ee.SickLeaveAllowance) )
	, p.PartNo
	
	FROM product p
	
	INNER JOIN (
	            SELECT
					e.RowID
					, pp.`Year`
					, pp.RowID `PayPeriodID`
					, STR_TO_DATE(MAX(pyp.PayToDate), @@date_format) `MaxPayToDate`
					, (e.DateRegularized BETWEEN param_date AND MAX(pyp.PayToDate)) `IsDateRegularizedThisYear`
					
					, pp.OrdinalValue
					, pp.PayFromDate, pp.PayToDate

					, (@_count := MAX(pyp.OrdinalValue)) `CountRecord`
					
					, (@_ctr := ((@_count - pp.OrdinalValue) + 1)) `CountRec`
					
					, e.LeaveAllowance
					, e.SickLeaveAllowance
					
					, ROUND((e.LeaveAllowance * ( @_ctr / @_count )), 2) `VLeaveBal`
					, ROUND((e.SickLeaveAllowance * ( @_ctr / @_count )), 2) `SLeaveBal`
					, ROUND((e.OtherLeaveAllowance * ( @_ctr / @_count )), 2) `OLeaveBal`
					, ROUND((e.MaternityLeaveAllowance * ( @_ctr / @_count )), 2) `MLeaveBal`
					
					FROM employee e
					INNER JOIN payperiod pp ON pp.TotalGrossSalary = e.PayFrequencyID AND pp.OrganizationID = e.OrganizationID AND e.DateRegularized BETWEEN pp.PayFromDate AND pp.PayToDate
					
					INNER JOIN payperiod pyp ON pyp.OrganizationID = pp.OrganizationID AND pyp.TotalGrossSalary = pp.TotalGrossSalary AND pyp.`Year` = pp.`Year`
					
					WHERE e.DateRegularized IS NOT NULL
					AND e.OrganizationID = og_rowid
					AND YEAR(e.DateRegularized) = YEAR(param_date)
					AND FIND_IN_SET(e.EmploymentStatus, UNEMPLOYEMENT_STATUSES()) = 0
					GROUP BY e.RowID
	            ) ee ON ee.RowID > 0 #AND ee.`IsDateRegularizedThisYear` = TRUE
	
	INNER JOIN leaveledger ll
	        ON ll.OrganizationID = p.OrganizationID
			     AND ll.ProductID = p.RowID
			     AND ll.EmployeeID = ee.RowID
	
	WHERE p.OrganizationID=og_rowid
	AND p.`Category`=leave_type
	AND p.PartNo IN ('Vacation leave', 'Sick leave')
	AND (IF(p.PartNo = 'Vacation leave', ee.`VLeaveBal`, ee.`SLeaveBal`) != 0
	     AND ABS( (ee.`SLeaveBal` - ee.SickLeaveAllowance) ) != 0)
	;
	
	UPDATE leaveledger ll
	INNER JOIN leavetransaction lt
	        ON lt.Created = ADDDATE(@curr_timestamp, INTERVAL 1 SECOND)
			     AND lt.OrganizationID=ll.OrganizationID
			     AND lt.EmployeeID = ll.EmployeeID
			     AND lt.LeaveLedgerID = ll.RowID
	SET
	ll.LastTransactionID = lt.RowID
	, ll.LastUpd = ADDDATE(@curr_timestamp, INTERVAL 1 SECOND)
	, ll.LastUpdBy = user_rowid
	WHERE ll.OrganizationID = og_rowid
	;
	
	# #######################################################################
	# #######################################################################
	
	/*
	- after credited pro-rated leave balances
	- gather all filed leaves and debit those to the credited leave balances
	- make an insert statement for leavetransaction
	- and update leaveledger to latest
	*/
	CALL UPD_leaveledger_newjoinedemployee(og_rowid, param_date, e_rowid, user_rowid);
	
ELSEIF e_rowid IS NOT NULL THEN

	UPDATE employee e
	INNER JOIN (
	            SELECT
					# e.*
					e.RowID
					, pp.`Year`
					, STR_TO_DATE(MAX(pyp.PayToDate), @@date_format) `MaxPayToDate`
					, (e.DateRegularized BETWEEN param_date AND MAX(pyp.PayToDate)) `IsDateRegularizedThisYear`
					
					, pp.OrdinalValue
					, pp.PayFromDate, pp.PayToDate

					, (@_count := COUNT(pyp.RowID)) `CountRecord`
					
					, (@_ctr := ((@_count - pp.OrdinalValue) + 1)) `CountRec`
					
					, ROUND((e.LeaveAllowance * ( @_ctr / @_count )), 2) `VLeaveBal`
					, ROUND((e.SickLeaveAllowance * ( @_ctr / @_count )), 2) `SLeaveBal`
					, ROUND((e.OtherLeaveAllowance * ( @_ctr / @_count )), 2) `OLeaveBal`
					, ROUND((e.MaternityLeaveAllowance * ( @_ctr / @_count )), 2) `MLeaveBal`
					
					FROM employee e
					INNER JOIN payperiod pp ON pp.TotalGrossSalary = e.PayFrequencyID AND pp.OrganizationID = e.OrganizationID AND e.DateRegularized BETWEEN pp.PayFromDate AND pp.PayToDate
					
					# INNER JOIN payperiod ppd ON ppd.TotalGrossSalary = e.PayFrequencyID AND ppd.OrganizationID = e.OrganizationID AND param_date BETWEEN ppd.PayFromDate AND ppd.PayToDate
					
					INNER JOIN payperiod pyp ON pyp.OrganizationID = pp.OrganizationID AND pyp.TotalGrossSalary = pp.TotalGrossSalary AND pyp.`Year` = pp.`Year`
					
					WHERE e.RowID = e_rowid
					AND e.DateRegularized IS NOT NULL
					AND e.OrganizationID = og_rowid
					AND YEAR(e.DateRegularized) = YEAR(param_date)
					AND FIND_IN_SET(e.EmploymentStatus, UNEMPLOYEMENT_STATUSES()) = 0
					GROUP BY e.RowID
	            ) ee ON ee.RowID = e.RowID #AND ee.`IsDateRegularizedThisYear` = TRUE
	
	SET
	e.LeaveBalance = ee.`VLeaveBal`
	, e.SickLeaveBalance = ee.`SLeaveBal`
	, e.OtherLeaveBalance = ee.`OLeaveBal`
	, e.MaternityLeaveBalance = ee.`MLeaveBal`
	WHERE e.OrganizationID = og_rowid
	;
	
	INSERT INTO leavetransaction(OrganizationID,Created,CreatedBy,EmployeeID,ReferenceID,LeaveLedgerID,PayPeriodID,TransactionDate,`Type`,Balance,Amount,Comments)
	SELECT
	p.OrganizationID
	, CURRENT_TIMESTAMP()
	, user_rowid
	, ee.RowID
	, NULL
	, ll.RowID
	, ee.`PayPeriodID`
	, ee.PayFromDate
	, 'Credit'
	, IF(p.PartNo = 'Vacation leave'
	     , ee.`VLeaveBal`
		  , IF(p.PartNo = 'Sick leave'
		       , ee.`SLeaveBal`
				 , IF(p.PartNo = 'Maternity/paternity leave'
				      , ee.`MLeaveBal`
						, IF(p.PartNo = 'Others'
						     , ee.`OLeaveBal`
							  , 0) # 'Leave w/o Pay'
							  )))
	, 0
	, p.PartNo
	
	FROM product p
	
	INNER JOIN (
	            SELECT
					e.RowID
					, pp.`Year`
					, pp.RowID `PayPeriodID`
					, STR_TO_DATE(MAX(pyp.PayToDate), @@date_format) `MaxPayToDate`
					, (e.DateRegularized BETWEEN param_date AND MAX(pyp.PayToDate)) `IsDateRegularizedThisYear`
					
					, pp.OrdinalValue
					, pp.PayFromDate, pp.PayToDate

					, (@_count := MAX(pyp.OrdinalValue)) `CountRecord`
					
					, (@_ctr := ((@_count - pp.OrdinalValue) + 1)) `CountRec`
					
					, ROUND((e.LeaveAllowance * ( @_ctr / @_count )), 2) `VLeaveBal`
					, ROUND((e.SickLeaveAllowance * ( @_ctr / @_count )), 2) `SLeaveBal`
					, ROUND((e.OtherLeaveAllowance * ( @_ctr / @_count )), 2) `OLeaveBal`
					, ROUND((e.MaternityLeaveAllowance * ( @_ctr / @_count )), 2) `MLeaveBal`
					
					FROM employee e
					INNER JOIN payperiod pp ON pp.TotalGrossSalary = e.PayFrequencyID AND pp.OrganizationID = e.OrganizationID AND e.DateRegularized BETWEEN pp.PayFromDate AND pp.PayToDate
					
					INNER JOIN payperiod pyp ON pyp.OrganizationID = pp.OrganizationID AND pyp.TotalGrossSalary = pp.TotalGrossSalary AND pyp.`Year` = pp.`Year`
					
					WHERE e.RowID = e_rowid
					AND e.DateRegularized IS NOT NULL
					AND e.OrganizationID = og_rowid
					AND YEAR(e.DateRegularized) = YEAR(param_date)
					AND FIND_IN_SET(e.EmploymentStatus, UNEMPLOYEMENT_STATUSES()) = 0
					GROUP BY e.RowID
	            ) ee ON ee.RowID > 0 #AND ee.`IsDateRegularizedThisYear` = TRUE
	
	INNER JOIN leaveledger ll
	        ON ll.OrganizationID = p.OrganizationID
			     AND ll.ProductID = p.RowID
			     AND ll.EmployeeID = ee.RowID
	
	WHERE p.OrganizationID=og_rowid
	AND p.`Category`=leave_type
	;
	
	UPDATE leaveledger ll
	INNER JOIN leavetransaction lt
	        ON lt.Created = @curr_timestamp
			     AND lt.OrganizationID=ll.OrganizationID
			     AND lt.EmployeeID = ll.EmployeeID
			     AND lt.EmployeeID = e_rowid
	SET
	ll.LastTransactionID = lt.RowID
	, ll.LastUpd = @curr_timestamp
	, ll.LastUpdBy = user_rowid
	WHERE ll.OrganizationID = og_rowid
	;
	
	# ###################################################################################
	/*
	UPDATE employee e
	INNER JOIN (
	            SELECT
					# e.*
					e.RowID
					, pp.`Year`
					, STR_TO_DATE(MAX(pyp.PayToDate), @@date_format) `MaxPayToDate`
					, (e.DateRegularized BETWEEN param_date AND MAX(pyp.PayToDate)) `IsDateRegularizedThisYear`
					
					, (@_count := COUNT(pyp.RowID)) `CountRecord`
					
					, e.LeaveAllowance `VLeaveBal`
					, e.SickLeaveAllowance `SLeaveBal`
					, e.OtherLeaveAllowance `OLeaveBal`
					, e.MaternityLeaveAllowance `MLeaveBal`
					
					FROM employee e
					INNER JOIN payperiod pp ON pp.TotalGrossSalary = e.PayFrequencyID AND pp.OrganizationID = e.OrganizationID AND e.DateRegularized BETWEEN pp.PayFromDate AND pp.PayToDate
					
					# INNER JOIN payperiod ppd ON ppd.TotalGrossSalary = e.PayFrequencyID AND ppd.OrganizationID = e.OrganizationID AND param_date BETWEEN ppd.PayFromDate AND ppd.PayToDate
					
					INNER JOIN payperiod pyp ON pyp.OrganizationID = pp.OrganizationID AND pyp.TotalGrossSalary = pp.TotalGrossSalary AND pyp.`Year` = pp.`Year`
					
					WHERE e.RowID = e_rowid
					AND e.DateRegularized IS NOT NULL
					AND e.OrganizationID = og_rowid
					AND YEAR(e.DateRegularized) < YEAR(param_date)
					GROUP BY e.RowID
	            ) ee ON ee.RowID = e.RowID
	
	SET
	e.LeaveBalance = ee.`VLeaveBal`
	, e.SickLeaveBalance = ee.`SLeaveBal`
	, e.OtherLeaveBalance = ee.`OLeaveBal`
	, e.MaternityLeaveBalance = ee.`MLeaveBal`
	WHERE e.OrganizationID = og_rowid
	;
	*/
END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
