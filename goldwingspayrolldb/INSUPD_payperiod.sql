-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.12-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for function INSUPD_payperiod
DROP FUNCTION IF EXISTS `INSUPD_payperiod`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_payperiod`(`payp_RowID` INT, `payp_OrganizationID` INT, `payp_CreatedBy` INT, `payp_LastUpdBy` INT, `payp_PayFromDate` DATE, `payp_PayToDate` DATE, `payp_TotalGrossSalary` DECIMAL(10,2), `payp_TotalNetSalary` DECIMAL(10,2), `payp_TotalEmpSSS` DECIMAL(10,2), `payp_TotalEmpWithholdingTax` DECIMAL(10,2), `payp_TotalCompSSS` DECIMAL(10,2), `payp_TotalEmpPhilhealth` DECIMAL(10,2), `payp_TotalCompPhilhealth` DECIMAL(10,2), `payp_TotalEmpHDMF` DECIMAL(10,2), `payp_TotalCompHDMF` DECIMAL(10,2)) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE payperiodID INT(11);

INSERT INTO payperiod
(
    RowID
    ,OrganizationID
    ,Created
    ,CreatedBy
    ,LastUpdBy
    ,PayFromDate
    ,PayToDate
    ,TotalGrossSalary
    ,TotalNetSalary
    ,TotalEmpSSS
    ,TotalEmpWithholdingTax
    ,TotalCompSSS
    ,TotalEmpPhilhealth
    ,TotalCompPhilhealth
    ,TotalEmpHDMF
    ,TotalCompHDMF
) VALUES (
    payp_RowID
    ,payp_OrganizationID
    ,CURRENT_TIMESTAMP()
    ,payp_CreatedBy
    ,payp_LastUpdBy
    ,payp_PayFromDate
    ,payp_PayToDate
    ,payp_TotalGrossSalary
    ,payp_TotalNetSalary
    ,payp_TotalEmpSSS
    ,payp_TotalEmpWithholdingTax
    ,payp_TotalCompSSS
    ,payp_TotalEmpPhilhealth
    ,payp_TotalCompPhilhealth
    ,payp_TotalEmpHDMF
    ,payp_TotalCompHDMF
) ON
DUPLICATE
KEY UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=payp_LastUpdBy
    ,PayFromDate=payp_PayFromDate
    ,PayToDate=payp_PayToDate
    ,TotalGrossSalary=payp_TotalGrossSalary
    ,TotalNetSalary=payp_TotalNetSalary
    ,TotalEmpSSS=payp_TotalEmpSSS
    ,TotalEmpWithholdingTax=payp_TotalEmpWithholdingTax
    ,TotalCompSSS=payp_TotalCompSSS
    ,TotalEmpPhilhealth=payp_TotalEmpPhilhealth
    ,TotalCompPhilhealth=payp_TotalCompPhilhealth
    ,TotalEmpHDMF=payp_TotalEmpHDMF
    ,TotalCompHDMF=payp_TotalCompHDMF;SELECT @@Identity AS id INTO payperiodID;

RETURN payperiodID;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
