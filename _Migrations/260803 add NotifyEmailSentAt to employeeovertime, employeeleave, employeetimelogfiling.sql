ALTER TABLE `employeeovertime`
	ADD COLUMN `NotifyEmailSentAt` DATETIME NULL DEFAULT NULL AFTER `IsNotifyEmail`;

ALTER TABLE `employeeleave`
	ADD COLUMN `NotifyEmailSentAt` DATETIME NULL DEFAULT NULL AFTER `IsNotifyEmail`;

ALTER TABLE `employeetimelogfiling`
	ADD COLUMN `NotifyEmailSentAt` DATETIME NULL DEFAULT NULL AFTER `IsNotifyEmail`;
