/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `DEL_employeeleave`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `DEL_employeeleave`(IN `leave_rowid` INT)
    DETERMINISTIC
BEGIN

DECLARE og_rowid
        ,e_rowid INT(11);

DECLARE start_date
        ,end_date DATE;

DECLARE start_time
        ,end_time TIME;

SELECT elv.OrganizationID
,elv.EmployeeID
,elv.LeaveStartDate
,elv.LeaveEndDate
,elv.LeaveStartTime
,elv.LeaveEndTime
FROM employeeleave elv
WHERE elv.RowID=leave_rowid
INTO og_rowid
     ,e_rowid
	  ,start_date
	  ,end_date
	  ,start_time
	  ,end_time;

DELETE FROM employeeleave_duplicate WHERE RowID = leave_rowid;

DELETE FROM employeeleave_duplicate
WHERE OrganizationID = og_rowid
AND EmployeeID = e_rowid
AND LeaveStartDate = start_date
AND LeaveEndDate = end_date
AND LeaveStartTime = start_time
AND LeaveEndTime = end_time;

ALTER TABLE employeeleave_duplicate AUTO_INCREMENT = 0;

DELETE FROM employeeleave WHERE RowID = leave_rowid;
ALTER TABLE employeeleave AUTO_INCREMENT = 0;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
