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

-- Dumping structure for trigger AFTINS_user
DROP TRIGGER IF EXISTS `AFTINS_user`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_user` AFTER INSERT ON `user` FOR EACH ROW BEGIN





INSERT INTO position_view
(
    PositionID
    ,ViewID
    ,Creates
    ,OrganizationID
    ,ReadOnly
    ,Updates
    ,Deleting
    ,Created
    ,CreatedBy
    ,LastUpdBy
) SELECT
    pos.RowID
    ,v.RowID
    ,'N'
    ,v.OrganizationID
    ,'N'
    ,'N'
    ,'N'
    ,CURRENT_TIMESTAMP()
    ,NEW.CreatedBy
    ,NEW.CreatedBy
    FROM `view` v
    LEFT JOIN (SELECT * FROM `position` GROUP BY PositionName) pos ON pos.RowID > 0 AND pos.RowID != IFNULL(NEW.PositionID,0)
    WHERE v.OrganizationID!=NEW.OrganizationID
UNION
    SELECT
    pos.RowID
    ,v.RowID
    ,'Y'
    ,v.OrganizationID
    ,'N'
    ,'Y'
    ,'Y'
    ,CURRENT_TIMESTAMP()
    ,NEW.CreatedBy
    ,NEW.CreatedBy
    FROM `view` v
    INNER JOIN `position` pos ON pos.RowID = IFNULL(NEW.PositionID,0)
    WHERE v.OrganizationID=NEW.OrganizationID
ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP();

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
