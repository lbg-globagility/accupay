/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFUPD_position`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFUPD_position` BEFORE UPDATE ON `position` FOR EACH ROW BEGIN

IF NEW.DivisionId IS NULL THEN

    INSERT INTO `division`(Name,TradeName,OrganizationID,MainPhone,FaxNumber,BusinessAddress,ContactName,EmailAddress,AltEmailAddress,AltPhone,URL,TINNo,Created,CreatedBy,DivisionType,GracePeriod,WorkDaysPerYear,PhHealthDeductSched,HDMFDeductSched,SSSDeductSched,WTaxDeductSched,DefaultVacationLeave,DefaultSickLeave,DefaultMaternityLeave,DefaultPaternityLeave,DefaultOtherLeave,PayFrequencyID,PhHealthDeductSchedAgency,HDMFDeductSchedAgency,SSSDeductSchedAgency,WTaxDeductSchedAgency,DivisionUniqueID,ParentDivisionID) SELECT 'Default Division', '', NEW.OrganizationID, '', '', '', '', '', '', '', '', '', CURRENT_TIMESTAMP(), NEW.CreatedBy, 'Department', 15.00, 313, 'Per pay period', 'Per pay period', 'Per pay period', 'Per pay period', 40.00, 40.00, 40.00, 40.00, 40.00, 1, 'Per pay period', 'Per pay period', 'Per pay period', 'Per pay period',2,d.RowID FROM division d WHERE d.OrganizationID=NEW.OrganizationID AND d.ParentDivisionID IS NOT NULL LIMIT 1 ON DUPLICATE KEY UPDATE LastUpd=CURRENT_TIMESTAMP(), LastUpdBy=NEW.CreatedBy; SELECT @@identity INTO @primkey_id;

    IF IFNULL(@primkey_id,0) = 0 THEN
        SET @primkey_id = (SELECT RowID FROM division d WHERE d.Name='Default Division' AND d.OrganizationID=NEW.OrganizationID AND d.ParentDivisionID IS NOT NULL LIMIT 1);
    END IF;

    SET NEW.DivisionId = @primkey_id;
END IF;

SELECT EXISTS(SELECT RowID FROM `user` WHERE PositionID=NEW.RowID LIMIT 1)
INTO @is_sysuser;

/*IF NEW.DivisionId IS NULL AND @is_sysuser = 0 THEN
    SET NEW.DivisionId = (SELECT RowID FROM division d WHERE d.OrganizationID=NEW.OrganizationID AND d.ParentDivisionID IS NOT NULL LIMIT 1);
END IF;

SELECT EXISTS(SELECT d.RowID FROM division d WHERE d.OrganizationID=NEW.OrganizationID AND d.RowID=NEW.DivisionId AND d.ParentDivisionID IS NULL AND @is_sysuser = 0 LIMIT 1)
INTO @invalid_divisionid;

IF @invalid_divisionid = 1 THEN
    SET NEW.DivisionId = (SELECT RowID FROM division d WHERE d.OrganizationID=NEW.OrganizationID AND d.ParentDivisionID IS NOT NULL LIMIT 1);
END IF;*/

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
