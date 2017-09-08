/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTINS_paystub_then_paystubitem`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_paystub_then_paystubitem` AFTER INSERT ON `paystub` FOR EACH ROW BEGIN

DECLARE viewID INT(11);

DECLARE IsrbxpayrollFirstHalfOfMonth TEXT;

DECLARE anyint INT(11);

DECLARE product_rowid INT(11);

DECLARE anyamount DECIMAL(11,6);

DECLARE allowancetype_IDs VARCHAR(150);

DECLARE loantype_IDs VARCHAR(150);

DECLARE payperiod_type VARCHAR(50);


DECLARE anyvchar VARCHAR(150);


DECLARE vl_per_payp DECIMAL(11,6);

DECLARE sl_per_payp DECIMAL(11,6);

DECLARE ml_per_payp DECIMAL(11,6);

DECLARE othr_per_payp DECIMAL(11,6);

DECLARE e_startdate DATE;

DECLARE e_type VARCHAR(50);

DECLARE IsFirstTimeSalary CHAR(1);

DECLARE totalworkamount DECIMAL(11,2);

DECLARE empsalRowID INT(11);

DECLARE actualrate DECIMAL(11,6);

DECLARE actualgross DECIMAL(11,2);

DECLARE pftype VARCHAR(50);

DECLARE MonthCount DECIMAL(11,2) DEFAULT 12.0;


SELECT pr.`Half` FROM payperiod pr WHERE pr.RowID=NEW.PayPeriodID INTO IsrbxpayrollFirstHalfOfMonth;

IF IsrbxpayrollFirstHalfOfMonth = '0' THEN

    SET payperiod_type = 'End of the month';

ELSE

    SET payperiod_type = 'First half';

END IF;


SELECT RowID FROM product WHERE PartNo='.PAGIBIG' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT INSUPD_paystubitem(NULL, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, NEW.TotalEmpHDMF) INTO anyint;

SELECT RowID FROM product WHERE PartNo='.PhilHealth' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT INSUPD_paystubitem(NULL, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, NEW.TotalEmpPhilhealth) INTO anyint;

SELECT RowID FROM product WHERE PartNo='.SSS' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT INSUPD_paystubitem(NULL, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, NEW.TotalEmpSSS) INTO anyint;

SELECT RowID FROM product WHERE PartNo='Absent' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT SUM(Absent) FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO anyamount;
SELECT INSUPD_paystubitem(NULL, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, anyamount) INTO anyint;

SELECT RowID FROM product WHERE PartNo='Tardiness' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT SUM(HoursLateAmount) FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO anyamount;
SELECT INSUPD_paystubitem(NULL, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, anyamount) INTO anyint;

SELECT RowID FROM product WHERE PartNo='Undertime' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT SUM(UndertimeHoursAmount) FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO anyamount;
SELECT INSUPD_paystubitem(NULL, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, anyamount) INTO anyint;





INSERT INTO paystubitem(RowID,OrganizationID,Created,CreatedBy,PayStubID,ProductID,PayAmount,Undeclared)
    SELECT psi.RowID,NEW.OrganizationID
    ,CURRENT_TIMESTAMP(),NEW.LastUpdBy
    ,NEW.RowID,p.RowID,i.Result,x.IsActual
    FROM product p
    INNER JOIN (SELECT 0 `IsActual` UNION SELECT 1 `IsActual`) x
    LEFT JOIN paystubitem psi ON psi.PayStubID=NEW.RowID AND psi.ProductID=p.RowID AND psi.Undeclared=x.IsActual
    LEFT JOIN (SELECT 0 `Actual`,SUM(et.HolidayPayAmount) AS Result FROM employeetimeentry et WHERE (et.OtherLeaveHours + et.MaternityLeaveHours + et.VacationLeaveHours + et.SickLeaveHours) != 0 AND et.EmployeeID=NEW.EmployeeID AND et.OrganizationID=NEW.OrganizationID AND et.`Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate
                    UNION
                    SELECT 1 `Actual`,SUM(et.HolidayPayAmount) AS Result FROM employeetimeentryactual et WHERE (et.OtherLeaveHours + et.MaternityLeaveHours + et.VacationLeaveHours + et.SickLeaveHours) != 0 AND et.EmployeeID=NEW.EmployeeID AND et.OrganizationID=NEW.OrganizationID AND et.`Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate) i ON i.Actual=psi.Undeclared
    WHERE p.PartNo='Holiday pay' AND p.`Category`='Miscellaneous'
    AND p.OrganizationID=NEW.OrganizationID
