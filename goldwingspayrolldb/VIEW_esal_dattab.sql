/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_esal_dattab`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_esal_dattab`(IN `OrganizID` INT, IN `PayPerFrom` DATE, IN `PayPerTo` DATE)
    DETERMINISTIC
BEGIN

SELECT
es.RowID
,es.EmployeeID
,es.Created
,es.CreatedBy
,es.LastUpd
,es.LastUpdBy
,es.OrganizationID
,es.FilingStatusID
,es.PaySocialSecurityID
,es.PayPhilhealthID
,es.HDMFAmount
,IF(e.StartDate BETWEEN PayPerFrom AND PayPerTo, ROUND(DATEDIFF(LAST_DAY(e.StartDate),e.StartDate) / DATEDIFF(LAST_DAY(e.StartDate),DATE_FORMAT(e.StartDate,'%Y-%m-16')) * 13) * GET_employeerateperday(es.EmployeeID, es.OrganizationID, PayPerTo), es.BasicPay) AS BasicPay
,es.Salary
,es.BasicDailyPay
,es.BasicHourlyPay
,es.NoofDependents
,es.MaritalStatus
,es.PositionID
,es.EffectiveDateFrom
,es.EffectiveDateTo
,IFNULL(pss.EmployeeContributionAmount,0.0) AS EmployeeContributionAmount
,IFNULL(pss.EmployerContributionAmount,0.0) AS EmployerContributionAmount
,IFNULL(phh.EmployeeShare,0.0) AS EmployeeShare
,IFNULL(phh.EmployerShare,0.0) AS EmployerShare
FROM employeesalary es
INNER JOIN organization og ON og.RowID=es.OrganizationID
INNER JOIN employee e ON e.RowID=es.EmployeeID
LEFT JOIN paysocialsecurity pss ON pss.RowID=es.PaySocialSecurityID
LEFT JOIN payphilhealth phh ON phh.RowID=es.PayPhilhealthID
WHERE es.OrganizationID=OrganizID
AND es.EffectiveDateTo IS NULL
GROUP BY es.EmployeeID
ORDER BY DATEDIFF(DATE_FORMAT(NOW(),'%Y-%m-%d'),es.EffectiveDateFrom);



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
