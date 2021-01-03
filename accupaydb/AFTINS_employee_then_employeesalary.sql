/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTINS_employee_then_employeesalary`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_employee_then_employeesalary` AFTER INSERT ON `employee` FOR EACH ROW BEGIN

DECLARE exist_empstatus INT(1);

DECLARE exist_emptype INT(1);

DECLARE exist_empsalutat INT(1);

DECLARE anyint INT(11);

INSERT INTO employeechecklist
(
    OrganizationID
    ,Created
    ,CreatedBy
    ,EmployeeID
    ,PerformanceAppraisal
    ,BIRTIN
    ,Diploma
    ,IDInfoSlip
    ,PhilhealthID
    ,HDMFID
    ,SSSNo
    ,TranscriptOfRecord
    ,BirthCertificate
    ,EmployeeContract
    ,MedicalExam
    ,NBIClearance
    ,COEEmployer
    ,MarriageContract
    ,HouseSketch
    ,TrainingAgreement
    ,HealthPermit
    ,ValidID
    ,Resume
) VALUES (
    NEW.OrganizationID
    ,CURRENT_TIMESTAMP()
    ,NEW.CreatedBy
    ,NEW.RowID
    ,0
    ,IF(COALESCE(NEW.TINNo,'   -   -   -') = '   -   -   -', 0, 1)
    ,0
    ,0
    ,IF(COALESCE(NEW.PhilHealthNo,'    -    -') = '    -    -', 0, 1)
    ,IF(COALESCE(NEW.HDMFNo,'    -    -') = '    -    -', 0, 1)
    ,IF(COALESCE(NEW.SSSNo,'  -       -') = '  -       -', 0, 1)
    ,0
    ,0
    ,0
    ,0
    ,0
    ,0
    ,0
    ,0
    ,0
    ,0
    ,0
    ,0
);

SELECT EXISTS(SELECT RowID FROM listofval WHERE Type='Employment Status' AND Active='Yes' AND DisplayValue=NEW.EmploymentStatus) INTO exist_empstatus;

IF exist_empstatus = 0 AND NEW.EmploymentStatus!='' THEN

    INSERT INTO listofval (`Type`,DisplayValue,Active,CreatedBy,Created,LastUpdBy,LastUpd) VALUES ('Employment Status',NEW.EmploymentStatus,'Yes',NEW.CreatedBy,CURRENT_TIMESTAMP(),NEW.CreatedBy,CURRENT_TIMESTAMP()) ON DUPLICATE KEY UPDATE LastUpd=CURRENT_TIMESTAMP();

END IF;

SELECT EXISTS(SELECT RowID FROM listofval WHERE Type='Employee Type' AND Active='Yes' AND DisplayValue=NEW.EmployeeType) INTO exist_emptype;

IF exist_emptype = 0 AND NEW.EmployeeType!='' THEN

    INSERT INTO listofval (`Type`,DisplayValue,Active,CreatedBy,Created,LastUpdBy,LastUpd) VALUES ('Employee Type',NEW.EmployeeType,'Yes',NEW.CreatedBy,CURRENT_TIMESTAMP(),NEW.CreatedBy,CURRENT_TIMESTAMP()) ON DUPLICATE KEY UPDATE LastUpd=CURRENT_TIMESTAMP();

END IF;



SELECT EXISTS(SELECT RowID FROM listofval WHERE Type='Salutation' AND Active='Yes' AND DisplayValue=NEW.Salutation) INTO exist_empsalutat;

IF exist_empsalutat = 0 AND NEW.Salutation!='' THEN

    INSERT INTO listofval (`Type`,DisplayValue,Active,CreatedBy,Created,LastUpdBy,LastUpd) VALUES ('Salutation',NEW.Salutation,'Yes',NEW.CreatedBy,CURRENT_TIMESTAMP(),NEW.CreatedBy,CURRENT_TIMESTAMP()) ON DUPLICATE KEY UPDATE LastUpd=CURRENT_TIMESTAMP();

END IF;


IF NEW.BankName IS NOT NULL THEN

    SELECT `INSUPD_listofval`(NEW.BankName,NEW.BankName, 'Bank Names',NEW.BankName, 'Yes', NEW.BankName, NEW.CreatedBy, '1') INTO anyint;

END IF;

CALL AUTOINS_leaveledger(NEW.OrganizationID, NEW.RowID, NEW.CreatedBy);

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
