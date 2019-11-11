/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `EMPLOYEE_payrollgen`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `EMPLOYEE_payrollgen`(
    IN `OrganizID` INT,
    IN `Pay_Date_From` DATE,
    IN `Pay_Date_To` DATE,
    IN `SelectedEmpRowID` VARCHAR(50)


)
    DETERMINISTIC
BEGIN

IF SelectedEmpRowID IS NULL THEN

    SELECT
    e.RowID
    ,e.EmployeeID,e.MaritalStatus
    ,e.NoOfDependents
    ,e.PayFrequencyID
    ,e.EmployeeType
    ,e.EmploymentStatus
    ,e.WorkDaysPerYear
    ,e.PositionID
    ,IF(e.AgencyID IS NOT NULL, IFNULL(d.PhHealthDeductSchedAgency,d.PhHealthDeductSched), d.PhHealthDeductSched) AS PhHealthDeductSched
    ,IF(e.AgencyID IS NOT NULL, IFNULL(d.HDMFDeductSchedAgency,d.HDMFDeductSched), d.HDMFDeductSched) AS HDMFDeductSched
    ,IF(e.AgencyID IS NOT NULL, IFNULL(d.SSSDeductSchedAgency,d.SSSDeductSched), d.SSSDeductSched) AS SSSDeductSched
    ,IF(e.AgencyID IS NOT NULL, IFNULL(d.WTaxDeductSchedAgency,d.WTaxDeductSched), d.WTaxDeductSched) AS WTaxDeductSched
    ,PAYFREQUENCY_DIVISOR(pf.PayFrequencyType) 'PAYFREQUENCY_DIVISOR'
    ,ROUND(GET_employeerateperday(e.RowID, e.OrganizationID, Pay_Date_To),2) 'EmpRatePerDay'

    ,IFNULL(dmw.Amount,d.MinimumWageAmount) AS MinimumWageAmount
    ,(e.StartDate BETWEEN Pay_Date_From AND Pay_Date_To) AS IsFirstTimeSalary
    ,GET_employeeStartingAttendanceCount(e.RowID,Pay_Date_From,Pay_Date_To) AS StartingAttendanceCount
    FROM employee e
    LEFT JOIN employeesalary esal ON e.RowID=esal.EmployeeID
    LEFT JOIN position p ON p.RowID=e.PositionID
    LEFT JOIN `division` d ON d.RowID=p.DivisionId
    LEFT JOIN agency ag ON ag.RowID=e.AgencyID
    INNER JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID
    LEFT JOIN divisionminimumwage dmw ON dmw.OrganizationID=e.OrganizationID AND dmw.DivisionID=d.RowID AND Pay_Date_To BETWEEN dmw.EffectiveDateFrom AND dmw.EffectiveDateTo
    WHERE e.OrganizationID=OrganizID AND Pay_Date_To BETWEEN esal.EffectiveDateFrom AND COALESCE(esal.EffectiveDateTo,Pay_Date_To)
    GROUP BY e.RowID
    ORDER BY e.RowID DESC;

ELSE

    SELECT
    e.RowID
    ,e.EmployeeID,e.MaritalStatus
    ,e.NoOfDependents
    ,e.PayFrequencyID
    ,e.EmployeeType
    ,e.EmploymentStatus
    ,e.WorkDaysPerYear
    ,e.PositionID
    ,IF(e.AgencyID IS NOT NULL, IFNULL(d.PhHealthDeductSchedAgency,d.PhHealthDeductSched), d.PhHealthDeductSched) AS PhHealthDeductSched
    ,IF(e.AgencyID IS NOT NULL, IFNULL(d.HDMFDeductSchedAgency,d.HDMFDeductSched), d.HDMFDeductSched) AS HDMFDeductSched
    ,IF(e.AgencyID IS NOT NULL, IFNULL(d.SSSDeductSchedAgency,d.SSSDeductSched), d.SSSDeductSched) AS SSSDeductSched
    ,IF(e.AgencyID IS NOT NULL, IFNULL(d.WTaxDeductSchedAgency,d.WTaxDeductSched), d.WTaxDeductSched) AS WTaxDeductSched
    ,PAYFREQUENCY_DIVISOR(pf.PayFrequencyType) 'PAYFREQUENCY_DIVISOR'
    ,ROUND(GET_employeerateperday(e.RowID, e.OrganizationID, Pay_Date_To),2) 'EmpRatePerDay'

    ,IFNULL(dmw.Amount,d.MinimumWageAmount) AS MinimumWageAmount
    ,(e.StartDate BETWEEN Pay_Date_From AND Pay_Date_To) AS IsFirstTimeSalary
    ,GET_employeeStartingAttendanceCount(e.RowID,Pay_Date_From,Pay_Date_To) AS StartingAttendanceCount
    FROM employee e
    LEFT JOIN employeesalary esal ON e.RowID=esal.EmployeeID
    LEFT JOIN position p ON p.RowID=e.PositionID
    LEFT JOIN `division` d ON d.RowID=p.DivisionId
    LEFT JOIN agency ag ON ag.RowID=e.AgencyID
    INNER JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID
    LEFT JOIN divisionminimumwage dmw ON dmw.OrganizationID=e.OrganizationID AND dmw.DivisionID=d.RowID AND Pay_Date_To BETWEEN dmw.EffectiveDateFrom AND dmw.EffectiveDateTo
    WHERE e.OrganizationID=OrganizID AND Pay_Date_To BETWEEN esal.EffectiveDateFrom AND COALESCE(esal.EffectiveDateTo,Pay_Date_To)


    AND e.EmployeeID=SelectedEmpRowID
    GROUP BY e.RowID
    ORDER BY e.RowID DESC;

END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
