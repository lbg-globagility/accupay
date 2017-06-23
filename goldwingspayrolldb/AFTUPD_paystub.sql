-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.11-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.0.0.4396
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for trigger goldwingspayrolldb.AFTUPD_paystub
DROP TRIGGER IF EXISTS `AFTUPD_paystub`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_paystub` AFTER UPDATE ON `paystub` FOR EACH ROW BEGIN
DECLARE auditRowID INT(11);

DECLARE viewRowID INT(11);


DECLARE viewID INT(11);

DECLARE IsrbxpayrollFirstHalfOfMonth TEXT;

DECLARE anyint INT(11);

DECLARE product_rowid INT(11);

DECLARE anyamount DECIMAL(11,6);

DECLARE allowancetype_IDs VARCHAR(150);

DECLARE loantype_IDs VARCHAR(150);

DECLARE payperiod_type VARCHAR(50);


DECLARE anyvchar VARCHAR(150);

DECLARE psi_RowID INT(11);


DECLARE e_startdate DATE;

DECLARE e_type VARCHAR(50);

DECLARE IsFirstTimeSalary CHAR(1);

DECLARE totalworkamount DECIMAL(11,6);

DECLARE empsalRowID INT(11);

DECLARE actualrate DECIMAL(11,6);

DECLARE actualgross DECIMAL(11,6);

DECLARE pftype VARCHAR(50);

DECLARE totalallowanceamount VARCHAR(50);

DECLARE MonthCount DECIMAL(11,2) DEFAULT 12.0;


SELECT pr.`Half` FROM payperiod pr WHERE pr.RowID=NEW.PayPeriodID INTO IsrbxpayrollFirstHalfOfMonth;

IF IsrbxpayrollFirstHalfOfMonth = '1' THEN

    SET payperiod_type = 'First half';

ELSE

    SET payperiod_type = 'End of the month';

END IF;


SELECT RowID FROM product WHERE PartNo='.PAGIBIG' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT RowID FROM paystubitem WHERE PayStubID=NEW.RowID AND OrganizationID=NEW.OrganizationID AND ProductID=product_rowid AND Undeclared='0' INTO psi_RowID;
SELECT INSUPD_paystubitem(psi_RowID, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, NEW.TotalEmpHDMF) INTO anyint;

SELECT RowID FROM product WHERE PartNo='.PhilHealth' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT RowID FROM paystubitem WHERE PayStubID=NEW.RowID AND OrganizationID=NEW.OrganizationID AND ProductID=product_rowid AND Undeclared='0' INTO psi_RowID;
SELECT INSUPD_paystubitem(psi_RowID, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, NEW.TotalEmpPhilhealth) INTO anyint;

SELECT RowID FROM product WHERE PartNo='.SSS' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT RowID FROM paystubitem WHERE PayStubID=NEW.RowID AND OrganizationID=NEW.OrganizationID AND ProductID=product_rowid AND Undeclared='0' INTO psi_RowID;
SELECT INSUPD_paystubitem(psi_RowID, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, NEW.TotalEmpSSS) INTO anyint;

SELECT RowID FROM product WHERE PartNo='Absent' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT RowID FROM paystubitem WHERE PayStubID=NEW.RowID AND OrganizationID=NEW.OrganizationID AND ProductID=product_rowid AND Undeclared='0' INTO psi_RowID;
SELECT SUM(Absent) FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO anyamount;
SELECT INSUPD_paystubitem(psi_RowID, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, anyamount) INTO anyint;

SELECT RowID FROM product WHERE PartNo='Tardiness' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT RowID FROM paystubitem WHERE PayStubID=NEW.RowID AND OrganizationID=NEW.OrganizationID AND ProductID=product_rowid AND Undeclared='0' INTO psi_RowID;
SELECT SUM(HoursLateAmount) FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO anyamount;
SELECT INSUPD_paystubitem(psi_RowID, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, anyamount) INTO anyint;

SELECT RowID
FROM product
WHERE PartNo='Undertime'
    AND OrganizationID=NEW.OrganizationID
INTO product_rowid;

SELECT RowID
FROM paystubitem
WHERE PayStubID=NEW.RowID
    AND OrganizationID=NEW.OrganizationID
    AND ProductID=product_rowid
    AND Undeclared='0'
INTO psi_RowID;

SELECT SUM(UndertimeHoursAmount)
FROM employeetimeentry
WHERE EmployeeID=NEW.EmployeeID
    AND OrganizationID=NEW.OrganizationID
    AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate
INTO anyamount;

SELECT INSUPD_paystubitem(psi_RowID, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, anyamount) INTO anyint;





INSERT INTO paystubitem(
    RowID,
    OrganizationID,
    Created,
    CreatedBy,
    PayStubID,
    ProductID,
    PayAmount,
    Undeclared
)
SELECT
    psi.RowID,
    NEW.OrganizationID,
    CURRENT_TIMESTAMP(),
    NEW.LastUpdBy,
    NEW.RowID,
    p.RowID,
    i.Result,
    x.IsActual
FROM product p
INNER JOIN (
        SELECT 0 `IsActual`
    UNION
        SELECT 1 `IsActual`
    ) x
LEFT JOIN paystubitem psi
    ON psi.PayStubID=NEW.RowID
    AND psi.ProductID=p.RowID
    AND psi.Undeclared=x.IsActual
LEFT JOIN (
        SELECT
            0 `Actual`,
            SUM(et.HolidayPayAmount) AS Result
        FROM employeetimeentry et
        WHERE (et.OtherLeaveHours + et.MaternityLeaveHours + et.VacationLeaveHours + et.SickLeaveHours) = 0
            AND et.EmployeeID=NEW.EmployeeID
            AND et.OrganizationID=NEW.OrganizationID
            AND et.`Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate
    UNION
        SELECT
            1 `Actual`,
            SUM(et.HolidayPayAmount) AS Result
        FROM employeetimeentryactual et
        WHERE (et.OtherLeaveHours + et.MaternityLeaveHours + et.VacationLeaveHours + et.SickLeaveHours) = 0
            AND et.EmployeeID=NEW.EmployeeID
            AND et.OrganizationID=NEW.OrganizationID
            AND et.`Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate
    ) i
    ON i.Actual=psi.Undeclared
WHERE p.PartNo='Holiday pay' AND p.`Category`='Miscellaneous'
    AND p.OrganizationID=NEW.OrganizationID
ON DUPLICATE KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP(),
    LastUpdBy=NEW.LastUpdBy,
    PayAmount=i.Result;





























SELECT RowID FROM product WHERE PartNo='Night differential' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT RowID FROM paystubitem WHERE PayStubID=NEW.RowID AND OrganizationID=NEW.OrganizationID AND ProductID=product_rowid AND Undeclared='0' INTO psi_RowID;
SELECT SUM(NightDiffHoursAmount) FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO anyamount;
SELECT INSUPD_paystubitem(psi_RowID, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, anyamount) INTO anyint;

SELECT RowID FROM product WHERE PartNo='Night differential OT' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT RowID FROM paystubitem WHERE PayStubID=NEW.RowID AND OrganizationID=NEW.OrganizationID AND ProductID=product_rowid AND Undeclared='0' INTO psi_RowID;
SELECT SUM(NightDiffOTHoursAmount) FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO anyamount;
SELECT INSUPD_paystubitem(psi_RowID, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, anyamount) INTO anyint;

SELECT RowID FROM product WHERE PartNo='Overtime' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT RowID FROM paystubitem WHERE PayStubID=NEW.RowID AND OrganizationID=NEW.OrganizationID AND ProductID=product_rowid AND Undeclared='0' INTO psi_RowID;
SELECT SUM(OvertimeHoursAmount) FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO anyamount;
SELECT INSUPD_paystubitem(psi_RowID, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, anyamount) INTO anyint;










SELECT RowID FROM product WHERE PartNo='Gross Income' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT RowID FROM paystubitem WHERE PayStubID=NEW.RowID AND OrganizationID=NEW.OrganizationID AND ProductID=product_rowid AND Undeclared='0' INTO psi_RowID;
SELECT INSUPD_paystubitem(psi_RowID, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, NEW.TotalGrossSalary) INTO anyint;

SELECT RowID FROM product WHERE PartNo='Net Income' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT RowID FROM paystubitem WHERE PayStubID=NEW.RowID AND OrganizationID=NEW.OrganizationID AND ProductID=product_rowid AND Undeclared='0' INTO psi_RowID;
SELECT INSUPD_paystubitem(psi_RowID, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, NEW.TotalNetSalary) INTO anyint;

SELECT RowID FROM product WHERE PartNo='Taxable Income' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT RowID FROM paystubitem WHERE PayStubID=NEW.RowID AND OrganizationID=NEW.OrganizationID AND ProductID=product_rowid AND Undeclared='0' INTO psi_RowID;
SELECT INSUPD_paystubitem(psi_RowID, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, NEW.TotalTaxableSalary) INTO anyint;

SELECT RowID FROM product WHERE PartNo='Withholding Tax' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT RowID FROM paystubitem WHERE PayStubID=NEW.RowID AND OrganizationID=NEW.OrganizationID AND ProductID=product_rowid AND Undeclared='0' INTO psi_RowID;
SELECT INSUPD_paystubitem(psi_RowID, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, NEW.TotalEmpWithholdingTax) INTO anyint;











SELECT GROUP_CONCAT(p.RowID)
FROM product p
INNER JOIN category c ON c.CategoryName='Allowance Type' AND c.OrganizationID=NEW.OrganizationID AND c.RowID=p.CategoryID
WHERE p.OrganizationID=NEW.OrganizationID
INTO allowancetype_IDs;

SELECT
GROUP_CONCAT(DISTINCT(ea.ProductID))
FROM employeetimeentry ete
INNER JOIN (SELECT * FROM employeeallowance WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND (EffectiveStartDate >= NEW.PayFromDate OR EffectiveEndDate >= NEW.PayFromDate) AND (EffectiveStartDate <= NEW.PayToDate OR EffectiveEndDate <= NEW.PayToDate)) ea ON ea.RowID > 0
WHERE ete.EmployeeID=NEW.EmployeeID
AND ete.OrganizationID=NEW.OrganizationID
AND ete.`Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate
INTO anyvchar;



