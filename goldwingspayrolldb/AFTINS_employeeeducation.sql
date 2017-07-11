/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTINS_employeeeducation`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_employeeeducation` AFTER INSERT ON `employeeeducation` FOR EACH ROW BEGIN

DECLARE viewID INT(11);

SELECT RowID FROM `view` WHERE ViewName='Employee Educational Background' AND OrganizationID=NEW.OrganizationID LIMIT 1 INTO viewID;

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EmployeeID',NEW.RowID,'',NEW.EmployeeID,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'DateFrom',NEW.RowID,'',NEW.DateFrom,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'DateTo',NEW.RowID,'',NEW.DateTo,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'School',NEW.RowID,'',NEW.School,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'Degree',NEW.RowID,'',NEW.Degree,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'Course',NEW.RowID,'',NEW.Course,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'Minor',NEW.RowID,'',NEW.Minor,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EducationType',NEW.RowID,'',NEW.EducationType,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'Remarks',NEW.RowID,'',NEW.Remarks,'Insert');



END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
