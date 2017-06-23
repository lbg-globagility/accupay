-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.11-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.0.0.4396
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for procedure goldwingspayrolldb.DEL_specificpaystub
DROP PROCEDURE IF EXISTS `DEL_specificpaystub`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `DEL_specificpaystub`(IN `paystub_RowID` INT)
    DETERMINISTIC
BEGIN

DECLARE vl_Amt DECIMAL(11,6);

DECLARE sl_Amt DECIMAL(11,6);

DECLARE ml_Amt DECIMAL(11,6);

DECLARE oth_Amt DECIMAL(11,6);

DECLARE emp_RowID INT(11);

DECLARE og_RowID INT(11);

DECLARE excesshrs DECIMAL(11,6);

DECLARE paydate_from DATE;

DECLARE paydate_to DATE;

DECLARE IsFirstHalfOfMonth CHAR(1);

DECLARE payperiod_rowid INT(11);


DECLARE isRollback BOOL DEFAULT 0;

DECLARE CONTINUE HANDLER FOR SQLEXCEPTION SET isRollback = 1;

START TRANSACTION;

SELECT
	ps.EmployeeID
	,ps.OrganizationID
	,ps.PayFromDate
	,ps.PayToDate
	,pp.`Half`
	,ps.PayPeriodID
FROM paystub ps
INNER JOIN payperiod pp ON pp.RowID=ps.PayPeriodID
WHERE ps.RowID=paystub_RowID
INTO emp_RowID
		,og_RowID
		,paydate_from
		,paydate_to
		,IsFirstHalfOfMonth
		,payperiod_rowid;

SELECT
psi.PayAmount
,psi1.PayAmount
,psi2.PayAmount
,psi3.PayAmount
FROM paystubitem psi
INNER JOIN product p 			ON p.PartNo='Vacation leave' 					AND psi.ProductID=p.RowID AND p.OrganizationID=psi.OrganizationID
INNER JOIN product p1 			ON p1.PartNo='Sick leave' 						AND p1.OrganizationID=psi.OrganizationID
INNER JOIN paystubitem psi1 	ON psi1.ProductID=p1.RowID AND psi1.PayStubID=psi.PayStubID
INNER JOIN product p2 			ON p2.PartNo='Maternity/paternity leave' 	AND p2.OrganizationID=psi.OrganizationID
INNER JOIN paystubitem psi2 	ON psi2.ProductID=p2.RowID AND psi2.PayStubID=psi.PayStubID
INNER JOIN product p3 			ON p3.PartNo='Others' 							AND p3.OrganizationID=psi.OrganizationID
INNER JOIN paystubitem psi3 	ON psi3.ProductID=p3.RowID AND psi3.PayStubID=psi.PayStubID
WHERE psi.PayStubID=paystub_RowID
INTO 	vl_Amt
		,sl_Amt
		,ml_Amt
		,oth_Amt;
	
	SELECT SUM(ete.OtherLeaveHours)
	FROM paystub ps
	INNER JOIN dates d ON d.DateValue BETWEEN ps.PayFromDate AND ps.PayToDate
	INNER JOIN employeetimeentry ete ON ete.EmployeeID=ps.EmployeeID AND ete.OrganizationID=ps.OrganizationID AND ete.`Date`=d.DateValue
	WHERE ps.RowID=paystub_RowID
	INTO excesshrs;
	
UPDATE employee e
LEFT JOIN (SELECT RowID
				,SUM(VacationLeaveHours) AS VacationLeaveHours
				,SUM(SickLeaveHours) AS SickLeaveHours
				,SUM(MaternityLeaveHours) AS MaternityLeaveHours
				,SUM(OtherLeaveHours) AS OtherLeaveHours
				FROM employeetimeentry
				WHERE EmployeeID=emp_RowID
				AND OrganizationID=og_RowID
				AND `Date` BETWEEN paydate_from AND paydate_to) ete ON ete.RowID IS NOT NULL
SET
e.LeaveBalance = e.LeaveBalance + IFNULL(ete.VacationLeaveHours,0)
,e.SickLeaveBalance = e.SickLeaveBalance + IFNULL(ete.SickLeaveHours,0)
,e.MaternityLeaveBalance = e.MaternityLeaveBalance + IFNULL(ete.MaternityLeaveHours,0)
,e.OtherLeaveBalance = e.OtherLeaveBalance + IFNULL(ete.OtherLeaveHours,0)
WHERE e.RowID=emp_RowID
AND ADDDATE(e.StartDate, INTERVAL 1 YEAR) NOT BETWEEN paydate_from AND paydate_to;