SET @dailyallowanceamount = 0.00;

SET @timediffcount = 0.00;









SET @timediffcount = 0.00;


UPDATE paystubitem psi INNER JOIN product p ON p.RowID=psi.ProductID AND p.OrganizationID=psi.OrganizationID AND p.`Category`='Allowance Type' SET psi.PayAmount=0,psi.LastUpd=CURRENT_TIMESTAMP(),psi.LastUpdBy=NEW.LastUpdBy WHERE psi.PayStubID=NEW.RowID;
SET @any_decimalamount = 0.00;
INSERT INTO paystubitem(ProductID,OrganizationID,Created,CreatedBy,PayStubID,PayAmount,Undeclared)
    SELECT
    ii.ProductID
    ,NEW.OrganizationID
    ,CURRENT_TIMESTAMP()
    ,NEW.LastUpdBy
    ,NEW.RowID
    ,(@any_decimalamount := SUM(ii.TotalAllowanceAmt))
    ,'0'
    FROM
    (
        SELECT DISTINCT(i.etRowID) AS et_RowID,i.ProductID,i.TotalAllowanceAmt FROM paystubitem_sum_daily_allowance_group_prodid i WHERE i.`Fixed` = 0 AND i.EmployeeID=NEW.EmployeeID AND i.OrganizationID=NEW.OrganizationID AND i.`Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate
    ) ii
    WHERE ii.ProductID IS NOT NULL GROUP BY ii.ProductID
ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=NEW.LastUpdBy
    ,PayAmount=@any_decimalamount;

INSERT INTO paystubitem(ProductID,OrganizationID,Created,CreatedBy,PayStubID,PayAmount,Undeclared)
    SELECT
    ii.ProductID
    ,NEW.OrganizationID
    ,CURRENT_TIMESTAMP()
    ,NEW.LastUpdBy
    ,NEW.RowID
    ,ii.TotalAllowanceAmount
    ,'0'
    FROM
    (
        SELECT ii.AllowanceAmount - (SUM(i.HoursToLess) * ((i.AllowanceAmount / (i.WorkDaysPerYear / (i.PAYFREQDIV * 12))) / 8)) AS TotalAllowanceAmount,ii.ProductID
            FROM paystubitem_sum_semimon_allowance_group_prodid i
            INNER JOIN (SELECT ea.*,MIN(d.DateValue) AS DateRange1,MAX(d.DateValue) AS DateRange2 FROM dates d INNER JOIN employeeallowance ea ON ea.AllowanceFrequency='Semi-monthly' AND ea.EmployeeID=NEW.EmployeeID AND ea.OrganizationID=NEW.OrganizationID AND d.DateValue BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate WHERE d.DateValue BETWEEN NEW.PayFromDate AND NEW.PayToDate GROUP BY ea.RowID ORDER BY d.DateValue) ii ON i.EmployeeID=ii.EmployeeID AND i.OrganizationID=ii.OrganizationID AND i.`Date` BETWEEN ii.DateRange1 AND ii.DateRange2
        AND i.`Fixed` = 0
        GROUP BY ii.RowID
    ) ii
    WHERE ii.ProductID IS NOT NULL
ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=NEW.LastUpdBy
    ,PayAmount=ii.TotalAllowanceAmount;

SET @var_int = 0;
IF IsrbxpayrollFirstHalfOfMonth = '0' THEN

    SET @dailyallowanceamount = 0.00;

    SET @timediffcount = 0.00;

    INSERT INTO paystubitem(ProductID,OrganizationID,Created,CreatedBy,PayStubID,PayAmount,Undeclared)
        SELECT
        ii.ProductID
        ,NEW.OrganizationID
        ,CURRENT_TIMESTAMP()
        ,NEW.LastUpdBy
        ,NEW.RowID
        ,ii.TotalAllowanceAmount
        ,'0'
        FROM
        (
            SELECT
            DISTINCT(ea.ProductID) AS ProductID
            ,(SELECT @dailyallowanceamount := ROUND((ea.AllowanceAmount / (e.WorkDaysPerYear / MonthCount)),2)) AS Column1
            ,(SELECT @timediffcount := COMPUTE_TimeDifference(sh.TimeFrom,sh.TimeTo)) AS Column2
            ,(SELECT @timediffcount := IF(@timediffcount < 5,@timediffcount,(@timediffcount - 1.0))) AS Column3
            ,SUM(@dailyallowanceamount - ( ( (et.HoursLate + et.UndertimeHours) / @timediffcount ) * @dailyallowanceamount )) AS TotalAllowanceAmount
            FROM employeetimeentry et
            INNER JOIN employee e ON e.OrganizationID=NEW.OrganizationID AND e.RowID=NEW.EmployeeID AND e.EmploymentStatus NOT IN ('Resigned','Terminated')
            INNER JOIN employeeshift es ON es.RowID=et.EmployeeShiftID AND es.OrganizationID=NEW.OrganizationID
            INNER JOIN shift sh ON sh.RowID=es.ShiftID
            INNER JOIN employeeallowance ea ON ea.AllowanceFrequency='Monthly' AND ea.EmployeeID=e.RowID AND ea.OrganizationID=NEW.OrganizationID AND et.`Date` BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate
            INNER JOIN product p ON p.RowID=ea.ProductID AND p.`Fixed`=0
            WHERE et.OrganizationID=NEW.OrganizationID AND et.EmployeeID=e.RowID AND et.`Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate
        UNION ALL
            SELECT
            DISTINCT(ea.ProductID) AS ProductID
            ,0 AS Column1
            ,0 AS Column2
            ,0 AS Column3
            ,SUM(ea.AllowanceAmount) AS TotalAllowanceAmount
            FROM employeeallowance ea
            INNER JOIN employee e ON e.OrganizationID=NEW.OrganizationID AND e.RowID=NEW.EmployeeID AND e.EmploymentStatus NOT IN ('Resigned','Terminated')
            INNER JOIN product p ON p.RowID=ea.ProductID AND p.`Fixed`=1
            WHERE ea.AllowanceFrequency IN ('Monthly','Semi-monthly')
            AND ea.EmployeeID=e.RowID
            AND ea.OrganizationID=NEW.OrganizationID
            AND (ea.EffectiveStartDate >= NEW.PayFromDate OR ea.EffectiveEndDate >= NEW.PayFromDate)
            AND (ea.EffectiveStartDate <= NEW.PayToDate OR ea.EffectiveEndDate <= NEW.PayToDate)
        ) ii
        WHERE ii.ProductID IS NOT NULL
    ON
    DUPLICATE
    KEY
    UPDATE
        LastUpd=CURRENT_TIMESTAMP()
        ,LastUpdBy=NEW.LastUpdBy
        ,PayAmount=ii.TotalAllowanceAmount;







