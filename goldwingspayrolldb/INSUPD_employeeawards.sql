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

-- Dumping structure for function goldwingspayrolldb.INSUPD_employeeawards
DROP FUNCTION IF EXISTS `INSUPD_employeeawards`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_employeeawards`(`eawa_RowID` INT, `eawa_OrganizationID` INT, `eawa_Created` TIMESTAMP, `eawa_CreatedBy` INT, `eawa_LastUpd` DATETIME, `eawa_LastUpdBy` INT, `eawa_EmployeeID` INT, `eawa_AwardType` VARCHAR(500), `eawa_AwardDescription` VARCHAR(500), `eawa_AwardDate` VARCHAR(500)) RETURNS int(11)
    DETERMINISTIC
    COMMENT 'will insert a row and return its RowID if ''eawa_int'' don''t exist in employeeawards table or else will update the table base on ''eawa_int'''
BEGIN

DECLARE eawa_int INT(11);	

INSERT INTO employeeawards
(
	RowID
	,OrganizationID
	,Created
	,CreatedBy
	,LastUpdBy
	,EmployeeID
	,AwardType
	,AwardDescription
	,AwardDate
) VALUES (
	eawa_RowID
	,eawa_OrganizationID
	,eawa_Created
	,eawa_CreatedBy
	,eawa_LastUpdBy
	,eawa_EmployeeID
	,eawa_AwardType
	,eawa_AwardDescription
	,eawa_AwardDate
) ON
DUPLICATE
KEY
UPDATE 
	LastUpd=CURRENT_TIMESTAMP()
	,LastUpdBy=eawa_LastUpdBy
	,AwardType=eawa_AwardType
	,AwardDescription=eawa_AwardDescription
	,AwardDate=eawa_AwardDate;SELECT @@Identity AS id INTO eawa_int;

RETURN eawa_int;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
