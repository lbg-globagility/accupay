-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.11-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.0.0.4396
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for function goldwingspayrolldb.INSUPD_agency
DROP FUNCTION IF EXISTS `INSUPD_agency`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `INSUPD_agency`(`ag_RowID` INT, `ag_OrganizationID` INT, `ag_UserRowID` INT, `ag_AgencyName` VARCHAR(50), `ag_AgencyFee` DECIMAL(11,2), `ag_AddressID` INT, `ag_IsActive` TINYINT) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11);

INSERT INTO agency
(
	RowID
	,OrganizationID
	,Created
	,CreatedBy
	,LastUpdBy
	,AgencyName
	,`AgencyFee`
	,AddressID
	,IsActive
)
SELECT
	ag_RowID
	,og.RowID
	,CURRENT_TIMESTAMP()
	,ag_UserRowID
	,ag_UserRowID
	,ag_AgencyName
	,ag_AgencyFee
	,ag_AddressID
	,ag_IsActive
FROM organization og
WHERE og.RowID != ag_OrganizationID
ON DUPLICATE KEY UPDATE
	LastUpd=CURRENT_TIMESTAMP()
	,LastUpdBy = ag_UserRowID
	,AgencyName = ag_AgencyName
	,AddressID = ag_AddressID
	,IsActive = ag_IsActive;
	
INSERT INTO agency
(
	RowID
	,OrganizationID
	,Created
	,CreatedBy
	,LastUpdBy
	,AgencyName
	,`AgencyFee`
	,AddressID
	,IsActive
)
VALUES
(
	ag_RowID
	,ag_OrganizationID
	,CURRENT_TIMESTAMP()
	,ag_UserRowID
	,ag_UserRowID
	,ag_AgencyName
	,ag_AgencyFee
	,ag_AddressID
	,ag_IsActive
)
ON DUPLICATE KEY UPDATE
	LastUpd=CURRENT_TIMESTAMP()
	,LastUpdBy=ag_UserRowID
	,AgencyName = ag_AgencyName
	,`AgencyFee` = ag_AgencyFee
	,AddressID = ag_AddressID
	,IsActive = ag_IsActive;

SELECT @@Identity AS ID
INTO returnvalue;

RETURN returnvalue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
