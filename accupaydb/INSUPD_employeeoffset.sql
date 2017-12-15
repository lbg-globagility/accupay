/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_employeeoffset`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_employeeoffset`(
	`eosRowID` INT,
	`eosOrganizationID` INT,
	`eosEmployeeID` INT,
	`eosUserRowID` INT,
	`eosType` VARCHAR(50),
	`eosStartTime` TIME,
	`eosEndTime` TIME,
	`eosStartDate` DATE,
	`eosEndDate` DATE,
	`eosStatus` VARCHAR(50),
	`eosReason` VARCHAR(500),
	`eosComments` VARCHAR(2000)
) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11);

INSERT INTO employeeoffset
(
	RowID
	,OrganizationID
	,EmployeeID
	,CreatedBy
	,LastUpdBy
	,Created
	,LastUpd
	,`Type`
	,StartTime
	,EndTime
	,StartDate
	,EndDate
	,`Status`
	,Reason
	,Comments
	,Image
) VALUES (
	eosRowID
	,eosOrganizationID
	,eosEmployeeID
	,eosUserRowID
	,eosUserRowID
	,CURRENT_TIMESTAMP()
	,CURRENT_TIMESTAMP()
	,eosType
	,eosStartTime
	,eosEndTime
	,eosStartDate
	,eosEndDate
	,eosStatus
	,eosReason
	,eosComments
	,NULL
) ON
DUPLICATE
KEY
UPDATE
	LastUpd = CURRENT_TIMESTAMP()
	,LastUpdBy = eosUserRowID
	,StartTime = eosStartTime
	,EndTime = eosEndTime
	,StartDate = eosStartDate
	,EndDate = eosEndDate
	,`Status` = eosStatus
	,Reason = eosReason
	,Comments = eosComments;
	SELECT @@Identity `ID` INTO returnvalue;
	
RETURN returnvalue;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
