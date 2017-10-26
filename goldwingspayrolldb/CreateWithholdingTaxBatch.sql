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

-- Dumping structure for procedure CreateWithholdingTaxBatch
DROP PROCEDURE IF EXISTS `CreateWithholdingTaxBatch`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `CreateWithholdingTaxBatch`(IN `I_OrganizationID` INT, IN `I_Year` INT)
    DETERMINISTIC
BEGIN

    INSERT INTO withholdingtaxbatch
    (
        OrganizationID,
        Year
    )
    VALUES
    (
        I_OrganizationID,
        I_Year
    );

    SELECT LAST_INSERT_ID();

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
