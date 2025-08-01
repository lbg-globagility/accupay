/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTINS_employeetimeentrydetails`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_employeetimeentrydetails` AFTER INSERT ON `employeetimeentrydetails` FOR EACH ROW BEGIN

DECLARE isAutomaticOvertimeFiling TINYINT(1);

DECLARE anyint INT(11);
DECLARE sh_timefrom TIME;
DECLARE sh_timeto TIME;
DECLARE today_timefrom DATETIME;
DECLARE today_timeto DATETIME;
DECLARE tomorrow_timefrom DATETIME DEFAULT NULL;
DECLARE tomorrow_timeto DATETIME DEFAULT NULL;
DECLARE isShiftRestDay CHAR(1);
DECLARE today_timein DATETIME;
DECLARE today_timeout DATETIME;
DECLARE day_of_rest CHAR(1);

DECLARE has_already_OTapproved CHAR(1);

SELECT
    d.AutomaticOvertimeFiling,
    e.DayOfRest
FROM employee e
INNER JOIN position p
    ON p.RowID=e.PositionID
INNER JOIN `division` d
    ON d.RowID=p.DivisionId
    AND d.ParentDivisionID IS NOT NULL
WHERE e.RowID=NEW.EmployeeID
INTO
    isAutomaticOvertimeFiling,
    day_of_rest;

SET today_timein = ADDTIME(TIMESTAMP(NEW.`Date`), NEW.TimeIn);

SET today_timeout = ADDTIME(
    IF(
        NEW.TimeOut > NEW.TimeIn AND TIME_FORMAT(NEW.TimeIn,'%p') != TIME_FORMAT(NEW.TimeOut,'%p'),
        TIMESTAMP(NEW.`Date`),
        TIMESTAMP(ADDDATE(NEW.`Date`, INTERVAL 1 DAY))
    ),
    NEW.TimeOut
);

IF isAutomaticOvertimeFiling THEN

    SELECT
        ss.StartTime,
        ss.EndTime,
        ss.IsRestDay,
        ADDTIME(TIMESTAMP(NEW.`Date`), ss.StartTime),
        ADDTIME(
            IF(
                ss.EndTime > ss.StartTime,
                TIMESTAMP(NEW.`Date`),
                TIMESTAMP(ADDDATE(NEW.`Date`, INTERVAL 1 DAY))
            ),
            ADDTIME(ss.EndTime, e.MinimumOvertime)
        )
    FROM shiftschedules ss
    INNER JOIN employee e
        ON e.RowID=ss.EmployeeID AND e.OrganizationID=ss.OrganizationID
    WHERE ss.EmployeeID=NEW.EmployeeID
        AND ss.OrganizationID=NEW.OrganizationID
        AND NEW.`Date` = ss.DATE
    LIMIT 1
    INTO
        sh_timefrom,
        sh_timeto,
        isShiftRestDay,
        today_timefrom,
        today_timeto;

    SET tomorrow_timefrom = TIMESTAMP(TIMESTAMPADD(HOUR,24,today_timefrom));
    SET tomorrow_timeto = TIMESTAMP(TIMESTAMPADD(HOUR,24,today_timeto));

    SELECT EXISTS(
        SELECT RowID
        FROM employeeovertime
        WHERE EmployeeID=NEW.EmployeeID
            AND OrganizationID=NEW.OrganizationID
            AND NEW.`Date` BETWEEN OTStartDate AND OTEndDate
    )
    INTO has_already_OTapproved;

    IF has_already_OTapproved = '0' THEN

        IF TIMESTAMP(today_timeout) BETWEEN TIMESTAMP(today_timeto) AND TIMESTAMP(TIMESTAMPADD(SECOND, -1, tomorrow_timefrom)) THEN

            SELECT INSUPD_employeeOT(
                NULL,
                NEW.OrganizationID,
                NEW.CreatedBy,
                NEW.CreatedBy,
                NEW.EmployeeID,
                'Overtime',
                sh_timeto,
                NEW.TimeOut,
                NEW.`Date`,
                NEW.`Date`,
                'Approved',
                '',
                '',
                NULL
            )
            INTO anyint;

        END IF;

    ELSE

        UPDATE employeeovertime
        SET
            OTStartTime = sh_timeto,
            OTEndTime = NEW.TimeOut,
            LastUpd = IFNULL(
                LastUpd,
                CURRENT_TIMESTAMP()
            ),
            LastUpdBy=NEW.CreatedBy
        WHERE EmployeeID=NEW.EmployeeID
            AND OrganizationID=NEW.OrganizationID
            AND NEW.`Date` BETWEEN OTStartDate AND OTEndDate
            AND sh_timeto IS NOT NULL;

    END IF;

END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
