/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP VIEW IF EXISTS `employeeshift_withshiftimestamp`;
CREATE TABLE `employeeshift_withshiftimestamp` (
	`RowID` INT(10) NOT NULL,
	`OrganizationID` INT(10) NOT NULL,
	`Created` TIMESTAMP NOT NULL,
	`CreatedBy` INT(10) NULL,
	`LastUpd` DATETIME NULL,
	`LastUpdBy` INT(10) NULL,
	`EmployeeID` INT(10) NULL,
	`ShiftID` INT(10) NULL,
	`EffectiveFrom` DATE NULL,
	`EffectiveTo` DATE NULL,
	`NightShift` TINYINT(1) NOT NULL,
	`RestDay` TINYINT(1) NOT NULL,
	`IsEncodedByDay` CHAR(1) NULL COLLATE 'latin1_swedish_ci',
	`DateValue` DATE NULL,
	`DateTimeFrom` DATETIME NULL,
	`DateTimeTo` DATETIME NULL
) ENGINE=MyISAM;

DROP VIEW IF EXISTS `employeeshift_withshiftimestamp`;
DROP TABLE IF EXISTS `employeeshift_withshiftimestamp`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `employeeshift_withshiftimestamp` AS SELECT esh.*

, d.DateValue
, CONCAT_DATETIME(d.DateValue, sh.TimeFrom) `DateTimeFrom`
, CONCAT_DATETIME(ADDDATE(d.DateValue, INTERVAL IS_TIMERANGE_REACHTOMORROW(sh.TimeFrom, sh.TimeTo) DAY), sh.TimeTo) `DateTimeTo`

FROM employeeshift esh
INNER JOIN dates d ON d.DateValue BETWEEN esh.EffectiveFrom AND esh.EffectiveTo
LEFT JOIN shift sh ON sh.RowID=esh.ShiftID ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
