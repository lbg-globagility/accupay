CREATE TABLE `emailtemplate` (
  `RowID` INT NOT NULL AUTO_INCREMENT,
  `OrganizationID` INT NULL,
  `Code` VARCHAR(100) NOT NULL,
  `Subject` VARCHAR(255) NOT NULL,
  `HtmlBody` TEXT NOT NULL,
  `TextBody` TEXT NULL,
  `IsActive` TINYINT(1) NOT NULL DEFAULT 1,
  `Created` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` INT NULL,
  `LastUpd` DATETIME NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `LastUpdBy` INT NULL,
  PRIMARY KEY (`RowID`),
  INDEX `IX_EmailTemplate_Code` (`Code`),
  INDEX `IX_EmailTemplate_OrganizationID` (`OrganizationID`),
  CONSTRAINT `FK_EmailTemplate_Organization` FOREIGN KEY (`OrganizationID`) REFERENCES `organization`(`RowID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Global default template (OrganizationID = NULL) for the timelog filing approval email.
-- Organizations can override it by inserting a row with their own OrganizationID and the same Code.
INSERT INTO `emailtemplate` (`OrganizationID`, `Code`, `Subject`, `HtmlBody`, `TextBody`, `IsActive`)
VALUES (
  NULL,
  'TimeLogFilingApproval',
  '[AccuPay] Timelog filing correction request',
  '<div style="font-family:Segoe UI, Arial, sans-serif;"><p>Hi {approver},</p><p>{employee} filed a time log correction for {date} ({time}).</p><p>Reason: {reason}</p><p>{approveButton} {rejectButton}</p></div>',
  'Hi {approver},\n\n{employee} filed a time log correction for {date} ({time}).\nReason: {reason}\n\nApprove: {approveButton}\nReject: {rejectButton}',
  1
);

-- Global default template (OrganizationID = NULL) for the leave filing approval email.
INSERT INTO `emailtemplate` (`OrganizationID`, `Code`, `Subject`, `HtmlBody`, `TextBody`, `IsActive`)
VALUES (
  NULL,
  'LeaveFilingApproval',
  '[AccuPay] Leave filing approval request',
  '<div style="font-family:Segoe UI, Arial, sans-serif;"><p>Hi {approver},</p><p>{employee} requested {leavetype} leave ({date} {time}).</p><p>Reason: {reason}</p><p>{approveButton} {rejectButton}</p></div>',
  'Hi {approver},\n\n{employee} requested {leavetype} leave ({date} {time}).\nReason: {reason}\n\nApprove: {approveButton}\nReject: {rejectButton}',
  1
);

-- Global default template (OrganizationID = NULL) for the overtime filing approval email.
INSERT INTO `emailtemplate` (`OrganizationID`, `Code`, `Subject`, `HtmlBody`, `TextBody`, `IsActive`)
VALUES (
  NULL,
  'OvertimeFilingApproval',
  '[AccuPay] Overtime filing approval request',
  '<div style="font-family:Segoe UI, Arial, sans-serif;"><p>Hi {approver},</p><p>{employee} filed {hours} h of overtime on {date}.</p><p>Reason: {reason}</p><p>{approveButton} {rejectButton}</p></div>',
  'Hi {approver},\n\n{employee} filed {hours} h of overtime on {date}.\nReason: {reason}\n\nApprove: {approveButton}\nReject: {rejectButton}',
  1
);
