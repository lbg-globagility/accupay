-- Add ApproverEmail column to employeetimelogfiling (MySQL)
ALTER TABLE `employeetimelogfiling`
    ADD COLUMN `ApproverEmail` VARCHAR(255) NULL DEFAULT NULL;