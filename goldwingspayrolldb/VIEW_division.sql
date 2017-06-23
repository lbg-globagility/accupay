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

-- Dumping structure for procedure goldwingspayrolldb.VIEW_division
DROP PROCEDURE IF EXISTS `VIEW_division`;
DELIMITER //
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `VIEW_division`(IN `OrganizID` INT, IN `SearchDivisionName` VARCHAR(100))
    DETERMINISTIC
BEGIN

IF SearchDivisionName = '' THEN
	
	SELECT
	d.Name
	,d.DivisionType
	,IFNULL(dd.Name,'') AS ParentDivName
	,d.RowID
	,d.TradeName
	,d.MainPhone
	,d.AltPhone
	,d.EmailAddress
	,d.AltEmailAddress
	,d.FaxNumber
	,d.TinNo
	,d.URL
	,d.ContactName
	,d.BusinessAddress
	,d.GracePeriod
	,d.WorkDaysPerYear
	,d.PhHealthDeductSched
	,d.HDMFDeductSched
	,d.SSSDeductSched
	,d.WTaxDeductSched
	,d.DefaultVacationLeave
	,d.DefaultSickLeave
	,d.DefaultMaternityLeave
	,d.DefaultPaternityLeave
	,d.DefaultOtherLeave
	,pf.PayFrequencyType
	,pf.RowID
	FROM `division` d
	LEFT JOIN payfrequency pf ON pf.RowID=d.PayFrequencyID
	LEFT JOIN `division` dd ON dd.RowID=d.ParentDivisionID
	WHERE d.OrganizationID=OrganizID;

ELSE

	SELECT
	d.Name
	,d.DivisionType
	,IFNULL(dd.Name,'') AS ParentDivName
	,d.RowID
	,d.TradeName
	,d.MainPhone
	,d.AltPhone
	,d.EmailAddress
	,d.AltEmailAddress
	,d.FaxNumber
	,d.TinNo
	,d.URL
	,d.ContactName
	,d.BusinessAddress
	,d.GracePeriod
	,d.WorkDaysPerYear
	,d.PhHealthDeductSched
	,d.HDMFDeductSched
	,d.SSSDeductSched
	,d.WTaxDeductSched
	,d.DefaultVacationLeave
	,d.DefaultSickLeave
	,d.DefaultMaternityLeave
	,d.DefaultPaternityLeave
	,d.DefaultOtherLeave
	,pf.PayFrequencyType
	,pf.RowID
	FROM `division` d
	LEFT JOIN payfrequency pf ON pf.RowID=d.PayFrequencyID
	LEFT JOIN `division` dd ON dd.RowID=d.ParentDivisionID
	WHERE d.OrganizationID=OrganizID
	AND d.Name=SearchDivisionName;

END IF;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
