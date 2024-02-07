/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `SEARCH_employeeprofile`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `SEARCH_employeeprofile`(
	IN `og_id` INT,
	IN `emp_id` VARCHAR(50),
	IN `emp_fname` VARCHAR(50),
	IN `emp_lname` VARCHAR(50),
	IN `page_number` INT
)
LANGUAGE SQL
DETERMINISTIC
CONTAINS SQL
SQL SECURITY DEFINER
COMMENT ''
BEGIN

DECLARE max_count_per_page INT(11) DEFAULT 50;
DECLARE leaveTypeCategoryId INT(11);
DECLARE isLaglobal BOOLEAN DEFAULT FALSE;

SET isLaglobal=EXISTS(SELECT * FROM systemowner so WHERE so.`Name`='LA Global' AND so.IsCurrentOwner='1' LIMTI 1);

IF isLaglobal=TRUE THEN

    SET leaveTypeCategoryId = (SELECT c.RowID FROM category c WHERE c.OrganizationID=og_id AND c.CategoryName='Leave type' LIMIT 1);

    SELECT e.RowID
            ,e.EmployeeID                                   `Employee ID`
            ,e.LastName                                     `Last Name`
            ,e.FirstName                                    `First Name`
            ,e.MiddleName                                   `Middle Name`
            ,e.Surname
            ,e.Nickname
            ,e.MaritalStatus                                `Marital Status`
            ,IFNULL(e.NoOfDependents,0)             `No. Of Dependents`
            ,DATE_FORMAT(e.Birthdate,'%m-%d-%Y')    `Birthdate`
            ,DATE_FORMAT(e.Startdate,'%m-%d-%Y')    `Startdate`
            ,IFNULL(d.Name,'')                          `Job Title`
            ,IFNULL(pos.PositionName,'')                `Position`
            ,e.Salutation
            ,e.TINNo                                            `TIN`
            ,e.SSSNo                                            `SSS No.`
            ,e.HDMFNo                                       `PAGIBIG No.`
            ,e.PhilHealthNo                             `PhilHealth No.`
            ,e.WorkPhone                                    `Work Phone No.`
            ,e.HomePhone                                    `Home Phone No.`
            ,e.MobilePhone                                  `Mobile Phone No.`
            ,e.HomeAddress                                  `Home address`
            ,e.EmailAddress                             `Email address`
            ,IF(e.Gender='M','Male','Female')       `Gender`
            ,e.EmploymentStatus                         `Employment Status`
            ,IFNULL(pf.PayFrequencyType,'')         `Pay Frequency`
            ,e.UndertimeOverride
            ,e.OvertimeOverride
            ,IFNULL(DATE_FORMAT(e.Created
                                    , '%m-%d-%Y'), '')  `Creation Date`
            ,CONCAT_WS(', '
                    , u.FirstName, u.LastName)          `Created by`
            ,IFNULL(DATE_FORMAT(e.Created
                                    , '%m-%d-%Y'), '')  `Last Update`
            ,CONCAT_WS(', '
                    , uu.FirstName, uu.LastName)        `LastUpdate by`
            ,IFNULL(pos.RowID,'')                       `PositionID`
            ,IFNULL(e.PayFrequencyID,'')                `PayFrequencyID`
            ,e.EmployeeType
            ,IFNULL(vacationLeaveTransaction.Balance, 0) `LeaveBalance`
            ,IFNULL(sickLeaveTransaction.Balance, 0) `SickLeaveBalance`
            ,e.MaternityLeaveBalance
            ,e.LeaveAllowance
            ,e.SickLeaveAllowance
            ,e.MaternityLeaveAllowance
            ,e.LeavePerPayPeriod
            ,e.SickLeavePerPayPeriod
            ,e.MaternityLeavePerPayPeriod
            ,IFNULL(fstat.RowID,3)                          `fstatRowID`
            ,e.AlphaListExempted
            ,e.WorkDaysPerYear
            ,CHAR_TO_DAYOFWEEK(e.DayOfRest)         `DayOfRest`
            ,e.ATMNo
            ,e.BankName
            ,e.OtherLeavePerPayPeriod
            ,e.OtherLeaveAllowance
            ,e.OtherLeaveBalance
            ,e.CalcHoliday
            ,e.CalcSpecialHoliday
            ,e.CalcNightDiff
            ,e.CalcNightDiffOT
            ,e.CalcRestDay
            ,e.CalcRestDayOT
            ,IFNULL(e.LateGracePeriod,0)                `LateGracePeriod`
            ,e.GracePeriodAsBuffer
            ,IFNULL(e.RevealInPayroll,1)                `RevealInPayroll`
            ,IFNULL(ag.AgencyName,'')                   `AgencyName`
            ,IFNULL(ag.RowID,'')                            `ag_RowID`
            ,e.BranchID
            ,e.BPIInsurance

            ,e.DateEvaluated
            ,e.DateRegularized
            
    FROM (SELECT * FROM employee WHERE OrganizationID=og_id AND EmployeeID  =emp_id     AND LENGTH(emp_id) > 0
        UNION
            SELECT * FROM employee WHERE OrganizationID=og_id AND FirstName =emp_fname  AND LENGTH(emp_fname) > 0
        UNION
            SELECT * FROM employee WHERE OrganizationID=og_id AND LastName      =emp_lname  AND LENGTH(emp_lname) > 0
        UNION
            SELECT * FROM employee WHERE OrganizationID=og_id AND LENGTH(TRIM(emp_id))=0 AND LENGTH(TRIM(emp_fname))=0 AND LENGTH(TRIM(emp_lname))=0
            ) e

    LEFT JOIN `aspnetusers` u              ON e.CreatedBy=u.Id
    LEFT JOIN `aspnetusers` uu             ON e.LastUpdBy=uu.Id
    LEFT JOIN `position` pos        ON e.PositionID=pos.RowID
    LEFT JOIN payfrequency pf       ON e.PayFrequencyID=pf.RowID
    LEFT JOIN filingstatus fstat    ON fstat.MaritalStatus=e.MaritalStatus AND fstat.Dependent=e.NoOfDependents
    LEFT JOIN agency ag             ON ag.RowID=e.AgencyID
    LEFT JOIN division d                ON d.RowID=pos.DivisionId
    LEFT JOIN product vacationLeaveProduct
    ON vacationLeaveProduct.PartNo = 'Vacation leave' AND
        vacationLeaveProduct.OrganizationID = og_id AND vacationLeaveProduct.CategoryID = leaveTypeCategoryId
    LEFT JOIN leaveledger vacationLeaveLedger
    ON vacationLeaveLedger.ProductID = vacationLeaveProduct.RowID AND
        vacationLeaveLedger.EmployeeID = e.RowID
    LEFT JOIN leavetransaction vacationLeaveTransaction
    ON vacationLeaveTransaction.RowID = vacationLeaveLedger.LastTransactionID
    LEFT JOIN product sickLeaveProduct
    ON sickLeaveProduct.PartNo = 'Sick leave' AND
        sickLeaveProduct.OrganizationID = og_id AND sickLeaveProduct.CategoryID = leaveTypeCategoryId
    LEFT JOIN leaveledger sickLeaveLedger
    ON sickLeaveLedger.ProductID = sickLeaveProduct.RowID AND
        sickLeaveLedger.EmployeeID = e.RowID
    LEFT JOIN leavetransaction sickLeaveTransaction
    ON sickLeaveTransaction.RowID = sickLeaveLedger.LastTransactionID

    ORDER BY e.LastName, e.FirstName
    LIMIT page_number, max_count_per_page;

