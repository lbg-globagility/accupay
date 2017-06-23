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

-- Dumping structure for procedure goldwingspayrolldb.VIEW_employeechecklist
DROP PROCEDURE IF EXISTS `VIEW_employeechecklist`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeechecklist`(IN `echk_EmployeeID` INT, IN `echk_OrganizationID` INT)
    DETERMINISTIC
BEGIN

SELECT 
RowID
,EmployeeID
,'      Performance appraisal'
,COALESCE(PerformanceAppraisal,0) 'val_PerformanceAppraisal'
,'      BIR TIN'
,COALESCE(BIRTIN,0) 'val_BIRTIN'
,'      Diploma'
,COALESCE(Diploma,0) 'val_Diploma'
,'      ID Info slip'
,COALESCE(IDInfoSlip,0) 'val_IDInfoSlip'
,'      Philhealth ID'
,COALESCE(PhilhealthID,0) 'val_PhilhealthID'
,'      HDMF ID'
,COALESCE(HDMFID,0) 'val_HDMFID'
,'      SSS No'
,COALESCE(SSSNo,0) 'val_SSSNo'
,'      Transcript of record'
,COALESCE(TranscriptOfRecord,0) 'val_TranscriptOfRecord'
,'      Birth certificate'
,COALESCE(BirthCertificate,0) 'val_BirthCertificate'
,'      Employee contract'
,COALESCE(EmployeeContract,0) 'val_EmployeeContract'
,'      Medical exam'
,COALESCE(MedicalExam,0) 'val_MedicalExam'
,'      NBI clearance'
,COALESCE(NBIClearance,0) 'val_NBIClearance'
,'      COE employer'
,COALESCE(COEEmployer,0) 'val_COEEmployer'
,'      Marriage contract'
,COALESCE(MarriageContract,0) 'val_MarriageContract'
,'      House sketch'
,COALESCE(HouseSketch,0) 'val_HouseSketch'
,'      2305'
,COALESCE(TrainingAgreement,0) 'val_TrainingAgreement'
,'      Health permit'
,COALESCE(HealthPermit,0) 'val_HealthPermit'
,'      SSS loan certificate'
,COALESCE(ValidID,0) 'val_ValidID'
,'      Resume'
,COALESCE(Resume,0) 'val_Resume'
,'      HDMF Loan'
,COALESCE(PAGIBIGLoan,0) 'val_PAGIBIGLoan'
,'      Clearance'
,COALESCE(Clearance,0) 'val_Clearance'
FROM employeechecklist
WHERE EmployeeID=echk_EmployeeID
AND OrganizationID=echk_OrganizationID
ORDER BY RowID DESC
LIMIT 1;



END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