END IF;





    INSERT INTO paystubitem(ProductID,OrganizationID,Created,CreatedBy,PayStubID,PayAmount,Undeclared)
        SELECT
        ii.ProductID
        ,NEW.OrganizationID
        ,CURRENT_TIMESTAMP()
        ,NEW.LastUpdBy
        ,NEW.RowID
        ,(@any_decimalamount := SUM(ii.TotalAllowanceAmt))
        ,'0'
        FROM
        (
            SELECT ea.RowID
            ,ea.ProductID
            ,ea.AllowanceAmount `TotalAllowanceAmt`
            FROM employeeallowance ea
            INNER JOIN payperiod pp ON pp.RowID = NEW.PayPeriodID AND pp.Half = 1
            INNER JOIN product p ON p.RowID = ea.ProductID AND p.`Fixed` = 1 AND p.OrganizationID = NEW.OrganizationID AND p.`Category` = 'Allowance Type'
            WHERE ea.EmployeeID         = NEW.EmployeeID
            AND ea.OrganizationID       = NEW.OrganizationID
            AND ea.AllowanceFrequency   = 'Semi-monthly'
            AND (ea.EffectiveStartDate >= NEW.PayFromDate OR ea.EffectiveEndDate >= NEW.PayFromDate)
            AND (ea.EffectiveStartDate <= NEW.PayToDate OR ea.EffectiveEndDate <= NEW.PayToDate)
        UNION
            SELECT ea.RowID
            ,ea.ProductID
            ,ea.AllowanceAmount `TotalAllowanceAmt`
            FROM employeeallowance ea
            INNER JOIN payperiod pp ON pp.RowID = NEW.PayPeriodID AND pp.Half = 0
            INNER JOIN product p ON p.RowID = ea.ProductID AND p.`Fixed` = 1 AND p.OrganizationID = NEW.OrganizationID AND p.`Category` = 'Allowance Type'
            WHERE ea.EmployeeID         = NEW.EmployeeID
            AND ea.OrganizationID       = NEW.OrganizationID
            AND ea.AllowanceFrequency IN ('Monthly','Semi-monthly')
            AND (ea.EffectiveStartDate >= NEW.PayFromDate OR ea.EffectiveEndDate >= NEW.PayFromDate)
            AND (ea.EffectiveStartDate <= NEW.PayToDate OR ea.EffectiveEndDate <= NEW.PayToDate)

        ) ii
        WHERE ii.ProductID IS NOT NULL GROUP BY ii.ProductID
    ON
    DUPLICATE
    KEY
    UPDATE
        LastUpd=CURRENT_TIMESTAMP()
        ,LastUpdBy=NEW.LastUpdBy
        ,PayAmount=@any_decimalamount;


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
    ) SELECT NULL,NEW.LastUpdBy,OrganizationID,NEW.PayPeriodID,EmployeeID,RowID,(LoanPayPeriodLeft - 1),(TotalBalanceLeft - DeductionAmount)
     FROM employeeloanschedule WHERE OrganizationID=NEW.OrganizationID AND BonusID IS NULL AND LoanPayPeriodLeft >= 1 AND `Status`='In Progress' AND EmployeeID=NEW.EmployeeID AND DeductionSchedule IN (payperiod_type,'Per pay period') AND (DedEffectiveDateFrom >= NEW.PayFromDate OR DedEffectiveDateTo >= NEW.PayFromDate) AND (DedEffectiveDateFrom <= NEW.PayToDate OR DedEffectiveDateTo <= NEW.PayToDate)
    ON DUPLICATE KEY UPDATE
        LastUpdBy=NEW.LastUpdBy;









