/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_attendance_sheet`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_attendance_sheet`(IN `OrganizationID` INT, IN `FromDate` DATE, IN `ToDate` DATE)
    DETERMINISTIC
BEGIN

SET @customDateFormat = '%c/%e/%Y';
SET @customTimeFormat = '%l:%i';

SELECT
	CONCAT_WS(' / ', e.EmployeeID, CONCAT_WS(', ', e.LastName, e.FirstName)) `DatCol1`
	, UCASE(LEFT(DAYNAME(ete.`Date`), 3)) `DatCol2`
	, DATE_FORMAT(ete.`Date`, @customDateFormat) `DatCol3`
    , CONCAT_WS(' to '
        , CONCAT(TIME_FORMAT(shiftschedules.StartTime, IF(MINUTE(shiftschedules.StartTime)=0, '%l', @customTimeFormat))
                    , LEFT(TIME_FORMAT(shiftschedules.StartTime, ' %p'), 2))
        , CONCAT(TIME_FORMAT(shiftschedules.EndTime, IF(MINUTE(shiftschedules.EndTime)=0, '%l', @customTimeFormat))
                    , LEFT(TIME_FORMAT(shiftschedules.EndTime, ' %p'), 2))
    )`DatCol4`
	,REPLACE(TIME_FORMAT(etd.TimeIn,'%l:%i %p'),'M','') `DatCol5`
	,'' AS `DatCol6`
	,'' AS `DatCol7`
	,REPLACE(TIME_FORMAT(etd.TimeOut,'%l:%i %p'),'M','') `DatCol8`
	, IFNULL(ete.RegularHoursWorked,0) `DatCol9`
	, IFNULL(ete.HoursLate,0) `DatCol10`
	, IFNULL(ete.UndertimeHours,0) `DatCol11`
	, IFNULL(ete.NightDifferentialHours,0) `DatCol12`
	, IFNULL(ete.OvertimeHoursWorked,0) `DatCol13`
	, IFNULL(ete.NightDifferentialOTHours,0) `DatCol14`
	,etd.TimeScheduleType `DatCol15`
FROM employeetimeentry ete
LEFT JOIN (
    SELECT EmployeeID, DATE,
    (SELECT RowID
    FROM employeetimeentrydetails
    WHERE EmployeeID = groupedEtd.EmployeeID
    AND DATE = groupedEtd.Date
    ORDER BY LastUpd DESC
    LIMIT 1) RowID
FROM employeetimeentrydetails groupedEtd
WHERE Date BETWEEN FromDate AND ToDate
GROUP BY EmployeeID, Date
) latest
    ON latest.EmployeeID = ete.EmployeeID AND
        latest.Date = ete.Date
LEFT JOIN employeetimeentrydetails etd
    ON etd.Date = ete.Date AND
        etd.OrganizationID = ete.OrganizationID AND
        etd.EmployeeID = ete.EmployeeID AND
        etd.RowID = latest.RowID
LEFT JOIN (
    SELECT EmployeeID, OffBusStartDate Date, MAX(Created) Created
    FROM employeeofficialbusiness
    WHERE OffBusStartDate BETWEEN FromDate AND ToDate
    GROUP BY EmployeeID, Date
) latestOb
    ON latestOb.EmployeeID = ete.EmployeeID AND
        latestOb.Date = ete.Date
LEFT JOIN employeeofficialbusiness ofb
    ON ofb.OffBusStartDate = ete.Date AND
        ofb.EmployeeID = ete.EmployeeID AND
        ofb.Created = latestOb.Created
LEFT JOIN employeeovertime ot
    ON ot.OTStartDate = ete.Date AND
        ot.EmployeeID = ete.EmployeeID AND
        ot.OTStatus = 'Approved'
LEFT JOIN payrate
    ON payrate.Date = ete.Date AND
        payrate.OrganizationID = ete.OrganizationID
LEFT JOIN shiftschedules
    ON shiftschedules.EmployeeID = ete.EmployeeID AND
        shiftschedules.`Date` = ete.`Date`
    
INNER JOIN employee e
    ON ete.EmployeeID = e.RowID

WHERE ete.OrganizationID = OrganizationID AND
    ete.`Date` BETWEEN FromDate AND ToDate
GROUP BY ete.RowID
ORDER BY  CONCAT(e.LastName, e.FirstName), ete.`Date`
;
	
END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
