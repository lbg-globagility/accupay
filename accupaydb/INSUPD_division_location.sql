/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_division_location`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `INSUPD_division_location`(`DivisionRowID` INT, `OrganizID` INT, `DivisionLocationName` VARCHAR(50), `UserRowID` INT) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11);

    INSERT INTO `division` (
        RowID,
        Name,
        OrganizationID,
        CreatedBy,
        ParentDivisionID
    )
    VALUES (
        DivisionRowID,
        DivisionLocationName,
        OrganizID,
        UserRowID,
        NULL
    )
    ON DUPLICATE KEY
    UPDATE
        LastUpd = CURRENT_TIMESTAMP(),
        LastUpdBy = UserRowID,
        Name = DivisionLocationName,
		  ParentDivisionID = NULL;

    SELECT @@Identity AS ID
    INTO returnvalue;

    IF DivisionRowID IS NULL THEN

        INSERT INTO `division`(
            Name,
            TradeName,
            OrganizationID,
            MainPhone,
            FaxNumber,
            BusinessAddress,
            ContactName,
            EmailAddress,
            AltEmailAddress,
            AltPhone,
            URL,
            TINNo,
            Created,
            CreatedBy,
            DivisionType,
            GracePeriod,
            WorkDaysPerYear,
            PhHealthDeductSched,
            HDMFDeductSched,
            SSSDeductSched,
            WTaxDeductSched,
            DefaultVacationLeave,
            DefaultSickLeave,
            DefaultMaternityLeave,
            DefaultPaternityLeave,
            DefaultOtherLeave,
            PayFrequencyID,
            PhHealthDeductSchedAgency,
            HDMFDeductSchedAgency,
            SSSDeductSchedAgency,
            WTaxDeductSchedAgency,
            DivisionUniqueID,
            ParentDivisionID
        )
        SELECT
            'Default Division',
            '',
            OrganizID,
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            CURRENT_TIMESTAMP(),
            UserRowID,
            'Department',
            15.00,
            313,
            'Per pay period',
            'Per pay period',
            'Per pay period',
            'Per pay period',
            40.00,
            40.00,
            40.00,
            40.00,
            40.00,
            1,
            'Per pay period',
            'Per pay period',
            'Per pay period',
            'Per pay period',
            2,
            returnvalue;

    END IF;

    RETURN returnvalue;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
