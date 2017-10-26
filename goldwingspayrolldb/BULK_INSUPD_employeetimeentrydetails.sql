/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `BULK_INSUPD_employeetimeentrydetails`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `BULK_INSUPD_employeetimeentrydetails`(IN `organiz_rowid` INT, IN `user_rowid` INT, IN `date_from` DATE, IN `date_to` DATE, IN `import_id` INT)
    DETERMINISTIC
BEGIN

DECLARE this_curr_timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP();

DECLARE one_sec_time TIME;

DECLARE int_time_allowance INT(11) DEFAULT 10;

DECLARE hous_per_day INT(11) DEFAULT 24;

DECLARE mins_per_hour INT(11) DEFAULT 60;

DECLARE secs_per_min INT(11) DEFAULT 60;

DECLARE divided_into_half INT(11) DEFAULT 2;

DECLARE less_one_second INT(11) DEFAULT 1;

DECLARE total_secs_per_hour INT(11);

SET total_secs_per_hour = (secs_per_min * mins_per_hour);

SET one_sec_time = SEC_TO_TIME(1);

/*SELECT
	DATE(MIN(DATE(DATE_FORMAT(tel.TimeStampLog, @@date_format))))
	,DATE(MAX(DATE(DATE_FORMAT(tel.TimeStampLog, @@date_format))))
FROM timeentrylogs tel
WHERE tel.OrganizationID=i.OrganizationID
AND tel.ImportID=import_id
INTO date_from
	, date_to;*/

SET @comput_sec1 = 0;
SET @comput_sec2 = 0;

SET @less_5hours = 18000; # (total_secs_per_hour * 5)
SET @add_10hours = 36000; # (total_secs_per_hour * 10)

