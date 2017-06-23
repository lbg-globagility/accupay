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

-- Dumping structure for function goldwingspayrolldb.GET_employeeallowancePerDay
DROP FUNCTION IF EXISTS `GET_employeeallowancePerDay`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GET_employeeallowancePerDay`(`OrganizID` INT, `EmpRowID` INT, `IsTaxable` CHAR(1), `PayPeriod_To` DATE) RETURNS decimal(11,4)
    DETERMINISTIC
BEGIN

DECLARE returnvalue DECIMAL(11,4) DEFAULT 0.0;

DECLARE og_WorkDayPerYear INT(11);

DECLARE og_dayCountPerMonth DECIMAL(11,4);

DECLARE og_dayCountPerSemiMonth DECIMAL(11,4);

	

	SELECT GET_empworkdaysperyear(EmpRowID) INTO og_WorkDayPerYear;
			
	SET og_dayCountPerMonth = og_WorkDayPerYear / 12;
	
	SET og_dayCountPerMonth = og_dayCountPerMonth / 2;
	

	
SELECT
SUM(TotalAllowanceAmount)
FROM
(
	SELECT
	EmployeeID
	,AllowanceAmount AS TotalAllowanceAmount
	FROM employeeallowance
	WHERE AllowanceFrequency IN ('Daily','One time')
	AND OrganizationID=OrganizID
	AND EmployeeID=EmpRowID
	AND TaxableFlag=IsTaxable
	AND PayPeriod_To BETWEEN EffectiveStartDate AND EffectiveEndDate
UNION
	SELECT
	EmployeeID
	,(AllowanceAmount / og_dayCountPerMonth) AS TotalAllowanceAmount
	FROM employeeallowance WHERE
	AllowanceFrequency='Monthly'
	AND OrganizationID=OrganizID
	AND EmployeeID=EmpRowID
	AND TaxableFlag=IsTaxable
	AND PayPeriod_To BETWEEN EffectiveStartDate AND EffectiveEndDate
UNION
	SELECT
	EmployeeID
	,(AllowanceAmount / og_dayCountPerMonth) AS TotalAllowanceAmount
	FROM employeeallowance
	WHERE AllowanceFrequency='Semi-monthly'
	AND OrganizationID=OrganizID
	AND EmployeeID=EmpRowID
	AND TaxableFlag=IsTaxable
	AND PayPeriod_To BETWEEN EffectiveStartDate AND EffectiveEndDate
) employeeallowanceperday
GROUP BY EmployeeID
INTO returnvalue;
	
	
	
	
RETURN returnvalue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
