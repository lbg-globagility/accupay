/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_allowanceperday`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_allowanceperday`(IN `og_rowid` INT, IN `e_rowid` INT, IN `from_date` DATE, IN `to_date` DATE, IN `allowance_frequency` VARCHAR(50), IN `allowance_name` VARCHAR(50))
    DETERMINISTIC
BEGIN

DECLARE daily_frequency
        ,semi_mon_frequency
		  ,custom_date_format
		  ,allowance_category VARCHAR(50);

SET allowance_category = 'Allowance Type';

SET custom_date_format = '%c/%e/%Y';

SET daily_frequency = 'Daily';
SET semi_mon_frequency = 'Semi-monthly';

SELECT ii.*
FROM (SELECT i.*
		FROM (SELECT
				apd.RowID
				,ea.`AllowanceName`
				, DATE_FORMAT(apd.`Date`, custom_date_format) `Date`
				, apd.Amount
				, ea.AllowanceFrequency
				, 'Group 1' `Result`
				FROM allowanceperday apd
				INNER JOIN allowanceitem ai ON ai.RowID = apd.AllowanceItemID
				INNER JOIN (SELECT a.*
				            , p.PartNo `AllowanceName`
				            FROM employeeallowance a
				            INNER JOIN product p
								        ON p.OrganizationID = a.OrganizationID
										     AND p.RowID = a.ProductID
										     AND p.`Category` = allowance_category
										     AND p.ActiveData = 1
				            WHERE a.OrganizationID = og_rowid
				            AND a.EmployeeID = e_rowid
				            AND a.AllowanceFrequency = daily_frequency
				            AND LENGTH(allowance_name) = 0
				            
							UNION
				            SELECT a.*
				            , p.PartNo `AllowanceName`
				            FROM employeeallowance a
				            INNER JOIN product p
								        ON p.OrganizationID = a.OrganizationID
										     AND p.RowID = a.ProductID
										     AND p.`Category` = allowance_category
										     AND p.PartNo = allowance_name
										     AND p.ActiveData = 1
				            WHERE a.OrganizationID = og_rowid
				            AND a.EmployeeID = e_rowid
				            AND a.AllowanceFrequency = daily_frequency
				            AND LENGTH(allowance_name) > 0
								) ea ON ea.RowID = ai.AllowanceID
				WHERE apd.`Date` BETWEEN from_date AND to_date
				ORDER BY ea.`AllowanceName`, apd.`Date`
				) i
		
		# ########################################################################
		UNION
		SELECT i.*
		FROM (SELECT
				apd.RowID
				,ea.`AllowanceName`
				, DATE_FORMAT(apd.`Date`, custom_date_format) `Date`
				, apd.Amount
				, ea.AllowanceFrequency
				, 'Group 2' `Result`
				FROM allowanceperday apd
				INNER JOIN allowanceitem ai ON ai.RowID = apd.AllowanceItemID
				INNER JOIN (SELECT a.*
				            , p.PartNo `AllowanceName`
				            FROM employeeallowance a
				            INNER JOIN product p
								        ON p.OrganizationID = a.OrganizationID
										     AND p.RowID = a.ProductID
										     AND p.`Category` = allowance_category
										     AND p.ActiveData = 1
				            WHERE a.OrganizationID = og_rowid
				            AND a.EmployeeID = e_rowid
				            AND a.AllowanceFrequency = semi_mon_frequency
				            AND LENGTH(allowance_name) = 0
				            
							UNION
				            SELECT a.*
				            , p.PartNo `AllowanceName`
				            FROM employeeallowance a
				            INNER JOIN product p
								        ON p.OrganizationID = a.OrganizationID
										     AND p.RowID = a.ProductID
										     AND p.`Category` = allowance_category
										     AND p.PartNo = allowance_name
										     AND p.ActiveData = 1
				            WHERE a.OrganizationID = og_rowid
				            AND a.EmployeeID = e_rowid
				            AND a.AllowanceFrequency = semi_mon_frequency
				            AND LENGTH(allowance_name) > 0            
				            ) ea ON ea.RowID = ai.AllowanceID
				WHERE apd.`Date` BETWEEN from_date AND to_date
				AND apd.Amount != 0
				ORDER BY ea.`AllowanceName`, apd.`Date`
				) i
		) ii
ORDER BY ii.`AllowanceName`, STR_TO_DATE(ii.`Date`, custom_date_format)
;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
