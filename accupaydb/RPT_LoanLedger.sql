/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `RPT_LoanLedger`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_LoanLedger`()
BEGIN

SET @i = 0;
SET @rowId = 0;
SET @isTrue = FALSE;

SET @notEqual = FALSE;
SET @currBal = 0.00;

SELECT
slp.EmployeeID `COL1`
, p.PartNo `COL2`
, els.TotalLoanAmount `COL3`
, DATE_FORMAT(els.DedEffectiveDateFrom, '%m/%d/%Y') `COL4`
, DATE_FORMAT(pp.PayToDate, '%m/%d/%Y') `COL5`
, slp.DeductionAmount `COL6`
, els.TotalBalanceLeft `COL7`
, CONCAT_WS(', ', e.LastName, e.FirstName) `COL8`

, slp.*
/*
, @isTrue := slp.EmployeeLoanRecordID != @rowId `IsAnotherEmployeeLoanID`
, IF(@isTrue, (@rowId := slp.EmployeeLoanRecordID), (@rowId := slp.EmployeeLoanRecordID)) `EmployeeLoanID`
, IF(@isTrue, (@i := 1), (@i := @i + 1)) `OrdinalIndex`

, IF(@isTrue
#		, (@currBal := (els.TotalLoanAmount - IF(els.NoOfPayPeriod=1, els.DeductionAmount, slp.DeductionAmount)))
		, (@currBal := (els.TotalLoanAmount - slp.DeductionAmount))
		, (@currBal := (@currBal - IF(els.NoOfPayPeriod=1, els.DeductionAmount, slp.DeductionAmount)))
		) `CurrentBalance`*/

FROM scheduledloansperpayperiod slp
INNER JOIN employee e ON e.RowID=slp.EmployeeID #AND e.RowID=5
INNER JOIN employeeloanschedule els ON els.RowID=slp.EmployeeLoanRecordID
INNER JOIN payperiod pp ON pp.RowID=slp.PayPeriodID AND (pp.PayFromDate <= els.DedEffectiveDateTo OR pp.PayToDate <= els.DedEffectiveDateTo)
INNER JOIN product p ON p.RowID=els.LoanTypeID

/*LEFT JOIN (SELECT els.*
		, pp.RowID `PayPeriodID`
		FROM employeeloanschedule els
		INNER JOIN payperiod pp ON pp.OrganizationID=els.OrganizationID AND els.DedEffectiveDateTo BETWEEN pp.PayFromDate AND pp.PayToDate
) i*/

ORDER BY CONCAT_WS('', e.LastName, e.FirstName, e.MiddleName), p.PartNo, pp.`Year`, pp.`Month`, pp.OrdinalValue
;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
