/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTUPD_employeedisciplinaryaction`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_employeedisciplinaryaction` AFTER UPDATE ON `employeedisciplinaryaction` FOR EACH ROW BEGIN

DECLARE viewID INT(11);

DECLARE suspendaycount INT(11);

DECLARE indx INT(11) DEFAULT 0;

DECLARE dateloop DATE;

DECLARE eshiftID INT(11);

DECLARE esalID INT(11);

DECLARE etentID INT(11);



DECLARE COMP_emptimeentry INT(11);

DECLARE emp_startdate DATE;

DECLARE etent_Date DATE;

DECLARE old_daycount INT(11);

DECLARE new_daycount INT(11);



SELECT RowID FROM `view` WHERE ViewName='Employee Disciplinary Action' AND OrganizationID=NEW.OrganizationID LIMIT 1 INTO viewID;

IF OLD.DateFrom != NEW.DateFrom THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'DateFrom',NEW.RowID,OLD.DateFrom,NEW.DateFrom,'Update');

END IF;

IF OLD.DateTo != NEW.DateTo THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'DateTo',NEW.RowID,OLD.DateTo,NEW.DateTo,'Update');

END IF;

IF OLD.FindingID != NEW.FindingID THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'FindingID',NEW.RowID,OLD.FindingID,NEW.FindingID,'Update');

END IF;

IF OLD.FindingDescription != NEW.FindingDescription THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'FindingDescription',NEW.RowID,OLD.FindingDescription,NEW.FindingDescription,'Update');

END IF;

IF OLD.Action != NEW.Action THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'Action',NEW.RowID,OLD.Action,NEW.Action,'Update');

END IF;

IF OLD.Penalty != NEW.Penalty THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'Penalty',NEW.RowID,OLD.Penalty,NEW.Penalty,'Update');

END IF;

IF OLD.Comments != NEW.Comments THEN

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.LastUpdBy,NEW.LastUpdBy,NEW.OrganizationID,viewID,'Comments',NEW.RowID,OLD.Comments,NEW.Comments,'Update');

END IF;













SELECT StartDate FROM employee WHERE RowID=NEW.EmployeeID INTO emp_startdate;

IF OLD.DateFrom = NEW.DateFrom AND OLD.DateTo = NEW.DateTo THEN

    IF OLD.Action != NEW.Action THEN

        IF NEW.Action IN ('1-3 Days Suspension','4-7 Days Suspension','8-14 Days Suspension') THEN

            SET indx = 0;

            timeentloop : LOOP

                IF indx <= old_daycount THEN

                    SET etent_Date = ADDDATE(OLD.DateFrom, INTERVAL indx DAY);



                    SELECT RowID FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND Date=etent_Date INTO etentID;

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
                        etentID
                        ,NEW.OrganizationID
                        ,CURRENT_TIMESTAMP()
                        ,NEW.LastUpdBy
                        ,etent_Date
                        ,eshiftID
                        ,NEW.EmployeeID
                        ,esalID
                        ,'0'
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
                        ,(SELECT RowID FROM payrate WHERE Date=dateloop AND OrganizationID=NEW.OrganizationID LIMIT 1)
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
                            ,LastUpdBy=NEW.LastUpdBy
                            ,RegularHoursWorked=0
                            ,RegularHoursAmount=0
                            ,TotalHoursWorked=0
                            ,OvertimeHoursWorked=0
                            ,OvertimeHoursAmount=0
                            ,UndertimeHours=0
                            ,UndertimeHoursAmount=0
                            ,NightDifferentialHours=0
                            ,NightDiffHoursAmount=0
                            ,NightDifferentialOTHours=0
                            ,NightDiffOTHoursAmount=0
                            ,HoursLate=0
                            ,HoursLateAmount=0
                            ,LateFlag='0'
                            ,TotalDayPay=0;

                    SET indx = indx + 1;

                ELSE

                    LEAVE timeentloop;

                END IF;

            END LOOP;



        ELSE

            SET indx = 0;

            timeentloop : LOOP

                IF indx <= old_daycount THEN

                    SET etent_Date = ADDDATE(OLD.DateFrom, INTERVAL indx DAY);



                    SELECT COMPUTE_employeetimeentry(NEW.EmployeeID,NEW.OrganizationID,etent_Date,NEW.LastUpdBy,NEW.LastUpdBy,emp_startdate) INTO COMP_emptimeentry;

                    SET indx = indx + 1;

                ELSE

                    LEAVE timeentloop;

                END IF;

            END LOOP;


        END IF;

    END IF;

