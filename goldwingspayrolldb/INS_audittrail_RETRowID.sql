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

-- Dumping structure for function INS_audittrail_RETRowID
DROP FUNCTION IF EXISTS `INS_audittrail_RETRowID`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INS_audittrail_RETRowID`(
	`au_CreatedBy` INT,
	`au_LastUpdBy` INT,
	`au_OrganizationID` INT,
	`au_ViewID` INT,
	`au_FieldChanged` VARCHAR(200),
	`au_ChangedRowID` VARCHAR(50),
	`au_OldValue` VARCHAR(200),
	`au_NewValue` VARCHAR(200),
	`au_ActionPerformed` VARCHAR(50)

) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11);

IF au_ActionPerformed = 'Update' THEN

    IF au_OldValue != au_NewValue THEN

        INSERT INTO audittrail
        (
            Created
            ,CreatedBy
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
            ,au_LastUpdBy
            ,au_OrganizationID
            ,au_ViewID
            ,au_FieldChanged
            ,au_ChangedRowID
            ,IFNULL(au_OldValue,'')
            ,IFNULL(au_NewValue,'')
            ,au_ActionPerformed
        );SELECT @@Identity AS Id INTO returnvalue;

    END IF;

ELSE

    INSERT INTO audittrail
    (
        Created
        ,CreatedBy
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
        ,au_LastUpdBy
        ,au_OrganizationID
        ,au_ViewID
        ,au_FieldChanged
        ,au_ChangedRowID
        ,IFNULL(au_OldValue,'')
        ,IFNULL(au_NewValue,'')
        ,au_ActionPerformed
    );SELECT @@Identity AS Id INTO returnvalue;

END IF;

RETURN returnvalue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
