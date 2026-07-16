-- Create Approver and EmployeeApprover tables for MySQL (consistent with existing migrations)
-- Drop if tables already exist (safe for idempotent execution in dev environments)
DROP TABLE IF EXISTS `employeeapprover`;
DROP TABLE IF EXISTS `approver`;

CREATE TABLE `approver` (
    `RowID` INT(10) NOT NULL AUTO_INCREMENT,
    `OrganizationID` INT(10) NULL DEFAULT NULL,
    `Created` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `CreatedBy` INT(10) NULL DEFAULT NULL,
    `LastUpd` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    `LastUpdBy` INT(10) NULL DEFAULT NULL,
    `FirstName` VARCHAR(200) NULL DEFAULT NULL,
    `LastName` VARCHAR(200) NULL DEFAULT NULL,
    `EmailAddress` VARCHAR(255) NULL DEFAULT NULL,
    `CompanyName` VARCHAR(255) NULL DEFAULT NULL,
    PRIMARY KEY (`RowID`)
)
COLLATE='latin1_swedish_ci'
ENGINE=InnoDB;

-- FK to organization (optional; follows pattern in other migrations)
ALTER TABLE `approver`
    ADD CONSTRAINT `FK_approver_organization_OrganizationID`
    FOREIGN KEY (`OrganizationID`) REFERENCES `organization` (`RowID`) ON DELETE RESTRICT;

CREATE TABLE `employeeapprover` (
    `RowID` INT(10) NOT NULL AUTO_INCREMENT,
    `Created` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `CreatedBy` INT(10) NULL DEFAULT NULL,
    `LastUpd` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    `LastUpdBy` INT(10) NULL DEFAULT NULL,
    `ApproverID` INT(10) NOT NULL,
    `EmployeeID` INT(10) NOT NULL,
    PRIMARY KEY (`RowID`),
    INDEX `IX_employeeapprover_ApproverID` (`ApproverID`),
    INDEX `IX_employeeapprover_EmployeeID` (`EmployeeID`)
)
COLLATE='latin1_swedish_ci'
ENGINE=InnoDB;

ALTER TABLE `employeeapprover`
    ADD CONSTRAINT `FK_employeeapprover_approver_ApproverID`
    FOREIGN KEY (`ApproverID`) REFERENCES `approver` (`RowID`) ON DELETE CASCADE,
    ADD CONSTRAINT `FK_employeeapprover_employee_EmployeeID`
    FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`RowID`) ON DELETE CASCADE;