SELECT GROUP_CONCAT(p.RowID)
FROM product p
INNER JOIN category c ON c.CategoryName='Loan Type' AND c.OrganizationID=NEW.OrganizationID AND c.RowID=p.CategoryID
WHERE p.OrganizationID=NEW.OrganizationID
INTO loantype_IDs;

SELECT
GROUP_CONCAT(DISTINCT(els.LoanTypeID))
FROM employeeloanschedule els
WHERE els.EmployeeID=NEW.EmployeeID
AND els.OrganizationID=NEW.OrganizationID AND els.BonusID IS NULL
AND els.DeductionSchedule IN ('Per pay period',payperiod_type)
AND (els.DedEffectiveDateFrom >= NEW.PayFromDate OR els.DedEffectiveDateTo >= NEW.PayFromDate)
AND (els.DedEffectiveDateFrom <= NEW.PayToDate OR els.DedEffectiveDateTo <= NEW.PayToDate)
INTO anyvchar;

INSERT INTO paystubitem(OrganizationID,Created,CreatedBy,PayStubID,ProductID,PayAmount,Undeclared)
    SELECT
    NEW.OrganizationID
    ,CURRENT_TIMESTAMP()
    ,NEW.CreatedBy
    ,NEW.RowID
    ,els.LoanTypeID
    ,els.DeductionAmount
    ,'0'
