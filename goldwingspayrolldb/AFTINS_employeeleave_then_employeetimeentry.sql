-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.12-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for trigger AFTINS_employeeleave_then_employeetimeentry
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

DECLARE month_count_peryear INT(11) DEFAULT 12;

SELECT NEW.LeaveType INTO leavetype;

IF LOCATE('aternity',leavetype) > 0 THEN

    SET @dutyhours = 0.0;

    SET @correct_date = CURDATE();

    SELECT EXISTS
    (
        SELECT INSERTUPDATE_employeetimeentry(
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
            IF(sh.DutyHoursCount > 5.0, (sh.DutyHoursCount - 1.0), sh.DutyHoursCount),
            0,
            0,
            0,
            NULL
        )
        FROM dates d
        INNER JOIN payrate pr ON pr.OrganizationID=NEW.OrganizationID AND pr.`Date`=d.DateValue
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
        ON sh.RowID = esh.ShiftID
        LEFT JOIN (
            SELECT *
            FROM employeesalary
            WHERE EmployeeID = NEW.EmployeeID AND
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

    IF TIME_FORMAT(NEW.LeaveStartTime, '%p') = 'PM' AND
        TIME_FORMAT(NEW.LeaveEndTime, '%p') = 'AM' THEN

        SET reghrsworkd = TIMEDIFF(ADDTIME(NEW.LeaveEndTime, '24:00'), NEW.LeaveStartTime);
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

            SELECT ADDDATE(NEW.LeaveStartDate, INTERVAL looper DAY)
            INTO dateloop;

            SELECT
                esh.RowID,
                esh.ShiftID
            FROM employeeshift esh
            INNER JOIN shift sh
            ON sh.RowID = esh.ShiftID
            WHERE esh.EmployeeID = NEW.EmployeeID AND
                esh.OrganizationID = NEW.OrganizationID AND
                dateloop BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
            ORDER BY DATEDIFF(dateloop, esh.EffectiveFrom) DESC
            LIMIT 1
            INTO
                empshiftRowID,
                shift_RowID;

            SELECT RowID
            FROM employeetimeentry
            WHERE EmployeeID = NEW.EmployeeID AND
                OrganizationID = NEW.OrganizationID AND
                `Date` = dateloop
            INTO etentRowID;

            SELECT
                esa.RowID,
                IF(
                    e.EmployeeType = 'Daily',
                    esa.BasicPay,
                    (esa.Salary / (e.WorkDaysPerYear / month_count_peryear))
                ) `BasicPay`
            FROM employeesalary esa
            INNER JOIN employee e
            ON e.RowID = esa.EmployeeID
            WHERE esa.EmployeeID = NEW.EmployeeID AND
                esa.OrganizationID=NEW.OrganizationID AND
                dateloop BETWEEN DATE(COALESCE(esa.EffectiveDateFrom, DATE_FORMAT(CURRENT_TIMESTAMP(), '%Y-%m-%d'))) AND DATE(COALESCE(esa.EffectiveDateTo, dateloop)) AND
                DATEDIFF(dateloop, esa.EffectiveDateFrom) >= 0
            ORDER BY DATEDIFF(DATE_FORMAT(dateloop, '%Y-%m-%d'), esa.EffectiveDateFrom)
            LIMIT 1
            INTO
                esal_RowID,
                dailypayamount;

            SELECT
                sh.TimeFrom,
                sh.TimeTo
            FROM shift sh
            WHERE sh.RowID = shift_RowID
            INTO
                sh_timefrom,
                sh_timeto;

            SET numhrsworkd = (
                TIMESTAMPDIFF(
                    SECOND,
                    CONCAT_DATETIME(NEW.LeaveStartDate, sh_timefrom),
                    CONCAT_DATETIME(ADDDATE(NEW.LeaveStartDate, INTERVAL IS_TIMERANGE_REACHTOMORROW(sh_timefrom, sh_timeto) DAY), sh_timeto)
                ) / 3600
            );

            IF (numhrsworkd < 6) = FALSE THEN
                SET numhrsworkd = (numhrsworkd - 1);
            END IF;

            SET numhrsworkd = IFNULL(numhrsworkd, 0);
            SET hourlypayamount = (dailypayamount / numhrsworkd);
            SET numhrsworkd = NEW.OfficialValidHours;

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

            SELECT
                etd.TimeIn,
                etd.TimeOut
            FROM employeetimeentrydetails etd
            WHERE etd.EmployeeID = NEW.EmployeeID AND
                etd.OrganizationID = NEW.OrganizationID AND
                etd.`Date`='1900-01-01'
            INTO
                etd_timein,
                etd_timeout;

            SET late_hrs = IFNULL(late_hrs,0);
            SET under_hrs = IFNULL(under_hrs,0);

            SELECT INSUPD_employeetimeentries(
                etentRowID,
                NEW.OrganizationID,
                NEW.CreatedBy,
                NEW.CreatedBy,
                dateloop,
                empshiftRowID,
                NEW.EmployeeID,
                esal_RowID,
                '0',
                IFNULL(extra_regularhours, 0),
                0,
                under_hrs,
                0,
                0,
                late_hrs,
                (
                    SELECT RowID
                    FROM payrate
                    WHERE `Date` = dateloop AND
                        OrganizationID = NEW.OrganizationID
                ),
                dailypayamount,
                IFNULL(extra_regularhours, 0),
                dailypayamount,
                0,
                under_hrs * hourlypayamount,
                0,
                0,
                late_hrs * hourlypayamount,
                0,
                0,
                0,
                0,
                0
            )
            INTO anyint;

            SELECT RowID
            FROM employeetimeentry
            WHERE EmployeeID = NEW.EmployeeID AND
                OrganizationID = NEW.OrganizationID AND
                `Date` = dateloop
            INTO anyint;

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

                SELECT
                    etd.TimeIn,
                    etd.TimeOut
                FROM employeetimeentrydetails etd
                WHERE etd.EmployeeID=NEW.EmployeeID AND
                    etd.OrganizationID=NEW.OrganizationID AND
                    etd.`Date`=dateloop
                INTO
                    etd_timein,
                    etd_timeout;

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
