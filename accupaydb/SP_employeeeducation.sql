/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `SP_employeeeducation`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_employeeeducation`(IN `I_DateFrom` VARCHAR(100), IN `I_OrganizationID` INT(10), IN `I_EmployeeID` INT(10), IN `I_DateTo` VARCHAR(50), IN `I_School` VARCHAR(100), IN `I_Degree` VARCHAR(100), IN `I_Course` VARCHAR(100), IN `I_Minor` VARCHAR(100), IN `I_EducationType` VARCHAR(100), IN `I_Remarks` VARCHAR(1000), IN `I_Created` DATETIME, IN `I_CreatedBy` INT(11), IN `I_LastUpd` DATETIME, IN `I_LastUpdBy` INT(11))
    DETERMINISTIC
BEGIN
INSERT INTO `employeeeducation` (
    DateFrom,
    OrganizationID,
    EmployeeID,
    DateTo,
    School,
    Degree,
    Course,
    Minor,
    EducationType,
    Remarks,
    Created,
    CreatedBy,
    LastUpd,
    LastUpdBy
)
VALUES
(
    I_DateFrom,
    I_OrganizationID,
    I_EmployeeID,
    I_DateTo,
    I_School,
    I_Degree,
    I_Course,
    I_Minor,
    I_EducationType,
    I_Remarks,
    I_Created,
    I_CreatedBy,
    I_LastUpd,
    I_LastUpdBy
);


END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
