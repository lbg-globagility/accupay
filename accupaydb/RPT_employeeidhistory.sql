/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_employeeidhistory`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_employeeidhistory`(IN `og_rowid` INT)
BEGIN

SELECT i.EmployeeRowID `DatCol1`
, (@mid_init := LEFT(i.MiddleName, 1)) `MiddleInitial`
, CONCAT_WS(', ', i.LastName, i.FirstName, IF(LENGTH(@mid_init) = 0, NULL, CONCAT(@mid_init, '.'))) `DatCol2`
, i.NewValue `DatCol3`
, i.ActionPerformed `DatCol4`
, '' `DatCol50`
FROM employeeidhistory i
WHERE i.OrganizationID = og_rowid
ORDER BY CONCAT(i.LastName, i.FirstName, i.MiddleName)
         , i.Created
;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
