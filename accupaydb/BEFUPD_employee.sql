/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFUPD_employee`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFUPD_employee` BEFORE UPDATE ON `employee` FOR EACH ROW BEGIN

DECLARE anyvchar VARCHAR(150);

DECLARE terminate_date DATE;

DECLARE first_date DATE;

DECLARE eom_payp CHAR(1);

DECLARE loan_num INT(11);

DECLARE loan_typeID INT(11);

DECLARE ps_RowID INT(11);

DECLARE payp_ID INT(11);

DECLARE IsDepartmentChanged CHAR(1);
DECLARE hasPrivilege CHAR(1);
SELECT EXISTS(SELECT pv.RowID FROM position_view pv INNER JOIN user u ON u.RowID=NEW.LastUpdBy INNER JOIN position p ON p.RowID=u.PositionID WHERE pv.PositionID=p.RowID AND pv.OrganizationID=NEW.OrganizationID AND pv.Updates='Y') INTO hasPrivilege;
IF hasPrivilege='1' AND OLD.EmploymentStatus NOT IN ('Resigned','Terminated') AND NEW.EmploymentStatus IN ('Resigned','Terminated') AND NEW.TerminationDate IS NOT NULL AND NEW.StartDate < NEW.TerminationDate THEN
    SET anyvchar = '';



    SELECT RowID,PayFromDate,PayToDate,`Half` FROM payperiod WHERE OrganizationID=NEW.OrganizationID AND TotalGrossSalary=NEW.PayFrequencyID AND NEW.TerminationDate BETWEEN PayFromDate AND PayToDate INTO payp_ID,first_date,terminate_date,eom_payp;





    SELECT COUNT(RowID) FROM employeeloanschedule WHERE OrganizationID=NEW.OrganizationID AND EmployeeID=NEW.RowID INTO loan_num;

    SELECT RowID FROM product WHERE OrganizationID=NEW.OrganizationID AND `Category`='Loan Type' AND PartNo='CASH ADVANCE' INTO loan_typeID;

    INSERT INTO employeeloanschedule(OrganizationID,Created,CreatedBy,EmployeeID,LoanNumber,DedEffectiveDateFrom,DedEffectiveDateTo,TotalLoanAmount,DeductionSchedule,TotalBalanceLeft,DeductionAmount,`Status`,LoanTypeID,DeductionPercentage,NoOfPayPeriod,LoanPayPeriodLeft,Comments) VALUES (NEW.OrganizationID,CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.RowID,(loan_num + 1),first_date,terminate_date,1500.0,IF(eom_payp='0', 'End of the month', 'First half'),1500.0,1500.0,'In progress',loan_typeID,0.0,1,1,'deposit for transpo allowance') ON DUPLICATE KEY UPDATE LastUpd=CURRENT_TIMESTAMP(),LastUpdBy=NEW.LastUpdBy;

    SELECT RowID FROM paystub WHERE EmployeeID=NEW.RowID AND OrganizationID=NEW.OrganizationID AND PayPeriodID=payp_ID INTO ps_RowID;

    IF ps_RowID > 0 THEN

        UPDATE paystubitem psi
        SET psi.PayAmount=psi.PayAmount + 1500.0
        ,psi.LastUpd=CURRENT_TIMESTAMP()
        ,psi.LastUpdBy=NEW.LastUpdBy
        WHERE psi.ProductID=loan_typeID
        AND psi.OrganizationID=NEW.OrganizationID
        AND psi.PayStubID=ps_RowID;

    END IF;

ELSE

    SET NEW.TerminationDate = NULL;

END IF;

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
SET NEW.MaritalStatus=IFNULL(NEW.MaritalStatus,'Single');
SET NEW.Birthdate=IFNULL(NEW.Birthdate,'1900-01-01');
SET NEW.StartDate=IFNULL(NEW.StartDate,'1900-01-01');

SET NEW.PayFrequencyID=IFNULL(NEW.PayFrequencyID,1);
SET NEW.NoOfDependents=IFNULL(NEW.NoOfDependents,0);
SET NEW.UndertimeOverride=IFNULL(NEW.UndertimeOverride,'1');
SET NEW.OvertimeOverride=IFNULL(NEW.OvertimeOverride,'1');
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
SET NEW.DayOfRest=IFNULL(NEW.DayOfRest,'1');
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

IF NEW.PositionID IS NULL THEN SET NEW.PositionID = OLD.PositionID; END IF;

IF OLD.PositionID != NEW.PositionID THEN

    SELECT (pos.DivisionId != IFNULL(pot.DivisionId,0))
    FROM position pos
    LEFT JOIN position pot ON pot.RowID=NEW.PositionID
    WHERE pos.RowID=OLD.PositionID
    INTO IsDepartmentChanged;

    IF IsDepartmentChanged = '1' THEN

        SET IsDepartmentChanged = '1';

    END IF;

END IF;
SET NEW.BranchID = IF(IFNULL(NEW.BranchID,0)=0,NULL,NEW.BranchID);
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
