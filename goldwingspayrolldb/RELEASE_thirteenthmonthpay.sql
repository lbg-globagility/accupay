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

-- Dumping structure for procedure RELEASE_thirteenthmonthpay
DROP PROCEDURE IF EXISTS `RELEASE_thirteenthmonthpay`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `RELEASE_thirteenthmonthpay`(IN `OrganizID` INT, IN `PayPeriodRowID` INT, IN `UserRowID` INT)
    DETERMINISTIC
BEGIN

DECLARE annual_first_date DATE;

DECLARE annual_last_date DATE;

DECLARE final_year INT(11);
DECLARE pay_freqID INT(11);
SELECT pp.`Year`,pp.TotalGrossSalary FROM payperiod pp WHERE pp.RowID=PayPeriodRowID INTO final_year,pay_freqID;

SELECT pp.PayToDate FROM payperiod pp WHERE pp.`Year`=final_year AND pp.TotalGrossSalary=pay_freqID AND pp.OrganizationID=OrganizID ORDER BY pp.PayFromDate,pp.PayToDate LIMIT 1 INTO annual_first_date;

SELECT pp.PayToDate FROM payperiod pp WHERE pp.`Year`=final_year AND pp.TotalGrossSalary=pay_freqID AND pp.OrganizationID=OrganizID ORDER BY pp.PayFromDate DESC,pp.PayToDate DESC LIMIT 1 INTO annual_last_date;



UPDATE paystub ps
INNER JOIN
(
    SELECT tmp.*
    ,SUM(tmp.Amount) AS tmpAmount
    ,ps.EmployeeID
    FROM thirteenthmonthpay tmp
    INNER JOIN paystub ps ON ps.RowID=tmp.PaystubID AND ps.OrganizationID=tmp.OrganizationID AND (ps.PayFromDate >= annual_first_date OR ps.PayToDate >= annual_first_date) AND (ps.PayFromDate <= annual_last_date OR ps.PayToDate <= annual_last_date)
    WHERE tmp.OrganizationID=OrganizID
    GROUP BY ps.EmployeeID
) ii ON ii.EmployeeID=ps.EmployeeID
SET ps.TotalGrossSalary = ps.TotalGrossSalary + ii.tmpAmount
,ps.TotalNetSalary = ps.TotalNetSalary + ii.tmpAmount
,ps.ThirteenthMonthInclusion = '1'
WHERE ps.OrganizationID=OrganizID
AND ps.PayPeriodID=PayPeriodRowID
AND ps.ThirteenthMonthInclusion = '0';


END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
