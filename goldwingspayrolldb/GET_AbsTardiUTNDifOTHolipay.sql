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

-- Dumping structure for procedure goldwingspayrolldb.GET_AbsTardiUTNDifOTHolipay
DROP PROCEDURE IF EXISTS `GET_AbsTardiUTNDifOTHolipay`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `GET_AbsTardiUTNDifOTHolipay`(IN `param_OrganizationID` INT, IN `param_EmployeeRowID` INT, IN `param_PayPeriodID1` INT, IN `param_PayPeriodID2` INT)
    DETERMINISTIC
BEGIN

SELECT psi.PayAmount,psi.Undeclared
,p.PartNo
,p.`Category`
FROM paystubitem psi
INNER JOIN product p ON p.RowID=psi.ProductID AND p.`Category` IN ('Deductions','Miscellaneous') AND p.PartNo NOT LIKE '%.%'
INNER JOIN payperiod pp1 ON pp1.RowID=param_PayPeriodID1
INNER JOIN payperiod pp2 ON pp2.RowID=param_PayPeriodID2
INNER JOIN paystub ps ON ps.RowID=psi.PayStubID AND ps.EmployeeID=param_EmployeeRowID AND ps.OrganizationID=param_OrganizationID AND ps.PayToDate BETWEEN pp1.PayFromDate AND pp2.PayToDate;








END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
