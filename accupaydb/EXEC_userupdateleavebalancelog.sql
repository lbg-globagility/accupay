/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `EXEC_userupdateleavebalancelog`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `EXEC_userupdateleavebalancelog`(IN `OrganizID` INT, IN `UserRowID` INT)
    DETERMINISTIC
BEGIN

DECLARE hasupdate TINYINT;

DECLARE minimum_date DATE;

DECLARE maximum_date DATE;

DECLARE custom_maximum_date DATE;

DECLARE curr_year YEAR;

SET curr_year = YEAR(CURDATE());

CALL PreserveLastYearsLeave(OrganizID, UserRowID, curr_year - 1);

SELECT EXISTS(SELECT RowID FROM userupdateleavebalancelog uu WHERE uu.OrganizationID=OrganizID AND uu.YearValue=curr_year) INTO hasupdate;

IF hasupdate = 0 THEN

    SELECT MIN(ps.PayFromDate) FROM paystub ps INNER JOIN payperiod pp ON pp.RowID=ps.PayPeriodID AND pp.`Year`=curr_year AND pp.OrganizationID=ps.OrganizationID WHERE ps.OrganizationID=OrganizID INTO minimum_date;

    SELECT MAX(ps.PayToDate) FROM paystub ps INNER JOIN payperiod pp ON pp.RowID=ps.PayPeriodID AND pp.`Year`=curr_year AND pp.OrganizationID=ps.OrganizationID WHERE ps.OrganizationID=OrganizID INTO maximum_date;

    SET minimum_date = IFNULL(minimum_date, MAKEDATE(curr_year, 1));

    SET custom_maximum_date = IFNULL(maximum_date, ADDDATE(SUBDATE(minimum_date, INTERVAL 1 DAY), INTERVAL 1 YEAR));

    UPDATE employee e
    LEFT JOIN (SELECT et.RowID,et.EmployeeID
                    ,SUM(et.VacationLeaveHours) AS VacationLeaveHours
                    ,SUM(et.SickLeaveHours) AS SickLeaveHours
                    ,SUM(et.MaternityLeaveHours) AS MaternityLeaveHours
                    ,SUM(et.OtherLeaveHours) AS OtherLeaveHours
                    FROM employeetimeentry et
                    WHERE et.OrganizationID=OrganizID
                    AND (et.VacationLeaveHours + et.SickLeaveHours + et.MaternityLeaveHours + et.OtherLeaveHours) > 0
                    AND et.`Date` BETWEEN minimum_date AND custom_maximum_date
                    GROUP BY et.EmployeeID) ete ON ete.RowID IS NOT NULL AND ete.EmployeeID=e.RowID
    SET
    e.LeaveBalance=e.LeaveAllowance - IFNULL(ete.VacationLeaveHours,0)
    ,e.SickLeaveBalance=e.SickLeaveAllowance - IFNULL(ete.SickLeaveHours,0)
    ,e.MaternityLeaveBalance=e.MaternityLeaveAllowance - IFNULL(ete.MaternityLeaveHours,0)
    ,e.OtherLeaveBalance=e.OtherLeaveAllowance - IFNULL(ete.OtherLeaveHours,0)
    ,e.LastUpd=CURRENT_TIMESTAMP()
    ,e.LastUpdBy=UserRowID
    WHERE e.OrganizationID=OrganizID
    AND (ADDDATE(e.StartDate, INTERVAL 2 YEAR) <= curr_year
            OR ADDDATE(e.StartDate, INTERVAL 1 YEAR) BETWEEN minimum_date AND custom_maximum_date);

    CALL UPD_leavebalance_newlyjoinedemployee(OrganizID, CURDATE(), NULL);
    
    INSERT INTO userupdateleavebalancelog(OrganizationID,Created,UserID,YearValue) VALUES (OrganizID,CURRENT_TIMESTAMP(),UserRowID,curr_year) ON DUPLICATE KEY UPDATE LastUpd=CURRENT_TIMESTAMP();
    
END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
