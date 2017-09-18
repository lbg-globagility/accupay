/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `PAYSTUB_prepare_loans`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `PAYSTUB_prepare_loans`(
	IN `OrganizID` INT,
	IN `Date_From` DATE,
	IN `Date_To` DATE,
	IN `ppRowID` INT
)
    DETERMINISTIC
BEGIN

DECLARE IsFirstHalfOfMonth CHAR(1) DEFAULT 0;

DECLARE payfreqID INT(11);

SELECT
    pp.`Half`,
    pp.TotalGrossSalary
FROM payperiod pp
INNER JOIN payperiod p2
ON p2.RowID=ppRowID
    AND pp.TotalGrossSalary=p2.TotalGrossSalary
WHERE pp.OrganizationID=OrganizID
    AND pp.PayFromDate=Date_From
    AND pp.PayToDate=Date_To
LIMIT 1
INTO
    IsFirstHalfOfMonth,
    payfreqID;

IF IsFirstHalfOfMonth = '1' THEN

    SELECT
        SUM((IFNULL(els.TotalLoanAmount,0) - IFNULL(els.TotalBalanceLeft,0))) `TotalLoanAmount`,
        SUM(els.DeductionAmount) `DeductionAmount`,
        els.EmployeeID
    FROM employeeloanschedule els
    INNER JOIN employee e
    ON e.RowID=els.EmployeeID
        AND e.OrganizationID=els.OrganizationID
        AND e.PayFrequencyID=payfreqID
    WHERE els.OrganizationID=OrganizID AND els.BonusID IS NULL
        AND (els.`Status`!='Cancelled' AND els.`Status` != 'On Hold')
        AND els.DeductionSchedule IN ('First half', 'Per pay period')
        AND (Date_To >= els.DedEffectiveDateFrom)
        AND els.TotalBalanceLeft > 0
        
        
    GROUP BY els.EmployeeID;

ELSE

    SELECT
        SUM((IFNULL(els.TotalLoanAmount, 0) - IFNULL(els.TotalBalanceLeft, 0))) `TotalLoanAmount`,
        SUM(els.DeductionAmount) `DeductionAmount`,
        els.EmployeeID
    FROM employeeloanschedule els
    INNER JOIN employee e
    ON e.RowID=els.EmployeeID
        AND e.OrganizationID=els.OrganizationID
        AND e.PayFrequencyID=payfreqID
    WHERE els.OrganizationID=OrganizID
        AND els.BonusID IS NULL
        AND (els.`Status`!='Cancelled' AND els.`Status` != 'On Hold')
        AND els.DeductionSchedule IN ('End of the month', 'Per pay period')
        AND (Date_To >= els.DedEffectiveDateFrom)
        AND els.TotalBalanceLeft > 0
        
        
    GROUP BY els.EmployeeID;

END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
