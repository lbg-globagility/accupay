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

-- Dumping structure for trigger AFTUPD_order_pullout_productinventorylocation
DROP TRIGGER IF EXISTS `AFTUPD_order_pullout_productinventorylocation`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_order_pullout_productinventorylocation` AFTER UPDATE ON `order` FOR EACH ROW BEGIN

DECLARE i_loop INT(11) DEFAULT 0;

DECLARE oitemcount INT(11);

DECLARE prodID INT(11);

DECLARE qtyapprvd INT(11);

DECLARE mainbranchID INT(11);

DECLARE frombranch INT(11);

DECLARE tobranch INT(11);





SELECT RowID FROM inventorylocation WHERE `Type`='Main' AND OrganizationID=NEW.OrganizationID AND MainBranch='M' AND Status='Active' ORDER BY RowID DESC LIMIT 1 INTO mainbranchID;

IF NEW.Status = 'Approved for Inventoryss' THEN

    SELECT COUNT(RowID) FROM orderitem WHERE ParentOrderID=NEW.RowID AND OrganizationID=NEW.OrganizationID INTO oitemcount;

    SELECT BranchFromID,BranchToID FROM `order` WHERE RowID=NEW.PulloutControlNo INTO frombranch,tobranch;

    loop_item: LOOP

        SELECT ProductID,COALESCE(QtyIssued,0) FROM orderitem WHERE ParentOrderID=NEW.RowID AND OrganizationID=NEW.OrganizationID LIMIT i_loop,1 INTO prodID,qtyapprvd;

        IF i_loop < oitemcount THEN

            UPDATE productinventorylocation SET
            TotalAvailbleItemQty=COALESCE(TotalAvailbleItemQty,0) - qtyapprvd
            WHERE OrganizationID=NEW.OrganizationID
            AND ProductID=prodID
            AND InventoryLocationID=IF(mainbranchID != frombranch,frombranch,NULL);

            UPDATE productinventorylocation SET
            TotalAvailbleItemQty=COALESCE(TotalAvailbleItemQty,0) + qtyapprvd
            WHERE OrganizationID=NEW.OrganizationID
            AND ProductID=prodID
            AND InventoryLocationID=IF(mainbranchID != tobranch,tobranch,NULL);

        ELSE
            LEAVE loop_item;
        END IF;

        SET i_loop = i_loop + 1;

    END LOOP;

END IF;



END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
