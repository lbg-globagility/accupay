/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_paysocialsecurity`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_paysocialsecurity`(`sss_RowID` INT, `sss_CreatedBy` INT, `sss_LastUpdBy` INT, `sss_RangeFromAmount` DECIMAL(10,2), `sss_RangeToAmount` DECIMAL(10,2), `sss_MonthlySalaryCredit` DECIMAL(10,2), `sss_EmployeeContributionAmount` DECIMAL(10,2), `sss_EmployerContributionAmount` DECIMAL(10,2), `sss_EmployeeECAmount` DECIMAL(10,2)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnval INT(11);

INSERT INTO paysocialsecurity
(
    RowID
    ,Created
    ,CreatedBy
    ,RangeFromAmount
    ,RangeToAmount
    ,MonthlySalaryCredit
    ,EmployeeContributionAmount
    ,EmployerContributionAmount
    ,EmployeeECAmount
) VALUES (
    RowID
    ,CURRENT_TIMESTAMP()
    ,sss_CreatedBy
    ,sss_RangeFromAmount
    ,sss_RangeToAmount
    ,sss_MonthlySalaryCredit
    ,sss_EmployeeContributionAmount
    ,sss_EmployerContributionAmount
    ,sss_EmployeeECAmount
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=sss_LastUpdBy
    ,RangeFromAmount=sss_RangeFromAmount
    ,RangeToAmount=sss_RangeToAmount
    ,MonthlySalaryCredit=sss_MonthlySalaryCredit
    ,EmployeeContributionAmount=sss_EmployeeContributionAmount
    ,EmployerContributionAmount=sss_EmployerContributionAmount
    ,EmployeeECAmount=sss_EmployeeECAmount;SELECT @@Identity AS id INTO returnval;

IF returnval = 0 THEN

    IF (SELECT EXISTS(SELECT RowID FROM paysocialsecurity WHERE RowID IS NULL LIMIT 1)) = 1 THEN

        UPDATE paysocialsecurity SET RowID=1 WHERE RowID='0';

        SET returnval = 1;

    END IF;

END IF;

RETURN returnval;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
