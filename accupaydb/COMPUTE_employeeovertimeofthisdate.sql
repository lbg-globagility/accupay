/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `COMPUTE_employeeovertimeofthisdate`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `COMPUTE_employeeovertimeofthisdate`(`ot_EmployeeID` INT, `ot_OrganizationID` INT, `ot_Date` DATE, `timelogout` TIME, `ValueAsNightDiff` CHAR(1)) RETURNS decimal(11,4)
    DETERMINISTIC
BEGIN

DECLARE returnvalue DECIMAL(11,4) DEFAULT 0.0;

DECLARE emp_OTTimeIn TIME;

DECLARE emp_OTTimeOut TIME;

DECLARE ndiffStartTime TIME;

DECLARE ndiffEndTime TIME;

DECLARE ndiffValue DECIMAL(11,4) DEFAULT 0.0;


SELECT og.NightDifferentialTimeFrom
, og.NightDifferentialTimeTo
FROM organization og
WHERE og.RowID=ot_OrganizationID
INTO ndiffStartTime, ndiffEndTime;


SELECT
OTStartTime
,OTEndTime
FROM employeeovertime
WHERE EmployeeID=ot_EmployeeID
AND OrganizationID=ot_OrganizationID
AND ot_Date
BETWEEN OTStartDate
AND COALESCE(OTEndDate,OTStartDate)
AND DATEDIFF(ot_Date,COALESCE(OTEndDate,OTStartDate)) >= 0
AND OTStatus='Approved'
ORDER BY DATEDIFF(ot_Date,COALESCE(OTEndDate,OTStartDate))
LIMIT 1
INTO emp_OTTimeIn
      ,emp_OTTimeOut;

IF emp_OTTimeIn IS NULL AND timelogout IS NULL THEN
    SET returnvalue = 0;
ELSE

    IF emp_OTTimeIn > timelogout THEN
        SET returnvalue = 0;
    ELSE

        IF timelogout > emp_OTTimeOut THEN
            SET timelogout = emp_OTTimeOut;
        END IF;

        IF TIME_FORMAT(emp_OTTimeIn,'%p') = 'PM' AND
            TIME_FORMAT(timelogout,'%p') = 'AM' THEN

            SELECT ((TIME_TO_SEC(TIMEDIFF(ADDTIME(timelogout,'24:00'),emp_OTTimeIn)) / 60) / 60) INTO returnvalue;

            IF TIME_FORMAT(timelogout,'%p') = 'PM' AND ValueAsNightDiff ='1' THEN

                IF timelogout >= ndiffStartTime
                    AND timelogout <= ndiffEndTime THEN

                    SELECT ((TIME_TO_SEC(TIMEDIFF(timelogout,ndiffStartTime)) / 60) / 60) INTO ndiffValue;

                ELSE

                    SET ndiffValue = 0;

                END IF;



            ELSE

                IF ndiffEndTime >= timelogout AND ValueAsNightDiff ='1' THEN

                    SELECT ((TIME_TO_SEC(TIMEDIFF(ADDTIME(timelogout,'24:00'),ndiffStartTime)) / 60) / 60) INTO ndiffValue;

                ELSE

                    SET ndiffValue = 0;

                END IF;

            END IF;

        ELSE

            SELECT ((TIME_TO_SEC(TIMEDIFF(timelogout,emp_OTTimeIn)) / 60) / 60) INTO returnvalue;


        END IF;

    END IF;

END IF;

IF ValueAsNightDiff = '1' THEN
    SET returnvalue = IFNULL(ndiffValue, 0.0);

ELSE
    IF ndiffValue IS NULL THEN
        SET returnvalue = 0;

    END IF;

END IF;

RETURN returnvalue;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
