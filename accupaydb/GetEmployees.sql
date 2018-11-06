/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `GetEmployees`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `GetEmployees`(
    IN `$OrganizationID` INT,
    IN `$PayDateFrom` DATE,
    IN `$PayDateTo` DATE
)
    DETERMINISTIC
BEGIN

    DECLARE i
	         , payfreq_rowid
	         , payfreqrowid INT(11) DEFAULT 0;
    DECLARE max_limit INT(11);
    DECLARE page_num INT(11) DEFAULT 50;
    DECLARE max_val INT(11) DEFAULT 0;
    
    SELECT pp.TotalGrossSalary
    , pf.RowID
    FROM payperiod pp
    INNER JOIN payfrequency pf ON pf.PayFrequencyType = 'WEEKLY'
    WHERE pp.OrganizationID = $OrganizationID
    AND pp.PayFromDate = $PayDateFrom
    AND pp.PayToDate = $PayDateTo
    LIMIT 1
    INTO payfreq_rowid
	      ,payfreqrowid;
    
    SELECT
        e.RowID,
        IFNULL(dmw.Amount,d.MinimumWageAmount) AS MinimumWageAmount,
        IF(
            e.EmployeeType = 'Daily',
            esal.BasicPay,
            esal.Salary / (e.WorkDaysPerYear / 12)
        ) `EmpRatePerDay`
    FROM employee e
    LEFT JOIN employeesalary esal
    ON e.RowID = esal.EmployeeID
    LEFT JOIN `position` p
    ON p.RowID = e.PositionID
    LEFT JOIN `division` d
    ON d.RowID = p.DivisionId
    LEFT JOIN agency ag
    ON ag.RowID = e.AgencyID
    INNER JOIN payfrequency pf
    ON pf.RowID = e.PayFrequencyID
    LEFT JOIN divisionminimumwage dmw
    ON dmw.OrganizationID = e.OrganizationID AND
        dmw.DivisionID = d.RowID AND
        $PayDateTo BETWEEN dmw.EffectiveDateFrom AND dmw.EffectiveDateTo
    WHERE e.OrganizationID = $OrganizationID AND
        $PayDateTo BETWEEN esal.EffectiveDateFrom AND COALESCE(esal.EffectiveDateTo, $PayDateTo) AND
        e.EmploymentStatus NOT IN ('Resigned', 'Terminated') AND
        e.PayFrequencyID = payfreq_rowid
    GROUP BY e.RowID
    ORDER BY e.LastName;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
