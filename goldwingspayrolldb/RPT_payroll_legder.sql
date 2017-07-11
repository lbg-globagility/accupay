/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_payroll_legder`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_payroll_legder`(IN `OrganizID` INT, IN `PayPerID1` INT, IN `PayPerID2` INT, IN `psi_undeclared` CHAR(1))
    DETERMINISTIC
BEGIN

DECLARE PayP_Date1 DATE;

DECLARE PayP_Date2 DATE;

SELECT PayFromDate FROM payperiod WHERE RowID=PayPerID1 INTO PayP_Date1;

SELECT PayToDate FROM payperiod WHERE RowID=PayPerID2 INTO PayP_Date2;

SELECT
ee.RowID
,ee.EmployeeID
,CONCAT(ee.LastName,',',ee.FirstName, IF(ee.MiddleName='','',','),INITIALS(ee.MiddleName,'. ','1')) 'Fullname'
,DATE_FORMAT(ps.PayFromDate,'%m/%e/%Y') 'PayFromDate'
,DATE_FORMAT(ps.PayToDate,'%m/%e/%Y') 'PayToDate'
,(SELECT SUM((IFNULL(TotalDayPay,0)) - (IFNULL(OvertimeHoursAmount,0) + IFNULL(NightDiffHoursAmount,0) + IFNULL(NightDiffOTHoursAmount,0))) FROM employeetimeentry WHERE OrganizationID=OrganizID AND EmployeeID=ps.EmployeeID AND `Date` BETWEEN PayP_Date1 AND PayP_Date2) * IF(psi_undeclared = '0', 1, (GET_employeeundeclaredsalarypercent(ps.EmployeeID,OrganizID,ps.PayFromDate,ps.PayToDate) + 1)) 'BasicPay'
,SUM(IFNULL(pst6.PayAmount,0)) 'TotalGrossSalary'
,SUM(IFNULL(pst7.PayAmount,0)) 'TotalNetSalary'
,SUM(IFNULL(pst8.PayAmount,0)) 'TotalTaxableSalary'
,ps.TotalEmpSSS 'TotalEmpSSS'
,SUM(IFNULL(pst9.PayAmount,0)) 'TotalEmpWithholdingTax'
,SUM(IFNULL(ps.TotalCompSSS,0)) 'TotalCompSSS'
,SUM(IFNULL(ps.TotalEmpPhilhealth,0)) 'TotalEmpPhilhealth'
,SUM(IFNULL(ps.TotalCompPhilhealth,0)) 'TotalCompPhilhealth'
,SUM(IFNULL(ps.TotalEmpHDMF,0)) 'TotalEmpHDMF'
,SUM(IFNULL(ps.TotalCompHDMF,0)) 'TotalCompHDMF'
,SUM(IFNULL(ps.TotalVacationDaysLeft,0)) 'TotalVacationDaysLeft'
,SUM(IFNULL(ps.TotalLoans,0)) 'TotalLoans'
,SUM(IFNULL(ps.TotalBonus,0)) 'TotalBonus'
,SUM(IFNULL(ps.TotalAllowance,0)) 'TotalAllowance'
,SUM(IFNULL(pst.PayAmount,0)) AS Tardiness
,SUM(IFNULL(pst1.PayAmount,0)) AS Undertime
,SUM(IFNULL(pst2.PayAmount,0)) AS NightDifftl
,SUM(IFNULL(pst3.PayAmount,0)) AS HolidayPay
,SUM(IFNULL(pst4.PayAmount,0)) AS OverTime
,SUM(IFNULL(pst5.PayAmount,0)) AS NightDifftlOT
FROM paystub ps
LEFT JOIN employee ee ON ee.RowID=ps.EmployeeID AND ee.OrganizationID=ps.OrganizationID

INNER JOIN product pd ON pd.OrganizationID=OrganizID AND pd.PartNo='Tardiness'
LEFT JOIN paystubitem pst ON pst.PayStubID=ps.RowID AND pst.ProductID=pd.RowID AND pst.`Undeclared`=psi_undeclared

INNER JOIN product pd1 ON pd1.OrganizationID=OrganizID AND pd1.PartNo='Undertime'
LEFT JOIN paystubitem pst1 ON pst1.PayStubID=ps.RowID AND pst1.ProductID=pd1.RowID AND pst1.`Undeclared`=psi_undeclared

INNER JOIN product pd2 ON pd2.OrganizationID=OrganizID AND pd2.PartNo='Night differential'
LEFT JOIN paystubitem pst2 ON pst2.PayStubID=ps.RowID AND pst2.ProductID=pd2.RowID AND pst2.`Undeclared`=psi_undeclared

INNER JOIN product pd3 ON pd3.OrganizationID=OrganizID AND pd3.PartNo='Holiday pay'
LEFT JOIN paystubitem pst3 ON pst3.PayStubID=ps.RowID AND pst3.ProductID=pd3.RowID AND pst3.`Undeclared`=psi_undeclared

INNER JOIN product pd4 ON pd4.OrganizationID=OrganizID AND pd4.PartNo='Overtime'
LEFT JOIN paystubitem pst4 ON pst4.PayStubID=ps.RowID AND pst4.ProductID=pd4.RowID AND pst4.`Undeclared`=psi_undeclared

INNER JOIN product pd5 ON pd5.OrganizationID=OrganizID AND pd5.PartNo='Night differential OT'
LEFT JOIN paystubitem pst5 ON pst5.PayStubID=ps.RowID AND pst5.ProductID=pd5.RowID AND pst5.`Undeclared`=psi_undeclared

INNER JOIN product pd6 ON pd6.OrganizationID=OrganizID AND pd6.PartNo='Gross Income'
LEFT JOIN paystubitem pst6 ON pst6.PayStubID=ps.RowID AND pst6.ProductID=pd6.RowID AND pst6.`Undeclared`=psi_undeclared

INNER JOIN product pd7 ON pd7.OrganizationID=OrganizID AND pd7.PartNo='Net Income'
LEFT JOIN paystubitem pst7 ON pst7.PayStubID=ps.RowID AND pst7.ProductID=pd7.RowID AND pst7.`Undeclared`=psi_undeclared

INNER JOIN product pd8 ON pd8.OrganizationID=OrganizID AND pd8.PartNo='Taxable Income'
LEFT JOIN paystubitem pst8 ON pst8.PayStubID=ps.RowID AND pst8.ProductID=pd8.RowID AND pst8.`Undeclared`=psi_undeclared

INNER JOIN product pd9 ON pd9.OrganizationID=OrganizID AND pd9.PartNo='Withholding Tax'
LEFT JOIN paystubitem pst9 ON pst9.PayStubID=ps.RowID AND pst9.ProductID=pd9.RowID AND pst9.`Undeclared`=psi_undeclared

WHERE ps.OrganizationID=OrganizID
AND ps.PayPeriodID BETWEEN PayPerID1 AND PayPerID2
GROUP BY ps.RowID
ORDER BY ee.LastName,ps.PayToDate;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
