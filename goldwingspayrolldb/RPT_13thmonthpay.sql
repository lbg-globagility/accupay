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

-- Dumping structure for procedure RPT_13thmonthpay
DROP PROCEDURE IF EXISTS `RPT_13thmonthpay`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_13thmonthpay`(IN `OrganizID` INT, IN `paramYear` INT)
    DETERMINISTIC
BEGIN

SELECT






e.EmployeeID AS DatCol1
,CONCAT_WS(', ',IF(e.LastName = '', NULL, e.LastName),e.FirstName) AS DatCol2
,FORMAT(SUM(ttmp.Amount),2) AS DatCol3
FROM thirteenthmonthpay ttmp
INNER JOIN paystub ps ON ps.RowID=ttmp.PaystubID
INNER JOIN employee e ON e.RowID=ps.EmployeeID AND e.OrganizationID=OrganizID
INNER JOIN payperiod pyp ON pyp.RowID=ps.PayPeriodID AND pyp.OrganizationID=OrganizID
WHERE ttmp.OrganizationID=OrganizID
AND YEAR(ps.PayFromDate)=paramYear
GROUP BY ps.EmployeeID;
END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
