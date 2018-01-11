/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `MASSUPD_NewPhilHealthDeduction`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `MASSUPD_NewPhilHealthDeduction`(IN `org_rowid` INT)
    DETERMINISTIC
BEGIN

DECLARE year_ofeffect INT(11) DEFAULT 2018;

DECLARE phh_rate DECIMAL(11, 4) DEFAULT 0.0275;

DECLARE default_phh_contrib DECIMAL(11, 4) DEFAULT 275;

DECLARE min_phh_contrib
        ,max_phh_contrib DECIMAL(11, 4);

DECLARE effect_fromdate
        , effect_todate DATE;

SET min_phh_contrib = default_phh_contrib;

SET max_phh_contrib = 1100;

SELECT pp.PayFromDate, pp.PayToDate
FROM payperiod pp
WHERE pp.OrganizationID = org_rowid
AND pp.TotalGrossSalary = 1
AND pp.`Year` = year_ofeffect
AND pp.OrdinalValue = 1
LIMIT 1
INTO effect_fromdate
     ,effect_todate;

UPDATE employeesalary esa
INNER JOIN (SELECT ii.RowID
            , ii.MonthlySalary
            , ROUND((ii.MonthlySalary * phh_rate), 4) `ComputedPhilHealth`
            FROM employeesalary_withdailyrate ii
            WHERE ii.OrganizationID = org_rowid
            ) i ON i.RowID = esa.RowID
SET
esa.LastUpd = CURRENT_TIMESTAMP()
,esa.LastUpdBy = IFNULL(esa.LastUpdBy, esa.CreatedBy)
,esa.PhilHealthDeduction = IFNULL(min_phh_contrib, default_phh_contrib)
WHERE i.`ComputedPhilHealth` < min_phh_contrib
AND ((effect_fromdate >= esa.EffectiveDateFrom OR effect_fromdate >= esa.EffectiveDateTo)
     OR (effect_todate >= esa.EffectiveDateFrom OR effect_todate >= esa.EffectiveDateTo))
;

UPDATE employeesalary esa
INNER JOIN (SELECT ii.RowID
            , ii.MonthlySalary
            , ROUND((ii.MonthlySalary * phh_rate), 4) `ComputedPhilHealth`
            FROM employeesalary_withdailyrate ii
            WHERE ii.OrganizationID = org_rowid
            ) i ON i.RowID = esa.RowID
SET
esa.LastUpd = CURRENT_TIMESTAMP()
,esa.LastUpdBy = IFNULL(esa.LastUpdBy, esa.CreatedBy)
,esa.PhilHealthDeduction = IFNULL(i.`ComputedPhilHealth`, default_phh_contrib)
WHERE i.`ComputedPhilHealth` BETWEEN min_phh_contrib AND max_phh_contrib
AND ((effect_fromdate >= esa.EffectiveDateFrom OR effect_fromdate >= esa.EffectiveDateTo)
     OR (effect_todate >= esa.EffectiveDateFrom OR effect_todate >= esa.EffectiveDateTo))
;

UPDATE employeesalary esa
INNER JOIN (SELECT ii.RowID
            , ii.MonthlySalary
            , ROUND((ii.MonthlySalary * phh_rate), 4) `ComputedPhilHealth`
            FROM employeesalary_withdailyrate ii
            WHERE ii.OrganizationID = org_rowid
            ) i ON i.RowID = esa.RowID
SET
esa.LastUpd = CURRENT_TIMESTAMP()
,esa.LastUpdBy = IFNULL(esa.LastUpdBy, esa.CreatedBy)
,esa.PhilHealthDeduction = IFNULL(max_phh_contrib, default_phh_contrib)
WHERE i.`ComputedPhilHealth` > max_phh_contrib
AND ((effect_fromdate >= esa.EffectiveDateFrom OR effect_fromdate >= esa.EffectiveDateTo)
     OR (effect_todate >= esa.EffectiveDateFrom OR effect_todate >= esa.EffectiveDateTo))
;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
