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

-- Dumping structure for function INSUPD_listofval
DROP FUNCTION IF EXISTS `INSUPD_listofval`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_listofval`(`DispVal` VARCHAR(50), `paramLIC` VARCHAR(50), `paramType` VARCHAR(50), `Parent_LIC` VARCHAR(50), `paramActive` CHAR(5), `Descript` VARCHAR(500), `UserRowID` INT, `Order_By` INT) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11);

DECLARE maxordby INT(11);



SELECT MAX(OrderBy) + 1 FROM listofval WHERE `LIC`=paramLIC AND `Type`=paramType AND `ParentLIC`=Parent_LIC INTO maxordby;

IF IFNULL(maxordby,0) = 0 THEN

    SET  maxordby = 0;

END IF;

INSERT INTO listofval
(
    DisplayValue
    ,LIC
    ,`Type`
    ,ParentLIC
    ,Active
    ,Description
    ,Created
    ,CreatedBy
    ,LastUpd
    ,OrderBy
    ,LastUpdBy
) VALUES (
    DispVal
    ,paramLIC
    ,paramType
    ,Parent_LIC
    ,paramActive
    ,Descript
    ,CURRENT_TIMESTAMP()
    ,UserRowID
    ,CURRENT_TIMESTAMP()
    ,maxordby
    ,UserRowID
) ON
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
