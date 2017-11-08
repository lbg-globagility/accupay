/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `PRINT_employee_profiles`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `PRINT_employee_profiles`(IN `og_rowid` INT)
    DETERMINISTIC
BEGIN

DECLARE pp_rowid
        ,result_count INT(11);

DECLARE preferred_dateformat VARCHAR(50) DEFAULT '%c/%e/%Y';

SELECT ps.PayPeriodID
,COUNT(e.RowID)
FROM paystub ps
INNER JOIN employee e
        ON e.RowID=ps.EmployeeID
           AND e.OrganizationID=ps.OrganizationID
           AND e.EmployeeType NOT IN ('Resigned', 'Terminated')
           AND e.RevealInPayroll IN ('1', 'Y')
INNER JOIN payperiod pp
        ON pp.RowID=ps.PayPeriodID
           AND pp.OrganizationID=ps.OrganizationID
           AND pp.`Year`=YEAR(CURDATE())
           AND pp.TotalGrossSalary=e.PayFrequencyID
WHERE ps.OrganizationID=og_rowid
LIMIT 1
INTO pp_rowid
     ,result_count;

IF pp_rowid IS NOT NULL THEN

	SELECT
	# e.RowID `DatCol1`,
	e.EmployeeID	`Employee ID`
	,e.LastName	`Last Name`
	,e.FirstName	`First name`
	,e.MiddleName	`Middle Name`
	,IFNULL(DATE_FORMAT(e.Birthdate, preferred_dateformat), '') `Birthdate`
	,e.Gender	`Sex`
	,e.MaritalStatus	`Marital Status`
	,e.EmployeeType	`Employee Type`
	,IFNULL(DATE_FORMAT(e.StartDate, preferred_dateformat), '') `Start Date`
	,IFNULL(DATE_FORMAT(e.DateRegularized, preferred_dateformat), '') `Date Regularized`
	,pf.PayFrequencyType	`Pay Frequency`
	,e.EmploymentStatus	`Employment Status`
	,pos.PositionName	`Position Name`
	,e.MobilePhone	`Mobile no.`
	,e.HomeAddress	`Address`
	,e.TINNo	`TIN`
	,e.SSSNo	`SSS No.`
	,e.HDMFNo	`HDMF No.`
	,e.PhilHealthNo	`PHIC No.`
	# ,e.ATMNo
	,IFNULL(COUNT(edp.RowID), 0) `Dependent count`
	/*
	,(@dependent_lists := REPLACE(	
	                      GROUP_CONCAT( CONCAT_WS('', CONCAT_WS(' ', edp.FirstName, edp.LastName), CONCAT('(', edp.RelationToEmployee, ')')) )
								 , ',', '\n')
								 )	`DatCol22`
   */
	FROM employee e
	LEFT JOIN employeedependents edp
	       ON edp.ParentEmployeeID=e.RowID
	          AND edp.OrganizationID=e.OrganizationID
	INNER JOIN payfrequency pf
	        ON pf.RowID=e.PayFrequencyID
	INNER JOIN `position` pos
	        ON pos.RowID=e.PositionID
	
	INNER JOIN paystub ps
	        ON ps.OrganizationID=e.OrganizationID
	           AND ps.EmployeeID=e.RowID
	           AND ps.PayPeriodID=pp_rowid
	
	WHERE e.OrganizationID=og_rowid
	AND e.EmployeeType NOT IN ('Resigned', 'Terminated')
	AND e.RevealInPayroll IN ('1', 'Y')
	GROUP BY e.RowID
	ORDER BY CONCAT_WS(',', e.LastName, e.FirstName, e.MiddleName)
	;
	
ELSE

	SELECT
	# e.RowID `DatCol1`,
	e.EmployeeID	`Employee ID`
	,e.LastName	`Last Name`
	,e.FirstName	`First name`
	,e.MiddleName	`Middle Name`
	,IFNULL(DATE_FORMAT(e.Birthdate, preferred_dateformat), '') `Birthdate`
	,e.Gender	`Sex`
	,e.MaritalStatus	`Marital Status`
	,e.EmployeeType	`Employee Type`
	,IFNULL(DATE_FORMAT(e.StartDate, preferred_dateformat), '') `Start Date`
	,IFNULL(DATE_FORMAT(e.DateRegularized, preferred_dateformat), '') `Date Regularized`
	,pf.PayFrequencyType	`Pay Frequency`
	,e.EmploymentStatus	`Employment Status`
	,pos.PositionName	`Position Name`
	,e.MobilePhone	`Mobile no.`
	,e.HomeAddress	`Address`
	,e.TINNo	`TIN`
	,e.SSSNo	`SSS No.`
	,e.HDMFNo	`HDMF No.`
	,e.PhilHealthNo	`PHIC No.`
	# ,e.ATMNo
	,IFNULL(COUNT(edp.RowID), 0) `Dependent count`
	/*
	,(@dependent_lists := REPLACE(	
	                      GROUP_CONCAT( CONCAT_WS('', CONCAT_WS(' ', edp.FirstName, edp.LastName), CONCAT('(', edp.RelationToEmployee, ')')) )
								 , ',', '\n')
								 )	`DatCol22`
   */
	FROM employee e
	LEFT JOIN employeedependents edp
	       ON edp.ParentEmployeeID=e.RowID
	          AND edp.OrganizationID=e.OrganizationID
	INNER JOIN payfrequency pf
	        ON pf.RowID=e.PayFrequencyID
	INNER JOIN `position` pos
	        ON pos.RowID=e.PositionID
	WHERE e.OrganizationID=og_rowid
	AND e.EmployeeType NOT IN ('Resigned', 'Terminated')
	AND e.RevealInPayroll IN ('1', 'Y')
	GROUP BY e.RowID
	ORDER BY CONCAT_WS(',', e.LastName, e.FirstName, e.MiddleName)
	;
	
END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
