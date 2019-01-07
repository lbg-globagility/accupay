/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP VIEW IF EXISTS `latestleaveledger`;
CREATE TABLE `latestleaveledger` (
	`RowID` INT(10) NOT NULL,
	`OrganizationID` INT(10) NULL,
	`Created` TIMESTAMP NOT NULL,
	`CreatedBy` INT(10) NULL,
	`LastUpd` TIMESTAMP NULL,
	`LastUpdBy` INT(10) NULL,
	`EmployeeID` INT(10) NULL,
	`ProductID` INT(10) NULL,
	`LastTransactionID` INT(10) NULL,
	`ReferenceID` INT(10) NULL,
	`PayPeriodID` INT(10) NULL,
	`LeaveLedgerID` INT(10) NULL,
	`TransactionDate` DATE NULL,
	`Type` VARCHAR(50) NULL COLLATE 'latin1_swedish_ci',
	`Balance` DECIMAL(10,2) NULL,
	`Amount` DECIMAL(10,2) NULL,
	`PayFromDate` DATE NULL,
	`PayToDate` DATE NULL
) ENGINE=MyISAM;

DROP VIEW IF EXISTS `latestleaveledger`;
DROP TABLE IF EXISTS `latestleaveledger`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `latestleaveledger` AS SELECT ll.*

, lt.ReferenceID
, lt.PayPeriodID
, lt.LeaveLedgerID
, lt.TransactionDate
, lt.`Type`
, lt.Balance
, lt.Amount

, pp.PayFromDate
, pp.PayToDate
FROM leaveledger ll
INNER JOIN product p ON p.RowID=ll.ProductID
INNER JOIN leavetransaction lt ON lt.LeaveLedgerID=ll.RowID
INNER JOIN payperiod pp ON pp.RowID=lt.PayPeriodID ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
