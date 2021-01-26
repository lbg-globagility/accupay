/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `employeechecklist` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(10) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT current_timestamp(),
  `CreatedBy` int(10) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `LastUpdBy` int(10) DEFAULT NULL,
  `EmployeeID` int(10) DEFAULT NULL,
  `PerformanceAppraisal` varchar(1) DEFAULT NULL,
  `BIRTIN` varchar(1) DEFAULT NULL,
  `Diploma` varchar(1) DEFAULT NULL,
  `IDInfoSlip` varchar(1) DEFAULT NULL,
  `PhilhealthID` varchar(1) DEFAULT NULL,
  `HDMFID` varchar(1) DEFAULT NULL,
  `SSSNo` varchar(1) DEFAULT NULL,
  `TranscriptOfRecord` varchar(1) DEFAULT NULL,
  `BirthCertificate` varchar(1) DEFAULT NULL,
  `EmployeeContract` varchar(1) DEFAULT NULL,
  `MedicalExam` varchar(1) DEFAULT NULL,
  `NBIClearance` varchar(1) DEFAULT NULL,
  `COEEmployer` varchar(1) DEFAULT NULL,
  `MarriageContract` varchar(1) DEFAULT NULL,
  `HouseSketch` varchar(1) DEFAULT NULL,
  `TrainingAgreement` varchar(1) DEFAULT NULL,
  `HealthPermit` varchar(1) DEFAULT NULL,
  `ValidID` varchar(1) DEFAULT NULL,
  `Resume` varchar(1) DEFAULT NULL,
  `PAGIBIGLoan` varchar(1) DEFAULT NULL,
  `Clearance` varchar(1) DEFAULT '0',
  PRIMARY KEY (`RowID`),
  KEY `FK_BaseTables_organization` (`OrganizationID`),
  KEY `FK_BaseTables_user` (`CreatedBy`),
  KEY `FK_BaseTables_user_2` (`LastUpdBy`),
  KEY `FK_basetables_employee` (`EmployeeID`),
  CONSTRAINT `employeechecklist_ibfk_1` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`) ON DELETE CASCADE,
  CONSTRAINT `employeechecklist_ibfk_2` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`),
  CONSTRAINT `employeechecklist_ibfk_3` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `employeechecklist_ibfk_4` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
