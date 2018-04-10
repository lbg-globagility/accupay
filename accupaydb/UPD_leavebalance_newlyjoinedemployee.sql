-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.12-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for procedure accupaydb.UPD_leavebalance_newlyjoinedemployee
DROP PROCEDURE IF EXISTS `UPD_leavebalance_newlyjoinedemployee`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `UPD_leavebalance_newlyjoinedemployee`(IN `og_rowid` INT, IN `param_date` DATE, IN `e_rowid` INT)
BEGIN

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
					GROUP BY e.RowID
	        /**/) ee ON ee.RowID = e.RowID #AND ee.`IsDateRegularizedThisYear` = TRUE
	
	SET
	e.LeaveBalance = ee.`VLeaveBal`
	, e.SickLeaveBalance = ee.`SLeaveBal`
	, e.OtherLeaveBalance = ee.`OLeaveBal`
	, e.MaternityLeaveBalance = ee.`MLeaveBal`
	WHERE e.OrganizationID = og_rowid
	;
	
	# ###################################################################################
	/*
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
					
					, (@_count := COUNT(pyp.RowID)) `CountRecord`
					
					, e.LeaveAllowance `VLeaveBal`
					, e.SickLeaveAllowance `SLeaveBal`
					, e.OtherLeaveAllowance `OLeaveBal`
					, e.MaternityLeaveAllowance `MLeaveBal`
					
					FROM employee e
					INNER JOIN payperiod pp ON pp.TotalGrossSalary = e.PayFrequencyID AND pp.OrganizationID = e.OrganizationID AND e.DateRegularized BETWEEN pp.PayFromDate AND pp.PayToDate
					
					# INNER JOIN payperiod ppd ON ppd.TotalGrossSalary = e.PayFrequencyID AND ppd.OrganizationID = e.OrganizationID AND param_date BETWEEN ppd.PayFromDate AND ppd.PayToDate
					
					INNER JOIN payperiod pyp ON pyp.OrganizationID = pp.OrganizationID AND pyp.TotalGrossSalary = pp.TotalGrossSalary AND pyp.`Year` = pp.`Year`
					
					WHERE e.DateRegularized IS NOT NULL
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
					GROUP BY e.RowID
	            ) ee ON ee.RowID = e.RowID #AND ee.`IsDateRegularizedThisYear` = TRUE
	
	SET
	e.LeaveBalance = ee.`VLeaveBal`
	, e.SickLeaveBalance = ee.`SLeaveBal`
	, e.OtherLeaveBalance = ee.`OLeaveBal`
	, e.MaternityLeaveBalance = ee.`MLeaveBal`
	WHERE e.OrganizationID = og_rowid
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
