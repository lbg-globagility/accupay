/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTINS_employee_then_employeesalary`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_employee_then_employeesalary` AFTER INSERT ON `employee` FOR EACH ROW BEGIN

DECLARE viewID INT(11);

DECLARE marit_stat CHAR(50);

DECLARE exist_empstatus INT(1);

DECLARE exist_emptype INT(1);

DECLARE exist_empsalutat INT(1);

DECLARE anyint INT(11);

DECLARE div_RowID INT(11);

DECLARE hasShiftForThisYear CHAR(1);

DECLARE EndingDate DATE DEFAULT LAST_DAY(DATE_FORMAT(CURDATE(),'%Y-12-01'));

DECLARE StartingDate DATE;

DECLARE dept_mate_empRowID INT(11);

DECLARE ordervaloforigin INT(11);

DECLARE the_date DATE;


SELECT IF(NEW.MaritalStatus IN ('Single','Married'),NEW.MaritalStatus,'Zero') INTO marit_stat;
/*
    INSERT INTO employeesalary
    (
        EmployeeID
        ,Created
        ,CreatedBy
        ,OrganizationID
        ,HDMFAmount
        ,BasicPay
        ,Salary
        ,BasicDailyPay
        ,BasicHourlyPay
        ,FilingStatusID
        ,NoofDependents
        ,MaritalStatus
        ,PositionID
        ,EffectiveDateFrom
    ) VALUES (
        NEW.RowID
        ,CURRENT_TIMESTAMP()
        ,NEW.CreatedBy
        ,NEW.OrganizationID
        ,100.0
        ,0
        ,0
        ,0
        ,0
        ,(SELECT RowID FROM filingstatus WHERE MaritalStatus=marit_stat AND Dependent=COALESCE(NEW.NoOfDependents,0))
        ,COALESCE(NEW.NoOfDependents,0)
        ,NEW.MaritalStatus
        ,NEW.PositionID
        ,NEW.StartDate
    );

*/

INSERT INTO employeechecklist
(
    OrganizationID
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
    NEW.OrganizationID
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
);



SELECT RowID FROM `view` WHERE ViewName='Employee Personal Profile' AND OrganizationID=NEW.OrganizationID LIMIT 1 INTO viewID;

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES
(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EmployeeID',NEW.RowID,'',NEW.EmployeeID,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'Salutation',NEW.RowID,'',NEW.Salutation,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'FirstName',NEW.RowID,'',NEW.FirstName,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'MiddleName',NEW.RowID,'',NEW.MiddleName,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'LastName',NEW.RowID,'',NEW.LastName,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'Surname',NEW.RowID,'',NEW.Surname,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TINNo',NEW.RowID,'',NEW.TINNo,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'SSSNo',NEW.RowID,'',NEW.SSSNo,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'HDMFNo',NEW.RowID,'',NEW.HDMFNo,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'PhilHealthNo',NEW.RowID,'',NEW.PhilHealthNo,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EmploymentStatus',NEW.RowID,'',NEW.EmploymentStatus,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EmailAddress',NEW.RowID,'',NEW.EmailAddress,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'WorkPhone',NEW.RowID,'',NEW.WorkPhone,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'HomePhone',NEW.RowID,'',NEW.HomePhone,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'MobilePhone',NEW.RowID,'',NEW.MobilePhone,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'HomeAddress',NEW.RowID,'',NEW.HomeAddress,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'Nickname',NEW.RowID,'',NEW.Nickname,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'JobTitle',NEW.RowID,'',NEW.JobTitle,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'Gender',NEW.RowID,'',NEW.Gender,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EmployeeType',NEW.RowID,'',NEW.EmployeeType,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'MaritalStatus',NEW.RowID,'',NEW.MaritalStatus,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'Birthdate',NEW.RowID,'',NEW.Birthdate,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'StartDate',NEW.RowID,'',NEW.StartDate,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TerminationDate',NEW.RowID,'',NEW.TerminationDate,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'PositionID',NEW.RowID,'',NEW.PositionID,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'PayFrequencyID',NEW.RowID,'',NEW.PayFrequencyID,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'NoOfDependents',NEW.RowID,'',NEW.NoOfDependents,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'UndertimeOverride',NEW.RowID,'',NEW.UndertimeOverride,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'OvertimeOverride',NEW.RowID,'',NEW.OvertimeOverride,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'NewEmployeeFlag',NEW.RowID,'',NEW.NewEmployeeFlag,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'LeaveBalance',NEW.RowID,'',NEW.LeaveBalance,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'SickLeaveBalance',NEW.RowID,'',NEW.SickLeaveBalance,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'MaternityLeaveBalance',NEW.RowID,'',NEW.MaternityLeaveBalance,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'LeaveAllowance',NEW.RowID,'',NEW.LeaveAllowance,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'SickLeaveAllowance',NEW.RowID,'',NEW.SickLeaveAllowance,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'MaternityLeaveAllowance',NEW.RowID,'',NEW.MaternityLeaveAllowance,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'LeavePerPayPeriod',NEW.RowID,'',NEW.LeavePerPayPeriod,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'SickLeavePerPayPeriod',NEW.RowID,'',NEW.SickLeavePerPayPeriod,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'MaternityLeavePerPayPeriod',NEW.RowID,'',NEW.MaternityLeavePerPayPeriod,'Insert');






