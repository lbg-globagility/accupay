/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP FUNCTION IF EXISTS `INSUPD_address`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `INSUPD_address`(
	`ad_rowid` INT,
	`user_rowid` INT,
	`ad_street1` VARCHAR(200),
	`ad_street2` VARCHAR(50),
	`ad_brgy` VARCHAR(50),
	`ad_city` VARCHAR(50),
	`ad_state` VARCHAR(50),
	`ad_country` VARCHAR(50),
	`ad_zipcode` VARCHAR(50)


) RETURNS int(11)
    DETERMINISTIC
BEGIN

DECLARE returnvalue INT(11);

INSERT INTO address
(
	RowID
	,StreetAddress1
	,StreetAddress2
	,Barangay
	,CityTown
	,Country
	,State
	,ZipCode
	,CreatedBy
	,LastUpdBy
	,Created
	,LastUpd
) VALUES (
	ad_rowid
	,ad_street1
	,ad_street2
	,ad_brgy
	,ad_city
	,ad_country
	,ad_state
	,ad_zipcode
	,user_rowid
	,user_rowid
	,CURRENT_TIMESTAMP()
	,CURRENT_TIMESTAMP()
) ON
DUPLICATE
KEY
UPDATE
	LastUpd = CURRENT_TIMESTAMP()
	,LastUpdBy = user_rowid
	,StreetAddress1 = ad_street1
	,StreetAddress2 = ad_street2
	,Barangay = ad_brgy
	,CityTown = ad_city
	,Country = ad_country
	,State = ad_state
	,ZipCode = ad_zipcode;
SELECT @@Identity `ID` INTO returnvalue;

RETURN returnvalue;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
