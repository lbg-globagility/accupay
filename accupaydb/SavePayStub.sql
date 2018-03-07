/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `SavePayStub`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `SavePayStub`(
    `pstub_RowID` INT,
    `pstub_OrganizationID` INT,
    `pstub_CreatedBy` INT,
    `pstub_LastUpdBy` INT,
    `pstub_PayPeriodID` INT,
    `pstub_EmployeeID` INT,
    `pstub_TimeEntryID` INT,
    `pstub_PayFromDate` DATE,
    `pstub_PayToDate` DATE,
    `$RegularHours` DECIMAL(15, 4),
    `$RegularPay` DECIMAL(15, 4),
    `$OvertimeHours` DECIMAL(15, 4),
    `$OvertimePay` DECIMAL(15, 4),
    `$NightDiffHours` DECIMAL(15, 4),
    `$NightDiffPay` DECIMAL(15, 4),
    `$NightDiffOvertimeHours` DECIMAL(15, 4),
    `$NightDiffOvertimePay` DECIMAL(15, 4),
    `$LateHours` DECIMAL(15, 4),
    `$LateDeduction` DECIMAL(15, 4),
    `$UndertimeHours` DECIMAL(15, 4),
    `$UndertimeDeduction` DECIMAL(15, 4),
    `$AbsentHours` DECIMAL(15, 4),
    `$AbsenceDeduction` DECIMAL(15, 4),
    `$RestDayHours` DECIMAL(15, 4),
    `$RestDayPay` DECIMAL(15, 4),
    `$RestDayOTHours` DECIMAL(15, 4),
    `$RestDayOTPay` DECIMAL(15, 4),
    `$LeaveHours` DECIMAL(15, 4),
    `$LeavePay` DECIMAL(15, 4),
    `$SpecialHolidayHours` DECIMAL(15, 4),
    `$SpecialHolidayPay` DECIMAL(15, 4),
    `$RegularHolidayHours` DECIMAL(15, 4),
    `$RegularHolidayPay` DECIMAL(15, 4),
    `$HolidayPay` DECIMAL(15, 4),
    `$WorkPay` DECIMAL(15, 4),
    `pstub_TotalGrossSalary` DECIMAL(15, 4),
    `pstub_TotalNetSalary` DECIMAL(15, 4),
    `pstub_TotalTaxableSalary` DECIMAL(15, 4),
    `pstub_TotalEmpSSS` DECIMAL(15, 4),
    `pstub_TotalCompSSS` DECIMAL(15, 4),
    `pstub_TotalEmpPhilHealth` DECIMAL(15, 4),
    `pstub_TotalCompPhilHealth` DECIMAL(15, 4),
    `pstub_TotalEmpHDMF` DECIMAL(15, 4),
    `pstub_TotalCompHDMF` DECIMAL(15, 4),
    `pstub_TotalEmpWithholdingTax` DECIMAL(15, 4),
    `pstub_TotalVacationDaysLeft` DECIMAL(15, 4),
    `pstub_TotalLoans` DECIMAL(15, 4),
    `pstub_TotalBonus` DECIMAL(15, 4),
    `pstub_TotalAllowance` DECIMAL(15, 4)
) RETURNS int(11)
BEGIN

DECLARE payStubID INT(11);
DECLARE payStubIDs VARCHAR(2000);

DECLARE totalAdjustments DECIMAL(15, 4);

DECLARE undeclaredSalaryRatio DECIMAL(15, 6);
DECLARE totalUndeclaredSalary DECIMAL(15, 4);

SELECT paystub.RowID
FROM paystub
WHERE paystub.EmployeeID = pstub_EmployeeID AND
    paystub.OrganizationID = pstub_OrganizationID AND
    paystub.PayFromDate = pstub_PayFromDate AND
    paystub.PayToDate = pstub_PayToDate
INTO payStubIDs;

DELETE FROM paystubactual
WHERE paystubactual.RowID != payStubIDs AND
    paystubactual.EmployeeID = pstub_EmployeeID AND
    paystubactual.OrganizationID = pstub_OrganizationID AND
    paystubactual.PayFromDate = pstub_PayFromDate AND
    paystubactual.PayToDate = pstub_PayToDate;

