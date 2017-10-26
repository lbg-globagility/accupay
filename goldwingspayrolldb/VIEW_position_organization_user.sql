/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_position_organization_user`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_position_organization_user`(IN `pos_OrganizationID` INT, IN `pagination` INT, IN `current_userID` INT)
    DETERMINISTIC
BEGIN

DECLARE userPositionID INT(11);

DECLARE userPositionName VARCHAR(50);

DECLARE user_ogRowID INT(11);

SELECT u.PositionID
,p.PositionName
,u.OrganizationID
FROM user u
INNER JOIN position p ON p.RowID=u.PositionID
WHERE u.RowID=current_userID
INTO userPositionID
        ,userPositionName
        ,user_ogRowID;


    SELECT
    p.RowID
    ,p.PositionName
    ,COALESCE(p.ParentPositionID,'') 'ParentPositionID'
    ,COALESCE(p.DivisionId,'') 'DivisionId'
    ,p.OrganizationID
    ,p.CreatedBy
    ,COALESCE(p.LastUpd,'') 'LastUpd'
    ,COALESCE(p.LastUpdBy,'') 'LastUpdBy'
    FROM position p
    WHERE p.OrganizationID=pos_OrganizationID AND p.PositionName!=userPositionName
UNION
    SELECT
    RowID
    ,CONCAT(PositionName,' (your position)') AS PositionName
    ,COALESCE(ParentPositionID,'') 'ParentPositionID'
    ,COALESCE(DivisionId,'') 'DivisionId'
    ,OrganizationID
    ,CreatedBy
    ,COALESCE(LastUpd,'') 'LastUpd'
    ,COALESCE(LastUpdBy,'') 'LastUpdBy'
    FROM position
    WHERE RowID=userPositionID
ORDER BY PositionName
LIMIT pagination,100;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
