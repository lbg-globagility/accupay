/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTINS_employeeshift`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_employeeshift` AFTER INSERT ON `employeeshift` FOR EACH ROW BEGIN

DECLARE viewID INT(11);

DECLARE emp_group_name VARCHAR(50);

DECLARE anyint INT(11);

DECLARE sh_timefrom TIME;

DECLARE sh_timeto TIME;

DECLARE isShiftRestDay CHAR(1);

DECLARE day_of_rest CHAR(1);

DECLARE has_already_OTapproved CHAR(1);


SELECT d.Name
,e.DayOfRest
FROM employee e
INNER JOIN position p ON p.RowID=e.PositionID
INNER JOIN `division` d ON d.RowID=p.DivisionId
WHERE e.RowID=NEW.EmployeeID
INTO emp_group_name
        ,day_of_rest;

IF emp_group_name = 'Comissary' THEN

    SELECT
    sh.TimeFrom
    ,sh.TimeTo
    FROM shift sh
    WHERE sh.RowID=NEW.ShiftID
    INTO sh_timefrom
            ,sh_timeto;

    SET isShiftRestDay = NEW.RestDay;



    IF isShiftRestDay IS NOT NULL THEN

        SET isShiftRestDay = NEW.RestDay;

        INSERT INTO employeeovertime(RowID,OrganizationID,Created,OTStartTime,OTType,OTStatus,CreatedBy,EmployeeID,OTEndTime,OTStartDate,OTEndDate,Reason,Comments,Image)
            SELECT
            eot.RowID
            ,NEW.OrganizationID
            ,CURRENT_TIMESTAMP()
            ,ADDTIME(sh_timeto,'00:01:00')
            ,'Overtime'
            ,'Approved'
            ,NEW.CreatedBy
            ,NEW.EmployeeID
            ,etd.TimeOut
            ,d.DateValue
            ,d.DateValue
            ,''
            ,''
            ,NULL
        FROM dates d
        LEFT JOIN employeeovertime eot ON eot.EmployeeID=NEW.EmployeeID AND eot.OrganizationID=NEW.OrganizationID AND d.DateValue BETWEEN eot.OTStartDate AND eot.OTEndDate
        INNER JOIN employeetimeentrydetails etd ON etd.EmployeeID=NEW.EmployeeID AND etd.OrganizationID=NEW.OrganizationID AND etd.`Date`=d.DateValue
        WHERE DAYOFWEEK(d.DateValue) != '1'
        AND TIME(ADDTIME(sh_timeto,'00:15:59')) < etd.TimeOut
        AND d.DateValue BETWEEN NEW.EffectiveFrom AND NEW.EffectiveTo
        ORDER BY d.DateValue
        ON
        DUPLICATE
        KEY
        UPDATE
            LastUpd=CURRENT_TIMESTAMP();


    END IF;

END IF;






















SELECT RowID FROM `view` WHERE ViewName='Employee Shift' AND OrganizationID=NEW.OrganizationID LIMIT 1 INTO viewID;

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EmployeeID',NEW.RowID,'',NEW.EmployeeID,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'ShiftID',NEW.RowID,'',NEW.ShiftID,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EffectiveFrom',NEW.RowID,'',NEW.EffectiveFrom,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EffectiveTo',NEW.RowID,'',NEW.EffectiveTo,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'NightShift',NEW.RowID,'',NEW.NightShift,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'RestDay',NEW.RowID,'',NEW.RestDay,'Insert');


UPDATE employeetimeentry ete
SET ete.EmployeeShiftID=NEW.RowID
,ete.LastUpd=CURRENT_TIMESTAMP()
,ete.LastUpdBy=NEW.CreatedBy
WHERE ete.EmployeeID=NEW.EmployeeID
AND ete.OrganizationID=NEW.OrganizationID
AND ete.`Date` BETWEEN NEW.EffectiveFrom AND NEW.EffectiveTo;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
