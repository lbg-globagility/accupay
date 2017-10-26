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

-- Dumping structure for procedure VIEW_paystubadjustment
DROP PROCEDURE IF EXISTS `VIEW_paystubadjustment`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_paystubadjustment`(IN `pa_EmployeeID` VARCHAR(50), IN `pa_PayPeriodID` INT, IN `pa_OrganizationID` INT, IN `pa_IsActual` TINYINT)
    DETERMINISTIC
BEGIN

SELECT psj.*
FROM (SELECT * FROM paystubadjustment WHERE pa_IsActual=0 AND OrganizationID=pa_OrganizationID
        UNION
        SELECT * FROM paystubadjustmentactual WHERE pa_IsActual=1 AND OrganizationID=pa_OrganizationID
        ) psj
INNER JOIN product p ON p.RowID=psj.ProductID
WHERE psj.PayStubID = FN_GetPayStubIDByEmployeeIDAndPayPeriodID(pa_EmployeeID, pa_PayPeriodID, pa_OrganizationID);

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
