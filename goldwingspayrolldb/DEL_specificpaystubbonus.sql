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

-- Dumping structure for procedure goldwingspayrolldb.DEL_specificpaystubbonus
DROP PROCEDURE IF EXISTS `DEL_specificpaystubbonus`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `DEL_specificpaystubbonus`(IN `paystubbonus_RowID` INT)
    DETERMINISTIC
BEGIN

DECLARE IsFirstHalfOfMonth CHAR(1);

DECLARE function_wrapper CHAR(1);

DECLARE OrganizID INT(11);

DECLARE EmpRowID INT(11);

DECLARE _PayFromDate DATE;

DECLARE _PayToDate DATE;

SELECT pp.`Half`,pp.PayFromDate,pp.PayToDate,pp.OrganizationID,psb.EmployeeID FROM payperiod pp INNER JOIN paystubbonus psb ON psb.RowID=paystubbonus_RowID AND psb.PayPeriodID=pp.RowID INTO IsFirstHalfOfMonth,_PayFromDate,_PayToDate,OrganizID,EmpRowID;

IF IsFirstHalfOfMonth = '1' THEN
	
	UPDATE employeeloanschedule els
	INNER JOIN (SELECT *
					FROM employeebonus
					WHERE EmployeeID=EmpRowID
					AND OrganizationID=OrganizID
					AND (EffectiveStartDate >= _PayFromDate OR EffectiveEndDate >= _PayFromDate)
					AND (EffectiveStartDate <= _PayToDate OR EffectiveEndDate <= _PayToDate)
	) eb ON eb.RowID = els.BonusID
	SET
	LoanPayPeriodLeft = IF((LoanPayPeriodLeft + 1) < 0, 0, (LoanPayPeriodLeft + 1))
	,TotalBalanceLeft = TotalBalanceLeft + DeductionAmount
	WHERE els.OrganizationID=OrganizID
	
	
	AND els.EmployeeID=EmpRowID
	AND els.DeductionSchedule IN ('First half','Per pay period')
	AND (els.DedEffectiveDateFrom >= _PayFromDate OR els.DedEffectiveDateTo >= _PayFromDate)
	AND (els.DedEffectiveDateFrom <= _PayToDate OR els.DedEffectiveDateTo <= _PayToDate);
	
ELSE

	UPDATE employeeloanschedule els
	INNER JOIN (SELECT *
					FROM employeebonus
					WHERE EmployeeID=EmpRowID
					AND OrganizationID=OrganizID
					AND (EffectiveStartDate >= _PayFromDate OR EffectiveEndDate >= _PayFromDate)
					AND (EffectiveStartDate <= _PayToDate OR EffectiveEndDate <= _PayToDate)
	) eb ON eb.RowID = els.BonusID
	SET
	LoanPayPeriodLeft = IF((LoanPayPeriodLeft + 1) < 0, 0, (LoanPayPeriodLeft + 1))
	,TotalBalanceLeft = TotalBalanceLeft + DeductionAmount
	WHERE els.OrganizationID=OrganizID
	
	
	AND els.EmployeeID=EmpRowID
	AND DeductionSchedule IN ('End of the month','Per pay period')
	AND (els.DedEffectiveDateFrom >= _PayFromDate OR els.DedEffectiveDateTo >= _PayFromDate)
	AND (els.DedEffectiveDateFrom <= _PayToDate OR els.DedEffectiveDateTo <= _PayToDate);
	
END IF;

DELETE FROM paystubitembonus WHERE PayStubBonusID=paystubbonus_RowID;
ALTER TABLE paystubitembonus AUTO_INCREMENT = 0;

DELETE FROM paystubbonus WHERE RowID=paystubbonus_RowID;
ALTER TABLE paystubbonus AUTO_INCREMENT = 0;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
