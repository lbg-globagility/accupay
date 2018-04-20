-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.12-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for function accupaydb_cinema2k.LeaveTransactionRowIdsWithinCutOff
DROP FUNCTION IF EXISTS `LeaveTransactionRowIdsWithinCutOff`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `LeaveTransactionRowIdsWithinCutOff`(`organiz_rowid` INT, `payperiod_rowid` INT) RETURNS tinytext CHARSET latin1
    DETERMINISTIC
BEGIN

DECLARE return_value TINYTEXT;

SET SESSION group_concat_max_len = 102400;

SELECT GROUP_CONCAT(i.`RowID`) `Result`
FROM (SELECT
		MAX(lt.RowID) `RowID`
				
		, e.EmployeeID
		, CONCAT_WS(', ', e.LastName, e.FirstName) `FullName`
		
		, lt.Balance
		, lt.Amount
		
		, pp.PayFromDate
		, pp.PayToDate
		
		, prd.PartNo
		
		, ppd.OrdinalValue
		# , pp.OrdinalValue
		# , MAX(pp.OrdinalValue) `OrdinalValue`
		
		, lt.PayPeriodID

		FROM leavetransaction lt
		
		INNER JOIN employee e ON e.RowID=lt.EmployeeID
		INNER JOIN payperiod ppd ON ppd.RowID=payperiod_rowid
		INNER JOIN payperiod pp ON pp.RowID=lt.PayPeriodID AND pp.`Year`=ppd.`Year` AND ppd.OrdinalValue >= pp.OrdinalValue
		
		INNER JOIN leaveledger ll ON ll.RowID=lt.LeaveLedgerID
		INNER JOIN product prd ON prd.RowID=ll.ProductID AND prd.PartNo IN ('Vacation leave', 'Sick leave')
		
		WHERE lt.OrganizationID = organiz_rowid
		GROUP BY e.RowID
		HAVING MAX(pp.OrdinalValue) <= ppd.OrdinalValue
		ORDER BY CONCAT(e.LastName, e.FirstName), pp.OrdinalValue
		) i
INTO return_value
;

SET return_value = IFNULL(return_value, '');

RETURN return_value;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
