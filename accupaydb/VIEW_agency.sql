/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_agency`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `VIEW_agency`(IN `OrganizID` INT, IN `SearchText` VARCHAR(100))
    DETERMINISTIC
BEGIN

IF SearchText = '' THEN

    SELECT ag.RowID
    ,ag.AgencyName
    ,ag.AgencyFee
    ,IF(ag.AddressID IS NULL, '', CONCAT(ad.StreetAddress1,ad.StreetAddress2,ad.Barangay,ad.CityTown,ad.State,ad.Country)) AS AgencyAddress
    ,IFNULL(ag.AddressID,'') AS AddressID
    ,ag.IsActive
    FROM agency ag
    LEFT JOIN address ad ON ad.RowID=ag.AddressID
    WHERE ag.OrganizationID=OrganizID;

ELSE

    SELECT
        ag.RowID
        ,ag.AgencyName
        ,ag.AgencyFee
        ,IF(ag.AddressID IS NULL, '', CONCAT(ad.StreetAddress1,ad.StreetAddress2,ad.Barangay,ad.CityTown,ad.State,ad.Country)) AS AgencyAddress
        ,IFNULL(ag.AddressID,'') AS AddressID
        ,ag.IsActive
    FROM agency ag
    LEFT JOIN address ad
        ON ad.RowID=ag.AddressID
    WHERE ag.OrganizationID=OrganizID
        AND ag.AgencyName=SearchText;

END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
