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

-- Dumping structure for function goldwingspayrolldb.INSUPD_paystub
DROP FUNCTION IF EXISTS `INSUPD_paystub`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_paystub`(`pstub_RowID` INT, `pstub_OrganizationID` INT, `pstub_CreatedBy` INT, `pstub_LastUpdBy` INT, `pstub_PayPeriodID` INT, `pstub_EmployeeID` INT, `pstub_TimeEntryID` INT, `pstub_PayFromDate` DATE, `pstub_PayToDate` DATE, `pstub_TotalGrossSalary` DECIMAL(10,2), `pstub_TotalNetSalary` DECIMAL(10,2), `pstub_TotalTaxableSalary` DECIMAL(10,2), `pstub_TotalEmpSSS` DECIMAL(10,2), `pstub_TotalEmpWithholdingTax` DECIMAL(10,2), `pstub_TotalCompSSS` DECIMAL(10,2), `pstub_TotalEmpPhilhealth` DECIMAL(10,2), `pstub_TotalCompPhilhealth` DECIMAL(10,2), `pstub_TotalEmpHDMF` DECIMAL(10,2), `pstub_TotalCompHDMF` DECIMAL(10,2), `pstub_TotalVacationDaysLeft` DECIMAL(10,2), `pstub_TotalLoans` DECIMAL(10,2), `pstub_TotalBonus` DECIMAL(10,2), `pstub_TotalAllowance` DECIMAL(10,2)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE paystubID INT(11);

DECLARE existingrowrecord INT(11);

DECLARE isexist INT(1);

DECLARE SumPayStubAdjustments DECIMAL(11,2);

DECLARE ps_TotalUndeclaredSalary DECIMAL(11,6) DEFAULT 0.0;
DECLARE ps_rowIDs VARCHAR(2000);
SELECT RowID FROM paystub WHERE EmployeeID=pstub_EmployeeID AND OrganizationID=pstub_OrganizationID AND PayFromDate=pstub_PayFromDate AND PayToDate=pstub_PayToDate INTO ps_rowIDs;
DELETE FROM paystubactual WHERE RowID != ps_rowIDs AND EmployeeID=pstub_EmployeeID AND OrganizationID=pstub_OrganizationID AND PayFromDate=pstub_PayFromDate AND PayToDate=pstub_PayToDate;

SELECT RowID FROM paystub WHERE PayPeriodID=pstub_PayPeriodID AND EmployeeID=pstub_EmployeeID AND OrganizationID=pstub_OrganizationID AND PayFromDate=pstub_PayFromDate AND PayToDate=pstub_PayToDate LIMIT 1 INTO existingrowrecord;

SET SumPayStubAdjustments = IFNULL(GET_SumPayStubAdjustments(IF(pstub_RowID IS NULL, existingrowrecord, pstub_RowID)),0);

SELECT GET_employeeundeclaredsalarypercent(pstub_EmployeeID,pstub_OrganizationID,pstub_PayFromDate,pstub_PayToDate) INTO ps_TotalUndeclaredSalary;

IF ps_TotalUndeclaredSalary < 1.0 THEN
	SET ps_TotalUndeclaredSalary = ps_TotalUndeclaredSalary + 1.000000;
ELSEIF ps_TotalUndeclaredSalary > 1.0 THEN
	SET ps_TotalUndeclaredSalary = ps_TotalUndeclaredSalary - 1.000000;
END IF;

SET ps_TotalUndeclaredSalary = CAST(ps_TotalUndeclaredSalary AS DECIMAL(11,6));

SET ps_TotalUndeclaredSalary = (pstub_TotalNetSalary + SumPayStubAdjustments) * ps_TotalUndeclaredSalary;

INSERT INTO paystub
(
	RowID
	,OrganizationID
	,Created
	,CreatedBy
	,PayPeriodID
	,EmployeeID
	,TimeEntryID
	,PayFromDate
	,PayToDate
	,TotalGrossSalary
	,TotalNetSalary
	,TotalTaxableSalary
	,TotalEmpSSS
	,TotalEmpWithholdingTax
	,TotalCompSSS
	,TotalEmpPhilhealth
	,TotalCompPhilhealth
	,TotalEmpHDMF
	,TotalCompHDMF
	,TotalVacationDaysLeft
	,TotalLoans
	,TotalBonus
	,TotalAllowance
	,TotalAdjustments
	,TotalUndeclaredSalary
) VALUES (
	IF(pstub_RowID IS NULL, existingrowrecord, pstub_RowID)
	,pstub_OrganizationID
	,CURRENT_TIMESTAMP()
	,pstub_CreatedBy
	,pstub_PayPeriodID
	,pstub_EmployeeID
	,pstub_TimeEntryID
	,pstub_PayFromDate
	,pstub_PayToDate
	,pstub_TotalGrossSalary
	,pstub_TotalNetSalary + SumPayStubAdjustments
	,pstub_TotalTaxableSalary
	,pstub_TotalEmpSSS
	,pstub_TotalEmpWithholdingTax
	,pstub_TotalCompSSS
	,pstub_TotalEmpPhilhealth
	,pstub_TotalCompPhilhealth
	,pstub_TotalEmpHDMF
	,pstub_TotalCompHDMF
	,COALESCE((SELECT COALESCE(LeaveBalance,0) + COALESCE(SickLeaveBalance,0) + COALESCE(MaternityLeaveBalance,0) FROM employee WHERE RowID=pstub_EmployeeID),0)
	,pstub_TotalLoans
	,pstub_TotalBonus
	,pstub_TotalAllowance
	,SumPayStubAdjustments
	,ps_TotalUndeclaredSalary
) ON 
DUPLICATE 
KEY 
UPDATE 
	LastUpd=CURRENT_TIMESTAMP()
	,LastUpdBy=pstub_LastUpdBy
	,PayPeriodID=pstub_PayPeriodID
	,EmployeeID=pstub_EmployeeID
	,TimeEntryID=pstub_TimeEntryID
	,PayFromDate=pstub_PayFromDate
	,PayToDate=pstub_PayToDate
	,TotalGrossSalary=pstub_TotalGrossSalary
	,TotalNetSalary=pstub_TotalNetSalary + SumPayStubAdjustments
	,TotalTaxableSalary=pstub_TotalTaxableSalary
	,TotalEmpSSS=pstub_TotalEmpSSS
	,TotalEmpWithholdingTax=pstub_TotalEmpWithholdingTax
	,TotalCompSSS=pstub_TotalCompSSS
	,TotalEmpPhilhealth=pstub_TotalEmpPhilhealth
	,TotalCompPhilhealth=pstub_TotalCompPhilhealth
	,TotalEmpHDMF=pstub_TotalEmpHDMF
	,TotalCompHDMF=pstub_TotalCompHDMF
	,TotalLoans=pstub_TotalLoans
	,TotalBonus=pstub_TotalBonus
	,TotalAllowance=pstub_TotalAllowance
	,TotalAdjustments=SumPayStubAdjustments
	,TotalUndeclaredSalary=ps_TotalUndeclaredSalary;SELECT @@Identity AS id INTO paystubID;


IF existingrowrecord IS NULL THEN

	RETURN paystubID;
	
ELSE
	
	RETURN existingrowrecord;

END IF;



END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
