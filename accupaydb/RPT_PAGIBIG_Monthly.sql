/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

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
        employee.HDMFNo `DatCol1`,
        CONCAT(
            employee.LastName,
            ',',
            employee.FirstName,
            IF(employee.MiddleName = '', '', ','),
            INITIALS(employee.MiddleName, '. ', '1')
        ) AS `DatCol2`,
        paystubsummary.TotalEmpHDMF AS `DatCol3`,
        paystubsummary.TotalCompHDMF AS `DatCol4`,
        (paystubsummary.TotalEmpHDMF + paystubsummary.TotalCompHDMF) AS `DatCol5`
    FROM employee
    INNER JOIN (
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
    WHERE employee.OrganizationID = OrganizID
	ORDER BY employee.LastName, employee.FirstName;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
