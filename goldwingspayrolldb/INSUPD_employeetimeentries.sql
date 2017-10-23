/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_employeetimeentries`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_employeetimeentries`(
    `etent_RowID` INT,
    `etent_OrganizationID` INT,
    `etent_CreatedBy` INT,
    `etent_LastUpdBy` INT,
    `etent_Date` DATE,
    `etent_EmployeeShiftID` INT,
    `etent_EmployeeID` INT,
    `etent_EmployeeSalaryID` INT,
    `etent_EmployeeFixedSalaryFlag` CHAR(50),
    `etent_RegularHoursWorked` DECIMAL(11,6),
    `etent_OvertimeHoursWorked` DECIMAL(11,6),
    `etent_UndertimeHours` DECIMAL(11,6),
    `etent_NightDifferentialHours` DECIMAL(11,6),
    `etent_NightDifferentialOTHours` DECIMAL(11,6),
    `etent_HoursLate` DECIMAL(11,6),
    `etent_PayRateID` INT,
    `etent_TotalDayPay` DECIMAL(11,6),
    `etent_TotalHoursWorked` DECIMAL(11,6),
    `etent_RegularHoursAmount` DECIMAL(11,6),
    `etent_OvertimeHoursAmount` DECIMAL(11,6),
    `etent_UndertimeHoursAmount` DECIMAL(11,6),
    `etent_NightDiffHoursAmount` DECIMAL(11,6),
    `etent_NightDiffOTHoursAmount` DECIMAL(11,6),
    `etent_HoursLateAmount` DECIMAL(11,6),
    `etent_RestDayHours` DECIMAL(11,6),
    `etent_RestDayAmount` DECIMAL(11,6),
    `etent_HolidayPay` DECIMAL(11,6),
    `etent_BasicDayPay` DECIMAL(11, 6),
    `etent_No` INT,
    `etent_LeaveType` VARCHAR(50),
    `etent_LeaveHours` DECIMAL(15, 4),
    `etent_LeavePay` DECIMAL(15, 4)
) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE etentID INT(11);

INSERT INTO employeetimeentry
(
    RowID,
    OrganizationID,
    Created,
    CreatedBy,
    LastUpdBy,
    `Date`,
    EmployeeShiftID,
    EmployeeID,
    EmployeeSalaryID,
    EmployeeFixedSalaryFlag,
    TotalHoursWorked,
    RegularHoursWorked,
    RegularHoursAmount,
    OvertimeHoursWorked,
    OvertimeHoursAmount,
    UndertimeHours,
    UndertimeHoursAmount,
    NightDifferentialHours,
    NightDiffHoursAmount,
    NightDifferentialOTHours,
    NightDiffOTHoursAmount,
    HoursLate,
    HoursLateAmount,
    LateFlag,
    PayRateID,
    TotalDayPay,
    HolidayPayAmount,
    BasicDayPay,
    RestDayHours,
    RestDayAmount,
    VacationLeaveHours,
    SickLeaveHours,
    MaternityLeaveHours,
    OtherLeaveHours,
    Leavepayment
) VALUES (
    etent_RowID,
    etent_OrganizationID,
    CURRENT_TIMESTAMP(),
    etent_CreatedBy,
    etent_CreatedBy,
    etent_Date,
    etent_EmployeeShiftID,
    etent_EmployeeID,
    etent_EmployeeSalaryID,
    etent_EmployeeFixedSalaryFlag,
    etent_TotalHoursWorked,
    etent_RegularHoursWorked,
    etent_RegularHoursAmount,
    etent_OvertimeHoursWorked,
    etent_OvertimeHoursAmount,
    etent_UndertimeHours,
    etent_UndertimeHoursAmount,
    etent_NightDifferentialHours,
    etent_NightDiffHoursAmount,
    etent_NightDifferentialOTHours,
    etent_NightDiffOTHoursAmount,
    etent_HoursLate,
    etent_HoursLateAmount,
    IF(etent_HoursLateAmount = 0, '0', '1'),
    etent_PayRateID,
    etent_TotalDayPay,
    etent_HolidayPay,
    etent_BasicDayPay,
    etent_RestDayHours,
    etent_RestDayAmount,
    IF(etent_LeaveType = 'Vacation leave', etent_LeaveHours, 0),
    IF(etent_LeaveType = 'Sick leave', etent_LeaveHours, 0),
    IF(etent_LeaveType = 'Maternity leave', etent_LeaveHours, 0),
    IF(etent_LeaveType = 'Other leave', etent_LeaveHours, 0),
    etent_LeavePay
)
ON DUPLICATE KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP(),
    LastUpdBy=etent_LastUpdBy,
    EmployeeShiftID = etent_EmployeeShiftID,
    TotalHoursWorked = etent_TotalHoursWorked,
    RegularHoursWorked = etent_RegularHoursWorked,
    RegularHoursAmount = etent_RegularHoursAmount,
    OvertimeHoursWorked = etent_OvertimeHoursWorked,
    OvertimeHoursAmount = etent_OvertimeHoursAmount,
    UndertimeHours = etent_UndertimeHours,
    UndertimeHoursAmount = etent_UndertimeHoursAmount,
    NightDifferentialHours = etent_NightDifferentialHours,
    NightDiffHoursAmount = etent_NightDiffHoursAmount,
    NightDifferentialOTHours = etent_NightDifferentialOTHours,
    NightDiffOTHoursAmount = etent_NightDiffOTHoursAmount,
    HoursLate = etent_HoursLate,
    HoursLateAmount = etent_HoursLateAmount,
    LateFlag = IF(etent_HoursLateAmount = 0, '0', '1'),
    TotalDayPay = etent_TotalDayPay,
    HolidayPayAmount = etent_HolidayPay,
    BasicDayPay = etent_BasicDayPay,
    RestDayHours = etent_RestDayHours,
    RestDayAmount = etent_RestDayAmount,
    VacationLeaveHours = IF(etent_LeaveType = 'Vacation leave', etent_LeaveHours, 0),
    SickLeaveHours = IF(etent_LeaveType = 'Sick leave', etent_LeaveHours, 0),
    MaternityLeaveHours = IF(etent_LeaveType = 'Maternity leave', etent_LeaveHours, 0),
    OtherLeaveHours = IF(etent_LeaveType = 'Other leave', etent_LeaveHours, 0),
    Leavepayment = etent_LeavePay;

SELECT @@Identity AS id INTO etentID;

RETURN etentID;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
