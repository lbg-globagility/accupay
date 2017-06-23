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

-- Dumping structure for procedure goldwingspayrolldb.I_ProdImport
DROP PROCEDURE IF EXISTS `I_ProdImport`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_ProdImport`(IN `I_Name` VARCHAR(200), IN `I_OrganizationID` INT(11), IN `I_PartNo` VARCHAR(200), IN `I_Created` DATETIME, IN `I_LastUpd` DATETIME, IN `I_CreatedBy` INT(11), IN `I_LastUpdBy` INT(11), IN `I_Category` VARCHAR(50), IN `I_Catalog` VARCHAR(50), IN `I_Status` VARCHAR(50), IN `I_UnitPrice` DECIMAL(10,2), IN `I_ReorderPoint` INT(10), IN `I_UnitOfMeasure` VARCHAR(50)
, IN `I_CategoryID` INT(11), IN `I_BrandName` VARCHAR(50))
    DETERMINISTIC
BEGIN
INSERT INTO product
	(
	Name,
	OrganizationID,
	PartNo,
	Created,
	LastUpd,
	CreatedBy,
	LastUpdBy,
	Category,
	Catalog,
	Status,
	UnitPrice,
	ReorderPoint,
	UnitOfMeasure,
	CategoryID,
	BrandName
	)
	VALUES
	(
	I_Name,
	I_OrganizationID,
	I_PartNo,
	I_Created,
	I_LastUpd,
	I_CreatedBy,
	I_LastUpdBy,
	I_Category,
	I_Catalog,
	I_Status,
	I_UnitPrice,
	I_ReorderPoint,
	I_UnitOfMeasure,
	I_CategoryID,
	I_BrandName
	);END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
