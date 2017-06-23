-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.11-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.0.0.4396
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for trigger goldwingspayrolldb.AFTINS_employeeloanhistory
DROP TRIGGER IF EXISTS `AFTINS_employeeloanhistory`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_employeeloanhistory` AFTER INSERT ON `employeeloanhistory` FOR EACH ROW BEGIN

INSERT INTO employeeloanhistoitem
(
	OrganizationID
	,CreatedBy
	,Created
	,LoanHistoID
	,EmpLoanID
	,LoanTypeID
	,PaidLoanAmount
) SELECT NEW.OrganizationID
	,NEW.CreatedBy
	,CURRENT_TIMESTAMP()
	,NEW.RowID
	,els.RowID
	,els.LoanTypeID
	,els.TotalLoanAmount - (els.TotalBalanceLeft - els.DeductionAmount)
	FROM employeeloanschedule els
	INNER JOIN product p ON p.PartNo=NEW.Comments AND p.`Category`='Loan Type'
	WHERE els.OrganizationID=NEW.OrganizationID
	AND els.EmployeeID=NEW.EmployeeID
	AND els.LoanTypeID=p.RowID
	AND NEW.DeductionDate BETWEEN els.DedEffectiveDateFrom AND els.DedEffectiveDateTo;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