SELECT EXISTS(SELECT RowID FROM listofval WHERE Type='Employment Status' AND Active='Yes' AND DisplayValue=NEW.EmploymentStatus) INTO exist_empstatus;

IF exist_empstatus = 0 AND NEW.EmploymentStatus!='' THEN

    INSERT INTO listofval (`Type`,DisplayValue,Active,CreatedBy,Created,LastUpdBy,LastUpd) VALUES ('Employment Status',NEW.EmploymentStatus,'Yes',NEW.CreatedBy,CURRENT_TIMESTAMP(),NEW.CreatedBy,CURRENT_TIMESTAMP()) ON DUPLICATE KEY UPDATE LastUpd=CURRENT_TIMESTAMP();

END IF;

SELECT EXISTS(SELECT RowID FROM listofval WHERE Type='Employee Type' AND Active='Yes' AND DisplayValue=NEW.EmployeeType) INTO exist_emptype;

IF exist_emptype = 0 AND NEW.EmployeeType!='' THEN

    INSERT INTO listofval (`Type`,DisplayValue,Active,CreatedBy,Created,LastUpdBy,LastUpd) VALUES ('Employee Type',NEW.EmployeeType,'Yes',NEW.CreatedBy,CURRENT_TIMESTAMP(),NEW.CreatedBy,CURRENT_TIMESTAMP()) ON DUPLICATE KEY UPDATE LastUpd=CURRENT_TIMESTAMP();

END IF;



SELECT EXISTS(SELECT RowID FROM listofval WHERE Type='Salutation' AND Active='Yes' AND DisplayValue=NEW.Salutation) INTO exist_empsalutat;

IF exist_empsalutat = 0 AND NEW.Salutation!='' THEN

    INSERT INTO listofval (`Type`,DisplayValue,Active,CreatedBy,Created,LastUpdBy,LastUpd) VALUES ('Salutation',NEW.Salutation,'Yes',NEW.CreatedBy,CURRENT_TIMESTAMP(),NEW.CreatedBy,CURRENT_TIMESTAMP()) ON DUPLICATE KEY UPDATE LastUpd=CURRENT_TIMESTAMP();

END IF;


IF NEW.BankName IS NOT NULL THEN

    SELECT `INSUPD_listofval`(NEW.BankName,NEW.BankName, 'Bank Names',NEW.BankName, 'Yes', NEW.BankName, NEW.CreatedBy, '1') INTO anyint;

END IF;

SELECT pos.DivisionId FROM position pos WHERE pos.RowID=IFNULL(NEW.PositionID,0) AND pos.OrganizationID=NEW.OrganizationID INTO div_RowID;

