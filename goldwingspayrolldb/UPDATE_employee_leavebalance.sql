/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `UPDATE_employee_leavebalance`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `UPDATE_employee_leavebalance`(IN `OrganizID` INT, IN `EmpID` INT, IN `PayPeriodRowID` INT, IN `UserRowID` INT)
    DETERMINISTIC
BEGIN

DECLARE minimum_date DATE;

DECLARE maximum_date DATE;

DECLARE curr_year INT(11);

    SELECT pp.`Year` FROM payperiod pp WHERE pp.RowID=PayPeriodRowID INTO curr_year;

    SELECT MIN(ps.PayFromDate) FROM paystub ps INNER JOIN payperiod pp ON pp.RowID=ps.PayPeriodID AND pp.`Year`=curr_year AND pp.OrganizationID=ps.OrganizationID WHERE ps.OrganizationID=OrganizID AND ps.EmployeeID=EmpID INTO minimum_date;

    SELECT MAX(pp.PayToDate) FROM paystub ps INNER JOIN payperiod pp ON pp.RowID=ps.PayPeriodID AND pp.`Year`=curr_year AND pp.OrganizationID=ps.OrganizationID INNER JOIN payperiod ppp ON ppp.RowID=PayPeriodRowID WHERE ps.OrganizationID=OrganizID AND ps.EmployeeID=EmpID INTO maximum_date;

    SET minimum_date = IFNULL(minimum_date, MAKEDATE(curr_year, 1));

    IF maximum_date IS NULL THEN SET maximum_date = ADDDATE(SUBDATE(minimum_date, INTERVAL 1 DAY), INTERVAL 1 YEAR); END IF;

INSERT INTO paystubitem(OrganizationID,Created,CreatedBy,PayStubID,ProductID,PayAmount,Undeclared)
    SELECT p.OrganizationID
    ,CURRENT_TIMESTAMP()
    ,UserRowID
    ,ps.RowID
    ,p.RowID
    ,IF(p.PartNo='Vacation leave', IFNULL(e.LeaveAllowance,0) - IFNULL(ete.VacationLeaveHours,0)
        , IF(p.PartNo='Sick leave', IFNULL(e.SickLeaveAllowance,0) - IFNULL(ete.SickLeaveHours,0)
            , IF(p.PartNo='Maternity/paternity leave', IFNULL(e.MaternityLeaveAllowance,0) - IFNULL(ete.MaternityLeaveHours,0)
                , IFNULL(e.OtherLeaveAllowance,0) - IFNULL(ete.OtherLeaveHours,0))))
    ,'0'
    FROM product p
    INNER JOIN paystub ps ON ps.EmployeeID=EmpID AND ps.OrganizationID=p.OrganizationID AND ps.PayPeriodID=PayPeriodRowID AND ps.RowID IS NOT NULL
    INNER JOIN category c ON c.CategoryName='Leave Type' AND c.OrganizationID=p.OrganizationID AND c.RowID=p.CategoryID

    LEFT JOIN employee e ON e.RowID=EmpID AND e.OrganizationID=OrganizID AND (ADDDATE(e.StartDate, INTERVAL 2 YEAR) <= ps.PayToDate OR ADDDATE(e.StartDate, INTERVAL 1 YEAR) BETWEEN minimum_date AND maximum_date)

    LEFT JOIN (SELECT et.RowID,et.EmployeeID
                    ,SUM(et.VacationLeaveHours) AS VacationLeaveHours
                    ,SUM(et.SickLeaveHours) AS SickLeaveHours
                    ,SUM(et.MaternityLeaveHours) AS MaternityLeaveHours
                    ,SUM(et.OtherLeaveHours) AS OtherLeaveHours
                    FROM employeetimeentry et
                    WHERE et.OrganizationID=OrganizID
                    AND (et.VacationLeaveHours + et.SickLeaveHours + et.MaternityLeaveHours + et.OtherLeaveHours) > 0
                    AND et.`Date` BETWEEN minimum_date AND maximum_date
                    GROUP BY et.EmployeeID) ete ON ete.RowID IS NOT NULL AND ete.EmployeeID=e.RowID

    WHERE p.OrganizationID=OrganizID
ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=UserRowID
    ,PayAmount=IF(p.PartNo='Vacation leave', IFNULL(e.LeaveAllowance,0) - IFNULL(ete.VacationLeaveHours,0)
                    , IF(p.PartNo='Sick leave', IFNULL(e.SickLeaveAllowance,0) - IFNULL(ete.SickLeaveHours,0)
                        , IF(p.PartNo='Maternity/paternity leave', IFNULL(e.MaternityLeaveAllowance,0) - IFNULL(ete.MaternityLeaveHours,0)
                            , IFNULL(e.OtherLeaveAllowance,0) - IFNULL(ete.OtherLeaveHours,0))));

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
