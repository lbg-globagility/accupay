/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTUPD_employeeofficialbusines_then_employeetimeentrydetails`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_employeeofficialbusines_then_employeetimeentrydetails` AFTER UPDATE ON `employeeofficialbusiness` FOR EACH ROW BEGIN



DECLARE eob_dayrange INT(11);

DECLARE prev_dayrange INT(11);

DECLARE i INT(11);

DECLARE viewID INT(11);

DECLARE etetn_RowID INT(11);

DECLARE OLD_OffBusStartDate DATE DEFAULT OLD.OffBusStartDate;

DECLARE one_datetimestamp DATETIME DEFAULT CURRENT_TIMESTAMP();

SET prev_dayrange = COALESCE(DATEDIFF(COALESCE(OLD.OffBusEndDate,OLD.OffBusStartDate),OLD.OffBusStartDate),0) + 1;

SET eob_dayrange = COALESCE(DATEDIFF(COALESCE(NEW.OffBusEndDate,NEW.OffBusStartDate),NEW.OffBusStartDate),0) + 1;

SET i=0;

IF (OLD.OffBusStatus != 'Approved' AND NEW.OffBusStatus = 'Approved')
    AND OLD.OffBusStartTime != NEW.OffBusStartTime
    AND OLD.OffBusEndTime != NEW.OffBusEndTime
    AND OLD.OffBusStartDate != NEW.OffBusStartDate
    AND OLD.OffBusEndDate != NEW.OffBusEndDate THEN

    IF NEW.OffBusStatus = 'Approved' THEN

        SELECT CURRENT_TIMESTAMP() INTO one_datetimestamp;

        INSERT INTO employeetimeentrydetails
        (
            RowID
            ,OrganizationID
            ,Created
            ,CreatedBy
            ,EmployeeID
            ,TimeIn
            ,TimeOut
            ,`Date`
            ,TimeScheduleType
            ,TimeEntryStatus
        ) SELECT etd.RowID
            ,NEW.OrganizationID
            ,one_datetimestamp
            ,NEW.CreatedBy
            ,NEW.EmployeeID
            ,NEW.OffBusStartTime
            ,NEW.OffBusEndTime
            ,d.DateValue
            ,''
            ,''
            FROM dates d
            LEFT JOIN employeetimeentrydetails etd ON etd.EmployeeID=NEW.EmployeeID AND etd.OrganizationID=NEW.OrganizationID AND etd.`Date`=d.DateValue
            WHERE d.DateValue BETWEEN NEW.OffBusStartDate AND NEW.OffBusEndDate
        ON
        DUPLICATE
        KEY
        UPDATE
            LastUpd = CURRENT_TIMESTAMP()
            ,LastUpdBy = NEW.CreatedBy
            ,TimeIn = IFNULL(NEW.OffBusStartTime,etd.TimeIn)
            ,TimeOut = IFNULL(NEW.OffBusEndTime,etd.TimeOut);

    END IF;

ELSEIF prev_dayrange > eob_dayrange AND NEW.OffBusStatus = 'Approved' THEN

    simple_loop: LOOP

        IF i >= prev_dayrange THEN
            LEAVE simple_loop;
        ELSE

            SELECT SUM(RowID) FROM employeetimeentrydetails WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND Date=DATE_ADD(NEW.OffBusStartDate, INTERVAL i DAY) LIMIT 1 INTO etetn_RowID;

            IF i < eob_dayrange THEN
                UPDATE employeetimeentrydetails SET
                LastUpd=one_datetimestamp
                ,LastUpdBy=NEW.LastUpdBy
                ,TimeIn=NEW.OffBusStartTime
                ,TimeOut=NEW.OffBusEndTime
                ,Date=DATE_ADD(NEW.OffBusStartDate, INTERVAL i DAY)
                ,TimeScheduleType='F'
                ,TimeEntryStatus=IF(NEW.OffBusStartTime IS NULL,'missing clock in',IF(NEW.OffBusEndTime IS NULL,'missing clock out','')) WHERE RowID=etetn_RowID;
            ELSE
                DELETE FROM employeetimeentrydetails WHERE RowID=etetn_RowID;
            END IF;

        END IF;

        SET i=i+1;

    END LOOP simple_loop;

ELSEIF prev_dayrange = eob_dayrange AND NEW.OffBusStatus = 'Approved' THEN

simple_loop: LOOP

    IF i >= eob_dayrange THEN
        LEAVE simple_loop;
    ELSE

        SELECT SUM(RowID) FROM employeetimeentrydetails WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND Date=DATE_ADD(NEW.OffBusStartDate, INTERVAL i DAY) LIMIT 1 INTO etetn_RowID;

        INSERT INTO employeetimeentrydetails
        (
            RowID
            ,OrganizationID
            ,Created
            ,CreatedBy
            ,EmployeeID
            ,TimeIn
            ,TimeOut
            ,Date
            ,TimeScheduleType
            ,TimeEntryStatus
        ) VALUES (
            COALESCE(etetn_RowID,NULL)
            ,NEW.OrganizationID
            ,one_datetimestamp
            ,NEW.CreatedBy
            ,NEW.EmployeeID
            ,NEW.OffBusStartTime
            ,NEW.OffBusEndTime
            ,DATE_ADD(NEW.OffBusStartDate, INTERVAL i DAY)
            ,'F'
            ,IF(NEW.OffBusStartTime IS NULL,'missing clock in',IF(NEW.OffBusEndTime IS NULL,'missing clock out',''))
        ) ON
        DUPLICATE
        KEY
        UPDATE
            LastUpd=one_datetimestamp
            ,LastUpdBy=NEW.LastUpdBy
            ,TimeIn=NEW.OffBusStartTime
            ,TimeOut=NEW.OffBusEndTime
            ,Date=DATE_ADD(NEW.OffBusStartDate, INTERVAL i DAY)
            ,TimeScheduleType='F'
            ,TimeEntryStatus=IF(NEW.OffBusStartTime IS NULL,'missing clock in',IF(NEW.OffBusEndTime IS NULL,'missing clock out',''));

    END IF;

    SET i=i+1;

END LOOP simple_loop;

END IF;





SELECT RowID FROM `view` WHERE ViewName='Official Business filing' AND OrganizationID=NEW.OrganizationID LIMIT 1 INTO viewID;

IF OLD.OffBusStartTime!=NEW.OffBusStartTime THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'OffBusStartTime',NEW.RowID,OLD.OffBusStartTime,NEW.OffBusStartTime,'Update');

END IF;

IF OLD.OffBusEndTime!=NEW.OffBusEndTime THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'OffBusEndTime',NEW.RowID,OLD.OffBusEndTime,NEW.OffBusEndTime,'Update');

END IF;

IF OLD.OffBusType!=NEW.OffBusType THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'OffBusType',NEW.RowID,OLD.OffBusType,NEW.OffBusType,'Update');

END IF;

IF OLD.OffBusStartDate!=NEW.OffBusStartDate THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'OffBusStartDate',NEW.RowID,OLD.OffBusStartDate,NEW.OffBusStartDate,'Update');

END IF;

IF OLD.OffBusEndDate!=NEW.OffBusEndDate THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'OffBusEndDate',NEW.RowID,OLD.OffBusEndDate,NEW.OffBusEndDate,'Update');

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
