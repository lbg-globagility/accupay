/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_AnnualizedWithholdingTax`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_AnnualizedWithholdingTax`(IN `OrganizID` INT, IN `AnnualDateFrom` DATE, IN `AnnualDateTo` DATE, IN `IsActual` CHAR(1))
    DETERMINISTIC
BEGIN

DECLARE allowance_catID INT(11);

DECLARE bonus_catID INT(11);

DECLARE lastDateOfYear DATE;

DECLARE orgworkdaysofyear INT(11);

SELECT RowID FROM category WHERE CategoryName='Allowance Type' AND OrganizationID=OrganizID INTO allowance_catID;

SELECT RowID FROM category WHERE CategoryName='Bonus' AND OrganizationID=OrganizID INTO bonus_catID;

SELECT WorkDaysPerYear FROM organization WHERE RowID=OrganizID INTO orgworkdaysofyear;

SET lastDateOfYear = LAST_DAY(CONCAT(YEAR(CURDATE()),'-12-01'));



SELECT
ps.RowID
,ps.OrganizationID
,ps.Created
,ps.CreatedBy
,ps.LastUpd
,ps.LastUpdBy
,ps.PayPeriodID
,ps.EmployeeID
,ps.TimeEntryID
,ps.PayFromDate
,ps.PayToDate
,SUM(IFNULL(pst5.PayAmount,0)) AS TotalGrossSalary
,SUM(IFNULL(pst6.PayAmount,0)) AS TotalNetSalary
,SUM(IFNULL(pst7.PayAmount,0)) AS TotalTaxableSalary
,ps.TotalEmpSSS
,ps.TotalEmpWithholdingTax
,ps.TotalCompSSS
,ps.TotalEmpPhilhealth
,ps.TotalCompPhilhealth
,ps.TotalEmpHDMF
,ps.TotalCompHDMF
,ps.TotalVacationDaysLeft
,ps.TotalUndeclaredSalary
,ps.TotalLoans
,ps.TotalBonus
,ps.TotalAllowance
,ps.TotalAdjustments
,IFNULL((SELECT SUM(PayAmount) FROM v_sumpsitaxableallowance WHERE PayStubID=ps.RowID AND Taxable='0' AND CategoryID=allowance_catID),0) AS AllowanceNoTax
,IFNULL((SELECT SUM(PayAmount) FROM v_sumpsitaxableallowance WHERE PayStubID=ps.RowID AND Taxable='1' AND CategoryID=allowance_catID),0) AS AllowanceYesTax
,50000 AS PersonalExemption
,(e.NoOfDependents * 25000) AS AdditionalPersonalExemption
,30000 AS DeMinimisExemption
,SUM(IFNULL(pst.PayAmount,0)) AS HolidayPay
,SUM(IFNULL(pst1.PayAmount,0)) AS OvertimePay
,SUM(IFNULL(pst1.PayAmount,0)) AS TotalOverTime
,SUM(IFNULL(pst2.PayAmount,0)) AS NightDiffPay
,SUM(IFNULL(pst4.PayAmount,0)) AS NightDiffOT
,IFNULL((SELECT SUM(ete.TotalDayPay) FROM employeetimeentry ete WHERE ete.OrganizationID=OrganizID AND ete.EmployeeID=ps.EmployeeID AND ete.`Date` BETWEEN ps.PayFromDate AND ps.PayToDate) * IF(IsActual = '0', 1, (GET_employeeundeclaredsalarypercent(ps.EmployeeID,OrganizID,ps.PayFromDate,ps.PayToDate) + 1)),0) AS TotalDayPay
,GET_employeeundeclaredsalarypercent(ps.EmployeeID,OrganizID,ps.PayFromDate,ps.PayToDate) AS employeeundeclaredsalarypercent
FROM paystub ps
INNER JOIN employee e ON e.RowID=ps.EmployeeID









INNER JOIN product pd ON pd.OrganizationID=OrganizID AND pd.PartNo='Holiday pay'
LEFT JOIN paystubitem pst ON pst.PayStubID=ps.RowID AND pst.ProductID=pd.RowID AND pst.Undeclared=IsActual

INNER JOIN product pd1 ON pd1.OrganizationID=OrganizID AND pd1.PartNo='Overtime'
LEFT JOIN paystubitem pst1 ON pst1.PayStubID=ps.RowID AND pst1.ProductID=pd1.RowID AND pst1.Undeclared=IsActual

INNER JOIN product pd2 ON pd2.OrganizationID=OrganizID AND pd2.PartNo='Night differential'
LEFT JOIN paystubitem pst2 ON pst2.PayStubID=ps.RowID AND pst2.ProductID=pd2.RowID AND pst2.Undeclared=IsActual

INNER JOIN product pd3 ON pd3.OrganizationID=OrganizID AND pd3.PartNo='Undertime'
LEFT JOIN paystubitem pst3 ON pst3.PayStubID=ps.RowID AND pst3.ProductID=pd3.RowID AND pst3.`Undeclared`=IsActual

INNER JOIN product pd4 ON pd4.OrganizationID=OrganizID AND pd4.PartNo='Night differential OT'
LEFT JOIN paystubitem pst4 ON pst4.PayStubID=ps.RowID AND pst4.ProductID=pd4.RowID AND pst4.`Undeclared`=IsActual

INNER JOIN product pd5 ON pd5.OrganizationID=OrganizID AND pd5.PartNo='Gross Income'
LEFT JOIN paystubitem pst5 ON pst5.PayStubID=ps.RowID AND pst5.ProductID=pd5.RowID AND pst5.`Undeclared`=IsActual

INNER JOIN product pd6 ON pd6.OrganizationID=OrganizID AND pd6.PartNo='Net Income'
LEFT JOIN paystubitem pst6 ON pst6.PayStubID=ps.RowID AND pst6.ProductID=pd6.RowID AND pst6.`Undeclared`=IsActual

INNER JOIN product pd7 ON pd7.OrganizationID=OrganizID AND pd7.PartNo='Taxable Income'
LEFT JOIN paystubitem pst7 ON pst7.PayStubID=ps.RowID AND pst7.ProductID=pd7.RowID AND pst7.`Undeclared`=IsActual

INNER JOIN product pd8 ON pd8.OrganizationID=OrganizID AND pd8.PartNo='Withholding Tax'
LEFT JOIN paystubitem pst8 ON pst8.PayStubID=ps.RowID AND pst8.ProductID=pd8.RowID AND pst8.`Undeclared`=IsActual

WHERE ps.OrganizationID=OrganizID
AND (AnnualDateFrom <= ps.PayFromDate OR AnnualDateFrom <= ps.PayToDate)
AND (AnnualDateTo >= ps.PayFromDate OR AnnualDateTo >= ps.PayToDate)
GROUP BY ps.RowID;




END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
