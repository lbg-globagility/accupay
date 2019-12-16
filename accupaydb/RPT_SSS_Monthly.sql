/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_SSS_Monthly`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_SSS_Monthly`(
	IN `OrganizID` INT,
	IN `paramDate` DATE
)
    DETERMINISTIC
BEGIN

    DECLARE year INT(10);
    DECLARE month INT(10);

    SET month = DATE_FORMAT(paramDate, '%m');
    SET year = DATE_FORMAT(paramDate, '%Y');

    SELECT
        employee.SSSNo `DatCol1`,
        CONCAT(
            employee.LastName,
            ', ',
            employee.FirstName,
            IF(
                employee.MiddleName = '',
                '',
                ' '
            ),
            INITIALS(employee.MiddleName, '. ', '1')
        ) AS `DatCol2`,
        paystubsummary.TotalEmpSSS  AS `DatCol3`,
        paysocialsecurity.EmployerContributionAmount AS `DatCol4`,
        paysocialsecurity.EmployeeECAmount AS `DatCol5`,
        (paystubsummary.TotalEmpSSS + paystubsummary.TotalCompSSS) AS `DatCol6`
    FROM employee
    INNER JOIN (
        SELECT
            SUM(paystub.TotalEmpSSS) AS TotalEmpSSS,
            SUM(paystub.TotalCompSSS) AS TotalCompSSS,
            paystub.EmployeeID
        FROM paystub
        INNER JOIN payperiod
        ON payperiod.RowID = paystub.PayPeriodID
        WHERE
            paystub.OrganizationID = OrganizID AND
            payperiod.Year = year AND
            payperiod.Month = month
        GROUP BY paystub.EmployeeID
    ) paystubsummary
    ON paystubsummary.EmployeeID = employee.RowID
    LEFT JOIN paysocialsecurity
    ON paysocialsecurity.EmployeeContributionAmount = paystubsummary.TotalEmpSSS
    WHERE employee.OrganizationID = OrganizID
    ORDER BY employee.LastName, employee.FirstName;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
