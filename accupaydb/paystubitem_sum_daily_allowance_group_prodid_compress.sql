/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP VIEW IF EXISTS `paystubitem_sum_daily_allowance_group_prodid_compress`;
CREATE TABLE `paystubitem_sum_daily_allowance_group_prodid_compress` (
	`etRowID` INT(10) NOT NULL,
	`eaRowID` INT(10) NOT NULL,
	`ProductID` INT(10) NULL,
	`EmployeeID` INT(10) NULL,
	`OrganizationID` INT(10) NOT NULL,
	`Date` DATE NOT NULL COMMENT 'time entry date',
	`Column1` INT(1) NOT NULL,
	`TaxableFlag` CHAR(50) NULL COLLATE 'latin1_swedish_ci',
	`PayType` VARCHAR(50) NULL COMMENT 'Regular, Holiday, Special Holiday, etc.' COLLATE 'latin1_swedish_ci',
	`TotalAllowanceAmt` DECIMAL(30,8) NULL,
	`Fixed` TINYINT(1) NULL
) ENGINE=MyISAM;

DROP VIEW IF EXISTS `paystubitem_sum_daily_allowance_group_prodid_compress`;
DROP TABLE IF EXISTS `paystubitem_sum_daily_allowance_group_prodid_compress`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` VIEW `paystubitem_sum_daily_allowance_group_prodid_compress` AS SELECT
    DISTINCT(et.RowID) AS `etRowID`,
    ea.RowID AS `eaRowID`,
    (ea.ProductID) AS ProductID,
    et.EmployeeID,
    et.OrganizationID,
    et.`Date`,
    0 AS Column1,
    ea.TaxableFlag,
    pr.PayType,
    (
        (
            IF(
                pr.PayType = 'Regular Day',
                (
                    IF(
                        es.RestDay = '0',
                        (
                            (et.VacationLeaveHours + et.SickLeaveHours + et.MaternityLeaveHours + et.OtherLeaveHours + et.RegularHoursWorked) / sh.DivisorToDailyRate
                        ) * ea.AllowanceAmount,
                        IF(
                            et.TotalHoursWorked > 0,
                            ea.AllowanceAmount,
                            0
                        )
                    )
                ),
                IF(
                    pr.PayType = 'Special Non-Working Holiday',
                    (
                        IF(e.EmployeeType = 'Daily'
                           , IF(et.RegularHoursWorked > 0,
		                          ea.AllowanceAmount,
		                          0
		                          )
		                     # this serves for Monthly-type employees
									, IF(EXISTS(SELECT RowID
									            FROM listofval
													WHERE LIC = 'EcolaCompressed'
													AND `Type` = 'MiscAllowance'
													AND DisplayValue = '1') = FALSE
									     , IFNULL(sh.DivisorToDailyRate, 8)
										  , IFNULL((TIMESTAMPDIFF(SECOND
										                  , CONCAT_DATETIME(et.`Date`, sh.TimeFrom)
																, CONCAT_DATETIME(ADDDATE(et.`Date`, INTERVAL IS_TIMERANGE_REACHTOMORROW(sh.TimeFrom, sh.TimeTo) DAY), sh.TimeTo)) / 3600
										            )
										           , 8))
								   )
                    ),
                    IF(
                        pr.PayType = 'Regular Holiday',
                        (
                            COALESCE(
                                ((et.RegularHoursWorked / sh.DivisorToDailyRate) * ea.AllowanceAmount),
                                0
                            ) +
                            (
                                IF(
                                    HasWorkedLastWorkingDay(e.RowID, et.Date),
                                    ea.AllowanceAmount,
                                    0
                                )
                            )
                        ),
                        0 
                    )
                )
            )
        )
    ) AS TotalAllowanceAmt,
    p.`Fixed`
FROM employeetimeentry et
INNER JOIN employee e
ON e.OrganizationID = et.OrganizationID AND
    e.RowID = et.EmployeeID AND
    e.EmploymentStatus NOT IN ('Resigned','Terminated')
LEFT JOIN employeeshift es
ON es.RowID = et.EmployeeShiftID
LEFT JOIN shift sh
ON sh.RowID = es.ShiftID
INNER JOIN employeeallowance ea
ON ea.AllowanceFrequency = 'Daily' AND
    ea.EmployeeID = e.RowID AND
    ea.OrganizationID = e.OrganizationID AND
    et.`Date` BETWEEN ea.EffectiveStartDate AND ea.EffectiveEndDate
INNER JOIN product p
ON p.RowID = ea.ProductID
INNER JOIN payrate pr
ON pr.RowID = et.PayRateID;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
