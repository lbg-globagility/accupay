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

-- Dumping structure for procedure RPT_AlphaListemployee
DROP PROCEDURE IF EXISTS `RPT_AlphaListemployee`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `RPT_AlphaListemployee`(IN `OrganizID` INT)
    DETERMINISTIC
BEGIN

SELECT e.RowID
,e.EmployeeID 'EmployeeID'
,UPPER(e.LastName) LastName
,UPPER(e.FirstName) FirstName
,IF(e.MiddleName = '', e.MiddleName, UPPER(e.MiddleName)) MiddleName
,REPLACE(e.TINNo,'-',' ') TINNo
,e.HomeAddress
,e.StartDate
,e.TerminationDate
,DATE_FORMAT(e.Birthdate,'%m%d%Y') Birthdate
,e.MobilePhone
,IF(e.MaritalStatus = 'Single', 'N', 'Y') AS ExemptionStatus
,e.EmployeeType
,e.EmploymentStatus
,fs.FilingStatus AS 'ExemptionCode'
,e.WorkDaysPerYear WorkDaysPerYear
,sa.BasicPay
FROM employee e
LEFT JOIN filingstatus fs
ON fs.MaritalStatus = e.MaritalStatus AND e.NoOfDependents = fs.Dependent
LEFT JOIN employeesalary sa
ON sa.RowID = (
    SELECT salid.RowID
    FROM employeesalary salid
    WHERE salid.EmployeeID = e.RowID
    ORDER BY salid.RowID
    LIMIT 1
)
WHERE e.OrganizationID=OrganizID
AND e.AlphaListExempted='0';

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