FROM employeeloanschedule els
WHERE els.EmployeeID=NEW.EmployeeID
AND els.OrganizationID=NEW.OrganizationID AND els.BonusID IS NULL
AND LOCATE(els.LoanTypeID,anyvchar) > 0
AND els.DeductionSchedule IN ('Per pay period',payperiod_type)
AND (els.DedEffectiveDateFrom >= NEW.PayFromDate OR els.DedEffectiveDateTo >= NEW.PayFromDate)
AND (els.DedEffectiveDateFrom <= NEW.PayToDate OR els.DedEffectiveDateTo <= NEW.PayToDate)
ON
DUPLICATE
KEY
UPDATE
    LastUpd = CURRENT_TIMESTAMP()
    ,LastUpdBy = NEW.LastUpdBy
    ,PayAmount = els.DeductionAmount;

SET loantype_IDs = REPLACE(loantype_IDs,',,',',');

INSERT INTO paystubitem(OrganizationID,Created,CreatedBy,PayStubID,ProductID,PayAmount,Undeclared)
    SELECT
    NEW.OrganizationID
    ,CURRENT_TIMESTAMP()
    ,NEW.CreatedBy
    ,NEW.RowID
    ,p.RowID
    ,0.0
    ,'0'
FROM product p
INNER JOIN category c ON c.CategoryName='Loan Type' AND c.OrganizationID=NEW.OrganizationID AND c.RowID=p.CategoryID
WHERE p.OrganizationID=NEW.OrganizationID
AND LOCATE(p.RowID,anyvchar) = 0
ON
DUPLICATE
KEY
UPDATE
    LastUpd = CURRENT_TIMESTAMP()
    ,LastUpdBy = NEW.LastUpdBy
    ,PayAmount = 0.0;













