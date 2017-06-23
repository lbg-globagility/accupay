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

-- Dumping structure for function goldwingspayrolldb.INSUPD_bonusloandeduction
DROP FUNCTION IF EXISTS `INSUPD_bonusloandeduction`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `INSUPD_bonusloandeduction`(`bld_RowID` INT, `OrganizID` INT, `UserRowID` INT, `bld_LoanSchedID` INT, `bld_PayPeriodID` INT, `bld_DeductionLoanAmount` DECIMAL(10,2)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11);

INSERT INTO bonusloandeduction
(
	RowID
	,OrganizationID
	,CreatedBy
	,LoanSchedID
	,PayPeriodID
	,DeductionLoanAmount
) VALUES (
	bld_RowID
	,OrganizID
	,UserRowID
	,bld_LoanSchedID
	,bld_PayPeriodID
	,bld_DeductionLoanAmount
) ON
DUPLICATE
KEY
UPDATE
	LastUpdBy=UserRowID;SELECT @@Identity AS ID INTO returnvalue;

RETURN returnvalue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
