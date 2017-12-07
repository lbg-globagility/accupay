/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `DEL_division_shift`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `DEL_division_shift`(IN `OrganizID` INT, IN `Division_RowID` INT)
    DETERMINISTIC
BEGIN

DECLARE is_query_exists CHAR(1);

DECLARE anyintiger INT(11) DEFAULT 0;

SELECT @@default_week_format INTO anyintiger;

SET group_concat_max_len = 1000000;

SET @esh_RowID = '';


                    SELECT (@esh_RowID := GROUP_CONCAT(esh.RowID))

                    FROM employeeshift esh

                    INNER JOIN position pos ON pos.DivisionId=Division_RowID AND pos.OrganizationID=esh.OrganizationID

                    INNER JOIN employee e ON e.RowID=esh.EmployeeID AND e.OrganizationID=esh.OrganizationID AND e.PositionID=pos.RowID



                    INNER JOIN (SELECT DateValue FROM dates WHERE YEAR(DateValue)=YEAR(CURDATE()) AND DAYOFWEEK(DateValue)=(@@default_week_format + 1) ORDER BY DateValue LIMIT 1) dd ON dd.DateValue IS NOT NULL

                    INNER JOIN (SELECT IF(LAST_DAY(DATE_FORMAT(CURDATE(),'%Y-12-01')) > d.DateValue
                                                , ADDDATE(d.DateValue, INTERVAL 1 WEEK)
                                                , LAST_DAY(DATE_FORMAT(CURDATE(),'%Y-12-01'))) AS EndingDate
                                    FROM dates d
                                    WHERE YEAR(d.DateValue) <= YEAR(CURDATE())
                                    AND DAYOFWEEK(d.DateValue) = IF(anyintiger - 1 < 0, 7, anyintiger)
                                    AND WEEKOFYEAR(d.DateValue) > 50
                                    ORDER BY d.DateValue DESC
                                    LIMIT 1) ddd ON ddd.EndingDate IS NOT NULL

                    WHERE esh.OrganizationID=OrganizID

                    AND (esh.EffectiveFrom >= IF(TIMESTAMPDIFF(YEAR,e.StartDate,CURDATE()) > 1, dd.DateValue, e.StartDate)
                            OR esh.EffectiveTo >= IF(TIMESTAMPDIFF(YEAR,e.StartDate,CURDATE()) > 1, dd.DateValue, e.StartDate))

                    AND (esh.EffectiveFrom <= ddd.EndingDate OR esh.EffectiveTo <= ddd.EndingDate);



        DELETE FROM employeeshift WHERE LOCATE(RowID,@esh_RowID) > 0;

        SET GLOBAL event_scheduler = ON;

        ALTER TABLE employeeshift AUTO_INCREMENT = 0;



SET group_concat_max_len = 1024;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
