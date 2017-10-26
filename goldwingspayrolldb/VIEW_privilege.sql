/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `VIEW_privilege`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `VIEW_privilege`(`vw_ViewName` VARCHAR(150), `vw_OrganizationID` INT) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE viewIsExists INT(1);

DECLARE view_RowID INT(11);

    SELECT RowID FROM `view` WHERE ViewName=vw_ViewName AND OrganizationID=vw_OrganizationID INTO view_RowID;

    IF view_RowID IS NULL AND LENGTH(IFNULL(vw_ViewName,'')) > 0 THEN

        INSERT INTO `view`(ViewName,OrganizationID) SELECT vw_ViewName,og.RowID FROM organization og;

        INSERT INTO position_view
        (
            PositionID
            ,ViewID
            ,Creates
            ,OrganizationID
            ,ReadOnly
            ,Updates
            ,Deleting
            ,Created
            ,CreatedBy
            ,LastUpdBy
        ) SELECT
            pos.RowID
            ,v.RowID
            ,'N'
            ,v.OrganizationID
            ,'Y'
            ,'N'
            ,'N'
            ,CURRENT_TIMESTAMP()
            ,(SELECT RowID FROM user ORDER BY LastUpd DESC LIMIT 1)
            ,(SELECT RowID FROM user ORDER BY LastUpd DESC LIMIT 1)
            FROM `view` v
            LEFT JOIN (SELECT * FROM position GROUP BY PositionName) pos ON pos.RowID > 0 AND pos.RowID != (SELECT PositionID FROM user ORDER BY LastUpd DESC LIMIT 1)
            WHERE v.ViewName=vw_ViewName
        UNION
            SELECT
            pos.RowID
            ,v.RowID
            ,'Y'
            ,v.OrganizationID
            ,'N'
            ,'Y'
            ,'Y'
            ,CURRENT_TIMESTAMP()
            ,(SELECT RowID FROM user ORDER BY LastUpd DESC LIMIT 1)
            ,(SELECT RowID FROM user ORDER BY LastUpd DESC LIMIT 1)
            FROM `view` v
            INNER JOIN position pos ON pos.RowID = (SELECT PositionID FROM user ORDER BY LastUpd DESC LIMIT 1)
            WHERE v.ViewName=vw_ViewName
        ON
        DUPLICATE
        KEY
        UPDATE
            LastUpd=CURRENT_TIMESTAMP();

        SELECT RowID FROM `view` WHERE ViewName=vw_ViewName AND OrganizationID=vw_OrganizationID INTO view_RowID;

    END IF;

RETURN view_RowID;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
