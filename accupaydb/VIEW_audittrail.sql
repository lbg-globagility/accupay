/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_audittrail`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_audittrail`(IN `OrganizID` INT, IN `View_ID` INT, IN `UserID` INT, IN `pagenumber` INT)
    DETERMINISTIC
BEGIN

    SELECT aut.RowID
    ,aut.ViewID
    ,aut.ChangedRowID
    ,DATE_FORMAT(aut.Created,'%m/%d/%Y %h:%i %p') 'Created'
    ,PROPERCASE(CONCAT(COALESCE(u.FirstName,''),' ', COALESCE(u.LastName,''))) 'CreatedBy'
    ,v.ViewName,aut.FieldChanged, COALESCE(aut.OldValue,'') 'OldValue'
    ,COALESCE(aut.NewValue,'') 'NewValue'
    ,aut.ActionPerformed
    FROM audittrail aut
    LEFT JOIN USER u ON u.RowID=aut.CreatedBy
    LEFT JOIN `view` v ON v.RowID = aut.ViewID
    WHERE aut.OrganizationID=OrganizID
    AND aut.ViewID=View_ID
    AND aut.CreatedBy=UserID
    AND aut.ActionPerformed IN ('Insert','Update')
    ORDER BY aut.Created DESC
    LIMIT pagenumber,20;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
