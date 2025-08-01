/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE TABLE IF NOT EXISTS `organization` (
  `RowID` int(10) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) DEFAULT NULL,
  `TradeName` varchar(100) DEFAULT NULL,
  `PrimaryAddressID` int(10) DEFAULT NULL,
  `PremiseAddressID` int(10) DEFAULT NULL,
  `MainPhone` varchar(50) DEFAULT NULL,
  `FaxNumber` varchar(50) DEFAULT NULL,
  `EmailAddress` varchar(50) DEFAULT NULL,
  `AltEmailAddress` varchar(50) DEFAULT NULL,
  `AltPhone` varchar(50) DEFAULT NULL,
  `URL` varchar(50) DEFAULT NULL,
  `TINNo` varchar(50) DEFAULT NULL,
  `BankAccountNo` varchar(50) DEFAULT NULL COMMENT 'where direct deposit would go',
  `BankName` varchar(100) DEFAULT NULL COMMENT 'where direct deposit payments would go',
  `Created` datetime DEFAULT current_timestamp(),
  `CreatedBy` int(11) DEFAULT NULL,
  `LastUpd` datetime DEFAULT NULL ON UPDATE current_timestamp(),
  `LastUpdBy` int(11) DEFAULT NULL,
  `Image` mediumblob DEFAULT NULL,
  `OrganizationType` varchar(50) DEFAULT NULL COMMENT 'Commercial Center, Office Building',
  `TotalFloorArea` decimal(10,2) DEFAULT NULL,
  `MinimumWater` decimal(10,2) DEFAULT NULL,
  `MinimumElectricity` decimal(10,2) DEFAULT NULL,
  `VacationLeaveDays` decimal(10,2) DEFAULT NULL,
  `SickLeaveDays` decimal(10,2) DEFAULT NULL,
  `MaternityLeaveDays` decimal(10,2) DEFAULT NULL,
  `OthersLeaveDays` decimal(10,2) DEFAULT NULL,
  `STPFlag` varchar(50) DEFAULT NULL COMMENT 'Sewage Treatment Plan. if this flag is checked, that means there is a 50% STP surcharge on water bill per month added on invoice',
  `PayFrequencyID` int(11) DEFAULT NULL,
  `PhilhealthDeductionSchedule` varchar(50) DEFAULT NULL COMMENT 'per Pay Frequency, or per End of Month',
  `SSSDeductionSchedule` varchar(50) DEFAULT NULL COMMENT 'per Pay Frequency, or per End of Month',
  `PagIbigDeductionSchedule` varchar(50) DEFAULT NULL COMMENT 'per Pay Frequency, or per End of Month',
  `WithholdingDeductionSchedule` varchar(50) DEFAULT NULL,
  `LoanDeductionSchedule` varchar(50) DEFAULT NULL,
  `ReportText` varchar(1000) DEFAULT NULL,
  `NightDifferentialTimeFrom` time DEFAULT NULL,
  `NightDifferentialTimeTo` time DEFAULT NULL,
  `NightShiftTimeFrom` time DEFAULT NULL,
  `NightShiftTimeTo` time DEFAULT NULL,
  `AllowNegativeLeaves` char(1) DEFAULT NULL,
  `LimitedAccess` char(1) DEFAULT 'N',
  `WorkDaysPerYear` decimal(11,0) DEFAULT 260,
  `GracePeriod` decimal(11,0) DEFAULT 0,
  `RDOCode` char(8) DEFAULT '',
  `ZIPCode` char(8) DEFAULT '',
  `MinWageEmpSSSContrib` decimal(11,2) DEFAULT 454.20,
  `MinWageEmpPhHContrib` decimal(11,2) DEFAULT 150.00,
  `MinWageEmpHDMFContrib` decimal(11,2) DEFAULT 100.00,
  `NoPurpose` char(1) DEFAULT '0' COMMENT 'This serves as a flag as dummy data, for developers purpose only',
  `LeaveBalanceAcquisition` char(1) DEFAULT '0' COMMENT 'Resets Annually,Accrual',
  `IsAgency` tinyint(4) NOT NULL DEFAULT 0,
  `ClientId` int(11) NOT NULL,
  PRIMARY KEY (`RowID`),
  UNIQUE KEY `Index 4` (`Name`),
  KEY `FK_organization_user` (`CreatedBy`),
  KEY `FK_organization_user_2` (`LastUpdBy`),
  KEY `FK_organization_address` (`PrimaryAddressID`),
  KEY `FK_organization_address_2` (`PremiseAddressID`),
  KEY `FK_organization_client_ClientId` (`ClientId`),
  CONSTRAINT `FK_organization_address` FOREIGN KEY (`PrimaryAddressID`) REFERENCES `address` (`RowID`),
  CONSTRAINT `FK_organization_address_2` FOREIGN KEY (`PremiseAddressID`) REFERENCES `address` (`RowID`),
  CONSTRAINT `FK_organization_client_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `client` (`Id`),
  CONSTRAINT `FK_organization_user` FOREIGN KEY (`CreatedBy`) REFERENCES `user` (`RowID`),
  CONSTRAINT `FK_organization_user_2` FOREIGN KEY (`LastUpdBy`) REFERENCES `user` (`RowID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='This is the internal Company';

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
