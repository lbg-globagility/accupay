/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `COMPUTE_employeetimeentry_PREVIUS_BUILD`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `COMPUTE_employeetimeentry_PREVIUS_BUILD`(`etent_EmployeeID` INT, `etent_OrganizationID` INT, `etent_Date` DATE, `etent_CreatedBy` INT, `etent_LastUpdBy` INT, `EmployeeStartDate` DATE) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE etentID INT(11);

DECLARE shiftRowID INT(11);

DECLARE etent_RowID INT(11);

DECLARE timediffreghrswok TIME;

DECLARE HRSworkd DECIMAL(11,2);

DECLARE shifttimefrom TIME;

DECLARE shifttimeto TIME;

DECLARE undrtime DECIMAL(11,2);

DECLARE hrslate DECIMAL(11,2);

DECLARE minslate DECIMAL(11,2);

DECLARE etent_EmployeeShiftID DECIMAL(11,2);

DECLARE etent_EmployeeSalaryID DECIMAL(11,2);

DECLARE etent_RegularHoursWorked DECIMAL(11,2);

DECLARE the_RegularHoursWorked DECIMAL(11,2);

DECLARE final_RegularHoursWorked DECIMAL(11,2);

DECLARE etent_OvertimeHoursWorked DECIMAL(11,2);

DECLARE eot_OvertimeHoursWorked DECIMAL(11,2);

DECLARE etent_UndertimeHours DECIMAL(11,2) DEFAULT 0;

DECLARE etent_NightDifferentialHours DECIMAL(11,2);

DECLARE etent_NightDifferentialOTHours DECIMAL(11,2);

DECLARE etent_HoursLate DECIMAL(11,2);

DECLARE etent_VacationLeaveHours DECIMAL(11,2);

DECLARE etent_SickLeaveHours DECIMAL(11,2);

DECLARE etent_MaternityLeaveHours DECIMAL(11,2);

DECLARE etent_OtherLeaveHours DECIMAL(11,2);

DECLARE etent_TotalDayPay DECIMAL(11,2);

DECLARE empBasicPay DECIMAL(11,2);

DECLARE rateperhour DECIMAL(11,3);

DECLARE regularpay DECIMAL(12,2);
DECLARE otpay DECIMAL(12,2);
DECLARE nightpay DECIMAL(12,2);
DECLARE nightotpay DECIMAL(12,2);

DECLARE dailyrate DECIMAL(11,2);

DECLARE prate DECIMAL(11,2);
DECLARE otrate DECIMAL(11,2);
DECLARE nightrate DECIMAL(11,2);
DECLARE nightotrate DECIMAL(11,2);
DECLARE rest_dayrate DECIMAL(11,2);
DECLARE restday_otrate DECIMAL(11,2);

DECLARE prateID INT(11);

DECLARE isleave INT(1);

DECLARE isNighShift INT(1);

DECLARE isRestDay INT(1);

DECLARE isprorated INT(1);

DECLARE isnotstarting INT(1);


DECLARE otoverride INT(1);

DECLARE calc_holiday CHAR(1);

DECLARE calc_spclholiday CHAR(1);

DECLARE calc_nightdiff CHAR(1);

DECLARE calc_nightdiffOT CHAR(1);

DECLARE calc_restday CHAR(1);

DECLARE calc_restdayOT CHAR(1);


DECLARE endboundday INT(11);

DECLARE PayFreqID INT(11) DEFAULT 1;

DECLARE numofweekthisyear INT(11) DEFAULT 53;

DECLARE emptype VARCHAR(50);

DECLARE org_nightdifftimefrom TIME;

DECLARE org_nightdifftimeto TIME;

DECLARE org_nightshfttimefrom TIME;

DECLARE org_nightshfttimeto TIME;

DECLARE etent_TotalHoursWorked DECIMAL(11,2);
DECLARE etent_RegularHoursAmount DECIMAL(11,2);
DECLARE etent_OvertimeHoursAmount DECIMAL(11,2);
DECLARE etent_UndertimeHoursAmount DECIMAL(11,2);
DECLARE etent_NightDiffHoursAmount DECIMAL(11,2);
DECLARE etent_NightDiffOTHoursAmount DECIMAL(11,2);
DECLARE etent_HoursLateAmount DECIMAL(11,2);


DECLARE emp_TimeIn TIME;

DECLARE emp_TimeOut TIME;

DECLARE emp_TrueTimeOut TIME;


DECLARE minnumday DECIMAL(11,2);

DECLARE tardilate DECIMAL(11,2);

DECLARE org_workdaysofyear INT(11);

DECLARE pr_DayBefore DATE;

DECLARE DayBeforeTotDayPay DECIMAL(11,2);

DECLARE has_filedleave INT(1);


DECLARE yes_true INT(1);

DECLARE is_absent INT(1);

DECLARE eeSalary DECIMAL(11,2) DEFAULT 0.0;

DECLARE pay_type TEXT;

DECLARE employee_dailyAllowance DECIMAL(11,2) DEFAULT 0;

DECLARE employee_dailyWageSalary DECIMAL(11,2) DEFAULT 0;

DECLARE pay_period_date_to DATE;

DECLARE paid_monthly_restday DECIMAL(11,2) DEFAULT 0;

DECLARE otstartingtime TIME;

DECLARE otendingtime TIME;

DECLARE regular_timehours DECIMAL(11,6) DEFAULT 0;


SELECT IFNULL(DayBefore,etent_Date),PayType FROM payrate WHERE `Date`=etent_Date AND OrganizationID=etent_OrganizationID INTO pr_DayBefore,pay_type;

