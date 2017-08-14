/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTINS_employeeleave_then_employeetimeentry`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_employeeleave_then_employeetimeentry` AFTER INSERT ON `employeeleave` FOR EACH ROW BEGIN

DECLARE shift_RowID INT(11);

DECLARE empshiftRowID INT(11);

DECLARE numofdaysofleave INT(11);

DECLARE looper INT(11) DEFAULT 0;

DECLARE reghrsworkd TIME;

DECLARE numhrsworkd DECIMAL(11,6);

DECLARE leavetype VARCHAR(50);

DECLARE dateloop DATE;

DECLARE etentRowID INT(11);

DECLARE viewID INT(11);


DECLARE dailypayamount DECIMAL(11,6);

DECLARE hourlypayamount DECIMAL(11,6);

DECLARE esal_RowID INT(11);


DECLARE emp_vacabal DECIMAL(11,6);

DECLARE emp_sickbal DECIMAL(11,6);

DECLARE emp_materbal DECIMAL(11,6);

DECLARE emp_othrbal DECIMAL(11,6);


DECLARE etd_timein TIME;

DECLARE etd_timeout TIME;


DECLARE sh_timefrom TIME;

DECLARE sh_timeto TIME;

DECLARE ete_HrsUnder DECIMAL(11,6);

DECLARE anyint INT(11);


DECLARE extra_regularhours DECIMAL(11,6);

DECLARE extra_regularamount DECIMAL(11,6);

DECLARE e_EmpType TEXT;

DECLARE empBasicPay DECIMAL(11,6);

DECLARE emp_sal DECIMAL(11,6);


DECLARE late_hrs DECIMAL(11,6);

DECLARE under_hrs DECIMAL(11,6);




SELECT NEW.LeaveType INTO leavetype;

IF LOCATE('aternity',leavetype) > 0 THEN

    SET @dutyhours = 0.0;
    SET @correct_date = CURDATE();

    SELECT EXISTS(
        SELECT
            INSERTUPDATE_employeetimeentry(
                NULL,
                NEW.OrganizationID,
                NEW.CreatedBy,
                d.DateValue,
                esh.RowID,
                NEW.EmployeeID,
                es.RowID,
                '0',
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                '0',
                pr.RowID,
                0,
                0,
                IF(
                    sh.DutyHoursCount > 5.0,
                    (sh.DutyHoursCount - 1.0),
                    sh.DutyHoursCount
                ),
                0,
                0,
                0,
                NULL
            )
        FROM dates d
        INNER JOIN payrate pr
        ON pr.OrganizationID=NEW.OrganizationID AND
            pr.`Date`=d.DateValue
        LEFT JOIN (
            SELECT *
            FROM employeeshift
            WHERE OrganizationID = NEW.OrganizationID AND
                EmployeeID=NEW.EmployeeID
        ) esh
        ON d.DateValue BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
        LEFT JOIN (
            SELECT
                *,
                IFNULL(COMPUTE_TimeDifference(TimeFrom, TimeTo), 0.0) AS DutyHoursCount
            FROM shift
        ) sh
        ON sh.RowID=esh.ShiftID
        LEFT JOIN (
            SELECT *
            FROM employeesalary
            WHERE EmployeeID=NEW.EmployeeID AND
                OrganizationID=NEW.OrganizationID AND
                (EffectiveDateFrom >= NEW.LeaveStartDate OR IFNULL(EffectiveDateTo, NEW.LeaveEndDate) >= NEW.LeaveStartDate) AND
                (EffectiveDateFrom <= NEW.LeaveEndDate OR IFNULL(EffectiveDateTo, NEW.LeaveEndDate) <= NEW.LeaveEndDate)
            LIMIT 1
        ) es
        ON d.DateValue BETWEEN es.EffectiveDateFrom AND IFNULL(es.EffectiveDateTo, NEW.LeaveEndDate)
        WHERE d.DateValue BETWEEN NEW.LeaveStartDate AND NEW.LeaveEndDate
        ORDER BY d.DateValue
    )
    INTO anyint;

