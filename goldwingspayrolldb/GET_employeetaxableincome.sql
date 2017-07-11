/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `GET_employeetaxableincome`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GET_employeetaxableincome`(`ps_EmployeeID` INT, `ps_OrganizID` INT, `ps_Date` DATE, `addvalue` DECIMAL(12,2)) RETURNS text CHARSET latin1
    DETERMINISTIC
BEGIN

DECLARE returnvalue DECIMAL(12,2);

DECLARE month_year TEXT;

DECLARE wtax_value DECIMAL(12,2);

DECLARE fstat_ID INT(11);

DECLARE payfreqID_conversion INT(11);

SET month_year = DATE_FORMAT(ps_Date,'%m%Y');



SELECT ps.TotalTaxableSalary
FROM paystub ps
LEFT JOIN payperiod pyp ON ps.PayPeriodID=pyp.RowID
WHERE DATE_FORMAT(pyp.PayToDate,'%m%Y')=month_year
AND ps.EmployeeID=ps_EmployeeID
AND ps.OrganizationID=ps_OrganizID
ORDER BY ps.PayPeriodID
LIMIT 1
INTO returnvalue;

SET returnvalue = IFNULL(returnvalue,0) + addvalue;


SELECT fs.RowID
,e.PayFrequencyID
FROM filingstatus fs
LEFT JOIN employee e ON e.MaritalStatus=fs.MaritalStatus AND e.NoOfDependents=fs.Dependent
WHERE e.RowID=ps_EmployeeID
LIMIT 1
INTO fstat_ID
      ,payfreqID_conversion;

IF payfreqID_conversion = 1 THEN

    SET payfreqID_conversion = 2;

ELSE

    SET payfreqID_conversion = 2;

END IF;

SELECT
((returnvalue - tx.TaxableIncomeFromAmount) * tx.ExemptionInExcessAmount) + tx.ExemptionAmount
FROM paywithholdingtax tx
WHERE tx.FilingStatusID=fstat_ID
AND returnvalue BETWEEN tx.TaxableIncomeFromAmount AND tx.TaxableIncomeToAmount
AND DATEDIFF(CURRENT_DATE(), COALESCE(tx.EffectiveDateTo, COALESCE(tx.EffectiveDateFrom, CURRENT_DATE()))) >= 0
AND tx.PayFrequencyID=payfreqID_conversion
ORDER BY DATEDIFF(CURRENT_DATE(), COALESCE(tx.EffectiveDateTo, COALESCE(tx.EffectiveDateFrom, CURRENT_DATE())))
LIMIT 1
INTO wtax_value;

SET wtax_value = IFNULL(wtax_value,0);



RETURN CONCAT(returnvalue,';',wtax_value);

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
