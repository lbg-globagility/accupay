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

-- Dumping structure for function goldwingspayrolldb.INSUPD_employeesalary
DROP FUNCTION IF EXISTS `INSUPD_employeesalary`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_employeesalary`(`esal_RowID` INT, `esal_EmployeeID` INT, `esal_CreatedBy` INT, `esal_LastUpdBy` INT, `esal_OrganizationID` INT, `esal_BasicPay` DECIMAL(11,6), `esal_Salary` DECIMAL(11,6), `esal_NoofDependents` INT, `esal_MaritalStatus` VARCHAR(50), `esal_PositionID` INT, `esal_EffectiveDateFrom` DATE, `esal_EffectiveDateTo` DATE, `esal_HDMFAmount` DECIMAL(10,2), `esal_TrueSalary` DECIMAL(10,2), `esal_IsDoneByImporting` TEXT, `esal_DiscardSSS` TINYINT, `esal_DiscardPhH` TINYINT) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE esalID INT(11);

DECLARE employeemaritstat VARCHAR(50);

DECLARE hdmf_amt DECIMAL(11,6);

DECLARE esal_BasicDailyPay DECIMAL(11,6);

DECLARE esal_BasicHourlyPay DECIMAL(11,6);

DECLARE EmpType VARCHAR(50);

DECLARE EmpPayFreqID INT(11);

DECLARE EmpFStatID INT(11);

DECLARE sss_amt DECIMAL(11,6);

DECLARE phh_amt DECIMAL(11,6);

DECLARE prevesalRowID INT(11) DEFAULT NULL;

DECLARE prevdatefrom DATE;

DECLARE existRecordRowID INT(11);

DECLARE viewID INT(11);


DECLARE recordexists INT(11);


DECLARE preEffDateFromEmpSal DATE DEFAULT CURRENT_DATE();

DECLARE preEffDateFromEmpSallatest DATE;

DECLARE preEffDateToEmpSallatest DATE DEFAULT CURRENT_DATE();

DECLARE salaryToUseForContrib DECIMAL(11,2) DEFAULT 0;

DECLARE org_workingdays INT(11);

DECLARE emp_countsalaries INT(11) DEFAULT -1;

DECLARE isEmpStatusContractual CHAR(1);

DECLARE nullcounteffectivedateto INT(11) DEFAULT 0;

DECLARE pay_freq_type VARCHAR(50);


SELECT (LCASE(e.EmploymentStatus) = 'contractual'),pf.PayFrequencyType FROM employee e INNER JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID WHERE e.RowID=esal_EmployeeID INTO isEmpStatusContractual, pay_freq_type;

IF isEmpStatusContractual = '0' THEN

	SELECT NULL INTO esal_EffectiveDateTo;
	
END IF;


SELECT RowID FROM `view` WHERE ViewName='Employee Salary' AND OrganizationID=esal_OrganizationID LIMIT 1 INTO viewID;

SELECT WorkDaysPerYear FROM organization WHERE RowID=esal_OrganizationID INTO org_workingdays;


IF esal_IsDoneByImporting = '1' THEN
	SET esal_IsDoneByImporting = '1';
END IF;
	

SELECT COUNT(RowID) FROM employeesalary WHERE EmployeeID=esal_EmployeeID AND OrganizationID=esal_OrganizationID INTO emp_countsalaries;


	
	
	
	SELECT RowID,EffectiveDateFrom FROM employeesalary WHERE EmployeeID=esal_EmployeeID AND OrganizationID=esal_OrganizationID AND EffectiveDateTo IS NULL ORDER BY DATEDIFF(DATE_FORMAT(NOW(),'%Y-%m-%d'),EffectiveDateFrom) LIMIT 1 INTO prevesalRowID,prevdatefrom;
	
	
	
	SET employeemaritstat = IF(esal_MaritalStatus IN ('Single','Married'),esal_MaritalStatus,'Zero');
	
	SELECT EmployeeType,PayFrequencyID FROM employee WHERE RowID=esal_EmployeeID AND OrganizationID=esal_OrganizationID INTO EmpType,EmpPayFreqID;
	
	SELECT RowID FROM filingstatus WHERE MaritalStatus=employeemaritstat AND Dependent=COALESCE(esal_NoofDependents,0) INTO EmpFStatID;
	
			
	
		IF EmpType = 'Fixed' OR EmpType = 'Monthly' THEN
			IF EmpPayFreqID = 1 THEN
			
				SET esal_BasicDailyPay = 0;
				SET esal_BasicHourlyPay = 0;
			ELSE
				SET esal_BasicPay = esal_TrueSalary;
				SET esal_BasicDailyPay = 0;
				SET esal_BasicHourlyPay = 0;
			END IF;
			
			
			SET salaryToUseForContrib = esal_Salary;
			
	
		ELSEIF EmpType = 'Daily' THEN
				SET esal_BasicPay = esal_BasicPay;
				SET esal_BasicDailyPay = esal_BasicPay;
				SET esal_BasicHourlyPay = 0;
				
			SET salaryToUseForContrib = (esal_BasicPay * org_workingdays) / 12;
			
		ELSEIF EmpType = 'Weekly' THEN
				SET esal_BasicPay = esal_BasicPay;
				SET esal_BasicDailyPay = FORMAT((esal_BasicPay / 5), 2);
				SET esal_BasicHourlyPay = 0;
				
			IF org_workingdays BETWEEN 310 AND 320 THEN
				
				SET salaryToUseForContrib = (esal_Salary * (org_workingdays / 6)) / 12;
				
			ELSEIF org_workingdays BETWEEN 260 AND 270 THEN
				
				SET salaryToUseForContrib = (esal_Salary * (org_workingdays / 5)) / 12;
				
			END IF;	
			
		END IF;
		
		SELECT RowID FROM paysocialsecurity WHERE COALESCE(salaryToUseForContrib,0) BETWEEN RangeFromAmount AND IF(COALESCE(salaryToUseForContrib,0) > RangeToAmount, COALESCE(salaryToUseForContrib,0) + 1, RangeToAmount) ORDER BY MonthlySalaryCredit DESC LIMIT 1 INTO sss_amt;
	
		SELECT RowID FROM payphilhealth WHERE COALESCE(salaryToUseForContrib,0) BETWEEN SalaryRangeFrom AND IF(COALESCE(salaryToUseForContrib,0) > SalaryRangeTo, COALESCE(salaryToUseForContrib,0) + 1, SalaryRangeTo) ORDER BY SalaryBase DESC LIMIT 1 INTO phh_amt;
	
	
	SELECT GET_HDMFAmount(salaryToUseForContrib) INTO hdmf_amt;
	
	
	
	IF esal_RowID IS NULL AND emp_countsalaries >= 1 THEN
	
		SET preEffDateFromEmpSal = prevdatefrom;
			
		SET preEffDateFromEmpSallatest = IF(DATEDIFF(CURRENT_DATE(),preEffDateFromEmpSal) = 0, ADDDATE(CURRENT_DATE(), INTERVAL 1 DAY), IF(DATEDIFF(CURRENT_DATE(),preEffDateFromEmpSal) < 0, ADDDATE(preEffDateFromEmpSal, INTERVAL 1 DAY), CURRENT_DATE()));
		
		SELECT EffectiveDateFrom FROM employeesalary WHERE EmployeeID=esal_EmployeeID AND OrganizationID=esal_OrganizationID ORDER BY EffectiveDateFrom DESC LIMIT 1 INTO prevdatefrom;
		
		IF prevdatefrom = esal_EffectiveDateFrom THEN
			
			UPDATE employeesalary SET
			EffectiveDateTo=esal_EffectiveDateFrom
			,LastUpd=CURRENT_TIMESTAMP()
			,LastUpdBy=esal_CreatedBy
			WHERE RowID=prevesalRowID;
		
			SET esal_EffectiveDateFrom = ADDDATE(preEffDateFromEmpSal, INTERVAL 1 DAY);
			
		ELSEIF prevdatefrom > esal_EffectiveDateFrom THEN
		
			UPDATE employeesalary SET
			EffectiveDateTo=CURDATE()
			,LastUpd=CURRENT_TIMESTAMP()
			,LastUpdBy=esal_CreatedBy
			WHERE RowID=prevesalRowID;
		
			
			
		ELSEIF prevdatefrom < esal_EffectiveDateFrom THEN
				
			UPDATE employeesalary SET
			EffectiveDateTo=SUBDATE(esal_EffectiveDateFrom, INTERVAL 1 DAY)
			,LastUpd=CURRENT_TIMESTAMP()
			,LastUpdBy=esal_CreatedBy
			WHERE RowID=prevesalRowID;
		
			
			
		END IF;

		
		
	
	
	END IF;
	
	
	
	
	
	INSERT INTO employeesalary
	(
		RowID
		,EmployeeID
		,Created
		,CreatedBy
		,OrganizationID
		,FilingStatusID
		,PaySocialSecurityID
		,PayPhilhealthID
		,HDMFAmount
		,BasicPay
		,Salary
		,BasicDailyPay
		,BasicHourlyPay
		,NoofDependents
		,MaritalStatus
		,PositionID
		,EffectiveDateFrom
		,EffectiveDateTo
		,TrueSalary
		,UndeclaredSalary, OverrideDiscardSSSContrib, OverrideDiscardPhilHealthContrib
	) SELECT
		esal_RowID
		,esal_EmployeeID
		,CURRENT_TIMESTAMP()
		,esal_CreatedBy
		,esal_OrganizationID
		,EmpFStatID
		,sss_amt
		,phh_amt
		,esal_HDMFAmount
		,esal_Salary / IF(LOCATE(EmpType,CONCAT('MonthlyFixed')) > 0, PAYFREQUENCY_DIVISOR(pay_freq_type), PAYFREQUENCY_DIVISOR(EmpType))
		,esal_Salary
		,esal_BasicDailyPay
		,esal_BasicHourlyPay
		,esal_NoofDependents
		,esal_MaritalStatus
		,esal_PositionID
		,esal_EffectiveDateFrom
		,esal_EffectiveDateTo
		,esal_TrueSalary
		,esal_TrueSalary - esal_Salary, esal_DiscardSSS, esal_DiscardPhH
		FROM employee WHERE RowID=esal_EmployeeID AND OrganizationID=esal_OrganizationID
	ON 
	DUPLICATE 
	KEY 
	UPDATE 
		LastUpd=CURRENT_TIMESTAMP()
		,LastUpdBy=esal_LastUpdBy
		,FilingStatusID=EmpFStatID
		,PaySocialSecurityID=sss_amt
		,PayPhilhealthID=phh_amt
		,HDMFAmount=IF(esal_HDMFAmount != 0,esal_HDMFAmount,hdmf_amt)
		,BasicPay=esal_Salary / IF(LOCATE(EmpType,CONCAT('MonthlyFixed')) > 0, PAYFREQUENCY_DIVISOR(pay_freq_type), PAYFREQUENCY_DIVISOR(EmpType))
		,Salary=esal_Salary
		,BasicDailyPay=esal_BasicDailyPay
		,BasicHourlyPay=esal_BasicHourlyPay
		,NoofDependents=esal_NoofDependents
		,MaritalStatus=esal_MaritalStatus
		,PositionID=esal_PositionID
		,EffectiveDateFrom=esal_EffectiveDateFrom
		,TrueSalary=esal_TrueSalary
		,UndeclaredSalary=esal_TrueSalary - esal_Salary
		,EffectiveDateTo=esal_EffectiveDateTo
		,OverrideDiscardSSSContrib			=esal_DiscardSSS
		,OverrideDiscardPhilHealthContrib=esal_DiscardPhH;SELECT @@Identity AS id INTO esalID;
	
	RETURN esalID;

SELECT COUNT(RowID) FROM employeesalary WHERE EmployeeID=esal_EmployeeID AND OrganizationID=esal_OrganizationID AND EffectiveDateTo IS NULL INTO nullcounteffectivedateto;

IF nullcounteffectivedateto >= 2 THEN
	SET nullcounteffectivedateto = 0;
END IF;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
