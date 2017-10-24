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

-- Dumping structure for trigger BEFINS_employeeloanschedule
DROP TRIGGER IF EXISTS `BEFINS_employeeloanschedule`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFINS_employeeloanschedule` BEFORE INSERT ON `employeeloanschedule` FOR EACH ROW BEGIN

DECLARE loancategName VARCHAR(50) DEFAULT 'Loan Type';

DECLARE categID INT(11);

DECLARE prod_RowID INT(11);

SELECT RowID FROM category WHERE CategoryName=loancategName AND OrganizationID=NEW.OrganizationID INTO categID;

IF NEW.LoanTypeID IS NULL THEN

    SET NEW.LoanTypeID = INSUPD_product(NULL,NEW.LoanName,NEW.OrganizationID,NEW.LoanName,NEW.CreatedBy,NEW.LastUpdBy,loancategName,categID,'Active',0,0,0,0);

END IF;

SET NEW.LoanPayPeriodLeft = ROUND(NEW.LoanPayPeriodLeft,0);

IF NEW.LoanPayPeriodLeft < 1 THEN
    SET NEW.`Status` = 'Complete';
END IF;

IF NEW.LoanNumber IS NULL THEN
    SET NEW.LoanNumber = '';
END IF;

IF LENGTH(TRIM(NEW.LoanName)) = 0 THEN
    SET NEW.LoanName = IFNULL((SELECT PartNo FROM product WHERE RowID=NEW.LoanTypeID),'');
END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
