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

    /*
     * Breakdown of daily allowances
     */
    SELECT
        prd.PartNo AS `PartNo`,
        sum.Date AS `Date`,
        sum.TotalAllowanceAmt AS `Amount`
    FROM paystubitem_sum_daily_allowance_group_prodid sum
    INNER JOIN product prd
    ON prd.RowID = sum.ProductID
    WHERE sum.EmployeeID = eallow_EmployeeID AND
        sum.Date BETWEEN effective_datefrom AND effective_dateto
    ORDER BY prd.PartNo, sum.Date;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
