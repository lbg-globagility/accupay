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

-- Dumping structure for function goldwingspayrolldb.GET_paystubadjustmenttaxabornontaxab
DROP FUNCTION IF EXISTS `GET_paystubadjustmenttaxabornontaxab`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `GET_paystubadjustmenttaxabornontaxab`(`PayStubRowID` INT, `IsTaxable` CHAR(50)) RETURNS decimal(11,6)
    DETERMINISTIC
BEGIN

DECLARE returnvalue DECIMAL(11,6);

SELECT SUM(IFNULL(psa.PayAmount,0))
FROM paystubadjustment psa
INNER JOIN product p ON p.RowID=psa.ProductID
WHERE psa.PayStubID=PayStubRowID
AND p.`Status`=IsTaxable
INTO returnvalue;

RETURN IFNULL(returnvalue,0);

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
