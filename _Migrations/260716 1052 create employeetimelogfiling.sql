CREATE TABLE `employeetimelogfiling` (
  `RowID` INT NOT NULL AUTO_INCREMENT,
  `OrganizationID` INT NULL,
  `EmployeeID` INT NULL,
  `EntryType` VARCHAR(100) NOT NULL,
  `LogDate` DATETIME NOT NULL,
  `Time` TIME NOT NULL,
  `Reason` VARCHAR(1000) NULL,
  `Status` VARCHAR(50) NOT NULL DEFAULT 'Pending',
  `Created` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` INT NULL,
  `LastUpd` DATETIME NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastUpdBy` INT NULL,
  PRIMARY KEY (`RowID`),
  INDEX `IX_EmployeeTimelogFiling_EmployeeID` (`EmployeeID`),
  INDEX `IX_EmployeeTimelogFiling_OrganizationID` (`OrganizationID`),
  CONSTRAINT `FK_EmployeeTimelogFiling_Employee` FOREIGN KEY (`EmployeeID`) REFERENCES `employee`(`RowID`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;