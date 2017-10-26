/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `MASS_generate_employeetimeentry`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `MASS_generate_employeetimeentry`(IN `OrganizID` INT, IN `Pay_FrequencyType` TEXT, IN `DivisionRowID` VARCHAR(200), IN `UserRowID` INT, IN `periodDateFrom` DATE, IN `periodDateTo` DATE)
    DETERMINISTIC
BEGIN

UPDATE `position` p
INNER JOIN (SELECT * FROM `user` GROUP BY PositionID) u ON u.RowID > 0
INNER JOIN `position` pos ON pos.RowID=u.PositionID AND LCASE(pos.PositionName)=LCASE(p.PositionName)
SET p.DivisionId    =NULL
        ,p.LastUpd  =IFNULL(ADDTIME(p.LastUpd, SEC_TO_TIME(1)), CURRENT_TIMESTAMP())
        ,p.LastUpdBy=IFNULL(p.LastUpdBy,p.CreatedBy)
WHERE p.OrganizationID=OrganizID;

INSERT INTO `position` (`PositionName`, `LastUpd`, `Created`, `CreatedBy`, `OrganizationID`, `LastUpdBy`, `ParentPositionID`, `DivisionId`, `LevelNumber`) SELECT 'Default Position', CURRENT_TIMESTAMP(), CURRENT_TIMESTAMP(), UserRowID, OrganizID, UserRowID, NULL, d.RowID, 3 FROM division d WHERE d.OrganizationID=OrganizID AND ParentDivisionID IS NOT NULL LIMIT 1 ON DUPLICATE KEY UPDATE LastUpd=CURRENT_TIMESTAMP(),LastUpdBy=UserRowID;

UPDATE employee e
INNER JOIN `position` p ON p.PositionName='Default Position' AND p.OrganizationID=e.OrganizationID
SET e.LastUpdBy=IFNULL(e.LastUpdBy,UserRowID),e.PositionID=p.RowID
WHERE e.OrganizationID=OrganizID AND e.PositionID IS NULL;

UPDATE employee e
INNER JOIN `position` pos ON pos.RowID=e.PositionID AND pos.OrganizationID=e.OrganizationID AND pos.DivisionId IS NULL
INNER JOIN `position` p ON p.PositionName='Default Position' AND p.OrganizationID=e.OrganizationID
SET e.LastUpdBy=IFNULL(e.LastUpdBy,UserRowID),e.PositionID=p.RowID WHERE e.OrganizationID=OrganizID;

UPDATE employee e
INNER JOIN `position` p ON p.PositionName='Default Position' AND p.OrganizationID=e.OrganizationID
INNER JOIN (SELECT * FROM `user` GROUP BY PositionID) u ON u.RowID > 0
INNER JOIN `position` pos ON pos.RowID=u.PositionID
INNER JOIN `position` pstn ON pstn.RowID=e.PositionID AND LCASE(pstn.PositionName)=LCASE(pos.PositionName)
SET e.LastUpdBy=IFNULL(e.LastUpdBy,UserRowID),e.PositionID=p.RowID
WHERE e.OrganizationID=OrganizID;

UPDATE employee SET DayOfRest='1' WHERE OrganizationID=OrganizID AND DayOfRest IN ('','0');

UPDATE employeeovertime eot
INNER JOIN payfrequency pf ON pf.PayFrequencyType=Pay_FrequencyType
INNER JOIN `position` ps ON ps.OrganizationID=eot.OrganizationID AND LOCATE(ps.DivisionId,DivisionRowID) > 0
INNER JOIN employee e ON e.RowID=eot.EmployeeID AND e.OrganizationID=eot.OrganizationID AND e.PayFrequencyID=pf.RowID AND e.PositionID=ps.RowID
SET
eot.LastUpd = ADDTIME(eot.LastUpd, '00:00:01'), eot.LastUpdBy = IFNULL(eot.LastUpdBy, eot.CreatedBy)
WHERE eot.OrganizationID = OrganizID
AND (eot.OTStartDate >= periodDateFrom OR eot.OTEndDate >= periodDateFrom)
AND (eot.OTStartDate <= periodDateTo OR eot.OTEndDate <= periodDateTo);

IF ISNULL(DivisionRowID) THEN
    SELECT GENERATE_employeetimeentry(e.RowID, OrganizID, d.DateValue, UserRowID)
    FROM dates d
    INNER JOIN payfrequency pf ON pf.PayFrequencyType=Pay_FrequencyType
    INNER JOIN employee e ON e.OrganizationID=OrganizID AND e.PayFrequencyID=pf.RowID
    WHERE d.DateValue BETWEEN periodDateFrom AND periodDateTo;
ELSE
    SELECT GENERATE_employeetimeentry(e.RowID, OrganizID, d.DateValue, UserRowID)
    FROM dates d
    INNER JOIN payfrequency pf ON pf.PayFrequencyType=Pay_FrequencyType
    INNER JOIN `position` ps ON ps.OrganizationID=OrganizID AND ps.DivisionId = DivisionRowID
    INNER JOIN employee e ON e.OrganizationID=OrganizID AND e.PayFrequencyID=pf.RowID AND e.PositionID=ps.RowID
    WHERE d.DateValue BETWEEN periodDateFrom AND periodDateTo;
END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
