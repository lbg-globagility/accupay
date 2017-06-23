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

-- Dumping structure for trigger goldwingspayrolldb.AFTINS_repairs
DROP TRIGGER IF EXISTS `AFTINS_repairs`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_repairs` AFTER INSERT ON `repairs` FOR EACH ROW BEGIN



DECLARE PulloutRowID INT(11);

DECLARE isPullout VARCHAR(50);

DECLARE BranchIDFrom INT(11);

DECLARE ProductRowID INT(11);

DECLARE BranchFromPILID INT(11);

SELECT RowID,IF(`Type`='Pullout','1','0') 'is_Pullout',BranchFromID FROM `order` WHERE RowID=NEW.ReferenceNumber INTO PulloutRowID,isPullout,BranchIDFrom;

IF isPullout = '1' THEN

	SELECT RowID FROM productinventorylocation WHERE InventoryLocationID=BranchIDFrom AND ProductID=NEW.ProductID AND OrganizationID=NEW.OrganizationID INTO BranchFromPILID;
	
	IF BranchFromPILID IS NULL THEN
	
		INSERT INTO productinventorylocation (Created,CreatedBy,OrganizationID,LastUpdBy,ProductID,InventoryLocationID,TotalAvailbleItemQty) VALUES (CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.OrganizationID,NEW.CreatedBy,NEW.ProductID,BranchIDFrom,0);
		
	ELSE
	
		UPDATE productinventorylocation SET
		TotalAvailbleItemQty=COALESCE(TotalAvailbleItemQty,0) - 1
		WHERE RowID=BranchFromPILID;
		
		
		
	END IF;

END IF;



END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
