/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTINS_employeeofficialbusiness_then_employeetimeentrydetails`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_employeeofficialbusiness_then_employeetimeentrydetails` AFTER INSERT ON `employeeofficialbusiness` FOR EACH ROW BEGIN

DECLARE eob_dayrange INT(11);

DECLARE i INT(11) DEFAULT 0;

DECLARE etetn_RowID INT(11);

DECLARE one_datetimestamp DATETIME DEFAULT CURRENT_TIMESTAMP();

DECLARE viewID INT(11);

SET eob_dayrange = DATEDIFF(COALESCE(NEW.OffBusEndDate,NEW.OffBusStartDate),NEW.OffBusStartDate) + 1;
SET one_datetimestamp = (SELECT etd.Created FROM employeetimeentrydetails etd INNER JOIN (SELECT pp.RowID,pp.PayFromDate, pp.PayToDate FROM employee e INNER JOIN payperiod pp ON pp.TotalGrossSalary=e.PayFrequencyID AND pp.OrganizationID=e.OrganizationID AND NEW.OffBusStartDate BETWEEN pp.PayFromDate AND pp.PayToDate WHERE e.RowID=NEW.EmployeeID AND e.OrganizationID=NEW.OrganizationID LIMIT 1) i ON i.RowID IS NOT NULL OR i.RowID IS NULL WHERE etd.EmployeeID=NEW.EmployeeID AND etd.OrganizationID=NEW.OrganizationID AND etd.`Date` BETWEEN i.PayFromDate AND i.PayToDate LIMIT 1);
SET one_datetimestamp = IFNULL(one_datetimestamp,CURRENT_TIMESTAMP());
SET i=0;

SELECT RowID FROM `view` WHERE ViewName='Official Business filing' AND OrganizationID=NEW.OrganizationID LIMIT 1 INTO viewID;

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'OffBusStartTime',NEW.RowID,'',NEW.OffBusStartTime,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'OffBusEndTime',NEW.RowID,'',NEW.OffBusEndTime,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'OffBusType',NEW.RowID,'',NEW.OffBusType,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'OffBusStartDate',NEW.RowID,'',NEW.OffBusStartDate,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'OffBusEndDate',NEW.RowID,'',NEW.OffBusEndDate,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'Reason',NEW.RowID,'',NEW.Reason,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'Comments',NEW.RowID,'',NEW.Comments,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EmployeeID',NEW.RowID,'',NEW.EmployeeID,'Insert');







END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
