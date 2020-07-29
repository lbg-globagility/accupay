/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `aspnetusers` (
  `Id` binary(16) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  `ConcurrencyStamp` mediumtext DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `EmailConfirmed` bit(1) NOT NULL,
  `LockoutEnabled` bit(1) NOT NULL,
  `LockoutEnd` timestamp NULL DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `PasswordHash` mediumtext DEFAULT NULL,
  `PhoneNumber` mediumtext DEFAULT NULL,
  `PhoneNumberConfirmed` bit(1) NOT NULL,
  `SecurityStamp` mediumtext DEFAULT NULL,
  `TwoFactorEnabled` bit(1) NOT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `FirstName` varchar(45) DEFAULT NULL,
  `LastName` varchar(45) DEFAULT NULL,
  `Status` varchar(255) NOT NULL DEFAULT 'Pending',
  `CreatedAt` timestamp NOT NULL DEFAULT current_timestamp(),
  `CreatedById` binary(16) DEFAULT NULL,
  `UpdatedAt` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `UpdatedById` binary(16) DEFAULT NULL,
  `ClientId` int(11) NOT NULL,
  `OriginalImageId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`),
  KEY `FK_aspnetusers_client_ClientId` (`ClientId`),
  CONSTRAINT `FK_aspnetusers_client_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `client` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
