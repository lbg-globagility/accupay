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

-- Dumping structure for procedure RPT_PAGIBIG_Monthly
DROP PROCEDURE IF EXISTS `RPT_PAGIBIG_Monthly`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_PAGIBIG_Monthly`(
	IN `OrganizID` INT,
	IN `paramDate` DATE
)
    DETERMINISTIC
BEGIN

    DECLARE year INT(11);
    DECLARE month INT(11);

    SET year = DATE_FORMAT(paramDate, '%Y');
    SET month = DATE_FORMAT(paramDate, '%m');

    SELECT
        employee.PhilHealthNo,
        CONCAT(
            employee.LastName,
            ',',
            employee.FirstName,
            IF(employee.MiddleName = '', '', ','),
            INITIALS(employee.MiddleName, '. ', '1')
        ) AS Fullname,
        paystubsummary.TotalEmpHDMF,
        paystubsummary.TotalCompHDMF,
        (paystubsummary.TotalEmpHDMF + paystubsummary.TotalCompHDMF) 'TotalContribution'
    FROM employee
    LEFT JOIN (
        SELECT
            paystub.EmployeeID,
            SUM(paystub.TotalEmpHDMF) 'TotalEmpHDMF',
            SUM(paystub.TotalCompHDMF) 'TotalCompHDMF'
        FROM paystub
        INNER JOIN payperiod
        ON payperiod.RowID = paystub.PayPeriodID
        WHERE paystub.OrganizationID = OrganizID AND
            payperiod.Year = year AND
            payperiod.Month = month
        GROUP BY paystub.EmployeeID
    ) paystubsummary
    ON paystubsummary.EmployeeID = employee.RowID
    WHERE employee.OrganizationID = OrganizID;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
