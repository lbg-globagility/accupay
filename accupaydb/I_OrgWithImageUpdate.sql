/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `I_OrgWithImageUpdate`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_OrgWithImageUpdate`(
	IN `I_Name` VARCHAR(100),
	IN `I_TradeName` VARCHAR(100),
	IN `I_PrimaryAddressID` INT(10),
	IN `I_PrimaryContactID` INT(10),
	IN `I_MainPhone` VARCHAR(50),
	IN `I_FaxNumber` VARCHAR(50),
	IN `I_EmailAddress` VARCHAR(50),
	IN `I_AltEmailAddress` VARCHAR(50),
	IN `I_AltPhone` VARCHAR(50),
	IN `I_URL` VARCHAR(50),
	IN `I_LastUpd` DATETIME,
	IN `I_LastUpdBy` INT(11),
	IN `I_TINNo` VARCHAR(50),
	IN `I_OrganizationType` VARCHAR(50),
	IN `I_Image` LONGBLOB,
	IN `I_RowID` INT(11),
	IN `I_VacationLeaveDays` DECIMAL(10,2),
	IN `I_SickLeaveDays` DECIMAL(10,2),
	IN `I_MaternityLeaveDays` DECIMAL(10,2),
	IN `I_OthersLeaveDays` DECIMAL(10,2),
	IN `I_NightDifferentialTimeFrom` TIME,
	IN `I_NightDifferentialTimeTo` TIME,
	IN `I_NightShiftTimeFrom` TIME,
	IN `I_NightShiftTimeTo` TIME,
	IN `I_PhilhealthDeductionSchedule` VARCHAR(50),
	IN `I_SSSDeductionSchedule` VARCHAR(50),
	IN `I_PagIbigDeductionSchedule` VARCHAR(50),
	IN `I_PayFrequencyID` INT,
	IN `I_WorkDaysPerYear` INT,
	IN `I_RDOCode` CHAR(50),
	IN `I_ZIPCode` CHAR(50),
	IN `I_WithholdingDeductionSchedule` CHAR(50),
	IN `I_IsAgency` TINYINT(1)
)
LANGUAGE SQL
DETERMINISTIC
CONTAINS SQL
SQL SECURITY DEFINER
COMMENT ''
BEGIN
UPDATE organization
SET
    Name = I_Name,
    TradeName = I_TradeName,
    PrimaryAddressID = IF(I_PrimaryAddressID=0,NULL,I_PrimaryAddressID),
    PrimaryContactID = IF(I_PrimaryContactID=0,NULL,I_PrimaryContactID),
    MainPhone = I_MainPhone,
    FaxNumber = I_FaxNumber,
    EmailAddress = I_EmailAddress,
    AltEmailAddress = I_AltEmailAddress,
    AltPhone = I_AltPhone,
    URL = I_URL,
    LastUpd = I_LastUpd,
    LastUpdBy = I_LastUpdBy,
    TINNo = I_TINNo,
    OrganizationType = I_OrganizationType,
    Image = I_Image,
    VacationLeaveDays = I_VacationLeaveDays,
    SickLeaveDays = I_SickLeaveDays,
    MaternityLeaveDays = I_MaternityLeaveDays,
    OthersLeaveDays = I_OthersLeaveDays,
    NightDifferentialTimeFrom = I_NightDifferentialTimeFrom,
    NightDifferentialTimeTo = I_NightDifferentialTimeTo,
    NightShiftTimeFrom = I_NightShiftTimeFrom,
    NightShiftTimeTo = I_NightShiftTimeTo,
    PhilhealthDeductionSchedule = I_PhilhealthDeductionSchedule,
    SSSDeductionSchedule = I_SSSDeductionSchedule,
    PagIbigDeductionSchedule = I_PagIbigDeductionSchedule,
    PayFrequencyID = I_PayFrequencyID,
    WorkDaysPerYear = IF(I_WorkDaysPerYear=0, IF(DAY(LAST_DAY(ADDDATE(MAKEDATE(YEAR(CURDATE()),1), INTERVAL 1 MONTH))) <= 28, 260, 261), I_WorkDaysPerYear),
    RDOCode=I_RDOCode,
    ZIPCode=I_ZIPCode,
    WithholdingDeductionSchedule=I_WithholdingDeductionSchedule,
    IsAgency=I_IsAgency
WHERE RowID = I_RowID;END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
