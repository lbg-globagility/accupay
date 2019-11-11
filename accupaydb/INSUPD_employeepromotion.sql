/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_employeepromotion`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `INSUPD_employeepromotion`(`epro_RowID` INT, `OrganizID` INT, `UserRowID` INT, `epro_EmpRowID` INT, `epro_PositionFrom` VARCHAR(50), `epro_PositionTo` VARCHAR(50), `epro_EffectiveDate` DATE, `epro_CompensationChange` CHAR(1), `epro_CompensationValue` DECIMAL(11,2), `epro_EmpSalID` INT, `epro_Reason` VARCHAR(200)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11);



INSERT INTO employeepromotions
(
    RowID
    ,OrganizationID
    ,Created
    ,CreatedBy
    ,EmployeeID
    ,PositionFrom
    ,PositionTo
    ,EffectiveDate
    ,CompensationChange
    ,CompensationValue
    ,Reason
) VALUES (
    epro_RowID
    ,OrganizID
    ,CURRENT_TIMESTAMP()
    ,UserRowID
    ,epro_EmpRowID
    ,epro_PositionFrom
    ,epro_PositionTo
    ,epro_EffectiveDate
    ,epro_CompensationChange
    ,epro_CompensationValue
    ,IFNULL(epro_Reason,'')
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP();SELECT @@Identity AS ID INTO returnvalue;

RETURN returnvalue;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
