/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `ComputeNightDiffHours`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `ComputeNightDiffHours`(
    `$dutyStart` DATETIME,
    `$dutyEnd` DATETIME,
    `$nightDiffRangeStart` DATETIME,
    `$nightDiffRangeEnd` DATETIME
) RETURNS decimal(15,4)
    DETERMINISTIC
BEGIN

    DECLARE nightDiffDutyStart DATETIME;
    DECLARE nightDiffDutyEnd DATETIME;
    DECLARE isDutyOverlappingNightDifferential TINYINT(1);
    DECLARE nightDiffHours DECIMAL(15, 4) DEFAULT 0.0;

    /*
     * Let's first check if the employee even worked during the night
     * differential hours. If not, we can skip computing and just return zero.
     */
    SET isDutyOverlappingNightDifferential = (
        ($dutyStart < $nightDiffRangeEnd) AND
        ($dutyEnd > $nightDiffRangeStart)
    );

    IF isDutyOverlappingNightDifferential THEN
        SET nightDiffDutyStart = GREATEST($dutyStart, $nightDiffRangeStart);
        SET nightDiffDutyEnd = LEAST($dutyEnd, $nightDiffRangeEnd);

        SET nightDiffHours = COMPUTE_TimeDifference(TIME(nightDiffDutyStart), TIME(nightDiffDutyEnd));
    END IF;

    RETURN IFNULL(nightDiffHours, 0);

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
