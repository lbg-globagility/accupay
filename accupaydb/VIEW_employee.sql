/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_employee`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employee`(IN `e_OrganizationID` INT, IN `pagination` INT)
    DETERMINISTIC
    COMMENT 'view all employee base on organization'
BEGIN

SELECT
e.RowID
,COALESCE(CAST(e.EmployeeID AS CHAR),'') 'Employee ID'
,COALESCE(e.FirstName,'') 'First Name'
,COALESCE(e.MiddleName,'') 'Middle Name'
,COALESCE(e.LastName,'') 'Last Name'
,COALESCE(e.Surname,'') 'Surname'
,COALESCE(e.Nickname,'') 'Nickname'
,COALESCE(e.MaritalStatus,'') 'Marital Status'
,COALESCE(e.NoOfDependents,0) 'No. Of Dependents'
,COALESCE(DATE_FORMAT(e.Birthdate,'%m/%d/%Y'),'') 'Birthdate'
,COALESCE(e.JobTitle,'') 'Job Title'
,COALESCE(pos.PositionName,'') 'Position'
,COALESCE(e.Salutation,'') 'Salutation'
,COALESCE(e.TINNo,'') 'TIN'
,COALESCE(e.SSSNo,'') 'SSS No.'
,COALESCE(e.HDMFNo,'') 'PAGIBIG No.'
,COALESCE(e.PhilHealthNo,'') 'PhilHealth No.'
,COALESCE(e.WorkPhone,'') 'Work Phone No.'
,COALESCE(e.HomePhone,'') 'Home Phone No.'
,COALESCE(e.MobilePhone,'') 'Mobile Phone No.'
,COALESCE(e.HomeAddress,'') 'Home address'
,COALESCE(e.EmailAddress,'') 'Email address'
,COALESCE(IF(e.Gender='M','Male','Female'),'') 'Gender'
,COALESCE(e.EmploymentStatus,'') 'Employment Status'
,COALESCE(pf.PayFrequencyType,'') 'Pay Frequency'
,COALESCE(e.UndertimeOverride,'') 'UndertimeOverride'
,COALESCE(e.OvertimeOverride,'') 'OvertimeOverride'
,DATE_FORMAT(e.Created,'%m/%d/%Y') 'Creation Date'
,CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2))) 'Created by',COALESCE(DATE_FORMAT(e.LastUpd,'%m/%d/%Y'),'') 'Last Update'
,(SELECT CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2)))  FROM user WHERE RowID=e.LastUpdBy) 'LastUpdate by'
,COALESCE(pos.RowID,'') 'PositionID'
,COALESCE(e.PayFrequencyID,'') 'PayFrequencyID'
,COALESCE(e.EmployeeType,'') 'EmployeeType'
,COALESCE(e.LeaveBalance,'') 'LeaveBalance'
,COALESCE(e.SickLeaveBalance,0) 'SickLeaveBalance'
,COALESCE(e.MaternityLeaveBalance,0) 'MaternityLeaveBalance'
,COALESCE(e.LeaveAllowance,'') 'LeaveAllowance'
,COALESCE(e.SickLeaveAllowance,0) 'SickLeaveAllowance'
,COALESCE(e.MaternityLeaveAllowance,0) 'MaternityLeaveAllowance'
,COALESCE(e.LeavePerPayPeriod,0) 'LeavePerPayPeriod'
,COALESCE(e.SickLeavePerPayPeriod,0) 'SickLeavePerPayPeriod'
,COALESCE(e.MaternityLeavePerPayPeriod,0) 'MaternityLeavePerPayPeriod'
,COALESCE(fstat.RowID,'') 'fstatRowID'
,COALESCE(e.Image,'') 'Image'
FROM employee e
LEFT JOIN user u ON e.CreatedBy=u.RowID
LEFT JOIN position pos ON e.PositionID=pos.RowID
LEFT JOIN payfrequency pf ON e.PayFrequencyID=pf.RowID
LEFT JOIN filingstatus fstat ON fstat.MaritalStatus=e.MaritalStatus AND fstat.Dependent=e.NoOfDependents
WHERE e.OrganizationID=e_OrganizationID
ORDER BY e.RowID DESC LIMIT pagination,100;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
