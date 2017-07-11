/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `I_ListValue`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_ListValue`(IN `I_DisplayValue` VARCHAR(50), IN `I_LIC` VARCHAR(50), IN `I_Type` VARCHAR(50), IN `I_ParentLIC` VARCHAR(50), IN `I_Active` VARCHAR(50), IN `I_Description` VARCHAR(500), IN `I_Created` DATETIME, IN `I_CreatedBy` VARCHAR(50), IN `I_LastUpd` DATETIME, IN `I_OrderBy` INT, IN `I_LastUpdBy` VARCHAR(50)
)
    DETERMINISTIC
BEGIN

INSERT INTO listofval
(
 DisplayValue,
 LIC,
 Type,
 ParentLIC,
 Active,
 Description,
 Created,
 CreatedBy,
 LastUpd,
 OrderBy,
 LastUpdBy
)
VALUES
(
 I_DisplayValue,
 I_LIC,
 I_Type,
 I_ParentLIC,
 I_Active,
 I_Description,
 I_Created,
 I_CreatedBy,
 I_LastUpd,
 I_OrderBy,
 I_LastUpdBy
);END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
