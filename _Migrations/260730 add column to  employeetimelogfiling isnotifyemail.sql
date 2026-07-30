ALTER TABLE `employeetimelogfiling`
	ADD COLUMN `IsNotifyEmail` TINYINT(1) NULL DEFAULT '0' AFTER `ApproverEmail`;
