/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `UPD_leaveledger_newjoinedemployee`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `UPD_leaveledger_newjoinedemployee`(IN `og_rowid` INT, IN `param_date` DATE, IN `e_rowid` INT, IN `user_rowid` INT)
BEGIN

DECLARE leave_type TEXT DEFAULT 'Leave type';

DECLARE vl_text
        ,sl_text
        ,ml_text
        ,ol_text
        ,lwp_text TEXT;
   
	SET vl_text = 'Vacation leave';
	SET sl_text = 'Sick leave';
	SET ml_text = 'Maternity/paternity leave';
	SET ol_text = 'Others';
	SET lwp_text = 'Leave w/o Pay';
	
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
	, 'Debit'
	, ( lt.Balance + (lt.Amount * IF(lt.`Type`='Credit', 1, -1)) )
	, prdct.`LeaveHours`
	, p.PartNo
	
	FROM product p
	
	INNER JOIN (SELECT
					e.RowID
					, pp.`Year`
					, pp.RowID `PayPeriodID`
					, STR_TO_DATE(MAX(pyp.PayToDate), @@date_format) `MaxPayToDate`
					, (e.DateRegularized BETWEEN param_date AND MAX(pyp.PayToDate)) `IsDateRegularizedThisYear`
					
					, pp.OrdinalValue
					, pp.PayFromDate, pp.PayToDate
					
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
	
	INNER JOIN leavetransaction lt ON lt.RowID=ll.LastTransactionID
	
	INNER JOIN (SELECT p.*
					, em.EmployeeID
					, em.`PayPeriodID`
					, em.`LeaveHours`
					FROM product p
					INNER JOIN (
									SELECT et.RowID
									, et.EmployeeID
									, ppd.RowID `PayPeriodID`
									, SUM(et.VacationLeaveHours) `LeaveHours`
									FROM employeetimeentry et
									INNER JOIN (
									            SELECT e.*
													, pp.PayFromDate
													, MAX(ppd.PayToDate) `PayToDate`
													, PROPERCASE(CONCAT_WS(', ', e.LastName, e.FirstName)) `FullName`
													FROM employee e
													INNER JOIN payperiod pp ON pp.OrganizationID=e.OrganizationID AND pp.TotalGrossSalary =e.PayFrequencyID AND e.DateRegularized BETWEEN pp.PayFromDate AND pp.PayToDate AND pp.`Year`=YEAR(param_date)
													
													INNER JOIN payperiod ppd ON ppd.OrganizationID=pp.OrganizationID AND ppd.TotalGrossSalary=pp.TotalGrossSalary AND ppd.`Year`=pp.`Year`
													GROUP BY e.RowID
									            ) ee ON ee.RowID = et.EmployeeID AND ee.OrganizationID = et.OrganizationID AND et.`Date` BETWEEN ee.PayFromDate AND ee.`PayToDate`
									
									INNER JOIN payperiod ppd ON ppd.OrganizationID=et.OrganizationID AND ppd.TotalGrossSalary=ee.PayFrequencyID AND et.`Date` BETWEEN ppd.PayFromDate AND ppd.PayToDate
									
									# WHERE (et.VacationLeaveHours + et.SickLeaveHours + et.MaternityLeaveHours + et.OtherLeaveHours) > 0
									WHERE et.OrganizationID = og_rowid
									GROUP BY et.EmployeeID#, ppd.RowID
					            ) em ON em.EmployeeID > 0
					WHERE p.`Category` = leave_type
					AND p.OrganizationID = og_rowid
					AND p.PartNo = vl_text
				UNION
					SELECT p.*
					, em.EmployeeID
					, em.`PayPeriodID`
					, em.`LeaveHours`
					FROM product p
					INNER JOIN (
									SELECT et.RowID
									, et.EmployeeID
									, ppd.RowID `PayPeriodID`
									, SUM(et.SickLeaveHours) `LeaveHours`
									FROM employeetimeentry et
									INNER JOIN (
									            SELECT e.*
													, pp.PayFromDate
													, MAX(ppd.PayToDate) `PayToDate`
													, PROPERCASE(CONCAT_WS(', ', e.LastName, e.FirstName)) `FullName`
													FROM employee e
													INNER JOIN payperiod pp ON pp.OrganizationID=e.OrganizationID AND pp.TotalGrossSalary =e.PayFrequencyID AND e.DateRegularized BETWEEN pp.PayFromDate AND pp.PayToDate AND pp.`Year`=YEAR(param_date)
													
													INNER JOIN payperiod ppd ON ppd.OrganizationID=pp.OrganizationID AND ppd.TotalGrossSalary=pp.TotalGrossSalary AND ppd.`Year`=pp.`Year`
													GROUP BY e.RowID
									            ) ee ON ee.RowID = et.EmployeeID AND ee.OrganizationID = et.OrganizationID AND et.`Date` BETWEEN ee.PayFromDate AND ee.`PayToDate`
									
									INNER JOIN payperiod ppd ON ppd.OrganizationID=et.OrganizationID AND ppd.TotalGrossSalary=ee.PayFrequencyID AND et.`Date` BETWEEN ppd.PayFromDate AND ppd.PayToDate
									
									WHERE et.OrganizationID = og_rowid
									GROUP BY et.EmployeeID#, ppd.RowID
					            ) em ON em.EmployeeID > 0
					WHERE p.`Category` = leave_type
					AND p.OrganizationID = og_rowid
					AND p.PartNo = sl_text
				UNION
					SELECT p.*
					, em.EmployeeID
					, em.`PayPeriodID`
					, em.`LeaveHours`
					FROM product p
					INNER JOIN (
									SELECT et.RowID
									, et.EmployeeID
									, ppd.RowID `PayPeriodID`
									, SUM(et.MaternityLeaveHours) `LeaveHours`
									FROM employeetimeentry et
									INNER JOIN (
									            SELECT e.*
													, pp.PayFromDate
													, MAX(ppd.PayToDate) `PayToDate`
													, PROPERCASE(CONCAT_WS(', ', e.LastName, e.FirstName)) `FullName`
													FROM employee e
													INNER JOIN payperiod pp ON pp.OrganizationID=e.OrganizationID AND pp.TotalGrossSalary =e.PayFrequencyID AND e.DateRegularized BETWEEN pp.PayFromDate AND pp.PayToDate AND pp.`Year`=YEAR(param_date)
													
													INNER JOIN payperiod ppd ON ppd.OrganizationID=pp.OrganizationID AND ppd.TotalGrossSalary=pp.TotalGrossSalary AND ppd.`Year`=pp.`Year`
													GROUP BY e.RowID
									            ) ee ON ee.RowID = et.EmployeeID AND ee.OrganizationID = et.OrganizationID AND et.`Date` BETWEEN ee.PayFromDate AND ee.`PayToDate`
									
									INNER JOIN payperiod ppd ON ppd.OrganizationID=et.OrganizationID AND ppd.TotalGrossSalary=ee.PayFrequencyID AND et.`Date` BETWEEN ppd.PayFromDate AND ppd.PayToDate
									
									WHERE et.OrganizationID = og_rowid
									GROUP BY et.EmployeeID#, ppd.RowID
					            ) em ON em.EmployeeID > 0
					WHERE p.`Category` = leave_type
					AND p.OrganizationID = og_rowid
					AND p.PartNo = ml_text
				UNION
					SELECT p.*
					, em.EmployeeID
					, em.`PayPeriodID`
					, em.`LeaveHours`
					FROM product p
					INNER JOIN (
									SELECT et.RowID
									, et.EmployeeID
									, ppd.RowID `PayPeriodID`
									, SUM(et.OtherLeaveHours) `LeaveHours`
									FROM employeetimeentry et
									INNER JOIN (
									            SELECT e.*
													, pp.PayFromDate
													, MAX(ppd.PayToDate) `PayToDate`
													, PROPERCASE(CONCAT_WS(', ', e.LastName, e.FirstName)) `FullName`
													FROM employee e
													INNER JOIN payperiod pp ON pp.OrganizationID=e.OrganizationID AND pp.TotalGrossSalary =e.PayFrequencyID AND e.DateRegularized BETWEEN pp.PayFromDate AND pp.PayToDate AND pp.`Year`=YEAR(param_date)
													
													INNER JOIN payperiod ppd ON ppd.OrganizationID=pp.OrganizationID AND ppd.TotalGrossSalary=pp.TotalGrossSalary AND ppd.`Year`=pp.`Year`
													GROUP BY e.RowID
									            ) ee ON ee.RowID = et.EmployeeID AND ee.OrganizationID = et.OrganizationID AND et.`Date` BETWEEN ee.PayFromDate AND ee.`PayToDate`
									
									INNER JOIN payperiod ppd ON ppd.OrganizationID=et.OrganizationID AND ppd.TotalGrossSalary=ee.PayFrequencyID AND et.`Date` BETWEEN ppd.PayFromDate AND ppd.PayToDate
									
									WHERE et.OrganizationID = og_rowid
									GROUP BY et.EmployeeID#, ppd.RowID
					            ) em ON em.EmployeeID > 0
					WHERE p.`Category` = leave_type
					AND p.OrganizationID = og_rowid
					AND p.PartNo = ol_text
					
				/*UNION
					SELECT p.*
					FROM product p
					WHERE p.`Category` = leave_type
					AND p.OrganizationID = og_rowid
					AND p.PartNo = lwp_text*/
	           ) prdct ON ll.ProductID = prdct.RowID AND ll.EmployeeID = prdct.EmployeeID
	
	WHERE p.OrganizationID=og_rowid
	AND p.`Category`=leave_type
	AND  (( lt.Balance + (lt.Amount * IF(lt.`Type`='Credit', 1, -1)) ) != 0
	      AND prdct.`LeaveHours` != 0)
	;
	
	UPDATE leaveledger ll
	INNER JOIN leavetransaction lt
	        ON lt.Created = @curr_timestamp
			     AND lt.OrganizationID=ll.OrganizationID
			     AND lt.EmployeeID = ll.EmployeeID
			     AND lt.LeaveLedgerID = ll.RowID
	SET
	ll.LastTransactionID = lt.RowID
	, ll.LastUpd = CURRENT_TIMESTAMP()
	, ll.LastUpdBy = user_rowid
	WHERE ll.OrganizationID = og_rowid
	;
	
END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