SELECT paystub.RowID
FROM paystub
WHERE paystub.PayPeriodID = pstub_PayPeriodID AND
    paystub.EmployeeID = pstub_EmployeeID AND
    paystub.OrganizationID = pstub_OrganizationID/*AND
    paystub.PayFromDate = pstub_PayFromDate AND
    paystub.PayToDate = pstub_PayToDate*/
LIMIT 1
INTO payStubID;

SET totalAdjustments = IFNULL(
    GET_SumPayStubAdjustments(
        IF(
            pstub_RowID IS NULL,
            payStubID,
            pstub_RowID
        )
    ),
    0
);

SELECT GET_employeeundeclaredsalarypercent(
    pstub_EmployeeID,
    pstub_OrganizationID,
    pstub_PayFromDate,
    pstub_PayToDate
)
INTO undeclaredSalaryRatio;

IF undeclaredSalaryRatio < 1.0 THEN
    SET undeclaredSalaryRatio = undeclaredSalaryRatio + 1.0;
ELSEIF undeclaredSalaryRatio > 1.0 THEN
    SET undeclaredSalaryRatio = undeclaredSalaryRatio - 1.0;
END IF;

SET totalUndeclaredSalary = (pstub_TotalNetSalary + totalAdjustments) * undeclaredSalaryRatio;