SELECT EXISTS(SELECT RowID FROM employeeleave WHERE OrganizationID=etent_OrganizationID AND EmployeeID=etent_EmployeeID AND pr_DayBefore BETWEEN LeaveStartDate AND LeaveEndDate LIMIT 1) INTO has_filedleave;



    SELECT IF(DATEDIFF(etent_Date,StartDate) < 0, 1, COALESCE(NewEmployeeFlag,0)),IF(DATEDIFF(etent_Date,StartDate) < 0, 1, 0),COALESCE(EmployeeType,''),COALESCE(OvertimeOverride,0), CalcHoliday, CalcSpecialHoliday, CalcNightDiff, CalcNightDiffOT, CalcRestDay, CalcRestDayOT
 FROM employee WHERE RowID=etent_EmployeeID AND OrganizationID=etent_OrganizationID INTO isprorated,isnotstarting,emptype,otoverride, calc_holiday, calc_spclholiday, calc_nightdiff, calc_nightdiffOT, calc_restday, calc_restdayOT;


    SELECT IFNULL(RestDay,'0') FROM employeeshift WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND pr_DayBefore BETWEEN EffectiveFrom AND COALESCE(EffectiveTo,EffectiveFrom) AND DATEDIFF(pr_DayBefore,EffectiveFrom) >= 0 AND COALESCE(RestDay,0)='1' ORDER BY DATEDIFF(pr_DayBefore,EffectiveFrom) LIMIT 1 INTO isRestDay;

    IF isRestDay IS NULL THEN

        SET pr_DayBefore = NULL;

        SELECT CAST(EXISTS(SELECT RowID FROM employeetimeentrydetails WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND `Date`=etent_Date LIMIT 1) AS INT) 'CharResult' INTO isRestDay;

        IF isRestDay = '1' THEN
            SELECT TimeIn, TimeOut FROM employeetimeentrydetails WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND `Date`=etent_Date LIMIT 1 INTO shifttimefrom,shifttimeto;

            SET isRestDay = '1';

        END IF;

    END IF;

SELECT IF(IFNULL(TotalHoursWorked,0) = 0 OR IFNULL(TotalDayPay,0) = 0, 1, 0) FROM employeetimeentry WHERE OrganizationID=etent_OrganizationID AND EmployeeID=etent_EmployeeID AND Date=pr_DayBefore INTO is_absent;

IF is_absent IS NULL THEN
    SET is_absent = 0;
END IF;

IF is_absent = 0 THEN
    SET has_filedleave = 1;
END IF;

SET yes_true = '1';

IF ISNULL(pr_DayBefore) = 0 AND has_filedleave = 0 AND emptype IN ('Daily','Monthly') THEN

SET yes_true = '0';

    SELECT EXISTS(SELECT RowID FROM employeetimeentry WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND Date=pr_DayBefore) INTO DayBeforeTotDayPay;

    IF DayBeforeTotDayPay = 1 THEN

        SELECT TotalHoursWorked FROM employeetimeentry WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND Date=pr_DayBefore INTO DayBeforeTotDayPay;

        SELECT RowID,COALESCE(RestDay,0) FROM employeeshift WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND etent_Date BETWEEN EffectiveFrom AND COALESCE(EffectiveTo,EffectiveFrom) AND DATEDIFF(etent_Date,EffectiveFrom) >= 0 AND COALESCE(RestDay,0)=1 ORDER BY DATEDIFF(etent_Date,EffectiveFrom) LIMIT 1 INTO etent_EmployeeShiftID,isRestDay;

        SET isRestDay = IFNULL(isRestDay,0);

        IF isRestDay = 1 THEN

            SET DayBeforeTotDayPay = 0;

        END IF;

        IF IFNULL(DayBeforeTotDayPay,0) = 0 THEN

            IF isRestDay = 1 THEN

                SELECT GET_employeerateperday(etent_EmployeeID, etent_OrganizationID, etent_Date) INTO paid_monthly_restday;

                SELECT RowID FROM employeetimeentry WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND Date=etent_Date INTO etent_RowID;

                SELECT RowID,BasicPay,Salary FROM employeesalary WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND etent_Date BETWEEN DATE(COALESCE(EffectiveDateFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveDateTo,etent_Date)) AND DATEDIFF(etent_Date,EffectiveDateFrom) >= 0 ORDER BY DATEDIFF(DATE_FORMAT(etent_Date,'%Y-%m-%d'),EffectiveDateFrom) LIMIT 1 INTO etent_EmployeeSalaryID,empBasicPay,eeSalary;

                SELECT RowID FROM payrate WHERE Date=etent_Date AND OrganizationID=etent_OrganizationID LIMIT 1 INTO prateID;

                INSERT INTO employeetimeentry
                (
                    RowID
                    ,OrganizationID
                    ,Created
                    ,CreatedBy
                    ,Date
                    ,EmployeeShiftID
                    ,EmployeeID
                    ,EmployeeSalaryID
                    ,EmployeeFixedSalaryFlag
                    ,RegularHoursWorked
                    ,RegularHoursAmount
                    ,TotalHoursWorked
                    ,OvertimeHoursWorked
                    ,OvertimeHoursAmount
                    ,UndertimeHours
                    ,UndertimeHoursAmount
                    ,NightDifferentialHours
                    ,NightDiffHoursAmount
                    ,NightDifferentialOTHours
                    ,NightDiffOTHoursAmount
                    ,HoursLate
                    ,HoursLateAmount
                    ,LateFlag
                    ,PayRateID
                    ,VacationLeaveHours
                    ,SickLeaveHours
                    ,MaternityLeaveHours
                    ,OtherLeaveHours
                    ,TotalDayPay
                ) VALUES (
                    etent_RowID
                    ,etent_OrganizationID
                    ,CURRENT_TIMESTAMP()
                    ,etent_CreatedBy
                    ,etent_Date
                    ,etent_EmployeeShiftID
                    ,etent_EmployeeID
                    ,etent_EmployeeSalaryID
                    ,'0'
                    , 0
                    ,paid_monthly_restday
                    ,0
                    ,0
                    ,0
                    ,0
                    ,0
                    ,0
                    ,0
                    ,0
                    ,0
                    ,0
                    ,0
                    ,'0'
                    ,prateID
                    ,0
                    ,0
                    ,0
                    ,0
                    ,paid_monthly_restday
                ) ON
                DUPLICATE
                KEY
                UPDATE
                    LastUpd=CURRENT_TIMESTAMP()
                    ,LastUpdBy=etent_CreatedBy
                    ,EmployeeShiftID=etent_EmployeeShiftID
                    ,EmployeeSalaryID=etent_EmployeeSalaryID
                    ,RegularHoursAmount=paid_monthly_restday
                    ,UndertimeHours=0
                    ,HoursLate=0
                    ,HoursLateAmount=0
                    ,TotalDayPay=paid_monthly_restday;SELECT @@Identity AS ID INTO etentID;


            ELSE

                IF isRestDay = 0 THEN

                    SELECT RowID,COALESCE(RestDay,0) FROM employeeshift WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND pr_DayBefore BETWEEN EffectiveFrom AND COALESCE(EffectiveTo,EffectiveFrom) AND DATEDIFF(etent_Date,EffectiveFrom) >= 0 AND COALESCE(RestDay,0)=1 ORDER BY DATEDIFF(etent_Date,EffectiveFrom) LIMIT 1 INTO etent_EmployeeShiftID,isRestDay;

                    SET isRestDay = IFNULL(isRestDay,0);

                    IF isRestDay = 1 THEN

                        SELECT `COMPUTE_emptimeentryRestDayHoliday`(etent_EmployeeID,etent_OrganizationID,etent_Date,etent_CreatedBy,etent_LastUpdBy,EmployeeStartDate) INTO etentID;

                    ELSE

                        SELECT `COMPUTE_DayBeforeTimeEntry`(etent_EmployeeID,etent_OrganizationID,pr_DayBefore,etent_CreatedBy,etent_LastUpdBy,EmployeeStartDate,'1') INTO etentID;

                        IF pay_type IN ('Regular Holiday','Special Non-Working Holiday') THEN

                            SELECT `COMPUTE_DayBeforeTimeEntry`(etent_EmployeeID,etent_OrganizationID,etent_Date,etent_CreatedBy,etent_LastUpdBy,EmployeeStartDate,'0') INTO etentID;
                        ELSE

                            SELECT `COMPUTE_DayBeforeTimeEntry`(etent_EmployeeID,etent_OrganizationID,etent_Date,etent_CreatedBy,etent_LastUpdBy,EmployeeStartDate,'1') INTO etentID;

                        END IF;

                    END IF;





                END IF;

            END IF;







        ELSE


            SELECT `COMPUTE_DayBeforeTimeEntry`(etent_EmployeeID,etent_OrganizationID,pr_DayBefore,etent_CreatedBy,etent_LastUpdBy,EmployeeStartDate,'0') INTO etentID;


            SELECT `COMPUTE_DayBeforeTimeEntry`(etent_EmployeeID,etent_OrganizationID,etent_Date,etent_CreatedBy,etent_LastUpdBy,EmployeeStartDate,'0') INTO etentID;

        END IF;


    END IF;

