-- Rows created from the same self-service date-range leave filing share the same
-- FilingGroupDate value, so they can be grouped back together as one request.
ALTER TABLE `employeeleave`
	ADD COLUMN `FilingGroupDate` DATETIME NULL DEFAULT NULL AFTER `ApproverEmail`;
