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

-- Dumping structure for function SETGET_thirteenthmonthpay
DROP FUNCTION IF EXISTS `SETGET_thirteenthmonthpay`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `SETGET_thirteenthmonthpay`(`PayStubRowID` INT) RETURNS decimal(11,2)
    DETERMINISTIC
BEGIN

DECLARE returnvalue DECIMAL(11,2) DEFAULT 0;

    SELECT
    SUM(tpay.Amount)
    FROM thirteenthmonthpay tpay
    LEFT JOIN paystub pys ON pys.RowID=PayStubRowID
    LEFT JOIN paystub ps ON YEAR(ps.PayFromDate)=YEAR(pys.PayFromDate) AND YEAR(ps.PayToDate)=YEAR(pys.PayToDate) AND ps.OrganizationID=pys.OrganizationID AND ps.EmployeeID=pys.EmployeeID
    WHERE tpay.PaystubID IN (ps.RowID)
    INTO returnvalue;

UPDATE paystub
SET TotalNetSalary=TotalNetSalary + returnvalue
,TotalGrossSalary = TotalGrossSalary + returnvalue
WHERE RowID=PayStubRowID;

RETURN returnvalue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
