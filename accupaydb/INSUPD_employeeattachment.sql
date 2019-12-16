/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_employeeattachment`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_employeeattachment`(`eatta_RowID` INT, `eatta_EmployeeID` INT, `eatta_CreatedBy` INT, `eatta_LastUpdBy` INT, `eatta_Type` VARCHAR(100), `eatta_FileName` VARCHAR(100), `eatta_FileType` VARCHAR(100), `eatta_AttachedFile` LONGBLOB) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE empattaID INT(11);



INSERT INTO employeeattachments
(
    RowID
    ,`Type`
    ,FileName
    ,FileType
    ,EmployeeID
    ,Created
    ,CreatedBy
    ,AttachedFile
) VALUES (
    eatta_RowID
    ,eatta_Type
    ,eatta_FileName
    ,eatta_FileType
    ,eatta_EmployeeID
    ,CURRENT_TIMESTAMP()
    ,eatta_CreatedBy
    ,eatta_AttachedFile
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=eatta_LastUpdBy
    ,`Type`=eatta_Type
    ,FileName=eatta_FileName
    ,FileType=eatta_FileType
    ,AttachedFile=eatta_AttachedFile;SELECT @@Identity AS id INTO empattaID;

RETURN empattaID;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
