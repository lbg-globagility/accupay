/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `AUTOMATICUPD_employeeshiftbyday`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `AUTOMATICUPD_employeeshiftbyday`(IN `OrganizID` INT, IN `EmployeeRowID` INT)
    DETERMINISTIC
BEGIN

DECLARE ordervaloforigin INT(11);

DECLARE StartingDate DATE;

DECLARE uniqshiftcount INT(11);

DECLARE uniqshiftRowID VARCHAR(100);

DECLARE defaultindex INT(11) DEFAULT 1;

DECLARE indx INT(11) DEFAULT 0;

DECLARE EndingDate DATE DEFAULT LAST_DAY(DATE_FORMAT(CURDATE(),'%Y-12-01'));

DECLARE the_date DATE;

DECLARE hasShiftForThisYear CHAR(1);

DECLARE anyintiger INT(11) DEFAULT 0;

SELECT CAST(@@default_week_format AS INT) INTO anyintiger;
SELECT LAST_DAY(DATE_FORMAT(CURDATE(),'%Y-12-01')) INTO EndingDate;
SELECT IF(EndingDate > DateValue, ADDDATE(DateValue, INTERVAL 1 WEEK), EndingDate)
FROM dates
WHERE YEAR(DateValue) <= YEAR(CURDATE())
AND DAYOFWEEK(DateValue) = IF(anyintiger - 1 < 0, 7, anyintiger)
AND WEEKOFYEAR(DateValue) > 50
ORDER BY DateValue DESC
LIMIT 1
INTO EndingDate;

SELECT StartDate FROM employee WHERE RowID=EmployeeRowID AND OrganizationID=OrganizID INTO StartingDate;

IF TIMESTAMPDIFF(YEAR,StartingDate,CURDATE()) > 1 THEN


    SELECT DateValue FROM dates WHERE YEAR(DateValue)=YEAR(CURDATE()) AND DAYOFWEEK(DateValue)=(@@default_week_format + 1) ORDER BY DateValue LIMIT 1 INTO StartingDate;

END IF;

SELECT EXISTS(SELECT RowID FROM employeeshift WHERE EmployeeID=EmployeeRowID AND OrganizationID=OrganizID AND (EffectiveFrom >= StartingDate OR EffectiveTo >= StartingDate) AND (EffectiveFrom <= EndingDate OR EffectiveTo <= EndingDate)) INTO hasShiftForThisYear;

IF hasShiftForThisYear = '1' THEN

    SELECT ADDDATE(EffectiveTo, INTERVAL 1 DAY) FROM employeeshift WHERE EmployeeID=EmployeeRowID AND OrganizationID=OrganizID ORDER BY EffectiveFrom DESC, EffectiveTo DESC LIMIT 1 INTO StartingDate;

END IF;


SET @uniqueshift = 0;

SET @indxcount = 0;

IF StartingDate < EndingDate AND hasShiftForThisYear = '0' THEN

    SELECT OrderByValue,SampleDate FROM employeeshiftbyday WHERE EmployeeID=EmployeeRowID AND OrganizationID=OrganizID AND OriginDay=0 LIMIT 1 INTO ordervaloforigin,the_date;


    UPDATE employeeshiftbyday esb
    SET esb.OriginDay = (esb.OrderByValue - ordervaloforigin)
    WHERE esb.EmployeeID=EmployeeRowID
    AND esb.OrganizationID=OrganizID
    ORDER BY esb.OrderByValue;

    UPDATE employeeshiftbyday esb
    SET esb.SampleDate=ADDDATE(the_date,INTERVAL esb.OriginDay DAY)
    WHERE esb.EmployeeID=EmployeeRowID
    AND esb.OrganizationID=OrganizID
    ORDER BY esb.OrderByValue;



    UPDATE employeeshiftbyday esd
    INNER JOIN (
                    SELECT *
                    ,(@indxcount := @indxcount + 1) AS IncrementUnique
                    FROM (
                            SELECT esd.RowID,esd.NameOfDay
                            ,IFNULL(esd.ShiftID,esd.NameOfDay) AS ShiftID
                            FROM employeeshiftbyday esd
                            WHERE esd.EmployeeID=EmployeeRowID
                            AND esd.OrganizationID=OrganizID

                            GROUP BY IFNULL(esd.ShiftID,esd.NameOfDay)
                            ORDER BY esd.SampleDate
                    ) i
    ) esdd ON IFNULL(esdd.ShiftID,esdd.NameOfDay) = IFNULL(esd.ShiftID,esd.NameOfDay)
    SET esd.UniqueShift=esdd.IncrementUnique
    WHERE esd.EmployeeID=EmployeeRowID
    AND esd.OrganizationID=OrganizID;

ELSE



    UPDATE employeeshiftbyday esd
    INNER JOIN (
                    SELECT *
                    ,(@indxcount := @indxcount + 1) AS IncrementUnique
                    FROM (
                            SELECT esd.RowID,esd.NameOfDay
                            ,IFNULL(esd.ShiftID,esd.NameOfDay) AS ShiftID
                            FROM employeeshiftbyday esd
                            WHERE esd.EmployeeID=EmployeeRowID
                            AND esd.OrganizationID=OrganizID

                            GROUP BY IFNULL(esd.ShiftID,esd.NameOfDay)
                            ORDER BY esd.SampleDate
                    ) i
    ) esdd ON IFNULL(esdd.ShiftID,esdd.NameOfDay) = IFNULL(esd.ShiftID,esd.NameOfDay)
    SET esd.UniqueShift=esdd.IncrementUnique
    WHERE esd.EmployeeID=EmployeeRowID
    AND esd.OrganizationID=OrganizID;

END IF;

    INSERT INTO employeefirstweekshift
    (

        OrganizationID
        ,CreatedBy
        ,EmployeeID
        ,ShiftID
        ,EffectiveFrom
        ,EffectiveTo
        ,NightShift
        ,RestDay
        ,IsEncodedByDay
    )   SELECT

        esd.OrganizationID
        ,esd.CreatedBy
        ,esd.EmployeeID
        ,esd.ShiftID
        ,esd.SampleDate
        ,ADDDATE(esd.SampleDate,INTERVAL (COUNT(RowID) - 1) DAY)
        ,esd.NightShift
        ,esd.RestDay
        ,esd.IsEncodedByDay
        FROM employeeshiftbyday esd

        WHERE esd.EmployeeID=EmployeeRowID
        GROUP BY esd.UniqueShift
        HAVING esd.OrganizationID = OrganizID;






END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
