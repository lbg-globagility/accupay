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

-- Dumping structure for procedure goldwingspayrolldb.RESET_employeeleave_duplicate
DROP PROCEDURE IF EXISTS `RESET_employeeleave_duplicate`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `RESET_employeeleave_duplicate`(IN `OrganizID` INT, IN `FromPayDate` DATE, IN `ToPayDate` DATE, IN `Pay_FrequencyType` VARCHAR(50), IN `DivisionRowID` VARCHAR(200))
    DETERMINISTIC
BEGIN

DECLARE atleast_one CHAR(1);

SELECT EXISTS(SELECT RowID FROM employeeleave WHERE OrganizationID=OrganizID AND (LeaveStartDate >= FromPayDate OR LeaveEndDate >= FromPayDate) AND (LeaveStartDate <= ToPayDate OR LeaveEndDate <= ToPayDate) LIMIT 1) INTO atleast_one;

IF atleast_one = '1' THEN

	UPDATE employeetimeentry et
	INNER JOIN payfrequency pf ON pf.PayFrequencyType=Pay_FrequencyType
	
	INNER JOIN employee e ON e.OrganizationID=et.OrganizationID AND e.PayFrequencyID=pf.RowID 
	SET
		et.RegularHoursWorked=0
		,et.RegularHoursAmount=0
		,et.TotalHoursWorked=0
		,et.OvertimeHoursWorked=0
		,et.OvertimeHoursAmount=0
		,et.UndertimeHours=0
		,et.UndertimeHoursAmount=0
		,et.NightDifferentialHours=0
		,et.NightDiffHoursAmount=0
		,et.NightDifferentialOTHours=0
		,et.NightDiffOTHoursAmount=0
		,et.HoursLate=0
		,et.HoursLateAmount=0
		,et.VacationLeaveHours=0
		,et.SickLeaveHours=0
		,et.MaternityLeaveHours=0
		,et.OtherLeaveHours=0
		,et.TotalDayPay=0
		,et.Absent=0
	WHERE et.OrganizationID=OrganizID
	AND et.EmployeeID=e.RowID
	AND et.`Date` BETWEEN FromPayDate AND ToPayDate;
	
	DELETE FROM employeeleave_duplicate WHERE OrganizationID=OrganizID AND (LeaveStartDate >= FromPayDate OR LeaveEndDate >= FromPayDate) AND (LeaveStartDate <= ToPayDate OR LeaveEndDate <= ToPayDate);ALTER TABLE employeeleave_duplicate AUTO_INCREMENT = 0;
	
	INSERT INTO employeeleave_duplicate
	(
		RowID
		,OrganizationID
		,Created
		,LeaveStartTime
		,LeaveType
		,CreatedBy
		,LastUpd
		,LastUpdBy
		,EmployeeID
		,LeaveEndTime
		,LeaveStartDate
		,LeaveEndDate
		,Reason
		,Comments
		,Image
		,`Status`
	) SELECT elv.RowID
		,elv.OrganizationID
		,elv.Created
		,elv.LeaveStartTime
		,elv.LeaveType
		,elv.CreatedBy
		,elv.LastUpd
		,elv.LastUpdBy
		,elv.EmployeeID
		,elv.LeaveEndTime
		,elv.LeaveStartDate
		,elv.LeaveEndDate
		,elv.Reason
		,elv.Comments
		,elv.Image
		,elv.`Status`
	FROM employeeleave elv
	WHERE elv.OrganizationID=OrganizID
	AND (elv.LeaveStartDate >= FromPayDate OR elv.LeaveEndDate >= FromPayDate)
	AND (elv.LeaveStartDate <= ToPayDate OR elv.LeaveEndDate <= ToPayDate);

END IF;
	
END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
