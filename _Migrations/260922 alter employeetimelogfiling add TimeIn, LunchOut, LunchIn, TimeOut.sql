ALTER TABLE `employeetimelogfiling`
	MODIFY COLUMN `EntryType` VARCHAR(100) NULL DEFAULT NULL,
	MODIFY COLUMN `Time` TIME NULL DEFAULT NULL,
	ADD COLUMN `TimeIn` TIME NULL DEFAULT NULL AFTER `Time`,
	ADD COLUMN `LunchOut` TIME NULL DEFAULT NULL AFTER `TimeIn`,
	ADD COLUMN `LunchIn` TIME NULL DEFAULT NULL AFTER `LunchOut`,
	ADD COLUMN `TimeOut` TIME NULL DEFAULT NULL AFTER `LunchIn`;

UPDATE `employeetimelogfiling` SET `TimeIn` = `Time` WHERE `EntryType` = 'CheckIn';
UPDATE `employeetimelogfiling` SET `LunchOut` = `Time` WHERE `EntryType` = 'LunchOut';
UPDATE `employeetimelogfiling` SET `LunchIn` = `Time` WHERE `EntryType` = 'LunchIn';
UPDATE `employeetimelogfiling` SET `TimeOut` = `Time` WHERE `EntryType` = 'CheckOut';
