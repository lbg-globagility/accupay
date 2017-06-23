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

-- Dumping structure for function goldwingspayrolldb.INSUPD_agencyfee
DROP FUNCTION IF EXISTS `INSUPD_agencyfee`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `INSUPD_agencyfee`(`agf_RowID` INT, `agf_OrganizationID` INT, `agf_UserRowID` INT, `agf_AgencyID` INT, `agf_EmployeeID` INT, `agf_EmpPositionID` INT, `agf_DivisionID` INT, `agf_TimeEntryID` INT, `agf_TimeEntryDate` DATE, `agf_DailyFee` DECIMAL(11,6)) RETURNS int(11)
    DETERMINISTIC
    COMMENT 'agencyfee'
BEGIN

DECLARE returnvalue INT(11);



INSERT INTO agencyfee
(
	RowID
	,OrganizationID
	,Created
	,CreatedBy
	,AgencyID
	,EmployeeID
	,EmpPositionID
	,DivisionID
	,TimeEntryID
	,TimeEntryDate
	,DailyFee
) VALUES (
	agf_RowID
	,agf_OrganizationID
	,CURRENT_TIMESTAMP()
	,agf_UserRowID
	,agf_AgencyID
	,agf_EmployeeID
	,agf_EmpPositionID
	,agf_DivisionID
	,agf_TimeEntryID
	,agf_TimeEntryDate
	,IFNULL(agf_DailyFee,0)
) ON
DUPLICATE
KEY
UPDATE
	LastUpd = CURRENT_TIMESTAMP()
	,LastUpdBy = agf_UserRowID
	,EmpPositionID = agf_EmpPositionID
	,EmpPositionID = agf_EmpPositionID
	,TimeEntryID = agf_TimeEntryID
	,DailyFee = IFNULL(agf_DailyFee,0);SELECT @@Identity AS ID INTO returnvalue;

RETURN returnvalue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
