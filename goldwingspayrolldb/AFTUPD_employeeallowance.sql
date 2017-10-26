/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTUPD_employeeallowance`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_employeeallowance` AFTER UPDATE ON `employeeallowance` FOR EACH ROW BEGIN

DECLARE viewID INT(11);

DECLARE totalAllowancePerDay DECIMAL(11,2) DEFAULT 0;

DECLARE empPaymentType TEXT;

DECLARE empWorkDaysPerYear DECIMAL(11,2);

DECLARE ag_RowID INT(11);


SELECT e.AgencyID FROM employee e WHERE e.RowID=NEW.EmployeeID INTO ag_RowID;

SELECT RowID FROM `view` WHERE ViewName='Employee Allowance' AND OrganizationID=NEW.OrganizationID LIMIT 1 INTO viewID;




IF NEW.TaxableFlag = '1' AND ag_RowID IS NULL THEN

    SELECT e.EmployeeType,e.WorkDaysPerYear FROM employee e WHERE e.RowID=NEW.EmployeeID INTO empPaymentType,empWorkDaysPerYear;

    SELECT GET_employeeallowancePerDay(NEW.OrganizationID,NEW.EmployeeID,NEW.TaxableFlag,CURDATE()) INTO totalAllowancePerDay;



        SET empWorkDaysPerYear = ROUND(empWorkDaysPerYear / 12, 0);

        SET totalAllowancePerDay = totalAllowancePerDay * empWorkDaysPerYear;

    IF empPaymentType IN ('Fixed','Monthly') THEN

        IF NEW.AllowanceFrequency = 'Semi-monthly' THEN

            UPDATE employeesalary es SET
            es.PaySocialSecurityID=(SELECT RowID FROM paysocialsecurity WHERE (es.Salary + (NEW.AllowanceAmount * 2.0)) BETWEEN RangeFromAmount AND RangeToAmount)
            ,es.PayPhilhealthID=(SELECT RowID FROM payphilhealth WHERE (es.Salary + (NEW.AllowanceAmount * 2.0)) BETWEEN SalaryRangeFrom AND SalaryRangeTo)
            ,es.LastUpdBy=NEW.CreatedBy
            WHERE es.EmployeeID=NEW.EmployeeID
            AND es.OrganizationID=NEW.OrganizationID
            AND (es.EffectiveDateFrom >= NEW.EffectiveStartDate OR IFNULL(es.EffectiveDateTo,NEW.EffectiveEndDate) >= NEW.EffectiveStartDate)
            AND (es.EffectiveDateFrom <= NEW.EffectiveEndDate OR IFNULL(es.EffectiveDateTo,NEW.EffectiveEndDate) <= NEW.EffectiveEndDate);

        ELSEIF NEW.AllowanceFrequency = 'Daily' THEN

            UPDATE employeesalary es SET
            es.PaySocialSecurityID=(SELECT RowID FROM paysocialsecurity WHERE (es.Salary + totalAllowancePerDay) BETWEEN RangeFromAmount AND RangeToAmount)
            ,es.PayPhilhealthID=(SELECT RowID FROM payphilhealth WHERE (es.Salary + totalAllowancePerDay) BETWEEN SalaryRangeFrom AND SalaryRangeTo)
            ,es.LastUpdBy=NEW.CreatedBy
            WHERE es.EmployeeID=NEW.EmployeeID
            AND es.OrganizationID=NEW.OrganizationID
            AND (es.EffectiveDateFrom >= NEW.EffectiveStartDate OR IFNULL(es.EffectiveDateTo,NEW.EffectiveEndDate) >= NEW.EffectiveStartDate)
            AND (es.EffectiveDateFrom <= NEW.EffectiveEndDate OR IFNULL(es.EffectiveDateTo,NEW.EffectiveEndDate) <= NEW.EffectiveEndDate);

        END IF;

    ELSEIF empPaymentType = 'Daily' THEN

        IF NEW.AllowanceFrequency = 'Semi-monthly' THEN

            UPDATE employeesalary es
            INNER JOIN employee e ON e.RowID=es.EmployeeID
            INNER JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID
            SET
            es.PaySocialSecurityID=(SELECT RowID FROM paysocialsecurity WHERE (((es.BasicPay + NEW.AllowanceAmount) * empWorkDaysPerYear) + (NEW.AllowanceAmount * PAYFREQUENCY_DIVISOR(pf.PayFrequencyType))) BETWEEN RangeFromAmount AND RangeToAmount)
            ,es.PayPhilhealthID=(SELECT RowID FROM payphilhealth WHERE (es.BasicPay * empWorkDaysPerYear) BETWEEN SalaryRangeFrom AND SalaryRangeTo LIMIT 1)
            ,es.LastUpdBy=NEW.CreatedBy
            ,es.LastUpd=CURRENT_TIMESTAMP()
            WHERE es.EmployeeID=NEW.EmployeeID
            AND es.OrganizationID=NEW.OrganizationID
            AND es.EffectiveDateTo IS NULL;

        ELSEIF NEW.AllowanceFrequency = 'Daily' THEN

            UPDATE employeesalary es SET
            es.PaySocialSecurityID=(SELECT RowID FROM paysocialsecurity WHERE ((es.BasicPay + NEW.AllowanceAmount) * empWorkDaysPerYear) BETWEEN RangeFromAmount AND RangeToAmount)
            ,es.PayPhilhealthID=(SELECT RowID FROM payphilhealth WHERE (es.BasicPay * empWorkDaysPerYear) BETWEEN SalaryRangeFrom AND SalaryRangeTo LIMIT 1)
            ,es.LastUpdBy=NEW.CreatedBy
            ,es.LastUpd=CURRENT_TIMESTAMP()
            WHERE es.EmployeeID=NEW.EmployeeID
            AND es.OrganizationID=NEW.OrganizationID
            AND es.EffectiveDateTo IS NULL;

        END IF;



    ELSEIF empPaymentType = 'Hourly' THEN

        UPDATE employeesalary es SET
        es.PaySocialSecurityID=(SELECT RowID FROM paysocialsecurity WHERE es.BasicPay + totalAllowancePerDay BETWEEN RangeFromAmount AND RangeToAmount LIMIT 1)
        ,es.LastUpdBy=NEW.CreatedBy
        WHERE es.EmployeeID=NEW.EmployeeID
        AND es.OrganizationID=NEW.OrganizationID
        AND es.EffectiveDateTo IS NULL;



    END IF;

