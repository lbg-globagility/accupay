/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_productmedrec`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_productmedrec`(IN `pmed_OrganizationID` INT, IN `pmed_CategoryID` INT)
    DETERMINISTIC
    COMMENT 'view the medical illness of an employee according to date from-to of employee''s medical record(s) and organization'
BEGIN

SELECT
p.RowID
,p.PartNo
FROM product p
LEFT JOIN category cat ON cat.RowID=p.CategoryID
WHERE cat.OrganizationID=pmed_OrganizationID
AND p.CategoryID=pmed_CategoryID
ORDER BY p.PartNo ASC;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
