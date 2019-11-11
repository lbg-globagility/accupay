/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `U_listofvalues`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `U_listofvalues`(IN `U_RowID` INT(10), IN `U_LastUpd` DATETIME, IN `U_LastUpdBy` VARCHAR(50)
, IN `U_DisplayValue` VARCHAR(100), IN `U_LIC` VARCHAR(100), IN `U_Type` VARCHAR(50), IN `U_ParentLIC` VARCHAR(50), IN `U_Status` VARCHAR(50), IN `U_Description` VARCHAR(2000), IN `U_SystemAccountFlg` CHAR(10), IN `U_DisplayAccountFlg` CHAR(10), IN `U_OrderBy` INT(10))
    DETERMINISTIC
BEGIN
UPDATE listofval Set
    LastUpd = U_LastUpd,
    LastUpdBy = U_LastUpdBy,
    DisplayValue = U_DisplayValue,
    LIC = U_LIC,
    Type = U_Type,
    ParentLIC = U_ParentLIC,
    Status = U_Status,
    Description = U_Description,
    SystemAccountFlg = U_SystemAccountFlg,
    DisplayAccountFlg = U_DisplayAccountFlg,
    OrderBy = U_OrderBy
WHERE RowID = U_RowID;
END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
