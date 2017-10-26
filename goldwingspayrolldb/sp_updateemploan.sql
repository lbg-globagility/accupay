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

-- Dumping structure for procedure sp_updateemploan
DROP PROCEDURE IF EXISTS `sp_updateemploan`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_updateemploan`(IN `I_Lastupd` DATETIME, IN `I_Lastupdby` INT(10), IN `I_LoanNumber` INT(10), IN `I_DedEffectiveDateFrom` DATE, IN `I_DedEffectiveDateTo` DATE, IN `I_TotalLoanAmount` DECIMAL(10,2), IN `I_DeductionSchedule` VARCHAR(50), IN `I_DeductionAmount` DECIMAL(10,2), IN `I_Status` VARCHAR(50), IN `I_DeductionPercentage` DECIMAL(10,2), IN `I_NoOfPayPeriod` DECIMAL(10,2), IN `I_Comments` VARCHAR(2000), IN `I_RowID` INT(10), IN `I_LoanTypeID` INT, IN `I_BonusID` INT)
    DETERMINISTIC
BEGIN
UPDATE employeeloanschedule
SET
    LastUpd = CURRENT_TIMESTAMP(),
    LastUpdBy = I_LastUpdBy,
    LoanNumber = I_LoanNumber,
    DedEffectiveDateFrom = I_DedEffectiveDateFrom,
    DedEffectiveDateTo = I_DedEffectiveDateTo,
    TotalLoanAmount = I_TotalLoanAmount,
    DeductionSchedule = I_DeductionSchedule,
    DeductionAmount = I_DeductionAmount,
    `Status` = I_Status,
    DeductionPercentage = I_DeductionPercentage,
    NoOfPayPeriod = I_NoOfPayPeriod,
    Comments = I_Comments,
    LoanTypeID = I_LoanTypeID,
    BonusID = I_BonusID
WHERE RowID = I_RowID;
END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