ELSE

    SELECT IF(
        DATEDIFF(NEW.LeaveStartDate,NEW.LeaveEndDate) < 0,
        DATEDIFF(NEW.LeaveStartDate,NEW.LeaveEndDate) * -1,
        DATEDIFF(NEW.LeaveStartDate,NEW.LeaveEndDate)
    )
    INTO numofdaysofleave;

    IF TIME_FORMAT(NEW.LeaveStartTime, '%p') = 'PM'
        AND TIME_FORMAT(NEW.LeaveEndTime, '%p') = 'AM' THEN

        SET reghrsworkd = TIMEDIFF(ADDDATE(NEW.LeaveEndTime, '24:00'), NEW.LeaveStartTime);
    ELSE
        SET reghrsworkd = TIMEDIFF(NEW.LeaveEndTime, NEW.LeaveStartTime);
    END IF;

    SET numhrsworkd = COMPUTE_TimeDifference(NEW.LeaveStartTime, NEW.LeaveEndTime);


    SELECT
        e.LeaveBalance,
        e.SickLeaveBalance,
        e.MaternityLeaveBalance,
        e.OtherLeaveBalance,
        e.EmployeeType
    FROM employee e
    WHERE e.RowID = NEW.EmployeeID
    INTO
        emp_vacabal,
        emp_sickbal,
        emp_materbal,
        emp_othrbal,
        e_EmpType;

    firstToLastDay: LOOP

            IF looper != numofdaysofleave + 1 THEN

                SELECT ADDDATE(NEW.LeaveStartDate, INTERVAL looper DAY) INTO dateloop;



                SELECT
                esh.RowID
                ,esh.ShiftID
                FROM employeeshift esh
                INNER JOIN shift sh ON sh.RowID=esh.ShiftID
                WHERE esh.EmployeeID=NEW.EmployeeID
                AND esh.OrganizationID=NEW.OrganizationID
                AND dateloop BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
                ORDER BY DATEDIFF(dateloop, esh.EffectiveFrom) DESC
                LIMIT 1
                INTO empshiftRowID,shift_RowID;



                SELECT RowID FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND `Date`=dateloop INTO etentRowID;


                SELECT RowID,BasicPay FROM employeesalary WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND dateloop BETWEEN DATE(COALESCE(EffectiveDateFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveDateTo,dateloop)) AND DATEDIFF(dateloop,EffectiveDateFrom) >= 0 ORDER BY DATEDIFF(DATE_FORMAT(dateloop,'%Y-%m-%d'),EffectiveDateFrom) LIMIT 1 INTO esal_RowID,dailypayamount;


                SELECT GET_employeerateperday(NEW.EmployeeID, NEW.OrganizationID, dateloop) INTO dailypayamount;

                IF e_EmpType = 'Monthly 15a1s5d1f5a1sd1f51asd51f5s1da5' THEN

                    SELECT BasicPay
                    ,Salary
                    FROM employeesalary
                    WHERE EmployeeID=NEW.EmployeeID
                    AND OrganizationID=NEW.OrganizationID
                    AND dateloop BETWEEN DATE(COALESCE(EffectiveDateFrom, DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d'))) AND DATE(COALESCE(EffectiveDateTo, ADDDATE(CURRENT_TIMESTAMP(), INTERVAL 1 MONTH)))
                    AND DATEDIFF(dateloop,EffectiveDateFrom) >= 0
                    ORDER BY DATEDIFF(DATE_FORMAT(dateloop,'%Y-%m-%d'),EffectiveDateFrom)
                    LIMIT 1
                    INTO empBasicPay
                            ,emp_sal;

                    SET dailypayamount = empBasicPay / GET_employeesalarydivisor(NEW.OrganizationID, NEW.EmployeeID, dateloop);

                END IF;



                SELECT sh.TimeFrom
                ,sh.TimeTo
                FROM shift sh
                WHERE sh.RowID=shift_RowID
                INTO sh_timefrom,sh_timeto;

                IF COMPUTE_TimeDifference(sh_timefrom, sh_timeto) < 6 THEN

                    SET numhrsworkd = COMPUTE_TimeDifference(NEW.LeaveStartTime, NEW.LeaveEndTime);

                ELSE

                    SET numhrsworkd = COMPUTE_TimeDifference(NEW.LeaveStartTime, NEW.LeaveEndTime) - 1.0;

                END IF;

                IF IFNULL(numhrsworkd,0) < 0 THEN
                    SET numhrsworkd = 0;
                END IF;

                SET hourlypayamount = dailypayamount / numhrsworkd;

                IF leavetype LIKE '%Vacation%' THEN

                    IF emp_vacabal - numhrsworkd < 0 THEN

                        SET dailypayamount = dailypayamount + ((emp_vacabal - numhrsworkd) * hourlypayamount);

                    ELSE

                        SET dailypayamount = hourlypayamount * numhrsworkd;

                    END IF;

                ELSEIF leavetype LIKE '%Sick%' THEN

                    IF emp_sickbal - numhrsworkd < 0 THEN

                        SET dailypayamount = dailypayamount + ((emp_sickbal - numhrsworkd) * hourlypayamount);

                    ELSE

                        SET dailypayamount = hourlypayamount * numhrsworkd;

                    END IF;

                ELSEIF leavetype LIKE '%aternity%' THEN

                    IF emp_materbal - numhrsworkd < 0 THEN

                        SET dailypayamount = dailypayamount + ((emp_materbal - numhrsworkd) * hourlypayamount);

                    ELSE

                        SET dailypayamount = hourlypayamount * numhrsworkd;

                    END IF;

                ELSE

                    IF emp_othrbal - numhrsworkd < 0 THEN

                        SET dailypayamount = dailypayamount + ((emp_othrbal - numhrsworkd) * hourlypayamount);

                    ELSE

                        SET dailypayamount = hourlypayamount * numhrsworkd;

                    END IF;

                END IF;

                SELECT etd.TimeIn
                ,etd.TimeOut
                FROM employeetimeentrydetails etd
                WHERE etd.EmployeeID=NEW.EmployeeID
                AND etd.OrganizationID=NEW.OrganizationID
                AND etd.`Date`='1900-01-01'
                INTO etd_timein,etd_timeout;

                IF etd_timein IS NOT NULL AND etd_timeout IS NOT NULL THEN

                    SELECT sh.TimeFrom
                    ,sh.TimeTo
                    FROM shift sh
                    WHERE sh.RowID=shift_RowID
                    INTO sh_timefrom,sh_timeto;

                    SELECT GRACE_PERIOD(etd_timein,sh_timefrom) INTO etd_timein;

                    IF etd_timein BETWEEN '12:00' AND '13:00' THEN
                        SET etd_timein = '13:00';
                    END IF;


                    IF COMPUTE_TimeDifference(etd_timeout,sh_timeto) > 4
                        AND COMPUTE_TimeDifference(etd_timeout,sh_timeto) < 5
                        AND COMPUTE_TimeDifference(sh_timefrom, sh_timeto) = 9 THEN

                        IF etd_timein BETWEEN sh_timefrom AND etd_timeout THEN

                            SET etd_timeout = ADDTIME(sh_timefrom,'04:00');

                        ELSEIF SUBTIME(sh_timeto,'04:00') BETWEEN etd_timein AND sh_timeto THEN

                            SET etd_timeout = SUBTIME(sh_timeto,'04:00');

                        ELSEIF etd_timein BETWEEN SUBTIME(sh_timeto,'04:00') AND sh_timeto THEN

                            SET etd_timeout = SUBTIME(sh_timeto,'04:00');

                        END IF;

                    ELSEIF COMPUTE_TimeDifference(etd_timeout,sh_timeto) > 5
                            AND COMPUTE_TimeDifference(etd_timeout,sh_timeto) < 6
                            AND COMPUTE_TimeDifference(sh_timefrom, sh_timeto) = 10 THEN

                        IF etd_timein BETWEEN sh_timefrom AND etd_timeout THEN

                            SET etd_timeout = ADDTIME(sh_timefrom,'05:00');

                        ELSEIF SUBTIME(sh_timeto,'05:00') BETWEEN etd_timein AND sh_timeto THEN

                            SET etd_timeout = SUBTIME(sh_timeto,'05:00');

                        ELSEIF etd_timein BETWEEN SUBTIME(sh_timeto,'05:00') AND sh_timeto THEN

                            SET etd_timeout = SUBTIME(sh_timeto,'05:00');

                        END IF;

                    END IF;



                    SET extra_regularhours = COMPUTE_TimeDifference(etd_timein,etd_timeout);

                    SET extra_regularamount = extra_regularhours * hourlypayamount;

                    SET dailypayamount = dailypayamount + extra_regularamount;

                END IF;


                SELECT etd.TimeIn
                ,etd.TimeOut
                FROM employeetimeentrydetails etd
                WHERE etd.EmployeeID=NEW.EmployeeID
                AND etd.OrganizationID=NEW.OrganizationID
                AND etd.`Date`=dateloop
                LIMIT 1
                INTO etd_timein,etd_timeout;

                SET etd_timein = IF(TIME_FORMAT(etd_timeout,'%H:%i') = TIME_FORMAT(NEW.LeaveStartTime,'%H:%i') AND NEW.LeaveStartTime < NEW.LeaveEndTime, TIME(ADDTIME(etd_timeout,'00:00:01')), NEW.LeaveStartTime);

                IF COMPUTE_TimeDifference(sh_timefrom, sh_timeto) = 9 THEN

                    IF SUBTIME(sh_timeto,'04:30') < etd_timein AND sh_timeto > NEW.LeaveEndTime THEN
                        SET under_hrs = COMPUTE_TimeDifference(etd_timein, NEW.LeaveEndTime);

                        SET under_hrs = 4 - under_hrs;

                    ELSEIF (etd_timein BETWEEN SUBTIME(sh_timeto,'04:30') AND sh_timeto)
                            AND sh_timeto = NEW.LeaveEndTime THEN
                        SET under_hrs = 0;

                    ELSEIF etd_timein BETWEEN SUBTIME(sh_timeto,'04:30') AND sh_timeto THEN
                        SET under_hrs = COMPUTE_TimeDifference(etd_timein, sh_timeto);

                    ELSE
                        SET under_hrs = 0;
                    END IF;


                    IF sh_timefrom <= etd_timein AND ADDTIME(sh_timefrom,'04:30') >= NEW.LeaveEndTime THEN
                        SET late_hrs = COMPUTE_TimeDifference(etd_timein, NEW.LeaveEndTime);

                        SET late_hrs = 4 - late_hrs;

                    ELSEIF (etd_timein BETWEEN sh_timefrom AND ADDTIME(sh_timefrom,'04:30'))
                            AND ADDTIME(sh_timefrom,'04:30') = NEW.LeaveEndTime THEN
                        SET late_hrs = COMPUTE_TimeDifference(etd_timein, sh_timefrom);

                    ELSEIF etd_timein BETWEEN sh_timefrom AND ADDTIME(sh_timefrom,'04:30') THEN
                        SET late_hrs = COMPUTE_TimeDifference(etd_timein, sh_timefrom);

                    ELSE
                        SET late_hrs = 0;
                    END IF;

                ELSEIF COMPUTE_TimeDifference(sh_timefrom, sh_timeto) = 10 THEN

                    IF SUBTIME(sh_timeto,'05:00') < etd_timein AND sh_timeto > NEW.LeaveEndTime THEN
                        SET under_hrs = COMPUTE_TimeDifference(etd_timein, NEW.LeaveEndTime);

                        SET under_hrs = 4.5 - under_hrs;

                    ELSEIF (etd_timein BETWEEN SUBTIME(sh_timeto,'05:00') AND sh_timeto)
                            AND sh_timeto = NEW.LeaveEndTime THEN
                        SET under_hrs = 0;

                    ELSEIF etd_timein BETWEEN SUBTIME(sh_timeto,'05:00') AND sh_timeto THEN
                        SET under_hrs = COMPUTE_TimeDifference(etd_timein, sh_timeto);

                    ELSE
                        SET under_hrs = 0;
                    END IF;


                    IF sh_timefrom <= etd_timein AND ADDTIME(sh_timefrom,'05:00') >= NEW.LeaveEndTime THEN
                        SET late_hrs = COMPUTE_TimeDifference(etd_timein, NEW.LeaveEndTime);

                        SET late_hrs = 4.5 - late_hrs;

                    ELSEIF (etd_timein BETWEEN sh_timefrom AND ADDTIME(sh_timefrom,'05:00'))
                            AND ADDTIME(sh_timefrom,'05:00') = NEW.LeaveEndTime THEN
                        SET late_hrs = 0;

                    ELSEIF etd_timein BETWEEN sh_timefrom AND ADDTIME(sh_timefrom,'05:00') THEN
                        SET late_hrs = COMPUTE_TimeDifference(etd_timein, sh_timefrom);

                    ELSE
                        SET late_hrs = 0;
                    END IF;

                ELSEIF COMPUTE_TimeDifference(sh_timefrom, sh_timeto) IN (4,5) THEN

                    SET late_hrs = 0;

                    SET under_hrs = 0;

                END IF;

                SET late_hrs = IFNULL(late_hrs,0);

                SET under_hrs = IFNULL(under_hrs,0);


                SELECT INSUPD_employeetimeentries(etentRowID
                                                            , NEW.OrganizationID
                                                            , NEW.CreatedBy
                                                            , NEW.CreatedBy
                                                            , dateloop
                                                            , empshiftRowID
                                                            , NEW.EmployeeID
                                                            , esal_RowID
                                                            , '0'
                                                            , IFNULL(extra_regularhours,0)
                                                            , 0
                                                            , under_hrs
                                                            , 0
                                                            , 0
                                                            , late_hrs
                                                            , (SELECT RowID FROM payrate WHERE `Date`=dateloop AND OrganizationID=NEW.OrganizationID)
                                                            , dailypayamount
                                                            , IFNULL(extra_regularhours,0)
                                                            , dailypayamount
                                                            , 0
                                                            , under_hrs * hourlypayamount
                                                            , 0
                                                            , 0
                                                            , late_hrs * hourlypayamount
                                                            , 0
                                                            , 0
                                                            , 0
                                                            , 0
                                                            , 0) INTO anyint;


                SELECT RowID FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND `Date`=dateloop INTO anyint;

                IF leavetype LIKE '%Vacation%' THEN

                    UPDATE employeetimeentry ete
                    SET ete.VacationLeaveHours=numhrsworkd
                    ,ete.SickLeaveHours=0
                    ,ete.MaternityLeaveHours=0
                    ,ete.OtherLeaveHours=0,ete.Leavepayment=dailypayamount
                    WHERE ete.RowID=anyint;

                ELSEIF leavetype LIKE '%Sick%' THEN

                    UPDATE employeetimeentry ete
                    SET ete.SickLeaveHours=numhrsworkd
                    ,ete.VacationLeaveHours=0
                    ,ete.MaternityLeaveHours=0
                    ,ete.OtherLeaveHours=0,ete.Leavepayment=dailypayamount
                    WHERE ete.RowID=anyint;

                ELSEIF leavetype LIKE '%aternity%' THEN

                    UPDATE employeetimeentry ete
                    SET ete.MaternityLeaveHours=numhrsworkd
                    ,ete.SickLeaveHours=0
                    ,ete.VacationLeaveHours=0
                    ,ete.OtherLeaveHours=0,ete.Leavepayment=dailypayamount
                    WHERE ete.RowID=anyint;

                ELSE

                    UPDATE employeetimeentry ete
                    SET ete.OtherLeaveHours=numhrsworkd
                    ,ete.SickLeaveHours=0
                    ,ete.MaternityLeaveHours=0
                    ,ete.VacationLeaveHours=0,ete.Leavepayment=dailypayamount
                    WHERE ete.RowID=anyint;

                END IF;


                IF NEW.LeaveEndTime < sh_timeto THEN

                    SELECT etd.TimeIn
                    ,etd.TimeOut
                    FROM employeetimeentrydetails etd
                    WHERE etd.EmployeeID=NEW.EmployeeID
                    AND etd.OrganizationID=NEW.OrganizationID
                    AND etd.`Date`=dateloop
                    INTO etd_timein,etd_timeout;

                    IF etd_timeout < NEW.LeaveEndTime THEN

                        IF leavetype = 'Others' THEN

                            SET leavetype = 'Others';



                        END IF;

                    END IF;

                END IF;


                IF etd_timein > sh_timefrom THEN

                    IF etd_timein >= NEW.LeaveStartTime
                        AND etd_timein >= NEW.LeaveEndTime THEN

                        IF leavetype = 'Others' THEN

                            SET leavetype = 'Others';



                        END IF;

                    END IF;

                END IF;

            ELSE
                LEAVE firstToLastDay;
            END IF;

            SET looper = looper + 1;

        END LOOP firstToLastDay;

END IF;




SELECT RowID FROM `view` WHERE ViewName='Employee Leave' AND OrganizationID=NEW.OrganizationID LIMIT 1 INTO viewID;

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES
(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'LeaveType',NEW.RowID,'',NEW.LeaveType,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'LeaveStartTime',NEW.RowID,'',NEW.LeaveStartTime,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'LeaveEndTime',NEW.RowID,'',NEW.LeaveEndTime,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'LeaveStartDate',NEW.RowID,'',NEW.LeaveStartDate,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'LeaveEndDate',NEW.RowID,'',NEW.LeaveEndDate,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'EmployeeID',NEW.RowID,'',NEW.EmployeeID,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'Reason',NEW.RowID,'',NEW.Reason,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,NEW.OrganizationID,viewID,'Comments',NEW.RowID,'',NEW.Comments,'Insert');

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
