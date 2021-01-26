/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `aspnetuserroles` (
  `UserId` binary(16) NOT NULL,
  `RoleId` binary(16) NOT NULL,
  `OrganizationId` int(11) NOT NULL,
  `CreatedAt` timestamp NOT NULL DEFAULT current_timestamp(),
  `CreatedById` binary(16) DEFAULT NULL,
  `UpdatedAt` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `UpdatedById` binary(16) DEFAULT NULL,
  PRIMARY KEY (`UserId`,`RoleId`,`OrganizationId`) USING BTREE,
  UNIQUE KEY `UserId_OrganizationId` (`UserId`,`OrganizationId`),
  KEY `FK_AspNetUserRoles_AspNetRoles` (`RoleId`),
  KEY `FK_AspNetUserRoles_organization` (`OrganizationId`) USING BTREE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_organization` FOREIGN KEY (`OrganizationId`) REFERENCES `organization` (`RowID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
