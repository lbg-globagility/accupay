/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `sp_emppromotion`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_emppromotion`(IN `I_OrganizationID` INT(10), IN `I_Created` DATETIME, IN `I_CreatedBy` INT(10), IN `I_LastUpd` DATETIME, IN `I_LastUpdby` INT(10), IN `I_EmployeeID` INT(10), IN `I_PositionFrom` VARCHAR(50), IN `I_PositionTo` VARCHAR(50), IN `I_EffectiveDate` DATE, IN `I_CompensationChange` CHAR(1), IN `I_EmployeeSalaryID` INT(11), IN `I_NewAmount` DECIMAL(11,2))
    DETERMINISTIC
BEGIN

DECLARE emp_numdepend INT(11);

DECLARE emp_maritstats VARCHAR(50);

INSERT INTO employeepromotions
 (
    OrganizationID,
    Created,
    CreatedBy,
    LastUpd,
    LastUpdBy,
    EmployeeID,
    PositionFrom,
    PositionTo,
    EffectiveDate,
    CompensationChange,
    EmployeeSalaryID,
    NewAmount
)
VALUES
(
    I_OrganizationID,
    I_Created,
    I_CreatedBy,
    I_LastUpd,
    I_LastUpdBy,
    I_EmployeeID,
    I_PositionFrom,
    I_PositionTo,
    I_EffectiveDate,
    I_CompensationChange,
    IF(COALESCE(I_EmployeeSalaryID,0) = 0, NULL, I_EmployeeSalaryID),
    I_NewAmount
);END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
