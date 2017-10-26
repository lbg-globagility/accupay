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

-- Dumping structure for function INSUPD_branch
DROP FUNCTION IF EXISTS `INSUPD_branch`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `INSUPD_branch`(`br_RowID` INT, `br_OrganizID` INT, `br_UserRowID` INT, `br_BranchCode` VARCHAR(100), `br_BranchName` VARCHAR(100), `br_areaID` INT) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11);

INSERT INTO branch
(
    RowID
    ,OrganizationID
    ,Created
    ,CreatedBy
    ,BranchCode
    ,BranchName
    ,AreaID
) VALUES (
    br_RowID
    ,br_OrganizID
    ,CURRENT_TIMESTAMP()
    ,br_UserRowID
    ,br_BranchCode
    ,br_BranchName
    ,br_areaID
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=br_UserRowID
    ,BranchCode=br_BranchCode
    ,BranchName=br_BranchName
    ,AreaID=br_areaID;SELECT @@Identity AS ID INTO returnvalue;

RETURN returnvalue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
