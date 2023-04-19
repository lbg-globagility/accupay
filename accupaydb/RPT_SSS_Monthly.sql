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
    DECLARE isMorningSunOwner BOOL DEFAULT FALSE;

    SET isMorningSunOwner = EXISTS(SELECT RowID FROM systemowner WHERE NAME='MorningSun' AND IsCurrentOwner='1');

    SET month = DATE_FORMAT(paramDate, '%m');
    SET year = DATE_FORMAT(paramDate, '%Y');
    
IF NOT isMorningSunOwner THEN

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
    WHERE employee.OrganizationID = OrganizID
    ORDER BY employee.LastName, employee.FirstName;

ELSE
	
	SET @loanCategoryId=(SELECT c.RowID FROM category c WHERE c.OrganizationID=OrganizID AND c.CategoryName='Loan Type' LIMIT 1);
	
	SET @_divisor=(SELECT og.PayFrequencyID FROM `organization` og WHERE og.RowID=OrganizID);
	
	DROP TEMPORARY TABLE IF EXISTS `SssPremium`;
	CREATE TEMPORARY TABLE IF NOT EXISTS `SssPremium`
	SELECT
	els.RowID,
	els.SssEmployeeShare / @_divisor `SssEmployeeShare`,
	els.SssEmployerShare / @_divisor `SssEmployerShare`,
	els.SssEcEmployerShare / @_divisor `SssEcEmployerShare`,
	els.SssWispEmployeeShare / @_divisor `SssWispEmployeeShare`,
	els.SssWispEmployerShare / @_divisor `SssWispEmployerShare`,
	s.RowID `LoanTransactionId`,
	s.PaystubID
	FROM employeeloanschedule els
	INNER JOIN product p ON p.RowID=els.LoanTypeID AND p.CategoryID=@loanCategoryId AND p.GovtDeductionType='SSS'
	INNER JOIN scheduledloansperpayperiod s ON s.EmployeeLoanRecordID=els.RowID
	WHERE els.OrganizationID=OrganizID
	;
	
    SELECT
        e.SSSNo `DatCol1`,
        CONCAT_WS(', ', e.LastName, e.FirstName) `DatCol2`,
        (SUM(ps.TotalEmpSSS) - SUM(els.SssWispEmployeeShare)) `DatCol3`,
        SUM(ps.TotalCompSSS) `DatCol4`,
        SUM(els.SssEcEmployerShare) `DatCol5`,
        SUM(els.SssWispEmployeeShare) `DatCol7`,
        SUM(els.SssWispEmployerShare) `DatCol8`,
        (SUM(ps.TotalEmpSSS) + SUM(ps.TotalCompSSS) + SUM(els.SssEcEmployerShare) + SUM(els.SssWispEmployeeShare) + SUM(els.SssWispEmployerShare)) `DatCol6`
        FROM employee e
        LEFT JOIN paystub ps ON ps.EmployeeID=e.RowID
        LEFT JOIN payperiod pp ON pp.RowID=ps.PayPeriodID AND pp.`Month`=month AND pp.`Year`=year AND pp.TotalGrossSalary=e.PayFrequencyID 
        LEFT JOIN `SssPremium` els ON els.PaystubID=ps.RowID
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
