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

DECLARE ppYear, ppMonth INT(11);

DECLARE isFirstHalf TINYINT(1) DEFAULT 0;

SET SESSION group_concat_max_len = 102400;

SELECT pp.`Year`
, pp.`Month`
, (pp.Half = 1) `isFirstHalf`
FROM payperiod pp
WHERE pp.RowID=payperiod_rowid
INTO ppYear, ppMonth, isFirstHalf
;

SELECT
GROUP_CONCAT(lt.RowID)
FROM (SELECT
		lt.EmployeeID
		, lt.LeaveLedgerID
		, MAX(ppd.OrdinalValue) `MaxOrdinalValue`
		FROM payperiod pp
		INNER JOIN leavetransaction lt ON lt.OrganizationID=organiz_rowid AND lt.LeaveLedgerID IS NOT NULL
		INNER JOIN payperiod ppd ON ppd.RowID=lt.PayPeriodID AND ppd.OrdinalValue <= pp.OrdinalValue
		WHERE pp.`Year`=ppYear
		AND pp.`Month`=ppMonth
		AND pp.Half=isFirstHalf
		AND pp.TotalGrossSalary=1
		AND pp.OrganizationID=lt.OrganizationID
		GROUP BY lt.EmployeeID, lt.LeaveLedgerID) i
INNER JOIN payperiod pp ON pp.`Year`=ppYear AND pp.OrdinalValue=i.MaxOrdinalValue
INNER JOIN leavetransaction lt ON lt.LeaveLedgerID=i.LeaveLedgerID AND lt.PayPeriodID=pp.RowID
INNER JOIN leaveledger ll ON ll.LastTransactionID=lt.RowID AND ll.RowID=i.LeaveLedgerID
INNER JOIN employee e ON e.RowID=ll.EmployeeID
INNER JOIN product p ON p.RowID=ll.ProductID AND p.PartNo IN ('Sick leave', 'Vacation leave')
INTO return_value
;

SET return_value = IFNULL(return_value, '');

RETURN return_value;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