ON DUPLICATE KEY UPDATE LastUpd=CURRENT_TIMESTAMP(),LastUpdBy=NEW.LastUpdBy,PayAmount=i.Result;

SELECT RowID FROM product WHERE PartNo='Night differential' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT SUM(NightDiffHoursAmount) FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO anyamount;
SELECT INSUPD_paystubitem(NULL, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, anyamount) INTO anyint;

SELECT RowID FROM product WHERE PartNo='Night differential OT' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT SUM(NightDiffOTHoursAmount) FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO anyamount;
SELECT INSUPD_paystubitem(NULL, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, anyamount) INTO anyint;

SELECT RowID FROM product WHERE PartNo='Overtime' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT SUM(OvertimeHoursAmount) FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO anyamount;
SELECT INSUPD_paystubitem(NULL, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, anyamount) INTO anyint;










SELECT RowID FROM product WHERE PartNo='Gross Income' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT INSUPD_paystubitem(NULL, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, NEW.TotalGrossSalary) INTO anyint;

SELECT RowID FROM product WHERE PartNo='Net Income' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT INSUPD_paystubitem(NULL, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, NEW.TotalNetSalary) INTO anyint;

SELECT RowID FROM product WHERE PartNo='Taxable Income' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT INSUPD_paystubitem(NULL, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, NEW.TotalTaxableSalary) INTO anyint;

