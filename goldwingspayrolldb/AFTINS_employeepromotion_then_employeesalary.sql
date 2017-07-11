/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTINS_employeepromotion_then_employeesalary`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_employeepromotion_then_employeesalary` AFTER INSERT ON `employeepromotions` FOR EACH ROW BEGIN


DECLARE marit_stat VARCHAR(50);

DECLARE numofdepends INT(11);

DECLARE positID INT(11);

DECLARE empstartdate DATE;

DECLARE payfreqID INT(11);

DECLARE prevesalRowID INT(11);

DECLARE empBasicPay DECIMAL(11,6);

DECLARE emptype VARCHAR(50);

DECLARE viewID INT(11);

    SELECT IF(MaritalStatus IN ('Single','Married'),MaritalStatus,'Zero'),NoOfDependents,PositionID,StartDate,EmployeeType FROM employee WHERE RowID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID INTO marit_stat,numofdepends,positID,empstartdate,emptype;

    SELECT PayFrequencyID FROM organization WHERE RowID=NEW.OrganizationID INTO payfreqID;

IF NEW.CompensationChange = '1' THEN


    UPDATE employeesalary es
    INNER JOIN (SELECT * FROM employeesalary WHERE OrganizationID=NEW.OrganizationID AND EmployeeID=NEW.EmployeeID AND RowID != IFNULL(NEW.EmployeeSalaryID,0) ORDER BY EffectiveDateFrom DESC LIMIT 1) ees ON ees.RowID > 0
    SET es.EffectiveDateTo=SUBDATE(NEW.EffectiveDate, INTERVAL 1 DAY)
    ,es.LastUpd=CURRENT_TIMESTAMP()
    ,es.LastUpdBy=NEW.CreatedBy
    WHERE es.RowID = ees.RowID;

ELSEIF NEW.CompensationChange = -1 THEN

    SELECT RowID,BasicPay FROM employeesalary WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND DATE(DATE_FORMAT(CURRENT_DATE(),'%Y-%m-%d')) BETWEEN DATE(COALESCE(EffectiveDateFrom,DATE_FORMAT(CURRENT_DATE(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveDateTo,ADDDATE(CURRENT_DATE(), INTERVAL 1 MONTH))) AND DATEDIFF(CURRENT_DATE(),EffectiveDateFrom) >= 0 ORDER BY DATEDIFF(CURRENT_DATE(),EffectiveDateFrom) LIMIT 1 INTO prevesalRowID,empBasicPay;

    UPDATE employeesalary SET
    EffectiveDateTo=ADDDATE(NEW.EffectiveDate, INTERVAL -1 DAY)
    WHERE RowID=prevesalRowID;

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
    ) VALUES(
        NEW.EmployeeID
        ,CURRENT_TIMESTAMP()
        ,NEW.CreatedBy
        ,NEW.OrganizationID
        ,100
        ,empBasicPay
        ,IF(emptype IN ('Daily','Hourly'), empBasicPay, IF(payfreqID = 1, empBasicPay * 2, empBasicPay))
        ,IF(emptype = 'Daily', empBasicPay, 0)
        ,IF(emptype = 'Hourly', empBasicPay, 0)
        ,(SELECT RowID FROM filingstatus WHERE MaritalStatus=marit_stat AND Dependent=COALESCE(numofdepends,0))
        ,COALESCE(numofdepends,0)
        ,marit_stat
        ,(SELECT RowID FROM `position` WHERE PositionName=NEW.PositionTo AND OrganizationID=NEW.OrganizationID LIMIT 1)
        ,NEW.EffectiveDate
    ) ON
    DUPLICATE
    KEY
    UPDATE
        LastUpdBy=NEW.CreatedBy
        ,LastUpd=CURRENT_TIMESTAMP();



ELSEIF COALESCE((SELECT PositionName FROM `position` WHERE RowID=positID AND OrganizationID=NEW.OrganizationID LIMIT 1),'') != NEW.PositionTo THEN

    UPDATE employee SET
    PositionID=(SELECT RowID FROM `position` WHERE PositionName=NEW.PositionTo AND OrganizationID=NEW.OrganizationID LIMIT 1)
    WHERE RowID=NEW.EmployeeID;

END IF;

SELECT RowID FROM `view` WHERE ViewName='Employee Promotion' AND OrganizationID=NEW.OrganizationID LIMIT 1 INTO viewID;

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EmployeeID',NEW.RowID,'',NEW.EmployeeID,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'PositionFrom',NEW.RowID,'',NEW.PositionFrom,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'PositionTo',NEW.RowID,'',NEW.PositionTo,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EffectiveDate',NEW.RowID,'',NEW.EffectiveDate,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'CompensationChange',NEW.RowID,'',NEW.CompensationChange,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EmployeeSalaryID',NEW.RowID,'',NEW.EmployeeSalaryID,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'Reason',NEW.RowID,'',NEW.Reason,'Insert');




END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
