/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTUPD_employee_then_employeesalary`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_employee_then_employeesalary` AFTER UPDATE ON `employee` FOR EACH ROW BEGIN

DECLARE emp_chklist_ID INT(11);

DECLARE viewID INT(11);

DECLARE NEW_agency_name VARCHAR(100);

DECLARE OLD_agency_name VARCHAR(100);

DECLARE NEW_agfee DECIMAL(11,2) DEFAULT 0;

DECLARE OLD_agfee DECIMAL(11,2) DEFAULT 0;

DECLARE agfee_percent DECIMAL(11,2) DEFAULT 0;

SELECT RowID FROM employeechecklist WHERE EmployeeID=NEW.RowID ORDER BY RowID DESC LIMIT 1 INTO emp_chklist_ID;

INSERT INTO employeechecklist
(
    RowID
    ,OrganizationID
    ,Created
    ,CreatedBy
    ,EmployeeID
    ,PerformanceAppraisal
    ,BIRTIN
    ,Diploma
    ,IDInfoSlip
    ,PhilhealthID
    ,HDMFID
    ,SSSNo
    ,TranscriptOfRecord
    ,BirthCertificate
    ,EmployeeContract
    ,MedicalExam
    ,NBIClearance
    ,COEEmployer
    ,MarriageContract
    ,HouseSketch
    ,TrainingAgreement
    ,HealthPermit
    ,ValidID
    ,Resume
) VALUES (
    emp_chklist_ID
    ,NEW.OrganizationID
    ,CURRENT_TIMESTAMP()
    ,NEW.CreatedBy
    ,NEW.RowID
    ,0
    ,IF(COALESCE(NEW.TINNo,'   -   -   -') = '   -   -   -', 0, 1)
    ,0
    ,0
    ,IF(COALESCE(NEW.PhilHealthNo,'    -    -') = '    -    -', 0, 1)
    ,IF(COALESCE(NEW.HDMFNo,'    -    -') = '    -    -', 0, 1)
    ,IF(COALESCE(NEW.SSSNo,'  -       -') = '  -       -', 0, 1)
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
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=NEW.LastUpdBy
    ,PerformanceAppraisal=0
    ,BIRTIN=IF(COALESCE(NEW.TINNo,'   -   -   -') = '   -   -   -', 0, 1)
    ,Diploma=0
    ,IDInfoSlip=0
    ,PhilhealthID=IF(COALESCE(NEW.PhilHealthNo,'    -    -') = '    -    -', 0, 1)
    ,HDMFID=IF(COALESCE(NEW.HDMFNo,'    -    -') = '    -    -', 0, 1)
    ,SSSNo=IF(COALESCE(NEW.SSSNo,'  -       -') = '  -       -', 0, 1)
    ,TranscriptOfRecord=0
    ,BirthCertificate=0
    ,EmployeeContract=0
    ,MedicalExam=0
    ,NBIClearance=0
    ,COEEmployer=0
    ,MarriageContract=0
    ,HouseSketch=0
    ,TrainingAgreement=0
    ,HealthPermit=0
    ,ValidID=0
    ,Resume=0;

SELECT RowID FROM `view` WHERE ViewName='Employee Personal Profile' AND OrganizationID=NEW.OrganizationID LIMIT 1 INTO viewID;

IF OLD.Salutation!=NEW.Salutation THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'Salutation',NEW.RowID,OLD.Salutation,NEW.Salutation,'Update'); END IF;

IF OLD.EmployeeID!=NEW.EmployeeID THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'EmployeeID',NEW.RowID,OLD.EmployeeID,NEW.Salutation,'Update'); END IF;

IF OLD.FirstName!=NEW.FirstName THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'FirstName',NEW.RowID,OLD.FirstName,NEW.Salutation,'Update'); END IF;

IF OLD.MiddleName!=NEW.MiddleName THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'MiddleName',NEW.RowID,OLD.MiddleName,NEW.Salutation,'Update'); END IF;

