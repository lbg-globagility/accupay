/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTUPD_paystubitem_then_employeeloanhistory`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_paystubitem_then_employeeloanhistory` AFTER UPDATE ON `paystubitem` FOR EACH ROW BEGIN

DECLARE categofprod VARCHAR(100);

DECLARE paypID INT(11);

DECLARE empID INT(11);

DECLARE paypDateTo DATE;

DECLARE pay_fromdate DATE;

DECLARE pay_todate DATE;

DECLARE loanRowID INT(11);

DECLARE pay_periodleft DECIMAL(11,2);

DECLARE remainderamout DECIMAL(11,2) DEFAULT 0;

DECLARE amountloan DECIMAL(11,2);

DECLARE deductamount DECIMAL(11,2);

DECLARE numofpayperiod DECIMAL(11,2);

DECLARE selectedEmployeeID VARCHAR(100);

DECLARE elh_RowID INT(11);

DECLARE loan_status VARCHAR(100);

DECLARE ItemName TEXT;



DECLARE prev13monthRowID INT(11);

DECLARE ps_EmployeeID INT(11);

DECLARE newvalue DECIMAL(11,2);

DECLARE divisor DECIMAL(11,6);

DECLARE categoryRowID INT(11);

DECLARE is_LastDateOfMonth CHAR(1);

DECLARE thisdatemonth CHAR(2);

DECLARE fistdatethismonth DATE;

DECLARE isItemLoan CHAR(1);


DECLARE IsrbxpayrollFirstHalfOfMonth CHAR(1);

DECLARE item_categName VARCHAR(150);

DECLARE e_RowID INT(11);

DECLARE pay_datefrom DATE;

DECLARE pay_dateto DATE;

DECLARE productName VARCHAR(50);






SELECT
    PayPeriodID,
    EmployeeID,
    PayToDate,
    PayFromDate
FROM paystub
WHERE RowID = NEW.PayStubID
INTO
    paypID,
    empID,
    paypDateTo,
    pay_fromdate;






SELECT RowID
FROM category
WHERE OrganizationID = NEW.OrganizationID
    AND CategoryName = 'Loan Type'
INTO categoryRowID;

SELECT
    (CategoryID = categoryRowID),
    PartNo
FROM product
WHERE RowID=NEW.ProductID
INTO
    isItemLoan,
    productName;

IF isItemLoan = '1' THEN

    INSERT INTO employeeloanhistory
    (
        OrganizationID
        ,Created
        ,CreatedBy
        ,EmployeeID
        ,PayPeriodID
        ,PayStubID
        ,DeductionDate
        ,DeductionAmount
        ,Comments
    ) VALUES (
        NEW.OrganizationID
        ,CURRENT_TIMESTAMP()
        ,NEW.CreatedBy
        ,empID
        ,paypID
        ,NEW.PayStubID
        ,pay_todate
        ,NEW.PayAmount
        ,productName
    );

END IF;







































































































































END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
