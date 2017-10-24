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

-- Dumping structure for trigger BEFDEL_divisionminimumwage
DROP TRIGGER IF EXISTS `BEFDEL_divisionminimumwage`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `BEFDEL_divisionminimumwage` BEFORE DELETE ON `divisionminimumwage` FOR EACH ROW BEGIN

DECLARE specialty CONDITION FOR SQLSTATE '45000';

DECLARE count_min_wage INT(11);

SELECT COUNT(RowID) FROM divisionminimumwage WHERE OrganizationID=OLD.OrganizationID AND DivisionID=OLD.DivisionID INTO count_min_wage;

IF count_min_wage <= 1 THEN

    SIGNAL specialty
    SET MESSAGE_TEXT = 'Invalid delete. This department should
have at least one minimum wage amount.';

END IF;

END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
