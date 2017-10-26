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

-- Dumping structure for procedure VIEW_employeedependents
DROP PROCEDURE IF EXISTS `VIEW_employeedependents`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeedependents`(IN `edep_ParentEmployeeID` INT, IN `edep_OrganizationID` INT)
    DETERMINISTIC
    COMMENT 'view all employee''s dependent(s) base on emloyee RowID and organization'
BEGIN

SELECT edep.RowID
,edep.ParentEmployeeID
,COALESCE(edep.Salutation,'') 'Salutation'
,edep.FirstName
,COALESCE(edep.MiddleName,'') 'MiddleName'
,edep.LastName
,COALESCE(edep.Surname,'') 'Surname'
,COALESCE(edep.RelationToEmployee,'') 'RelationToEmployee'
,COALESCE(edep.TINNo,'') 'TINNo'
,COALESCE(edep.SSSNo,'') 'SSSNo'
,COALESCE(edep.HDMFNo,'') 'HDMFNo'
,COALESCE(edep.PhilHealthNo,'') 'PhilHealthNo'
,COALESCE(edep.EmailAddress,'') 'EmailAddress'
,COALESCE(edep.WorkPhone,'') 'WorkPhone'
,COALESCE(edep.HomePhone,'') 'HomePhone'
,COALESCE(edep.MobilePhone,'') 'MobilePhone'
,COALESCE(edep.HomeAddress,'') 'HomeAddress'
,COALESCE(edep.Nickname,'') 'Nickname'
,COALESCE(edep.JobTitle,'') 'JobTitle'
,COALESCE(edep.Gender,'') 'Gender'
,IF(edep.ActiveFlag='Y',TRUE,FALSE) 'ActiveFlag'
,COALESCE(DATE_FORMAT(edep.Birthdate,'%m/%d/%Y'),'') 'Birthdate'
,CONCAT(u.FirstName,' ',u.LastName) 'CreatedBy'
,DATE_FORMAT(edep.Created,'%m-%d-%Y') 'Created'
,COALESCE((SELECT CONCAT(FirstName,' ',LastName) FROM user WHERE RowID=edep.LastUpdBy),'') 'LastUpdBy'
,COALESCE(DATE_FORMAT(edep.LastUpd,'%m-%d-%Y'),'') 'LastUpd'
FROM employeedependents edep
LEFT JOIN user u ON u.RowID=edep.CreatedBy
WHERE edep.OrganizationID=edep_OrganizationID
AND edep.ParentEmployeeID=edep_ParentEmployeeID;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
