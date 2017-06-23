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

-- Dumping structure for procedure goldwingspayrolldb.I_PiliProc
DROP PROCEDURE IF EXISTS `I_PiliProc`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_PiliProc`(IN `I_Created` DATETIME, IN `I_LastUpd` DATETIME, IN `I_CreatedBy` INT(11), IN `I_OrganizationID` INT(11), IN `I_LastShippedToLocDate` DATE, IN `I_LastCycleCountDate` DATE, IN `I_LastUpdBy` INT(11), IN `I_ProdInvLocID` INT(10), IN `I_AvailableQty` INT(10), IN `I_DistributedQty` INT(10), IN `I_ReservedQty` INT(10), IN `I_SupplierProblemQty` INT(10), IN `I_DamagedQty` INT(10), IN `I_RackNo` INT(10), IN `I_ShelfNo` INT(10), IN `I_ColumnNo` INT(10)
)
BEGIN 
INSERT INTO prodinvlocinventory
(
	Created,
	LastUpd,
	CreatedBy,
	OrganizationID,
	LastShippedToLocDate,
	LastCycleCountDate,
	LastUpdBy,
	ProdInvLocID,
	AvailableQty,
	DistributedQty,
	ReservedQty,
	SupplierProblemQty,
	DamagedQty,
	RackNo,
	ShelfNo,
	ColumnNo
)
VALUES
(
	I_Created,
	I_LastUpd,
	I_CreatedBy,
	I_OrganizationID,
	I_LastShippedToLocDate,
	I_LastCycleCountDate,
	I_LastUpdBy,
	I_ProdInvLocID,
	I_AvailableQty,
	I_DistributedQty,
	I_ReservedQty,
	I_SupplierProblemQty,
	I_DamagedQty,
	I_RackNo,
	I_ShelfNo,
	I_ColumnNo
);END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