UPDATE employee e
SET e.LeaveBalance = 0
,e.SickLeaveBalance = 0
,e.MaternityLeaveBalance = 0
,e.OtherLeaveBalance = 0
WHERE e.RowID=emp_RowID
AND ADDDATE(e.StartDate, INTERVAL 1 YEAR) BETWEEN paydate_from AND paydate_to;



IF IsFirstHalfOfMonth = '1' THEN

	UPDATE employeeloanschedule els
	INNER JOIN payperiod pp ON pp.RowID=payperiod_rowid
	SET
	els.LoanPayPeriodLeft = els.LoanPayPeriodLeft + 1
	,els.TotalBalanceLeft = els.TotalBalanceLeft + els.DeductionAmount
	,els.`Status`='In Progress'
	WHERE els.OrganizationID=og_RowID AND els.BonusID IS NULL
	
	AND els.EmployeeID IS NULL
	
	AND els.DeductionSchedule IN ('First half','Per pay period')
	AND (els.DedEffectiveDateFrom >= pp.PayFromDate OR IFNULL(els.SubstituteEndDate,els.DedEffectiveDateTo) >= pp.PayFromDate)
	AND (els.DedEffectiveDateFrom <= pp.PayToDate OR IFNULL(els.SubstituteEndDate,els.DedEffectiveDateTo) <= pp.PayToDate);   															                
ELSE
                                                                                               
	UPDATE employeeloanschedule els
	INNER JOIN payperiod pp ON pp.RowID=payperiod_rowid
	SET
	els.LoanPayPeriodLeft = els.LoanPayPeriodLeft + 1
	,els.TotalBalanceLeft = els.TotalBalanceLeft + els.DeductionAmount
	,els.`Status`='In Progress'
	WHERE els.OrganizationID=og_RowID AND els.BonusID IS NULL
	
	AND els.EmployeeID IS NULL
	
	AND els.DeductionSchedule IN ('End of the month','Per pay period')
	AND (els.DedEffectiveDateFrom >= pp.PayFromDate OR IFNULL(els.SubstituteEndDate,els.DedEffectiveDateTo) >= pp.PayFromDate)
	AND (els.DedEffectiveDateFrom <= pp.PayToDate OR IFNULL(els.SubstituteEndDate,els.DedEffectiveDateTo) <= pp.PayToDate); 
END IF;

	UPDATE employeeloanschedule els
	INNER JOIN scheduledloansperpayperiod slp ON slp.EmployeeLoanRecordID=els.RowID AND slp.PayPeriodID=payperiod_rowid AND slp.OrganizationID=els.OrganizationID AND slp.EmployeeID=els.EmployeeID
	SET
	els.LoanPayPeriodLeft = els.LoanPayPeriodLeft + 1
	,els.TotalBalanceLeft = els.TotalBalanceLeft + els.DeductionAmount
	,els.`Status`='In Progress'
	WHERE els.OrganizationID=og_RowID AND els.BonusID IS NULL
	AND els.EmployeeID=emp_RowID;
	

UPDATE thirteenthmonthpay tmp
SET tmp.Amount=0
WHERE tmp.PaystubID=paystub_RowID
AND tmp.OrganizationID=og_RowID;

DELETE FROM employeeloanhistory WHERE PayStubID=paystub_RowID;
ALTER TABLE employeeloanhistory AUTO_INCREMENT = 0;

DELETE FROM paystubitem WHERE PayStubID=paystub_RowID;
ALTER TABLE paystubitem AUTO_INCREMENT = 0;

DELETE FROM thirteenthmonthpay WHERE PayStubID=paystub_RowID;
ALTER TABLE thirteenthmonthpay AUTO_INCREMENT = 0;

DELETE FROM paystubadjustment WHERE PayStubID=paystub_RowID;
ALTER TABLE paystubadjustment AUTO_INCREMENT = 0;

DELETE FROM paystub WHERE RowID=paystub_RowID;
ALTER TABLE paystub AUTO_INCREMENT = 0;

DELETE FROM paystubactual WHERE EmployeeID=emp_RowID AND OrganizationID=og_RowID AND PayPeriodID=payperiod_rowid;

DELETE FROM scheduledloansperpayperiod WHERE OrganizationID=og_RowID AND EmployeeID=emp_RowID AND PayPeriodID=payperiod_rowid; ALTER TABLE scheduledloansperpayperiod AUTO_INCREMENT = 0;

IF isRollback THEN
	ROLLBACK;
	
ELSE
	COMMIT;
	
END IF;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
