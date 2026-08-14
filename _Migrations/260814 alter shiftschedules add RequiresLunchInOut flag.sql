ALTER TABLE `shiftschedules`
	ADD COLUMN `RequiresLunchInOut` TINYINT(1) NOT NULL DEFAULT 0 AFTER `MarkedAsWholeDay`;
