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

-- Dumping structure for function goldwingspayrolldb.GET_employeeallowance
DROP FUNCTION IF EXISTS `GET_employeeallowance`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GET_employeeallowance`(`payp_From` DATE, `payp_To` DATE, `EmployeeRowID` INT, `OrganizationRowID` INT, `Taxable` VARCHAR(1), `Frequency` VARCHAR(50)) RETURNS decimal(10,2)
    DETERMINISTIC
BEGIN

DECLARE date_diff INT(11);

DECLARE dayofwik DATE;

DECLARE dateloop DATE;

DECLARE emphourworked DECIMAL(10,2) DEFAULT 0;

DECLARE dutyhours DECIMAL(10,2) DEFAULT 0;

DECLARE empallowancecount INT(11);

DECLARE empallowanceamounts DECIMAL(10,2) DEFAULT 0;

DECLARE totalAllowanceWork DECIMAL(10,2) DEFAULT 0;

DECLARE indx INT(11) DEFAULT 0;

DECLARE ii INT(11) DEFAULT 0;

SELECT DATEDIFF(payp_To,payp_From) INTO date_diff;

indx_loop : LOOP

	IF indx < date_diff THEN
	
		SELECT ADDDATE(payp_From,INTERVAL indx DAY) INTO dateloop;
	
		SELECT ((TIME_TO_SEC(TIMEDIFF(IF(TimeIn>TimeOut,ADDTIME(TimeOut,'24:00:00'),TimeOut),TimeIn)) / 60) / 60) 'HrsWorked' FROM employeetimeentrydetails WHERE EmployeeID=EmployeeRowID AND OrganizationID=OrganizationRowID AND Date=dateloop INTO emphourworked;
	
		SET emphourworked = IF(emphourworked IS NULL, 0, IFNULL(emphourworked,0));
		
		IF emphourworked != 0 THEN
		
			SELECT ((TIME_TO_SEC(TIMEDIFF(IF(sh.TimeFrom>sh.TimeTo,ADDTIME(sh.TimeTo,'24:00:00'),sh.TimeTo),sh.TimeFrom)) / 60) / 60) 'DutyHrs' FROM employeeshift esh LEFT JOIN shift sh ON sh.RowID=esh.ShiftID WHERE esh.EmployeeID=EmployeeRowID AND esh.OrganizationID=OrganizationRowID AND dateloop BETWEEN DATE(COALESCE(esh.EffectiveFrom, DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(esh.EffectiveTo, ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) AND DATEDIFF(dateloop,esh.EffectiveFrom) >= 0 AND COALESCE(esh.RestDay,0)='0' ORDER BY DATEDIFF(DATE_FORMAT(dateloop,'%Y-%m-%d'),esh.EffectiveFrom) LIMIT 1 INTO dutyhours;
			
			SET dutyhours = IF(dutyhours IS NULL, 0, IFNULL(dutyhours,0));
			
			SET dutyhours = IF(dutyhours > 8, (dutyhours - 1), dutyhours);
			
			
			IF dutyhours != 0 THEN
			
					SELECT COUNT(RowID) FROM employeeallowance WHERE EmployeeID=EmployeeRowID AND OrganizationID=OrganizationRowID AND TaxableFlag=Taxable AND AllowanceFrequency=Frequency AND dateloop BETWEEN EffectiveStartDate AND EffectiveEndDate INTO empallowancecount;
				
					IF empallowancecount != 0 THEN
						
						countallowance : LOOP
							
								IF ii < empallowancecount THEN
								
										
									SELECT IFNULL(AllowanceAmount,0) FROM employeeallowance WHERE EmployeeID=EmployeeRowID AND OrganizationID=OrganizationRowID AND TaxableFlag=Taxable AND AllowanceFrequency=Frequency AND dateloop BETWEEN EffectiveStartDate AND EffectiveEndDate ORDER BY EffectiveStartDate LIMIT ii,1 INTO empallowanceamounts;
								
									SET empallowanceamounts = IFNULL(empallowanceamounts,0);
								
									
									SET totalAllowanceWork = totalAllowanceWork + ((empallowanceamounts / dutyhours) * emphourworked);
									
									
									SET ii = ii + 1;
									
								ELSE
									
									LEAVE countallowance;
								
								END IF;
						
						
						END LOOP;
					
					END IF;
						
			
			END IF;
			
		
		END IF;

		SET ii = 0;
		
		SET indx = indx  + 1;
		
	ELSE
	
		LEAVE indx_loop;
	
	END IF;

END LOOP;

RETURN totalAllowanceWork;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
