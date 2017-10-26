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

-- Dumping structure for function INSUPD_employeemedicalrecord
DROP FUNCTION IF EXISTS `INSUPD_employeemedicalrecord`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_employeemedicalrecord`(`emedrec_RowID` INT, `emedrec_OrganizationID` INT, `emedrec_Created` TIMESTAMP, `emedrec_CreatedBy` INT, `emedrec_LastUpdBy` INT, `emedrec_EmployeeID` INT, `emedrec_DateFrom` DATE, `emedrec_DateTo` DATE, `emedrec_ProductID` INT, `emedrec_Finding` VARCHAR(50)) RETURNS int(11)
    DETERMINISTIC
    COMMENT 'will insert a row and return its RowID if ''emedrecID'' don''t exist in employeemedicalrecord table or else will update the table base on ''emedrecID'''
BEGIN

DECLARE emedrecID INT(11);

INSERT INTO employeemedicalrecord
(
    RowID
    ,OrganizationID
    ,Created
    ,CreatedBy
    ,LastUpdBy
    ,EmployeeID
    ,DateFrom
    ,DateTo
    ,ProductID
    ,Finding
) VALUES (
    emedrec_RowID
    ,emedrec_OrganizationID
    ,emedrec_Created
    ,emedrec_CreatedBy
    ,emedrec_LastUpdBy
    ,emedrec_EmployeeID
    ,emedrec_DateFrom
    ,emedrec_DateTo
    ,emedrec_ProductID
    ,emedrec_Finding
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=emedrec_LastUpdBy
    ,DateFrom=emedrec_DateFrom
    ,DateTo=emedrec_DateTo
    ,ProductID=emedrec_ProductID
    ,Finding=emedrec_Finding;SELECT @@Identity AS id INTO emedrecID;

RETURN emedrecID;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
