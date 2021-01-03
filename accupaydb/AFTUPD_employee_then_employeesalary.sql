/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTUPD_employee_then_employeesalary`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTUPD_employee_then_employeesalary` AFTER UPDATE ON `employee` FOR EACH ROW BEGIN

DECLARE emp_chklist_ID INT(11);

DECLARE NEW_agfee DECIMAL(11,2) DEFAULT 0;

DECLARE OLD_agfee DECIMAL(11,2) DEFAULT 0;

DECLARE agfee_percent DECIMAL(11,2) DEFAULT 0;

SELECT RowID FROM employeechecklist WHERE EmployeeID=NEW.RowID ORDER BY RowID DESC LIMIT 1 INTO emp_chklist_ID;

INSERT INTO employeechecklist
(
    RowID
    ,OrganizationID
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
    emp_chklist_ID
    ,NEW.OrganizationID
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
) ON
DUPLICATE
KEY
UPDATE
    LastUpd=CURRENT_TIMESTAMP()
    ,LastUpdBy=NEW.LastUpdBy
    ,PerformanceAppraisal=0
    ,BIRTIN=IF(COALESCE(NEW.TINNo,'   -   -   -') = '   -   -   -', 0, 1)
    ,Diploma=0
    ,IDInfoSlip=0
    ,PhilhealthID=IF(COALESCE(NEW.PhilHealthNo,'    -    -') = '    -    -', 0, 1)
    ,HDMFID=IF(COALESCE(NEW.HDMFNo,'    -    -') = '    -    -', 0, 1)
    ,SSSNo=IF(COALESCE(NEW.SSSNo,'  -       -') = '  -       -', 0, 1)
    ,TranscriptOfRecord=0
    ,BirthCertificate=0
    ,EmployeeContract=0
    ,MedicalExam=0
    ,NBIClearance=0
    ,COEEmployer=0
    ,MarriageContract=0
    ,HouseSketch=0
    ,TrainingAgreement=0
    ,HealthPermit=0
    ,ValidID=0
    ,Resume=0;

IF IFNULL(OLD.AgencyID,0) != IFNULL(NEW.AgencyID,0) THEN

    SELECT ag.`AgencyFee` FROM agency ag WHERE ag.RowID=OLD.AgencyID INTO OLD_agfee;

    SET OLD_agfee = IFNULL(OLD_agfee,0.0);

    SELECT ag.`AgencyFee` FROM agency ag WHERE ag.RowID=NEW.AgencyID INTO NEW_agfee;

    SET NEW_agfee = IFNULL(NEW_agfee,0.0);

    SET agfee_percent = NEW_agfee / OLD_agfee;



    IF agfee_percent != 1.0 THEN

        UPDATE agencyfee agf
        SET agf.DailyFee = agf.DailyFee * agfee_percent
        WHERE agf.OrganizationID=NEW.OrganizationID
        AND agf.AgencyID=NEW.AgencyID
        AND agf.EmployeeID=NEW.RowID
        AND agf.EmpPositionID=NEW.PositionID;

    END IF;

END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
