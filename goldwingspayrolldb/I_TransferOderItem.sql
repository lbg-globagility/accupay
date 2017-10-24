-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.12-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for procedure I_TransferOderItem
DROP PROCEDURE IF EXISTS `I_TransferOderItem`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_TransferOderItem`(IN `I_ProductID` INT(10), IN `I_ParentOrderID` INT(10), IN `I_ProductName` VARCHAR(50), IN `I_PartNo` VARCHAR(200), IN `I_OrganizationID` INT(11), IN `I_Created` DATETIME, IN `I_LineNum` INT(11), IN `I_QtyRequested` INT(11), IN `I_QtyApproved` INT(11), IN `I_Status` VARCHAR(10), IN `I_MainOfficeEndingInventory` INT(11), IN `I_SRP` INT(11), IN `I_TotalOverallEndingInventory` INT(11), IN `I_CreatedBy` INT(11), IN `I_LastUpd` DATETIME, IN `I_LastUpdBy` INT(11)

)
    DETERMINISTIC
BEGIN
INSERT INTO orderitem
(
    ProductID,
    ParentOrderID,
    ProductName,
    PartNo,
    OrganizationID,
    Created,
    LineNum,
    QtyRequested,
    QtyApproved,
    Status,
    MainOfficeEndingInventory,
    SRP,
    TotalOverallEndingInventory,
    CreatedBy,
    LastUpd,
    LastUpdBy
)
VALUES
(
    I_ProductID,
    I_ParentOrderID,
    I_ProductName,
    I_PartNo,
    I_OrganizationID,
    I_Created,
    I_LineNum,
    I_QtyRequested,
    I_QtyApproved,
    I_Status,
    I_MainOfficeEndingInventory,
    I_SRP,
    I_TotalOverallEndingInventory,
    I_CreatedBy,
    I_LastUpd,
    I_LastUpdBy
);END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
