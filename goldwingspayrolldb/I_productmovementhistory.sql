/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `I_productmovementhistory`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `I_productmovementhistory`(IN `I_OrganizationID` INT(10), IN `I_Created` DATETIME, IN `I_CreatedBy` INT(11), IN `I_LastUpd` DATETIME, IN `I_LastUpdBy` INT(11), IN `I_ProductID` INT(10), IN `I_Comments` VARCHAR(2000), IN `I_TransactionType` VARCHAR(50), IN `I_OriginatingInvLocationID` INT(11), IN `I_DestinationInvLocationID` INT(11), IN `I_CurrentQty` INT(11), IN `I_NewQty` INT(11), IN `I_OrigRackNo` INT(11), IN `I_OrigShelfNo` INT(11), IN `I_OrigColumnNo` INT(11), IN `I_DestinationRackNo` INT(11), IN `I_DestinationhelfNo` INT(11), IN `I_DestinationColumnNo` INT(11)
)
BEGIN
INSERT INTO productmovementhistory
(
    OrganizationID,
    Created,
    CreatedBy,
    LastUpd,
    LastUpdBy,
    ProductID,
    Comments,
    TransactionType,
    OriginatingInvLocationID,
    DestinationInvLocationID,
    CurrentQty,
    NewQty,
    OrigRackNo,
    OrigShelfNo,
    OrigColumnNo,
    DestinationRackNo,
    DestinationhelfNo,
    DestinationColumnNo
)
VALUES
(
    I_OrganizationID,
    I_Created,
    I_CreatedBy,
    I_LastUpd,
    I_LastUpdBy,
    I_ProductID,
    I_Comments,
    I_TransactionType,
    I_OriginatingInvLocationID,
    I_DestinationInvLocationID,
    I_CurrentQty,
    I_NewQty,
    I_OrigRackNo,
    I_OrigShelfNo,
    I_OrigColumnNo,
    I_DestinationRackNo,
    I_DestinationhelfNo,
    I_DestinationColumnNo
);END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
