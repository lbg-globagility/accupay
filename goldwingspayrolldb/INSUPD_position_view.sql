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

-- Dumping structure for function INSUPD_position_view
DROP FUNCTION IF EXISTS `INSUPD_position_view`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_position_view`(`pv_RowID` INT, `pv_PositionID` INT, `pv_OrganizationID` INT, `pv_CreatedBy` INT, `pv_LastUpdBy` INT, `pv_ViewID` INT, `pv_Creates` CHAR(1), `pv_ReadOnly` CHAR(1), `pv_Updates` CHAR(1), `pv_Deleting` CHAR(1)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE pvRowID INT(11);

DECLARE strView TEXT;

DECLARE posit_name TEXT;

SELECT p.PositionName FROM position p WHERE p.RowID=pv_PositionID LIMIT 1 INTO posit_name;

INSERT INTO position_view
(
    RowID
    ,PositionID
    ,ViewID
    ,Creates
    ,OrganizationID
    ,ReadOnly
    ,Updates
    ,Deleting
    ,Created
    ,CreatedBy
    ,LastUpdBy
) VALUES (
    pv_RowID
    ,pv_PositionID
    ,pv_ViewID
    ,pv_Creates
    ,pv_OrganizationID
    ,pv_ReadOnly
    ,pv_Updates
    ,pv_Deleting
    ,CURRENT_TIMESTAMP()
    ,pv_CreatedBy
    ,pv_CreatedBy
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=pv_LastUpdBy
    ,Creates=pv_Creates
    ,ReadOnly=pv_ReadOnly
    ,Updates=pv_Updates
    ,Deleting=pv_Deleting;SELECT @@Identity AS id INTO pvRowID;

UPDATE position_view pv
INNER JOIN (SELECT u.PositionID FROM user u INNER JOIN organization og ON og.RowID=pv_OrganizationID AND u.RowID!=og.CreatedBy INNER JOIN position p ON p.RowID=u.PositionID AND p.PositionName=posit_name) ppv ON ppv.PositionID=pv.PositionID
SET
    pv.LastUpd=CURRENT_TIMESTAMP()
    ,pv.LastUpdBy=pv_LastUpdBy
    ,pv.Creates=pv_Creates
    ,pv.ReadOnly=pv_ReadOnly
    ,pv.Updates=pv_Updates
    ,pv.Deleting=pv_Deleting
WHERE pv.OrganizationID=pv_OrganizationID AND pv.ViewID=pv_ViewID;

RETURN pvRowID;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
