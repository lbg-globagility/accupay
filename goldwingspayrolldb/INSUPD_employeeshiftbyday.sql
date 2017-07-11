/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_employeeshiftbyday`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `INSUPD_employeeshiftbyday`(`OrganizID` INT, `UserRowID` INT, `EmployeeRowID` INT, `ShiftRowID` INT, `NameOfTheDay` VARCHAR(50), `IsNightShift` CHAR(1), `IsRestDay` CHAR(1), `ByOrderValue` INT, `AsNewRecord` CHAR(1)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11);

DECLARE has_fullshift_thisyear CHAR(1);

DECLARE last_shiftdate_thisyear DATE;

SELECT d.DateValue FROM dates d WHERE YEAR(d.DateValue)=YEAR(CURDATE()) AND DAYOFWEEK(d.DateValue)=(((@@default_week_format + 6) MOD 7) + 1) ORDER BY d.DateValue DESC LIMIT 1 INTO last_shiftdate_thisyear;

IF AsNewRecord = '0' THEN

    INSERT INTO `employeeshiftbyday`
    (
        OrganizationID
        ,Created
        ,CreatedBy
        ,EmployeeID
        ,ShiftID
        ,NameOfDay
        ,NightShift
        ,RestDay
        ,OrderByValue
    ) VALUES ( OrganizID
        ,CURRENT_TIMESTAMP()
        ,UserRowID
        ,EmployeeRowID
        ,ShiftRowID
        ,NameOfTheDay
        ,IsNightShift
        ,IsRestDay
        ,ByOrderValue)
    ON
    DUPLICATE
    KEY
    UPDATE
        LastUpd=CURRENT_TIMESTAMP()
        ,LastUpdBy=UserRowID
        ,ShiftID=ShiftRowID
        ,RestDay=IsRestDay;SELECT @@Identity AS ID INTO returnvalue;

ELSEIF AsNewRecord = '1' THEN

    SELECT EXISTS(SELECT RowID FROM employeeshift esh WHERE esh.EmployeeID=EmployeeRowID AND esh.OrganizationID=OrganizID AND last_shiftdate_thisyear BETWEEN esh.EffectiveFrom AND esh.EffectiveTo ORDER BY esh.EffectiveFrom DESC,esh.EffectiveTo DESC LIMIT 1) INTO has_fullshift_thisyear;

    IF has_fullshift_thisyear = '1' THEN

        INSERT INTO `employeeshiftbyday`
        (
            OrganizationID
            ,Created
            ,CreatedBy
            ,EmployeeID
            ,ShiftID
            ,NameOfDay
            ,NightShift
            ,RestDay
            ,OrderByValue
        ) VALUES (OrganizID
            ,CURRENT_TIMESTAMP()
            ,UserRowID
            ,EmployeeRowID
            ,ShiftRowID
            ,NameOfTheDay
            ,IsNightShift
            ,IsRestDay
            ,ByOrderValue
        ) ON
        DUPLICATE
        KEY
        UPDATE
            LastUpd=CURRENT_TIMESTAMP()
            ,LastUpdBy=UserRowID
            ,ShiftID=ShiftRowID
            ,RestDay=IsRestDay
            ,SampleDate=ADDDATE(last_shiftdate_thisyear,INTERVAL (ByOrderValue + 1) DAY);SELECT @@Identity AS ID INTO returnvalue;

    ELSE

        INSERT INTO `employeeshiftbyday`
        (
            OrganizationID
            ,Created
            ,CreatedBy
            ,EmployeeID
            ,ShiftID
            ,NameOfDay
            ,NightShift
            ,RestDay
            ,OrderByValue
        ) VALUES (OrganizID
            ,CURRENT_TIMESTAMP()
            ,UserRowID
            ,EmployeeRowID
            ,ShiftRowID
            ,NameOfTheDay
            ,IsNightShift
            ,IsRestDay
            ,ByOrderValue
        ) ON
        DUPLICATE
        KEY
        UPDATE
            LastUpd=CURRENT_TIMESTAMP()
            ,LastUpdBy=UserRowID
            ,ShiftID=ShiftRowID
            ,RestDay=IsRestDay;SELECT @@Identity AS ID INTO returnvalue;

    END IF;

END IF;

RETURN returnvalue;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
