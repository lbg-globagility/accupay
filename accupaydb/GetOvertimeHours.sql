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
FROM (SELECT
	   @starttime := CONCAT_DATETIME(d.DateValue, ot.OTStartTime) `StartTime`
		,@endtime := CONCAT_DATETIME(ADDDATE(d.DateValue, INTERVAL IS_TIMERANGE_REACHTOMORROW(ot.OTStartTime, ot.OTEndTime) DAY), ot.OTEndTime) `EndTime`

		,@shstarttime := CONCAT_DATETIME(d.DateValue, sh.TimeFrom) `ShiftTimeFrom`
		,@shendtime := CONCAT_DATETIME(ADDDATE(d.DateValue, INTERVAL IS_TIMERANGE_REACHTOMORROW(sh.TimeFrom, sh.TimeTo) DAY), sh.TimeTo) `ShiftTimeTo`

		, @g := DATE_FORMAT(GREATEST(etd.TimeStampIn, TIMESTAMP(@starttime)), @@datetime_format) `G`
		, @l := DATE_FORMAT(LEAST(etd.TimeStampOut, TIMESTAMP(@endtime)), @@datetime_format) `L`

		,   (TIMESTAMPDIFF(SECOND
						   , TIMESTAMP(@g)
								 , TIMESTAMP(@l)
								 ) / sec_per_hour) `Result`

		FROM employeeovertime ot

		INNER JOIN dates d
				ON d.DateValue BETWEEN ot.OTStartDate AND ot.OTEndDate
				
		INNER JOIN employeeshift esh
				ON esh.EmployeeID=ot.EmployeeID
					 AND esh.OrganizationID=ot.OrganizationID
					 AND d.DateValue BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
					  # AND (esh.EffectiveFrom >= ot.OTStartDate OR esh.EffectiveTo >= ot.OTStartDate)
					  # AND (esh.EffectiveFrom <= ot.OTEndDate OR esh.EffectiveTo <= ot.OTEndDate)
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
		GROUP BY ot.RowID
		ORDER BY d.DateValue) i

INTO returnvalue
;

RETURN IFNULL(returnvalue, 0);

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
