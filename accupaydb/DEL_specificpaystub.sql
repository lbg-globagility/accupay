/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `DEL_specificpaystub`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `DEL_specificpaystub`(IN `paystub_RowID` INT)
    DETERMINISTIC
BEGIN

DECLARE emp_RowID INT(11);
DECLARE og_RowID INT(11);
DECLARE payperiod_rowid INT(11);

DECLARE paydate_from DATE;
DECLARE paydate_to DATE;

DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

START TRANSACTION;

SELECT
    ps.EmployeeID,
    ps.OrganizationID,
    ps.PayFromDate,
    ps.PayToDate,
    ps.PayPeriodID
FROM paystub ps
INNER JOIN payperiod pp ON pp.RowID=ps.PayPeriodID
WHERE ps.RowID=paystub_RowID
INTO emp_RowID,
    og_RowID,
    paydate_from,
    paydate_to,
    payperiod_rowid;

UPDATE employee e
LEFT JOIN (
    SELECT
        RowID,
        SUM(VacationLeaveHours) AS VacationLeaveHours,
        SUM(SickLeaveHours) AS SickLeaveHours,
        SUM(MaternityLeaveHours) AS MaternityLeaveHours,
        SUM(OtherLeaveHours) AS OtherLeaveHours
    FROM employeetimeentry
    WHERE EmployeeID = emp_RowID AND
        OrganizationID = og_RowID AND
        `Date` BETWEEN paydate_from AND paydate_to
) ete
ON ete.RowID IS NOT NULL
SET e.LeaveBalance = e.LeaveBalance + IFNULL(ete.VacationLeaveHours,0),
    e.SickLeaveBalance = e.SickLeaveBalance + IFNULL(ete.SickLeaveHours,0),
    e.MaternityLeaveBalance = e.MaternityLeaveBalance + IFNULL(ete.MaternityLeaveHours,0),
    e.OtherLeaveBalance = e.OtherLeaveBalance + IFNULL(ete.OtherLeaveHours,0)
WHERE e.RowID = emp_RowID AND
    ADDDATE(e.StartDate, INTERVAL 1 YEAR) NOT BETWEEN paydate_from AND paydate_to;

UPDATE employee e
SET e.LeaveBalance = 0,
    e.SickLeaveBalance = 0,
    e.MaternityLeaveBalance = 0,
    e.OtherLeaveBalance = 0
WHERE e.RowID=emp_RowID
AND ADDDATE(e.StartDate, INTERVAL 1 YEAR) BETWEEN paydate_from AND paydate_to;


UPDATE employeeloanschedule els
INNER JOIN scheduledloansperpayperiod slp
ON slp.EmployeeLoanRecordID = els.RowID AND
    slp.PayPeriodID = payperiod_rowid AND
    slp.OrganizationID = els.OrganizationID AND
    slp.EmployeeID = els.EmployeeID
SET els.TotalBalanceLeft = els.TotalBalanceLeft + slp.DeductionAmount,
    els.LastUpdBy = IFNULL(els.LastUpdBy, els.CreatedBy)
WHERE els.OrganizationID = og_RowID AND
    els.EmployeeID = emp_RowID;

UPDATE thirteenthmonthpay tmp
SET tmp.Amount=0
WHERE tmp.PaystubID=paystub_RowID
AND tmp.OrganizationID=og_RowID;

DELETE FROM employeeloanhistory
WHERE PayStubID = paystub_RowID;

DELETE FROM paystubitem
WHERE PayStubID = paystub_RowID;

DELETE FROM thirteenthmonthpay
WHERE PayStubID=paystub_RowID;

DELETE FROM paystubadjustment
WHERE PayStubID=paystub_RowID;

DELETE FROM paystubadjustmentactual
WHERE PayStubID=paystub_RowID;

DELETE FROM paystubactual
WHERE EmployeeID=emp_RowID AND OrganizationID=og_RowID AND PayPeriodID=payperiod_rowid;

DELETE FROM paystub
WHERE RowID=paystub_RowID;

DELETE FROM scheduledloansperpayperiod
WHERE OrganizationID = og_RowID AND
    EmployeeID = emp_RowID AND
    PayPeriodID=payperiod_rowid;

COMMIT;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
