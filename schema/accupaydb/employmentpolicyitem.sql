/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `employmentpolicyitem` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `EmploymentPolicyId` int(11) NOT NULL,
  `EmploymentPolicyTypeId` int(11) NOT NULL,
  `Value` varchar(255) DEFAULT NULL,
  `CreatedAt` timestamp NOT NULL DEFAULT current_timestamp(),
  `CreatedById` int(11) DEFAULT NULL,
  `UpdatedAt` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `UpdatedById` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_employmentpolicyitem_employmentpolicytype` (`EmploymentPolicyTypeId`),
  KEY `FK_employmentpolicyitem_employmentpolicy` (`EmploymentPolicyId`),
  CONSTRAINT `FK_employmentpolicyitem_employmentpolicy` FOREIGN KEY (`EmploymentPolicyId`) REFERENCES `employmentpolicy` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_employmentpolicyitem_employmentpolicytype` FOREIGN KEY (`EmploymentPolicyTypeId`) REFERENCES `employmentpolicytype` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
