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

-- Dumping structure for procedure VIEW_specificemployeesalary
DROP PROCEDURE IF EXISTS `VIEW_specificemployeesalary`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_specificemployeesalary`(IN `esal_EmployeeID` INT, IN `esal_OrganizationID` INT, IN `esal_Date` DATE)
    DETERMINISTIC
BEGIN

SELECT
esal.RowID
,esal.EmployeeID
,esal.FilingStatusID
,COALESCE((SELECT FilingStatus FROM filingstatus WHERE RowID=esal.FilingStatusID),'') 'FilingStatus'
,COALESCE(esal.PaySocialSecurityID,'') 'PaySocialSecurityID'
,COALESCE((SELECT EmployeeContributionAmount FROM paysocialsecurity WHERE RowID=esal.PaySocialSecurityID),'') 'EmployeeContributionAmount'
,esal.PayPhilhealthID
,COALESCE((SELECT EmployeeShare FROM payphilhealth WHERE RowID=esal.PayPhilhealthID),'') 'EmployeeShare'
,COALESCE(esal.HDMFAmount,50) 'HDMFAmount'
,esal.BasicPay
,esal.NoofDependents
,COALESCE(esal.MaritalStatus,'') 'MaritalStatus'
,COALESCE(esal.PositionID,'') 'PositionID'
,COALESCE((SELECT PositionName FROM position WHERE RowID=esal.PositionID),'') 'Position'
,esal.EffectiveDateFrom
,COALESCE(IF(DATEDIFF(NOW(),esal.EffectiveDateFrom)<0,'tomorrow',COALESCE(esal.EffectiveDateTo,ADDDATE(CURRENT_DATE(),INTERVAL 100 YEAR))),'') 'EffectiveDateTo'
FROM employeesalary esal
WHERE esal.OrganizationID=esal_OrganizationID
AND esal.EmployeeID=esal_EmployeeID
AND esal_Date BETWEEN DATE(COALESCE(EffectiveDateFrom,DATE_FORMAT(CURRENT_TIMESTAMP(),'%Y-%m-%d')))
AND DATE(COALESCE(EffectiveDateTo,ADDDATE(esal_Date, INTERVAL 1 MONTH)))
AND DATEDIFF(esal_Date,EffectiveDateFrom) >= 0
ORDER BY DATEDIFF(DATE_FORMAT(esal_Date,'%Y-%m-%d'),EffectiveDateFrom) LIMIT 1;




END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
