-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               10.4.28-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win64
-- HeidiSQL Version:             11.3.0.6295
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

-- Dumping structure for procedure accupaydb_rgi.DBoard_FrequentTardiness
DELIMITER //
CREATE PROCEDURE `DBoard_FrequentTardiness`(
	IN `OrganizID` INT
)
BEGIN

SELECT result.EmployeeID,result.Employee_Fullname,result.TotalHoursLate FROM
	(SELECT `t`.EmployeeID,CONCAT(`t.Employee`.Lastname,',',`t.Employee`.Firstname,',',INITIALS(`t.Employee`.MiddleName,'.','1')) 'Employee_Fullname',SUM(t.HoursLate) AS TotalHoursLate
		 FROM `employeetimeentry` AS `t`
      LEFT JOIN `employee` AS `t.Employee` ON `t`.`EmployeeID` = `t.Employee`.`RowID`
   WHERE (  MONTH(t.Date) = MONTH(CURDATE()) AND YEAR(t.Date) = YEAR(CURDATE()) and (`t`.`OrganizationID` =OrganizID)) AND (`t`.`HoursLate` > 0.0) GROUP BY t.EmployeeID) AS result 
WHERE result.TotalHoursLate>=10;
END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
