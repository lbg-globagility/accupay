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

-- Dumping structure for procedure goldwingspayrolldb.RPT_leave_ledger
DROP PROCEDURE IF EXISTS `RPT_leave_ledger`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_leave_ledger`(IN `OrganizID` INT, IN `paramDateFrom` DATE, IN `paramDateTo` DATE, IN `PayPeriodDateFromID` INT, IN `PayPeriodDateToID` INT)
    DETERMINISTIC
BEGIN

DECLARE payfreqID INT(11);

DECLARE dateloop DATE;

DECLARE indx INT(11) DEFAULT 1;

DECLARE cnt INT(11) DEFAULT 0;




DECLARE datetoOfDateFromID DATE;


DECLARE leavetypecount INT(11);

DECLARE leavetypetext VARCHAR(50);

DECLARE i_count INT(11) DEFAULT 0;

DECLARE emp_count INT(11) DEFAULT 0;

DECLARE emp_counts INT(11) DEFAULT 0;


SELECT PayFrequencyID FROM organization WHERE RowID=OrganizID INTO payfreqID;







IF payfreqID = 1 THEN
	
	
	
	
	SELECT COUNT_pay_period(paramDateFrom,paramDateTo) INTO cnt;
	
	
	
	

	IF (SELECT EXISTS(SELECT RowID FROM paystub WHERE PayPeriodID=PayPeriodDateToID AND OrganizationID=OrganizID LIMIT 1)) = 1 THEN
		
		SELECT 
		ee.EmployeeID
		,CONCAT(ee.LastName,',',ee.FirstName, IF(ee.MiddleName='','',','),INITIALS(ee.MiddleName,'. ','1')) AS Fullname
		,CONCAT(INITIALS(p.PartNo,'','1'),IF(LOCATE('Others',p.PartNo) > 0, 'L', '')) 'PartNo'
		,psi1.`EarnedHrs` AS EarnedHrs
		,FORMAT(psi1.`EarnedHrs` / 8,2) AS EarnedDays
		,IFNULL(FORMAT(SUM(IF(eelv.LeaveHrs >= 9, 8, eelv.LeaveHrs)),2),0.00) AS AvailHrs
		,IFNULL(FORMAT(SUM(IF(eelv.LeaveHrs >= 9, 8, eelv.LeaveHrs)),2) / 8,0.00) AS AvailDays
		,psi1.`EarnedHrs` - IFNULL(FORMAT(SUM(IF(eelv.LeaveHrs >= 9, 8, eelv.LeaveHrs)),2),0.00) 'BalHrs'
		,FORMAT((psi1.`EarnedHrs` - IFNULL(FORMAT(SUM(IF(eelv.LeaveHrs >= 9, 8, eelv.LeaveHrs)),2),0.00)) / 8,2) 'BalDays'
		FROM paystubitem psi
		LEFT JOIN product p ON p.RowID=psi.ProductID AND p.OrganizationID=psi.OrganizationID
		LEFT JOIN paystub ps ON ps.RowID=psi.PayStubID AND ps.OrganizationID=psi.OrganizationID
		LEFT JOIN employee ee ON ee.RowID=ps.EmployeeID AND ee.OrganizationID=psi.OrganizationID
		LEFT JOIN (SELECT RowID,EmployeeID,OrganizationID,LeaveStartDate,LeaveEndDate,LeaveType,((TIME_TO_SEC(TIMEDIFF(LeaveEndTime,LeaveStartTime)) / 60) / 60) AS LeaveHrs FROM employeeleave) AS eelv ON eelv.EmployeeID=ps.EmployeeID AND eelv.OrganizationID=ps.OrganizationID AND eelv.LeaveStartDate >= paramDateFrom AND eelv.LeaveEndDate <= paramDateTo AND eelv.LeaveType=p.PartNo
		LEFT JOIN (SELECT psii.RowID,IF(psii.PayAmount <= 0, IF(LOCATE('Vacation',pp.PartNo) > 0, e1.LeavePerPayPeriod * cnt, IF(LOCATE('Sick',pp.PartNo) > 0, e1.SickLeavePerPayPeriod * cnt, IF(LOCATE('aternity',pp.PartNo) > 0, e1.MaternityLeavePerPayPeriod * cnt, 0.00))), psii.PayAmount) AS EarnedHrs FROM paystubitem psii LEFT JOIN product pp ON pp.RowID=psii.ProductID AND pp.OrganizationID=psii.OrganizationID LEFT JOIN paystub pss ON pss.RowID=psii.PayStubID AND pss.OrganizationID=psii.OrganizationID LEFT JOIN employee e1 ON e1.RowID=pss.EmployeeID AND e1.OrganizationID=psii.OrganizationID WHERE pp.Category='Leave Type') AS psi1 ON psi1.RowID=psi.RowID
		WHERE psi.OrganizationID=OrganizID
		AND ee.EmploymentStatus != 'Terminated'
		AND p.Category='Leave Type'
		AND ps.PayToDate = paramDateTo
		GROUP BY psi.ProductID,ps.EmployeeID
		ORDER BY ee.LastName;
		
	ELSE
	
		SELECT MAX(PayToDate) FROM paystub WHERE OrganizationID=OrganizID AND PayPeriodID BETWEEN PayPeriodDateFromID AND PayPeriodDateToID INTO paramDateTo;
	
		SELECT 
		ee.EmployeeID
		,CONCAT(ee.LastName,',',ee.FirstName, IF(ee.MiddleName='','',','),INITIALS(ee.MiddleName,'. ','1')) AS Fullname
		,CONCAT(INITIALS(p.PartNo,'','1'),IF(LOCATE('Others',p.PartNo) > 0, 'L', '')) 'PartNo'
		,psi1.`EarnedHrs` AS EarnedHrs
		,FORMAT(psi1.`EarnedHrs` / 8,2) AS EarnedDays
		,IFNULL(FORMAT(SUM(IF(eelv.LeaveHrs >= 9, 8, eelv.LeaveHrs)),2),0.00) AS AvailHrs
		,IFNULL(FORMAT(SUM(IF(eelv.LeaveHrs >= 9, 8, eelv.LeaveHrs)),2) / 8,0.00) AS AvailDays
		,FORMAT((psi1.`EarnedHrs` - IFNULL(SUM(IF(eelv.LeaveHrs >= 9, 8, eelv.LeaveHrs)),0.00)),2) 'BalHrs'
		,FORMAT((psi1.`EarnedHrs` - IFNULL(FORMAT(SUM(IF(eelv.LeaveHrs >= 9, 8, eelv.LeaveHrs)),2),0.00)) / 8,2) 'BalDays'
		FROM paystubitem psi
		LEFT JOIN product p ON p.RowID=psi.ProductID AND p.OrganizationID=psi.OrganizationID
		LEFT JOIN paystub ps ON ps.RowID=psi.PayStubID AND ps.OrganizationID=psi.OrganizationID
		LEFT JOIN employee ee ON ee.RowID=ps.EmployeeID AND ee.OrganizationID=psi.OrganizationID
		LEFT JOIN (SELECT RowID,EmployeeID,OrganizationID,LeaveStartDate,LeaveEndDate,LeaveType,((TIME_TO_SEC(TIMEDIFF(LeaveEndTime,LeaveStartTime)) / 60) / 60) AS LeaveHrs FROM employeeleave) AS eelv ON eelv.EmployeeID=ps.EmployeeID AND eelv.OrganizationID=ps.OrganizationID AND eelv.LeaveStartDate >= paramDateFrom AND eelv.LeaveEndDate <= paramDateTo AND eelv.LeaveType=p.PartNo
		LEFT JOIN (SELECT psii.RowID,IF(LOCATE('Vacation',pp.PartNo) > 0, e1.LeavePerPayPeriod * cnt, IF(LOCATE('Sick',pp.PartNo) > 0, e1.SickLeavePerPayPeriod * cnt, IF(LOCATE('aternity',pp.PartNo) > 0, e1.MaternityLeavePerPayPeriod * cnt, 0.00))) AS EarnedHrs FROM paystubitem psii LEFT JOIN product pp ON pp.RowID=psii.ProductID AND pp.OrganizationID=psii.OrganizationID LEFT JOIN paystub pss ON pss.RowID=psii.PayStubID AND pss.OrganizationID=psii.OrganizationID LEFT JOIN employee e1 ON e1.RowID=pss.EmployeeID AND e1.OrganizationID=psii.OrganizationID WHERE pp.Category='Leave Type') AS psi1 ON psi1.RowID=psi.RowID
		WHERE psi.OrganizationID=OrganizID
		AND ee.EmploymentStatus != 'Terminated'
		AND p.Category='Leave Type'
		AND ps.PayToDate = paramDateTo
		GROUP BY psi.ProductID,ps.EmployeeID
		ORDER BY ee.LastName;
		
		
		
		END IF;
	
END IF;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
