/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `GetOvertimeHours`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GetOvertimeHours`(`org_rowid` INT, `emp_rowid` INT, `param_date` daTE) RETURNS decimal(15,4)
    DETERMINISTIC
BEGIN

DECLARE approve_status VARCHAR(50) DEFAULT 'Approved';

DECLARE sec_per_hour INT(11) DEFAULT 3600;

DECLARE returnvalue DECIMAL(15,4);

DECLARE datetime1
        ,datetime2
        
        ,datetime3
        ,datetime4
        
        ,datetime5
        ,datetime6 DATETIME;


SELECT SUM(i.`Result`)
FROM (
      SELECT ot.*
      FROM (SELECT
				ot.RowID, 'Query1' `Group`, d.DateValue,

				@shstarttime := CONCAT_DATETIME(d.DateValue, sh.TimeFrom) `ShiftTimeFrom`
				,@shendtime := CONCAT_DATETIME(ADDDATE(d.DateValue, INTERVAL IS_TIMERANGE_REACHTOMORROW(sh.TimeFrom, sh.TimeTo) DAY), sh.TimeTo) `ShiftTimeTo`

				,@starttime := IF(sh.TimeTo <= ot.OTStartTime
								      AND sh.TimeTo <= ot.OTEndTime
									  
									  , CONCAT_DATETIME(DATE(@shendtime), ot.OTStartTime)
									  , IF(sh.TimeFrom >= ot.OTStartTime
									  AND sh.TimeFrom >= ot.OTEndTime
									  
											 , CONCAT_DATETIME(DATE(@shstarttime), ot.OTStartTime)
											 , CONCAT_DATETIME(d.DateValue, ot.OTStartTime)))
				`StartTime`
				,@endtime := IF(sh.TimeTo <= ot.OTStartTime
								    AND sh.TimeTo <= ot.OTEndTime
									  
									  , CONCAT_DATETIME(DATE(@shendtime), ot.OTEndTime)
									  , IF(sh.TimeFrom >= ot.OTStartTime
									  AND sh.TimeFrom >= ot.OTEndTime
									  
											 , CONCAT_DATETIME(DATE(@shstarttime), ot.OTEndTime)
											 , CONCAT_DATETIME(ADDDATE(d.DateValue, INTERVAL IS_TIMERANGE_REACHTOMORROW(ot.OTStartTime, ot.OTEndTime) DAY), ot.OTEndTime)))
				`EndTime`

				, @g := DATE_FORMAT(GREATEST(etd.TimeStampIn, TIMESTAMP(@starttime)), @@datetime_format) `G`
				, @l := DATE_FORMAT(LEAST(etd.TimeStampOut, TIMESTAMP(@endtime)), @@datetime_format) `L`

				,   (TIMESTAMPDIFF(SECOND
								   , TIMESTAMP(@g)
										 , TIMESTAMP(@l)
										 ) / sec_per_hour) `Result`

				,ot.OTStartTime
				,ot.OTEndTime

				,sh.TimeFrom
				,sh.TimeTo

				,etd.TimeStampIn
				,etd.TimeStampOut

				,DATE(@shstarttime) < DATE(@shendtime) `CustomColumn`

				FROM employeeovertime ot

				INNER JOIN dates d
						ON d.DateValue BETWEEN ot.OTStartDate AND ot.OTEndDate
						
				INNER JOIN employeeshift esh
						ON esh.EmployeeID=ot.EmployeeID
							 AND esh.OrganizationID=ot.OrganizationID
							 AND d.DateValue BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
							 
				INNER JOIN shift sh
						ON sh.RowID=esh.ShiftID
				INNER JOIN employeetimeentrydetails etd
						ON etd.EmployeeID=ot.EmployeeID
						   AND etd.OrganizationID=ot.OrganizationID
						   AND etd.`Date`=d.DateValue
						   
				WHERE ot.EmployeeID = emp_rowid
				AND ot.OrganizationID = org_rowid
				AND ot.OTStatus = approve_status
				AND param_date BETWEEN ot.OTStartDate AND ot.OTEndDate
				AND DATE(@shstarttime) < DATE(@shendtime)

				UNION

				SELECT
				ot.RowID, 'Query2' `Group`, d.DateValue,

				@shstarttime := CONCAT_DATETIME(d.DateValue, sh.TimeFrom) `ShiftTimeFrom`
				,@shendtime := CONCAT_DATETIME(ADDDATE(d.DateValue, INTERVAL IS_TIMERANGE_REACHTOMORROW(sh.TimeFrom, sh.TimeTo) DAY), sh.TimeTo) `ShiftTimeTo`

				,@starttime := IF(sh.TimeTo <= ot.OTStartTime
								      AND sh.TimeTo <= ot.OTEndTime
									  
									  , CONCAT_DATETIME(DATE(@shendtime), ot.OTStartTime)
									  , IF(sh.TimeFrom >= ot.OTStartTime
									  AND sh.TimeFrom >= ot.OTEndTime
									  
											 , CONCAT_DATETIME(DATE(@shstarttime), ot.OTStartTime)
											 , CONCAT_DATETIME(d.DateValue, ot.OTStartTime)))
				`StartTime`
				,@endtime := IF(sh.TimeTo <= ot.OTStartTime
								    AND sh.TimeTo <= ot.OTEndTime
									  
									  , CONCAT_DATETIME(DATE(@shendtime), ot.OTEndTime)
									  , IF(sh.TimeFrom >= ot.OTStartTime
									  AND sh.TimeFrom >= ot.OTEndTime
									  
											 , CONCAT_DATETIME(DATE(@shstarttime), ot.OTEndTime)
											 , CONCAT_DATETIME(ADDDATE(d.DateValue, INTERVAL IS_TIMERANGE_REACHTOMORROW(ot.OTStartTime, ot.OTEndTime) DAY), ot.OTEndTime)))
				`EndTime`

				, @g := DATE_FORMAT(GREATEST(etd.TimeStampIn, TIMESTAMP(@starttime)), @@datetime_format) `G`
				, @l := DATE_FORMAT(LEAST(etd.TimeStampOut, TIMESTAMP(@endtime)), @@datetime_format) `L`

				,   (TIMESTAMPDIFF(SECOND
								   , TIMESTAMP(@g)
										 , TIMESTAMP(@l)
										 ) / sec_per_hour) `Result`

				,ot.OTStartTime
				,ot.OTEndTime

				,sh.TimeFrom
				,sh.TimeTo

				,etd.TimeStampIn
				,etd.TimeStampOut

				,DATE(@shstarttime) < DATE(@shendtime) `CustomColumn`

				FROM employeeovertime ot

				INNER JOIN dates d
						ON d.DateValue BETWEEN ot.OTStartDate AND ot.OTEndDate
						
				INNER JOIN employeeshift esh
						ON esh.EmployeeID=ot.EmployeeID
							 AND esh.OrganizationID=ot.OrganizationID
							 AND d.DateValue BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
							 
				INNER JOIN shift sh
						ON sh.RowID=esh.ShiftID
				INNER JOIN employeetimeentrydetails etd
						ON etd.EmployeeID=ot.EmployeeID
						   AND etd.OrganizationID=ot.OrganizationID
						   AND etd.`Date`=d.DateValue
						   
				WHERE ot.EmployeeID = emp_rowid
				AND ot.OrganizationID = org_rowid
				AND ot.OTStatus = approve_status
				AND param_date BETWEEN ot.OTStartDate AND ot.OTEndDate
				AND DATE(@shstarttime) = DATE(@shendtime)
		      ) ot
		GROUP BY ot.RowID
		ORDER BY ot.DateValue) i

INTO returnvalue
;

RETURN IFNULL(returnvalue, 0);

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
