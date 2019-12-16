/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `I_paystubadjustmentactual`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `I_paystubadjustmentactual`(`pa_OrganizationID` INT, `pa_CurrentUser` INT, `pa_ProductID` INT, `pa_PayAmount` DECIMAL(10,2), `pa_Comment` VARCHAR(200), `pa_EmployeeID` VARCHAR(50), `pa_PayPeriodID` INT, `psa_RowID` INT) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE pa_PayStubID INT;
DECLARE returnvalue INT(11) DEFAULT 0;
SET pa_PayStubID = (SELECT FN_GetPayStubIDByEmployeeIDAndPayPeriodID(pa_EmployeeID, pa_PayPeriodID, OrganizationID) FROM payperiod WHERE RowID=pa_PayPeriodID);

INSERT INTO paystubadjustmentactual
(
    RowID,
    OrganizationID,
    Created,
    CreatedBy,
    LastUpdBy,
    PayStubID,
    ProductID,
    PayAmount,
    `Comment`
)
VALUES
(
    psa_RowID,
    pa_OrganizationID,
    CURRENT_TIMESTAMP(),
    pa_CurrentUser,
    pa_CurrentUser,
    pa_PayStubID,
    pa_ProductID,
    pa_PayAmount,
    pa_Comment
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=pa_CurrentUser
    ,`Comment`=pa_Comment
    ,PayAmount=pa_PayAmount;SELECT @@Identity AS ID INTO returnvalue;

RETURN IFNULL(returnvalue,0);
END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
