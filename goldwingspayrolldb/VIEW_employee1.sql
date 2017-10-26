/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_employee1`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employee1`(IN `e_OrganizationID` INT, IN `pagination` INT, IN `SearchString` TEXT)
    COMMENT 'view all employee base on organization'
BEGIN

IF SearchString = '' THEN

	SELECT 
	e.RowID
	,IFNULL(e.EmployeeID,'') 'Employee ID'
	,IFNULL(e.FirstName,'') 'First Name'
	,IFNULL(e.MiddleName,'') 'Middle Name'
	,IFNULL(e.LastName,'') 'Last Name'
	,IFNULL(IF(e.Gender='M','Male','Female'),'') 'Gender'
	,IFNULL(e.EmploymentStatus,'') 'Employment Status'
	,IFNULL(pf.PayFrequencyType,'') 'Pay Frequency'
	,IFNULL(pos.PositionName,'') 'Position'
	,IFNULL(pos.RowID,'') 'PositionID'
	,IFNULL(e.PayFrequencyID,'') 'PayFrequencyID'
	,IFNULL(e.EmployeeType,'') 'Employee Type' 
	,IFNULL(e.OffsetBalance,'0.0') 'Offset Balance'
	,IF(IFNULL(e.MiddleName,'')='', CONCAT(e.LastName,', ',e.FirstName), CONCAT(e.LastName,', ',e.FirstName,', ',INITIALS(e.MiddleName,'.','1'))) 'Employee Fullname'
	,CONCAT('ID# ', e.EmployeeID, ', ', e.EmployeeType, ' salary') 'Details'
	,IFNULL(e.Image,'') 'Image' 
	FROM employee e 
	LEFT JOIN user u ON e.CreatedBy=u.RowID 
	LEFT JOIN position pos ON e.PositionID=pos.RowID 
	LEFT JOIN payfrequency pf ON e.PayFrequencyID=pf.RowID 
	LEFT JOIN filingstatus fstat ON fstat.MaritalStatus=e.MaritalStatus AND fstat.Dependent=e.NoOfDependents
	WHERE e.OrganizationID=e_OrganizationID
	ORDER BY e.RowID DESC LIMIT pagination,20;

ELSE

	SELECT 
	e.RowID
	,IFNULL(e.EmployeeID,'') 'Employee ID'
	,IFNULL(e.FirstName,'') 'First Name'
	,IFNULL(e.MiddleName,'') 'Middle Name'
	,IFNULL(e.LastName,'') 'Last Name'
	,IFNULL(IF(e.Gender='M','Male','Female'),'') 'Gender'
	,IFNULL(e.EmploymentStatus,'') 'Employment Status'
	,IFNULL(pf.PayFrequencyType,'') 'Pay Frequency'
	,IFNULL(pos.PositionName,'') 'Position'
	,IFNULL(pos.RowID,'') 'PositionID'
	,IFNULL(e.PayFrequencyID,'') 'PayFrequencyID'
	,IFNULL(e.EmployeeType,'') 'Employee Type' 
	,IFNULL(e.OffsetBalance,'0.0') 'Offset Balance'
	,IF(IFNULL(e.MiddleName,'')='', CONCAT(e.LastName,', ',e.FirstName), CONCAT(e.LastName,', ',e.FirstName,', ',INITIALS(e.MiddleName,'.','1'))) 'Employee Fullname'
	,CONCAT('ID# ', e.EmployeeID, ', ', e.EmployeeType, ' salary') 'Details'
	,IFNULL(e.Image,'') 'Image' 
	FROM employee e 
	LEFT JOIN user u ON e.CreatedBy=u.RowID 
	LEFT JOIN position pos ON e.PositionID=pos.RowID 
	LEFT JOIN payfrequency pf ON e.PayFrequencyID=pf.RowID 
	LEFT JOIN filingstatus fstat ON fstat.MaritalStatus=e.MaritalStatus AND fstat.Dependent=e.NoOfDependents
	WHERE e.OrganizationID=e_OrganizationID
	AND (e.EmployeeID=SearchString OR e.FirstName=SearchString OR e.LastName=SearchString)
	ORDER BY e.RowID DESC LIMIT pagination,20;

END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
