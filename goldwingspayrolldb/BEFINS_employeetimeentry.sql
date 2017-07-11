/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `BEFINS_employeetimeentry`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFINS_employeetimeentry` BEFORE INSERT ON `employeetimeentry` FOR EACH ROW BEGIN

DECLARE isSupervisor CHAR(1);

DECLARE programmer_comments VARCHAR(1500);

DECLARE isRest_day CHAR(1);

DECLARE has_shift CHAR(1);

DECLARE perfect_hrs DECIMAL(11,6);

DECLARE absent_amount DECIMAL(11,6);

DECLARE isDateNotHoliday CHAR(1);
DECLARE isPresentInWorkingDaysPriorToThisDate CHAR(1) DEFAULT '0';
DECLARE TaxableDailyAllowanceAmount DECIMAL(11,6);
DECLARE isSpecialHoliday CHAR(1);
DECLARE rate_this_date DECIMAL(11,6);
DECLARE hourly_rate DECIMAL(11,6);
DECLARE payrate_this_date DECIMAL(11,2);

DECLARE e_rateperday DECIMAL(12,6) DEFAULT 0;

DECLARE default_workhours_everyday DECIMAL(11,6) DEFAULT 8;

DECLARE emp_type VARCHAR(50);

SET @e_rateperday = 0.0;
SELECT IF(e.EmployeeType = 'Daily', es.BasicPay, (es.Salary / (e.WorkDaysPerYear / 24))) FROM employeesalary es INNER JOIN employee e ON e.RowID=es.EmployeeID AND e.OrganizationID=es.OrganizationID WHERE es.RowID=NEW.EmployeeSalaryID INTO @e_rateperday;
SET e_rateperday = IFNULL(@e_rateperday,0);

SELECT EXISTS(SELECT et.RowID FROM employeetimeentry et INNER JOIN payrate pr ON pr.RowID=et.PayRateID AND pr.PayType='Regular Day' WHERE et.EmployeeID=NEW.EmployeeID AND et.OrganizationID=NEW.OrganizationID AND et.`Date` BETWEEN SUBDATE(NEW.`Date`, INTERVAL 6 DAY) AND SUBDATE(NEW.`Date`, INTERVAL 1 DAY) AND et.EmployeeShiftID IS NOT NULL AND et.TotalDayPay > 0 ORDER BY et.`Date` DESC LIMIT 1) INTO isPresentInWorkingDaysPriorToThisDate;
SELECT GET_employeerateperday(NEW.EmployeeID, NEW.OrganizationID, NEW.`Date`) INTO rate_this_date;

SELECT (PayType = 'Regular Day'),(LOCATE('Special',PayType) > 0) FROM payrate WHERE RowID=NEW.PayRateID INTO isDateNotHoliday,isSpecialHoliday;

SET NEW.VacationLeaveHours = IFNULL(NEW.VacationLeaveHours,0);
SET NEW.SickLeaveHours = IFNULL(NEW.SickLeaveHours,0);
SET NEW.MaternityLeaveHours = IFNULL(NEW.MaternityLeaveHours,0);
SET NEW.OtherLeaveHours = IFNULL(NEW.OtherLeaveHours,0);



SELECT RestDay FROM employeeshift WHERE RowID=NEW.EmployeeShiftID INTO isRest_day;
SELECT (e.DayOfRest = DAYOFWEEK(NEW.`Date`)) FROM employee e WHERE e.RowID=NEW.EmployeeID INTO isRest_day;

SET isRest_day = IFNULL(isRest_day,'1');

SET programmer_comments = 'Supervisors is equal to DivisionUniqueID 3';

SELECT (d.DivisionUniqueID = 3)
FROM position p
INNER JOIN employee e ON e.RowID=NEW.EmployeeID
INNER JOIN `division` d ON d.RowID=p.DivisionId
WHERE p.RowID=e.PositionID
INTO isSupervisor;

SET NEW.RegularHoursWorked = IFNULL(NEW.RegularHoursWorked,0);
SET NEW.RegularHoursAmount = IFNULL(NEW.RegularHoursAmount,0);

IF isSupervisor = '9' AND isRest_day = '9' THEN
SET isSupervisor = '1';

IF NEW.RegularHoursWorked < 4 THEN
SET NEW.RegularHoursWorked = 4;
SET NEW.RegularHoursAmount = GET_employeerateperday(NEW.EmployeeID, NEW.OrganizationID, NEW.`Date`) / 2;

ELSEIF NEW.RegularHoursWorked > 4 THEN
SET NEW.RegularHoursWorked = 8;
SET NEW.RegularHoursAmount = GET_employeerateperday(NEW.EmployeeID, NEW.OrganizationID, NEW.`Date`);

END IF;

END IF;

SET NEW.TotalHoursWorked = IFNULL(NEW.TotalHoursWorked,0);
SET NEW.OvertimeHoursWorked = IFNULL(NEW.OvertimeHoursWorked,0);
SET NEW.OvertimeHoursAmount = IFNULL(NEW.OvertimeHoursAmount,0);
SET NEW.UndertimeHours = IFNULL(NEW.UndertimeHours,0);
SET NEW.UndertimeHoursAmount = IFNULL(NEW.UndertimeHoursAmount,0);
SET NEW.NightDifferentialHours = IFNULL(NEW.NightDifferentialHours,0);
SET NEW.NightDiffHoursAmount = IFNULL(NEW.NightDiffHoursAmount,0);
SET NEW.NightDifferentialOTHours = IFNULL(NEW.NightDifferentialOTHours,0);
SET NEW.NightDiffOTHoursAmount = IFNULL(NEW.NightDiffOTHoursAmount,0);
SET NEW.HoursLate = IFNULL(NEW.HoursLate,0);
SET NEW.HoursLateAmount = IFNULL(NEW.HoursLateAmount,0);
SET NEW.TotalDayPay = IFNULL(NEW.TotalDayPay,0);

IF isRest_day = '0' THEN

SELECT EXISTS(SELECT RowID FROM employeeshift esh WHERE esh.EmployeeID=NEW.EmployeeID AND esh.OrganizationID=NEW.OrganizationID AND esh.RestDay='0' AND NEW.`Date` BETWEEN esh.EffectiveFrom AND esh.EffectiveTo LIMIT 1) INTO has_shift;
SET @fullshifthrs = 0.00;

IF has_shift = '1' AND isDateNotHoliday = '1' THEN

SELECT sh.DivisorToDailyRate,COMPUTE_TimeDifference(sh.TimeFrom,sh.TimeTo)
FROM employeeshift esh
INNER JOIN shift sh ON sh.RowID=esh.ShiftID
WHERE esh.EmployeeID=NEW.EmployeeID AND esh.OrganizationID=NEW.OrganizationID AND esh.RestDay='0' AND NEW.`Date` BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
LIMIT 1
INTO perfect_hrs,@fullshifthrs;

SET perfect_hrs = IFNULL(perfect_hrs,0);

IF perfect_hrs > 0 AND perfect_hrs NOT IN (3,4,5) THEN

SET perfect_hrs = perfect_hrs;

END IF;

SET absent_amount = @fullshifthrs * (GET_employeerateperday(NEW.EmployeeID, NEW.OrganizationID, NEW.`Date`) / perfect_hrs);

IF absent_amount < NEW.Absent THEN
SET NEW.Absent = 0.0;


ELSEIF (SUBSTRING_INDEX(absent_amount,'.',1) * 1) = (SUBSTRING_INDEX(NEW.HoursLateAmount,'.',1) * 1) THEN
SET NEW.Absent = 0.0;
ELSEIF (SUBSTRING_INDEX(absent_amount,'.',1) * 1) = (SUBSTRING_INDEX(NEW.UndertimeHoursAmount,'.',1) * 1) THEN
SET NEW.Absent = 0.0;
ELSE

IF NEW.TotalDayPay = 0 THEN

SET NEW.Absent = absent_amount;

ELSE

SET NEW.Absent = 0;

END IF;

END IF;

ELSE


IF isDateNotHoliday = '0' THEN

SET @calclegalholi = '0';
SET @calcspecholi = '0';

SET @daily_pay = 0.00;

SELECT (pr.PayType = 'Regular Holiday' AND e.CalcHoliday = '1' AND e.StartDate <= NEW.`Date`)
,(pr.PayType = 'Special Non-Working Holiday' AND e.CalcSpecialHoliday = '1' AND e.StartDate <= NEW.`Date`)
,e.EmployeeType

,GET_employeerateperday(NEW.EmployeeID, NEW.OrganizationID, NEW.`Date`) `Result`
FROM payrate pr
INNER JOIN employee e ON e.RowID=NEW.EmployeeID
INNER JOIN (SELECT * FROM employeesalary WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND NEW.`Date` BETWEEN EffectiveDateFrom AND IFNULL(EffectiveDateTo,NEW.`Date`) LIMIT 1) es ON es.RowID > 0
WHERE pr.RowID=NEW.PayRateID
INTO @calclegalholi
,@calcspecholi,emp_type
,@daily_pay;

IF (NEW.VacationLeaveHours + NEW.SickLeaveHours + NEW.MaternityLeaveHours + NEW.OtherLeaveHours) > 0 THEN
SET NEW.Absent = 0.0;
ELSEIF has_shift = '1' AND (NEW.VacationLeaveHours + NEW.SickLeaveHours + NEW.MaternityLeaveHours + NEW.OtherLeaveHours) = 0
AND NEW.TotalDayPay = 0
AND (@calclegalholi = 0 AND @calcspecholi = 0) THEN

SET NEW.Absent = @daily_pay;

ELSEIF has_shift = '1' AND (NEW.VacationLeaveHours + NEW.SickLeaveHours + NEW.MaternityLeaveHours + NEW.OtherLeaveHours) = 0
AND NEW.TotalDayPay = 0
AND @calclegalholi = 1 THEN

SET NEW.TotalDayPay = @daily_pay;
SET NEW.Absent = 0.0;

ELSEIF has_shift = '1' AND (NEW.VacationLeaveHours + NEW.SickLeaveHours + NEW.MaternityLeaveHours + NEW.OtherLeaveHours) = 0
AND NEW.TotalDayPay = 0 THEN

IF @calcspecholi = 1 THEN

IF emp_type = 'Daily' THEN

SET NEW.TotalDayPay = 0.0;
SET NEW.Absent = @daily_pay;

ELSEIF emp_type != 'Daily' THEN

SET NEW.TotalDayPay = @daily_pay;
SET NEW.Absent = 0.0;

END IF;

ELSE

SET NEW.TotalDayPay = 0.0;

SET NEW.Absent = @daily_pay;

END IF;

ELSE
SET NEW.Absent = 0.0;
END IF;
ELSE
SET NEW.Absent = IFNULL(NEW.Absent,0);

END IF;

END IF;

ELSE
SET NEW.Absent = 0.0;


END IF;
SET NEW.Absent = IFNULL(NEW.Absent,0);
IF isRest_day = '1' && NEW.EmployeeShiftID IS NOT NULL THEN
SET NEW.EmployeeShiftID = NULL;

END IF;

SELECT SUM(ea.AllowanceAmount) FROM employeeallowance ea WHERE ea.AllowanceFrequency='Daily' AND ea.TaxableFlag='1' AND ea.EmployeeID=NEW.EmployeeID AND ea.OrganizationID=NEW.OrganizationID AND NEW.`Date` BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate INTO TaxableDailyAllowanceAmount;
SET @rate_this_date = IFNULL((SELECT sh.DivisorToDailyRate - COMPUTE_TimeDifference(sh.BreakTimeFrom,sh.BreakTimeTo)
FROM employeeshift esh INNER JOIN shift sh ON sh.RowID=esh.ShiftID WHERE esh.RowID=NEW.EmployeeShiftID),0);
SET @daily_salary = GET_employeerateperday(NEW.EmployeeID, NEW.OrganizationID, NEW.`Date`);
SET @leave_hrs = (NEW.VacationLeaveHours + NEW.SickLeaveHours + NEW.MaternityLeaveHours + NEW.OtherLeaveHours);
SET NEW.TaxableDailyAllowance = (SELECT (IF(pr.PayType='Regular Day'
, IF(NEW.TotalDayPay > NEW.RegularHoursAmount AND @leave_hrs > 0, IF(NEW.RegularHoursAmount=0, NEW.TotalDayPay, NEW.RegularHoursAmount), IF(NEW.RegularHoursAmount > @daily_salary, @daily_salary, NEW.RegularHoursAmount))
, IF(pr.PayType='Special Non-Working Holiday' AND e.CalcSpecialHoliday = '1'
, IF(e.EmployeeType = 'Daily', (NEW.RegularHoursAmount / pr.`PayRate`), NEW.HolidayPayAmount)
, IF(pr.PayType='Special Non-Working Holiday' AND e.CalcSpecialHoliday = '0'
, IF(e.EmployeeType = 'Daily', NEW.RegularHoursAmount, NEW.HolidayPayAmount)
, IF(pr.PayType='Regular Holiday' AND e.CalcHoliday = '1'

, NEW.HolidayPayAmount + ((NEW.VacationLeaveHours + NEW.SickLeaveHours + NEW.MaternityLeaveHours + NEW.OtherLeaveHours) * (@daily_salary / @rate_this_date))
, 0)))) / @daily_salary) * TaxableDailyAllowanceAmount
FROM employee e
INNER JOIN payrate pr ON pr.RowID=NEW.PayRateID
WHERE e.RowID=NEW.EmployeeID AND e.OrganizationID=NEW.OrganizationID);

IF NEW.TaxableDailyAllowance IS NULL THEN
SET NEW.TaxableDailyAllowance = 0;
END IF;

SELECT `PayRate` FROM payrate pr WHERE pr.RowID=NEW.PayRateID INTO payrate_this_date;

SET NEW.HolidayPayAmount = (SELECT IF(NEW.TotalDayPay > 0 AND NEW.RegularHoursAmount = 0 AND e.CalcHoliday='1' AND LOCATE('Regular Holi',pr.PayType) > 0
, rate_this_date
, IF(e.CalcSpecialHoliday = '1' AND e.EmployeeType = 'Monthly' AND pr.PayType = 'Special Non-Working Holiday' AND isPresentInWorkingDaysPriorToThisDate = '1' AND NEW.TotalDayPay > 0 AND NEW.RegularHoursAmount = 0
, rate_this_date
, IF(e.CalcSpecialHoliday='1' AND LOCATE('Special',pr.PayType) > 0
, NEW.RegularHoursAmount - (NEW.RegularHoursAmount / payrate_this_date)
, IF(e.CalcHoliday='1' AND pr.PayType='Regular Holiday' AND NEW.TotalDayPay > 0 AND NEW.RegularHoursAmount > 0
, IF(NEW.TotalDayPay > NEW.RegularHoursAmount, NEW.RegularHoursAmount, NEW.TotalDayPay) / payrate_this_date, 0)
)))
FROM employee e
INNER JOIN payrate pr ON pr.RowID=NEW.PayRateID AND pr.PayType!='Regular Day'
WHERE e.RowID=NEW.EmployeeID AND e.OrganizationID=NEW.OrganizationID);

IF NEW.HolidayPayAmount IS NULL THEN
SET NEW.HolidayPayAmount = 0;
END IF;

IF NEW.Leavepayment < 0 THEN
SET NEW.Leavepayment = 0; SET NEW.TotalDayPay = 0; SET NEW.Absent = NEW.Leavepayment * -1;
ELSEIF NEW.Leavepayment > 0 THEN
SET NEW.Absent = 0;
END IF;



SET @ecalcnd = (SELECT e.CalcNightDiff FROM employee e WHERE e.RowID=NEW.EmployeeID AND e.OrganizationID=NEW.OrganizationID);

IF @ecalcnd = 1 THEN

    SET @og_ndtimefrom=CURTIME();
    SET @og_ndtimeto=CURTIME();
    SELECT og.NightDifferentialTimeFrom,og.NightDifferentialTimeTo FROM organization og WHERE og.RowID=NEW.OrganizationID INTO @og_ndtimefrom,@og_ndtimeto;

    SET @breaktimehrs = 0; SET @breakstarttime = NULL; SET @breakendtime = NULL; SET @isNightShift = 0; SET @divisortodailyrate = 0;

    SET @sh_tf = CURTIME(); SET @sh_tt = CURTIME();

    SET @sh_timefrom=CURTIME();
    SET @sh_timeto=CURTIME();

    SELECT COMPUTE_TimeDifference(sh.BreakTimeFrom,sh.BreakTimeTo), sh.BreakTimeFrom, sh.BreakTimeTo,esh.NightShift, sh.TimeFrom, sh.TimeTo, sh.DivisorToDailyRate FROM employeeshift esh LEFT JOIN shift sh ON sh.RowID=esh.ShiftID WHERE esh.RowID=NEW.EmployeeShiftID AND NEW.`Date` BETWEEN esh.EffectiveFrom AND esh.EffectiveTo LIMIT 1 INTO @breaktimehrs, @breakstarttime, @breakendtime, @isNightShift, @sh_timefrom, @sh_timeto, @divisortodailyrate;

    IF ADDTIME(TIMESTAMP(NEW.`Date`), @og_ndtimefrom) BETWEEN TIMESTAMP(NEW.`Date`) AND ADDTIME(TIMESTAMP(NEW.`Date`), '23:59:59') AND @isNightShift = 1 THEN

        SET @etd_tsin = NULL; SET @etd_tsout = NULL;
        SET @og_ndtsfrom = NULL; SET @og_ndtsto = NULL;

        SET @is_reg_shift_valid_for_ndiff=0;

        SELECT etd.TimeStampIn
        ,ADDTIME(TIMESTAMP(DATE_FORMAT(etd.TimeStampIn,@@date_format)), og.NightDifferentialTimeFrom)

        ,ADDTIME(TIMESTAMP(DATE_FORMAT(etd.TimeStampOut,@@date_format)), og.NightDifferentialTimeTo)
        ,etd.TimeStampOut

        ,(  CONCAT_DATETIME(NEW.`Date`
                                , IF(etd.TimeIn < @sh_timefrom
                                        , @sh_timefrom
                                        , etd.TimeIn))
        <= CONCAT_DATETIME(NEW.`Date`, @og_ndtimefrom)
        AND
            CONCAT_DATETIME(ADDDATE(NEW.`Date`, INTERVAL 1 DAY), @og_ndtimeto)
        >= CONCAT_DATETIME(ADDDATE(NEW.`Date`, INTERVAL 1 DAY)
                                , IF(etd.TimeOut > @sh_timeto
                                        , @sh_timeto
                                        , etd.TimeOut))
        AND (TIME_FORMAT(@sh_timefrom, '%p') = TIME_FORMAT(@og_ndtimefrom, '%p')
                AND TIME_FORMAT(@sh_timeto, '%p') = TIME_FORMAT(@og_ndtimeto, '%p'))
            ) `is_reg_shift_valid_for_ndiff`
        FROM employeetimeentrydetails etd
        INNER JOIN organization og ON og.RowID=etd.OrganizationID
        WHERE etd.EmployeeID=NEW.EmployeeID AND etd.OrganizationID=NEW.OrganizationID AND etd.`Date`=NEW.`Date`
        AND TIME_FORMAT(og.NightDifferentialTimeFrom,'%p')  =TIME_FORMAT(@sh_timefrom,'%p')
        AND TIME_FORMAT(og.NightDifferentialTimeTo,'%p')    =TIME_FORMAT(@sh_timeto,'%p')
        ORDER BY IFNULL(etd.LastUpd,etd.Created) DESC LIMIT 1
        INTO
            @etd_tsin, @og_ndtsfrom
            ,@og_ndtsto, @etd_tsout
            ,@is_reg_shift_valid_for_ndiff;

        SET @TimeOne = TIME_FORMAT(IF(@etd_tsin < @og_ndtsfrom, @og_ndtsfrom, @etd_tsin), @@time_format);
        SET @TimeTwo = TIME_FORMAT(IF(@og_ndtsto < @etd_tsout, @og_ndtsto, @etd_tsout), @@time_format);

        SET @breaktimehrs   =IF(TIME_FORMAT(@etd_tsin, @@time_format) BETWEEN @breakstarttime AND @breakendtime
                                    , COMPUTE_TimeDifference(TIME_FORMAT(@etd_tsin, @@time_format), @breakendtime)
                                    , IF(TIME_FORMAT(@etd_tsout, @@time_format) BETWEEN @breakstarttime AND @breakendtime
                                        , COMPUTE_TimeDifference(@breakstarttime, TIME_FORMAT(@etd_tsout, @@time_format))
                                        , @breaktimehrs));

        SET NEW.NightDifferentialHours  =NEW.NightDifferentialHours
                                                    + COMPUTE_TimeDifference(@TimeOne, @TimeTwo) - @breaktimehrs;

        SET @daily_pay = 0.00;

        IF emp_type = 'Daily' THEN
            SET @daily_pay = (SELECT BasicPay FROM employeesalary WHERE RowID=NEW.EmployeeSalaryID);

        ELSE
            SET @daily_pay = GET_employeerateperday(NEW.EmployeeID, NEW.OrganizationID, NEW.`Date`);

        END IF;

        SET NEW.NightDiffHoursAmount        =NEW.NightDiffHoursAmount
                                                    + (
                                                        (NEW.NightDifferentialHours / IFNULL(@divisortodailyrate,default_workhours_everyday))
                                                        * (SELECT @daily_pay * (pr.NightDifferentialRate MOD 1) FROM payrate pr WHERE pr.RowID=NEW.PayRateID)
                                                        );
        SET NEW.TotalDayPay =NEW.TotalDayPay + NEW.NightDiffHoursAmount;
    ELSE
        SET NEW.NightDifferentialHours = 0;

    END IF;

ELSE
    SET NEW.NightDifferentialHours = 0;

END IF;



END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
