/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP VIEW IF EXISTS `expectedhours`;
CREATE TABLE `expectedhours` (
	`EmployeeID` INT(10) NULL,
	`OrganizationID` INT(10) NOT NULL,
	`Date` DATE NOT NULL COMMENT 'time entry date',
	`TotalExpectedHours` DECIMAL(12,2) NULL
) ENGINE=MyISAM;

DROP VIEW IF EXISTS `expectedhours`;
DROP TABLE IF EXISTS `expectedhours`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` VIEW `expectedhours` AS SELECT et.EmployeeID
, et.OrganizationID
, et.`Date`
,	 IF((TIMESTAMPDIFF(SECOND
	                   , CONCAT_DATETIME(et.`Date`, sh.TimeFrom)
							 , CONCAT_DATETIME(ADDDATE(et.`Date`, INTERVAL IS_TIMERANGE_REACHTOMORROW(sh.TimeFrom, sh.TimeTo) DAY), sh.TimeTo)) / 3600)
	    > (et.RegularHoursWorked + (et.HoursLate + et.UndertimeHours))
		   
		 , (et.RegularHoursWorked + (et.HoursLate + et.UndertimeHours))
	   , sh.DivisorToDailyRate)
    `TotalExpectedHours`

FROM employeetimeentry et
INNER JOIN employee ee
       ON ee.RowID=et.EmployeeID
		    AND ee.OrganizationID=et.OrganizationID
			 AND ee.EmploymentStatus NOT IN ('Resigned', 'Terminated')
# INNER JOIN `position` pos ON pos.RowID=ee.PositionID AND pos.DivisionId = div_rowid
INNER JOIN employeeshift esh
       ON esh.RowID=et.EmployeeShiftID
INNER JOIN shift sh
       ON sh.RowID=esh.ShiftID ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
