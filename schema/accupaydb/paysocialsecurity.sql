/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `paysocialsecurity` (
  `RowID` int(11) NOT NULL AUTO_INCREMENT,
  `Created` timestamp NULL DEFAULT current_timestamp(),
  `CreatedBy` int(10) NOT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `LastUpdBy` int(10) DEFAULT NULL,
  `RangeFromAmount` decimal(10,2) NOT NULL,
  `RangeToAmount` decimal(10,2) NOT NULL,
  `MonthlySalaryCredit` decimal(10,2) NOT NULL,
  `EmployeeContributionAmount` decimal(10,2) NOT NULL,
  `EmployerContributionAmount` decimal(10,2) NOT NULL,
  `EmployeeECAmount` decimal(10,2) NOT NULL,
  `HiddenData` tinyint(1) NOT NULL DEFAULT 0,
  `EffectiveDateFrom` date NOT NULL,
  `EffectiveDateTo` date NOT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `AK_paysocialsecurity_MonthlySalaryCredit_EffctvDtFrm_EffctvDtT` (`MonthlySalaryCredit`,`EffectiveDateFrom`,`EffectiveDateTo`),
  KEY `FK_paypocialsecurity_user_CreatedBy` (`CreatedBy`),
  KEY `FK_paysocialsecurity_user_LastUpdBy` (`LastUpdBy`),
  CONSTRAINT `FK_paypocialsecurity_user_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_paysocialsecurity_user_LastUpdBy` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Social Security Table';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