ELSE

    SET old_daycount = DATEDIFF(OLD.DateTo,OLD.DateFrom);

    SET new_daycount = DATEDIFF(NEW.DateTo,NEW.DateFrom);



    IF old_daycount > new_daycount THEN



        IF NEW.Action IN ('1-3 Days Suspension','4-7 Days Suspension','8-14 Days Suspension') THEN

            SET indx = 0;

            timeentloop : LOOP

                IF indx <= old_daycount THEN

                    SET etent_Date = ADDDATE(OLD.DateFrom, INTERVAL indx DAY);



                    SELECT RowID FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND Date=etent_Date INTO etentID;

                    IF etent_Date BETWEEN NEW.DateFrom AND NEW.DateTo THEN

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
                            etentID
                            ,NEW.OrganizationID
                            ,CURRENT_TIMESTAMP()
                            ,NEW.LastUpdBy
                            ,etent_Date
                            ,eshiftID
                            ,NEW.EmployeeID
                            ,esalID
                            ,'0'
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
                            ,(SELECT RowID FROM payrate WHERE Date=dateloop AND OrganizationID=NEW.OrganizationID LIMIT 1)
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
                                ,LastUpdBy=NEW.LastUpdBy
                                ,RegularHoursWorked=0
                                ,RegularHoursAmount=0
                                ,TotalHoursWorked=0
                                ,OvertimeHoursWorked=0
                                ,OvertimeHoursAmount=0
                                ,UndertimeHours=0
                                ,UndertimeHoursAmount=0
                                ,NightDifferentialHours=0
                                ,NightDiffHoursAmount=0
                                ,NightDifferentialOTHours=0
                                ,NightDiffOTHoursAmount=0
                                ,HoursLate=0
                                ,HoursLateAmount=0
                                ,LateFlag='0'
                                ,TotalDayPay=0;

                    ELSE

                        SELECT COMPUTE_employeetimeentry(NEW.EmployeeID,NEW.OrganizationID,etent_Date,NEW.LastUpdBy,NEW.LastUpdBy,emp_startdate) INTO COMP_emptimeentry;

                    END IF;


                    SET indx = indx + 1;

                ELSE

                    LEAVE timeentloop;

                END IF;

            END LOOP;



        ELSE

            SET indx = 0;

            timeentloop : LOOP

                IF indx <= old_daycount THEN

                    SET etent_Date = ADDDATE(NEW.DateFrom, INTERVAL indx DAY);



                    SELECT COMPUTE_employeetimeentry(NEW.EmployeeID,NEW.OrganizationID,etent_Date,NEW.LastUpdBy,NEW.LastUpdBy,emp_startdate) INTO COMP_emptimeentry;

                    SET indx = indx + 1;

                ELSE

                    LEAVE timeentloop;

                END IF;

            END LOOP;


        END IF;


    ELSEIF old_daycount < new_daycount THEN



        IF NEW.Action IN ('1-3 Days Suspension','4-7 Days Suspension','8-14 Days Suspension') THEN

            SET indx = 0;

            timeentloop : LOOP

                IF indx <= new_daycount THEN

                    SET etent_Date = ADDDATE(NEW.DateFrom, INTERVAL indx DAY);



                    SELECT RowID FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND Date=etent_Date INTO etentID;

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
                        etentID
                        ,NEW.OrganizationID
                        ,CURRENT_TIMESTAMP()
                        ,NEW.LastUpdBy
                        ,etent_Date
                        ,eshiftID
                        ,NEW.EmployeeID
                        ,esalID
                        ,'0'
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
                        ,(SELECT RowID FROM payrate WHERE Date=dateloop AND OrganizationID=NEW.OrganizationID LIMIT 1)
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
                            ,LastUpdBy=NEW.LastUpdBy
                            ,RegularHoursWorked=0
                            ,RegularHoursAmount=0
                            ,TotalHoursWorked=0
                            ,OvertimeHoursWorked=0
                            ,OvertimeHoursAmount=0
                            ,UndertimeHours=0
                            ,UndertimeHoursAmount=0
                            ,NightDifferentialHours=0
                            ,NightDiffHoursAmount=0
                            ,NightDifferentialOTHours=0
                            ,NightDiffOTHoursAmount=0
                            ,HoursLate=0
                            ,HoursLateAmount=0
                            ,LateFlag='0'
                            ,TotalDayPay=0;

                    SET indx = indx + 1;

                ELSE

                    LEAVE timeentloop;

                END IF;

            END LOOP;



        ELSE

            SET indx = 0;

            timeentloop : LOOP

                IF indx <= new_daycount THEN

                    SET etent_Date = ADDDATE(NEW.DateFrom, INTERVAL indx DAY);



                    SELECT COMPUTE_employeetimeentry(NEW.EmployeeID,NEW.OrganizationID,etent_Date,NEW.LastUpdBy,NEW.LastUpdBy,emp_startdate) INTO COMP_emptimeentry;

                    SET indx = indx + 1;

                ELSE

                    LEAVE timeentloop;

                END IF;

            END LOOP;


        END IF;


    ELSEIF old_daycount = new_daycount THEN

        IF OLD.Action != NEW.Action THEN

            IF NEW.Action IN ('1-3 Days Suspension','4-7 Days Suspension','8-14 Days Suspension') THEN


                SET indx = 0;

                timeentloop : LOOP

                    IF indx <= new_daycount THEN

                        SET etent_Date = ADDDATE(OLD.DateFrom, INTERVAL indx DAY);



                        SELECT RowID FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND Date=etent_Date INTO etentID;

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
                            etentID
                            ,NEW.OrganizationID
                            ,CURRENT_TIMESTAMP()
                            ,NEW.LastUpdBy
                            ,etent_Date
                            ,eshiftID
                            ,NEW.EmployeeID
                            ,esalID
                            ,'0'
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
                            ,(SELECT RowID FROM payrate WHERE Date=dateloop AND OrganizationID=NEW.OrganizationID LIMIT 1)
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
                                ,LastUpdBy=NEW.LastUpdBy
                                ,RegularHoursWorked=0
                                ,RegularHoursAmount=0
                                ,TotalHoursWorked=0
                                ,OvertimeHoursWorked=0
                                ,OvertimeHoursAmount=0
                                ,UndertimeHours=0
                                ,UndertimeHoursAmount=0
                                ,NightDifferentialHours=0
                                ,NightDiffHoursAmount=0
                                ,NightDifferentialOTHours=0
                                ,NightDiffOTHoursAmount=0
                                ,HoursLate=0
                                ,HoursLateAmount=0
                                ,LateFlag='0'
                                ,TotalDayPay=0;

                        SET indx = indx + 1;

                    ELSE

                        LEAVE timeentloop;

                    END IF;

                END LOOP;

            ELSE


                SET indx = 0;

                timeentloop : LOOP

                    IF indx <= old_daycount THEN

                        SET etent_Date = ADDDATE(OLD.DateFrom, INTERVAL indx DAY);



                        SELECT COMPUTE_employeetimeentry(NEW.EmployeeID,NEW.OrganizationID,etent_Date,NEW.LastUpdBy,NEW.LastUpdBy,emp_startdate) INTO COMP_emptimeentry;

                        SET indx = indx + 1;

                    ELSE

                        LEAVE timeentloop;

                    END IF;

                END LOOP;


            END IF;

        ELSE

            SET indx = 0;

            timeentloop : LOOP

                IF indx <= old_daycount THEN

                    SET etent_Date = ADDDATE(OLD.DateFrom, INTERVAL indx DAY);



                    SELECT COMPUTE_employeetimeentry(NEW.EmployeeID,NEW.OrganizationID,etent_Date,NEW.LastUpdBy,NEW.LastUpdBy,emp_startdate) INTO COMP_emptimeentry;

                    SET indx = indx + 1;

                ELSE

                    LEAVE timeentloop;

                END IF;

            END LOOP;










            SET indx = 0;

            timeentloop : LOOP

                IF indx <= new_daycount THEN

                    SET etent_Date = ADDDATE(NEW.DateFrom, INTERVAL indx DAY);

                    SELECT RowID FROM employeetimeentry WHERE EmployeeID=NEW.EmployeeID AND OrganizationID=NEW.OrganizationID AND Date=etent_Date INTO etentID;

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
                        etentID
                        ,NEW.OrganizationID
                        ,CURRENT_TIMESTAMP()
                        ,NEW.LastUpdBy
                        ,etent_Date
                        ,eshiftID
                        ,NEW.EmployeeID
                        ,esalID
                        ,'0'
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
                        ,(SELECT RowID FROM payrate WHERE Date=dateloop AND OrganizationID=NEW.OrganizationID LIMIT 1)
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
                            ,LastUpdBy=NEW.LastUpdBy
                            ,RegularHoursWorked=0
                            ,RegularHoursAmount=0
                            ,TotalHoursWorked=0
                            ,OvertimeHoursWorked=0
                            ,OvertimeHoursAmount=0
                            ,UndertimeHours=0
                            ,UndertimeHoursAmount=0
                            ,NightDifferentialHours=0
                            ,NightDiffHoursAmount=0
                            ,NightDifferentialOTHours=0
                            ,NightDiffOTHoursAmount=0
                            ,HoursLate=0
                            ,HoursLateAmount=0
                            ,LateFlag='0'
                            ,TotalDayPay=0;


                    SET indx = indx + 1;

                ELSE

                    LEAVE timeentloop;

                END IF;

            END LOOP;

        END IF;

    END IF;

END IF;




END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
