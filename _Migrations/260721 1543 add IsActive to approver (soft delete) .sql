ALTER TABLE `approver`
	ADD COLUMN `IsActive` INT(1) NULL DEFAULT '1' AFTER `CompanyName`;