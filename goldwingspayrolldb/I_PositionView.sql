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

-- Dumping structure for procedure goldwingspayrolldb.I_PositionView
DROP PROCEDURE IF EXISTS `I_PositionView`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_PositionView`(IN `I_PositionID` INT(11), IN `I_ViewID` INT(11), IN `I_Creates` CHAR(1), IN `I_OrganizationID` INT(11), IN `I_ReadOnly` CHAR(1), IN `I_Updates` CHAR(1), IN `I_Deleting` CHAR(1), IN `I_Created` DATETIME, IN `I_CreatedBy` INT(11), IN `I_LastUpd` DATETIME, IN `I_LastUpdBy` INT(11)
)
BEGIN 
INSERT INTO position_view
	(
	PositionID,
	ViewID,
	Creates,
	OrganizationID,
	ReadOnly,
	Updates,
	Deleting,
	Created,
	CreatedBy,
	LastUpd,
	LastUpdBy
	)
VALUES
	(
	I_PositionID,
	I_ViewID,
	I_Creates,
	I_OrganizationID,
	I_ReadOnly,
	I_Updates,
	I_Deleting,
	I_Created,
	I_CreatedBy,
	I_LastUpd,
	I_LastUpdBy
	);END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
