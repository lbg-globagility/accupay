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

-- Dumping structure for function COUNT_payperiodthisyear
DROP FUNCTION IF EXISTS `COUNT_payperiodthisyear`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `COUNT_payperiodthisyear`(`organization_ID` INT, `PayFrequencyID` INT) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE payp_count INT(11) DEFAULT 24;

IF PayFrequencyID = 2 THEN

    SELECT COUNT(RowID) FROM payperiod WHERE YEAR(COALESCE(PayFromDate,YEAR(NOW())))=YEAR(NOW()) AND OrganizatioNID=organization_ID AND DATEDIFF(PayToDate,PayFromDate)!=4 INTO payp_count;

ELSEIF PayFrequencyID = 3 THEN

    SELECT COUNT(RowID) FROM payperiod WHERE YEAR(COALESCE(PayFromDate,YEAR(NOW())))=YEAR(NOW()) AND OrganizatioNID=organization_ID AND DATEDIFF(PayToDate,PayFromDate)!=4 INTO payp_count;

ELSEIF PayFrequencyID = 4 THEN



    SELECT COUNT(RowID) FROM payperiod WHERE OrganizationID=organization_ID AND TotalGrossSalary=PayFrequencyID AND `Year`=YEAR(CURDATE()) INTO payp_count;

ELSE



    SELECT COUNT(RowID) FROM payperiod WHERE OrganizationID=organization_ID AND TotalGrossSalary=PayFrequencyID AND `Year`=YEAR(CURDATE()) INTO payp_count;

END IF;

RETURN payp_count;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
