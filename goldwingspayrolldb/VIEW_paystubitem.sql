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

-- Dumping structure for procedure goldwingspayrolldb.VIEW_paystubitem
DROP PROCEDURE IF EXISTS `VIEW_paystubitem`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_paystubitem`(IN `paystitm_PayStubID` INT)
    DETERMINISTIC
BEGIN

SELECT paystitm.RowID 'paystitmID'
,paystitm.PayStubID 'PayStubID'
,paystitm.ProductID 'ProductID'
,SUBSTRING_INDEX(p.PartNo,'.',-1) 'Item'
,paystitm.PayAmount 'PayAmount'
FROM paystubitem paystitm
LEFT JOIN product p ON p.RowID=paystitm.ProductID
WHERE PayStubID=paystitm_PayStubID
AND Undeclared = '0';

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
