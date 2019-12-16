/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFINS_employeepromotions`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFINS_employeepromotions` BEFORE INSERT ON `employeepromotions` FOR EACH ROW BEGIN

DECLARE e_salID INT(11);

DECLARE computed_monthlysalary DECIMAL(11,6);

DECLARE emp_type VARCHAR(50);

DECLARE curr_timestamp DATETIME;


IF NEW.CompensationChange = '1' THEN

SELECT IF(e.EmployeeType IN ('Fixed','Monthly'), NEW.CompensationValue, (NEW.CompensationValue * (e.WorkDaysPerYear / 12.0))),e.EmployeeType FROM employee e WHERE e.RowID=NEW.EmployeeID INTO computed_monthlysalary,emp_type;

    SELECT CURRENT_TIMESTAMP() INTO curr_timestamp;

    INSERT INTO employeesalary
    (
        EmployeeID
        ,Created
        ,CreatedBy
        ,OrganizationID
        ,FilingStatusID
        ,PaySocialSecurityID
        ,PayPhilhealthID
        ,HDMFAmount
        ,TrueSalary
        ,BasicPay
        ,Salary
        ,BasicDailyPay
        ,BasicHourlyPay
        ,NoofDependents
        ,MaritalStatus
        ,PositionID
        ,EffectiveDateFrom
        ,EffectiveDateTo
    ) SELECT
        NEW.EmployeeID
        ,curr_timestamp
        ,NEW.CreatedBy
        ,NEW.OrganizationID
        ,fs.RowID
        , pss.RowID
        , phh.RowID
        , 100.0
        , NEW.CompensationValue
        , NEW.CompensationValue / PAYFREQUENCY_DIVISOR(pf.PayFrequencyType)
        , NEW.CompensationValue
        , IF(emp_type='Daily', NEW.CompensationValue, 0)
        , 0
        , e.NoOfDependents
        , e.MaritalStatus
        , pos.RowID
        , NEW.EffectiveDate
        , NULL
    FROM employee e
    INNER JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID
    INNER JOIN filingstatus fs ON fs.MaritalStatus=e.MaritalStatus AND fs.Dependent=e.NoOfDependents
    INNER JOIN (SELECT RowID FROM position WHERE PositionName=NEW.PositionTo AND OrganizationID=NEW.OrganizationID LIMIT 1) pos ON pos.RowID > 0
    INNER JOIN paysocialsecurity pss ON computed_monthlysalary BETWEEN pss.RangeFromAmount AND pss.RangeToAmount
    INNER JOIN payphilhealth phh ON computed_monthlysalary BETWEEN phh.SalaryRangeFrom AND phh.SalaryRangeTo
    WHERE e.RowID=NEW.EmployeeID;SELECT @@Identity AS ID INTO e_salID;

    SET NEW.EmployeeSalaryID = e_salID;





ELSE

    SELECT RowID FROM employeesalary WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID ORDER BY EffectiveDateFrom DESC LIMIT 1 INTO e_salID;

    SET NEW.EmployeeSalaryID = e_salID;

END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
