/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFINS_division`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFINS_division` BEFORE INSERT ON `division` FOR EACH ROW BEGIN

DECLARE min_start_date_of_employee DATE;
DECLARE count_min_wage INT(11);
DECLARE anyint INT(11);

SELECT COUNT(RowID)
FROM divisionminimumwage
WHERE OrganizationID = NEW.OrganizationID AND
    DivisionID = NEW.RowID
INTO count_min_wage;

IF count_min_wage = 0 THEN

    SELECT MIN(e.StartDate)
    FROM employee e
    LEFT JOIN position pos
    ON pos.RowID = e.PositionID AND
        pos.DivisionId = NEW.RowID
    WHERE e.OrganizationID = NEW.OrganizationID
    INTO min_start_date_of_employee;

    IF min_start_date_of_employee IS NULL THEN
        SELECT Created
        FROM organization
        WHERE RowID = NEW.OrganizationID
        INTO min_start_date_of_employee;
    END IF;

    SELECT INSUPD_divisionminimumwage(
        NULL,
        NEW.OrganizationID,
        NEW.LastUpdBy,
        NEW.RowID,
        481.0,
        min_start_date_of_employee,
        pp.PayToDate
    )
    FROM payperiod pp
    WHERE min_start_date_of_employee < pp.PayToDate AND
        pp.OrganizationID = NEW.OrganizationID AND
        pp.TotalGrossSalary = 1 AND
        CURDATE() BETWEEN pp.PayFromDate AND pp.PayToDate
    INTO anyint;

END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
