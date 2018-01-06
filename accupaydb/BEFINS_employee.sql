/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFINS_employee`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFINS_employee` BEFORE INSERT ON `employee` FOR EACH ROW BEGIN

DECLARE marit_stat VARCHAR(50);

SET NEW.Salutation=IFNULL(NEW.Salutation,'');
SET NEW.FirstName=IFNULL(NEW.FirstName,'');
SET NEW.MiddleName=IFNULL(NEW.MiddleName,'');
SET NEW.LastName=IFNULL(NEW.LastName,'');
SET NEW.Surname=IFNULL(NEW.Surname,'');
SET NEW.EmployeeID=IFNULL(NEW.EmployeeID,'');
SET NEW.TINNo=IFNULL(NEW.TINNo,'   -   -   -');
SET NEW.SSSNo=IFNULL(NEW.SSSNo,'  -       -');
SET NEW.HDMFNo=IFNULL(NEW.HDMFNo,'    -    -');
SET NEW.PhilHealthNo=IFNULL(NEW.PhilHealthNo,'    -    -');
SET NEW.EmploymentStatus=IFNULL(NEW.EmploymentStatus,'Probationary');
SET NEW.EmailAddress=IFNULL(NEW.EmailAddress,'');
SET NEW.WorkPhone=IFNULL(NEW.WorkPhone,'');
SET NEW.HomePhone=IFNULL(NEW.HomePhone,'');
SET NEW.MobilePhone=IFNULL(NEW.MobilePhone,'');
SET NEW.HomeAddress=IFNULL(NEW.HomeAddress,'');
SET NEW.Nickname=IFNULL(NEW.Nickname,'');
SET NEW.JobTitle=IFNULL(NEW.JobTitle,'');
SET NEW.Gender=IFNULL(UPPER(SUBSTR(NEW.Gender,1,1)),'M');
SET NEW.EmployeeType=IFNULL(NEW.EmployeeType,'Daily');

SET marit_stat=IFNULL(NEW.MaritalStatus,'Single');
IF marit_stat NOT IN ('Married','Single') THEN
    SET marit_stat='Single';
END IF;
SET NEW.MaritalStatus=marit_stat;

SET NEW.Birthdate=IFNULL(NEW.Birthdate,'1900-01-01');
SET NEW.StartDate=IFNULL(NEW.StartDate,'1900-01-01');

SET NEW.PayFrequencyID=IFNULL(NEW.PayFrequencyID,1);
SET NEW.NoOfDependents=IFNULL(NEW.NoOfDependents,0);
SET NEW.UndertimeOverride=IFNULL(NEW.UndertimeOverride,'1');
SET NEW.OvertimeOverride=IFNULL(NEW.OvertimeOverride,'0');
SET NEW.NewEmployeeFlag=IFNULL(NEW.NewEmployeeFlag,'1');
SET NEW.LeaveBalance=IFNULL(NEW.LeaveBalance,0);
SET NEW.SickLeaveBalance=IFNULL(NEW.SickLeaveBalance,0);
SET NEW.MaternityLeaveBalance=IFNULL(NEW.MaternityLeaveBalance,0);
SET NEW.OtherLeaveBalance=IFNULL(NEW.OtherLeaveBalance,0);
SET NEW.LeaveAllowance=IFNULL(NEW.LeaveAllowance,0);
SET NEW.SickLeaveAllowance=IFNULL(NEW.SickLeaveAllowance,0);
SET NEW.MaternityLeaveAllowance=IFNULL(NEW.MaternityLeaveAllowance,0);
SET NEW.OtherLeaveAllowance=IFNULL(NEW.OtherLeaveAllowance,0);

SET NEW.LeavePerPayPeriod=IFNULL(NEW.LeavePerPayPeriod,0);
SET NEW.SickLeavePerPayPeriod=IFNULL(NEW.SickLeavePerPayPeriod,0);
SET NEW.MaternityLeavePerPayPeriod=IFNULL(NEW.MaternityLeavePerPayPeriod,0);
SET NEW.OtherLeavePerPayPeriod=IFNULL(NEW.OtherLeavePerPayPeriod,0);
SET NEW.AlphaListExempted=IFNULL(NEW.AlphaListExempted,'0');
SET NEW.WorkDaysPerYear=IFNULL(NEW.WorkDaysPerYear,313);

SET NEW.ATMNo=IFNULL(NEW.ATMNo,'');
SET NEW.BankName=IFNULL(NEW.BankName,'');
SET NEW.CalcHoliday=IFNULL(NEW.CalcHoliday,'1');
SET NEW.CalcSpecialHoliday=IFNULL(NEW.CalcSpecialHoliday,'1');
SET NEW.CalcNightDiff=IFNULL(NEW.CalcNightDiff,'1');
SET NEW.CalcNightDiffOT=IFNULL(NEW.CalcNightDiffOT,'1');
SET NEW.CalcRestDay=IFNULL(NEW.CalcRestDay,'1');
SET NEW.CalcRestDayOT=IFNULL(NEW.CalcRestDayOT,'1');

SET NEW.RevealInPayroll=IFNULL(NEW.RevealInPayroll,'1');
SET NEW.LateGracePeriod=IFNULL(NEW.LateGracePeriod,0);

SET NEW.OffsetBalance=IFNULL(NEW.OffsetBalance,'0');

SET NEW.ATMNo=IFNULL(NEW.ATMNo,'');
SET NEW.BankName=IFNULL(NEW.BankName,'');

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
