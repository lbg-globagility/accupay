/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_position`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_position`(`pos_RowID` INT, `pos_PositionName` VARCHAR(50), `pos_CreatedBy` INT, `pos_OrganizationID` INT, `pos_LastUpdBy` INT, `pos_ParentPositionID` INT, `pos_DivisionId` INT, `pos_JobLevelID` INT) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE positID INT(11);

DECLARE defaultDivisID INT(11);

SELECT COUNT(RowID)
FROM `division`
WHERE OrganizationID = pos_OrganizationID
INTO defaultDivisID;

IF defaultDivisID > 0 THEN

    IF pos_DivisionId IS NULL THEN
        SELECT RowID
        FROM division
        WHERE OrganizationID = pos_OrganizationID AND
            ParentDivisionID IS NOT NULL
        ORDER BY RowID
        LIMIT 1
        INTO pos_DivisionId;
    END IF;

    INSERT INTO `position` (
        RowID,
        PositionName,
        Created,
        CreatedBy,
        OrganizationID,
        LastUpdBy,
        ParentPositionID,
        DivisionId,
        JobLevelID
    )
    VALUES (
        pos_RowID,
        pos_PositionName,
        CURRENT_TIMESTAMP(),
        pos_CreatedBy,
        pos_OrganizationID,
        pos_LastUpdBy,
        pos_ParentPositionID,
        pos_DivisionId,
        pos_JobLevelID
    )
    ON DUPLICATE KEY
    UPDATE
        PositionName = pos_PositionName,
        LastUpd = CURRENT_TIMESTAMP(),
        LastUpdBy = pos_LastUpdBy,
        ParentPositionID = pos_ParentPositionID,
        DivisionId = pos_DivisionId,
        JobLevelID = pos_JobLevelID;

    SELECT @@Identity AS Id
    INTO positID;

ELSE

    INSERT INTO `division`
    (
        Name
        ,OrganizationID
        ,CreatedBy
        ,Created
    ) VALUES (
        'Division One'
        ,pos_OrganizationID
        ,pos_CreatedBy
        ,CURRENT_TIMESTAMP()
    ) ON
    DUPLICATE
    KEY
    UPDATE
        LastUpd=CURRENT_TIMESTAMP()
        ,LastUpdBy=pos_LastUpdBy;

    SELECT RowID FROM `division` WHERE OrganizationID=pos_OrganizationID ORDER BY RowID DESC LIMIT 1 INTO defaultDivisID;

    INSERT INTO `position`
    (
        RowID
        ,PositionName
        ,Created
        ,CreatedBy
        ,OrganizationID
        ,LastUpdBy
        ,ParentPositionID
        ,DivisionId
    ) VALUES (
        pos_RowID
        ,pos_PositionName
        ,CURRENT_TIMESTAMP()
        ,pos_CreatedBy
        ,pos_OrganizationID
        ,pos_LastUpdBy
        ,pos_ParentPositionID
        ,defaultDivisID
    ) ON
    DUPLICATE
    KEY
    UPDATE
        PositionName=pos_PositionName
        ,LastUpd=CURRENT_TIMESTAMP()
        ,LastUpdBy=pos_LastUpdBy
        ,ParentPositionID=pos_ParentPositionID
        ,DivisionId=defaultDivisID;SELECT @@Identity AS Id INTO positID;

END IF;

IF IFNULL(positID,0) = 0 AND IFNULL(pos_RowID,0) != 0 THEN
    SET positID = pos_RowID;
ELSEIF IFNULL(positID,0) = 0 THEN
    SELECT RowID FROM `position` pt WHERE pt.PositionName=pos_PositionName AND pt.OrganizationID=pos_OrganizationID LIMIT 1 INTO positID;
END IF;

RETURN positID;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
