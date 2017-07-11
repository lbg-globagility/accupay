/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_loans`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_loans`(IN `OrganizID` INT, IN `PayDateFrom` DATE, IN `PayDateTo` DATE, IN `LoanTypeID` INT)
    DETERMINISTIC
BEGIN

DECLARE strloantype TEXT;

SELECT PartNo FROM product WHERE RowID=LoanTypeID INTO strloantype;

SELECT
elh.Comments
,ee.EmployeeID
,CONCAT(ee.LastName,',',ee.FirstName, IF(ee.MiddleName='','',','),INITIALS(ee.MiddleName,'. ','1')) 'Fullname'
,FORMAT(SUM(IFNULL(elh.DeductionAmount,0)),2) 'DeductionAmount'
,els.TotalLoanAmount
,els.TotalBalanceLeft
FROM employeeloanhistory elh
LEFT JOIN paystub ps ON ps.RowID=elh.PayStubID AND ps.OrganizationID=elh.OrganizationID
LEFT JOIN employee ee ON ee.RowID=ps.EmployeeID AND ee.OrganizationID=elh.OrganizationID
INNER JOIN employeeloanschedule els ON els.LoanTypeID=LoanTypeID AND els.OrganizationID=OrganizID AND (els.DedEffectiveDateFrom>=PayDateFrom OR els.DedEffectiveDateTo>=PayDateFrom) AND (els.DedEffectiveDateFrom<=PayDateTo OR els.DedEffectiveDateTo<=PayDateTo)
WHERE elh.DeductionAmount!=0
AND elh.OrganizationID=OrganizID
AND elh.DeductionDate BETWEEN PayDateFrom AND PayDateTo
AND elh.Comments=strloantype
GROUP BY elh.EmployeeID, elh.Comments, els.RowID
ORDER BY elh.Comments,ee.LastName;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
