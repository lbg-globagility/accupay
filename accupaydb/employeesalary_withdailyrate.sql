/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP VIEW IF EXISTS `employeesalary_withdailyrate`;
CREATE TABLE `employeesalary_withdailyrate` (
	`RowID` INT(11) NOT NULL,
	`EmployeeID` INT(11) NOT NULL,
	`Created` DATETIME NOT NULL,
	`CreatedBy` INT(11) NOT NULL,
	`LastUpd` DATETIME NULL,
	`LastUpdBy` INT(11) NULL,
	`OrganizationID` INT(11) NOT NULL,
	`FilingStatusID` INT(11) NOT NULL,
	`PaySocialSecurityID` INT(11) NULL,
	`PayPhilhealthID` INT(11) NULL,
	`PhilHealthDeduction` DECIMAL(15,4) NULL,
	`HDMFAmount` DECIMAL(11,2) NULL,
	`TrueSalary` DECIMAL(11,2) NULL,
	`BasicPay` DECIMAL(11,2) NULL,
	`Salary` DECIMAL(11,2) NULL,
	`UndeclaredSalary` DECIMAL(11,2) NULL,
	`BasicDailyPay` DECIMAL(11,2) NULL,
	`BasicHourlyPay` DECIMAL(11,2) NULL,
	`NoofDependents` INT(11) NULL,
	`MaritalStatus` VARCHAR(50) NULL COLLATE 'latin1_swedish_ci',
	`PositionID` INT(11) NULL,
	`EffectiveDateFrom` DATE NULL,
	`EffectiveDateTo` DATE NULL,
	`OverrideDiscardSSSContrib` TINYINT(4) NULL,
	`OverrideDiscardPhilHealthContrib` TINYINT(4) NULL,
	`DailyRate` DECIMAL(19,6) NULL
) ENGINE=MyISAM;

DROP VIEW IF EXISTS `employeesalary_withdailyrate`;
DROP TABLE IF EXISTS `employeesalary_withdailyrate`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` VIEW `employeesalary_withdailyrate` AS SELECT esa.*
	,(esa.Salary
	  / (e.WorkDaysPerYear
	     / 12 # count of months per year
		  )) `DailyRate`
	FROM employeesalary esa
	INNER JOIN employee e ON e.RowID=esa.EmployeeID AND e.EmployeeType IN ('Monthly', 'Fixed')
	
UNION
	SELECT esa.*
	,esa.BasicPay `DailyRate`
	FROM employeesalary esa
	INNER JOIN employee e ON e.RowID=esa.EmployeeID AND e.EmployeeType = 'Daily' ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
