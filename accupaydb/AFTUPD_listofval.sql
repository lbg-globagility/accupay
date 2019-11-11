/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTUPD_listofval`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_listofval` AFTER UPDATE ON `listofval` FOR EACH ROW BEGIN



DECLARE anyvchar VARCHAR(150);

IF SUBSTRING(NEW.`Type`,1,31) = 'Default Government Contribution' THEN

    SET anyvchar = 'Any';

    IF NEW.`LIC` = 'SSS' THEN

        SET anyvchar = 'Any';

        UPDATE payphilhealth
        SET EmployeeShare = CAST(NEW.DisplayValue AS DECIMAL(11,6))
        ,EmployerShare = CAST(NEW.DisplayValue AS DECIMAL(11,6))
        WHERE SalaryRangeFrom = -2000.00
        AND SalaryRangeTo = -1000.00
        AND SalaryBase = -1500.00
        AND HiddenData = '1';


    ELSEIF NEW.`LIC` = 'PhilHealth' THEN

        SET anyvchar = 'Any';

        UPDATE paysocialsecurity
        SET EmployeeContributionAmount = CAST(NEW.DisplayValue AS DECIMAL(11,6))
        ,EmployerContributionAmount = CAST(NEW.DisplayValue AS DECIMAL(11,6))
        WHERE RangeFromAmount = -2000.00
        AND RangeToAmount = -1000.00
        AND MonthlySalaryCredit = -1500.00
        AND HiddenData = '1';

    END IF;

ELSEIF LOCATE('Minimum Wage Rate',NEW.`Type`) > 0 THEN

    SET anyvchar = 'sets the minimum wage rate field/data in the division table';

END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
