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

-- Dumping structure for function GET_paystubitemallowancenotecola
DROP FUNCTION IF EXISTS `GET_paystubitemallowancenotecola`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GET_paystubitemallowancenotecola`(`OrganizID` INT, `EmpRowID` INT, `PayPeriodRowID` INT) RETURNS decimal(11,6)
    DETERMINISTIC
BEGIN

DECLARE returnvalue DECIMAL(11,6) DEFAULT 0.0;

SELECT SUM(psi.PayAmount)
FROM paystubitem psi
INNER JOIN paystub ps ON ps.EmployeeID=EmpRowID AND ps.OrganizationID=OrganizID AND ps.PayPeriodID=PayPeriodRowID
INNER JOIN product p ON p.RowID=psi.ProductID
WHERE psi.PayStubID=ps.RowID
AND p.Category='Allowance Type'
AND p.PartNo!='Ecola'
INTO returnvalue;

RETURN returnvalue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
