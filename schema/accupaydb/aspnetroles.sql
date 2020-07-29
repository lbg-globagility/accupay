/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `aspnetroles` (
  `Id` binary(16) NOT NULL,
  `ConcurrencyStamp` mediumtext DEFAULT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(256) DEFAULT NULL,
  `IsAdmin` tinyint(4) NOT NULL DEFAULT 0,
  `CreatedAt` timestamp NOT NULL DEFAULT current_timestamp(),
  `CreatedById` binary(16) DEFAULT NULL,
  `UpdatedAt` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `UpdatedById` binary(16) DEFAULT NULL,
  `ClientId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_aspnetroles_client` (`ClientId`),
  CONSTRAINT `FK_aspnetroles_client` FOREIGN KEY (`ClientId`) REFERENCES `client` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
