/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTUPD_organization`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_organization` AFTER UPDATE ON `organization` FOR EACH ROW BEGIN

DECLARE view_RowID INT(11);

DECLARE INS_audit_ID INT(11);



    SELECT RowID FROM `view` WHERE ViewName='Organization' AND OrganizationID=NEW.RowID INTO view_RowID;



SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'Name',NEW.RowID,OLD.Name,NEW.Name,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'TradeName',NEW.RowID,OLD.TradeName,NEW.TradeName,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'PrimaryAddressID',NEW.RowID,OLD.PrimaryAddressID,NEW.PrimaryAddressID,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'PrimaryContactID',NEW.RowID,OLD.PrimaryContactID,NEW.PrimaryContactID,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'PremiseAddressID',NEW.RowID,OLD.PremiseAddressID,NEW.PremiseAddressID,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'MainPhone',NEW.RowID,OLD.MainPhone,NEW.MainPhone,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'FaxNumber',NEW.RowID,OLD.FaxNumber,NEW.FaxNumber,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'EmailAddress',NEW.RowID,OLD.EmailAddress,NEW.EmailAddress,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'AltEmailAddress',NEW.RowID,OLD.AltEmailAddress,NEW.AltEmailAddress,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'AltPhone',NEW.RowID,OLD.AltPhone,NEW.AltPhone,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'URL',NEW.RowID,OLD.URL,NEW.URL,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'TINNo',NEW.RowID,OLD.TINNo,NEW.TINNo,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'BankAccountNo',NEW.RowID,OLD.BankAccountNo,NEW.BankAccountNo,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'BankName',NEW.RowID,OLD.BankName,NEW.BankName,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'OrganizationType',NEW.RowID,OLD.OrganizationType,NEW.OrganizationType,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'TotalFloorArea',NEW.RowID,OLD.TotalFloorArea,NEW.TotalFloorArea,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'MinimumWater',NEW.RowID,OLD.MinimumWater,NEW.MinimumWater,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'VacationLeaveDays',NEW.RowID,OLD.VacationLeaveDays,NEW.VacationLeaveDays,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'SickLeaveDays',NEW.RowID,OLD.SickLeaveDays,NEW.SickLeaveDays,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'MaternityLeaveDays',NEW.RowID,OLD.MaternityLeaveDays,NEW.MaternityLeaveDays,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'OthersLeaveDays',NEW.RowID,OLD.OthersLeaveDays,NEW.OthersLeaveDays,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'STPFlag',NEW.RowID,OLD.STPFlag,NEW.STPFlag,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'PayFrequencyID',NEW.RowID,OLD.PayFrequencyID,NEW.PayFrequencyID,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'PhilhealthDeductionSchedule',NEW.RowID,OLD.PhilhealthDeductionSchedule,NEW.PhilhealthDeductionSchedule,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'SSSDeductionSchedule',NEW.RowID,OLD.SSSDeductionSchedule,NEW.SSSDeductionSchedule,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'PagIbigDeductionSchedule',NEW.RowID,OLD.PagIbigDeductionSchedule,NEW.PagIbigDeductionSchedule,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'ReportText',NEW.RowID,OLD.ReportText,NEW.ReportText,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'NightDifferentialTimeFrom',NEW.RowID,OLD.NightDifferentialTimeFrom,NEW.NightDifferentialTimeFrom,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'NightDifferentialTimeTo',NEW.RowID,OLD.NightDifferentialTimeTo,NEW.NightDifferentialTimeTo,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'NightShiftTimeFrom',NEW.RowID,OLD.NightShiftTimeFrom,NEW.NightShiftTimeFrom,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'NightShiftTimeTo',NEW.RowID,OLD.NightShiftTimeTo,NEW.NightShiftTimeTo,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'AllowNegativeLeaves',NEW.RowID,OLD.AllowNegativeLeaves,NEW.AllowNegativeLeaves,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'LimitedAccess',NEW.RowID,OLD.LimitedAccess,NEW.LimitedAccess,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'WorkDaysPerYear',NEW.RowID,OLD.WorkDaysPerYear,NEW.WorkDaysPerYear,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.LastUpdBy,NEW.LastUpdBy,NEW.RowID,view_RowID,'RDOCode',NEW.RowID,OLD.RDOCode,NEW.RDOCode,'Update') INTO INS_audit_ID;

SELECT `INS_audittrail_RETRowID`(NEW.CreatedBy,NEW.CreatedBy,NEW.RowID,view_RowID,'ZIPCode',NEW.RowID,OLD.ZIPCode,NEW.ZIPCode,'Update') INTO INS_audit_ID;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