IF OLD.LastName!=NEW.LastName THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'LastName',NEW.RowID,OLD.LastName,NEW.Salutation,'Update'); END IF;

IF OLD.TINNo!=NEW.TINNo THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'TINNo',NEW.RowID,OLD.TINNo,NEW.Salutation,'Update'); END IF;

IF OLD.SSSNo!=NEW.SSSNo THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'SSSNo',NEW.RowID,OLD.SSSNo,NEW.Salutation,'Update'); END IF;

IF OLD.HDMFNo!=NEW.HDMFNo THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'HDMFNo',NEW.RowID,OLD.HDMFNo,NEW.Salutation,'Update'); END IF;

IF OLD.PhilHealthNo!=NEW.PhilHealthNo THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'PhilHealthNo',NEW.RowID,OLD.PhilHealthNo,NEW.Salutation,'Update'); END IF;

IF OLD.EmploymentStatus!=NEW.EmploymentStatus THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'EmploymentStatus',NEW.RowID,OLD.EmploymentStatus,NEW.Salutation,'Update'); END IF;

IF OLD.EmailAddress!=NEW.EmailAddress THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'EmailAddress',NEW.RowID,OLD.EmailAddress,NEW.Salutation,'Update'); END IF;

IF OLD.WorkPhone!=NEW.WorkPhone THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'WorkPhone',NEW.RowID,OLD.WorkPhone,NEW.Salutation,'Update'); END IF;

IF OLD.HomePhone!=NEW.HomePhone THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'HomePhone',NEW.RowID,OLD.HomePhone,NEW.Salutation,'Update'); END IF;

IF OLD.MobilePhone!=NEW.MobilePhone THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'MobilePhone',NEW.RowID,OLD.MobilePhone,NEW.Salutation,'Update'); END IF;

IF OLD.HomeAddress!=NEW.HomeAddress THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'HomeAddress',NEW.RowID,OLD.HomeAddress,NEW.Salutation,'Update'); END IF;

IF OLD.Nickname!=NEW.Nickname THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'Nickname',NEW.RowID,OLD.Nickname,NEW.Salutation,'Update'); END IF;

IF OLD.JobTitle!=NEW.JobTitle THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'JobTitle',NEW.RowID,OLD.JobTitle,NEW.Salutation,'Update'); END IF;

IF OLD.Gender!=NEW.Gender THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'Gender',NEW.RowID,OLD.Gender,NEW.Salutation,'Update'); END IF;

IF OLD.EmployeeType!=NEW.EmployeeType THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'EmployeeType',NEW.RowID,OLD.EmployeeType,NEW.Salutation,'Update'); END IF;

IF OLD.MaritalStatus!=NEW.MaritalStatus THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'MaritalStatus',NEW.RowID,OLD.MaritalStatus,NEW.MaritalStatus,'Update'); END IF;

IF OLD.Birthdate!=NEW.Birthdate THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'Birthdate',NEW.RowID,OLD.Birthdate,NEW.Salutation,'Update'); END IF;

IF OLD.StartDate!=NEW.StartDate THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'StartDate',NEW.RowID,OLD.StartDate,NEW.Salutation,'Update'); END IF;

IF OLD.TerminationDate!=NEW.TerminationDate THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'TerminationDate',NEW.RowID,OLD.TerminationDate,NEW.Salutation,'Update'); END IF;

IF OLD.PayFrequencyID!=NEW.PayFrequencyID THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'PayFrequencyID',NEW.RowID,OLD.PayFrequencyID,NEW.Salutation,'Update'); END IF;

IF OLD.NoOfDependents!=NEW.NoOfDependents THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'NoOfDependents',NEW.RowID,OLD.NoOfDependents,NEW.Salutation,'Update'); END IF;

IF OLD.UndertimeOverride!=NEW.UndertimeOverride THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'UndertimeOverride',NEW.RowID,OLD.UndertimeOverride,NEW.Salutation,'Update'); END IF;

