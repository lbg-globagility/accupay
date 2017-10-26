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

-- Dumping structure for procedure U_PILI
DROP PROCEDURE IF EXISTS `U_PILI`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `U_PILI`(IN `I_RowID` INT(10), IN `I_AvailableQty` INT(10), IN `I_DistributedQty` INT(10), IN `I_RackNo` INT(10), IN `I_ShelfNo` INT(10), IN `I_ColumnNo` INT(10)
)
BEGIN
UPDATE prodinvlocinventory
SET
    AvailableQty = I_AvailableQty,
    DistributedQty = I_DistributedQty,
    RackNo = I_RackNo,
    ShelfNo = I_ShelfNo,
    ColumnNo = I_ColumnNo
Where   RowID = I_RowID;End//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
