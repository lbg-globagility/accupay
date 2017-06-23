-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.11-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.0.0.4396
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for procedure goldwingspayrolldb.RECOMPUTE_agencytotalbill
DROP PROCEDURE IF EXISTS `RECOMPUTE_agencytotalbill`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `RECOMPUTE_agencytotalbill`(IN `OrganizID` INT, IN `FromPayDate` DATE, IN `ToPayDate` DATE, IN `UserRowID` INT)
    DETERMINISTIC
BEGIN

DECLARE payperiod_rowid INT(11);

SELECT RowID
FROM payperiod
WHERE OrganizationID=OrganizID
AND PayFromDate=FromPayDate
AND PayToDate=ToPayDate
INTO payperiod_rowid;



INSERT INTO agencytotalbill
(
	RowID
	,OrganizationID
	,Created
	,CreatedBy
	,AgencyID
	,PayPeriodID
	,TotalBilled
) SELECT
	ii.atbRowID
	,OrganizID
	,CURRENT_TIMESTAMP()
	,UserRowID
	,ii.AgencyID
	,payperiod_rowid
	,ii.SumDailyFee
	FROM (
		SELECT
		atb.RowID AS atbRowID
		,agf.AgencyID
		,SUM(IFNULL(agf.DailyFee,0)) AS SumDailyFee
		FROM agencyfee agf
		LEFT JOIN agencytotalbill atb ON atb.AgencyID=agf.AgencyID AND atb.PayPeriodID=payperiod_rowid AND atb.OrganizationID=OrganizID
		WHERE IFNULL(agf.DailyFee,0)!=0 AND agf.OrganizationID=OrganizID AND agf.TimeEntryDate BETWEEN FromPayDate AND ToPayDate
		GROUP BY agf.AgencyID
	) ii
ON
DUPLICATE
KEY
UPDATE
	LastUpd=CURRENT_TIMESTAMP()
	,LastUpdBy=UserRowID
	,TotalBilled=ii.SumDailyFee;
	
END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
