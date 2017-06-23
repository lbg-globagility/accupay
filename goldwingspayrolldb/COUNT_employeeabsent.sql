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

-- Dumping structure for function goldwingspayrolldb.COUNT_employeeabsent
DROP FUNCTION IF EXISTS `COUNT_employeeabsent`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `COUNT_employeeabsent`(`EmpID` INT, `OrgID` INT, `EmpStartDate` DATE, `payperiodDateFrom` DATE, `payperiodDateTo` DATE) RETURNS decimal(11,2)
    DETERMINISTIC
BEGIN

DECLARE absentcount DECIMAL(11,2);

DECLARE isNightShift CHAR(1);

DECLARE daterange DATE;

DECLARE shiftRowID INT(11);

DECLARE timedifference TIME;

DECLARE hoursofduty DECIMAL(11,3);

DECLARE yes_else INT(1);

SELECT IF(EmpStartDate > payperiodDateFrom, EmpStartDate, payperiodDateFrom) INTO daterange;

SELECT COALESCE(NightShift,0),ShiftID FROM employeeshift WHERE EmployeeID=EmpID AND OrganizationID=OrgID AND payperiodDateFrom BETWEEN DATE(COALESCE(EffectiveFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) AND DATEDIFF(payperiodDateFrom,EffectiveFrom) >= 0 AND COALESCE(RestDay,0)=0 ORDER BY DATEDIFF(DATE_FORMAT(payperiodDateFrom,'%Y-%m-%d'),EffectiveFrom) LIMIT 1 INTO isNightShift,shiftRowID;

SELECT SUBSTRING_INDEX(TIMEDIFF(TimeFrom,IF(TimeFrom>TimeTo,ADDTIME(TimeTo,'24:00:00'),TimeTo)),'-',-1) FROM shift WHERE RowID=shiftRowID INTO timedifference;

SET hoursofduty = ((TIME_TO_SEC(COALESCE(timedifference,'00:00:00')) / 60) / 60);

IF COALESCE(isNightShift,0) = 1 THEN
SET yes_else = 0;
	SELECT COUNT(RowID) FROM employeetimeentry WHERE EmployeeID=EmpID AND OrganizationID=OrgID AND COALESCE(UndertimeHours,0)=COALESCE(NightDifferentialHours,0) AND COALESCE(UndertimeHours,0)!=0 AND Date BETWEEN daterange AND payperiodDateTo INTO absentcount;

ELSE
SET yes_else = 1;
	SELECT COUNT(RowID) FROM employeetimeentry WHERE EmployeeID=EmpID AND OrganizationID=OrgID AND COALESCE(UndertimeHours,0)=COALESCE(RegularHoursWorked,0) AND COALESCE(UndertimeHours,0)!=0 AND Date BETWEEN daterange AND payperiodDateTo INTO absentcount;

END IF;

SET absentcount = COALESCE(absentcount,0.00) * COALESCE(hoursofduty,0.00);

RETURN hoursofduty;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
