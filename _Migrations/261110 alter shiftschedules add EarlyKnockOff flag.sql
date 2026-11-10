ALTER TABLE `shiftschedules`
	ADD COLUMN `EarlyKnockOff` TINYINT(1) NOT NULL DEFAULT 0 AFTER `RequiresLunchInOut`;
