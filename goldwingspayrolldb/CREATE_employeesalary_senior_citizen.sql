/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `CREATE_employeesalary_senior_citizen`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `CREATE_employeesalary_senior_citizen`(IN `OrganizID` INT, IN `UserRowID` INT)
    DETERMINISTIC
BEGIN

DECLARE hasalreadyseniorcitizensalary CHAR(1);

DECLARE userhasprivilege CHAR(1);

SET @thisyearfirstdate = MAKEDATE(YEAR(CURDATE()),1);

SET @negativedatediff = 0;

SELECT (pv.Creates = 'Y' AND pv.Updates = 'Y')
FROM position_view pv
INNER JOIN user u ON u.RowID=UserRowID
INNER JOIN `view` v ON v.RowID=pv.ViewID
WHERE pv.PositionID=u.PositionID
AND pv.OrganizationID=OrganizID
LIMIT 1
INTO userhasprivilege;

IF userhasprivilege IS NULL THEN
    SET userhasprivilege = '0';
END IF;

SELECT EXISTS(SELECT RowID
                    FROM employeesalary
                    WHERE EmployeeID IN (SELECT RowID
                                                FROM employee e
                                                WHERE e.OrganizationID=OrganizID
                                                AND e.StartDate < @thisyearfirstdate
                                                AND TIMESTAMPDIFF(YEAR,e.Birthdate, CURDATE()) >= 59
                                                AND e.EmploymentStatus IN ('Regular','Probationary'))
                    AND OrganizationID=OrganizID
                    AND EffectiveDateFrom=@thisyearfirstdate
                    AND DATE_FORMAT(Created,'%Y-%m-%d')<=CURDATE())
INTO hasalreadyseniorcitizensalary;

