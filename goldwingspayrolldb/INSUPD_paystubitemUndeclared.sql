/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_paystubitemUndeclared`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_paystubitemUndeclared`(`psi_RowID` INT, `psi_OrganizationID` INT, `psi_CreatedBy` INT, `psi_LastUpdBy` INT, `psi_PayStubID` INT, `psi_ProductID` INT, `psi_PayAmount` DECIMAL(11,2)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11) DEFAULT 0;

DECLARE psiRowID INT(11);

DECLARE undeclaredamount DECIMAL(11,6);

DECLARE e_RowID INT(11);

DECLARE ps_DateFrom DATE;

DECLARE ps_DateTo DATE;

IF psi_ProductID IN (SELECT p.RowID
                            FROM product p
                            INNER JOIN category c ON c.RowID=p.CategoryID
                            WHERE c.CategoryName IN ('Deductions','Miscellaneous','Totals')
                            AND p.OrganizationID=psi_OrganizationID
                            AND LOCATE('.',p.PartNo) = 0
                            ORDER BY p.Category) THEN

    SELECT EmployeeID,PayFromDate,PayToDate FROM paystub WHERE RowID=psi_PayStubID INTO e_RowID,ps_DateFrom,ps_DateTo;

    SELECT GET_employeeundeclaredsalarypercent(e_RowID,psi_OrganizationID,ps_DateFrom,ps_DateTo) INTO undeclaredamount;

    SELECT RowID FROM paystubitem WHERE OrganizationID=psi_OrganizationID AND ProductID=psi_ProductID AND PayStubID=psi_PayStubID AND Undeclared='1' INTO psiRowID;

    INSERT INTO paystubitem
    (
        RowID
        ,OrganizationID
        ,Created
        ,CreatedBy
        ,PayStubID
        ,ProductID
        ,PayAmount
        ,Undeclared
    ) VALUES (
        IF(psi_RowID IS NULL, psiRowID, psi_RowID)
        ,psi_OrganizationID
        ,CURRENT_TIMESTAMP()
        ,psi_CreatedBy
        ,psi_PayStubID
        ,psi_ProductID
        ,(psi_PayAmount * undeclaredamount)
        ,'1'
    ) ON
    DUPLICATE
    KEY
    UPDATE
        LastUpd=CURRENT_TIMESTAMP()
        ,LastUpdBy=psi_LastUpdBy
        ,PayAmount=(psi_PayAmount * undeclaredamount)
        ,Undeclared='1';SELECT @@Identity AS ID INTO returnvalue;

END IF;

RETURN returnvalue;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
