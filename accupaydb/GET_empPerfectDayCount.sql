/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `GET_empPerfectDayCount`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GET_empPerfectDayCount`(`OrganizID` INT, `EmpRowID` INT, `FirstDate` DATE, `LastDate` DATE) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11) DEFAULT 0;

DECLARE emp_HireDate DATE;

DECLARE orgMinWorkDays INT(11);

SELECT GET_empworkdaysperyear(EmpRowID) INTO orgMinWorkDays;



SELECT e.StartDate FROM employee e WHERE e.RowID=EmpRowID INTO emp_HireDate;


IF emp_HireDate < FirstDate THEN
    SET emp_HireDate = FirstDate;
END IF;


IF orgMinWorkDays BETWEEN 310 AND 320 THEN

    SELECT COUNT(*)
    FROM (
        SELECT pr.RowID
        FROM payrate pr
        LEFT JOIN employeetimeentry ete ON ete.Date=pr.Date AND ete.EmployeeID=EmpRowID AND ete.OrganizationID=OrganizID
        WHERE pr.OrganizationID=OrganizID
        AND pr.Date BETWEEN emp_HireDate AND LastDate
        AND DAYOFWEEK(pr.Date) != 1
        AND pr.PayType!='Regular Holiday'
    UNION
        SELECT pr.RowID
        FROM payrate pr
        LEFT JOIN employeetimeentry ete ON ete.Date=pr.Date AND ete.EmployeeID=EmpRowID AND ete.OrganizationID=OrganizID
        WHERE pr.OrganizationID=OrganizID
        AND pr.Date BETWEEN emp_HireDate AND LastDate
        AND DAYOFWEEK(pr.Date) != 1
        AND pr.PayType='Regular Holiday'
    ) dd
    INTO returnvalue;

ELSEIF orgMinWorkDays BETWEEN 255 AND 265 THEN

    SELECT COUNT(*)
    FROM (
        SELECT pr.RowID
        FROM payrate pr
        LEFT JOIN employeetimeentry ete ON ete.Date=pr.Date AND ete.EmployeeID=EmpRowID AND ete.OrganizationID=OrganizID
        WHERE pr.OrganizationID=OrganizID
        AND pr.Date BETWEEN emp_HireDate AND LastDate
        AND DAYOFWEEK(pr.Date) NOT IN(1,7)
        AND pr.PayType!='Regular Holiday'
    UNION
        SELECT pr.RowID
        FROM payrate pr
        LEFT JOIN employeetimeentry ete ON ete.Date=pr.Date AND ete.EmployeeID=EmpRowID AND ete.OrganizationID=OrganizID
        WHERE pr.OrganizationID=OrganizID
        AND pr.Date BETWEEN emp_HireDate AND LastDate
        AND DAYOFWEEK(pr.Date) NOT IN(1,7)
        AND pr.PayType='Regular Holiday'
    ) dd
    INTO returnvalue;

END IF;


RETURN returnvalue;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