END IF;


IF OLD.ProductID!=NEW.ProductID THEN

    INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'ProductID',NEW.RowID,OLD.ProductID,NEW.ProductID,'Update');

END IF;

IF OLD.AllowanceFrequency!=NEW.AllowanceFrequency THEN

    INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'AllowanceFrequency',NEW.RowID,OLD.AllowanceFrequency,NEW.AllowanceFrequency,'Update');

END IF;

IF OLD.EffectiveStartDate!=NEW.EffectiveStartDate THEN

    INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'EffectiveStartDate',NEW.RowID,OLD.EffectiveStartDate,NEW.EffectiveStartDate,'Update');

END IF;

IF OLD.EffectiveEndDate!=NEW.EffectiveEndDate THEN

    INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'EffectiveEndDate',NEW.RowID,OLD.EffectiveEndDate,NEW.EffectiveEndDate,'Update');

END IF;

IF OLD.TaxableFlag!=NEW.TaxableFlag THEN

    INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'TaxableFlag',NEW.RowID,OLD.TaxableFlag,NEW.TaxableFlag,'Update');

END IF;

IF OLD.AllowanceAmount!=NEW.AllowanceAmount THEN

    INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'AllowanceAmount',NEW.RowID,OLD.AllowanceAmount,NEW.AllowanceAmount,'Update');

END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
