/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_payperiodofyear`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_payperiodofyear`(IN `payp_OrganizationID` INT, IN `param_Date` DATE, IN `FormatNumber` INT)
    DETERMINISTIC
BEGIN
SET param_Date = IFNULL(STR_TO_DATE(param_Date,@@date_format),CURDATE());
SELECT payp.RowID AS ppRowID
,DATE_FORMAT(payp.PayFromDate,'%c/%e/%Y') AS `Pay from date`
,DATE_FORMAT(payp.PayToDate,'%c/%e/%Y') AS `Pay to date`
,payp.PayFromDate
,payp.PayToDate
,COALESCE(payp.TotalGrossSalary,0) 'TotalGrossSalary'
,COALESCE(payp.TotalNetSalary,0) 'TotalNetSalary'
,COALESCE(payp.TotalEmpSSS,0) 'TotalEmpSSS'
,COALESCE(payp.TotalEmpWithholdingTax,0) 'TotalEmpWithholdingTax'
,COALESCE(payp.TotalCompSSS,0) 'TotalCompSSS'
,COALESCE(payp.TotalEmpPhilhealth,0) 'TotalEmpPhilhealth'
,COALESCE(payp.TotalCompPhilhealth,0) 'TotalCompPhilhealth'
,COALESCE(payp.TotalEmpHDMF,0) 'TotalEmpHDMF'
# ,IF(payp.TotalGrossSalary = 4, 'WEEKLY', 'SEMI-MONTHLY') 'TotalCompHDMF'
, pf.PayFrequencyType `TotalCompHDMF`
,IF(DATE_FORMAT(NOW(),'%Y-%m-%d') BETWEEN payp.PayFromDate AND payp.PayToDate,'0',IF(DATE_FORMAT(NOW(),'%Y-%m-%d') > payp.PayFromDate,'-1','1')) 'now_origin'
,payp.Half AS eom
,payp.IsClosed

FROM payperiod payp
INNER JOIN (    SELECT ps.RowID, ps.PayPeriodID, FormatNumber AS FormatNum, e.PayFrequencyID FROM paystub ps
                INNER JOIN employee e ON e.RowID=ps.EmployeeID
				    WHERE FormatNumber = 0 AND ps.OrganizationID=payp_OrganizationID
            UNION
                SELECT ps.RowID, ps.PayPeriodID, FormatNumber AS FormatNum, e.PayFrequencyID FROM paystubbonus ps
                INNER JOIN employee e ON e.RowID=ps.EmployeeID
                WHERE FormatNumber = 1 AND ps.OrganizationID=payp_OrganizationID
                ) payst ON payst.PayPeriodID=payp.RowID
INNER JOIN payfrequency pf ON pf.RowID=payp.TotalGrossSalary
WHERE payp.OrganizationID=payp_OrganizationID
AND payp.`Year`=YEAR(param_Date)
AND payp.TotalGrossSalary = payst.PayFrequencyID
GROUP BY payst.PayPeriodID
ORDER BY payp.PayFromDate DESC,payp.PayToDate DESC;









END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
