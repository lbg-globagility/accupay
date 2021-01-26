/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_employee`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_employee`(
	`emplo_RowID` INT,
	`emplo_UserID` INT,
	`emplo_OrganizationID` INT,
	`emplo_Salutation` VARCHAR(50),
	`emplo_FirstName` VARCHAR(100),
	`emplo_MiddleName` VARCHAR(100),
	`emplo_LastName` VARCHAR(100),
	`emplo_Surname` VARCHAR(50),
	`emplo_EmployeeID` VARCHAR(50),
	`emplo_TINNo` VARCHAR(50),
	`emplo_SSSNo` VARCHAR(50),
	`emplo_HDMFNo` VARCHAR(50),
	`emplo_PhilHealthNo` VARCHAR(50),
	`emplo_EmploymentStatus` VARCHAR(50),
	`emplo_EmailAddress` VARCHAR(50),
	`emplo_WorkPhone` VARCHAR(50),
	`emplo_HomePhone` VARCHAR(50),
	`emplo_MobilePhone` VARCHAR(50),
	`emplo_HomeAddress` VARCHAR(1000),
	`emplo_Nickname` VARCHAR(50),
	`emplo_JobTitle` VARCHAR(50),
	`emplo_Gender` VARCHAR(50),
	`emplo_EmployeeType` VARCHAR(50),
	`emplo_MaritalStatus` VARCHAR(50),
	`emplo_Birthdate` DATE,
	`emplo_Startdate` DATE,
	`emplo_TerminationDate` DATE,
	`emplo_PositionID` INT,
	`emplo_Image` LONGBLOB,
	`emplo_LeaveBalance` DECIMAL(10,2),
	`emplo_SickLeaveBalance` DECIMAL(10,2),
	`emplo_MaternityLeaveBalance` DECIMAL(10,2),
	`emplo_OtherLeaveBalance` DECIMAL(10,2),
	`emplo_LeaveAllowance` DECIMAL(10,2),
	`emplo_SickLeaveAllowance` DECIMAL(10,2),
	`emplo_MaternityLeaveAllowance` DECIMAL(10,2),
	`emplo_OtherLeaveAllowance` DECIMAL(10,2),
	`emplo_AlphaListExempted` VARCHAR(50),
	`emplo_WorkDaysPerYear` DECIMAL(11,4),
	`emplo_DayOfRest` CHAR(1),
	`emplo_ATMNo` VARCHAR(50),
	`emplo_BankName` VARCHAR(50),
	`emplo_CalcHoliday` CHAR(1),
	`emplo_CalcSpecialHoliday` CHAR(1),
	`emplo_CalcNightDiff` CHAR(1),
	`emplo_CalcNightDiffOT` CHAR(1),
	`emplo_CalcRestDay` CHAR(1),
	`emplo_CalcRestDayOT` CHAR(1),
	`emplo_DateRegularized` DATE,
	`emplo_DateEvaluated` DATE,
	`emplo_RevealInPayroll` CHAR(1),
	`emplo_LateGracePeriod` DECIMAL(10,2),
	`emplo_AgencyID` INT,
	`emplo_BranchID` INT,
	`emplo_BPIInsurance` DECIMAL(10,2)
)
RETURNS int(11)
LANGUAGE SQL
DETERMINISTIC
CONTAINS SQL
SQL SECURITY DEFINER
COMMENT ''
BEGIN

DECLARE emploRowID INT(11);

