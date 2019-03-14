/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_employeetimeentrydetails`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeetimeentrydetails`(
	IN `etentd_TimeentrylogsImportID` VARCHAR(100),
	IN `etentd_OrganizationID` INT,
	IN `etd_EmployeeNumber` VARCHAR(50),
	IN `e_FirstName` VARCHAR(50),
	IN `e_LastName` VARCHAR(50)

)
LANGUAGE SQL
DETERMINISTIC
CONTAINS SQL
SQL SECURITY DEFINER
COMMENT ''
BEGIN

IF etd_EmployeeNumber IS NULL THEN
    SET etd_EmployeeNumber = '';
END IF;

IF etd_EmployeeNumber = '' THEN

    SELECT etentd.RowID
    ,COALESCE(etentd.EmployeeID,'') 'empRowID'
    ,COALESCE(e.EmployeeID,'') 'EmployeeID'
    ,CONCAT(e.LastName,',',e.FirstName, IF(e.MiddleName='','',','),INITIALS(e.MiddleName,'. ','1')) 'Fullname'
    ,COALESCE((SELECT CONCAT(COALESCE(CONCAT(SUBSTRING_INDEX(TIME_FORMAT(shft.TimeFrom,'%r'),':',2),' ',SUBSTRING_INDEX(TIME_FORMAT(shft.TimeFrom,'%r'),' ',-1)),''),' to ',COALESCE(CONCAT(SUBSTRING_INDEX(TIME_FORMAT(shft.TimeTo,'%r'),':',2),' ',SUBSTRING_INDEX(TIME_FORMAT(shft.TimeTo,'%r'),' ',-1)),'')) FROM shift shft LEFT JOIN employeeshift esh ON esh.ShiftID=shft.RowID WHERE esh.EmployeeID=etentd.EmployeeID AND etentd.`Date` BETWEEN DATE(COALESCE(esh.EffectiveFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(esh.EffectiveTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) ORDER BY DATEDIFF(DATE_FORMAT(NOW(),'%Y-%m-%d'),esh.EffectiveFrom) LIMIT 1),'') 'EmployeeShift'
    ,COALESCE(CONCAT(SUBSTRING_INDEX(TIME_FORMAT(etentd.TimeIn,'%r'),':',2),TIME_FORMAT(etentd.TimeIn,' %p')),'') 'TimeIn'
    ,CAST(IFNULL(etentd.`Date`,'1900-01-01') AS DATE) AS `Date`
    ,COALESCE(CONCAT(SUBSTRING_INDEX(TIME_FORMAT(etentd.TimeOut,'%r'),':',2),TIME_FORMAT(etentd.TimeOut,' %p')),'') 'TimeOut'
    ,CAST(IF(DATE(etentd.`TimeStampIn`) <> DATE(etentd.`TimeStampOut`),DATE(etentd.`TimeStampOut`),"") as DATE) AS `Date`
    ,COALESCE(etentd.TimeScheduleType,'') 'TimeScheduleType'
    ,COALESCE(etentd.TimeEntryStatus,'') 'TimeEntryStatus'
    FROM employeetimeentrydetails etentd
    LEFT JOIN employee e ON e.RowID=etentd.EmployeeID
    WHERE etentd.TimeentrylogsImportID =etentd_TimeentrylogsImportID
        AND (e.FirstName LIKE CONCAT('%', e_FirstName, '%') OR e_FirstName = '')
        AND (e.LastName LIKE CONCAT('%', e_LastName, '%') OR e_LastName = '')
    AND etentd.OrganizationID=etentd_OrganizationID
    ORDER BY etentd.EmployeeID,etentd.`Date`;

ELSE

    SELECT etentd.RowID
    ,COALESCE(etentd.EmployeeID,'') 'empRowID'
    ,COALESCE(e.EmployeeID,'') 'EmployeeID'
    ,CONCAT(e.LastName,',',e.FirstName, IF(e.MiddleName='','',','),INITIALS(e.MiddleName,'. ','1')) 'Fullname'
    ,COALESCE((SELECT CONCAT(COALESCE(CONCAT(SUBSTRING_INDEX(TIME_FORMAT(shft.TimeFrom,'%r'),':',2),' ',SUBSTRING_INDEX(TIME_FORMAT(shft.TimeFrom,'%r'),' ',-1)),''),' to ',COALESCE(CONCAT(SUBSTRING_INDEX(TIME_FORMAT(shft.TimeTo,'%r'),':',2),' ',SUBSTRING_INDEX(TIME_FORMAT(shft.TimeTo,'%r'),' ',-1)),'')) FROM shift shft LEFT JOIN employeeshift esh ON esh.ShiftID=shft.RowID WHERE esh.EmployeeID=etentd.EmployeeID AND etentd.`Date` BETWEEN DATE(COALESCE(esh.EffectiveFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(esh.EffectiveTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) ORDER BY DATEDIFF(DATE_FORMAT(NOW(),'%Y-%m-%d'),esh.EffectiveFrom) LIMIT 1),'') 'EmployeeShift'
    ,COALESCE(CONCAT(SUBSTRING_INDEX(TIME_FORMAT(etentd.TimeIn,'%r'),':',2),TIME_FORMAT(etentd.TimeIn,' %p')),'') 'TimeIn'
    ,CAST(IFNULL(etentd.`Date`,'1900-01-01') AS DATE) AS `Date`
    ,COALESCE(CONCAT(SUBSTRING_INDEX(TIME_FORMAT(etentd.TimeOut,'%r'),':',2),TIME_FORMAT(etentd.TimeOut,' %p')),'') 'TimeOut'
	 ,CAST(IF(DATE(etentd.`TimeStampIn`) <> DATE(etentd.`TimeStampOut`),DATE(etentd.`TimeStampOut`),"") as DATE) AS `Date`
    ,COALESCE(etentd.TimeScheduleType,'') 'TimeScheduleType'
    ,COALESCE(etentd.TimeEntryStatus,'') 'TimeEntryStatus'
    FROM employeetimeentrydetails etentd
    INNER JOIN employee e ON e.EmployeeID=etd_EmployeeNumber AND e.OrganizationID=etentd_OrganizationID
    WHERE etentd.TimeentrylogsImportID = etentd_TimeentrylogsImportID
    AND etentd.OrganizationID=etentd_OrganizationID
    AND etentd.EmployeeID=e.RowID
    ORDER BY etentd.EmployeeID,etentd.`Date`;

END IF;


END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