IF hasalreadyseniorcitizensalary = '0' AND userhasprivilege = '1' THEN

    UPDATE employeesalary es
    INNER JOIN employee e ON e.OrganizationID=OrganizID AND e.StartDate < @thisyearfirstdate AND TIMESTAMPDIFF(YEAR,e.Birthdate, CURDATE()) >= 59 AND e.EmploymentStatus IN ('Regular','Probationary') AND es.EmployeeID=e.RowID
    SET es.PaySocialSecurityID=NULL
    ,es.PayPhilhealthID=NULL
    ,es.HDMFAmount=0
    ,es.LastUpd=CURRENT_TIMESTAMP()
    ,es.LastUpdBy=UserRowID
    WHERE es.OrganizationID=OrganizID
    AND es.EffectiveDateTo IS NOT NULL
    AND (es.EffectiveDateFrom >= @thisyearfirstdate AND IFNULL(es.EffectiveDateTo,SUBDATE(@thisyearfirstdate,INTERVAL 1 DAY)) >= @thisyearfirstdate);

    INSERT INTO employeesalary(RowID,EmployeeID,Created,CreatedBy,OrganizationID,FilingStatusID,PaySocialSecurityID,PayPhilhealthID,HDMFAmount,TrueSalary,BasicPay,Salary,UndeclaredSalary,BasicDailyPay,BasicHourlyPay,NoofDependents,MaritalStatus,PositionID,EffectiveDateFrom,EffectiveDateTo
    )
    SELECT
    es.RowID
    ,e.RowID
    ,CURRENT_TIMESTAMP()
    ,UserRowID
    ,OrganizID
    ,IFNULL(es.FilingStatusID,ees.FilingStatusID)
    ,es.PaySocialSecurityID
    ,es.PayPhilhealthID
    ,IFNULL(es.HDMFAmount,0)
    ,IFNULL(es.TrueSalary,ees.TrueSalary)
    ,IFNULL(es.BasicPay,ees.BasicPay)
    ,IFNULL(es.Salary,ees.Salary)
    ,IFNULL(es.UndeclaredSalary,ees.UndeclaredSalary)
    ,IFNULL(es.BasicDailyPay,ees.BasicDailyPay)
    ,IFNULL(es.BasicHourlyPay,ees.BasicHourlyPay)
    ,IFNULL(es.NoofDependents,ees.NoofDependents)
    ,IFNULL(es.MaritalStatus,ees.MaritalStatus)
    ,IFNULL(es.PositionID,ees.PositionID)
    ,IFNULL(es.EffectiveDateFrom,@thisyearfirstdate)
    ,es.EffectiveDateTo
    FROM employee e
    LEFT JOIN employeesalary es ON es.EffectiveDateFrom >= @thisyearfirstdate AND IFNULL(es.EffectiveDateTo,SUBDATE(@thisyearfirstdate,INTERVAL 1 DAY)) >= @thisyearfirstdate AND es.EmployeeID=e.RowID AND es.OrganizationID=e.OrganizationID AND es.EffectiveDateTo IS NULL

    INNER JOIN (SELECT *
                    FROM employeesalary
                    WHERE EmployeeID IN (SELECT RowID
                                                FROM employee e
                                                WHERE e.OrganizationID=OrganizID
                                                AND e.StartDate < @thisyearfirstdate
                                                AND TIMESTAMPDIFF(YEAR,e.Birthdate, CURDATE()) >= 59
                                                AND e.EmploymentStatus IN ('Regular','Probationary'))
                    AND OrganizationID=OrganizID
                    AND EffectiveDateTo IS NULL
                    GROUP BY EmployeeID) ees ON ees.EmployeeID=e.RowID

    WHERE e.OrganizationID=OrganizID AND e.StartDate < @thisyearfirstdate AND TIMESTAMPDIFF(YEAR,e.Birthdate, CURDATE()) >= 59 AND e.EmploymentStatus IN ('Regular','Probationary')
    ON
    DUPLICATE
    KEY
    UPDATE
        LastUpd=CURRENT_TIMESTAMP()
        ,LastUpdBy=UserRowID
        ,PaySocialSecurityID=NULL
        ,PayPhilhealthID=NULL
        ,HDMFAmount=0;



    UPDATE employeesalary es
    INNER JOIN employee e ON e.OrganizationID=OrganizID
                                AND e.StartDate < @thisyearfirstdate
                                AND TIMESTAMPDIFF(YEAR,e.Birthdate, CURDATE()) >= 59
                                AND e.EmploymentStatus IN ('Regular','Probationary')
                                AND es.EmployeeID=e.RowID
    SET es.EffectiveDateTo=IF(SUBDATE(@thisyearfirstdate, INTERVAL 1 DAY) >= es.EffectiveDateFrom, SUBDATE(@thisyearfirstdate, INTERVAL 1 DAY), es.EffectiveDateFrom)
    ,es.LastUpdBy=UserRowID
    WHERE es.OrganizationID=OrganizID
    AND es.EffectiveDateTo IS NULL
    AND es.EffectiveDateFrom < @thisyearfirstdate;

END IF;

























SELECT EXISTS(SELECT RowID
                    FROM employeesalary
                    WHERE EmployeeID IN (SELECT RowID
                                                FROM employee e
                                                WHERE e.OrganizationID=OrganizID
                                                AND e.StartDate > @thisyearfirstdate
                                                AND TIMESTAMPDIFF(YEAR,e.Birthdate, CURDATE()) >= 59
                                                AND e.EmploymentStatus IN ('Regular','Probationary'))
                    AND OrganizationID=OrganizID
                    AND EffectiveDateFrom=@thisyearfirstdate
                    AND DATE_FORMAT(LastUpd,'%Y-%m-%d')=CURDATE())
INTO hasalreadyseniorcitizensalary;

IF hasalreadyseniorcitizensalary = '0' AND userhasprivilege = '1' THEN
    SET hasalreadyseniorcitizensalary = '0';

    UPDATE employeesalary es
    INNER JOIN employee e ON e.OrganizationID=OrganizID
                                AND e.StartDate > @thisyearfirstdate
                                AND TIMESTAMPDIFF(YEAR,e.Birthdate, CURDATE()) >= 59
                                AND e.EmploymentStatus IN ('Regular','Probationary')
                                AND es.EmployeeID=e.RowID
    SET es.PaySocialSecurityID=NULL
    ,es.PayPhilhealthID=NULL
    ,es.HDMFAmount=0
    ,es.LastUpd=CURRENT_TIMESTAMP()
    ,es.LastUpdBy=UserRowID
    WHERE es.OrganizationID=OrganizID;

END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
