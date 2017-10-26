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

-- Dumping structure for trigger BEFUPD_paystubadjustment
DROP TRIGGER IF EXISTS `BEFUPD_paystubadjustment`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFUPD_paystubadjustment` BEFORE UPDATE ON `paystubadjustment` FOR EACH ROW BEGIN

DECLARE prod_rowID INT(11);

DECLARE isadjamounttaxab TEXT;

DECLARE thevalue DECIMAL(11,6);

SELECT `Status` FROM product WHERE RowID=NEW.ProductID INTO isadjamounttaxab;

SET thevalue = OLD.PayAmount - NEW.PayAmount;


IF isadjamounttaxab = '2' THEN

    SELECT RowID FROM product p WHERE p.PartNo='Taxable Income' AND p.OrganizationID=NEW.OrganizationID INTO prod_rowID;

    UPDATE paystubitem psi
    SET psi.PayAmount=(psi.PayAmount - (OLD.PayAmount)) + NEW.PayAmount
    WHERE psi.PayStubID=NEW.PayStubID
    AND psi.ProductID=prod_rowID
    AND psi.OrganizationID=NEW.OrganizationID;

    UPDATE paystub ps
    INNER JOIN paystubitem psi ON psi.ProductID=prod_rowID AND psi.OrganizationID=NEW.OrganizationID AND psi.PayStubID=NEW.PayStubID
    SET ps.TotalTaxableSalary=psi.PayAmount
    WHERE ps.RowID=NEW.PayStubID;

END IF;


SELECT RowID FROM product p WHERE p.PartNo='Net Income' AND p.OrganizationID=NEW.OrganizationID INTO prod_rowID;










UPDATE paystub ps
SET ps.TotalNetSalary=(ps.TotalNetSalary + (NEW.PayAmount)) - OLD.PayAmount
,ps.TotalAdjustments=(ps.TotalAdjustments + (NEW.PayAmount)) - OLD.PayAmount

WHERE ps.RowID=NEW.PayStubID AND IFNULL(OLD.PayAmount,0) != IFNULL(NEW.PayAmount,0);


END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
