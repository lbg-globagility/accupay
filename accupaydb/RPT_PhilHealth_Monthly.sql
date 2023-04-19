/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_PhilHealth_Monthly`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_PhilHealth_Monthly`(
	IN `OrganizID` INT,
	IN `paramDate` DATE
)
    DETERMINISTIC
BEGIN

    DECLARE year INT(11);
    DECLARE month INT(11);
    DECLARE isMorningSunOwner BOOL DEFAULT FALSE;

    SET isMorningSunOwner = EXISTS(SELECT RowID FROM systemowner WHERE NAME='MorningSun' AND IsCurrentOwner='1');

    SET year = DATE_FORMAT(paramDate, '%Y');
    SET month = DATE_FORMAT(paramDate, '%m');

IF NOT isMorningSunOwner THEN

    SELECT
        emp.PhilHealthNo `DatCol1`,
        CONCAT(
            emp.LastName,
            ',',
            emp.FirstName,
            IF(emp.MiddleName = '', '', ','),
            INITIALS(emp.MiddleName, '. ', '1')
        ) AS `DatCol2`,
        paystubsummary.TotalEmpPhilHealth AS `DatCol3`,
        paystubsummary.TotalCompPhilHealth AS `DatCol4`,
        (paystubsummary.TotalEmpPhilHealth + paystubsummary.TotalCompPhilHealth) AS `DatCol5`
    FROM employee emp
    INNER JOIN (
        SELECT
            paystub.EmployeeID,
            SUM(paystub.TotalEmpPhilhealth) 'TotalEmpPhilHealth',
            SUM(paystub.TotalCompPhilhealth) 'TotalCompPhilHealth'
        FROM paystub
        INNER JOIN payperiod
        ON payperiod.RowID = paystub.PayPeriodID
        WHERE paystub.OrganizationID = OrganizID AND
            payperiod.Year = year AND
            payperiod.Month = month
        GROUP BY paystub.EmployeeID
    ) paystubsummary
    ON paystubsummary.EmployeeID = emp.RowID
    WHERE emp.OrganizationID = OrganizID
    ORDER BY emp.LastName, emp.FirstName;

ELSE

    SELECT
        e.PhilHealthNo `DatCol1`,
        CONCAT_WS(', ', e.LastName, e.FirstName) `DatCol2`,
        SUM(ps.TotalEmpPhilhealth) `DatCol3`,
        SUM(ps.TotalCompPhilhealth) `DatCol4`,
        (SUM(ps.TotalEmpPhilhealth) + SUM(ps.TotalCompPhilhealth)) `DatCol5`
        FROM employee e
        LEFT JOIN paystub ps ON ps.EmployeeID=e.RowID
        LEFT JOIN payperiod pp ON pp.RowID=ps.PayPeriodID AND pp.`Month`=month AND pp.`Year`=year AND pp.TotalGrossSalary=e.PayFrequencyID 

        WHERE e.OrganizationID=OrganizID
        GROUP BY e.RowID
        ORDER BY CONCAT(e.LastName, e.FirstName)
        ;

END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