INSERT INTO employee
(
    RowID
    ,CreatedBy
    ,LastUpdBy
    ,Created
    ,OrganizationID
    ,Salutation
    ,FirstName
    ,MiddleName
    ,LastName
    ,Surname
    ,EmployeeID
    ,TINNo
    ,SSSNo
    ,HDMFNo
    ,PhilHealthNo
    ,EmploymentStatus
    ,EmailAddress
    ,WorkPhone
    ,HomePhone
    ,MobilePhone
    ,HomeAddress
    ,Nickname
    ,JobTitle
    ,Gender
    ,EmployeeType
    ,MaritalStatus
    ,Birthdate
    ,StartDate
    ,TerminationDate
    ,PositionID
    ,PayFrequencyID
    ,NoOfDependents
    ,UndertimeOverride
    ,OvertimeOverride
    ,NewEmployeeFlag
    ,LeaveBalance
    ,SickLeaveBalance
    ,MaternityLeaveBalance
    ,OtherLeaveBalance
    ,LeaveAllowance
    ,SickLeaveAllowance
    ,MaternityLeaveAllowance
    ,OtherLeaveAllowance
    ,Image
    ,LeavePerPayPeriod
    ,SickLeavePerPayPeriod
    ,MaternityLeavePerPayPeriod
    ,OtherLeavePerPayPeriod
    ,AlphaListExempted
    ,WorkDaysPerYear
    ,DayOfRest
    ,ATMNo
    ,BankName
    ,CalcHoliday
    ,CalcSpecialHoliday
    ,CalcNightDiff
    ,CalcNightDiffOT
    ,CalcRestDay
    ,CalcRestDayOT
    ,DateRegularized
    ,DateEvaluated
    ,RevealInPayroll
    ,LateGracePeriod
    ,AgencyID
    ,BranchID
    ,BPIInsurance
) VALUES (
    emplo_RowID
    ,emplo_UserID
    ,emplo_UserID
    ,CURRENT_TIMESTAMP()
    ,emplo_OrganizationID
    ,emplo_Salutation
    ,emplo_FirstName
    ,emplo_MiddleName
    ,emplo_LastName
    ,emplo_Surname
    ,emplo_EmployeeID
    ,emplo_TINNo
    ,emplo_SSSNo
    ,emplo_HDMFNo
    ,emplo_PhilHealthNo
    ,emplo_EmploymentStatus
    ,emplo_EmailAddress
    ,emplo_WorkPhone
    ,emplo_HomePhone
    ,emplo_MobilePhone
    ,emplo_HomeAddress
    ,emplo_Nickname
    ,emplo_JobTitle
    ,emplo_Gender
    ,emplo_EmployeeType
    ,emplo_MaritalStatus
    ,emplo_Birthdate
    ,emplo_Startdate
    ,emplo_TerminationDate
    ,emplo_PositionID
    ,1 #PayFrequencySemiMonthlyId
    ,0
    ,FALSE
    ,FALSE
    ,'1'
    ,emplo_LeaveBalance
    ,emplo_SickLeaveBalance
    ,emplo_MaternityLeaveBalance
    ,emplo_OtherLeaveBalance
    ,emplo_LeaveAllowance
    ,emplo_SickLeaveAllowance
    ,emplo_MaternityLeaveAllowance
    ,emplo_OtherLeaveAllowance
    ,emplo_Image
    ,0
    ,0
    ,0
    ,0
    ,emplo_AlphaListExempted
    ,emplo_WorkDaysPerYear
    ,emplo_DayOfRest
    ,emplo_ATMNo
    ,emplo_BankName
    ,emplo_CalcHoliday
    ,emplo_CalcSpecialHoliday
    ,emplo_CalcNightDiff
    ,emplo_CalcNightDiffOT
    ,emplo_CalcRestDay
    ,emplo_CalcRestDayOT
    ,emplo_DateRegularized
    ,emplo_DateEvaluated
    ,emplo_RevealInPayroll
    ,emplo_LateGracePeriod
    ,emplo_AgencyID
    ,emplo_BranchID
    ,emplo_BPIInsurance
) ON
DUPLICATE
KEY
UPDATE
    LastUpdBy=emplo_UserID
    ,LastUpd=CURRENT_TIMESTAMP()
    ,Salutation=emplo_Salutation
    ,FirstName=emplo_FirstName
    ,MiddleName=emplo_MiddleName
    ,LastName=emplo_LastName
    ,Surname=emplo_Surname
    ,EmployeeID=emplo_EmployeeID
    ,TINNo=emplo_TINNo
    ,SSSNo=emplo_SSSNo
    ,HDMFNo=emplo_HDMFNo
    ,PhilHealthNo=emplo_PhilHealthNo
    ,EmploymentStatus=emplo_EmploymentStatus
    ,EmailAddress=emplo_EmailAddress
    ,WorkPhone=emplo_WorkPhone
    ,HomePhone=emplo_HomePhone
    ,MobilePhone=emplo_MobilePhone
    ,HomeAddress=emplo_HomeAddress
    ,Nickname=emplo_Nickname
    ,JobTitle=emplo_JobTitle
    ,Gender=emplo_Gender
    ,EmployeeType=emplo_EmployeeType
    ,MaritalStatus=emplo_MaritalStatus
    ,Birthdate=emplo_Birthdate
    ,StartDate=emplo_Startdate
    ,PositionID=emplo_PositionID
    ,LeaveAllowance=emplo_LeaveAllowance
    ,SickLeaveAllowance=emplo_SickLeaveAllowance
    ,MaternityLeaveAllowance=emplo_MaternityLeaveAllowance
    ,OtherLeaveAllowance=emplo_OtherLeaveAllowance
    ,Image=emplo_Image
    ,AlphaListExempted=emplo_AlphaListExempted
    ,WorkDaysPerYear=emplo_WorkDaysPerYear
    ,DayOfRest=emplo_DayOfRest
    ,ATMNo=emplo_ATMNo
    ,BankName=emplo_BankName
    ,CalcHoliday=emplo_CalcHoliday
    ,CalcSpecialHoliday=emplo_CalcSpecialHoliday
    ,CalcNightDiff=emplo_CalcNightDiff
    ,CalcNightDiffOT=emplo_CalcNightDiffOT
    ,CalcRestDay=emplo_CalcRestDay
    ,CalcRestDayOT=emplo_CalcRestDayOT
    ,DateRegularized=emplo_DateRegularized
    ,DateEvaluated=emplo_DateEvaluated
    ,RevealInPayroll=emplo_RevealInPayroll
    ,LateGracePeriod=emplo_LateGracePeriod
    ,AgencyID=emplo_AgencyID
    ,MaternityLeaveBalance=emplo_MaternityLeaveBalance
    ,OtherLeaveBalance=emplo_OtherLeaveBalance
	,BranchID=emplo_BranchID
	,BPIInsurance=emplo_BPIInsurance;SELECT @@Identity AS id INTO emploRowID;

RETURN emploRowID;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