SELECT RowID FROM `view` v WHERE v.OrganizationID=NEW.OrganizationID AND v.ViewName='Employee Pay Slip' INTO viewRowID;

SELECT INS_audittrail_RETRowID(NEW.LastUpdBy, NEW.LastUpdBy, NEW.OrganizationID, viewRowID, 'TotalAdjustments', NEW.RowID, OLD.TotalAdjustments, NEW.TotalAdjustments, 'Update') INTO auditRowID;















SELECT GET_employeeundeclaredsalarypercent(NEW.EmployeeID,NEW.OrganizationID,NEW.PayFromDate,NEW.PayToDate) INTO actualrate;

SELECT e.StartDate,e.EmployeeType,pf.PayFrequencyType FROM employee e INNER JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID WHERE e.RowID=NEW.EmployeeID AND e.OrganizationID=NEW.OrganizationID INTO e_startdate,e_type,pftype;

SELECT (e_startdate BETWEEN NEW.PayFromDate AND NEW.PayToDate) INTO IsFirstTimeSalary;

IF e_type IN ('Fixed','Monthly') AND IsFirstTimeSalary = '1' THEN

    IF e_type = 'Monthly' THEN

        SELECT SUM(TotalDayPay),EmployeeSalaryID FROM employeetimeentryactual WHERE OrganizationID=NEW.OrganizationID AND EmployeeID=NEW.EmployeeID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO totalworkamount,empsalRowID;

        IF totalworkamount IS NULL THEN

            SELECT SUM(TotalDayPay),EmployeeSalaryID FROM employeetimeentry WHERE OrganizationID=NEW.OrganizationID AND EmployeeID=NEW.EmployeeID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO totalworkamount,empsalRowID;

            SET totalworkamount = IFNULL(totalworkamount,0);

            SELECT totalworkamount + (totalworkamount * actualrate) INTO totalworkamount;

        END IF;

        SET totalworkamount = IFNULL(totalworkamount,0);

    ELSEIF e_type = 'Fixed' THEN

        SELECT es.BasicPay FROM employeesalary es WHERE es.EmployeeID=NEW.EmployeeID AND es.OrganizationID=NEW.OrganizationID AND (es.EffectiveDateFrom >= NEW.PayFromDate OR IFNULL(es.EffectiveDateTo,NEW.PayToDate) >= NEW.PayFromDate) AND (es.EffectiveDateFrom <= NEW.PayToDate OR IFNULL(es.EffectiveDateTo,NEW.PayToDate) <= NEW.PayToDate) ORDER BY es.EffectiveDateFrom DESC LIMIT 1 INTO totalworkamount;

        SET totalworkamount = IFNULL(totalworkamount,0) * (IF(actualrate < 1, (actualrate + 1), actualrate));

    END IF;

