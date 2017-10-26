/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `SP_payfrequency`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_payfrequency`(IN `I_CreatedBy` INT(10), IN `I_LastupdBy` INT(10), IN `I_Created` DATETIME, IN `I_LastUpd` DATETIME, IN `I_PayFrequencyType` VARCHAR(50), IN `I_PayFrequencyStartDate` DATE)
    DETERMINISTIC
BEGIN
INSERT INTO payfrequency
(
CreatedBy,
LastUpdby,
Created,
LastUpd,
`PayFrequencyType`,
PayFrequencyStartDate
)
VALUES
(
I_CreatedBy,
I_LastUpdby,
I_Created,
I_LastUpd,
I_PayFrequencyType,
I_PayFrequencyStartDate
);
END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
