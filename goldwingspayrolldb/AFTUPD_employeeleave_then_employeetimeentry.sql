-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.12-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for trigger hyundaipayrolldb_dev.AFTUPD_employeeleave_then_employeetimeentry
DROP TRIGGER IF EXISTS `AFTUPD_employeeleave_then_employeetimeentry`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_employeeleave_then_employeetimeentry` AFTER UPDATE ON `employeeleave` FOR EACH ROW BEGIN

DECLARE looper INT(11) DEFAULT 0;

DECLARE leavetype VARCHAR(50);

DECLARE dateloop DATE;

DECLARE newleavenumdays INT(11);

DECLARE oldleavenumdays INT(11);

DECLARE reghrsworkd TIME;

DECLARE numhrsworkd DECIMAL(10,2);

DECLARE prateID INT(11);

DECLARE viewID INT(11);

SELECT NEW.LeaveType INTO leavetype;

SELECT ADDDATE(TIMEDIFF(IF(NEW.LeaveStartTime>NEW.LeaveEndTime,ADDTIME(NEW.LeaveEndTime,'24:00:00'),NEW.LeaveEndTime),NEW.LeaveStartTime), INTERVAL 0 HOUR) INTO reghrsworkd;



SET reghrsworkd = IF('09:00:00' > reghrsworkd, reghrsworkd, ADDDATE(reghrsworkd, INTERVAL -1 HOUR));

SELECT ((TIME_TO_SEC(reghrsworkd) / 60) / 60) INTO numhrsworkd;

/*IF numhrsworkd >= 9 THEN
    SET numhrsworkd = 8;

END IF;*/

SET numhrsworkd = NEW.OfficialValidHours;

UPDATE employeetimeentry et
INNER JOIN employee e ON e.RowID=et.EmployeeID AND e.OrganizationID=et.OrganizationID
INNER JOIN employeesalary es ON es.RowID=et.EmployeeSalaryID
INNER JOIN employeeshift esh ON esh.RowID=et.EmployeeShiftID
INNER JOIN shift sh ON sh.RowID=esh.ShiftID
SET
et.LastUpd=CURRENT_TIMESTAMP()
,et.LastUpdBy=NEW.LastUpdBy
,et.VacationLeaveHours=IF(leavetype LIKE '%Vacation%',numhrsworkd,0.0)
,et.SickLeaveHours=IF(leavetype LIKE '%Sick%',numhrsworkd,0.0)
,et.MaternityLeaveHours=IF(leavetype LIKE '%aternity%',numhrsworkd,0.0)
,et.OtherLeaveHours=IF(leavetype LIKE '%Others%',numhrsworkd,0.0)
,et.Leavepayment = (numhrsworkd * IF(e.EmployeeType = 'Daily'
                                     , (es.BasicPay / sh.DivisorToDailyRate)
												 , ((es.Salary / (e.WorkDaysPerYear / 12)) / sh.DivisorToDailyRate)))
WHERE et.OrganizationID=NEW.OrganizationID
AND et.EmployeeID=NEW.EmployeeID
AND et.`Date` BETWEEN NEW.LeaveStartDate AND NEW.LeaveEndDate;

/*IF OLD.LeaveStartDate = NEW.LeaveStartDate AND OLD.LeaveEndDate = NEW.LeaveEndDate THEN

    IF OLD.LeaveStartTime != NEW.LeaveStartTime OR OLD.LeaveEndTime != NEW.LeaveEndTime THEN

        UPDATE employeetimeentry SET
        LastUpd=CURRENT_TIMESTAMP()
        ,RegularHoursWorked=0.0
        ,VacationLeaveHours=IF(leavetype LIKE '%Vacation%',numhrsworkd,0.0)
        ,SickLeaveHours=IF(leavetype LIKE '%Sick%',numhrsworkd,0.0)
        ,MaternityLeaveHours=IF(leavetype LIKE '%aternity%',numhrsworkd,0.0)
        ,OtherLeaveHours=IF(leavetype LIKE '%Others%',numhrsworkd,0.0)
        WHERE `Date` BETWEEN OLD.LeaveStartDate AND OLD.LeaveEndDate
        AND OrganizationID=OLD.OrganizationID
        AND EmployeeID=OLD.EmployeeID;

    END IF;

ELSE

        SELECT IF(DATEDIFF(NEW.LeaveStartDate,NEW.LeaveEndDate) < 0,DATEDIFF(NEW.LeaveStartDate,NEW.LeaveEndDate) * -1,DATEDIFF(NEW.LeaveStartDate,NEW.LeaveEndDate)) INTO newleavenumdays;

        SELECT IF(DATEDIFF(OLD.LeaveStartDate,OLD.LeaveEndDate) < 0,DATEDIFF(OLD.LeaveStartDate,OLD.LeaveEndDate) * -1,DATEDIFF(OLD.LeaveStartDate,OLD.LeaveEndDate)) INTO oldleavenumdays;

        IF newleavenumdays > oldleavenumdays THEN

            loop1: LOOP

                SELECT ADDDATE(NEW.LeaveStartDate, INTERVAL looper DAY) INTO dateloop;

                SELECT RowID FROM payrate WHERE Date=dateloop AND OrganizationID=NEW.OrganizationID INTO prateID;

                IF looper < oldleavenumdays THEN

                    UPDATE employeetimeentry SET
                    LastUpd=CURRENT_TIMESTAMP()
                    ,Date=dateloop
                    ,VacationLeaveHours=IF(leavetype LIKE '%Vacation%',numhrsworkd,0.0)
                    ,SickLeaveHours=IF(leavetype LIKE '%Sick%',numhrsworkd,0.0)
                    ,MaternityLeaveHours=IF(leavetype LIKE '%aternity%',numhrsworkd,0.0)
                    ,OtherLeaveHours=IF(leavetype LIKE '%Others%',numhrsworkd,0.0)
                    ,PayRateID=prateID
                    WHERE Date=ADDDATE(OLD.LeaveStartDate, INTERVAL looper DAY)
                    AND EmployeeID=OLD.EmployeeID
                    AND OrganizationID=OLD.OrganizationID;

                ELSE

                    UPDATE employeetimeentry SET
                    LastUpd=CURRENT_TIMESTAMP()
                    ,Date=dateloop
                    ,VacationLeaveHours=IF(leavetype LIKE '%Vacation%',numhrsworkd,0.0)
                    ,SickLeaveHours=IF(leavetype LIKE '%Sick%',numhrsworkd,0.0)
                    ,MaternityLeaveHours=IF(leavetype LIKE '%aternity%',numhrsworkd,0.0)
                    ,OtherLeaveHours=IF(leavetype LIKE '%Others%',numhrsworkd,0.0)
                    ,PayRateID=prateID
                    ,EmployeeShiftID=(SELECT RowID FROM employeeshift WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND dateloop BETWEEN DATE(COALESCE(EffectiveFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) ORDER BY DATEDIFF(dateloop,EffectiveFrom) LIMIT 1)
                    ,EmployeeSalaryID=(SELECT RowID FROM employeesalary WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND dateloop BETWEEN DATE(COALESCE(EffectiveDateFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveDateTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) ORDER BY DATEDIFF(dateloop,EffectiveDateFrom) LIMIT 1)
                    WHERE Date=ADDDATE(OLD.LeaveStartDate, INTERVAL looper DAY)
                    AND EmployeeID=OLD.EmployeeID
                    AND OrganizationID=OLD.OrganizationID;

                    INSERT INTO employeetimeentry
                    (
                        OrganizationID
                        ,Created
                        ,CreatedBy
                        ,Date
                        ,EmployeeShiftID
                        ,EmployeeID
                        ,EmployeeSalaryID
                        ,RegularHoursWorked
                        ,PayRateID
                        ,VacationLeaveHours
                        ,SickLeaveHours
                        ,TotalDayPay
                    ) VALUES (
                        NEW.OrganizationID
                        ,CURRENT_TIMESTAMP()
                        ,NEW.CreatedBy
                        ,ADDDATE(dateloop, INTERVAL 1 DAY)
                        ,(SELECT RowID FROM employeeshift WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND dateloop BETWEEN DATE(COALESCE(EffectiveFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) ORDER BY DATEDIFF(dateloop,EffectiveFrom) LIMIT 1)
                        ,NEW.EmployeeID
                        ,(SELECT RowID FROM employeesalary WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND dateloop BETWEEN DATE(COALESCE(EffectiveDateFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveDateTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) ORDER BY DATEDIFF(dateloop,EffectiveDateFrom) LIMIT 1)
                        ,0.0
                        ,prateID
                        ,IF(leavetype LIKE '%Vacation%',numhrsworkd,0.0)
                        ,IF(leavetype LIKE '%Sick%',numhrsworkd,0.0)
                        ,TotalDayPay
                    ) ON
                    DUPLICATE
                    KEY
                    UPDATE
                        LastUpd=CURRENT_TIMESTAMP()
                        ,LastUpdBy=NEW.LastUpdBy
                        ,PayRateID=prateID
                        ,VacationLeaveHours=IF(leavetype LIKE '%Vacation%',numhrsworkd,0.0)
                        ,SickLeaveHours=IF(leavetype LIKE '%Sick%',numhrsworkd,0.0)
                        ,MaternityLeaveHours=IF(leavetype LIKE '%aternity%',numhrsworkd,0.0)
                        ,OtherLeaveHours=IF(leavetype LIKE '%Others%',numhrsworkd,0.0)
                        ,TotalDayPay=TotalDayPay;

                    IF looper + 1 = newleavenumdays THEN
                        SELECT 0 INTO looper;
                        LEAVE loop1;
                    END IF;

                END IF;

                SET looper = looper + 1;

            END LOOP;

        ELSEIF oldleavenumdays > newleavenumdays THEN

            loop2: LOOP

                SELECT ADDDATE(NEW.LeaveStartDate, INTERVAL looper DAY) INTO dateloop;

                SELECT RowID FROM payrate WHERE Date=dateloop AND OrganizationID=NEW.OrganizationID INTO prateID;

                IF looper < newleavenumdays THEN

                    UPDATE employeetimeentry SET
                    LastUpd=CURRENT_TIMESTAMP()
                    ,Date=dateloop
                    ,VacationLeaveHours=IF(leavetype LIKE '%Vacation%',numhrsworkd,0.0)
                    ,SickLeaveHours=IF(leavetype LIKE '%Sick%',numhrsworkd,0.0)
                    ,MaternityLeaveHours=IF(leavetype LIKE '%aternity%',numhrsworkd,0.0)
                    ,OtherLeaveHours=IF(leavetype LIKE '%Others%',numhrsworkd,0.0)
                    ,PayRateID=prateID
                    ,EmployeeShiftID=(SELECT RowID FROM employeeshift WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND dateloop BETWEEN DATE(COALESCE(EffectiveFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) ORDER BY DATEDIFF(dateloop,EffectiveFrom) LIMIT 1)
                    ,EmployeeSalaryID=(SELECT RowID FROM employeesalary WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND dateloop BETWEEN DATE(COALESCE(EffectiveDateFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveDateTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) ORDER BY DATEDIFF(dateloop,EffectiveDateFrom) LIMIT 1)
                    WHERE Date=ADDDATE(OLD.LeaveStartDate, INTERVAL looper DAY)
                    AND EmployeeID=OLD.EmployeeID
                    AND OrganizationID=OLD.OrganizationID;

                    IF looper + 1 = newleavenumdays THEN
                        SELECT 0 INTO looper;
                        LEAVE loop2;
                    END IF;
                END IF;

                SET looper = looper + 1;

            END LOOP;

            DELETE FROM employeetimeentry
            WHERE Date BETWEEN ADDDATE(OLD.LeaveStartDate, INTERVAL newleavenumdays + 1 DAY) AND ADDDATE(OLD.LeaveStartDate, INTERVAL oldleavenumdays DAY)
            AND EmployeeID=OLD.EmployeeID
            AND OrganizationID=OLD.OrganizationID;

        ELSE

            loop3: LOOP

                SELECT ADDDATE(NEW.LeaveStartDate, INTERVAL looper DAY) INTO dateloop;

                SELECT RowID FROM payrate WHERE Date=dateloop AND OrganizationID=NEW.OrganizationID INTO prateID;

                IF looper <= newleavenumdays THEN

                    UPDATE employeetimeentry SET
                    LastUpd=CURRENT_TIMESTAMP()
                    ,VacationLeaveHours=0
                    ,SickLeaveHours=0
                    WHERE Date=ADDDATE(OLD.LeaveStartDate, INTERVAL looper DAY)
                    AND EmployeeID=OLD.EmployeeID
                    AND OrganizationID=OLD.OrganizationID;

                    UPDATE employeetimeentry SET
                    LastUpd=CURRENT_TIMESTAMP()
                    ,VacationLeaveHours=IF(leavetype LIKE '%Vacation%',numhrsworkd,0.0)
                    ,SickLeaveHours=IF(leavetype LIKE '%Sick%',numhrsworkd,0.0)
                    ,MaternityLeaveHours=IF(leavetype LIKE '%aternity%',numhrsworkd,0.0)
                    ,OtherLeaveHours=IF(leavetype LIKE '%Others%',numhrsworkd,0.0)
                    WHERE Date=ADDDATE(NEW.LeaveStartDate, INTERVAL looper DAY)
                    AND EmployeeID=NEW.EmployeeID
                    AND OrganizationID=NEW.OrganizationID;



                ELSE
                    SELECT 0 INTO looper;
                    LEAVE loop3;
                END IF;

                SET looper = looper + 1;

            END LOOP;

        END IF;

END IF;*/

SELECT RowID FROM `view` WHERE ViewName='Employee Leave' AND OrganizationID=NEW.OrganizationID LIMIT 1 INTO viewID;

IF OLD.LeaveType!=NEW.LeaveType THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'LeaveType',NEW.RowID,OLD.LeaveType,NEW.LeaveType,'Update');

END IF;

IF OLD.LeaveStartTime!=NEW.LeaveStartTime THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'LeaveStartTime',NEW.RowID,OLD.LeaveStartTime,NEW.LeaveStartTime,'Update');

END IF;

IF OLD.LeaveEndTime!=NEW.LeaveEndTime THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'LeaveEndTime',NEW.RowID,OLD.LeaveEndTime,NEW.LeaveEndTime,'Update');

END IF;

IF OLD.LeaveStartDate!=NEW.LeaveStartDate THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'LeaveStartDate',NEW.RowID,OLD.LeaveStartDate,NEW.LeaveStartDate,'Update');

END IF;

IF OLD.LeaveEndDate!=NEW.LeaveEndDate THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'LeaveEndDate',NEW.RowID,OLD.LeaveEndDate,NEW.LeaveEndDate,'Update');

END IF;

IF OLD.Reason!=NEW.Reason THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'Reason',NEW.RowID,OLD.Reason,NEW.Reason,'Update');

END IF;

IF OLD.Comments!=NEW.Comments THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'Comments',NEW.RowID,OLD.Comments,NEW.Comments,'Update');

END IF;








END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