ELSEIF e_type IN ('Fixed','Monthly') AND IsFirstTimeSalary = '0' THEN

    IF e_type = 'Monthly' THEN

        SELECT (TrueSalary / PAYFREQUENCY_DIVISOR(pftype)) FROM employeesalary es WHERE es.EmployeeID=NEW.EmployeeID AND es.OrganizationID=NEW.OrganizationID AND (es.EffectiveDateFrom >= NEW.PayFromDate OR IFNULL(es.EffectiveDateTo,CURDATE()) >= NEW.PayFromDate) AND (es.EffectiveDateFrom <= NEW.PayToDate OR IFNULL(es.EffectiveDateTo,CURDATE()) <= NEW.PayToDate) ORDER BY es.EffectiveDateFrom DESC LIMIT 1 INTO totalworkamount;

        SELECT totalworkamount - (SUM(HoursLateAmount) + SUM(UndertimeHoursAmount) + SUM(Absent)) FROM employeetimeentryactual WHERE OrganizationID=NEW.OrganizationID AND EmployeeID=NEW.EmployeeID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO totalworkamount;

        IF totalworkamount IS NULL THEN

            SELECT SUM(HoursLateAmount + UndertimeHoursAmount + Absent) FROM employeetimeentry WHERE OrganizationID=NEW.OrganizationID AND EmployeeID=NEW.EmployeeID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO totalworkamount;

            SET totalworkamount = IFNULL(totalworkamount,0);

            SELECT totalworkamount + (totalworkamount * actualrate) INTO totalworkamount;

        END IF;

        SET totalworkamount = IFNULL(totalworkamount,0);

    ELSEIF e_type = 'Fixed' THEN

        SELECT es.BasicPay FROM employeesalary es WHERE es.EmployeeID=NEW.EmployeeID AND es.OrganizationID=NEW.OrganizationID AND (es.EffectiveDateFrom >= NEW.PayFromDate OR IFNULL(es.EffectiveDateTo,NEW.PayToDate) >= NEW.PayFromDate) AND (es.EffectiveDateFrom <= NEW.PayToDate OR IFNULL(es.EffectiveDateTo,NEW.PayToDate) <= NEW.PayToDate) ORDER BY es.EffectiveDateFrom DESC LIMIT 1 INTO totalworkamount;

        SET totalworkamount = IFNULL(totalworkamount,0) * (IF(actualrate < 1, (actualrate + 1), actualrate));

    END IF;

ELSE

    SELECT SUM(TotalDayPay),EmployeeSalaryID FROM employeetimeentryactual WHERE OrganizationID=NEW.OrganizationID AND EmployeeID=NEW.EmployeeID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO totalworkamount,empsalRowID;

    IF totalworkamount IS NULL THEN

        SELECT SUM(TotalDayPay),EmployeeSalaryID FROM employeetimeentry WHERE OrganizationID=NEW.OrganizationID AND EmployeeID=NEW.EmployeeID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO totalworkamount,empsalRowID;

        SET totalworkamount = IFNULL(totalworkamount,0);

        SELECT totalworkamount + (totalworkamount * actualrate) INTO totalworkamount;

    END IF;

    SET totalworkamount = IFNULL(totalworkamount,0);

END IF;

SET actualgross = totalworkamount + NEW.TotalAllowance + NEW.TotalBonus;

SET @totaladjust_actual = IFNULL(
    (
        SELECT SUM(pa.PayAmount)
        FROM paystubadjustmentactual pa
        WHERE pa.PayStubID=NEW.RowID
    ),
    0
);

