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

-- Dumping structure for function INSUPD_timeentrylogs
DROP FUNCTION IF EXISTS `INSUPD_timeentrylogs`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `INSUPD_timeentrylogs`(`OrganizID` INT, `EmployeeIdentificationKey` VARCHAR(50), `DateTimeLogStamp` VARCHAR(50), `Import_ID` INT) RETURNS int(11)
    DETERMINISTIC
BEGIN
DECLARE returnvalue INT(11) DEFAULT 0;
DECLARE custom_datetimeformat VARCHAR(50) DEFAULT '%m-%d-%Y %H:%i';
INSERT INTO timeentrylogs
(
	OrganizationID
	,EmployeeRowID
	,TimeStampLog
	,ImportID
) SELECT
	OrganizID
	,e.RowID
	,STR_TO_DATE(DateTimeLogStamp, custom_datetimeformat)
	,Import_ID
FROM employee e
WHERE e.OrganizationID=OrganizID
AND e.EmployeeID=EmployeeIdentificationKey
AND STR_TO_DATE(DateTimeLogStamp, custom_datetimeformat) IS NOT NULL
ON
DUPLICATE
KEY
UPDATE
	LastUpd=CURRENT_TIMESTAMP();SELECT @@Identity AS ID INTO returnvalue;
	
RETURN returnvalue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
