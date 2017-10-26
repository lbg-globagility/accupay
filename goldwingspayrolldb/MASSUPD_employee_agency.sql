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

-- Dumping structure for procedure MASSUPD_employee_agency
DROP PROCEDURE IF EXISTS `MASSUPD_employee_agency`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `MASSUPD_employee_agency`(IN `OrganizID` INT, IN `AgencyRowID` INT, IN `UserRowID` INT, IN `EmployeeListRowID` VARCHAR(1000))
    DETERMINISTIC
    COMMENT 'the comment'
BEGIN

DECLARE param_str_length INT(11);

SET param_str_length = LENGTH(EmployeeListRowID);

SELECT SUBSTRING(EmployeeListRowID,2,param_str_length) INTO EmployeeListRowID;

UPDATE employee
SET AgencyID = AgencyRowID
,LastUpd=CURRENT_TIMESTAMP()
,LastUpdBy=UserRowID
WHERE OrganizationID = OrganizID
AND LOCATE(RowID,EmployeeListRowID) > 0;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
