/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_EmployeeOffenses`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_EmployeeOffenses`(IN `organizationID` INT, IN `dateFrom` DATE, IN `dateTo` DATE)
    DETERMINISTIC
BEGIN

SELECT
    ee.EmployeeID `DatCol1`,
    CONCAT(ee.LastName, ', ', ee.FirstName, ' ', INITIALS(ee.MiddleName)) `DatCol2`,
    act.DateFrom `DatCol3`,
    act.DateTo `DatCol4`,
    p.PartNo `DatCol5`,
    act.FindingDescription `DatCol6`,
    act.Action `DatCol7`,
    act.Penalty `DatCol8`,
    act.Comments `DatCol9`
FROM employeedisciplinaryaction act
INNER JOIN product p
ON p.RowID = act.FindingID
INNER JOIN employee ee
ON ee.RowID = act.EmployeeID
AND FIND_IN_SET(ee.EmploymentStatus, UNEMPLOYEMENT_STATUSES()) = 0
WHERE act.OrganizationID = organizationID AND
    act.Created BETWEEN dateFrom AND dateTo;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