INSERT INTO paystub
(
    paystub.RowID,
    paystub.OrganizationID,
    paystub.CreatedBy,
    paystub.PayPeriodID,
    paystub.EmployeeID,
    paystub.TimeEntryID,
    paystub.PayFromDate,
    paystub.PayToDate,
    paystub.RegularHours,
    paystub.RegularPay,
    paystub.OvertimeHours,
    paystub.OvertimePay,
    paystub.NightDiffHours,
    paystub.NightDiffPay,
    paystub.NightDiffOvertimeHours,
    paystub.NightDiffOvertimePay,
    paystub.RestDayHours,
    paystub.RestDayPay,
    paystub.RestDayOTHours,
    paystub.RestDayOTPay,
    paystub.LeaveHours,
    paystub.LeavePay,
    paystub.SpecialHolidayHours,
    paystub.SpecialHolidayPay,
    paystub.RegularHolidayHours,
    paystub.RegularHolidayPay,
    paystub.HolidayPay,
    paystub.LateHours,
    paystub.LateDeduction,
    paystub.UndertimeHours,
    paystub.UndertimeDeduction,
    paystub.AbsentHours,
    paystub.AbsenceDeduction,
    paystub.WorkPay,
    paystub.TotalGrossSalary,
    paystub.TotalNetSalary,
    paystub.TotalTaxableSalary,
    paystub.TotalEmpSSS,
    paystub.TotalCompSSS,
    paystub.TotalEmpPhilhealth,
    paystub.TotalCompPhilhealth,
    paystub.TotalEmpHDMF,
    paystub.TotalCompHDMF,
    paystub.TotalEmpWithholdingTax,
    paystub.TotalVacationDaysLeft,
    paystub.TotalLoans,
    paystub.TotalBonus,
    paystub.TotalAllowance,
    paystub.TotalAdjustments,
    paystub.TotalUndeclaredSalary
)
VALUES
(
    pstub_RowID,
    pstub_OrganizationID,
    pstub_CreatedBy,
    pstub_PayPeriodID,
    pstub_EmployeeID,
    pstub_TimeEntryID,
    pstub_PayFromDate,
    pstub_PayToDate,
    $RegularHours,
    $RegularPay,
    $OvertimeHours,
    $OvertimePay,
    $NightDiffHours,
    $NightDiffPay,
    $NightDiffOvertimeHours,
    $NightDiffOvertimePay,
    $RestDayHours,
    $RestDayPay,
    $RestDayOTHours,
    $RestDayOTPay,
    $LeaveHours,
    $LeavePay,
    $SpecialHolidayHours,
    $SpecialHolidayPay,
    $RegularHolidayHours,
    $RegularHolidayPay,
    $HolidayPay,
    $LateHours,
    $LateDeduction,
    $UndertimeHours,
    $UndertimeDeduction,
    $AbsentHours,
    $AbsenceDeduction,
    $WorkPay,
    pstub_TotalGrossSalary,
    (pstub_TotalNetSalary + (totalAdjustments)),
    pstub_TotalTaxableSalary,
    pstub_TotalEmpSSS,
    pstub_TotalCompSSS,
    pstub_TotalEmpPhilHealth,
    pstub_TotalCompPhilHealth,
    pstub_TotalEmpHDMF,
    pstub_TotalCompHDMF,
    pstub_TotalEmpWithholdingTax,
    pstub_TotalVacationDaysLeft,
    pstub_TotalLoans,
    pstub_TotalBonus,
    pstub_TotalAllowance,
    totalAdjustments,
    totalUndeclaredSalary
)
ON DUPLICATE KEY
UPDATE
    paystub.LastUpd = CURRENT_TIMESTAMP(),
    paystub.LastUpdBy = pstub_LastUpdBy,
    paystub.PayPeriodID = pstub_PayPeriodID,
    paystub.EmployeeID = pstub_EmployeeID,
    paystub.TimeEntryID = pstub_TimeEntryID,
    paystub.PayFromDate = pstub_PayFromDate,
    paystub.PayToDate = pstub_PayToDate,
    paystub.RegularHours = $RegularHours,
    paystub.RegularPay = $RegularPay,
    paystub.OvertimeHours = $OvertimeHours,
    paystub.OvertimePay = $OvertimePay,
    paystub.NightDiffHours = $NightDiffHours,
    paystub.NightDiffPay = $NightDiffPay,
    paystub.NightDiffOvertimeHours = $NightDiffOvertimeHours,
    paystub.NightDiffOvertimePay = $NightDiffOvertimePay,
    paystub.RestDayHours = $RestDayHours,
    paystub.RestDayPay = $RestDayPay,
    paystub.RestDayOTHours = $RestDayOTHours,
    paystub.RestDayOTPay = $RestDayOTPay,
    paystub.LeaveHours = $LeaveHours,
    paystub.LeavePay = $LeavePay,
    paystub.SpecialHolidayHours = $SpecialHolidayHours,
    paystub.SpecialHolidayPay = $SpecialHolidayPay,
    paystub.RegularHolidayHours = $RegularHolidayHours,
    paystub.RegularHolidayPay = $RegularHolidayPay,
    paystub.HolidayPay = $HolidayPay,
    paystub.LateHours = $LateHours,
    paystub.LateDeduction = $LateDeduction,
    paystub.UndertimeHours = $UndertimeHours,
    paystub.UndertimeDeduction = $UndertimeDeduction,
    paystub.AbsentHours = $AbsentHours,
    paystub.AbsenceDeduction = $AbsenceDeduction,
    paystub.WorkPay = $WorkPay,
    paystub.TotalGrossSalary = pstub_TotalGrossSalary,
    paystub.TotalNetSalary = (pstub_TotalNetSalary + (totalAdjustments)),
    paystub.TotalTaxableSalary = pstub_TotalTaxableSalary,
    paystub.TotalEmpSSS = pstub_TotalEmpSSS,
    paystub.TotalCompSSS = pstub_TotalCompSSS,
    paystub.TotalEmpPhilhealth = pstub_TotalEmpPhilHealth,
    paystub.TotalCompPhilhealth = pstub_TotalCompPhilHealth,
    paystub.TotalEmpHDMF = pstub_TotalEmpHDMF,
    paystub.TotalCompHDMF = pstub_TotalCompHDMF,
    paystub.TotalEmpWithholdingTax = pstub_TotalEmpWithholdingTax,
    paystub.TotalVacationDaysLeft = pstub_TotalVacationDaysLeft,
    paystub.TotalLoans = pstub_TotalLoans,
    paystub.TotalBonus = pstub_TotalBonus,
    paystub.TotalAllowance = pstub_TotalAllowance,
    paystub.TotalAdjustments = totalAdjustments,
    paystub.TotalUndeclaredSalary = totalUndeclaredSalary;

IF payStubID IS NULL THEN
    RETURN LAST_INSERT_ID();
ELSE
    RETURN payStubID;
END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
