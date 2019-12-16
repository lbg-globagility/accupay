/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `PRINT_employee_profile`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `PRINT_employee_profile`(IN `OrganizID` INT, IN `EmpRowID` INT)
BEGIN

SELECT
e.RowID AS Col1
,og.Name AS Col2
,e.Salutation AS Col3
,e.FirstName AS Col4
,e.MiddleName AS Col5
,e.LastName AS Col6
,e.Surname AS Col7
,e.EmployeeID AS Col8
,e.TINNo AS Col9
,e.SSSNo AS Col10
,e.HDMFNo AS Col11
,e.PhilHealthNo AS Col12
,e.EmploymentStatus AS Col13
,e.EmailAddress AS Col14
,e.WorkPhone AS Col15
,e.HomePhone AS Col16
,e.MobilePhone AS Col17
,e.HomeAddress AS Col18
,e.Nickname AS Col19
,e.JobTitle AS Col20
,IF(e.Gender = 'M','Male','Female') AS Col21
,e.EmployeeType AS Col22
,e.MaritalStatus AS Col23
,DATE_FORMAT(e.Birthdate,'%c/%e/%Y') AS Col24
,DATE_FORMAT(e.StartDate,'%c/%e/%Y') AS Col25
,IF(e.TerminationDate IS NULL,'',DATE_FORMAT(e.TerminationDate,'%c/%e/%Y')) AS Col26
,IFNULL(pos.PositionName,'') AS Col27
,pf.PayFrequencyType AS Col28
,e.NoOfDependents AS Col29
,e.UndertimeOverride AS Col30
,e.OvertimeOverride AS Col31
,e.NewEmployeeFlag AS Col32
,e.LeaveBalance AS Col33
,e.SickLeaveBalance AS Col34
,e.MaternityLeaveBalance AS Col35
,e.OtherLeaveBalance AS Col36
,e.LeaveAllowance AS Col37
,e.SickLeaveAllowance AS Col38
,e.MaternityLeaveAllowance AS Col39
,e.OtherLeaveAllowance AS Col40
,e.LeavePerPayPeriod AS Col41
,e.SickLeavePerPayPeriod AS Col42
,e.MaternityLeavePerPayPeriod AS Col43
,e.OtherLeavePerPayPeriod AS Col44
,e.AlphaListExempted AS Col45
,e.WorkDaysPerYear AS Col46
,e.DayOfRest AS Col47
,e.ATMNo AS Col48
,e.BankName AS Col49
,e.CalcHoliday AS Col50
,e.CalcSpecialHoliday AS Col51
,e.CalcNightDiff AS Col52
,e.CalcNightDiffOT AS Col53
,e.CalcRestDay AS Col54
,e.CalcRestDayOT AS Col55
,IF(e.DateRegularized IS NULL,'',DATE_FORMAT(e.DateRegularized,'%c/%e/%Y')) AS Col56
,IF(e.DateEvaluated IS NULL,'',DATE_FORMAT(e.DateEvaluated,'%c/%e/%Y')) AS Col57
,CONCAT(IFNULL(edp.LastName,''),IFNULL(CONCAT(',',edp.FirstName),''),IFNULL(CONCAT(',',edp.MiddleName),'')) AS Col58
,IFNULL(edp.MiddleName,'') AS Col59
,IFNULL(edp.LastName,'') AS Col60
,IFNULL(edp.Surname,'') AS Col61
,IFNULL(edp.TINNo,'') AS Col62
,IFNULL(edp.SSSNo,'') AS Col63
,IFNULL(edp.HDMFNo,'') AS Col64
,IFNULL(edp.PhilHealthNo,'') AS Col65
,IFNULL(edp.EmailAddress,'') AS Col66
,IFNULL(edp.WorkPhone,'') AS Col67
,IFNULL(edp.HomePhone,'') AS Col68
,IFNULL(edp.MobilePhone,'') AS Col69
,IFNULL(edp.HomeAddress,'') AS Col70
,IFNULL(edp.Nickname,'') AS Col71
,IFNULL(edp.JobTitle,'') AS Col72
,edp.Gender AS Col73
,IFNULL(edp.RelationToEmployee,'') AS Col74
,IF(edp.ActiveFlag = 'Y','Yes','No') AS Col75
,IF(edp.Birthdate IS NULL,'',DATE_FORMAT(edp.Birthdate,'%c/%e/%Y')) AS Col76
,IFNULL(e.ATMNo,'') AS COL119
FROM employee e
INNER JOIN organization og ON og.RowID=e.OrganizationID
INNER JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID
LEFT JOIN `position` pos ON pos.RowID=e.PositionID
LEFT JOIN (SELECT * FROM employeedependents WHERE ParentEmployeeID=EmpRowID AND OrganizationID=OrganizID) edp ON edp.RowID > 0
WHERE e.RowID=EmpRowID
AND e.OrganizationID=OrganizID;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
