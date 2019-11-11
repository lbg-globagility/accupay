/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTINS_division`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_division` AFTER INSERT ON `division` FOR EACH ROW BEGIN

DECLARE countchildposition INT(11);
DECLARE defaultpositionname VARCHAR(120);
DECLARE anyint INT(11);

SELECT COUNT(RowID)
FROM position
WHERE DivisionId = NEW.RowID
INTO countchildposition;

SET countchildposition = IFNULL(countchildposition, 0);

IF countchildposition = -1 THEN

    SELECT SUBSTRING_INDEX(PositionName, ' ', -1)
    FROM position
    WHERE PositionName LIKE '%Default%'
    ORDER BY SUBSTRING_INDEX(PositionName, ' ', -1) DESC
    LIMIT 1
    INTO defaultpositionname;

    SELECT INSUPD_position(
        NULL,
        CONCAT('Default Position ',defaultpositionname + 1),
        NEW.CreatedBy,
        NEW.OrganizationID,
        NEW.LastUpdBy,
        NULL,
        NEW.RowID
    )
    INTO anyint;

    SET countchildposition = 1;

END IF;
/*
UPDATE employee e
INNER JOIN position ps
ON ps.DivisionId = NEW.RowID
SET e.PayFrequencyID = NEW.PayFrequencyID,
    e.WorkDaysPerYear = NEW.WorkDaysPerYear,
    e.LateGracePeriod = NEW.GracePeriod
WHERE e.OrganizationID = NEW.OrganizationID AND
    e.PositionID = ps.RowID;
*/
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
