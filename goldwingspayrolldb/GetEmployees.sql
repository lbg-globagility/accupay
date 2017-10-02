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

    DECLARE i INT(11) DEFAULT 0;
    DECLARE max_limit INT(11);
    DECLARE page_num INT(11) DEFAULT 50;
    DECLARE max_val INT(11) DEFAULT 0;

    SELECT
        e.RowID,
        e.EmployeeID,
        e.MaritalStatus,
        e.NoOfDependents,
        e.PayFrequencyID,
        e.EmployeeType,
        e.EmploymentStatus,
        e.WorkDaysPerYear,
        e.PositionID,
        IF(
            e.AgencyID IS NOT NULL,
            IFNULL(
                d.PhHealthDeductSchedAgency,
                d.PhHealthDeductSched
            ),
            d.PhHealthDeductSched
        ) AS PhHealthDeductSched,
        IF(
            e.AgencyID IS NOT NULL,
            IFNULL(
                d.HDMFDeductSchedAgency,
                d.HDMFDeductSched
            ),
            d.HDMFDeductSched
        ) AS HDMFDeductSched,
        IF(
            e.AgencyID IS NOT NULL,
            IFNULL(
                d.SSSDeductSchedAgency,
                d.SSSDeductSched
            ),
            d.SSSDeductSched
        ) AS SSSDeductSched,
        IF(
            e.AgencyID IS NOT NULL,
            IFNULL(
                d.WTaxDeductSchedAgency,
                d.WTaxDeductSched
            ),
            d.WTaxDeductSched
        ) AS WTaxDeductSched,
        PAYFREQUENCY_DIVISOR(pf.PayFrequencyType) 'PAYFREQUENCY_DIVISOR',
        IFNULL(dmw.Amount,d.MinimumWageAmount) AS MinimumWageAmount,
        (e.StartDate BETWEEN $PayDateFrom AND $PayDateTo) AS IsFirstTimeSalary,
        IF(
            e.EmployeeType = 'Daily',
            esal.BasicPay,
            esal.Salary / (e.WorkDaysPerYear / 12)
        ) `EmpRatePerDay`,
        IFNULL(et.`etcount`, 0) AS StartingAttendanceCount
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
    LEFT JOIN (
        SELECT COUNT(RowID) `etcount`, EmployeeID
        FROM employeetimeentry
        WHERE TotalDayPay != 0 AND
            OrganizationID = $OrganizationID AND
            `Date` BETWEEN $PayDateFrom AND $PayDateTo
        GROUP BY EmployeeID
    ) et
    ON et.EmployeeID = e.RowID
    WHERE e.OrganizationID = $OrganizationID AND
        $PayDateTo BETWEEN esal.EffectiveDateFrom AND COALESCE(esal.EffectiveDateTo, $PayDateTo)
    GROUP BY e.RowID
    ORDER BY e.LastName;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
