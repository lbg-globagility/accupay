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

-- Dumping structure for procedure goldwingspayrolldb.SEARCH_employee_paystub
DROP PROCEDURE IF EXISTS `SEARCH_employee_paystub`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `SEARCH_employee_paystub`(IN `og_rowid` INT, IN `unified_search_string` VARCHAR(50), IN `page_number` INT)
    DETERMINISTIC
BEGIN

DECLARE max_count_per_page INT(11) DEFAULT 20;

SELECT e.RowID
		,e.EmployeeID
		,e.FirstName
		,e.MiddleName
		,e.LastName
		,e.Surname
		,e.Nickname
		,e.MaritalStatus
		,e.NoOfDependents
		,e.Birthdate
		,e.StartDate
		,e.JobTitle
		,pos.PositionName
		,e.Salutation
		,e.TINNo
		,e.SSSNo
		,e.HDMFNo
		,e.PhilHealthNo
		,e.WorkPhone
		,e.HomePhone
		,e.MobilePhone
		,e.HomeAddress
		,e.EmailAddress
		,e.Gender
		,e.EmploymentStatus
		
		,pf.PayFrequencyType
		,e.UndertimeOverride
		,e.OvertimeOverride
		,e.PositionID
		,e.PayFrequencyID
		,e.EmployeeType
		,e.LeaveBalance
		,e.SickLeaveBalance
		,e.MaternityLeaveBalance
		,e.LeaveAllowance
		,e.SickLeaveAllowance
		,e.MaternityLeaveAllowance
		
		,fstat.`FilingStatus`
		,''									`Nothing`
		,e.Created
		,e.CreatedBy
		,e.LastUpd
		,e.LastUpdBy
		
FROM (SELECT * FROM employee WHERE OrganizationID=og_rowid AND EmployeeID	=unified_search_string	AND LENGTH(unified_search_string) > 0
	UNION
		SELECT * FROM employee WHERE OrganizationID=og_rowid AND LastName		=unified_search_string	AND LENGTH(unified_search_string) > 0
	UNION
		SELECT * FROM employee WHERE OrganizationID=og_rowid AND FirstName	=unified_search_string	AND LENGTH(unified_search_string) > 0
	UNION
		SELECT * FROM employee WHERE OrganizationID=og_rowid AND LENGTH(TRIM(unified_search_string))=0
		) e
		
LEFT JOIN `user` u				ON e.CreatedBy=u.RowID
LEFT JOIN `user` uu				ON e.LastUpdBy=uu.RowID
LEFT JOIN `position` pos		ON e.PositionID=pos.RowID
LEFT JOIN payfrequency pf		ON e.PayFrequencyID=pf.RowID
LEFT JOIN filingstatus fstat	ON fstat.MaritalStatus=e.MaritalStatus AND fstat.Dependent=e.NoOfDependents
LEFT JOIN agency ag				ON ag.RowID=e.AgencyID
LEFT JOIN division d				ON d.RowID=pos.DivisionId

ORDER BY e.LastName ASC, e.FirstName ASC
LIMIT page_number, max_count_per_page;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
