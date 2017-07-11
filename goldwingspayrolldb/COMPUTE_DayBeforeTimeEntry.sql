/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `COMPUTE_DayBeforeTimeEntry`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `COMPUTE_DayBeforeTimeEntry`(`etent_EmployeeID` INT, `etent_OrganizationID` INT, `etent_Date` DATE, `etent_CreatedBy` INT, `etent_LastUpdBy` INT, `EmployeeStartDate` DATE, `AbsentThisTimeEntry` CHAR(1)) RETURNS int(11)
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
DECLARE restdayrate DECIMAL(11,2);
DECLARE restdayotrate DECIMAL(11,2);

DECLARE prateID INT(11);

DECLARE isleave INT(1);

DECLARE isNighShift INT(1);

DECLARE isRestDay INT(1);

DECLARE isprorated INT(1);

DECLARE isnotstarting INT(1);

DECLARE otoverride INT(1);

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

DECLARE minnumday DECIMAL(11,2);

DECLARE tardilate DECIMAL(11,2);

DECLARE org_workdaysofyear INT(11);

DECLARE pay_type TEXT;




        SELECT IF(DAY(etent_Date) <= 15,15,DAY(LAST_DAY(etent_Date))) INTO endboundday;

    SELECT RowID FROM employeetimeentry WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND Date=etent_Date INTO etent_RowID;

    SELECT SUM(COALESCE(VacationLeaveHours,0)),SUM(COALESCE(SickLeaveHours,0)),SUM(COALESCE(MaternityLeaveHours,0)),SUM(COALESCE(OtherLeaveHours,0)) FROM employeetimeentry WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND Date=etent_Date INTO etent_VacationLeaveHours,etent_SickLeaveHours,etent_MaternityLeaveHours,etent_OtherLeaveHours;

    SET isleave = IF(COALESCE(etent_VacationLeaveHours,0) != 0 OR COALESCE(etent_SickLeaveHours,0) != 0 OR COALESCE(etent_MaternityLeaveHours,0) != 0 OR COALESCE(etent_OtherLeaveHours,0) != 0, 1, 0);

    SELECT RowID,ShiftID FROM employeeshift WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND etent_Date BETWEEN DATE(COALESCE(EffectiveFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveTo,ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH))) AND DATEDIFF(etent_Date,EffectiveFrom) >= 0 AND COALESCE(RestDay,0)=0 ORDER BY DATEDIFF(DATE_FORMAT(etent_Date,'%Y-%m-%d'),EffectiveFrom) LIMIT 1 INTO etent_EmployeeShiftID,shiftRowID;



    SELECT COALESCE(RestDay,0) FROM employeeshift WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND etent_Date BETWEEN EffectiveFrom AND COALESCE(EffectiveTo,EffectiveFrom) AND DATEDIFF(etent_Date,EffectiveFrom) >= 0 AND COALESCE(RestDay,0)=1 ORDER BY DATEDIFF(etent_Date,EffectiveFrom) LIMIT 1 INTO isRestDay;



    SET isRestDay = COALESCE(isRestDay,0);



    SELECT RowID,BasicPay FROM employeesalary WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND etent_Date BETWEEN DATE(COALESCE(EffectiveDateFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveDateTo,etent_Date)) AND DATEDIFF(etent_Date,EffectiveDateFrom) >= 0 ORDER BY DATEDIFF(DATE_FORMAT(etent_Date,'%Y-%m-%d'),EffectiveDateFrom) LIMIT 1 INTO etent_EmployeeSalaryID,empBasicPay;



    SELECT IF(DATEDIFF(etent_Date,StartDate) < 0, 1, COALESCE(NewEmployeeFlag,0)),IF(DATEDIFF(etent_Date,StartDate) < 0, 1, 0),COALESCE(EmployeeType,''),COALESCE(OvertimeOverride,0) FROM employee WHERE RowID=etent_EmployeeID AND OrganizationID=etent_OrganizationID INTO isprorated,isnotstarting,emptype,otoverride;





    SELECT SUBSTRING_INDEX(TIMEDIFF(TimeFrom,IF(TimeFrom>TimeTo,ADDTIME(TimeTo,'24:00:00'),TimeTo)),'-',-1) FROM shift WHERE RowID=shiftRowID INTO timediffreghrswok;

    SELECT NightDifferentialTimeFrom,NightDifferentialTimeTo,NightShiftTimeFrom,NightShiftTimeTo,WorkDaysPerYear FROM organization WHERE RowID=etent_OrganizationID INTO org_nightdifftimefrom,org_nightdifftimeto,org_nightshfttimefrom,org_nightshfttimeto,org_workdaysofyear;

    SELECT GET_empworkdaysperyear(etent_EmployeeID) INTO org_workdaysofyear;

    SELECT TimeFrom,TimeTo FROM shift WHERE RowID=shiftRowID INTO shifttimefrom,shifttimeto;



    SELECT ((TIME_TO_SEC(timediffreghrswok) / 60) / 60) INTO the_RegularHoursWorked;

    SET the_RegularHoursWorked = IF(the_RegularHoursWorked > 8, (the_RegularHoursWorked - 1), the_RegularHoursWorked);

    SET isNighShift = IF(((TIME_TO_SEC(TIMEDIFF(org_nightshfttimefrom,shifttimefrom)) /60) / 60) <= 0,1,0);

    SELECT SUM((TIME_TO_SEC(TIMEDIFF(TIME_FORMAT(IF(TimeIn>TimeOut,ADDTIME(TimeOut, '24:00:00'),TimeOut),'%H:%i:%s'), TIME_FORMAT(IF(TimeIn<=shifttimefrom,shifttimefrom,TimeIn),'%H:%i:%s'))) / 60) / 60) FROM employeetimeentrydetails WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND Date=etent_Date GROUP BY Created ORDER BY TIME_FORMAT(TimeIn,'%H:%m:%s'),Created DESC LIMIT 1 INTO HRSworkd;

    SET etent_TotalHoursWorked = IF(COALESCE(HRSworkd,0) >= 8, (COALESCE(HRSworkd,0) - 1), COALESCE(HRSworkd,0));

        IF isNighShift = 1 THEN
            SET etent_RegularHoursWorked = 0;

            SELECT SUM(((TIME_TO_SEC(TIMEDIFF(IF(TimeIn>TimeOut,IF(TimeOut>shifttimeto,ADDTIME(shifttimeto,'24:00:00'),ADDTIME(TimeOut,'24:00:00')),IF(TimeOut>shifttimeto,shifttimeto,TimeOut)),TimeIn)) / 60) / 60)) FROM employeetimeentrydetails WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND Date=etent_Date AND shifttimefrom <= TimeIn AND shifttimeto <= TimeOut GROUP BY Created ORDER BY TIME_FORMAT(TimeIn,'%H:%m:%s') ASC,Created DESC LIMIT 1 INTO etent_NightDifferentialHours;

            SET etent_NightDifferentialHours = COALESCE(etent_NightDifferentialHours,0);

        ELSE

            SET etent_RegularHoursWorked = the_RegularHoursWorked;

            SET etent_NightDifferentialHours = 0;

        END IF;







    SELECT (TIME_TO_SEC(TIMEDIFF(TimeOut,shifttimeto)) / 60) / 60 FROM employeetimeentrydetails WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND Date=etent_Date ORDER BY RowID DESC LIMIT 1 INTO etent_UndertimeHours;

    SET etent_UndertimeHours = IF(COALESCE(etent_UndertimeHours,0) < 0, etent_UndertimeHours * -1, COALESCE(etent_UndertimeHours,0));













    IF otoverride = 1 THEN

        SELECT SUM(((TIME_TO_SEC(TIMEDIFF(OTEndTime,OTStartTime)) / 60) / 60)) 'OT' FROM employeeovertime WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND etent_Date BETWEEN OTStartDate AND COALESCE(OTEndDate,OTStartDate) AND DATEDIFF(etent_Date,COALESCE(OTEndDate,OTStartDate)) >= 0 AND OTStatus='Approved' ORDER BY DATEDIFF(etent_Date,COALESCE(OTEndDate,OTStartDate)) LIMIT 1 INTO eot_OvertimeHoursWorked;

        IF isNighShift = 1 THEN
            SET etent_OvertimeHoursWorked = 0;

        ELSE
            SET etent_OvertimeHoursWorked = IF(COALESCE(eot_OvertimeHoursWorked,0) <= 0,0,COALESCE(eot_OvertimeHoursWorked,0));

        END IF;

    ELSE
        SET etent_OvertimeHoursWorked = 0;
    END IF;



    IF otoverride = 1 THEN



        IF isNighShift = 1 THEN
            SET etent_NightDifferentialOTHours = COALESCE(eot_OvertimeHoursWorked,0);

        ELSE
            SET etent_NightDifferentialOTHours = 0;

        END IF;

    ELSE
        SET etent_NightDifferentialOTHours = 0;
    END IF;



    SELECT IF(TIMEDIFF(TimeIn,ADDTIME(shifttimefrom, '00:31:00')) < 0,0,(TIME_TO_SEC(TIMEDIFF(TimeIn,shifttimefrom)) / 60) / 60) FROM employeetimeentrydetails WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND Date=etent_Date AND  TIMEDIFF(shifttimefrom,TimeIn) <= 0 GROUP BY Created ORDER BY ((TIME_TO_SEC(TIMEDIFF(shifttimefrom,TimeIn)) / 60) / 60) DESC,Created DESC LIMIT 1 INTO etent_HoursLate;

    SELECT IF(TIMEDIFF(TimeIn,shifttimefrom) < 0,0,(TIME_TO_SEC(TIMEDIFF(TimeIn,shifttimefrom)) / 60) / 60) FROM employeetimeentrydetails WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND Date=etent_Date AND  TIMEDIFF(shifttimefrom,TimeIn) <= 0 GROUP BY Created ORDER BY ((TIME_TO_SEC(TIMEDIFF(shifttimefrom,TimeIn)) / 60) / 60) DESC,Created DESC LIMIT 1 INTO tardilate;






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


                        SET final_RegularHoursWorked = etent_TotalHoursWorked;

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

                        SET final_RegularHoursWorked = etent_TotalHoursWorked;



                    END IF;

                END IF;

            END IF;



    END IF;

    SELECT PayRate,OvertimeRate,NightDifferentialRate,NightDifferentialOTRate,RestDayRate,RestDayOvertimeRate FROM payrate WHERE RowID=prateID INTO prate,otrate,nightrate,nightotrate,restdayrate,restdayotrate;

    SELECT PayFrequencyID,WEEKOFYEAR(LAST_DAY(CONCAT(YEAR(etent_Date),'-12-01'))) FROM organization WHERE RowID=etent_OrganizationID INTO PayFreqID,numofweekthisyear;


    IF DAY(etent_Date) <= 15 THEN

        SET minnumday = 15;

    ELSE

        SET minnumday = DAY(LAST_DAY(etent_Date)) - 15;

    END IF;


        IF emptype IN ('Fixed','Monthly') THEN

            IF PayFreqID = 1 THEN
                    SET dailyrate = (empBasicPay * 24) / org_workdaysofyear;



            ELSEIF PayFreqID = 2 THEN
                    SET dailyrate = (empBasicPay * 12) / org_workdaysofyear;

            ELSEIF PayFreqID = 3 THEN
                    SET dailyrate = empBasicPay;

            ELSEIF PayFreqID = 4 THEN
                    SET dailyrate = (empBasicPay * numofweekthisyear) / org_workdaysofyear;

            END IF;

            IF isNighShift = 1 THEN
                SET rateperhour = dailyrate / 8;

            ELSE
                SET rateperhour = dailyrate / 8;

            END IF;

        ELSEIF emptype = 'Daily' THEN

            IF isNighShift = 1 THEN
                SET dailyrate = empBasicPay;

                SET rateperhour = dailyrate / 8;

            ELSE
                SET dailyrate = empBasicPay;

                SET rateperhour = dailyrate / 8;

            END IF;

        ELSEIF emptype = 'Hourly' THEN
            SET rateperhour = empBasicPay;

        END IF;



    IF isRestDay = 0 THEN

        IF isNighShift = 1 THEN

                SET regularpay = (rateperhour) * etent_NightDifferentialHours;



                SET nightpay = (rateperhour * (nightrate- 1)) * etent_NightDifferentialHours;

                IF the_RegularHoursWorked <= etent_NightDifferentialHours THEN

                    SET regularpay = (dailyrate);



                    SET nightpay = (dailyrate * (nightrate - 1));

                END IF;







                SET nightotpay = (rateperhour * nightotrate) * COALESCE(etent_NightDifferentialOTHours,0);

        ELSE

                SET regularpay = (rateperhour * prate) * (final_RegularHoursWorked);







                SET otpay = (rateperhour * otrate) * COALESCE(etent_OvertimeHoursWorked,0);

                IF emptype = 'Fixed' THEN

                    SET regularpay = dailyrate * prate;

                END IF;


                SET nightpay= 0;

                SET nightotpay= 0;

        END IF;



    ELSE

        IF isNighShift = 1 THEN

                SET regularpay = (rateperhour * nightrate * (restdayrate - 1)) * etent_NightDifferentialHours;

                SET nightpay = (rateperhour * nightrate * (restdayrate - 1)) * etent_NightDifferentialHours;

                IF the_RegularHoursWorked <= etent_NightDifferentialHours THEN

                    SET regularpay = (dailyrate * nightrate * (restdayrate - 1));

                    SET nightpay = (dailyrate * nightrate * (restdayrate - 1));

                END IF;






                SET nightotpay = (rateperhour * nightotrate * (restdayotrate - 1)) * COALESCE(etent_NightDifferentialOTHours,0);

            ELSE

                SET regularpay = (rateperhour * prate * (restdayrate - 1)) * etent_TotalHoursWorked;







                SET otpay = (rateperhour * restdayotrate) * COALESCE(etent_OvertimeHoursWorked,0);

                IF emptype = 'Fixed' THEN

                    SET regularpay = (dailyrate * prate) * (restdayrate - 1);

                END IF;

                SET nightpay= 0;

                SET nightotpay= 0;

            END IF;




    END IF;




        IF isNighShift = 1 THEN

            SET etent_TotalDayPay = (regularpay + IFNULL(nightpay,0) + IFNULL(nightotpay,0));

        ELSE

            SET etent_TotalDayPay = (regularpay + IFNULL(otpay,0));

        END IF;


        SET etent_HoursLateAmount = COALESCE(etent_HoursLate,0) * rateperhour;

        SET etent_UndertimeHoursAmount = COALESCE(etent_UndertimeHours,0) * rateperhour;

        SELECT PayType FROM payrate WHERE `Date`=etent_Date AND OrganizationID=etent_OrganizationID INTO pay_type;

        IF pay_type IN ('Regular Holiday','Special Non-Working Holiday') AND IFNULL(etent_TotalDayPay,0) = 0 THEN

                SET etent_TotalDayPay = dailyrate;

                SET regularpay = dailyrate;

            END IF;









        IF isprorated = 1 AND isnotstarting = 1 THEN

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
                ,PayRateID=prateID
                ,TotalDayPay=0;



        ELSE

            IF AbsentThisTimeEntry = '1' THEN

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
                    ,the_RegularHoursWorked
                    ,0
                    ,etent_OvertimeHoursWorked
                    ,0
                    ,etent_UndertimeHours
                    ,COALESCE(dailyrate,IF(emptype = 'Fixed',dailyrate,0))
                    ,0
                    ,COALESCE(nightpay,IF(emptype = 'Fixed',dailyrate,0))
                    ,etent_NightDifferentialOTHours
                    ,nightotpay
                    ,IF(isleave = 1, 0, tardilate)
                    ,etent_HoursLateAmount
                    ,IF(COALESCE(etent_HoursLate,0)>0,1,0)
                    ,prateID
                    ,etent_VacationLeaveHours
                    ,etent_SickLeaveHours
                    ,etent_MaternityLeaveHours
                    ,etent_OtherLeaveHours
                    ,0
                ) ON
                DUPLICATE
                KEY
                UPDATE
                    LastUpd=CURRENT_TIMESTAMP()
                    ,LastUpdBy=etent_LastUpdBy
                    ,EmployeeShiftID=etent_EmployeeShiftID
                    ,EmployeeSalaryID=etent_EmployeeSalaryID
                    ,EmployeeFixedSalaryFlag=IF(emptype = 'Fixed',1,0)
                    ,TotalHoursWorked=0
                    ,RegularHoursWorked=8
                    ,RegularHoursAmount=0
                    ,OvertimeHoursWorked=0
                    ,OvertimeHoursAmount=0
                    ,UndertimeHours=8
                    ,UndertimeHoursAmount=0
                    ,NightDifferentialHours=0
                    ,NightDiffHoursAmount=0
                    ,NightDifferentialOTHours=0
                    ,NightDiffOTHoursAmount=0
                    ,HoursLate=IF(isleave = 1, 0, tardilate)
                    ,HoursLateAmount=etent_HoursLateAmount
                    ,LateFlag=IF(COALESCE(HoursLate,0)>0,1,0)
                    ,VacationLeaveHours=etent_VacationLeaveHours
                    ,SickLeaveHours=etent_SickLeaveHours
                    ,MaternityLeaveHours=etent_MaternityLeaveHours
                    ,OtherLeaveHours=etent_OtherLeaveHours
                    ,PayRateID=prateID
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
                    ,COALESCE(dailyrate,IF(emptype = 'Fixed',dailyrate,0))
                    ,etent_OvertimeHoursWorked
                    ,otpay
                    ,etent_UndertimeHours
                    ,etent_UndertimeHoursAmount
                    ,IF(isNighShift = 1,final_RegularHoursWorked,0)
                    ,COALESCE(nightpay,IF(emptype = 'Fixed',dailyrate,0))
                    ,etent_NightDifferentialOTHours
                    ,nightotpay
                    ,IF(isleave = 1, 0, tardilate)
                    ,etent_HoursLateAmount
                    ,IF(COALESCE(etent_HoursLate,0)>0,1,0)
                    ,prateID
                    ,etent_VacationLeaveHours
                    ,etent_SickLeaveHours
                    ,etent_MaternityLeaveHours
                    ,etent_OtherLeaveHours
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
                    ,RegularHoursAmount=COALESCE(dailyrate,IF(emptype = 'Fixed',dailyrate,0))
                    ,OvertimeHoursWorked=etent_OvertimeHoursWorked
                    ,OvertimeHoursAmount=otpay
                    ,UndertimeHours=etent_UndertimeHours
                    ,UndertimeHoursAmount=etent_UndertimeHoursAmount
                    ,NightDifferentialHours=IF(isNighShift = 1,COALESCE(final_RegularHoursWorked,IF(emptype = 'Fixed',8.00,0)),0)
                    ,NightDiffHoursAmount=COALESCE(nightpay,IF(emptype = 'Fixed',dailyrate,0))
                    ,NightDifferentialOTHours=etent_NightDifferentialOTHours
                    ,NightDiffOTHoursAmount=nightotpay
                    ,HoursLate=IF(isleave = 1, 0, tardilate)
                    ,HoursLateAmount=etent_HoursLateAmount
                    ,LateFlag=IF(COALESCE(HoursLate,0)>0,1,0)
                    ,VacationLeaveHours=etent_VacationLeaveHours
                    ,SickLeaveHours=etent_SickLeaveHours
                    ,MaternityLeaveHours=etent_MaternityLeaveHours
                    ,OtherLeaveHours=etent_OtherLeaveHours
                    ,PayRateID=prateID
                    ,TotalDayPay=IF(emptype = 'Fixed' AND etent_TotalDayPay = 0,dailyrate,etent_TotalDayPay);


            END IF;



        END IF;



            IF etent_Date = EmployeeStartDate THEN

                UPDATE employee SET
                NewEmployeeFlag=0
                ,LastUpdBy=etent_LastUpdBy
                WHERE RowID=etent_EmployeeID
                AND OrganizationID=etent_OrganizationID
                AND NewEmployeeFlag=1;

            END IF;

            SELECT RowID FROM employeetimeentry WHERE EmployeeID=etent_EmployeeID AND OrganizationID=etent_OrganizationID AND `Date`=etent_Date ORDER BY RowID DESC LIMIT 1 INTO etentID;






    RETURN etentID;


END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