INSERT INTO employeetimeentrydetails
(
	OrganizationID
	,Created
	,CreatedBy
	,EmployeeID
	,TimeIn
	,TimeOut
	,`Date`
	,TimeentrylogsImportID
) SELECT
	organiz_rowid
	,this_curr_timestamp
	,user_rowid
	,etd.EmployeeID
	,TIME(etd.`TimeIn`)
	,TIME(etd.`TimeOut`)
	,etd.DateValue
	,etd.ImportID
	FROM (SELECT i.*

         ,MIN(tel.TimeStampLog) `TimeIn`
			,MAX(tlg.TimeStampLog) `TimeOut`
			,IFNULL(tel.ImportID, tlg.ImportID) `ImportID`
			FROM (# day shift, in and out at same day
					SELECT esh.*
					,d.DateValue
					,'Day shift' `ShiftCategory`
					/*,CONCAT_DATETIME(SUBDATE(d.DateValue, INTERVAL 1 DAY), ADDTIME(sh.TimeTo, one_sec_time)) `RealisticTimeRangeFrom`
					,CONCAT_DATETIME(ADDDATE(d.DateValue, INTERVAL 1 DAY), SUBTIME(sh.TimeFrom, one_sec_time)) `RealisticTimeRangeTo`*/
					/*,(@comput_sec1 := ROUND((((hous_per_day - COMPUTE_TimeDifference(sh.TimeFrom, sh.TimeTo))
					                          / divided_into_half)
											         * total_secs_per_hour), 0)) `Result`*/
					,(@comput_sec1 := ((COMPUTE_TimeDifference(sh.TimeFrom, sh.TimeTo) / divided_into_half) * total_secs_per_hour)) `Result`
					,SUBDATE(CONCAT_DATETIME(d.DateValue, sh.TimeFrom), INTERVAL @less_5hours SECOND) `RealisticTimeRangeFrom`
					,ADDDATE(CONCAT_DATETIME(d.DateValue, sh.TimeFrom), INTERVAL CAST(@comput_sec1 AS INT) SECOND) `RealisticTimeRangeFromHalf`
					
					,ADDDATE(CONCAT_DATETIME(d.DateValue, sh.TimeTo), INTERVAL @add_10hours SECOND) `RealisticTimeRangeTo`
					,SUBDATE(CONCAT_DATETIME(d.DateValue, sh.TimeTo), INTERVAL CAST((@comput_sec1-less_one_second) AS INT) SECOND) `RealisticTimeRangeToHalf`
					FROM employeeshift esh
					INNER JOIN dates d ON d.DateValue BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
					INNER JOIN shift sh ON sh.RowID=esh.ShiftID AND IS_TIMERANGE_REACHTOMORROW(sh.TimeFrom, sh.TimeTo) = FALSE
					WHERE esh.OrganizationID=organiz_rowid
				
				UNION # night shift, log out on the next day
					SELECT esh.*
					,d.DateValue
					,'Night shift' `ShiftCategory`
					/*,CONCAT_DATETIME(d.DateValue, ADDTIME(sh.TimeTo, one_sec_time)) `RealisticTimeRangeFrom`
					,CONCAT_DATETIME(ADDDATE(d.DateValue, INTERVAL 1 DAY), SUBTIME(sh.TimeFrom, one_sec_time)) `RealisticTimeRangeTo`*/
					/*,(@comput_sec2 := ROUND((((hous_per_day - COMPUTE_TimeDifference(sh.TimeFrom, sh.TimeTo))
					                          / divided_into_half)
											         * total_secs_per_hour), 0)) `Result`*/
					,(@comput_sec2 := ((TIMESTAMPDIFF(HOUR, CONCAT_DATETIME(d.DateValue, sh.TimeFrom), CONCAT_DATETIME(ADDDATE(d.DateValue, INTERVAL 1 DAY), sh.TimeTo)) / divided_into_half) * total_secs_per_hour)) `Result`
					,SUBDATE(CONCAT_DATETIME(d.DateValue, sh.TimeFrom), INTERVAL @less_5hours SECOND) `RealisticTimeRangeFrom`
					,ADDDATE(CONCAT_DATETIME(d.DateValue, sh.TimeFrom), INTERVAL CAST(@comput_sec2 AS INT) SECOND) `RealisticTimeRangeFromHalf`
					
					,ADDDATE(CONCAT_DATETIME(ADDDATE(d.DateValue, INTERVAL 1 DAY), sh.TimeTo), INTERVAL @add_10hours SECOND) `RealisticTimeRangeTo`
					,SUBDATE(CONCAT_DATETIME(ADDDATE(d.DateValue, INTERVAL 1 DAY), sh.TimeTo), INTERVAL CAST((@comput_sec2-less_one_second) AS INT) SECOND) `RealisticTimeRangeToHalf`
					FROM employeeshift esh
					INNER JOIN dates d ON d.DateValue BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
					INNER JOIN shift sh ON sh.RowID=esh.ShiftID AND IS_TIMERANGE_REACHTOMORROW(sh.TimeFrom, sh.TimeTo) = TRUE
					WHERE esh.OrganizationID=organiz_rowid
					
				UNION # rest day without assigned shift, log in and out at the same day
					SELECT esh.*
					,d.DateValue
					,'Rest day - day shift' `ShiftCategory`
					,0 `Result`
					,CONCAT_DATETIME(d.DateValue, SEC_TO_TIME(0)) `RealisticTimeRangeFrom`
					,CONCAT_DATETIME(d.DateValue, TIME('12:00:00')) `RealisticTimeRangeFromHalf`
					
					,CONCAT_DATETIME(d.DateValue, TIME('23:59:59')) `RealisticTimeRangeTo`
					,CONCAT_DATETIME(d.DateValue, TIME('12:00:01')) `RealisticTimeRangeToHalf`
					FROM employeeshift esh
					INNER JOIN dates d ON d.DateValue BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
					WHERE esh.OrganizationID=organiz_rowid
					AND esh.ShiftID IS NULL
					AND esh.RestDay = 1) i
					
					
			LEFT JOIN timeentrylogs tel
						ON tel.EmployeeRowID		=i.EmployeeID
						AND tel.OrganizationID	=i.OrganizationID
						AND tel.ImportID			=import_id
						# AND tel.TimeStampLog BETWEEN i.`RealisticTimeRangeFrom` AND i.`RealisticTimeRangeTo`
						AND tel.TimeStampLog BETWEEN i.`RealisticTimeRangeFrom` AND i.`RealisticTimeRangeFromHalf`
						
			LEFT JOIN timeentrylogs tlg
						ON tlg.EmployeeRowID		=i.EmployeeID
						AND tlg.OrganizationID	=i.OrganizationID
						AND tlg.ImportID			=import_id
						AND tlg.TimeStampLog BETWEEN i.`RealisticTimeRangeToHalf` AND i.`RealisticTimeRangeTo`
						
			GROUP BY i.EmployeeID, i.DateValue
			ORDER BY i.EmployeeID, i.DateValue) etd

			# WHERE etd.`TimeIn` < etd.`TimeOut`
			# WHERE @max_time_in < @max_time_out
			WHERE etd.ImportID IS NOT NULL

ON
DUPLICATE
KEY
UPDATE
	LastUpd		=CURRENT_TIMESTAMP()
	,LastUpdBy	=user_rowid
	,TimeIn		=etd.`TimeIn`
	,TimeOut		=etd.`TimeOut`;

/*SET @min_date = CURDATE(); SET @max_date = CURDATE();

SELECT
	MIN(DATE(DATE_FORMAT(tel.TimeStampLog,@@date_format)))
	,MAX(DATE(DATE_FORMAT(tel.TimeStampLog,@@date_format)))
FROM timeentrylogs tel
WHERE tel.ImportID = import_id
AND tel.OrganizationID = organiz_rowid
INTO @min_date, @max_date;

UPDATE employeetimeentrydetails etd
SET
	etd.LastUpd		=CURRENT_TIMESTAMP()
	,etd.LastUpdBy	=user_rowid
	,etd.TimeOut	=NULL
WHERE etd.OrganizationID=organiz_rowid
# AND etd.`Date` BETWEEN date_from AND date_to;
AND etd.`Date` BETWEEN @min_date AND @max_date;*/

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
