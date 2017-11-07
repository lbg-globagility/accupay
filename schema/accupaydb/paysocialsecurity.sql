/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `paysocialsecurity` (
  `RowID` int(11) NOT NULL AUTO_INCREMENT,
  `Created` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` int(10) NOT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastUpdBy` int(10) DEFAULT NULL,
  `RangeFromAmount` decimal(10,2) NOT NULL,
  `RangeToAmount` decimal(10,2) NOT NULL,
  `MonthlySalaryCredit` decimal(10,2) NOT NULL,
  `EmployeeContributionAmount` decimal(10,2) NOT NULL,
  `EmployerContributionAmount` decimal(10,2) NOT NULL,
  `EmployeeECAmount` decimal(10,2) NOT NULL,
  `HiddenData` tinyint(1) NOT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `MonthlySalaryCredit` (`MonthlySalaryCredit`),
  KEY `FK_PaySocialSecurity_user` (`CreatedBy`),
  KEY `FK_PaySocialSecurity_user_2` (`LastUpdBy`),
  CONSTRAINT `FK_PaySocialSecurity_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_PaySocialSecurity_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=latin1 COMMENT='Social Security Table';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