ELSE

    SELECT e.RowID
            ,e.EmployeeID                                   `Employee ID`
            ,e.LastName                                     `Last Name`
            ,e.FirstName                                    `First Name`
            ,e.MiddleName                                   `Middle Name`
            ,e.Surname
            ,e.Nickname
            ,e.MaritalStatus                                `Marital Status`
            ,IFNULL(e.NoOfDependents,0)             `No. Of Dependents`
            ,DATE_FORMAT(e.Birthdate,'%m-%d-%Y')    `Birthdate`
            ,DATE_FORMAT(e.Startdate,'%m-%d-%Y')    `Startdate`
            ,IFNULL(d.Name,'')                          `Job Title`
            ,IFNULL(pos.PositionName,'')                `Position`
            ,e.Salutation
            ,e.TINNo                                            `TIN`
            ,e.SSSNo                                            `SSS No.`
            ,e.HDMFNo                                       `PAGIBIG No.`
            ,e.PhilHealthNo                             `PhilHealth No.`
            ,e.WorkPhone                                    `Work Phone No.`
            ,e.HomePhone                                    `Home Phone No.`
            ,e.MobilePhone                                  `Mobile Phone No.`
            ,e.HomeAddress                                  `Home address`
            ,e.EmailAddress                             `Email address`
            ,IF(e.Gender='M','Male','Female')       `Gender`
            ,e.EmploymentStatus                         `Employment Status`
            ,IFNULL(pf.PayFrequencyType,'')         `Pay Frequency`
            ,e.UndertimeOverride
            ,e.OvertimeOverride
            ,IFNULL(DATE_FORMAT(e.Created
                                    , '%m-%d-%Y'), '')  `Creation Date`
            ,CONCAT_WS(', '
                    , u.FirstName, u.LastName)          `Created by`
            ,IFNULL(DATE_FORMAT(e.Created
                                    , '%m-%d-%Y'), '')  `Last Update`
            ,CONCAT_WS(', '
                    , uu.FirstName, uu.LastName)        `LastUpdate by`
            ,IFNULL(pos.RowID,'')                       `PositionID`
            ,IFNULL(e.PayFrequencyID,'')                `PayFrequencyID`
            ,e.EmployeeType
            ,IFNULL(vacationLeaveTransaction.Balance, 0) `LeaveBalance`
            ,IFNULL(sickLeaveTransaction.Balance, 0) `SickLeaveBalance`
            ,e.MaternityLeaveBalance
            ,e.LeaveAllowance
            ,e.SickLeaveAllowance
            ,e.MaternityLeaveAllowance
            ,e.LeavePerPayPeriod
            ,e.SickLeavePerPayPeriod
            ,e.MaternityLeavePerPayPeriod
            ,IFNULL(fstat.RowID,3)                          `fstatRowID`
            ,e.AlphaListExempted
            ,e.WorkDaysPerYear
            ,CHAR_TO_DAYOFWEEK(e.DayOfRest)         `DayOfRest`
            ,e.ATMNo
            ,e.BankName
            ,e.OtherLeavePerPayPeriod
            ,e.OtherLeaveAllowance
            ,e.OtherLeaveBalance
            ,e.CalcHoliday
            ,e.CalcSpecialHoliday
            ,e.CalcNightDiff
            ,e.CalcNightDiffOT
            ,e.CalcRestDay
            ,e.CalcRestDayOT
            ,IFNULL(e.LateGracePeriod,0)                `LateGracePeriod`
            ,e.GracePeriodAsBuffer
            ,IFNULL(e.RevealInPayroll,1)                `RevealInPayroll`
            ,IFNULL(ag.AgencyName,'')                   `AgencyName`
            ,IFNULL(ag.RowID,'')                            `ag_RowID`
            ,e.BranchID
            ,e.BPIInsurance

            ,e.DateEvaluated
            ,e.DateRegularized
            
    FROM (SELECT * FROM employee WHERE OrganizationID=og_id AND EmployeeID  =emp_id     AND LENGTH(emp_id) > 0
        UNION
            SELECT * FROM employee WHERE OrganizationID=og_id AND FirstName =emp_fname  AND LENGTH(emp_fname) > 0
        UNION
            SELECT * FROM employee WHERE OrganizationID=og_id AND LastName      =emp_lname  AND LENGTH(emp_lname) > 0
        UNION
            SELECT * FROM employee WHERE OrganizationID=og_id AND LENGTH(TRIM(emp_id))=0 AND LENGTH(TRIM(emp_fname))=0 AND LENGTH(TRIM(emp_lname))=0
            ) e

    LEFT JOIN `aspnetusers` u              ON e.CreatedBy=u.Id
    LEFT JOIN `aspnetusers` uu             ON e.LastUpdBy=uu.Id
    LEFT JOIN `position` pos        ON e.PositionID=pos.RowID
    LEFT JOIN payfrequency pf       ON e.PayFrequencyID=pf.RowID
    LEFT JOIN filingstatus fstat    ON fstat.MaritalStatus=e.MaritalStatus AND fstat.Dependent=e.NoOfDependents
    LEFT JOIN agency ag             ON ag.RowID=e.AgencyID
    LEFT JOIN division d                ON d.RowID=pos.DivisionId
    LEFT JOIN product vacationLeaveProduct
    ON vacationLeaveProduct.PartNo = 'Vacation leave' AND
        vacationLeaveProduct.OrganizationID = og_id
    LEFT JOIN leaveledger vacationLeaveLedger
    ON vacationLeaveLedger.ProductID = vacationLeaveProduct.RowID AND
        vacationLeaveLedger.EmployeeID = e.RowID
    LEFT JOIN leavetransaction vacationLeaveTransaction
    ON vacationLeaveTransaction.RowID = vacationLeaveLedger.LastTransactionID
    LEFT JOIN product sickLeaveProduct
    ON sickLeaveProduct.PartNo = 'Sick leave' AND
        sickLeaveProduct.OrganizationID = og_id
    LEFT JOIN leaveledger sickLeaveLedger
    ON sickLeaveLedger.ProductID = sickLeaveProduct.RowID AND
        sickLeaveLedger.EmployeeID = e.RowID
    LEFT JOIN leavetransaction sickLeaveTransaction
    ON sickLeaveTransaction.RowID = sickLeaveLedger.LastTransactionID

    ORDER BY e.LastName, e.FirstName
    LIMIT page_number, max_count_per_page;

END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
