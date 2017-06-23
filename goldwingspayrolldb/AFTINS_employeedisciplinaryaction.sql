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

-- Dumping structure for trigger goldwingspayrolldb.AFTINS_employeedisciplinaryaction
DROP TRIGGER IF EXISTS `AFTINS_employeedisciplinaryaction`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_employeedisciplinaryaction` AFTER INSERT ON `employeedisciplinaryaction` FOR EACH ROW BEGIN


DECLARE viewID INT(11);

DECLARE suspendaycount INT(11);

DECLARE indx INT(11) DEFAULT 0;

DECLARE dateloop DATE;

DECLARE eshiftID INT(11);

DECLARE esalID INT(11);

DECLARE etentID INT(11);

IF NEW.Action IN ('1-3 Days Suspension','4-7 Days Suspension','8-14 Days Suspension') THEN

	SELECT DATEDIFF(NEW.DateTo,NEW.DateFrom) INTO suspendaycount;
	
	timeentloop : LOOP
	
		IF indx <= suspendaycount THEN
			
			SELECT ADDDATE(NEW.DateFrom,INTERVAL indx DAY) INTO dateloop;
			
			SELECT RowID FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND Date=dateloop LIMIT 1 INTO etentID;
			
			SELECT RowID FROM employeeshift WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND dateloop BETWEEN DATE(COALESCE(EffectiveFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) AND DATEDIFF(CURRENT_DATE(),EffectiveFrom) >= 0 ORDER BY DATEDIFF(DATE_FORMAT(NOW(),'%Y-%m-%d'),EffectiveFrom) LIMIT 1 INTO eshiftID;
			
			SELECT RowID FROM employeesalary WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND dateloop BETWEEN DATE(COALESCE(EffectiveDateFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveDateTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) ORDER BY DATEDIFF(DATE_FORMAT(NOW(),'%Y-%m-%d'),EffectiveDateFrom) LIMIT 1 INTO esalID;
			
			INSERT INTO employeetimeentry
			(
				RowID
				,OrganizationID
				,Created
				,CreatedBy
				,Date
				,EmployeeShiftID
				,EmployeeID
				,EmployeeSalaryID
				,EmployeeFixedSalaryFlag
				,RegularHoursWorked
				,RegularHoursAmount
				,TotalHoursWorked
				,OvertimeHoursWorked
				,OvertimeHoursAmount
				,UndertimeHours
				,UndertimeHoursAmount
				,NightDifferentialHours
				,NightDiffHoursAmount
				,NightDifferentialOTHours
				,NightDiffOTHoursAmount
				,HoursLate
				,HoursLateAmount
				,LateFlag
				,PayRateID
				,VacationLeaveHours
				,SickLeaveHours
				,MaternityLeaveHours
				,OtherLeaveHours
				,TotalDayPay
			) VALUES (
				etentID
				,NEW.OrganizationID
				,CURRENT_TIMESTAMP()
				,NEW.CreatedBy
				,dateloop
				,eshiftID
				,NEW.EmployeeID
				,esalID
				,'0'
				,0
				,0
				,0
				,0
				,0
				,0
				,0
				,0
				,0
				,0
				,0
				,0
				,0
				,0
				,(SELECT RowID FROM payrate WHERE Date=dateloop AND OrganizationID=NEW.OrganizationID LIMIT 1)
				,0
				,0
				,0
				,0
				,0
			) ON
			DUPLICATE
			KEY
			UPDATE 
					LastUpd=CURRENT_TIMESTAMP()
					,LastUpdBy=NEW.CreatedBy
					,RegularHoursWorked=0
					,RegularHoursAmount=0
					,TotalHoursWorked=0
					,OvertimeHoursWorked=0
					,OvertimeHoursAmount=0
					,UndertimeHours=0
					,UndertimeHoursAmount=0
					,NightDifferentialHours=0
					,NightDiffHoursAmount=0
					,NightDifferentialOTHours=0
					,NightDiffOTHoursAmount=0
					,HoursLate=0
					,HoursLateAmount=0
					,LateFlag='0'
					,TotalDayPay=0;
			
			SET indx = indx + 1;
			
		ELSE
		
			LEAVE timeentloop;
		
		END IF;
		
	END LOOP;

END IF;
	







SELECT RowID FROM `view` WHERE ViewName='Employee Disciplinary Action' AND OrganizationID=NEW.OrganizationID LIMIT 1 INTO viewID;

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EmployeeID',NEW.RowID,'',NEW.EmployeeID,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'DateFrom',NEW.RowID,'',NEW.DateFrom,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'DateTo',NEW.RowID,'',NEW.DateTo,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'FindingID',NEW.RowID,'',NEW.FindingID,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'FindingDescription',NEW.RowID,'',NEW.FindingDescription,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'Action',NEW.RowID,'',NEW.Action,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'Penalty',NEW.RowID,'',NEW.Penalty,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'Comments',NEW.RowID,'',NEW.Comments,'Insert');



END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
