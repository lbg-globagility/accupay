/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_13thmonthpayDetailed`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_13thmonthpayDetailed`(IN `OrganizID` INT, IN `pay_date_from` DATE, IN `pay_date_to` DATE)
    DETERMINISTIC
BEGIN

DECLARE custom_dateformat VARCHAR(50) DEFAULT '%c/%e/%Y';

DECLARE month_count_peryear INT(11) DEFAULT 12;

DECLARE ecola_rowid INT(11);

SELECT p.RowID
FROM product p
WHERE p.OrganizationID=OrganizID
AND p.PartNo='Ecola'
LIMIT 1
INTO ecola_rowid
;

SELECT
e.EmployeeID `DatCol1`
,CONCAT_WS(', ', e.LastName, e.FirstName) `DatCol2`

,CONCAT_WS(' - '
           , DATE_FORMAT(pyp.PayFromDate, custom_dateformat)
           , DATE_FORMAT(pyp.PayToDate, custom_dateformat)) `DatCol3`

,ROUND(ttmp.BasicPay, 2) `DatCol4`
,ttmp.Amount `DatCol5`

,e.EmployeeType `DatCol6`
,esa.BasicPay `DatCol7`
,IFNULL(ea.AllowanceAmount, 0) `DatCol8`

FROM thirteenthmonthpay ttmp
INNER JOIN paystub ps
        ON ps.RowID=ttmp.PaystubID

INNER JOIN employee e
        ON e.RowID=ps.EmployeeID
		     AND e.OrganizationID=ttmp.OrganizationID

INNER JOIN employeesalary esa
        ON esa.EmployeeID=e.RowID
           AND (esa.EffectiveDateFrom >= pay_date_from OR IFNULL(esa.EffectiveDateTo, ADDDATE(esa.EffectiveDateFrom, INTERVAL 99 YEAR)) >= pay_date_from)
           AND (esa.EffectiveDateFrom <= pay_date_to OR IFNULL(esa.EffectiveDateTo, ADDDATE(esa.EffectiveDateFrom, INTERVAL 99 YEAR)) <= pay_date_to)

INNER JOIN payperiod pyp
        ON pyp.RowID=ps.PayPeriodID
		     AND pyp.OrganizationID=ttmp.OrganizationID
		     AND (pyp.PayFromDate >= pay_date_from OR pyp.PayToDate >= pay_date_from)
		     AND (pyp.PayFromDate <= pay_date_to OR pyp.PayToDate <= pay_date_to)
		     
LEFT JOIN employeeallowance ea
       ON ea.EmployeeID=e.RowID
          AND ea.OrganizationID=e.OrganizationID
          AND (ea.EffectiveStartDate >= pay_date_from OR ea.EffectiveEndDate >= pay_date_from)
          AND (ea.EffectiveStartDate <= pay_date_to OR ea.EffectiveEndDate <= pay_date_to)
          AND ea.ProductID=ecola_rowid

WHERE ttmp.OrganizationID=OrganizID
GROUP BY ps.RowID
ORDER BY CONCAT(e.LastName, e.FirstName), pyp.OrdinalValue
;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
