/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_employeeshiftbyday_secondary`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `INSUPD_employeeshiftbyday_secondary`(`OrganizID` INT, `EmpRowID` INT, `ShiftRowIDID` INT, `_NameOfDay` VARCHAR(50), `_SampleDate` DATE, `_NightShift` CHAR(1), `_RestDay` CHAR(1), `_IsEncodedByDay` CHAR(1), `_OrderByValue` INT, `_OriginDay` INT, `_UniqueShift` INT) RETURNS timestamp
    DETERMINISTIC
BEGIN

DECLARE returnvalue TIMESTAMP;
SELECT CURRENT_TIMESTAMP() INTO returnvalue;
INSERT INTO employeeshiftbyday_secondary(OrganizationID,EmployeeID,ShiftID,NameOfDay,SampleDate,NightShift,RestDay,IsEncodedByDay,OrderByValue,OriginDay,UniqueShift) VALUES (OrganizID,EmpRowID,ShiftRowIDID,_NameOfDay,_SampleDate,_NightShift,_RestDay,_IsEncodedByDay,_OrderByValue,_OriginDay,_UniqueShift) ON DUPLICATE KEY UPDATE OrganizationID=OrganizID;
RETURN returnvalue;
END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
