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

-- Dumping structure for procedure goldwingspayrolldb.U_PILIRSCQ
DROP PROCEDURE IF EXISTS `U_PILIRSCQ`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `U_PILIRSCQ`(IN `U_RowID` INT(10), IN `U_AvailableQty` INT(10), IN `U_RackNo` INT(10), IN `U_ShelfNo` INT(10), IN `U_ColumnNo` INT(10)
	)
    DETERMINISTIC
BEGIN
	UPDATE prodinvlocinventory SET
	AvailableQty = U_AvailableQty,
	RackNo = U_RackNo ,
	ShelfNo = U_ShelfNo,
	ColumnNo = U_ColumnNo
	WHERE RowID = U_RowID;END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
