/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `GET_rawgrossincome`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GET_rawgrossincome`(`OrganizID` INT, `EmpID` INT, `PayPeriodF` DATE, `PayPeriodT` DATE) RETURNS text CHARSET latin1
    DETERMINISTIC
BEGIN

DECLARE returnvalue TEXT;

SELECT psi.PayAmount
FROM paystub ps
INNER JOIN product p ON p.PartNo='Gross Income' AND p.OrganizationID=OrganizID
INNER JOIN paystubitem psi ON psi.ProductID=p.RowID AND psi.PayStubID=ps.RowID
WHERE ps.EmployeeID=EmpID
AND ps.OrganizationID=OrganizID
AND ps.PayFromDate=PayPeriodF
AND ps.PayToDate=PayPeriodT
INTO returnvalue;

RETURN returnvalue;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
