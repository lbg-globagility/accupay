-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.12-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for procedure IMPORT_OB
DROP PROCEDURE IF EXISTS `IMPORT_OB`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `IMPORT_OB`(IN `og_id` INT, IN `user_rowid` INT, IN `emp_num` VARCHAR(50), IN `allowance_name` VARCHAR(50), IN `start_date` VARCHAR(50), IN `i_starttime` VARCHAR(50), IN `end_date` VARCHAR(50), IN `i_endtime` VARCHAR(50))
    DETERMINISTIC
BEGIN

INSERT INTO employeeofficialbusiness
(
	OrganizationID	
	,Created
	,CreatedBy
	,LastUpdBy
	,EmployeeID
	,OffBusType
	,OffBusStartDate
	,OffBusStartTime
	,OffBusEndDate
	,OffBusEndTime
      ,OffBusStatus
) SELECT
	e.OrganizationID
	,CURRENT_TIMESTAMP()
	,user_rowid
	,user_rowid
	,e.RowID
	,allowance_name
	,STR_TO_DATE(start_date, @@datetime_format)
	,i_starttime
	,STR_TO_DATE(end_date, @@datetime_format)
      ,i_endtime
     ,'Approved'
FROM employee e
WHERE e.OrganizationID=og_id AND e.EmployeeID=emp_num

ON
DUPLICATE
KEY
UPDATE
	LastUpd=CURRENT_TIMESTAMP()
	,LastUpdBy=user_rowid;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