IF div_RowID IS NOT NULL THEN



    SELECT e.RowID FROM employee e INNER JOIN position pos ON pos.DivisionId=div_RowID AND pos.OrganizationID=e.OrganizationID WHERE e.OrganizationID=NEW.OrganizationID LIMIT 1 INTO dept_mate_empRowID;

    INSERT INTO `employeeshiftbyday`
    (
        OrganizationID
        ,Created
        ,CreatedBy
        ,EmployeeID
        ,ShiftID
        ,NameOfDay
        ,NightShift
        ,RestDay
        ,OrderByValue
        ,IsEncodedByDay
        ,UniqueShift
    ) SELECT
        NEW.OrganizationID
        ,CURRENT_TIMESTAMP()
        ,NEW.CreatedBy
        ,NEW.RowID
        ,esd.ShiftID
        ,esd.NameOfDay
        ,esd.NightShift
        ,esd.RestDay
        ,esd.OrderByValue
        ,esd.IsEncodedByDay
        ,esd.UniqueShift
        FROM employeeshiftbyday esd
        WHERE esd.OrganizationID=NEW.OrganizationID
        AND esd.EmployeeID=dept_mate_empRowID
        ORDER BY esd.OrderByValue
        LIMIT 7;

        SELECT OrderByValue,SampleDate FROM employeeshiftbyday WHERE EmployeeID=NEW.RowID AND OrganizationID=NEW.OrganizationID AND OriginDay=0 LIMIT 1 INTO ordervaloforigin,the_date;

        UPDATE employeeshiftbyday esb
        SET esb.OriginDay = esb.OrderByValue - ordervaloforigin
        WHERE esb.EmployeeID=NEW.RowID
        AND esb.OrganizationID=NEW.OrganizationID
        ORDER BY esb.OrderByValue;

        UPDATE employeeshiftbyday esb
        SET esb.SampleDate=ADDDATE(the_date,esb.OriginDay)
        WHERE esb.EmployeeID=NEW.RowID
        AND esb.OrganizationID=NEW.OrganizationID
        ORDER BY esb.OrderByValue;

        SET @uniqueshift = 0;

        SET @indxcount = 0;

        UPDATE employeeshiftbyday esd
        INNER JOIN (
                        SELECT *
                        ,(@indxcount := @indxcount + 1) AS IncrementUnique
                        FROM (
                                SELECT esd.RowID
                                ,esd.ShiftID
                                FROM employeeshiftbyday esd
                                WHERE esd.EmployeeID=NEW.RowID
                                AND esd.OrganizationID=NEW.OrganizationID
                                AND esd.ShiftID IS NOT NULL
                                GROUP BY esd.ShiftID
                                ORDER BY esd.SampleDate
                        ) i
        ) esdd ON esdd.ShiftID = esd.ShiftID
        SET esd.UniqueShift=esdd.IncrementUnique
        WHERE esd.EmployeeID=NEW.RowID
        AND esd.OrganizationID=NEW.OrganizationID;

        INSERT INTO employeefirstweekshift
        (
            OrganizationID
            ,CreatedBy
            ,EmployeeID
            ,ShiftID
            ,EffectiveFrom
            ,EffectiveTo
            ,NightShift
            ,RestDay
            ,IsEncodedByDay
        )   SELECT esd.OrganizationID
            ,esd.CreatedBy
            ,esd.EmployeeID
            ,esd.ShiftID
            ,esd.SampleDate
            ,ADDDATE(esd.SampleDate,INTERVAL (COUNT(RowID) - 1) DAY)
            ,esd.NightShift
            ,esd.RestDay
            ,esd.IsEncodedByDay
            FROM employeeshiftbyday esd
            WHERE esd.EmployeeID=NEW.RowID
            GROUP BY esd.ShiftID
            HAVING esd.OrganizationID = NEW.OrganizationID;

END IF;

CALL AUTOINS_leaveledger(NEW.OrganizationID, NEW.RowID, NEW.CreatedBy);

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