SELECT RowID FROM product WHERE PartNo='Withholding Tax' AND OrganizationID=NEW.OrganizationID INTO product_rowid;
SELECT INSUPD_paystubitem(NULL, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, NEW.TotalEmpWithholdingTax) INTO anyint;
















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
        ,(SELECT @dailyallowanceamount := ROUND((ea.AllowanceAmount / (e.WorkDaysPerYear / (PAYFREQUENCY_DIVISOR(pf.PayFrequencyType) * MonthCount))),2)) AS Column1
        ,(SELECT @timediffcount := COMPUTE_TimeDifference(sh.TimeFrom,sh.TimeTo)) AS Column2
        ,(SELECT @timediffcount := IF(@timediffcount IN (4,5),@timediffcount,(@timediffcount - 1.0))) AS Column3
        ,SUM((et.RegularHoursWorked / @timediffcount) * @dailyallowanceamount) AS TotalAllowanceAmount
        FROM employeetimeentry et
        INNER JOIN employee e ON e.OrganizationID=NEW.OrganizationID AND e.RowID=NEW.EmployeeID AND e.EmploymentStatus NOT IN ('Resigned','Terminated')
        INNER JOIN payfrequency pf ON pf.RowID=e.PayFrequencyID
        INNER JOIN employeeshift es ON es.RowID=et.EmployeeShiftID AND es.OrganizationID=NEW.OrganizationID
        INNER JOIN shift sh ON sh.RowID=es.ShiftID
        INNER JOIN employeeallowance ea ON ea.AllowanceFrequency='Semi-monthly' AND ea.EmployeeID=e.RowID AND ea.OrganizationID=NEW.OrganizationID AND et.`Date` BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate
        INNER JOIN product p ON p.RowID=ea.ProductID AND p.`Fixed` = 0
        WHERE et.OrganizationID=NEW.OrganizationID AND et.EmployeeID=e.RowID AND et.`Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate
    ) ii
    WHERE ii.ProductID IS NOT NULL
ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=NEW.LastUpdBy
    ,PayAmount=ii.TotalAllowanceAmount;



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
        ea.ProductID
        ,SUM(ea.AllowanceAmount) AS TotalAllowanceAmount
        FROM employeeallowance ea
        INNER JOIN product p ON p.RowID=ea.ProductID AND p.`Fixed`=1
        WHERE ea.AllowanceFrequency='Semi-monthly' AND ea.EmployeeID=NEW.EmployeeID AND ea.OrganizationID=NEW.OrganizationID
        AND (ea.EffectiveStartDate >= NEW.PayFromDate OR ea.EffectiveEndDate >= NEW.PayFromDate)
        AND (ea.EffectiveStartDate <= NEW.PayToDate OR ea.EffectiveEndDate <= NEW.PayToDate)
        GROUP BY ea.ProductID
    ) ii
    WHERE ii.ProductID IS NOT NULL
ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=NEW.LastUpdBy
    ,PayAmount=ii.TotalAllowanceAmount;



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

        (ea.ProductID) AS ProductID


        ,0 AS Column1
        ,0 AS Column2
        ,SUM(IF(LOCATE('Regular',pr.PayType) > 0 AND LOCATE('cola',p.PartNo) > 0
            ,IF(et.TotalDayPay = GET_employeerateperday(et.EmployeeID,et.OrganizationID,et.`Date`)
                 AND et.RegularHoursAmount = 0
                , ea.AllowanceAmount
                , (et.TotalDayPay / GET_employeerateperday(et.EmployeeID,et.OrganizationID,et.`Date`)) * ea.AllowanceAmount)
                ,IFNULL(((et.RegularHoursWorked / (IF(COMPUTE_TimeDifference(sh.TimeFrom,sh.TimeTo) < 5,
                                                                    COMPUTE_TimeDifference(sh.TimeFrom,sh.TimeTo),
                                                                    (COMPUTE_TimeDifference(sh.TimeFrom,sh.TimeTo) - 1.0)))) * ea.AllowanceAmount),0))) AS TotalAllowanceAmount
        FROM employeetimeentry et
        INNER JOIN employee e ON e.OrganizationID=NEW.OrganizationID AND e.RowID=NEW.EmployeeID AND e.EmploymentStatus NOT IN ('Resigned','Terminated')
        LEFT JOIN employeeshift es ON es.RowID=et.EmployeeShiftID
        LEFT JOIN shift sh ON sh.RowID=es.ShiftID
        INNER JOIN employeeallowance ea ON ea.AllowanceFrequency='Daily' AND ea.EmployeeID=e.RowID AND ea.OrganizationID=NEW.OrganizationID AND et.`Date` BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate
        INNER JOIN product p ON p.RowID=ea.ProductID AND p.`Fixed`=0
        INNER JOIN payrate pr ON pr.RowID=et.PayRateID
        WHERE et.OrganizationID=NEW.OrganizationID AND et.EmployeeID=e.RowID AND et.`Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate
        GROUP BY ea.ProductID
    ) ii
    WHERE ii.ProductID IS NOT NULL
ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=NEW.LastUpdBy
    ,PayAmount=ii.TotalAllowanceAmount;


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
            ,(SELECT @timediffcount := IF(@timediffcount IN (4,5),@timediffcount,(@timediffcount - 1.0))) AS Column3
            ,SUM(@dailyallowanceamount - ( ( (et.HoursLate + et.UndertimeHours) / @timediffcount ) * @dailyallowanceamount )) AS TotalAllowanceAmount
            FROM employeetimeentry et
            INNER JOIN employee e ON e.OrganizationID=NEW.OrganizationID AND e.RowID=NEW.EmployeeID AND e.EmploymentStatus NOT IN ('Resigned','Terminated')
            INNER JOIN employeeshift es ON es.RowID=et.EmployeeShiftID AND es.OrganizationID=NEW.OrganizationID
            INNER JOIN shift sh ON sh.RowID=es.ShiftID
            INNER JOIN employeeallowance ea ON ea.AllowanceFrequency='Monthly' AND ea.EmployeeID=e.RowID AND ea.OrganizationID=NEW.OrganizationID AND et.`Date` BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate
            INNER JOIN product p ON p.RowID=ea.ProductID AND p.`Fixed` = 0
            WHERE et.OrganizationID=NEW.OrganizationID AND et.EmployeeID=e.RowID AND et.`Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate
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
ON DUPLICATE KEY UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=NEW.CreatedBy
    ,PayAmount=els.DeductionAmount;

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
AND LOCATE(p.RowID,anyvchar) = 0;












SET @var_int = 0;

IF IsrbxpayrollFirstHalfOfMonth = '1' THEN

    UPDATE employeeloanschedule SET
    LoanPayPeriodLeft = IF((LoanPayPeriodLeft - 1) < 0, 0, (LoanPayPeriodLeft - 1))
    ,TotalBalanceLeft = TotalBalanceLeft - DeductionAmount
    WHERE OrganizationID=NEW.OrganizationID AND BonusID IS NULL
    AND LoanPayPeriodLeft >= 1
    AND `Status`='In Progress'
    AND EmployeeID IS NULL
    AND DeductionSchedule IN ('First half','Per pay period')
    AND (DedEffectiveDateFrom >= NEW.PayFromDate OR DedEffectiveDateTo >= NEW.PayFromDate)
    AND (DedEffectiveDateFrom <= NEW.PayToDate OR DedEffectiveDateTo <= NEW.PayToDate);



ELSE

    UPDATE employeeloanschedule SET
    LoanPayPeriodLeft = IF((LoanPayPeriodLeft - 1) < 0, 0, (LoanPayPeriodLeft - 1))
    ,TotalBalanceLeft = TotalBalanceLeft - DeductionAmount
    WHERE OrganizationID=NEW.OrganizationID AND BonusID IS NULL
    AND LoanPayPeriodLeft >= 1
    AND `Status`='In Progress'
    AND EmployeeID IS NULL
    AND DeductionSchedule IN ('End of the month','Per pay period')
    AND (DedEffectiveDateFrom >= NEW.PayFromDate OR DedEffectiveDateTo >= NEW.PayFromDate)
    AND (DedEffectiveDateFrom <= NEW.PayToDate OR DedEffectiveDateTo <= NEW.PayToDate);



END IF;

INSERT INTO scheduledloansperpayperiod(
    RowID,
    CreatedBy,
    OrganizationID,
    PayPeriodID,
    EmployeeID,
    EmployeeLoanRecordID,
    LoanPayPeriodLeft,
    TotalBalanceLeft
)
SELECT
    NULL,
    NEW.CreatedBy,
    OrganizationID,
    NEW.PayPeriodID,
    EmployeeID,
    RowID,
    (LoanPayPeriodLeft - 1),
    (TotalBalanceLeft - DeductionAmount)
FROM employeeloanschedule
WHERE OrganizationID = NEW.OrganizationID AND
    BonusID IS NULL AND
    LoanPayPeriodLeft >= 1 AND
    `Status` = 'In Progress' AND
    EmployeeID = NEW.EmployeeID AND
    DeductionSchedule IN (payperiod_type, 'Per pay period') AND
    (DedEffectiveDateFrom >= NEW.PayFromDate OR DedEffectiveDateTo >= NEW.PayFromDate) AND
    (DedEffectiveDateFrom <= NEW.PayToDate OR DedEffectiveDateTo <= NEW.PayToDate)
ON DUPLICATE KEY
UPDATE
    LastUpdBy = NEW.CreatedBy;

SELECT RowID FROM `view` WHERE ViewName='Employee Pay Slip' AND OrganizationID=NEW.OrganizationID LIMIT 1 INTO viewID;

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EmployeeID',NEW.RowID,'',NEW.EmployeeID,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'PayPeriodID',NEW.RowID,'',NEW.PayPeriodID,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TimeEntryID',NEW.RowID,'',NEW.TimeEntryID,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'PayFromDate',NEW.RowID,'',NEW.PayFromDate,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'PayToDate',NEW.RowID,'',NEW.PayToDate,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TotalGrossSalary',NEW.RowID,'',NEW.TotalGrossSalary,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TotalNetSalary',NEW.RowID,'',NEW.TotalNetSalary,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TotalTaxableSalary',NEW.RowID,'',NEW.TotalTaxableSalary,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TotalEmpSSS',NEW.RowID,'',NEW.TotalEmpSSS,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TotalEmpWithholdingTax',NEW.RowID,'',NEW.TotalEmpWithholdingTax,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TotalCompSSS',NEW.RowID,'',NEW.TotalCompSSS,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TotalEmpPhilhealth',NEW.RowID,'',NEW.TotalEmpPhilhealth,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TotalCompPhilhealth',NEW.RowID,'',NEW.TotalCompPhilhealth,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TotalEmpHDMF',NEW.RowID,'',NEW.TotalEmpHDMF,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TotalCompHDMF',NEW.RowID,'',NEW.TotalCompHDMF,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TotalVacationDaysLeft',NEW.RowID,'',NEW.TotalVacationDaysLeft,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TotalLoans',NEW.RowID,'',NEW.TotalLoans,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TotalBonus',NEW.RowID,'',NEW.TotalBonus,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'TotalAllowance',NEW.RowID,'',NEW.TotalAllowance,'Insert');

SET @skipthiscondition = '1';
IF @skipthiscondition = '1' THEN
    CALL UPDATE_employee_leavebalance(NEW.OrganizationID,NEW.EmployeeID,NEW.PayPeriodID,NEW.LastUpdBy);
ELSE
    SELECT
    e.LeaveBalance
    ,e.SickLeaveBalance
    ,e.MaternityLeaveBalance
    ,e.OtherLeaveBalance
    FROM employee e
    WHERE e.RowID=NEW.EmployeeID
    AND e.OrganizationID=NEW.OrganizationID
    AND DATEDIFF(NEW.PayToDate,ADDDATE(e.StartDate, INTERVAL 1 YEAR)) >= 0
    INTO vl_per_payp
            ,sl_per_payp
            ,ml_per_payp
            ,othr_per_payp;

    IF vl_per_payp IS NULL THEN
        SET vl_per_payp = 0.0;
    END IF;

    IF sl_per_payp IS NULL THEN
        SET sl_per_payp = 0.0;
    END IF;

    IF ml_per_payp IS NULL THEN
        SET ml_per_payp = 0.0;
    END IF;

    IF othr_per_payp IS NULL THEN
        SET othr_per_payp = 0.0;
    END IF;


    SELECT RowID FROM product WHERE PartNo='Maternity/paternity leave' AND OrganizationID=NEW.OrganizationID INTO product_rowid;

    SELECT SUM(MaternityLeaveHours) FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO anyamount;

    SET anyamount = ml_per_payp;

    SELECT INSUPD_paystubitem(NULL, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, anyamount) INTO anyint;


    SELECT RowID FROM product WHERE PartNo='Others' AND OrganizationID=NEW.OrganizationID AND `Category`='Leave Type' INTO product_rowid;

    SELECT SUM(OtherLeaveHours) FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO anyamount;

    SET anyamount = othr_per_payp;

    SELECT INSUPD_paystubitem(NULL, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, anyamount) INTO anyint;


    SELECT RowID FROM product WHERE PartNo='Sick leave' AND OrganizationID=NEW.OrganizationID INTO product_rowid;

    SELECT SUM(SickLeaveHours) FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO anyamount;

    SET anyamount = sl_per_payp;

    SELECT INSUPD_paystubitem(NULL, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, anyamount) INTO anyint;


    SELECT RowID FROM product WHERE PartNo='Vacation leave' AND OrganizationID=NEW.OrganizationID INTO product_rowid;

    SELECT SUM(VacationLeaveHours) FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND `Date` BETWEEN NEW.PayFromDate AND NEW.PayToDate INTO anyamount;

    SET anyamount = vl_per_payp;

    SELECT INSUPD_paystubitem(NULL, NEW.OrganizationID, NEW.CreatedBy, NEW.CreatedBy, NEW.RowID, product_rowid, anyamount) INTO anyint;
END IF;






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
SET @totaladjust_actual = IFNULL((SELECT SUM(pa.PayAmount) FROM paystubadjustmentactual pa WHERE pa.PayStubID=NEW.RowID),0);
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
    ,(actualgross - (NEW.TotalEmpSSS + NEW.TotalEmpPhilhealth + NEW.TotalEmpHDMF + NEW.TotalEmpWithholdingTax)) - NEW.TotalLoans + (NEW.TotalAdjustments * actualrate)
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
    ,(NEW.TotalAdjustments * actualrate)
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
    ,TotalNetSalary=(actualgross - (NEW.TotalEmpSSS + NEW.TotalEmpPhilhealth + NEW.TotalEmpHDMF + NEW.TotalEmpWithholdingTax)) - NEW.TotalLoans + (NEW.TotalAdjustments * actualrate)
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
    ,TotalAdjustments=(NEW.TotalAdjustments * actualrate)
    ,ThirteenthMonthInclusion=NEW.ThirteenthMonthInclusion
    ,FirstTimeSalary=NEW.FirstTimeSalary;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
