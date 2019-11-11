/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTINS_employeeattachment_then_employeechecklist`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_employeeattachment_then_employeechecklist` AFTER INSERT ON `employeeattachments` FOR EACH ROW BEGIN

DECLARE OrganizID INT(11);

DECLARE empchklstID INT(11);

DECLARE lovchklstID INT(11);

DECLARE viewID INT(11);

SELECT RowID FROM listofval WHERE DisplayValue=NEW.Type AND Type='Employee Checklist' LIMIT 1 INTO lovchklstID;

SELECT RowID FROM employeechecklist WHERE EmployeeID=NEW.EmployeeID AND Created<=NEW.Created LIMIT 1 INTO empchklstID;

SET lovchklstID = COALESCE(lovchklstID,0);

IF lovchklstID = 367 THEN
    UPDATE employeechecklist SET
    PerformanceAppraisal='1'
    WHERE RowID=empchklstID;
ELSEIF lovchklstID = 368 THEN
    UPDATE employeechecklist SET
    BIRTIN='1'
    WHERE RowID=empchklstID;
ELSEIF lovchklstID = 369 THEN
    UPDATE employeechecklist SET
    Diploma='1'
    WHERE RowID=empchklstID;
ELSEIF lovchklstID = 370 THEN
    UPDATE employeechecklist SET
    IDInfoSlip='1'
    WHERE RowID=empchklstID;
ELSEIF lovchklstID = 371 THEN
    UPDATE employeechecklist SET
    PhilhealthID='1'
    WHERE RowID=empchklstID;
ELSEIF lovchklstID = 372 THEN
    UPDATE employeechecklist SET
    HDMFID='1'
    WHERE RowID=empchklstID;
ELSEIF lovchklstID = 373 THEN
    UPDATE employeechecklist SET
    SSSNo='1'
    WHERE RowID=empchklstID;
ELSEIF lovchklstID = 374 THEN
    UPDATE employeechecklist SET
    TranscriptOfRecord='1'
    WHERE RowID=empchklstID;
ELSEIF lovchklstID = 375 THEN
    UPDATE employeechecklist SET
    BirthCertificate='1'
    WHERE RowID=empchklstID;
ELSEIF lovchklstID = 376 THEN
    UPDATE employeechecklist SET
    EmployeeContract='1'
    WHERE RowID=empchklstID;
ELSEIF lovchklstID = 377 THEN
    UPDATE employeechecklist SET
    MedicalExam='1'
    WHERE RowID=empchklstID;
ELSEIF lovchklstID = 378 THEN
    UPDATE employeechecklist SET
    COEEmployer='1'
    WHERE RowID=empchklstID;
ELSEIF lovchklstID = 379 THEN
    UPDATE employeechecklist SET
    MarriageContract='1'
    WHERE RowID=empchklstID;
ELSEIF lovchklstID = 380 THEN
    UPDATE employeechecklist SET
    HouseSketch='1'
    WHERE RowID=empchklstID;
ELSEIF lovchklstID = 381 THEN
    UPDATE employeechecklist SET
    TrainingAgreement='1'
    WHERE RowID=empchklstID;
ELSEIF lovchklstID = 382 THEN
    UPDATE employeechecklist SET
    HealthPermit='1'
    WHERE RowID=empchklstID;
ELSEIF lovchklstID = 383 THEN
    UPDATE employeechecklist SET
    ValidID='1'
    WHERE RowID=empchklstID;
ELSEIF lovchklstID = 384 THEN
    UPDATE employeechecklist SET
    Resume='1'
    WHERE RowID=empchklstID;
ELSEIF lovchklstID = 385 THEN
    UPDATE employeechecklist SET
    NBIClearance='1'
    WHERE RowID=empchklstID;
END IF;

SELECT OrganizationID FROM user WHERE RowID=NEW.CreatedBy INTO OrganizID;

SELECT RowID FROM `view` WHERE ViewName='Employee Attachment' AND OrganizationID=OrganizID LIMIT 1 INTO viewID;

INSERT INTO audittrail (Created,CreatedBy,LastUpdBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed
) VALUES (CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,OrganizID,viewID,'EmployeeID',NEW.RowID,'',NEW.EmployeeID,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,OrganizID,viewID,'Type',NEW.RowID,'',NEW.Type,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,OrganizID,viewID,'FileName',NEW.RowID,'',NEW.FileName,'Insert')
,(CURRENT_TIMESTAMP(),NEW.CreatedBy,NEW.CreatedBy,OrganizID,viewID,'FileType',NEW.RowID,'',NEW.FileType,'Insert');





END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
