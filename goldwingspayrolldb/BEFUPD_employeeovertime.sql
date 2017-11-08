/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFUPD_employeeovertime`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFUPD_employeeovertime` BEFORE UPDATE ON `employeeovertime` FOR EACH ROW BEGIN

DECLARE eshiftID INT(11);

SELECT esh.ShiftID
FROM employeeshift esh
WHERE esh.OrganizationID = NEW.OrganizationID AND
    esh.EmployeeID = NEW.EmployeeID AND
    (
        esh.EffectiveFrom >= NEW.OTStartDate OR
        esh.EffectiveTo >= NEW.OTStartDate
    ) AND
    (
        esh.EffectiveFrom <= NEW.OTEndDate OR
        esh.EffectiveTo <= NEW.OTEndDate
    )
ORDER BY esh.EffectiveFrom, esh.EffectiveTo
LIMIT 1
INTO eshiftID;

IF eshiftID IS NOT NULL THEN
    SET NEW.OTStartTime = (
        SELECT IF(
            ADDTIME(sh.TimeTo, SEC_TO_TIME(60)) = NEW.OTStartTime OR sh.TimeTo = NEW.OTStartTime,
            sh.TimeTo,
            TIME_FORMAT(NEW.OTStartTime, @@time_format)
        )
        FROM shift sh
        WHERE sh.RowID = eshiftID
    );

    SET eshiftID = NULL;
END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
