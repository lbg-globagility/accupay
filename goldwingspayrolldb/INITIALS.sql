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

-- Dumping structure for function goldwingspayrolldb.INITIALS
DROP FUNCTION IF EXISTS `INITIALS`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INITIALS`(`str` TEXT, `strtoadd` TEXT, `isuppercase` CHAR(1)) RETURNS text CHARSET latin1
    DETERMINISTIC
BEGIN

    DECLARE result text DEFAULT '';
    
    DECLARE buffer text DEFAULT '';
    
    DECLARE i int DEFAULT 1;
    
    DECLARE expr text DEFAULT '[[:alnum:]]';
    
    IF(str IS NULL) THEN
    
        RETURN '';
        
    ELSE
    
	    SET buffer = TRIM(str);
	    
	    WHILE i <= LENGTH(buffer) DO
	    
	        IF SUBSTR(buffer, i, 1) REGEXP expr THEN
	        
	            SET result = CONCAT(result, SUBSTR( buffer, i, 1), strtoadd);
	            
	            SET i = i + 1;
	            
	            WHILE i <= LENGTH(buffer) AND SUBSTR(buffer, i, 1) REGEXP expr DO
	            
	                SET i = i + 1;
	                
	            END WHILE;
	            
	            WHILE i <= LENGTH(buffer) AND SUBSTR(buffer, i, 1) NOT REGEXP expr DO
	            
	                SET i = i + 1;
	                
	            END WHILE;
	            
	        ELSE
	        
	            SET i = i + 1;
	            
	        END IF;
	        
	    END WHILE;
	    
	    IF isuppercase = '1' THEN
	    
	    	SET result = TRIM(UCASE(result));
	    
	    ELSE
	    
	    	SET result = TRIM(LCASE(result));
	    	
	    END IF;
	    
	    RETURN result;
	    
	 END IF;
    
END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