IF OLD.OvertimeOverride!=NEW.OvertimeOverride THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'OvertimeOverride',NEW.RowID,OLD.OvertimeOverride,NEW.Salutation,'Update'); END IF;

IF OLD.NewEmployeeFlag!=NEW.NewEmployeeFlag THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'NewEmployeeFlag',NEW.RowID,OLD.NewEmployeeFlag,NEW.Salutation,'Update'); END IF;

IF OLD.LeaveBalance!=NEW.LeaveBalance THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'LeaveBalance',NEW.RowID,OLD.LeaveBalance,NEW.Salutation,'Update'); END IF;

IF OLD.SickLeaveBalance!=NEW.SickLeaveBalance THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'SickLeaveBalance',NEW.RowID,OLD.SickLeaveBalance,NEW.Salutation,'Update'); END IF;

IF OLD.MaternityLeaveBalance!=NEW.MaternityLeaveBalance THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'MaternityLeaveBalance',NEW.RowID,OLD.MaternityLeaveBalance,NEW.Salutation,'Update'); END IF;

IF OLD.LeaveAllowance!=NEW.LeaveAllowance THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'LeaveAllowance',NEW.RowID,OLD.LeaveAllowance,NEW.Salutation,'Update'); END IF;

IF OLD.SickLeaveAllowance!=NEW.SickLeaveAllowance THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'SickLeaveAllowance',NEW.RowID,OLD.SickLeaveAllowance,NEW.Salutation,'Update'); END IF;

IF OLD.MaternityLeaveAllowance!=NEW.MaternityLeaveAllowance THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'MaternityLeaveAllowance',NEW.RowID,OLD.MaternityLeaveAllowance,NEW.Salutation,'Update'); END IF;

IF OLD.LeavePerPayPeriod!=NEW.LeavePerPayPeriod THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'LeavePerPayPeriod',NEW.RowID,OLD.LeavePerPayPeriod,NEW.Salutation,'Update'); END IF;

IF OLD.SickLeavePerPayPeriod!=NEW.SickLeavePerPayPeriod THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'SickLeavePerPayPeriod',NEW.RowID,OLD.SickLeavePerPayPeriod,NEW.Salutation,'Update'); END IF;

IF OLD.MaternityLeavePerPayPeriod!=NEW.MaternityLeavePerPayPeriod THEN INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'MaternityLeavePerPayPeriod',NEW.RowID,OLD.MaternityLeavePerPayPeriod,NEW.Salutation,'Update'); END IF;


IF OLD.PositionID!=NEW.PositionID THEN

    INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'PositionID',NEW.RowID,OLD.PositionID,NEW.PositionID,'Update');

END IF;


IF IFNULL(OLD.AgencyID,0) != IFNULL(NEW.AgencyID,0) THEN

    SELECT ag.AgencyName,ag.`AgencyFee` FROM agency ag WHERE ag.RowID=OLD.AgencyID INTO OLD_agency_name,OLD_agfee;

    SET OLD_agency_name = IFNULL(OLD_agency_name,'');
    SET OLD_agfee = IFNULL(OLD_agfee,0.0);

    SELECT ag.AgencyName,ag.`AgencyFee` FROM agency ag WHERE ag.RowID=NEW.AgencyID INTO NEW_agency_name,NEW_agfee;

    SET NEW_agency_name = IFNULL(NEW_agency_name,'');
    SET NEW_agfee = IFNULL(NEW_agfee,0.0);

    SET agfee_percent = NEW_agfee / OLD_agfee;



    IF agfee_percent != 1.0 THEN

        UPDATE agencyfee agf
        SET agf.DailyFee = agf.DailyFee * agfee_percent
        WHERE agf.OrganizationID=NEW.OrganizationID
        AND agf.AgencyID=NEW.AgencyID
        AND agf.EmployeeID=NEW.RowID
        AND agf.EmpPositionID=NEW.PositionID;

    END IF;

    INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'Agency',NEW.RowID,OLD_agency_name,NEW_agency_name,'Update');

END IF;



END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
