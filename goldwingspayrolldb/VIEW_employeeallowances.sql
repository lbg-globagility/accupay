/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_employeeallowances`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeeallowances`(IN `eallow_EmployeeID` INT, IN `eallow_OrganizationID` INT, IN `effective_datefrom` DATE, IN `effective_dateto` DATE, IN `ExceptThisAllowance` TEXT)
    DETERMINISTIC
BEGIN


IF ExceptThisAllowance = '' THEN

    SELECT
    psi.RowID
    ,p.PartNo
    ,ROUND(psi.PayAmount,2) AS PayAmount
    ,'Monthly' AS Frequency
    ,ps.PayFromDate
    ,ps.PayToDate
    ,IF(p.`Status` = '0', 'No', 'Yes') AS `Status`
    ,p.RowID
    FROM paystubitem psi
    INNER JOIN product p ON p.RowID=psi.ProductID AND p.`Category`='Allowance Type'
    INNER JOIN payperiod pyp ON pyp.PayFromDate=effective_datefrom AND pyp.PayToDate=effective_dateto AND pyp.OrganizationID=eallow_OrganizationID
    INNER JOIN paystub ps ON ps.RowID=psi.PayStubID AND ps.EmployeeID=eallow_EmployeeID AND ps.OrganizationID=eallow_OrganizationID AND ps.PayPeriodID=pyp.RowID
    WHERE IFNULL(psi.PayAmount,0)!=0;

ELSE

    SELECT
    psi.RowID
    ,p.PartNo
    ,ROUND(psi.PayAmount,2) AS PayAmount
    ,'Monthly' AS Frequency
    ,ps.PayFromDate
    ,ps.PayToDate
    ,IF(p.`Status` = '0', 'No', 'Yes') AS `Status`
    ,p.RowID
    FROM paystubitem psi
    INNER JOIN payperiod pyp ON pyp.PayFromDate=effective_datefrom AND pyp.PayToDate=effective_dateto AND pyp.OrganizationID=eallow_OrganizationID
    INNER JOIN paystub ps ON ps.EmployeeID=eallow_EmployeeID AND ps.OrganizationID=eallow_OrganizationID AND ps.PayPeriodID=pyp.RowID AND psi.PayStubID=ps.RowID
    INNER JOIN product p ON p.RowID=psi.ProductID
    WHERE p.Category='Allowance Type'
    AND p.PartNo != ExceptThisAllowance
    AND IFNULL(psi.PayAmount,0)!=0;

END IF;



END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