ELSE


    SELECT EXISTS(SELECT RowID FROM employeeleave WHERE OrganizationID=etent_OrganizationID AND EmployeeID=etent_EmployeeID AND etent_Date BETWEEN LeaveStartDate AND LeaveEndDate LIMIT 1) INTO has_filedleave;

    IF has_filedleave = 0 THEN












            SELECT IF(DAY(etent_Date) <= 15,15,DAY(LAST_DAY(etent_Date))) INTO endboundday;

        SELECT RowID FROM employeetimeentry WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND Date=etent_Date INTO etent_RowID;

        SELECT SUM(COALESCE(VacationLeaveHours,0)),SUM(COALESCE(SickLeaveHours,0)),SUM(COALESCE(MaternityLeaveHours,0)),SUM(COALESCE(OtherLeaveHours,0)) FROM employeetimeentry WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND Date=etent_Date INTO etent_VacationLeaveHours,etent_SickLeaveHours,etent_MaternityLeaveHours,etent_OtherLeaveHours;

        SET isleave = IF(COALESCE(etent_VacationLeaveHours,0) != 0 OR COALESCE(etent_SickLeaveHours,0) != 0 OR COALESCE(etent_MaternityLeaveHours,0) != 0 OR COALESCE(etent_OtherLeaveHours,0) != 0, 1, 0);

         IF emptype = 'Monthly' THEN

            SELECT RowID,ShiftID FROM employeeshift WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND etent_Date BETWEEN DATE(COALESCE(EffectiveFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) AND DATEDIFF(etent_Date,EffectiveFrom) >= 0 ORDER BY DATEDIFF(DATE_FORMAT(etent_Date,'%Y-%m-%d'),EffectiveFrom) LIMIT 1 INTO etent_EmployeeShiftID,shiftRowID;

         ELSE

            SELECT RowID,ShiftID FROM employeeshift WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND etent_Date BETWEEN DATE(COALESCE(EffectiveFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) AND DATEDIFF(etent_Date,EffectiveFrom) >= 0 AND COALESCE(RestDay,0)=0 ORDER BY DATEDIFF(DATE_FORMAT(etent_Date,'%Y-%m-%d'),EffectiveFrom) LIMIT 1 INTO etent_EmployeeShiftID,shiftRowID;

         END IF;







        SELECT RowID,BasicPay,Salary FROM employeesalary WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND etent_Date BETWEEN DATE(COALESCE(EffectiveDateFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveDateTo,etent_Date)) AND DATEDIFF(etent_Date,EffectiveDateFrom) >= 0 ORDER BY DATEDIFF(DATE_FORMAT(etent_Date,'%Y-%m-%d'),EffectiveDateFrom) LIMIT 1 INTO etent_EmployeeSalaryID,empBasicPay,eeSalary;



        SELECT IF(DATEDIFF(etent_Date,StartDate) < 0, 1, COALESCE(NewEmployeeFlag,0)),IF(DATEDIFF(etent_Date,StartDate) < 0, 1, 0),COALESCE(EmployeeType,''),COALESCE(OvertimeOverride,0) FROM employee WHERE RowID=etent_EmployeeID AND OrganizationID=etent_OrganizationID INTO isprorated,isnotstarting,emptype,otoverride;





        SELECT SUBSTRING_INDEX(TIMEDIFF(TimeFrom,IF(TimeFrom>TimeTo,ADDTIME(TimeTo,'24:00:00'),TimeTo)),'-',-1) FROM shift WHERE RowID=shiftRowID INTO timediffreghrswok;

        SELECT NightDifferentialTimeFrom,NightDifferentialTimeTo,NightShiftTimeFrom,NightShiftTimeTo FROM organization WHERE RowID=etent_OrganizationID INTO org_nightdifftimefrom,org_nightdifftimeto,org_nightshfttimefrom,org_nightshfttimeto;

        SELECT GET_empworkdaysperyear(etent_EmployeeID) INTO org_workdaysofyear;

        SELECT TimeFrom,TimeTo FROM shift WHERE RowID=shiftRowID INTO shifttimefrom,shifttimeto;

        IF emptype IN ('Monthly','Fixed') THEN

            SELECT (DAYOFWEEK(etent_Date) = e.DayOfRest) FROM employee e WHERE e.RowID=etent_EmployeeID INTO isRestDay;

            IF isRestDay = 1 THEN

                SELECT CAST(EXISTS(SELECT RowID FROM employeetimeentrydetails WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND `Date`=etent_Date LIMIT 1) AS INT) 'CharResult' INTO isRestDay;

                IF isRestDay = '1' THEN
                    SELECT TimeIn, TimeOut FROM employeetimeentrydetails WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND `Date`=etent_Date LIMIT 1 INTO shifttimefrom,shifttimeto;

                ELSE
                    SET shifttimefrom = '12:00';
                    SET shifttimeto = '12:00';

                END IF;

                SET isRestDay = 1;

            END IF;

        ELSE

            SELECT RestDay FROM employeeshift WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND etent_Date BETWEEN EffectiveFrom AND COALESCE(EffectiveTo,EffectiveFrom) AND DATEDIFF(etent_Date,EffectiveFrom) >= 0 AND COALESCE(RestDay,0)='1' ORDER BY DATEDIFF(etent_Date,EffectiveFrom) LIMIT 1 INTO isRestDay;



            IF isRestDay IS NULL AND (SELECT CAST((DAYOFWEEK(etent_Date) = e.DayOfRest) AS CHAR) 'CharResult' FROM employee e WHERE e.RowID=etent_EmployeeID) = '1' THEN

                SELECT CAST(EXISTS(SELECT RowID FROM employeetimeentrydetails WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND `Date`=etent_Date LIMIT 1) AS CHAR) 'CharResult' INTO isRestDay;

                IF isRestDay = '1' THEN



                    SET shifttimefrom = '12:00';
                    SET shifttimeto = '12:00';








                END IF;



            ELSE

                SELECT CAST((DAYOFWEEK(etent_Date) = e.DayOfRest) AS CHAR) 'CharResult' FROM employee e WHERE e.RowID=etent_EmployeeID INTO isRestDay;

            END IF;

        END IF;

        SELECT
        OTStartTime
        ,OTEndTime
        FROM employeeovertime
        WHERE EmployeeID=etent_EmployeeID
        AND OrganizationID=etent_OrganizationID
        AND etent_Date
        BETWEEN OTStartDate
        AND COALESCE(OTEndDate,OTStartDate)
        AND OTStatus='Approved'
        ORDER BY DATEDIFF(etent_Date,COALESCE(OTEndDate,OTStartDate))
        LIMIT 1
        INTO otstartingtime
              ,otendingtime;

        IF DATE_FORMAT(shifttimefrom,'%p') = 'PM'
            AND DATE_FORMAT(shifttimeto,'%p') = 'AM' THEN



            SELECT
            IF(DATE_FORMAT(TimeOut,'%p') = 'AM' AND DATE_FORMAT(TimeIn,'%p') = 'PM'
            ,((TIME_TO_SEC(TIMEDIFF(ADDTIME(IF(otendingtime IS NULL, IF(TimeOut > shifttimeto, shifttimeto, TimeOut), IF(otendingtime BETWEEN shifttimeto AND TimeOut, SUBTIME(shifttimeto, '00:01'), TimeOut)), '24:00'),GRACE_PERIOD(TimeIn, shifttimefrom))) / 60) / 60)
            ,((TIME_TO_SEC(TIMEDIFF(TimeOut,TimeIn)) / 60) / 60))
            ,GRACE_PERIOD(TimeIn, shifttimefrom)
            ,IF(otendingtime IS NULL, IF(TimeOut > shifttimeto, shifttimeto, TimeOut), IF(otendingtime BETWEEN shifttimeto AND TimeOut, SUBTIME(shifttimeto, '00:01'), IF(TimeOut > shifttimeto, shifttimeto, TimeOut)))
            ,TimeOut
            FROM employeetimeentrydetails
            WHERE EmployeeID=etent_EmployeeID
            AND OrganizationID=etent_OrganizationID
            AND Date=etent_Date
            GROUP BY Created
            ORDER BY TIME_FORMAT(TimeIn,'%H:%m:%s'),Created DESC
            LIMIT 1
            INTO HRSworkd
                    ,emp_TimeIn
                    ,emp_TimeOut
                    ,emp_TrueTimeOut;



            SET timediffreghrswok = TIMEDIFF(ADDTIME(shifttimeto,'24:00'),shifttimefrom);

            SET regular_timehours = ((TIME_TO_SEC(TIMEDIFF(ADDTIME(emp_TimeOut,'24:00'),emp_TimeIn)) / 60) / 60);

        ELSE


            SELECT
            0
            ,GRACE_PERIOD(TimeIn, shifttimefrom)
            ,IF(otendingtime IS NULL, IF(TimeOut > shifttimeto, shifttimeto, TimeOut), IF(otendingtime BETWEEN shifttimeto AND TimeOut, SUBTIME(shifttimeto, '00:01'), IF(TimeOut > shifttimeto, shifttimeto, TimeOut)))
            ,TimeOut
            FROM employeetimeentrydetails
            WHERE EmployeeID=etent_EmployeeID
            AND OrganizationID=etent_OrganizationID
            AND Date=etent_Date
            GROUP BY Created
            ORDER BY TIME_FORMAT(TimeIn,'%H:%m:%s'),Created DESC
            LIMIT 1
            INTO HRSworkd
                    ,emp_TimeIn
                    ,emp_TimeOut
                    ,emp_TrueTimeOut;

            SET timediffreghrswok = TIMEDIFF(shifttimeto,shifttimefrom);

            SET HRSworkd = ((TIME_TO_SEC(TIMEDIFF(emp_TimeOut,emp_TimeIn)) / 60) / 60);

            SET regular_timehours = ((TIME_TO_SEC(TIMEDIFF(IF(otendingtime IS NULL, emp_TimeOut, shifttimeto),emp_TimeIn)) / 60) / 60);

            SET regular_timehours = regular_timehours - 1;

        END IF;

        SET etent_TotalHoursWorked = IF(IFNULL(HRSworkd,0) >= 8, (IFNULL(HRSworkd,0) - 1), IFNULL(HRSworkd,0));

        SELECT ((TIME_TO_SEC(timediffreghrswok) / 60) / 60) INTO the_RegularHoursWorked;


        IF emptype = 'Monthly' AND isRestDay = 1 THEN

            SET the_RegularHoursWorked = 8;

        END IF;


        SET the_RegularHoursWorked = IF(the_RegularHoursWorked > 8, (the_RegularHoursWorked - 1), the_RegularHoursWorked);







        SET isNighShift = IF(((TIME_TO_SEC(TIMEDIFF(org_nightshfttimefrom,shifttimefrom)) /60) / 60) <= 0,1,0);


            IF isNighShift = 1 THEN
                SET etent_RegularHoursWorked = 0;

                SELECT SUM(((TIME_TO_SEC(TIMEDIFF(IF(TimeIn>TimeOut,IF(TimeOut>shifttimeto,ADDTIME(shifttimeto,'24:00:00'),ADDTIME(TimeOut,'24:00:00')),IF(TimeOut>shifttimeto,shifttimeto,TimeOut)),TimeIn)) / 60) / 60)) FROM employeetimeentrydetails WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND Date=etent_Date AND shifttimefrom <= TimeIn AND shifttimeto <= TimeOut GROUP BY Created ORDER BY TIME_FORMAT(TimeIn,'%H:%m:%s') ASC,Created DESC LIMIT 1 INTO etent_NightDifferentialHours;

                SET etent_NightDifferentialHours = etent_TotalHoursWorked;

            ELSE

                SET etent_RegularHoursWorked = the_RegularHoursWorked;

                SET etent_NightDifferentialHours = 0;

            END IF;







        SELECT (TIME_TO_SEC(TIMEDIFF(TimeOut,shifttimeto)) / 60) / 60 FROM employeetimeentrydetails WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND Date=etent_Date ORDER BY RowID DESC LIMIT 1 INTO etent_UndertimeHours;

        IF isRestDay = '1' THEN
            SET etent_UndertimeHours = 0.0;
        ElSE
            SET etent_UndertimeHours = IF(COALESCE(etent_UndertimeHours,0) > 0, 0, etent_UndertimeHours * -1);
        END IF;











        IF otoverride = 1 THEN





            SELECT COMPUTE_employeeovertimeofthisdate(etent_EmployeeID,etent_OrganizationID,etent_Date, IF(TIME_FORMAT(emp_TrueTimeOut,'%p') = 'AM' AND HOUR(emp_TrueTimeOut) != 24, ADDTIME(emp_TrueTimeOut, '24:00'), emp_TrueTimeOut), '0') INTO etent_OvertimeHoursWorked;

            IF etent_OvertimeHoursWorked IS NULL THEN
                SET etent_OvertimeHoursWorked = 0;
            END IF;

            SELECT COMPUTE_employeeovertimeofthisdate(etent_EmployeeID,etent_OrganizationID,etent_Date,IF(TIME_FORMAT(emp_TrueTimeOut,'%p') = 'AM' AND HOUR(emp_TrueTimeOut) != 24, ADDTIME(emp_TrueTimeOut, '24:00'), emp_TrueTimeOut), '1') INTO etent_NightDifferentialOTHours;

            IF etent_NightDifferentialOTHours IS NULL THEN
                SET etent_NightDifferentialOTHours = 0;
            END IF;

        ELSE
            SET etent_OvertimeHoursWorked = 0;

            SET etent_NightDifferentialOTHours = 0;

        END IF;





        SELECT IF(TIMEDIFF(TimeIn,ADDTIME(shifttimefrom, '00:30:00')) < 0,0,(TIME_TO_SEC(TIMEDIFF(TimeIn,shifttimefrom)) / 60) / 60) FROM employeetimeentrydetails WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND Date=etent_Date AND  TIMEDIFF(shifttimefrom,TimeIn) <= 0 GROUP BY Created ORDER BY ((TIME_TO_SEC(TIMEDIFF(shifttimefrom,TimeIn)) / 60) / 60) DESC,Created DESC LIMIT 1 INTO etent_HoursLate;

        SET etent_HoursLate = IFNULL(etent_HoursLate,0);

        SELECT ((TIME_TO_SEC(TIMEDIFF(GRACE_PERIOD(TimeIn, shifttimefrom), shifttimefrom)) / 60) / 60) FROM employeetimeentrydetails WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND Date=etent_Date GROUP BY Created ORDER BY ((TIME_TO_SEC(TIMEDIFF(shifttimefrom,TimeIn)) / 60) / 60) DESC,Created DESC LIMIT 1 INTO tardilate;

        SET tardilate = ABS(IFNULL(tardilate,0));




        SELECT RowID FROM payrate WHERE Date=etent_Date AND OrganizationID=etent_OrganizationID LIMIT 1 INTO prateID;





        IF isNighShift = 1 THEN



                IF etent_TotalHoursWorked > the_RegularHoursWorked THEN
                    SET final_RegularHoursWorked = the_RegularHoursWorked;
                ELSE
                    IF etent_HoursLate = 0 THEN

                        SET final_RegularHoursWorked = etent_TotalHoursWorked + tardilate;

                    ELSE

                        IF etent_TotalHoursWorked > the_RegularHoursWorked THEN

                            SET final_RegularHoursWorked = HRSworkd - the_RegularHoursWorked;

                            SET final_RegularHoursWorked = HRSworkd - final_RegularHoursWorked;

                            SET final_RegularHoursWorked = final_RegularHoursWorked - etent_HoursLate;

                        ELSE

                            SET final_RegularHoursWorked = HRSworkd - etent_HoursLate;

                        END IF;

                    END IF;

                END IF;



        ELSE




                IF etent_TotalHoursWorked > the_RegularHoursWorked THEN
                    SET final_RegularHoursWorked = the_RegularHoursWorked;
                ELSE
                    IF etent_HoursLate = 0 THEN

                        SET final_RegularHoursWorked = etent_TotalHoursWorked + tardilate;

                    ELSE

                        IF etent_TotalHoursWorked > the_RegularHoursWorked THEN

                            SET final_RegularHoursWorked = HRSworkd - the_RegularHoursWorked;

                            SET final_RegularHoursWorked = HRSworkd - final_RegularHoursWorked;

                            SET final_RegularHoursWorked = final_RegularHoursWorked - etent_HoursLate;



                        ELSE

                            IF HRSworkd > the_RegularHoursWorked THEN

                                SET final_RegularHoursWorked = the_RegularHoursWorked - etent_HoursLate;

                            ELSE

                                SET final_RegularHoursWorked = HRSworkd - etent_HoursLate;

                            END IF;



                        END IF;

                    END IF;

                END IF;



        END IF;



        SELECT
            IF(PayType = 'Special Non-Working Holiday', IF(calc_spclholiday = 'Y', PayRate, 1), IF(PayType = 'Regular Holiday', IF(calc_holiday = 'Y', PayRate, 1), PayRate))
            ,OvertimeRate
            ,IF(calc_nightdiff = 'Y', NightDifferentialRate, 1)
            ,IF(calc_nightdiffOT = 'Y', NightDifferentialOTRate, 1)
            ,IF(calc_restday = 'Y', RestDayRate, 1)
            ,IF(calc_restdayOT = 'Y', RestDayOvertimeRate, 1)
        FROM payrate
        WHERE RowID=prateID
        INTO prate
            ,otrate
            ,nightrate
            ,nightotrate
            ,rest_dayrate
            ,restday_otrate;

        SELECT PayFrequencyID,WEEKOFYEAR(LAST_DAY(CONCAT(YEAR(etent_Date),'-12-01'))) FROM organization WHERE RowID=etent_OrganizationID INTO PayFreqID,numofweekthisyear;


        IF DAY(etent_Date) <= 15 THEN

            SET minnumday = 15;

        ELSE

            SET minnumday = DAY(LAST_DAY(etent_Date)) - 15;

        END IF;

IF org_workdaysofyear BETWEEN 260 AND 270 THEN
    SELECT COUNT(d.DateValue) FROM dates d WHERE YEAR(d.DateValue)=YEAR(etent_Date) AND DAYOFWEEK(d.DateValue) NOT IN (6,7) INTO org_workdaysofyear;
ELSEIF org_workdaysofyear BETWEEN 314 AND 320 THEN
    SELECT COUNT(d.DateValue) FROM dates d WHERE YEAR(d.DateValue)=YEAR(etent_Date) AND DAYOFWEEK(d.DateValue)!=7 INTO org_workdaysofyear;
END IF;

            IF emptype IN ('Fixed','Monthly') THEN

                IF PayFreqID = 1 THEN

                        SET dailyrate = eeSalary / (org_workdaysofyear / 12);



                ELSEIF PayFreqID = 2 THEN
                        SET dailyrate = (empBasicPay * 12) / org_workdaysofyear;


                ELSEIF PayFreqID = 3 THEN
                        SET dailyrate = empBasicPay;

                ELSEIF PayFreqID = 4 THEN
                        SET dailyrate = (empBasicPay * numofweekthisyear) / org_workdaysofyear;


                END IF;



                SET rateperhour = dailyrate / IF(the_RegularHoursWorked = 0, 8, the_RegularHoursWorked);

            ELSEIF emptype = 'Daily' THEN

                SET dailyrate = empBasicPay;

                IF the_RegularHoursWorked = 0 THEN

                    SET rateperhour = dailyrate / 8;

                ELSE

                    SET rateperhour = dailyrate / the_RegularHoursWorked;

                END IF;





            ELSEIF emptype = 'Hourly' THEN
                SET rateperhour = empBasicPay;

            END IF;

        IF isRestDay = 0 THEN

            IF isNighShift = 1 THEN

                SET regularpay = (rateperhour) * etent_NightDifferentialHours;



                SET nightpay = (rateperhour * (nightrate - 1)) * etent_NightDifferentialHours;

                IF the_RegularHoursWorked <= etent_NightDifferentialHours THEN

                    SET regularpay = (dailyrate);



                    SET nightpay = (dailyrate * (nightrate - 1));

                END IF;





                SET otpay = (rateperhour * otrate) * COALESCE(etent_OvertimeHoursWorked,0);

                SET nightotpay = (rateperhour * nightotrate) * COALESCE(etent_NightDifferentialOTHours,0);

            ELSE

                SET regularpay = (rateperhour * prate) * (regular_timehours);

                IF (IFNULL(tardilate,0) <= 0.50
                    AND the_RegularHoursWorked <= etent_TotalHoursWorked)
                    OR emptype = 'Fixed' THEN

                    SET regularpay = (dailyrate * prate);

                END IF;





                SET otpay = (rateperhour * otrate) * COALESCE(etent_OvertimeHoursWorked,0);

                SET nightpay= 0;

                SET nightotpay= 0;

            END IF;



        ELSE

            IF isNighShift = 1 THEN

                SET regularpay = (rateperhour * nightrate * (rest_dayrate - 1)) * etent_NightDifferentialHours;

                SET nightpay = (rateperhour * nightrate * (rest_dayrate - 1)) * etent_NightDifferentialHours;

                IF the_RegularHoursWorked <= etent_NightDifferentialHours THEN

                    SET regularpay = (dailyrate * nightrate * (rest_dayrate - 1));

                    SET nightpay = (dailyrate * nightrate * (rest_dayrate - 1));

                END IF;




                SET otpay = (rateperhour * restday_otrate) * COALESCE(etent_OvertimeHoursWorked,0);

                SET nightotpay = (rateperhour * nightotrate * (restday_otrate - 1)) * COALESCE(etent_NightDifferentialOTHours,0);

            ELSE


                SET regularpay = (rateperhour * etent_TotalHoursWorked) * ((prate + rest_dayrate) - 1);

                IF (IFNULL(tardilate,0) <= 0.50
                    AND the_RegularHoursWorked <= etent_TotalHoursWorked)
                    AND emptype = 'Fixed' THEN

                    SET regularpay = (rateperhour * etent_TotalHoursWorked);

                ELSEIF emptype = 'Monthly' AND isRestDay = 1 AND org_workdaysofyear >= 360 THEN

                    SET regularpay = dailyrate;

                END IF;



                SET otpay = (rateperhour * restday_otrate) * COALESCE(etent_OvertimeHoursWorked,0);

                SET nightpay= 0;

                SET nightotpay= 0;

            END IF;



        END IF;

        IF isNighShift = 1 THEN

            SET etent_TotalDayPay = (regularpay + IFNULL(nightpay,0) + IFNULL(nightotpay,0));

        ELSE


            SET etent_TotalDayPay = (regularpay + IFNULL(otpay,0));

        END IF;


        SET etent_TotalDayPay = (IFNULL(regularpay,0) + IFNULL(nightpay,0) + IFNULL(nightotpay,0) + IFNULL(otpay,0));


        SET etent_HoursLateAmount = COALESCE(etent_HoursLate,0) * rateperhour;

        SET etent_UndertimeHoursAmount = COALESCE(etent_UndertimeHours,0) * rateperhour;


        IF pay_type IN ('Regular Holiday','Special Non-Working Holiday') AND IFNULL(etent_TotalDayPay,0) = 0 THEN

            IF DAY(etent_Date) <= 15 THEN

                SET pay_period_date_to = DATE_FORMAT(etent_Date, '%Y-%m-15');

            ELSE

                SET pay_period_date_to = LAST_DAY(etent_Date);

            END IF;

            SELECT GET_employeerateperhour(etent_EmployeeID, etent_OrganizationID, pay_period_date_to) INTO employee_dailyWageSalary;



            SET etent_TotalDayPay = dailyrate;

            SET regularpay = dailyrate;

        END IF;

IF isNighShift = 1 THEN

    SET etent_TotalHoursWorked = regular_timehours + etent_NightDifferentialOTHours;

ELSE

    SET etent_TotalHoursWorked = regular_timehours + etent_NightDifferentialHours + etent_OvertimeHoursWorked;

END IF;










            IF etent_Date < EmployeeStartDate THEN

                INSERT INTO employeetimeentry
                (
                    RowID
                    ,OrganizationID
                    ,Created
                    ,CreatedBy
                    ,Date
                    ,EmployeeShiftID
                    ,EmployeeID
                    ,EmployeeSalaryID
                    ,EmployeeFixedSalaryFlag
                    ,TotalHoursWorked
                    ,RegularHoursWorked
                    ,RegularHoursAmount
                    ,OvertimeHoursWorked
                    ,OvertimeHoursAmount
                    ,UndertimeHours
                    ,UndertimeHoursAmount
                    ,NightDifferentialHours
                    ,NightDiffHoursAmount
                    ,NightDifferentialOTHours
                    ,NightDiffOTHoursAmount
                    ,HoursLate
                    ,HoursLateAmount
                    ,LateFlag
                    ,PayRateID
                    ,VacationLeaveHours
                    ,SickLeaveHours
                    ,MaternityLeaveHours
                    ,OtherLeaveHours
                    ,TotalDayPay
                ) VALUES (
                    etent_RowID
                    ,etent_OrganizationID
                    ,CURRENT_TIMESTAMP()
                    ,etent_CreatedBy
                    ,etent_Date
                    ,etent_EmployeeShiftID
                    ,etent_EmployeeID
                    ,etent_EmployeeSalaryID
                    ,IF(emptype = 'Fixed',1,0)
                    ,0
                    ,0
                    ,0
                    ,0
                    ,0
                    ,0
                    ,0
                    ,0
                    ,0
                    ,0
                    ,0
                    ,0
                    ,0
                    ,0
                    ,prateID
                    ,0
                    ,0
                    ,0
                    ,0
                    ,0
                ) ON
                DUPLICATE
                KEY
                UPDATE
                    LastUpd=CURRENT_TIMESTAMP()
                    ,LastUpdBy=etent_LastUpdBy
                    ,Date=etent_Date
                    ,EmployeeShiftID=etent_EmployeeShiftID
                    ,EmployeeSalaryID=etent_EmployeeSalaryID
                    ,EmployeeFixedSalaryFlag=IF(emptype = 'Fixed',1,0)
                    ,RegularHoursWorked=COALESCE(etent_RegularHoursWorked,0)
                    ,OvertimeHoursWorked=0
                    ,UndertimeHours=COALESCE(etent_RegularHoursWorked,0)
                    ,NightDifferentialHours=0
                    ,NightDifferentialOTHours=0
                    ,HoursLate=0
                    ,LateFlag=0
                    ,VacationLeaveHours=0
                    ,SickLeaveHours=0
                    ,MaternityLeaveHours=0
                    ,OtherLeaveHours=0
                    ,TotalDayPay=0;



            ELSE

                INSERT INTO employeetimeentry
                (
                    RowID
                    ,OrganizationID
                    ,Created
                    ,CreatedBy
                    ,Date
                    ,EmployeeShiftID
                    ,EmployeeID
                    ,EmployeeSalaryID
                    ,EmployeeFixedSalaryFlag
                    ,TotalHoursWorked
                    ,RegularHoursWorked
                    ,RegularHoursAmount
                    ,OvertimeHoursWorked
                    ,OvertimeHoursAmount
                    ,UndertimeHours
                    ,UndertimeHoursAmount
                    ,NightDifferentialHours
                    ,NightDiffHoursAmount
                    ,NightDifferentialOTHours
                    ,NightDiffOTHoursAmount
                    ,HoursLate
                    ,HoursLateAmount
                    ,LateFlag
                    ,PayRateID
                    ,VacationLeaveHours
                    ,SickLeaveHours
                    ,MaternityLeaveHours
                    ,OtherLeaveHours
                    ,TotalDayPay
                ) VALUES (
                    etent_RowID
                    ,etent_OrganizationID
                    ,CURRENT_TIMESTAMP()
                    ,etent_CreatedBy
                    ,etent_Date
                    ,etent_EmployeeShiftID
                    ,etent_EmployeeID
                    ,etent_EmployeeSalaryID
                    ,IF(emptype = 'Fixed',1,0)
                    ,etent_TotalHoursWorked
                    ,the_RegularHoursWorked
                    ,COALESCE(regularpay,IF(emptype = 'Fixed',dailyrate,0))
                    ,etent_OvertimeHoursWorked
                    ,otpay
                    ,IF(emptype = 'Fixed', 0, etent_UndertimeHours)
                    ,IF(emptype = 'Fixed', 0, etent_UndertimeHoursAmount)
                    ,IF(isNighShift = 1,final_RegularHoursWorked,0)
                    ,IF(emptype = 'Fixed' AND isNighShift = 1,dailyrate,COALESCE(nightpay,0))
                    ,etent_NightDifferentialOTHours
                    ,nightotpay
                    ,IF(isleave = 1, 0, tardilate)
                    ,IFNULL(etent_HoursLateAmount,0)
                    ,IF(COALESCE(etent_HoursLate,0)>0,1,0)
                    ,prateID
                    ,IFNULL(etent_VacationLeaveHours,0)
                    ,IFNULL(etent_SickLeaveHours,0)
                    ,IFNULL(etent_MaternityLeaveHours,0)
                    ,IFNULL(etent_OtherLeaveHours,0)
                    ,IF(emptype = 'Fixed' AND etent_TotalDayPay = 0,dailyrate,etent_TotalDayPay)
                ) ON
                DUPLICATE
                KEY
                UPDATE
                    LastUpd=CURRENT_TIMESTAMP()
                    ,LastUpdBy=etent_LastUpdBy
                    ,Date=etent_Date
                    ,EmployeeShiftID=etent_EmployeeShiftID
                    ,EmployeeSalaryID=etent_EmployeeSalaryID
                    ,EmployeeFixedSalaryFlag=IF(emptype = 'Fixed',1,0)
                    ,TotalHoursWorked=etent_TotalHoursWorked
                    ,RegularHoursWorked=the_RegularHoursWorked
                    ,RegularHoursAmount=COALESCE(regularpay,IF(emptype = 'Fixed',dailyrate,0))
                    ,OvertimeHoursWorked=etent_OvertimeHoursWorked
                    ,OvertimeHoursAmount=otpay
                    ,UndertimeHours=IF(emptype = 'Fixed', 0, etent_UndertimeHours)
                    ,UndertimeHoursAmount=IF(emptype = 'Fixed', 0, etent_UndertimeHoursAmount)
                    ,NightDifferentialHours=IF(isNighShift = 1,COALESCE(final_RegularHoursWorked,IF(emptype = 'Fixed',8.00,0)),0)
                    ,NightDiffHoursAmount=COALESCE(nightpay,IF(emptype = 'Fixed',dailyrate,0))
                    ,NightDifferentialOTHours=etent_NightDifferentialOTHours
                    ,NightDiffOTHoursAmount=nightotpay
                    ,HoursLate=IF(isleave = 1, 0, IFNULL(tardilate,0))
                    ,HoursLateAmount=IFNULL(etent_HoursLateAmount,0)
                    ,LateFlag=IF(COALESCE(HoursLate,0)>0,1,0)
                    ,VacationLeaveHours=IFNULL(etent_VacationLeaveHours,0)
                    ,SickLeaveHours=IFNULL(etent_SickLeaveHours,0)
                    ,MaternityLeaveHours=IFNULL(etent_MaternityLeaveHours,0)
                    ,OtherLeaveHours=IFNULL(etent_OtherLeaveHours,0)
                    ,TotalDayPay=IF(emptype = 'Fixed' AND etent_TotalDayPay = 0,dailyrate,etent_TotalDayPay);SELECT @@Identity AS ID INTO etentID;










            END IF;



                IF etent_Date = EmployeeStartDate THEN

                    UPDATE employee SET
                    NewEmployeeFlag=0
                    ,LastUpdBy=etent_LastUpdBy
                    WHERE RowID=etent_EmployeeID
                    AND OrganizationID=etent_OrganizationID
                    AND NewEmployeeFlag=1;

                END IF;













    END IF;

END IF;


    RETURN etentID;






END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
