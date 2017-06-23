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

-- Dumping structure for function goldwingspayrolldb.FN_GetPaystubIDByEmployeeIDAndPayPeriodID
DROP FUNCTION IF EXISTS `FN_GetPaystubIDByEmployeeIDAndPayPeriodID`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `FN_GetPaystubIDByEmployeeIDAndPayPeriodID`(`ps_EmployeeID` VARCHAR(50), `ps_PayPeriodID` INT, `OrganizID` INT) RETURNS int(11)
    DETERMINISTIC
BEGIN

RETURN
IFNULL(
(
	SELECT 
		RowID 
	FROM 
		PayStub 
	WHERE 
		EmployeeID = (
							SELECT 
								RowID 
							FROM 
								employee 
							WHERE 
								EmployeeID = ps_EmployeeID
						  	AND
						  		OrganizationID = OrganizID
						  ) AND 
		PayPeriodID = ps_PayPeriodID
	AND 
		OrganizationID = OrganizID
)
, NULL);

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
