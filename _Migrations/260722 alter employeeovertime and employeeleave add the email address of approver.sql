-- Add ApproverEmail column to employeeovertime and employeeleave (MySQL)
ALTER TABLE `employeeovertime`
    ADD COLUMN `ApproverEmail` VARCHAR(255) NULL DEFAULT NULL;

ALTER TABLE `employeeleave`
    ADD COLUMN `ApproverEmail` VARCHAR(255) NULL DEFAULT NULL;
