/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP TABLE IF EXISTS `resetleavecredititem`;
CREATE TABLE IF NOT EXISTS `resetleavecredititem` (
  `RowID` int(11) NOT NULL AUTO_INCREMENT,
  `OrganizationID` int(11) NOT NULL,
  `Created` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `CreatedBy` int(11) NOT NULL,
  `LastUpd` timestamp NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `LastUpdBy` int(11) DEFAULT NULL,
  `EmployeeID` int(11) NOT NULL,
  `ResetLeaveCreditId` int(11) NOT NULL,
  `VacationLeaveCredit` decimal(20,6) NOT NULL DEFAULT 0.000000,
  `SickLeaveCredit` decimal(20,6) NOT NULL DEFAULT 0.000000,
  `IsSelected` tinyint(4) NOT NULL DEFAULT 0,
  `IsApplied` tinyint(4) NOT NULL DEFAULT 0,
  PRIMARY KEY (`RowID`) USING BTREE,
  KEY `FK_resetleavecredititem_organization` (`OrganizationID`),
  KEY `FK_resetleavecredititem_aspnetusers` (`CreatedBy`),
  KEY `FK_resetleavecredititem_aspnetusers_2` (`LastUpdBy`),
  KEY `FK_resetleavecredititem_employee` (`EmployeeID`),
  KEY `FK_resetleavecredititem_resetleavecredit` (`ResetLeaveCreditId`),
  CONSTRAINT `FK_resetleavecredititem_aspnetusers` FOREIGN KEY (`CreatedBy`) REFERENCES `aspnetusers` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_resetleavecredititem_aspnetusers_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `aspnetusers` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_resetleavecredititem_employee` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_resetleavecredititem_organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_resetleavecredititem_resetleavecredit` FOREIGN KEY (`ResetLeaveCreditId`) REFERENCES `resetleavecredit` (`RowID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
