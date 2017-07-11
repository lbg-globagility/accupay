/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_paystubbonus`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `INSUPD_paystubbonus`(`OrganizID` INT, `EmpRowID` INT, `UserRowID` INT, `psb_PayPeriodID` INT
, `psb_PayFromDate` DATE
, `psb_PayToDate` DATE
, `psb_TotalGrossSalary` DECIMAL(11,6)
, `psb_TotalNetSalary` DECIMAL(11,6)
, `psb_TotalTaxableSalary` DECIMAL(11,6)
, `psb_TotalEmpSSS` DECIMAL(11,6)
, `psb_TotalEmpWithholdingTax` DECIMAL(11,6)
, `psb_TotalCompSSS` DECIMAL(11,6)
, `psb_TotalEmpPhilhealth` DECIMAL(11,6)
, `psb_TotalCompPhilhealth` DECIMAL(11,6)
, `psb_TotalEmpHDMF` DECIMAL(11,6)
, `psb_TotalCompHDMF` DECIMAL(11,6)
, `psb_TotalVacationDaysLeft` DECIMAL(11,6)
, `psb_TotalUndeclaredSalary` DECIMAL(11,6)
, `psb_TotalLoans` DECIMAL(11,6)
, `psb_TotalBonus` DECIMAL(11,6)
, `psb_TotalAllowance` DECIMAL(11,6)
, `psb_TotalAdjustments` DECIMAL(11,6)
, `psb_ThirteenthMonthInclusion` CHAR(1)
, `psb_FirstTimeSalary` CHAR(1)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11);

INSERT INTO paystubbonus
(

    OrganizationID
    ,Created
    ,CreatedBy
    ,PayPeriodID
    ,EmployeeID
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
    ,TotalUndeclaredSalary
    ,TotalLoans
    ,TotalBonus
    ,TotalAllowance
    ,TotalAdjustments
    ,ThirteenthMonthInclusion
    ,FirstTimeSalary

)   SELECT

    OrganizID
    ,CURRENT_TIMESTAMP()
    ,UserRowID
    ,psb_PayPeriodID
    ,EmpRowID
    ,psb_PayFromDate
    ,psb_PayToDate
    ,psb_TotalGrossSalary
    ,IFNULL(eb.SumBonus,0)
    ,psb_TotalTaxableSalary
    ,psb_TotalEmpSSS
    ,psb_TotalEmpWithholdingTax
    ,psb_TotalCompSSS
    ,psb_TotalEmpPhilhealth
    ,psb_TotalCompPhilhealth
    ,psb_TotalEmpHDMF
    ,psb_TotalCompHDMF
    ,psb_TotalVacationDaysLeft
    ,psb_TotalUndeclaredSalary
    ,psb_TotalLoans
    ,psb_TotalBonus
    ,psb_TotalAllowance
    ,psb_TotalAdjustments
    ,psb_ThirteenthMonthInclusion
    ,((e.StartDate BETWEEN psb_PayFromDate AND psb_PayToDate) OR (e.StartDate <= psb_PayFromDate)) AND (IFNULL(psb.FirstTimeSalary,0) = '0')
    FROM employee e

    LEFT JOIN (SELECT RowID,FirstTimeSalary FROM paystubbonus WHERE EmployeeID=EmpRowID AND OrganizationID=OrganizID ORDER BY DATEDIFF(PayToDate,CURDATE()) LIMIT 1) psb ON psb.RowID IS NULL OR psb.RowID IS NOT NULL
    LEFT JOIN (SELECT eb.*,SUM(eb.BonusAmount) AS SumBonus
                    FROM employeebonus eb
                    INNER JOIN payperiod pp ON pp.RowID=psb_PayPeriodID
                    WHERE eb.EmployeeID=EmpRowID
                    AND eb.OrganizationID=OrganizID
                    AND (eb.EffectiveStartDate >= pp.PayFromDate OR eb.EffectiveEndDate >= pp.PayFromDate)
                    AND (eb.EffectiveStartDate <= pp.PayToDate OR eb.EffectiveEndDate <= pp.PayToDate)
    ) eb ON eb.RowID IS NULL OR eb.RowID IS NOT NULL
    WHERE e.RowID=EmpRowID AND e.OrganizationID=OrganizID
 ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=EmpRowID;SELECT @@Identity AS ID INTO returnvalue;



RETURN returnvalue;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
