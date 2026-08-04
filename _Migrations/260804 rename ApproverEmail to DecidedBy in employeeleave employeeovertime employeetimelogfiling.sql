-- Rename ApproverEmail to DecidedBy (approve and reject both stamp it now, not just approve) (MySQL)
ALTER TABLE `employeeleave`
    CHANGE COLUMN `ApproverEmail` `DecidedBy` VARCHAR(255) NULL DEFAULT NULL;

ALTER TABLE `employeeovertime`
    CHANGE COLUMN `ApproverEmail` `DecidedBy` VARCHAR(255) NULL DEFAULT NULL;

ALTER TABLE `employeetimelogfiling`
    CHANGE COLUMN `ApproverEmail` `DecidedBy` VARCHAR(255) NULL DEFAULT NULL;
