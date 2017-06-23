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

-- Dumping structure for function goldwingspayrolldb.INSUPD_employeeallowance
DROP FUNCTION IF EXISTS `INSUPD_employeeallowance`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_employeeallowance`(`eall_RowID` INT, `eall_OrganizationID` INT, `eall_EmployeeID` INT, `eall_CreatedBy` INT, `eall_LastUpdBy` INT, `eall_ProductID` INT, `eall_AllowanceFrequency` VARCHAR(50), `eall_EffectiveStartDate` DATE, `eall_EffectiveEndDate` DATE, `eall_Amount` DECIMAL(11,6)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE eallow_RowID INT(11);

INSERT INTO employeeallowance
(
	RowID
	,OrganizationID
	,Created
	,CreatedBy
	,EmployeeID
	,ProductID
	,EffectiveStartDate
	,AllowanceFrequency
	,EffectiveEndDate
	,TaxableFlag
	,AllowanceAmount
) VALUES (
	eall_RowID
	,eall_OrganizationID
	,CURRENT_TIMESTAMP()
	,eall_CreatedBy
	,eall_EmployeeID
	,eall_ProductID
	,eall_EffectiveStartDate
	,eall_AllowanceFrequency
	,eall_EffectiveEndDate
	,(SELECT `Status` FROM product WHERE RowID=eall_ProductID)
	,eall_Amount
) ON
DUPLICATE
KEY
UPDATE
	LastUpd=CURRENT_TIMESTAMP()
	,LastUpdBy=eall_LastUpdBy
	,EmployeeID=eall_EmployeeID
	,ProductID=eall_ProductID
	,EffectiveStartDate=eall_EffectiveStartDate
	,AllowanceFrequency=eall_AllowanceFrequency
	,EffectiveEndDate=eall_EffectiveEndDate
	,TaxableFlag=(SELECT `Status` FROM product WHERE RowID=eall_ProductID)
	,AllowanceAmount=eall_Amount;SELECT @@Identity AS ID INTO eallow_RowID;

RETURN eallow_RowID;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
