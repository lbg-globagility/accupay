/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_employeeoffbusi_indepen`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_employeeoffbusi_indepen`(`obf_RowID` INT, `obf_OrganizationID` INT, `obf_CreatedBy` INT, `obf_LastUpdBy` INT, `obf_EmployeeID` INT, `obf_Type` VARCHAR(50), `obf_StartTime` VARCHAR(50), `obf_EndTime` VARCHAR(50), `obf_StartDate` DATE, `obf_EndDate` DATE, `obf_Reason` VARCHAR(500), `obf_Comments` VARCHAR(2000), `obf_Image` LONGBLOB, `obf_OffBusStatus` VARCHAR(500)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE obf_ID INT(11);

SET obf_StartTime = STR_TO_DATE(obf_StartTime, '%r');
SET obf_EndTime = STR_TO_DATE(obf_EndTime, '%r');

INSERT INTO employeeofficialbusiness
(
    RowID
    ,OrganizationID
    ,Created
    ,OffBusStartTime
    ,OffBusType
    ,CreatedBy
    ,EmployeeID
    ,OffBusEndTime
    ,OffBusStartDate
    ,OffBusEndDate
    ,Reason
    ,Comments
    ,OffBusStatus
    ,Image
) VALUES (
    obf_RowID
    ,obf_OrganizationID
    ,CURRENT_TIMESTAMP()
    ,obf_StartTime
    ,obf_Type
    ,DEFAULT_internal_sys_user() # IFNULL(DEFAULT_internal_sys_user(), obf_CreatedBy)
    ,obf_EmployeeID
    ,obf_EndTime
    ,obf_StartDate
    ,obf_EndDate
    ,obf_Reason
    ,obf_Comments
    ,obf_OffBusStatus
    ,obf_Image
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=obf_LastUpdBy
    ,OffBusStartTime=obf_StartTime
    ,OffBusType=obf_Type
    ,OffBusEndTime=obf_EndTime
    ,OffBusStartDate=obf_StartDate
    ,OffBusEndDate=obf_EndDate
    ,Reason=obf_Reason
    ,Comments=obf_Comments
    ,OffBusStatus=obf_OffBusStatus
    ,Image=obf_Image;SELECT @@Identity AS id INTO obf_ID;

RETURN obf_ID;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
