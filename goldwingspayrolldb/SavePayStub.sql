/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `SavePayStub`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `SavePayStub`(
	`RowID` INT,
	`OrganizationID` INT,
	`Created` INT,
	`CreatedBy` INT,
	`PayPeriodID` INT,
	`EmployeeID` INT,
	`TimeEntryID` INT,
	`PayFromDate` INT,
	`PayToDate` INT,
	`TotalGrossSalary` INT,
	`TotalNetSalary` INT,
	`TotalTaxableSalary` INT,
	`TotalEmpSSS` INT,
	`TotalCompSSS` INT,
	`TotalEmpPhilHealth` INT,
	`TotalCompPhilHealth` INT,
	`TotalEmpHDMF` INT,
	`TotalCompHDMF` INT,
	`TotalEmpWithholdingTax` INT,
	`TotalVacationDaysLeft` INT,
	`TotalLoans` INT,
	`TotalBonus` INT,
	`TotalAllowance` INT,
	`TotalUndeclaredSalary` INT

) RETURNS int(11)
BEGIN

    INSERT INTO paystub
    (
        paystub.RowID,
        paystub.OrganizationID,
        paystub.Created,
        paystub.CreatedBy,
        paystub.PayPeriodID,
        paystub.EmployeeID,
        paystub.TimeEntryID,
        paystub.PayFromDate,
        paystub.PayToDate,
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
        RowID,
        OrganizationID,
        Created,
        CreatedBy,
        PayPeriodID,
        EmployeeID,
        TimeEntryID,
        PayFromDate,
        PayToDate,
        TotalGrossSalary,
        TotalNetSalary,
        TotalTaxableSalary,
        TotalEmpSSS,
        TotalCompSSS,
        TotalEmpPhilHealth,
        TotalCompPhilHealth,
        TotalEmpHDMF,
        TotalCompHDMF,
        TotalEmpWithholdingTax,
        TotalVacationDaysLeft,
        TotalLoans,
        TotalBonus,
        TotalAllowance,
        TotalAdjustments,
        TotalUndeclaredSalary
    );

    RETURN LAST_INSERT_ID();

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
