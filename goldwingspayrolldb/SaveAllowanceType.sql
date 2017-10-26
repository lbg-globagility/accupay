/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `SaveAllowanceType`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `SaveAllowanceType`(IN `ProductID` INT, IN `IsRegularDayProrated` INT, IN `IsRestDayProrated` INT, IN `IsSpecialHolidayProrated` INT)
BEGIN

    INSERT INTO allowancetype
    (
        allowancetype.ProductID,
        allowancetype.IsRegularDayProrated,
        allowancetype.IsRestDayProrated,
        allowancetype.IsSpecialHolidayProrated
    )
    VALUES
    (
        ProductID,
        IsRegularDayProrated,
        IsRestDayProrated,
        IsSpecialHolidayProrated
    );

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
