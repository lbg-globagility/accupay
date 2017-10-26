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

-- Dumping structure for procedure VIEW_employeeloan_indate
DROP PROCEDURE IF EXISTS `VIEW_employeeloan_indate`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeeloan_indate`(
    IN `eloan_EmployeeID` INT,
    IN `eloan_OrganizationID` INT,
    IN `effectivedatefrom` DATE,
    IN `effectivedateto` DATE
)
    DETERMINISTIC
BEGIN

DECLARE isEndOfMonth CHAR(1);
DECLARE month_type VARCHAR(150);
DECLARE payPeriodID INT;

SELECT pp.RowID, pp.`Half`
FROM payperiod pp
WHERE pp.OrganizationID = eloan_OrganizationID AND
    pp.PayFromDate = effectivedatefrom AND
    pp.PayToDate = effectivedateto AND
    pp.TotalGrossSalary = 1
INTO payPeriodID, isEndOfMonth;

SET month_type = IF(isEndOfMonth = '0', 'End of the month', 'First half');

SELECT
    IFNULL(l.LoanNumber, '') `LoanNumber`,
    IFNULL(FORMAT(l.TotalLoanAmount, 2), 0.00) `TotalLoanAmount`,
    IFNULL(FORMAT(s.TotalBalanceLeft, 2), 0.00) `TotalBalanceLeft`,
    IFNULL(FORMAT(l.DeductionAmount, 2), 0.00) `DeductionAmount`,
    IFNULL(FORMAT(l.DeductionPercentage, 2), 0.00) `DeductionPercentage`,
    IFNULL(l.DeductionSchedule, '') `DeductionSchedule`,
    IFNULL(l.NoOfPayPeriod, 0) `NoOfPayPeriod`,
    IFNULL(l.Comments, '') `Comments`,
    l.RowID,
    IFNULL(l.`Status`, '') `Status`,
    p.PartNo
FROM scheduledloansperpayperiod s
INNER JOIN employeeloanschedule l
ON l.RowID = s.EmployeeLoanRecordID
LEFT JOIN product p
ON p.RowID = l.LoanTypeID
WHERE s.EmployeeID = eloan_EmployeeID AND
    s.PayPeriodID = payPeriodID;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
