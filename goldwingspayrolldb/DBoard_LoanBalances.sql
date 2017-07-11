/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `DBoard_LoanBalances`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `DBoard_LoanBalances`(IN `OrganizID` INT)
    DETERMINISTIC
BEGIN

SELECT
e.EmployeeID
,CONCAT(e.LastName,',',e.FirstName,',',INITIALS(e.MiddleName,'.','1')) 'Employee Fullname'
,els.`Status`
,DATE_FORMAT(els.DedEffectiveDateFrom,'%m-%e-%Y') AS DedEffectiveDateFrom
,DATE_FORMAT(els.DedEffectiveDateTo,'%m-%e-%Y') AS DedEffectiveDateTo
,FORMAT(els.TotalBalanceLeft,2) AS TotalBalanceLeft
,SUBSTRING_INDEX(els.LoanPayPeriodLeft,'.',1) AS LoanPayPeriodLeft
FROM employeeloanschedule els
INNER JOIN employee e ON e.RowID=els.EmployeeID
INNER JOIN (SELECT *,(TotalLoanAmount - (DeductionAmount * NoOfPayPeriod)) AS Butal FROM employeeloanschedule) eels ON eels.RowID=els.RowID
WHERE els.LoanPayPeriodLeft != 0
AND els.TotalBalanceLeft > 0
AND els.OrganizationID=OrganizID;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
