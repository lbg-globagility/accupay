-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.11-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.0.0.4396
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for function goldwingspayrolldb.GET_lastdateofdayname
DROP FUNCTION IF EXISTS `GET_lastdateofdayname`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` FUNCTION `GET_lastdateofdayname`(`ParamDate` DATE, `DayNameIndex` CHAR(2)) RETURNS date
    DETERMINISTIC
BEGIN

DECLARE returnvalue DATE;

DECLARE loopindex INT(11) DEFAULT 0;

DECLARE catchdate DATE;

IF DayNameIndex < 0 THEN
	
	SET loopindex = loopindex * (-1);
	
	thisloop: LOOP
		
		SET catchdate = ADDDATE(ParamDate, INTERVAL loopindex DAY);
		
		IF CONCAT('-',DAYOFWEEK(catchdate)) = DayNameIndex THEN
			
			SET returnvalue = catchdate;
			
			LEAVE thisloop;
			
		END IF;
		
		SET loopindex = loopindex - 1;
		
		IF loopindex <= -31 THEN
		
			LEAVE thisloop;
			
		END IF;
		
	END LOOP thisloop;

ELSE

	thisloop: LOOP
		
		SET catchdate = ADDDATE(ParamDate, INTERVAL loopindex DAY);
		
		IF DAYOFWEEK(catchdate) = DayNameIndex THEN
			
			SET returnvalue = catchdate;
			
			LEAVE thisloop;
			
		END IF;
		
		SET loopindex = loopindex + 1;
		
		IF loopindex <= -31 THEN
		
			LEAVE thisloop;
			
		END IF;
		
	END LOOP thisloop;

END IF;

RETURN returnvalue;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
