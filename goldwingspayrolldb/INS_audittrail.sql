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

-- Dumping structure for procedure goldwingspayrolldb.INS_audittrail
DROP PROCEDURE IF EXISTS `INS_audittrail`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `INS_audittrail`(IN `au_CreatedBy` INT, IN `au_LastUpdBy` INT, IN `au_OrganizationID` INT, IN `au_ViewID` INT, IN `au_FieldChanged` VARCHAR(200), IN `au_ChangedRowID` VARCHAR(50), IN `au_OldValue` VARCHAR(200), IN `au_NewValue` VARCHAR(200), IN `au_ActionPerformed` VARCHAR(50))
    DETERMINISTIC
BEGIN

INSERT INTO audittrail
(
	Created
	,CreatedBy
	,LastUpd
	,LastUpdBy
	,OrganizationID
	,ViewID
	,FieldChanged
	,ChangedRowID
	,OldValue
	,NewValue
	,ActionPerformed
) VALUES (
	CURRENT_TIMESTAMP()
	,au_CreatedBy
	,CURRENT_TIMESTAMP()
	,au_LastUpdBy
	,au_OrganizationID
	,au_ViewID
	,au_FieldChanged
	,au_ChangedRowID
	,au_OldValue
	,au_NewValue
	,au_ActionPerformed
);



END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
