/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `TRIGG_UPD_employeeallowance`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `TRIGG_UPD_employeeallowance`(`OrganizID` INT, `UserRowID` INT, `EmpRowID` INT, `es_datefrom` DATE, `es_dateto` DATE) RETURNS char(1) CHARSET utf8
    DETERMINISTIC
BEGIN

DECLARE returnvalue CHAR(1) DEFAULT '0';

UPDATE employeeallowance ea
SET ea.LastUpd = CURRENT_TIMESTAMP()
,ea.LastUpdBy=UserRowID
WHERE ea.TaxableFlag='1'
AND ea.OrganizationID=OrganizID
AND ea.EmployeeID=EmpRowID
AND (ea.EffectiveStartDate >= es_datefrom OR ea.EffectiveEndDate >= es_datefrom)
AND (ea.EffectiveStartDate <= IFNULL(es_dateto,ea.EffectiveEndDate) OR ea.EffectiveEndDate <= IFNULL(es_dateto,ea.EffectiveEndDate));

RETURN returnvalue;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
