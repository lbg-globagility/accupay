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

-- Dumping structure for procedure goldwingspayrolldb.mydetails
DROP PROCEDURE IF EXISTS `mydetails`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `mydetails`()
    DETERMINISTIC
BEGIN

DECLARE i INT(11) DEFAULT 0;

DECLARE max_limit INT(11) DEFAULT 50;

DECLARE page_num INT(11) DEFAULT 50;

DECLARE max_val INT(11) DEFAULT 0;

SELECT (COUNT(e.RowID) / max_limit) + 1 FROM employee e WHERE e.OrganizationID=3 INTO max_val;

WHILE i < max_val DO
	
	SET page_num = i * max_limit;
	
	
	
	SELECT
	e.RowID
	,e.EmployeeID,e.MaritalStatus
	,e.NoOfDependents
	,e.PayFrequencyID
	,e.EmployeeType
	,e.EmploymentStatus
	,e.WorkDaysPerYear
	,e.PositionID
	,IF(e.AgencyID IS NOT NULL, IFNULL(d.PhHealthDeductSchedAgency,d.PhHealthDeductSched), d.PhHealthDeductSched) AS PhHealthDeductSched
	,IF(e.AgencyID IS NOT NULL, IFNULL(d.HDMFDeductSchedAgency,d.HDMFDeductSched), d.HDMFDeductSched) AS HDMFDeductSched
	,IF(e.AgencyID IS NOT NULL, IFNULL(d.SSSDeductSchedAgency,d.SSSDeductSched), d.SSSDeductSched) AS SSSDeductSched
	,IF(e.AgencyID IS NOT NULL, IFNULL(d.WTaxDeductSchedAgency,d.WTaxDeductSched), d.WTaxDeductSched) AS WTaxDeductSched
	,PAYFREQUENCY_DIVISOR(pf.PayFrequencyType) 'PAYFREQUENCY_DIVISOR'
	,IFNULL(dmw.Amount,d.MinimumWageAmount) AS MinimumWageAmount
	,(e.StartDate BETWEEN '2016-10-21' AND '2016-11-05') AS IsFirstTimeSalary
	
	FROM employee e
	LEFT JOIN employeesalary esal ON e.RowID=esal.EmployeeID
	LEFT JOIN `position` p ON p.RowID=e.PositionID
	LEFT JOIN `division` d ON d.RowID=p.DivisionId
	LEFT JOIN agency ag ON ag.RowID=e.AgencyID
	INNER JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID
	LEFT JOIN divisionminimumwage dmw ON dmw.OrganizationID=e.OrganizationID AND dmw.DivisionID=d.RowID AND '2016-11-05' BETWEEN dmw.EffectiveDateFrom AND dmw.EffectiveDateTo
	WHERE e.OrganizationID=3 AND '2016-11-05' BETWEEN esal.EffectiveDateFrom AND COALESCE(esal.EffectiveDateTo,'2016-11-05')
	GROUP BY e.RowID
	ORDER BY e.RowID DESC
	LIMIT page_num, max_limit;
	
	SET i = i + 1;

END WHILE;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
