/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_payphilhealth`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_payphilhealth`(`phh_RowID` INT, `phh_CreatedBy` INT, `phh_LastUpdBy` INT, `phh_SalaryRangeFrom` DECIMAL(10,2), `phh_SalaryRangeTo` DECIMAL(10,2), `phh_SalaryBase` DECIMAL(10,2), `phh_TotalMonthlyPremium` DECIMAL(10,2), `phh_EmployeeShare` DECIMAL(10,2), `phh_EmployerShare` DECIMAL(10,2), `phh_SalaryBracket` DECIMAL(10,2)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnval INT(11);

DECLARE maxsalarybrack INT(11);




INSERT INTO payphilhealth
(
    RowID
    ,Created
    ,CreatedBy
    ,SalaryBracket
    ,SalaryRangeFrom
    ,SalaryRangeTo
    ,SalaryBase
    ,TotalMonthlyPremium
    ,EmployeeShare
    ,EmployerShare
) VALUES (
    phh_RowID
    ,CURRENT_TIMESTAMP()
    ,phh_CreatedBy
    ,phh_SalaryBracket
    ,phh_SalaryRangeFrom
    ,phh_SalaryRangeTo
    ,phh_SalaryBase
    ,phh_TotalMonthlyPremium
    ,phh_EmployeeShare
    ,phh_EmployerShare
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=phh_LastUpdBy
    ,SalaryBracket=phh_SalaryBracket
    ,SalaryRangeFrom=phh_SalaryRangeFrom
    ,SalaryRangeTo=phh_SalaryRangeTo
    ,SalaryBase=phh_SalaryBase
    ,TotalMonthlyPremium=phh_TotalMonthlyPremium
    ,EmployeeShare=phh_EmployeeShare
    ,EmployerShare=phh_EmployerShare;SELECT @@Identity AS id INTO returnval;

RETURN returnval;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
