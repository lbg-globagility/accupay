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

-- Dumping structure for procedure common_payrate
DROP PROCEDURE IF EXISTS `common_payrate`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `common_payrate`()
    DETERMINISTIC
BEGIN

SET @i = 2017;

WHILE (@i BETWEEN 2017 AND 2020) DO

    SELECT INSUPD_payrate(NULL,1,1,1,CONCAT(@i,'-01-01'),'Regular Holiday','New Year\'s Day',2.00,2.60,2.20,2.86,2.60,3.38) UNION

    SELECT INSUPD_payrate(NULL,1,1,1,CONCAT(@i,'-02-25'),'Special Non-Working Holiday','People Power Anniversary',1.30,1.69,1.43,1.86,1.50,1.95) UNION

    SELECT INSUPD_payrate(NULL,1,1,1,CONCAT(@i,'-04-09'),'Regular Holiday','The Day of Valor',2.00,2.60,2.20,2.86,2.60,3.38) UNION
    SELECT INSUPD_payrate(NULL,1,1,1,CONCAT(@i,'-05-01'),'Regular Holiday','Labor Day',2.00,2.60,2.20,2.86,2.60,3.38) UNION
    SELECT INSUPD_payrate(NULL,1,1,1,CONCAT(@i,'-06-12'),'Regular Holiday','Independence Day',2.00,2.60,2.20,2.86,2.60,3.38) UNION

    SELECT INSUPD_payrate(NULL,1,1,1,CONCAT(@i,'-08-21'),'Special Non-Working Holiday','Ninoy Aquino Day',1.30,1.69,1.43,1.86,1.50,1.95) UNION

    SELECT INSUPD_payrate(NULL,1,1,1,CONCAT(@i,'-11-01'),'Special Non-Working Holiday','All Saints\' Day',1.30,1.69,1.43,1.86,1.50,1.95) UNION
    SELECT INSUPD_payrate(NULL,1,1,1,CONCAT(@i,'-11-30'),'Regular Holiday','Bonifacio Day',2.00,2.60,2.20,2.86,2.60,3.38) UNION
    SELECT INSUPD_payrate(NULL,1,1,1,CONCAT(@i,'-12-24'),'Special Non-Working Holiday','Christmas Eve',1.30,1.69,1.43,1.86,1.50,1.95) UNION
    SELECT INSUPD_payrate(NULL,1,1,1,CONCAT(@i,'-12-25'),'Regular Holiday','Christmas Day',2.00,2.60,2.20,2.86,2.60,3.38) UNION
    SELECT INSUPD_payrate(NULL,1,1,1,CONCAT(@i,'-12-30'),'Regular Holiday','Rizal Day',2.00,2.60,2.20,2.86,2.60,3.38) UNION

    SELECT INSUPD_payrate(NULL,1,1,1,CONCAT(@i,'-12-31'),'Special Non-Working Holiday','New Year\'s Eve',1.30,1.69,1.43,1.86,1.50,1.95);



    SET @i = @i + 1;

END WHILE;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
