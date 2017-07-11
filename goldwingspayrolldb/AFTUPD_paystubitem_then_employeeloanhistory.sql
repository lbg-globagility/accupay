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

DECLARE lastdateofmonth DATE;

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



SELECT Category FROM product WHERE RowID=NEW.ProductID INTO categofprod;

SELECT PayPeriodID,EmployeeID,PayToDate,PayFromDate FROM paystub WHERE RowID=NEW.PayStubID INTO paypID,empID,paypDateTo,pay_fromdate;

SELECT EmployeeID FROM paystub WHERE RowID=NEW.PayStubID INTO selectedEmployeeID;

SELECT RowID FROM category WHERE OrganizationID=NEW.OrganizationID AND CategoryName='Loan Type' INTO categoryRowID;

SELECT (CategoryID = categoryRowID) FROM product WHERE RowID=NEW.ProductID INTO isItemLoan;

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
            ,(SELECT PartNo FROM product WHERE RowID=NEW.ProductID)
        );



    END IF;




        SELECT PartNo FROM product WHERE RowID=NEW.ProductID INTO ItemName;

        IF ItemName = 'Gross Incomefasd5f15asd15 151d51151df51a5sd1f51sd5a1f5as1d' THEN

            SELECT PayToDate FROM paystub WHERE RowID=NEW.PayStubID INTO lastdateofmonth;

            SELECT `Half`,`Month` FROM payperiod WHERE RowID=paypID INTO is_LastDateOfMonth,thisdatemonth;

            IF is_LastDateOfMonth = '0' THEN



                SELECT pyp.PayToDate FROM payperiod pyp WHERE pyp.OrganizationID=NEW.OrganizationID AND pyp.`Month`=thisdatemonth AND pyp.`Year`=YEAR(CURDATE()) ORDER BY pyp.PayFromDate DESC,pyp.PayToDate DESC LIMIT 1 INTO lastdateofmonth;

                SELECT pyp.PayFromDate FROM payperiod pyp WHERE pyp.OrganizationID=NEW.OrganizationID AND pyp.`Month`=thisdatemonth AND pyp.`Year`=YEAR(CURDATE()) ORDER BY pyp.PayFromDate,pyp.PayToDate LIMIT 1 INTO fistdatethismonth;

                SELECT RowID FROM thirteenthmonthpay WHERE OrganizationID=NEW.OrganizationID AND PaystubID=NEW.PayStubID LIMIT 1 INTO prev13monthRowID;

                SELECT EmployeeID FROM paystub WHERE RowID=NEW.PayStubID INTO ps_EmployeeID;



                SELECT SUM(psi.PayAmount)
                FROM paystubitem psi
                INNER JOIN paystub ps ON ps.OrganizationID=NEW.OrganizationID
                AND ps.EmployeeID=ps_EmployeeID
                AND (ps.PayFromDate >= fistdatethismonth OR ps.PayToDate >= fistdatethismonth)
                AND (ps.PayFromDate <= lastdateofmonth OR ps.PayToDate <= lastdateofmonth)
                WHERE psi.PayStubID=ps.RowID
                AND psi.ProductID=NEW.ProductID
                AND psi.RowID!=prev13monthRowID
                INTO newvalue;

                IF newvalue IS NULL THEN

                    SET newvalue = 0;

                END IF;

                SET newvalue = newvalue + NEW.PayAmount;

                INSERT INTO thirteenthmonthpay
                (
                    RowID
                    ,OrganizationID
                    ,Created
                    ,CreatedBy
                    ,PaystubID
                    ,Amount
                ) VALUES (
                    prev13monthRowID
                    ,NEW.OrganizationID
                    ,CURRENT_TIMESTAMP()
                    ,NEW.CreatedBy
                    ,NEW.PayStubID
                    ,(newvalue) / 12
                ) ON
                DUPLICATE
                KEY
                UPDATE
                    LastUpd=CURRENT_TIMESTAMP()
                    ,LastUpdBy=NEW.CreatedBy
                    ,Amount=(newvalue) / 12;

            END IF;

        END IF;






















SELECT pp.`Half`
,ps.EmployeeID
,pp.PayFromDate
,pp.PayToDate
FROM payperiod pp
INNER JOIN paystub ps ON ps.RowID=NEW.PayStubID AND pp.RowID=ps.PayPeriodID
INTO IsrbxpayrollFirstHalfOfMonth
        ,e_RowID
        ,pay_datefrom
        ,pay_dateto;

SELECT p.`Category` FROM product p WHERE p.RowID=NEW.ProductID INTO item_categName;

IF item_categName = 'Loan Type'
    && OLD.PayAmount = 0.0
    && NEW.PayAmount != 0.0
    && OLD.PayAmount != NEW.PayAmount THEN

    SET item_categName = 'Loan Type';

    IF IsrbxpayrollFirstHalfOfMonth = '1' THEN

        SET item_categName = 'Loan Type';

    ELSE

        SET item_categName = 'Loan Type';

    END IF;

END IF;


END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
