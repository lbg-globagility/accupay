/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `SP_LeaveTransaction_Recalculate_2019`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_LeaveTransaction_Recalculate_2019`(
	IN `p_EmployeeID` INT


)
LANGUAGE SQL
NOT DETERMINISTIC
CONTAINS SQL
SQL SECURITY DEFINER
COMMENT ''
BEGIN

DECLARE v_UserID INT(11) DEFAULT NULL;
DECLARE v_StartDate DATE DEFAULT '2018-12-01';
DECLARE v_OrganizationID INT(11) DEFAULT NULL;
DECLARE v_StartDatePayPeriodID INT(11) DEFAULT NULL;
DECLARE v_VacationLeaveLedgerID INT(11) DEFAULT NULL;
DECLARE v_SickLeaveLedgerID INT(11) DEFAULT NULL;
DECLARE v_InitialVacationLeave DECIMAL(11, 2) DEFAULT NULL;
DECLARE v_InitialSickLeave DECIMAL(11, 2) DEFAULT NULL;

SET v_UserID = (SELECT RowID FROM user LIMIT 1);

SELECT LeaveAllowance, SickLeaveAllowance, OrganizationID FROM employee
WHERE RowID = p_EmployeeID
INTO v_InitialVacationLeave, v_InitialSickLeave, v_OrganizationID
;


IF v_InitialVacationLeave IS NULL OR v_InitialSickLeave IS NULL OR v_OrganizationID IS NULL THEN
	SIGNAL SQLSTATE '45000'
	SET MESSAGE_TEXT = 'Cannot retrieve basic employee data.';
	CALL non_existent_procedure_to_force_exception();
END IF
;

SET v_StartDatePayPeriodID = (SELECT RowID FROM payperiod
				WHERE v_StartDate 
					BETWEEN payperiod.PayFromDate
					AND payperiod.PayToDate
				AND payperiod.TotalGrossSalary = 1
				AND payperiod.OrganizationID = v_OrganizationID );

IF v_StartDatePayPeriodID IS NULL THEN
	SIGNAL SQLSTATE '45000'
	SET MESSAGE_TEXT = 'Cannot get the pay period for the initial leave transaction';
	CALL non_existent_procedure_to_force_exception();
END IF
;


SET v_VacationLeaveLedgerID = (SELECT leaveledger.RowID FROM leaveledger
										INNER JOIN product
										ON leaveledger.ProductID = product.RowID
										AND product.PartNo = 'Vacation leave'
										WHERE leaveledger.EmployeeID = p_EmployeeID);

SET v_SickLeaveLedgerID = 		(SELECT leaveledger.RowID FROM leaveledger
										INNER JOIN product
										ON leaveledger.ProductID = product.RowID
										AND product.PartNo = 'Sick leave'
										WHERE leaveledger.EmployeeID = p_EmployeeID);

IF v_VacationLeaveLedgerID IS NULL OR v_SickLeaveLedgerID IS NULL THEN
	SIGNAL SQLSTATE '45000'
	SET MESSAGE_TEXT = 'Cannot get the leave ledger Ids';
	CALL non_existent_procedure_to_force_exception();
END IF
;

########################
# Vacation Leave Reset
########################

# employeetimeentry count should be equal to employeeleave count
# if it does not, it means that the corresponding leave for the time entry is either deleted or changed its type or date
IF (SELECT COUNT(*) FROM employeetimeentry
	WHERE EmployeeID = p_EmployeeID
	AND VacationLeaveHours > 0
	AND DATE >= v_StartDate
	ORDER BY EmployeeID, DATE)
	<>
	(SELECT COUNT(*) FROM employeeleave
	WHERE LeaveStartDate >= v_StartDate
	AND LeaveType = 'Vacation leave'
	AND EmployeeID = p_EmployeeID)
THEN
	SIGNAL SQLSTATE '45000'
	SET MESSAGE_TEXT = 'Employee Time Entry count is not the same with the Employee Leave count.';
	CALL non_existent_procedure_to_force_exception();
END IF
;

SET @vacationLeaveBalance = v_InitialVacationLeave;

DROP TEMPORARY TABLE IF EXISTS NEW_LEAVE_TRANSACTIONS_VACATION_UNORDERED;
CREATE TEMPORARY TABLE IF NOT EXISTS NEW_LEAVE_TRANSACTIONS_VACATION_UNORDERED
SELECT
	employeetimeentry.OrganizationID,
	employeetimeentry.EmployeeID,
	employeeleave.RowID AS 'ReferenceID',
	v_VacationLeaveLedgerID AS 'LeaveLedgerID',
	(SELECT payperiod.RowID FROM payperiod
	WHERE employeetimeentry.DATE 
		BETWEEN payperiod.PayFromDate
		AND payperiod.PayToDate
	AND payperiod.TotalGrossSalary = 1
	AND payperiod.OrganizationID = employeetimeentry.OrganizationID) AS 'PayperiodID',
	employeetimeentry.DATE AS 'TransactionDate',
	'Debit' AS 'Type',
	employeetimeentry.VacationLeaveHours AS 'Amount',
	employeeleave.LeaveStartDate,
	employeeleave.LeaveStartTime,
	employeeleave.LeaveEndTime
FROM employeetimeentry
LEFT JOIN employeeleave
ON employeeleave.EmployeeID = employeetimeentry.EmployeeID
AND employeeleave.LeaveStartDate = employeetimeentry.DATE
AND employeeleave.LeaveType = 'Vacation leave'
WHERE employeetimeentry.EmployeeID = p_EmployeeID
AND employeetimeentry.VacationLeaveHours > 0
AND employeetimeentry.DATE >= v_StartDate
ORDER BY employeetimeentry.DATE
;

DROP TEMPORARY TABLE IF EXISTS NEW_LEAVE_TRANSACTIONS_VACATION;
CREATE TEMPORARY TABLE IF NOT EXISTS NEW_LEAVE_TRANSACTIONS_VACATION
SELECT
	OrganizationID,
	EmployeeID,
	ReferenceID,
	LeaveLedgerID,
	PayperiodID,
	TransactionDate,
	`Type`,
	@vacationLeaveBalance := @vacationLeaveBalance - Amount AS 'Balance',
	Amount,
	LeaveStartDate,
	LeaveStartTime,
	LeaveEndTime
FROM NEW_LEAVE_TRANSACTIONS_VACATION_UNORDERED
ORDER BY TransactionDate
;

# There should be no null columns
# If there is, it should not be saved since it does not have the proper data
IF EXISTS(SELECT 1 FROM NEW_LEAVE_TRANSACTIONS_VACATION
				WHERE ReferenceID IS NULL
				OR PayperiodID IS NULL
				OR Balance IS NULL)
THEN
	SIGNAL SQLSTATE '45000'
	SET MESSAGE_TEXT = 'One row has NULL ReferenceID or PayperiodID or Balance.';
	CALL non_existent_procedure_to_force_exception();
END IF
;

# Remove current leavetransactions
DELETE FROM leavetransaction
WHERE TransactionDate >= v_StartDate
AND EmployeeID = p_EmployeeID
AND LeaveLedgerID = v_VacationLeaveLedgerID
;

# Insert beginning leave transaction
INSERT INTO leavetransaction
(
	OrganizationID,
	CreatedBy,
	EmployeeID,
	ReferenceID,
	LeaveLedgerID,
	PayPeriodID,
	TransactionDate,
	`Type`,
	Balance,
	Amount
)
VALUES
(
	v_OrganizationID,
	v_UserID,
	p_EmployeeID,
	NULL,
	v_VacationLeaveLedgerID,
	v_StartDatePayPeriodID,
	v_StartDate,
	'Credit',
	v_InitialVacationLeave,
	v_InitialVacationLeave
)
;

UPDATE leaveledger
SET LastTransactionID = LAST_INSERT_ID()
WHERE RowID = v_VacationLeaveLedgerID;


IF EXISTS(SELECT 1 FROM NEW_LEAVE_TRANSACTIONS_VACATION) THEN
	# Insert all leavetransactions
	INSERT INTO leavetransaction
	(
		OrganizationID,
		CreatedBy,
		EmployeeID,
		ReferenceID,
		LeaveLedgerID,
		PayPeriodID,
		TransactionDate,
		`Type`,
		Balance,
		Amount
	)
	SELECT
		OrganizationID,
		v_UserID,
		EmployeeID,
		ReferenceID,
		LeaveLedgerID,
		PayperiodID,
		TransactionDate,
		`Type`,
		Balance,
		Amount
	FROM
		NEW_LEAVE_TRANSACTIONS_VACATION
	;
	
	UPDATE leaveledger
	SET LastTransactionID = (SELECT MAX(RowID) FROM leavetransaction)
	WHERE RowID = v_VacationLeaveLedgerID;

END IF;


########################
# Sick Leave Reset
########################

# employeetimeentry count should be equal to employeeleave count
# if it does not, it means that the corresponding leave for the time entry is either deleted or changed its type or date
IF (SELECT COUNT(*) FROM employeetimeentry
	WHERE EmployeeID = p_EmployeeID
	AND SickLeaveHours > 0
	AND DATE >= v_StartDate
	ORDER BY EmployeeID, DATE)
	<>
	(SELECT COUNT(*) FROM employeeleave
	WHERE LeaveStartDate >= v_StartDate
	AND LeaveType = 'Sick leave'
	AND EmployeeID = p_EmployeeID)
THEN
	SIGNAL SQLSTATE '45000'
	SET MESSAGE_TEXT = 'Employee Time Entry count is not the same with the Employee Leave count.';
	CALL non_existent_procedure_to_force_exception();
END IF
;

SET @sickLeaveBalance = v_InitialSickLeave;

DROP TEMPORARY TABLE IF EXISTS NEW_LEAVE_TRANSACTIONS_SICK_UNORDERED;
CREATE TEMPORARY TABLE IF NOT EXISTS NEW_LEAVE_TRANSACTIONS_SICK_UNORDERED
SELECT
	employeetimeentry.OrganizationID,
	employeetimeentry.EmployeeID,
	employeeleave.RowID AS 'ReferenceID',
	v_SickLeaveLedgerID AS 'LeaveLedgerID',
	(SELECT payperiod.RowID FROM payperiod
	WHERE employeetimeentry.DATE 
		BETWEEN payperiod.PayFromDate
		AND payperiod.PayToDate
	AND payperiod.TotalGrossSalary = 1
	AND payperiod.OrganizationID = employeetimeentry.OrganizationID) AS 'PayperiodID',
	employeetimeentry.DATE AS 'TransactionDate',
	'Debit' AS 'Type',
	employeetimeentry.SickLeaveHours AS 'Amount',
	employeeleave.LeaveStartDate,
	employeeleave.LeaveStartTime,
	employeeleave.LeaveEndTime
FROM employeetimeentry
LEFT JOIN employeeleave
ON employeeleave.EmployeeID = employeetimeentry.EmployeeID
AND employeeleave.LeaveStartDate = employeetimeentry.DATE
AND employeeleave.LeaveType = 'Sick leave'
WHERE employeetimeentry.EmployeeID = p_EmployeeID
AND employeetimeentry.SickLeaveHours > 0
AND employeetimeentry.DATE >= v_StartDate
ORDER BY employeetimeentry.DATE
;

DROP TEMPORARY TABLE IF EXISTS NEW_LEAVE_TRANSACTIONS_SICK;
CREATE TEMPORARY TABLE IF NOT EXISTS NEW_LEAVE_TRANSACTIONS_SICK
SELECT
	OrganizationID,
	EmployeeID,
	ReferenceID,
	LeaveLedgerID,
	PayperiodID,
	TransactionDate,
	`Type`,
	@sickLeaveBalance := @sickLeaveBalance - Amount AS 'Balance',
	Amount,
	LeaveStartDate,
	LeaveStartTime,
	LeaveEndTime
FROM NEW_LEAVE_TRANSACTIONS_SICK_UNORDERED
ORDER BY TransactionDate
;

# There should be no null columns
# If there is, it should not be saved since it does not have the proper data
IF EXISTS(SELECT 1 FROM NEW_LEAVE_TRANSACTIONS_SICK
				WHERE ReferenceID IS NULL
				OR PayperiodID IS NULL
				OR Balance IS NULL)
THEN
	SIGNAL SQLSTATE '45000'
	SET MESSAGE_TEXT = 'One row has NULL ReferenceID or PayperiodID or Balance.';
	CALL non_existent_procedure_to_force_exception();
END IF
;

# Remove current leavetransactions
DELETE FROM leavetransaction
WHERE TransactionDate >= v_StartDate
AND EmployeeID = p_EmployeeID
AND LeaveLedgerID = v_SickLeaveLedgerID
;

# Insert beginning leave transaction
INSERT INTO leavetransaction
(
	OrganizationID,
	CreatedBy,
	EmployeeID,
	ReferenceID,
	LeaveLedgerID,
	PayPeriodID,
	TransactionDate,
	`Type`,
	Balance,
	Amount
)
VALUES
(
	v_OrganizationID,
	v_UserID,
	p_EmployeeID,
	NULL,
	v_SickLeaveLedgerID,
	v_StartDatePayPeriodID,
	v_StartDate,
	'Credit',
	v_InitialSickLeave,
	v_InitialSickLeave
)
;

UPDATE leaveledger
SET LastTransactionID = LAST_INSERT_ID()
WHERE RowID = v_SickLeaveLedgerID;

IF EXISTS(SELECT 1 FROM NEW_LEAVE_TRANSACTIONS_SICK) THEN
	
	# Insert all leavetransactions
	INSERT INTO leavetransaction
	(
		OrganizationID,
		CreatedBy,
		EmployeeID,
		ReferenceID,
		LeaveLedgerID,
		PayPeriodID,
		TransactionDate,
		`Type`,
		Balance,
		Amount
	)
	SELECT
		OrganizationID,
		v_UserID,
		EmployeeID,
		ReferenceID,
		LeaveLedgerID,
		PayperiodID,
		TransactionDate,
		`Type`,
		Balance,
		Amount
	FROM
		NEW_LEAVE_TRANSACTIONS_SICK
	;
	
	UPDATE leaveledger
	SET LastTransactionID = (SELECT MAX(RowID) FROM leavetransaction)
	WHERE RowID = v_SickLeaveLedgerID;

END IF
;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;