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

-- Dumping structure for function hyundaipayrolldb_dev.DEFAULT_internal_sys_user
DROP FUNCTION IF EXISTS `DEFAULT_internal_sys_user`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `DEFAULT_internal_sys_user`() RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE default_zero INT(11) DEFAULT 0;

DECLARE default_internal_sys_user_rowid INT(11);

SELECT RowID FROM `user` WHERE RowID=default_zero INTO default_internal_sys_user_rowid;

IF default_internal_sys_user_rowid IS NULL THEN

	INSERT INTO `user` (`RowID`, `LastName`, `FirstName`, `MiddleName`, `UserID`, `Password`, `OrganizationID`, `PositionID`, `Created`, `LastUpdBy`, `CreatedBy`, `LastUpd`, `Status`, `EmailAddress`, `AllowLimitedAccess`, `Column 16`, `InSession`) VALUES (0, 'user', 'user', 'user', 'æéòîóæ', 'æéòîó', 1, 1, '2017-03-09 00:00:00', 1, 1, '2017-10-06 09:33:49', 'Active', 'user@email.com.ph', '1', NULL, '1'); SELECT @@identity INTO default_internal_sys_user_rowid;

   UPDATE `user` SET RowID=default_zero WHERE RowID=default_internal_sys_user_rowid;

END IF;

RETURN default_zero;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
