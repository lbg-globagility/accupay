/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_employeeOT_indepen`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_employeeOT_indepen`(`eot_RowID` INT, `eot_OrganizationID` INT, `eot_CreatedBy` INT, `eot_LastUpdBy` INT, `eot_EmployeeID` INT, `eot_OTType` VARCHAR(50), `eot_OTStartTime` TIME, `eot_OTEndTime` TIME, `eot_OTStartDate` DATE, `eot_OTEndDate` DATE, `eot_OTStatus` VARCHAR(50), `eot_Reason` VARCHAR(500), `eot_Comments` VARCHAR(2000), `eot_Image` LONGBLOB) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE eot_ID INT(11);

DECLARE othrscount DECIMAL(11,2);

DECLARE endovertime TIME;



SET endovertime = IF(HOUR(eot_OTEndTime) = 24, TIME_FORMAT(eot_OTEndTime,'00:%i:%s'), eot_OTEndTime);

INSERT INTO employeeovertime
(
    RowID
    ,OrganizationID
    ,Created
    ,CreatedBy
    ,EmployeeID
    ,OTType
    ,OTStartTime
    ,OTEndTime
    ,OTStartDate
    ,OTEndDate
    ,OTStatus
    ,Reason
    ,Comments
    ,Image
) VALUES (
    eot_RowID
    ,eot_OrganizationID
    ,CURRENT_TIMESTAMP()
    ,DEFAULT_internal_sys_user() # IFNULL(DEFAULT_internal_sys_user(), eot_CreatedBy)
    ,eot_EmployeeID
    ,eot_OTType
    ,eot_OTStartTime
    ,endovertime
    ,eot_OTStartDate
    ,eot_OTEndDate
    ,eot_OTStatus
    ,eot_Reason
    ,eot_Comments
    ,eot_Image
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=eot_LastUpdBy
    ,OTType=eot_OTType
    ,OTStartTime=eot_OTStartTime
    ,OTEndTime=endovertime
    ,OTStartDate=eot_OTStartDate
    ,OTEndDate=eot_OTEndDate
    ,OTStatus=eot_OTStatus
    ,Reason=eot_Reason
    ,Comments=eot_Comments
    ,Image=eot_Image;SELECT @@Identity AS id INTO eot_ID;



RETURN eot_ID;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
