/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `RESET_LEAVECREDIT`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `RESET_LEAVECREDIT`(
	`employeeRowId` INT,
	`orgId` INT,
	`userId` INT,
	`forYear` INT,
	`vacationLeaveAllowance` DECIMAL(10,4),
	`vacationLeaveBalance` DECIMAL(10,4),
	`sickLeaveAllowance` DECIMAL(10,4),
	`sickLeaveBalance` DECIMAL(10,4),
	`OLDvacationLeaveAllowance` DECIMAL(10,4),
	`OLDvacationLeaveBalance` DECIMAL(10,4),
	`OLDsickLeaveAllowance` DECIMAL(10,4),
	`OLDsickLeaveBalance` DECIMAL(10,4)






) RETURNS tinyint(1)
    COMMENT 'for Cinema 2000'
BEGIN

DECLARE itemName VARCHAR(50);

DECLARE leaveLedgerId
        ,leaveTransId
		  ,privilegeViewID INT(11);

SELECT v.RowID FROM `view` v WHERE v.OrganizationID=orgId AND v.ViewName='Employee Personal Profile' INTO privilegeViewID;

INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),userId,userId,orgId,privilegeViewID,'LeaveAllowance',employeeRowId,OLDvacationLeaveAllowance,vacationLeaveAllowance,'Update');

INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),userId,userId,orgId,privilegeViewID,'LeaveBalance',employeeRowId,OLDvacationLeaveBalance,vacationLeaveBalance,'Update');


INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),userId,userId,orgId,privilegeViewID,'SickLeaveAllowance',employeeRowId,OLDsickLeaveAllowance,sickLeaveAllowance,'Update');

INSERT INTO audittrail (LastUpd,LastUpdBy,CreatedBy,OrganizationID,ViewID,FieldChanged,ChangedRowID,OldValue,NewValue,ActionPerformed) VALUES (CURRENT_TIMESTAMP(),userId,userId,orgId,privilegeViewID,'SickLeaveBalance',employeeRowId,OLDsickLeaveBalance,sickLeaveBalance,'Update');


# VACATION LEAVE
SET itemName = 'Vacation leave';
SET leaveLedgerId = NULL;
INSERT INTO `leaveledger` (`OrganizationID`, `Created`, `CreatedBy`, `LastUpd`, `LastUpdBy`, `EmployeeID`, `ProductID`, `LastTransactionID`) SELECT p.OrganizationID, CURRENT_TIMESTAMP(), 0, CURRENT_TIMESTAMP(), NULL, employeeRowId, p.RowID, NULL FROM (SELECT RowID, OrganizationID, PartNo, `Category`, CategoryID FROM product WHERE OrganizationID=orgId AND PartNo=itemName) p INNER JOIN (SELECT RowID, OrganizationID, CategoryName FROM category WHERE OrganizationID=orgId AND CategoryName='Leave Type') c ON c.RowID=p.CategoryID AND c.OrganizationID=p.OrganizationID ON DUPLICATE KEY UPDATE LastUpd=CURRENT_TIMESTAMP();

SELECT ll.RowID FROM leaveledger ll INNER JOIN (SELECT RowID, OrganizationID, PartNo, `Category`, CategoryID FROM product) p ON p.OrganizationID=ll.OrganizationID AND p.PartNo=itemName INNER JOIN (SELECT RowID, CategoryName FROM `category`) c ON c.RowID=p.CategoryID AND c.CategoryName='Leave Type' WHERE ll.EmployeeID=employeeRowId AND ll.OrganizationID=orgId AND ll.ProductID=p.RowID INTO leaveLedgerId;

SET leaveTransId = NULL;
INSERT INTO `leavetransaction` (`OrganizationID`, `Created`, `CreatedBy`, `LastUpd`, `LastUpdBy`, `EmployeeID`, `ReferenceID`, `LeaveLedgerID`, `PayPeriodID`, `TransactionDate`, `Type`, `Balance`, `Amount`, `Comments`) SELECT pp.OrganizationID, CURRENT_TIMESTAMP(), 0, CURRENT_TIMESTAMP(), NULL, employeeRowId, NULL, leaveLedgerId, pp.RowID, pp.PayFromDate, 'Credit', vacationLeaveBalance, vacationLeaveBalance, '' FROM payperiod pp WHERE pp.OrganizationID=orgId AND pp.`Month`=12 AND pp.`Half`=0 AND pp.`Year`=2018; SELECT @@identity INTO leaveTransId;

UPDATE leaveledger SET LastTransactionID=leaveTransId, LastUpd=CURRENT_TIMESTAMP(), LastUpdBy=IFNULL(LastUpdBy, CreatedBy) WHERE RowID=leaveLedgerId;


# SICK LEAVE
SET itemName = 'Sick leave';
SET leaveLedgerId = NULL;
INSERT INTO `leaveledger` (`OrganizationID`, `Created`, `CreatedBy`, `LastUpd`, `LastUpdBy`, `EmployeeID`, `ProductID`, `LastTransactionID`) SELECT p.OrganizationID, CURRENT_TIMESTAMP(), 0, CURRENT_TIMESTAMP(), NULL, employeeRowId, p.RowID, NULL FROM (SELECT RowID, OrganizationID, PartNo, `Category`, CategoryID FROM product WHERE OrganizationID=orgId AND PartNo=itemName) p INNER JOIN (SELECT RowID, OrganizationID, CategoryName FROM category WHERE OrganizationID=orgId AND CategoryName='Leave Type') c ON c.RowID=p.CategoryID AND c.OrganizationID=p.OrganizationID ON DUPLICATE KEY UPDATE LastUpd=CURRENT_TIMESTAMP();

SELECT ll.RowID FROM leaveledger ll INNER JOIN (SELECT RowID, OrganizationID, PartNo, `Category`, CategoryID FROM product) p ON p.OrganizationID=ll.OrganizationID AND p.PartNo=itemName INNER JOIN (SELECT RowID, CategoryName FROM `category`) c ON c.RowID=p.CategoryID AND c.CategoryName='Leave Type' WHERE ll.EmployeeID=employeeRowId AND ll.OrganizationID=orgId AND ll.ProductID=p.RowID INTO leaveLedgerId;

SET leaveTransId = NULL;
INSERT INTO `leavetransaction` (`OrganizationID`, `Created`, `CreatedBy`, `LastUpd`, `LastUpdBy`, `EmployeeID`, `ReferenceID`, `LeaveLedgerID`, `PayPeriodID`, `TransactionDate`, `Type`, `Balance`, `Amount`, `Comments`) SELECT pp.OrganizationID, CURRENT_TIMESTAMP(), 0, CURRENT_TIMESTAMP(), NULL, employeeRowId, NULL, leaveLedgerId, pp.RowID, pp.PayFromDate, 'Credit', sickLeaveBalance, sickLeaveBalance, '' FROM payperiod pp WHERE pp.OrganizationID=orgId AND pp.`Month`=12 AND pp.`Half`=0 AND pp.`Year`=2018; SELECT @@identity INTO leaveTransId;

UPDATE leaveledger SET LastTransactionID=leaveTransId, LastUpd=CURRENT_TIMESTAMP(), LastUpdBy=IFNULL(LastUpdBy, CreatedBy) WHERE RowID=leaveLedgerId;


RETURN TRUE;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
