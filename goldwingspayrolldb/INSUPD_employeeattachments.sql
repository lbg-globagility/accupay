/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_employeeattachments`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_employeeattachments`(`eatta_RowID` INT, `eatta_CreatedBy` INT, `eatta_LastUpdBy` INT, `eatta_EmployeeID` INT, `eatta_Type` VARCHAR(50), `eatta_AttachedFile` LONGBLOB, `eatta_FileType` VARCHAR(50), `eatta_FileName` VARCHAR(50)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE eatta_ID INT(11);

DECLARE record_attaID INT(11) DEFAULT NULL;

SELECT RowID FROM employeeattachments WHERE eatta_EmployeeID=eatta_EmployeeID AND `Type`=eatta_Type LIMIT 1 INTO record_attaID;

INSERT INTO employeeattachments
(
    RowID
    ,CreatedBy
    ,EmployeeID
    ,`Type`
    ,Created
    ,AttachedFile
    ,FileType
    ,FileName
) VALUES (
    IF(eatta_RowID IS NULL,record_attaID,eatta_RowID)
    ,eatta_CreatedBy
    ,eatta_EmployeeID
    ,eatta_Type
    ,CURRENT_TIMESTAMP()
    ,eatta_AttachedFile
    ,eatta_FileType
    ,eatta_FileName
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=eatta_LastUpdBy
    ,FileType=eatta_FileType
    ,FileName=eatta_FileName;SELECT @@Identity AS id INTO eatta_ID;

RETURN eatta_ID;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
