/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_employeeleave_indepen`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_employeeleave_indepen`(
    `elv_RowID` INT,
    `elv_OrganizationID` INT,
    `elv_LeaveStartTime` TIME,
    `elv_LeaveType` VARCHAR(50),
    `elv_CreatedBy` INT,
    `elv_LastUpdBy` INT,
    `elv_EmployeeID` INT,
    `elv_LeaveEndTime` TIME,
    `elv_LeaveStartDate` DATE,
    `elv_LeaveEndDate` DATE,
    `elv_Reason` VARCHAR(500),
    `elv_Comments` VARCHAR(2000),
    `elv_Image` LONGBLOB,
    `elv_Status` VARCHAR(50)
) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE empleaveID INT(11);
DECLARE specialty CONDITION FOR SQLSTATE '45000';
DECLARE emp_employment_stat TEXT;
DECLARE allowLeave BOOLEAN DEFAULT FALSE;
DECLARE has_leave_balance BOOL DEFAULT FALSE;

SELECT EmploymentStatus
FROM employee
WHERE RowID = elv_EmployeeID
INTO emp_employment_stat;

SET allowLeave = (emp_employment_stat = 'Regular') OR
    (elv_LeaveType = 'Leave w/o Pay');

SELECT EXISTS
(
SELECT i.RowID
FROM (
	SELECT e.RowID
	, e.LeaveBalance `BalanceLeave`
	FROM employee e
	WHERE e.RowID = elv_EmployeeID
	AND e.OrganizationID = elv_OrganizationID
	AND elv_LeaveType = 'Vacation leave'
UNION
	SELECT e.RowID
	, e.SickLeaveBalance `BalanceLeave`
	FROM employee e
	WHERE e.RowID = elv_EmployeeID
	AND e.OrganizationID = elv_OrganizationID
	AND elv_LeaveType = 'Sick leave'
UNION
	SELECT e.RowID
	, e.MaternityLeaveBalance `BalanceLeave`
	FROM employee e
	WHERE e.RowID = elv_EmployeeID
	AND e.OrganizationID = elv_OrganizationID
	# AND elv_LeaveType = 'Maternity/paternity leave'
	AND LOCATE('aternity', elv_LeaveType) > 0
UNION
	SELECT e.RowID
	, e.OtherLeaveBalance `BalanceLeave`
	FROM employee e
	WHERE e.RowID = elv_EmployeeID
	AND e.OrganizationID = elv_OrganizationID
	AND elv_LeaveType = 'Others'
     ) i
WHERE i.`BalanceLeave` > 0
)
INTO has_leave_balance;

IF allowLeave AND has_leave_balance THEN

    INSERT INTO employeeleave
    (
        RowID
        ,OrganizationID
        ,Created
        ,LeaveStartTime
        ,LeaveType
        ,CreatedBy
        ,LastUpdBy
        ,EmployeeID
        ,LeaveEndTime
        ,LeaveStartDate
        ,LeaveEndDate
        ,Reason
        ,Comments
        ,Image
        ,`Status`
    ) VALUES (
        elv_RowID
        ,elv_OrganizationID
        ,CURRENT_TIMESTAMP()
        ,elv_LeaveStartTime
        ,elv_LeaveType
        ,DEFAULT_internal_sys_user() # IFNULL(DEFAULT_internal_sys_user(), elv_CreatedBy)
        ,elv_LastUpdBy
        ,elv_EmployeeID
        ,elv_LeaveEndTime
        ,elv_LeaveStartDate
        ,elv_LeaveEndDate
        ,elv_Reason
        ,elv_Comments
        ,elv_Image
        ,IF(elv_Status = '', 'Pending', elv_Status)
    ) ON
    DUPLICATE
    KEY
    UPDATE
        LeaveStartTime=elv_LeaveStartTime
        ,LeaveType=elv_LeaveType
        ,LastUpd=CURRENT_TIMESTAMP()
        ,LastUpdBy=elv_LastUpdBy
        ,LeaveEndTime=elv_LeaveEndTime
        ,LeaveStartDate=elv_LeaveStartDate
        ,LeaveEndDate=elv_LeaveEndDate
        ,Reason=elv_Reason
        ,Comments=elv_Comments
        ,Image=elv_Image
        ,`Status`=IF(elv_Status = '', 'Pending', elv_Status);SELECT @@Identity AS id INTO empleaveID;

    INSERT INTO employeeleave_duplicate
    (
        RowID
        ,OrganizationID
        ,Created
        ,LeaveStartTime
        ,LeaveType
        ,CreatedBy
        ,LastUpdBy
        ,EmployeeID
        ,LeaveEndTime
        ,LeaveStartDate
        ,LeaveEndDate
        ,Reason
        ,Comments
        ,Image
        ,`Status`
    ) SELECT
        empleaveID
        ,elv_OrganizationID
        ,CURRENT_TIMESTAMP()
        ,elv_LeaveStartTime
        ,elv_LeaveType
        ,DEFAULT_internal_sys_user() # IFNULL(DEFAULT_internal_sys_user(), elv_CreatedBy)
        ,elv_LastUpdBy
        ,elv_EmployeeID
        ,elv_LeaveEndTime
        ,elv_LeaveStartDate
        ,elv_LeaveEndDate
        ,elv_Reason
        ,elv_Comments
        ,elv_Image
        ,IF(elv_Status = '', 'Pending', elv_Status)

        FROM (SELECT IFNULL(empleaveID,0) AS NewRowID) i WHERE i.NewRowID > 0
    ON DUPLICATE KEY UPDATE
        LeaveStartTime=elv_LeaveStartTime
        ,LeaveType=elv_LeaveType
        ,LastUpd=CURRENT_TIMESTAMP()
        ,LastUpdBy=elv_LastUpdBy
        ,LeaveEndTime=elv_LeaveEndTime
        ,LeaveStartDate=elv_LeaveStartDate
        ,LeaveEndDate=elv_LeaveEndDate
        ,Reason=elv_Reason
        ,Comments=elv_Comments
        ,Image=elv_Image
        ,`Status`=IF(elv_Status = '', 'Pending', elv_Status);

ELSEIF has_leave_balance = FALSE THEN

    SIGNAL specialty
    SET MESSAGE_TEXT = 'Insufficient leave balance';
    
    SET empleaveID = 0;
    
ELSE

    SIGNAL specialty
    SET MESSAGE_TEXT = 'LEAVE FILING APPLIES ONLY TO REGULAR EMPLOYEES';

    SET empleaveID = 0;

END IF;



RETURN empleaveID;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
