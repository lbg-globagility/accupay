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

-- Dumping structure for function goldwingspayrolldb.INSUPD_employeeloanschedule
DROP FUNCTION IF EXISTS `INSUPD_employeeloanschedule`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `INSUPD_employeeloanschedule`(`els_RowID` INT(11)
, `els_OrganizID` INT(11)
, `els_UserRowID` INT(11)
, `els_EmpNumber` VARCHAR(50)
, `els_LoanNum` VARCHAR(50)
, `els_DateFrom` DATE
, `els_TotLoanAmt` DECIMAL(20,6)
, `els_DeductSched` VARCHAR(50)
, `els_TotBalLeft` DECIMAL(20,6)
, `els_DeductAmt` DECIMAL(11,6)
, `els_Status` VARCHAR(50)
, `els_LoanTypeID` INT(11)
, `els_DeductPerc` DECIMAL(11,6)
, `els_NoOfPayPer` INT(11)
, `els_LoanPayPerLeft` INT(11)
, `els_Comments` VARCHAR(2000)
, `els_BonusID` INT(11)
, `els_LoanName` VARCHAR(50)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11);

INSERT INTO employeeloanschedule
(
	RowID
	,OrganizationID
	,Created
	,CreatedBy
	,EmployeeID
	,LoanNumber
	,DedEffectiveDateFrom
	,DedEffectiveDateTo
	,TotalLoanAmount
	,DeductionSchedule
	,TotalBalanceLeft
	,DeductionAmount
	,`Status`
	,LoanTypeID
	,DeductionPercentage
	,NoOfPayPeriod
	,LoanPayPeriodLeft
	,Comments
	,BonusID
	,LoanName
) SELECT
	IFNULL(els_RowID,els.RowID)
	,els_OrganizID
	,CURRENT_TIMESTAMP()
	,els_UserRowID
	,e.RowID
	,TRIM(els_LoanNum)
	,els_DateFrom
	,PAYTODATE_OF_NoOfPayPeriod(els_DateFrom,els_NoOfPayPer,e.RowID,els_DeductSched)
	,els_TotLoanAmt
	,els_DeductSched
	,els_TotBalLeft
	,els_DeductAmt
	,els_Status
	,NULL
	,els_DeductPerc
	,els_NoOfPayPer
	,(els_TotBalLeft / els_TotLoanAmt) *  els_NoOfPayPer
	,TRIM(els_Comments)
	,els_BonusID
	,TRIM(els_LoanName)
	FROM employee e
	LEFT JOIN employeeloanschedule els ON els.EmployeeID=e.RowID AND els.OrganizationID=e.OrganizationID AND els.LoanName=TRIM(els_LoanName) AND els.DedEffectiveDateFrom=els_DateFrom AND els.`Status`=els_Status AND els.BonusID=0
	WHERE e.EmployeeID=TRIM(els_EmpNumber) AND e.OrganizationID=els_OrganizID AND LENGTH(TRIM(els_LoanName)) > 0
	LIMIT 1
ON
DUPLICATE
KEY
UPDATE
	LastUpd=CURRENT_TIMESTAMP()
	,LastUpdBy=els_UserRowID;SELECT @@Identity AS ID INTO returnvalue;

RETURN returnvalue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
