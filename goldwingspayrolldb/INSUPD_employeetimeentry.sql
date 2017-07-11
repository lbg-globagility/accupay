/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_employeetimeentry`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_employeetimeentry`(`etent_RowID` INT, `etent_OrganizationID` INT, `etent_CreatedBy` INT, `etent_LastUpdBy` INT, `etent_Date` DATE, `etent_EmployeeShiftID` INT, `etent_EmployeeID` INT, `etent_EmployeeSalaryID` INT, `etent_EmployeeFixedSalaryFlag` CHAR(50), `etent_RegularHoursWorked` DECIMAL(11,6), `etent_OvertimeHoursWorked` DECIMAL(11,6), `etent_UndertimeHours` DECIMAL(11,6), `etent_NightDifferentialHours` DECIMAL(11,6), `etent_NightDifferentialOTHours` DECIMAL(11,6), `etent_HoursLate` DECIMAL(11,6), `etent_PayRateID` INT, `etent_VacationLeaveHours` DECIMAL(11,6), `etent_SickLeaveHours` DECIMAL(11,6), `etent_TotalDayPay` DECIMAL(11,6), `etent_IsNightShift` CHAR(50), `etent_TotalHoursWorked` DECIMAL(11,6), `etent_RegularHoursAmount` DECIMAL(11,6), `etent_OvertimeHoursAmount` DECIMAL(11,6), `etent_UndertimeHoursAmount` DECIMAL(11,6), `etent_NightDiffHoursAmount` DECIMAL(11,6), `etent_NightDiffOTHoursAmount` DECIMAL(11,6), `etent_HoursLateAmount` DECIMAL(11,6), `etent_MaternityLeaveHours` DECIMAL(11,6), `etent_OtherLeaveHours` DECIMAL(11,6)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE etentID INT(11);

INSERT INTO employeetimeentry
(
    RowID
    ,OrganizationID
    ,Created
    ,CreatedBy
    ,LastUpdBy
    ,Date
    ,EmployeeShiftID
    ,EmployeeID
    ,EmployeeSalaryID
    ,EmployeeFixedSalaryFlag
    ,TotalHoursWorked
    ,RegularHoursWorked
    ,RegularHoursAmount
    ,OvertimeHoursWorked
    ,OvertimeHoursAmount
    ,UndertimeHours
    ,UndertimeHoursAmount
    ,NightDifferentialHours
    ,NightDiffHoursAmount
    ,NightDifferentialOTHours
    ,NightDiffOTHoursAmount
    ,HoursLate
    ,HoursLateAmount
    ,LateFlag
    ,PayRateID
    ,VacationLeaveHours
    ,SickLeaveHours
    ,MaternityLeaveHours
    ,OtherLeaveHours
    ,TotalDayPay
) VALUES (
    etent_RowID
    ,etent_OrganizationID
    ,CURRENT_TIMESTAMP()
    ,etent_CreatedBy
    ,etent_LastUpdBy
    ,COALESCE(etent_Date,(SELECT Date FROM payrate WHERE RowID=etent_PayRateID))
    ,COALESCE(etent_EmployeeShiftID,(SELECT RowID FROM employeeshift WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND etent_Date BETWEEN DATE(COALESCE(EffectiveFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) AND DATEDIFF(CURRENT_DATE(),EffectiveFrom) >= 0 ORDER BY DATEDIFF(DATE_FORMAT(NOW(),'%Y-%m-%d'),EffectiveFrom) LIMIT 1))
    ,etent_EmployeeID
    ,COALESCE(etent_EmployeeSalaryID,(SELECT RowID FROM employeesalary WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND etent_Date BETWEEN DATE(COALESCE(EffectiveDateFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveDateTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) ORDER BY DATEDIFF(DATE_FORMAT(NOW(),'%Y-%m-%d'),EffectiveDateFrom) LIMIT 1))
    ,etent_EmployeeFixedSalaryFlag
    ,etent_TotalHoursWorked
    ,etent_RegularHoursWorked
    ,etent_RegularHoursAmount
    ,IF(etent_IsNightShift = 1,0,etent_OvertimeHoursWorked)
    ,etent_OvertimeHoursAmount
    ,IF(etent_UndertimeHours < 0,etent_UndertimeHours * -1,etent_UndertimeHours)
    ,etent_UndertimeHoursAmount
    ,etent_NightDifferentialHours
    ,etent_NightDiffHoursAmount
    ,IF(etent_IsNightShift = 1,etent_OvertimeHoursWorked,0)
    ,etent_NightDiffOTHoursAmount
    ,IF(etent_HoursLate < 0,etent_HoursLate * -1,etent_HoursLate)
    ,etent_HoursLateAmount
    ,IF(COALESCE(etent_HoursLate,0)>0,1,0)
    ,COALESCE(etent_PayRateID,(SELECT RowID FROM payrate WHERE Date=etent_Date AND OrganizationID=etent_OrganizationID LIMIT 1))
    ,etent_VacationLeaveHours
    ,etent_SickLeaveHours
    ,etent_MaternityLeaveHours
    ,etent_OtherLeaveHours
    ,etent_TotalDayPay
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=etent_LastUpdBy
    ,Date=COALESCE(etent_Date,(SELECT Date FROM payrate WHERE RowID=etent_PayRateID))
    ,EmployeeShiftID=
        COALESCE(
            etent_EmployeeShiftID,
            (
                SELECT RowID
                FROM employeeshift
                WHERE EmployeeID = etent_EmployeeID
                    AND OrganizationID = etent_OrganizationID
                    AND etent_Date
                        BETWEEN DATE(COALESCE(EffectiveFrom, DATE_FORMAT(CURRENT_TIMESTAMP(), '%Y-%m-%d')))
                        AND DATE(COALESCE(EffectiveTo, ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH)))
                ORDER BY DATEDIFF(DATE_FORMAT(NOW(), '%Y-%m-%d'), EffectiveFrom)
                LIMIT 1
            )
        )
    ,EmployeeSalaryID=COALESCE(etent_EmployeeSalaryID,(SELECT RowID FROM employeesalary WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND etent_Date BETWEEN DATE(COALESCE(EffectiveDateFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveDateTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) ORDER BY DATEDIFF(DATE_FORMAT(NOW(),'%Y-%m-%d'),EffectiveDateFrom) LIMIT 1))
    ,EmployeeFixedSalaryFlag=etent_EmployeeFixedSalaryFlag
    ,TotalHoursWorked=etent_TotalHoursWorked
    ,RegularHoursWorked=etent_RegularHoursWorked
    ,RegularHoursAmount=etent_RegularHoursAmount
    ,OvertimeHoursWorked=IF(etent_IsNightShift = 1,0,etent_OvertimeHoursWorked)
    ,OvertimeHoursAmount=etent_OvertimeHoursAmount
    ,UndertimeHours=IF(COALESCE(etent_HoursLate,0)>0,0,IF(etent_UndertimeHours < 0,etent_UndertimeHours * -1,etent_UndertimeHours))
    ,UndertimeHoursAmount=etent_UndertimeHoursAmount
    ,NightDifferentialHours=etent_NightDifferentialHours
    ,NightDiffHoursAmount=etent_NightDiffHoursAmount
    ,NightDifferentialOTHours=IF(etent_IsNightShift = 1,etent_OvertimeHoursWorked,0)
    ,NightDiffOTHoursAmount=etent_NightDiffOTHoursAmount
    ,HoursLate=IF(etent_HoursLate < 0,etent_HoursLate * -1,etent_HoursLate)
    ,HoursLateAmount=etent_HoursLateAmount
    ,LateFlag=IF(COALESCE(etent_HoursLate,0)>=1,1,0)
    ,PayRateID=COALESCE(etent_PayRateID,(SELECT RowID FROM payrate WHERE Date=etent_Date AND OrganizationID=etent_OrganizationID LIMIT 1))
    ,VacationLeaveHours=etent_VacationLeaveHours
    ,SickLeaveHours=etent_SickLeaveHours
    ,MaternityLeaveHours=etent_MaternityLeaveHours
    ,OtherLeaveHours=etent_OtherLeaveHours
    ,TotalDayPay=etent_TotalDayPay;SELECT @@Identity AS id INTO etentID;

RETURN etentID;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
