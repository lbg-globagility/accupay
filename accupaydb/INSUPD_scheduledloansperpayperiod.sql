/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_scheduledloansperpayperiod`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `INSUPD_scheduledloansperpayperiod`(`slp_RowID` INT(11)
, `slp_OrganizID` INT(11)
, `UserRowID` INT(11)
, `slp_PayPeriodID` INT(11)
, `slp_EmployeeID` INT(11)
, `slp_EmpLoanRecID` INT(11)
, `slp_LoanPayPeriodLeft` INT(11)
, `slp_TotBalLeft` DECIMAL(20,6)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11);

INSERT INTO scheduledloansperpayperiod
(
    RowID
    ,CreatedBy
    ,OrganizationID
    ,PayPeriodID
    ,EmployeeID
    ,EmployeeLoanRecordID
    ,LoanPayPeriodLeft
    ,TotalBalanceLeft
) VALUES (
    slp_RowID
    ,UserRowID
    ,slp_OrganizID
    ,slp_PayPeriodID
    ,slp_EmployeeID
    ,slp_EmpLoanRecID
    ,slp_LoanPayPeriodLeft
    ,slp_TotBalLeft
) ON
DUPLICATE
KEY
UPDATE
    LastUpdBy=UserRowID;SELECT @@Identity AS ID INTO returnvalue;

RETURN returnvalue;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
