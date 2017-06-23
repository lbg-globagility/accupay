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

-- Dumping structure for function goldwingspayrolldb.INSUPD_timeentry
DROP FUNCTION IF EXISTS `INSUPD_timeentry`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_timeentry`(`timent_RowID` INT, `timent_OrganizationID` INT, `timent_CreatedBy` INT, `timent_LastUpdBy` INT, `timent_PayPeriodID` INT, `timent_EmployeeID` INT, `timent_TotalPay` DECIMAL(10,2), `timent_TotalHoursWorked` DECIMAL(10,2), `timent_TotalVLConsumed` DECIMAL(10,2), `timent_TotalSLConsumed` DECIMAL(10,2)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE timeentryID INT(11);

INSERT INTO timeentry 
(
	RowID
	,OrganizationID
	,Created
	,CreatedBy
	,LastUpdBy
	,PayPeriodID
	,EmployeeID
	,TotalPay
	,TotalHoursWorked
	,TotalVLConsumed
	,TotalSLConsumed
) VALUES (
	timent_RowID
	,timent_OrganizationID
	,CURRENT_TIMESTAMP()
	,timent_CreatedBy
	,timent_LastUpdBy
	,timent_PayPeriodID
	,timent_EmployeeID
	,timent_TotalPay
	,timent_TotalHoursWorked
	,timent_TotalVLConsumed
	,timent_TotalSLConsumed
) ON 
DUPLICATE 
KEY 
UPDATE 
	LastUpd=CURRENT_TIMESTAMP()
	,LastUpdBy=timent_LastUpdBy
	,PayPeriodID=timent_PayPeriodID
	,TotalPay=timent_TotalPay
	,TotalHoursWorked=timent_TotalHoursWorked
	,TotalVLConsumed=timent_TotalVLConsumed
	,TotalSLConsumed=timent_TotalSLConsumed;



SELECT @@Identity AS id INTO timeentryID;

RETURN timeentryID;



END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
