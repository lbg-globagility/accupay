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

-- Dumping structure for function goldwingspayrolldb.INSUPD_employee_01
DROP FUNCTION IF EXISTS `INSUPD_employee_01`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_employee_01`(`RID` INT, `UserRowID` INT, `OrganizID` INT, `Salutat` VARCHAR(50), `FName` VARCHAR(50), `MName` VARCHAR(50), `LName` VARCHAR(50), `Surname` VARCHAR(50), `EmpID` VARCHAR(50), `TIN` VARCHAR(50), `SSS` VARCHAR(50), `HDMF` VARCHAR(50), `PhH` VARCHAR(50), `EmpStatus` VARCHAR(50), `EmailAdd` VARCHAR(50), `WorkNo` VARCHAR(50), `HomeNo` VARCHAR(50), `MobileNo` VARCHAR(50), `HAddress` VARCHAR(2000), `Nick` VARCHAR(50), `JTitle` VARCHAR(50), `Gend` VARCHAR(50), `EmpType` VARCHAR(50), `MaritStat` VARCHAR(50), `BDate` DATE, `Start_Date` DATE, `TerminatDate` DATE, `PositID` INT, `PayFreqID` INT, `NumDependent` INT, `UTOverride` VARCHAR(50), `OTOverride` VARCHAR(50), `NewEmpFlag` VARCHAR(50), `LeaveBal` DECIMAL(10,2), `SickBal` DECIMAL(10,2), `MaternBal` DECIMAL(10,2), `LeaveAllow` DECIMAL(10,2), `SickAllow` DECIMAL(10,2), `MaternAllow` DECIMAL(10,2), `Imag` MEDIUMBLOB, `LeavePayPer` DECIMAL(10,2), `SickPayPer` DECIMAL(10,2), `MaternPayPer` DECIMAL(10,2), `IsExemptAlphaList` TEXT, `Work_DaysPerYear` INT, `Day_Rest` CHAR(1), `ATM_No` VARCHAR(50), `OtherLeavePayPer` DECIMAL(10,2), `Bank_Name` VARCHAR(50), `Calc_Holiday` CHAR(1), `Calc_SpecialHoliday` CHAR(1), `Calc_NightDiff` CHAR(1), `Calc_NightDiffOT` CHAR(1), `Calc_RestDay` CHAR(1), `Calc_RestDayOT` CHAR(1), `PositionTextName` VARCHAR(50)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnval INT(11) DEFAULT 0;

DECLARE termin_date DATE;

DECLARE number_dependnt INT(11);

DECLARE exist_empRowID INT(11);

DECLARE division_parentID INT(11);

SELECT RowID FROM employee WHERE EmployeeID=EmpID AND OrganizationID=OrganizID INTO exist_empRowID;

SELECT TerminationDate FROM Employee WHERE RowID=RID INTO termin_date;

SELECT COUNT(edep.RowID) FROM employeedependents edep LEFT JOIN employee e ON e.RowID=edep.ParentEmployeeID AND e.OrganizationID=edep.OrganizationID WHERE e.RowID=RID AND edep.OrganizationID=OrganizID INTO number_dependnt;

SET PositID = IFNULL(PositID,0);

IF EmpID != '' THEN

	SELECT pt.DivisionId FROM position pt WHERE pt.PositionName=PositionTextName AND pt.OrganizationID=OrganizID LIMIT 1 INTO division_parentID;
	
	IF division_parentID IS NULL THEN
		SELECT RowID FROM `division` WHERE OrganizationID=OrganizID LIMIT 1 INTO division_parentID;
	END IF;
	
	INSERT INTO employee
	(
		RowID
		,CreatedBy
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
		,LeaveAllowance
		,SickLeaveAllowance
		,MaternityLeaveAllowance
		,Image
		,LeavePerPayPeriod
		,SickLeavePerPayPeriod
		,MaternityLeavePerPayPeriod
		,AlphaListExempted
		,DayOfRest
		,ATMNo
		,OtherLeavePerPayPeriod
		,OtherLeaveAllowance
		,BankName
		,CalcHoliday	
		,CalcSpecialHoliday
		,CalcNightDiff
		,CalcNightDiffOT
		,CalcRestDay
		,CalcRestDayOT
	) VALUES (
		IFNULL(RID,exist_empRowID)
		,UserRowID
		,CURRENT_TIMESTAMP()
		,OrganizID
		,Salutat
		,FName
		,MName
		,LName
		,Surname
		,EmpID
		,TIN
		,SSS
		,HDMF
		,PhH
		,EmpStatus
		,EmailAdd
		,WorkNo
		,HomeNo
		,MobileNo
		,SUBSTRING(HAddress,1,1000)
		,Nick
		,JTitle
		,Gend
		,EmpType
		,MaritStat
		,BDate
		,Start_Date
		,termin_date
		,INSUPD_position(NULL,PositionTextName,UserRowID,OrganizID,UserRowID,NULL,division_parentID)
		,PayFreqID
		,number_dependnt
		,UTOverride
		,OTOverride
		,IF(EmpStatus = 'Probationary', '1', '0')
		,LeaveBal
		,SickBal
		,MaternBal
		,LeaveAllow
		,SickAllow
		,MaternAllow
		,Imag
		,LeavePayPer
		,SickPayPer
		,MaternPayPer
		,IsExemptAlphaList
		,Day_Rest
		,ATM_No
		,OtherLeavePayPer
		,OtherLeavePayPer * COUNT_payperiodthisyear(OrganizID,PayFreqID)
		,Bank_Name
		,Calc_Holiday
		,Calc_SpecialHoliday
		,Calc_NightDiff
		,Calc_NightDiffOT
		,Calc_RestDay
		,Calc_RestDayOT
	) ON
	DUPLICATE
	KEY
	UPDATE
		LastUpdBy=UserRowID
		,LastUpd=CURRENT_TIMESTAMP()
		,OrganizationID=OrganizID
		,Salutation=Salutat
		,FirstName=FName
		,MiddleName=MName
		,LastName=LName
		,Surname=Surname
		,EmployeeID=EmpID
		,TINNo=TIN
		,SSSNo=SSS
		,HDMFNo=HDMF
		,PhilHealthNo=PhH
		,EmploymentStatus=EmpStatus
		,EmailAddress=EmailAdd
		,WorkPhone=WorkNo
		,HomePhone=HomeNo
		,MobilePhone=MobileNo
		,HomeAddress=HAddress
		,Nickname=Nick
		,JobTitle=JTitle
		,Gender=Gend
		,EmployeeType=EmpType
		,MaritalStatus=MaritStat
		,Birthdate=BDate
		,StartDate=Start_Date
		,TerminationDate=termin_date
		,PositionID=INSUPD_position(NULL,PositionTextName,UserRowID,OrganizID,UserRowID,NULL,division_parentID)
		,PayFrequencyID=PayFreqID
		,NoOfDependents=number_dependnt
		,UndertimeOverride=UTOverride
		,OvertimeOverride=OTOverride
		,NewEmployeeFlag=NewEmpFlag
		,LeaveBalance=LeaveBal
		,SickLeaveBalance=SickBal
		,MaternityLeaveBalance=MaternBal
		,LeaveAllowance=LeaveAllow
		,SickLeaveAllowance=SickAllow
		,MaternityLeaveAllowance=MaternAllow
		,Image=Imag
		,LeavePerPayPeriod=LeavePayPer
		,SickLeavePerPayPeriod=SickPayPer
		,MaternityLeavePerPayPeriod=MaternPayPer
		,AlphaListExempted=IsExemptAlphaList
		,WorkDaysPerYear=Work_DaysPerYear
		,DayOfRest=Day_Rest
		,ATMNo=ATM_No
		,OtherLeavePerPayPeriod=OtherLeavePayPer
		,OtherLeaveAllowance=OtherLeavePayPer * COUNT_payperiodthisyear(OrganizID,PayFreqID)
		,BankName=Bank_Name
		,CalcHoliday=Calc_Holiday
		,CalcSpecialHoliday=Calc_SpecialHoliday
		,CalcNightDiff=Calc_NightDiff
		,CalcNightDiffOT=Calc_NightDiffOT
		,CalcRestDay=Calc_RestDay
		,CalcRestDayOT=Calc_RestDayOT;SELECT @@Identity AS id INTO returnval;
		
END IF;

RETURN returnval;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
