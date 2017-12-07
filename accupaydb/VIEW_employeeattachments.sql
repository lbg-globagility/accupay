/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_employeeattachments`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeeattachments`(IN `EmpID` INT)
    DETERMINISTIC
BEGIN

SELECT RowID 'eatt_RowID'
,COALESCE(`Type`,'') 'eatt_Type'
,COALESCE(FileName,'') 'eatt_FileName'
,COALESCE(FileType,'') 'eatt_FileType'
,EmployeeID 'eatt_EmployeeID'
,Created 'eatt_Created'
,CreatedBy 'eatt_CreatedBy'
,COALESCE(LastUpd,'') 'eatt_LastUpd'
,COALESCE(LastUpdBy,'') 'eatt_LastUpdBy'
,COALESCE(AttachedFile,'') 'eatt_AttachedFile'
,'view this'
,COALESCE(`Type`,'')
FROM employeeattachments
WHERE EmployeeID=EmpID
AND LOCATE('@',`Type`) <= 0;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
