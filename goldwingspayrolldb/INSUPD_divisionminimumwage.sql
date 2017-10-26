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

-- Dumping structure for function INSUPD_divisionminimumwage
DROP FUNCTION IF EXISTS `INSUPD_divisionminimumwage`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `INSUPD_divisionminimumwage`(`dmw_RowID` INT, `dmw_OrganizID` INT, `dmw_UserRowID` INT, `dmw_DivisionID` INT, `dmw_Amount` DECIMAL(10,2), `dmw_EffectiveDateFrom` DATE, `dmw_EffectiveDateTo` DATE) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11);

INSERT INTO divisionminimumwage
(
    RowID
    ,OrganizationID
    ,CreatedBy
    ,DivisionID
    ,Amount
    ,EffectiveDateFrom
    ,EffectiveDateTo
) VALUES (
    dmw_RowID
    ,dmw_OrganizID
    ,dmw_UserRowID
    ,dmw_DivisionID
    ,dmw_Amount
    ,dmw_EffectiveDateFrom
    ,dmw_EffectiveDateTo
) ON
DUPLICATE
KEY
UPDATE
    LastUpdBy=dmw_UserRowID
    ,Amount=dmw_Amount
    ,EffectiveDateFrom=dmw_EffectiveDateFrom
    ,EffectiveDateTo=dmw_EffectiveDateTo;SELECT @@Identity AS ID INTO returnvalue;

RETURN returnvalue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
