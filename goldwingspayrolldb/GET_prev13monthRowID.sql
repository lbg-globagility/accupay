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

-- Dumping structure for function goldwingspayrolldb.GET_prev13monthRowID
DROP FUNCTION IF EXISTS `GET_prev13monthRowID`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GET_prev13monthRowID`(`OrganizID` INT, `EndOfMonthPayPeriodID` INT, `EmpRowID` INT) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT;

SELECT RowID
FROM thirteenthmonthpay
WHERE OrganizationID=OrganizID
AND PaystubID=(SELECT RowID FROM paystub WHERE PayPeriodID=EndOfMonthPayPeriodID AND OrganizationID=OrganizID AND EmployeeID=EmpRowID)
INTO returnvalue;

RETURN returnvalue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
