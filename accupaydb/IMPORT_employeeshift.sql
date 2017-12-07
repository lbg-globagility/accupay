/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `IMPORT_employeeshift`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `IMPORT_employeeshift`(IN `i_EmployeeID` VARCHAR(50), IN `OrganizID` INT, IN `CreatedLastUpdBy` INT, IN `i_TimeFrom` TIME, IN `i_TimeTo` TIME, IN `i_DateFrom` DATE, IN `i_DateTo` DATE, IN `i_SchedType` TEXT)
    DETERMINISTIC
BEGIN

DECLARE employeeRowID INT(11);
DECLARE shiftRowID INT(11);
DECLARE employeeshiftID INT(11);
DECLARE restDay INT(11);
DECLARE calcNightShift INT(11);

SELECT
    RowID,
    DayOfRest,
    CalcNightDiff
FROM employee
WHERE EmployeeID = i_EmployeeID
    AND OrganizationID = OrganizID
LIMIT 1
INTO
    employeeRowID,
    restDay,
    calcNightShift;

IF employeeRowID IS NOT NULL
    AND i_DateFrom IS NOT NULL THEN

    IF (i_TimeFrom IS NULL AND i_TimeTo IS NULL)
        OR (i_TimeFrom IS NULL OR i_TimeTo IS NULL) THEN

        SET shiftRowID = NULL;

    ELSE
        SELECT RowID
        FROM shift sh
        WHERE sh.TimeFrom=i_TimeFrom
            AND sh.TimeTo=i_TimeTo
            AND sh.OrganizationID=OrganizID
        INTO shiftRowID;

        IF shiftRowID IS NULL THEN
            INSERT INTO shift
            (
                OrganizationID
                ,Created
                ,CreatedBy
                ,TimeFrom
                ,TimeTo
            ) VALUES (
                OrganizID
                ,CURRENT_TIMESTAMP()
                ,CreatedLastUpdBy
                ,i_TimeFrom
                ,i_TimeTo
            ) ON
            DUPLICATE
            KEY
            UPDATE
                LastUpd=CURRENT_TIMESTAMP();

            SELECT @@Identity AS Id INTO shiftRowID;
        END IF;
    END IF;

    SELECT RowID
    FROM employeeshift
    WHERE OrganizationID=OrganizID
        AND EmployeeID=employeeRowID
        AND EffectiveFrom=i_DateFrom
        AND EffectiveTo=i_DateTo
    INTO employeeshiftID;

    INSERT INTO employeeshift
    (
        RowID
        ,OrganizationID
        ,Created
        ,CreatedBy
        ,EmployeeID
        ,ShiftID
        ,EffectiveFrom
        ,EffectiveTo
        ,NightShift
        ,RestDay
    ) VALUES (
        employeeshiftID
        ,OrganizID
        ,CURRENT_TIMESTAMP()
        ,CreatedLastUpdBy
        ,employeeRowID
        ,shiftRowID
        ,i_DateFrom
        ,i_DateTo
        ,calcNightShift
        ,0
    )
    ON DUPLICATE KEY
    UPDATE
        ShiftID = shiftRowID,
        RestDay = 0,
        LastUpdBy = CreatedLastUpdBy,
        LastUpd=CURRENT_TIMESTAMP();

END IF;





END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
