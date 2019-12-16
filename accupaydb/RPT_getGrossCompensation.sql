/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_getGrossCompensation`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_getGrossCompensation`(IN `OrganizID` INT, IN `LastDateOfFinancialYear` DATE, IN `FirstDateOfFinancialYear` DATE)
    DETERMINISTIC
BEGIN

DECLARE lastDateOfYear DATE;

DECLARE orgworkdaysofyear INT(11);

DECLARE allowance_catID INT(11);

SELECT WorkDaysPerYear FROM organization WHERE RowID=OrganizID INTO orgworkdaysofyear;

SET lastDateOfYear = MAKEDATE(YEAR(LastDateOfFinancialYear), DAYOFYEAR(LAST_DAY(CONCAT(YEAR(CURDATE()),'-12-01'))));

SELECT RowID FROM category WHERE CategoryName='Allowance Type' AND OrganizationID=OrganizID INTO allowance_catID;






SELECT es.*
,IF(e.EmployeeType = 'Fixed', (es.Salary * 12), IF(e.EmployeeType = 'Daily', (es.BasicPay * GET_OrgProRatedCountOfDays(orgworkdaysofyear, es.EffectiveDateFrom, IFNULL(es.EffectiveDateTo,LastDateOfFinancialYear), eal.AllowanceFrequency)), (((es.BasicPay * 8) * orgworkdaysofyear) / 12) * 12)) AS TotalGrossCompensation
,IFNULL((pss.EmployeeContributionAmount * 12),0) AS EmployeeContributionAmount
,(phh.EmployeeShare * 12) AS EmployeeShare
,(es.HDMFAmount) * 12 AS HDMFAmount
    , IFNULL((
    SELECT SUM(IFNULL(tmp.Amount,0))
    FROM v_sumthirteenthmonthpay tmp
    WHERE tmp.OrganizationID=OrganizID AND tmp.EmployeeID=es.EmployeeID AND (GET_paytodatepayperiod(OrganizID,FirstDateOfFinancialYear,e.PayFrequencyID) <= tmp.PayFromDate OR GET_paytodatepayperiod(OrganizID,FirstDateOfFinancialYear,e.PayFrequencyID) <= tmp.PayToDate) AND (GET_paytodatepayperiod(OrganizID,LastDateOfFinancialYear,e.PayFrequencyID) >= tmp.PayFromDate OR GET_paytodatepayperiod(OrganizID,LastDateOfFinancialYear,e.PayFrequencyID) >= tmp.PayToDate)),0) AS ThirteenthMonthPay
,SUM(eal.TotalAllowance) AS TotalAllowance
FROM employeesalary es
LEFT JOIN employee e ON e.RowID=es.EmployeeID AND FIND_IN_SET(e.EmploymentStatus, UNEMPLOYEMENT_STATUSES()) = 0
LEFT JOIN paysocialsecurity pss ON pss.RowID=es.PaySocialSecurityID
LEFT JOIN payphilhealth phh ON phh.RowID=es.PayPhilhealthID
LEFT JOIN (SELECT ea.*
                ,IF(ea.AllowanceFrequency='Daily', ea.AllowanceAmount * GET_OrgProRatedCountOfDays(orgworkdaysofyear, ea.EffectiveStartDate, ea.EffectiveEndDate, ea.AllowanceFrequency), IF(ea.AllowanceFrequency='Semi-monthly', ea.AllowanceAmount * 24, IF(ea.AllowanceFrequency='Monthly', ea.AllowanceAmount * 12, ea.AllowanceAmount))) AS TotalAllowance
                FROM employeeallowance ea
                WHERE OrganizationID=OrganizID
                AND (FirstDateOfFinancialYear <= ea.EffectiveStartDate OR FirstDateOfFinancialYear <= ea.EffectiveEndDate)
                AND (LastDateOfFinancialYear >= ea.EffectiveStartDate OR LastDateOfFinancialYear >= ea.EffectiveEndDate)) eal ON eal.EmployeeID=es.EmployeeID
WHERE es.OrganizationID=OrganizID
AND (FirstDateOfFinancialYear <= es.EffectiveDateFrom OR FirstDateOfFinancialYear <= IFNULL(es.EffectiveDateTo,FirstDateOfFinancialYear))
AND (LastDateOfFinancialYear >= es.EffectiveDateFrom OR LastDateOfFinancialYear >= IFNULL(es.EffectiveDateTo,LastDateOfFinancialYear))
GROUP BY es.EmployeeID;





END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
