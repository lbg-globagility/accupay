/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_SSS_Monthly`;
DELIMITER //
CREATE PROCEDURE `RPT_SSS_Monthly`(
	IN `OrganizID` INT,
	IN `paramDate` DATE
)
    DETERMINISTIC
BEGIN

    DECLARE year INT(10);
    DECLARE month INT(10);

    SET month = DATE_FORMAT(paramDate, '%m');
    SET year = DATE_FORMAT(paramDate, '%Y');

    DROP TEMPORARY TABLE IF EXISTS `NonPayPeriodSchedPayroll`;
    CREATE TEMPORARY TABLE IF NOT EXISTS `NonPayPeriodSchedPayroll`
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
        (paystubsummary.TotalEmpSSS - paysocialsecurity.EmployeeMPFAmount)  AS `DatCol3`,
        paysocialsecurity.EmployerContributionAmount AS `DatCol4`,
        paysocialsecurity.EmployerECAmount AS `DatCol5`,
        paysocialsecurity.EmployeeMPFAmount AS `DatCol7`,
        paysocialsecurity.EmployerMPFAmount AS `DatCol8`,
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
    ON paysocialsecurity.EmployeeContributionAmount = (paystubsummary.TotalEmpSSS - paysocialsecurity.EmployeeMPFAmount)
    AND paramDate BETWEEN paysocialsecurity.EffectiveDateFrom AND paysocialsecurity.EffectiveDateTo
    INNER JOIN `position` pos ON pos.RowID=employee.PositionID
    INNER JOIN division d ON d.RowID=pos.DivisionId AND d.SSSDeductSched!='Per pay period'
    WHERE employee.OrganizationID = OrganizID
    ORDER BY employee.LastName, employee.FirstName;

	DROP TEMPORARY TABLE IF EXISTS `PayPeriodSchedPayroll`;
	CREATE TEMPORARY TABLE IF NOT EXISTS `PayPeriodSchedPayroll`
	SELECT
	e.SSSNo `DatCol1`,
	CONCAT_WS(', ', e.LastName, e.FirstName) `DatCol2`,
	SUM(sss.EmployeeContributionAmount / 2) `DatCol3`,
	SUM(sss.EmployerContributionAmount / 2) `DatCol4`,
	SUM(sss.EmployerECAmount / 2) `DatCol5`,
	SUM(sss.EmployeeMPFAmount / 2) `DatCol7`,
	SUM(sss.EmployerMPFAmount / 2) `DatCol8`,
	SUM(ps.TotalEmpSSS + ps.TotalCompSSS) `DatCol6`
	
	FROM paystub ps
	INNER JOIN payperiod pp ON pp.RowID=ps.PayPeriodID AND pp.`Month`=month AND pp.`Year`=year
	INNER JOIN employee e ON e.RowID=ps.EmployeeID
	INNER JOIN `position` pos ON pos.RowID=e.PositionID
	INNER JOIN division d ON d.RowID=pos.DivisionId AND d.SSSDeductSched = 'Per pay period'
	LEFT JOIN paysocialsecurity sss ON ps.PayFromDate BETWEEN sss.EffectiveDateFrom AND sss.EffectiveDateTo
	AND (sss.EmployeeContributionAmount + sss.EmployeeMPFAmount) = (ps.TotalEmpSSS * 2)
	WHERE ps.OrganizationID=OrganizID
	GROUP BY ps.EmployeeID
	;

	SELECT
	t.*
	FROM (SELECT
			i.*
			FROM `NonPayPeriodSchedPayroll` i
		UNION
			SELECT
			ii.*
			FROM `PayPeriodSchedPayroll` ii
			) t
	WHERE CONCAT(t.`DatCol3`, t.`DatCol4`, t.`DatCol5`, t.`DatCol7`, t.`DatCol8`) IS NOT NULL
	ORDER BY t.`DatCol1`
;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