INSERT INTO paystubactual
(
    RowID
    ,OrganizationID
    ,PayPeriodID
    ,EmployeeID
    ,TimeEntryID
    ,PayFromDate
    ,PayToDate
    ,TotalGrossSalary
    ,TotalNetSalary
    ,TotalTaxableSalary
    ,TotalEmpSSS
    ,TotalEmpWithholdingTax
    ,TotalCompSSS
    ,TotalEmpPhilhealth
    ,TotalCompPhilhealth
    ,TotalEmpHDMF
    ,TotalCompHDMF
    ,TotalVacationDaysLeft
    ,TotalLoans
    ,TotalBonus
    ,TotalAllowance
    ,TotalAdjustments
    ,ThirteenthMonthInclusion
    ,FirstTimeSalary
) VALUES (
    NEW.RowID
    ,NEW.OrganizationID
    ,NEW.PayPeriodID
    ,NEW.EmployeeID
    ,NEW.TimeEntryID
    ,NEW.PayFromDate
    ,NEW.PayToDate
    ,actualgross
    ,(actualgross - (NEW.TotalEmpSSS + NEW.TotalEmpPhilhealth + NEW.TotalEmpHDMF + NEW.TotalEmpWithholdingTax)) - NEW.TotalLoans + (NEW.TotalAdjustments + @totaladjust_actual)
    ,NEW.TotalTaxableSalary + ((NEW.TotalTaxableSalary + NEW.TotalEmpSSS + NEW.TotalEmpPhilhealth + NEW.TotalEmpHDMF) * actualrate)
    ,NEW.TotalEmpSSS
    ,NEW.TotalEmpWithholdingTax
    ,NEW.TotalCompSSS
    ,NEW.TotalEmpPhilhealth
    ,NEW.TotalCompPhilhealth
    ,NEW.TotalEmpHDMF
    ,NEW.TotalCompHDMF
    ,NEW.TotalVacationDaysLeft
    ,NEW.TotalLoans
    ,NEW.TotalBonus
    ,NEW.TotalAllowance
    ,(NEW.TotalAdjustments + @totaladjust_actual)
    ,NEW.ThirteenthMonthInclusion
    ,NEW.FirstTimeSalary
) ON
DUPLICATE
KEY
UPDATE
    OrganizationID=NEW.OrganizationID
    ,PayPeriodID=NEW.PayPeriodID
    ,EmployeeID=NEW.EmployeeID
    ,TimeEntryID=NEW.TimeEntryID
    ,PayFromDate=NEW.PayFromDate
    ,PayToDate=NEW.PayToDate
    ,TotalGrossSalary=actualgross
    ,TotalNetSalary=(actualgross - (NEW.TotalEmpSSS + NEW.TotalEmpPhilhealth + NEW.TotalEmpHDMF + NEW.TotalEmpWithholdingTax)) - NEW.TotalLoans + (NEW.TotalAdjustments + @totaladjust_actual)
    ,TotalTaxableSalary=NEW.TotalTaxableSalary + ((NEW.TotalTaxableSalary + NEW.TotalEmpSSS + NEW.TotalEmpPhilhealth + NEW.TotalEmpHDMF) * actualrate)
    ,TotalEmpSSS=NEW.TotalEmpSSS
    ,TotalEmpWithholdingTax=NEW.TotalEmpWithholdingTax
    ,TotalCompSSS=NEW.TotalCompSSS
    ,TotalEmpPhilhealth=NEW.TotalEmpPhilhealth
    ,TotalCompPhilhealth=NEW.TotalCompPhilhealth
    ,TotalEmpHDMF=NEW.TotalEmpHDMF
    ,TotalCompHDMF=NEW.TotalCompHDMF
    ,TotalVacationDaysLeft=NEW.TotalVacationDaysLeft
    ,TotalLoans=NEW.TotalLoans
    ,TotalBonus=NEW.TotalBonus
    ,TotalAllowance=NEW.TotalAllowance
    ,TotalAdjustments=(NEW.TotalAdjustments + @totaladjust_actual)
    ,ThirteenthMonthInclusion=NEW.ThirteenthMonthInclusion
    ,FirstTimeSalary=NEW.FirstTimeSalary;
CALL UPDATE_employee_leavebalance(NEW.OrganizationID,NEW.EmployeeID,NEW.PayPeriodID,NEW.LastUpdBy);
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
