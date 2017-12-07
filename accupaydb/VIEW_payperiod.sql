/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP PROCEDURE IF EXISTS `VIEW_payperiod`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_payperiod`(IN `payp_OrganizationID` INT, IN `param_Date` DATE, IN `PayFreqType` TEXT)
    DETERMINISTIC
BEGIN

IF PayFreqType = 'SEMI-MONTHLY' THEN

    SELECT RowID
    ,COALESCE(DATE_FORMAT(PayFromDate,'%m/%d/%Y'),'') 'Pay period from'
    ,COALESCE(DATE_FORMAT(PayToDate,'%m/%d/%Y'),'') 'Pay period to'
    ,COALESCE(TotalGrossSalary,0) 'TotalGrossSalary'
    ,COALESCE(TotalNetSalary,0) 'TotalNetSalary'
    ,COALESCE(TotalEmpSSS,0) 'TotalEmpSSS'
    ,COALESCE(TotalEmpWithholdingTax,0) 'TotalEmpWithholdingTax'
    ,COALESCE(TotalCompSSS,0) 'TotalCompSSS'
    ,COALESCE(TotalEmpPhilhealth,0) 'TotalEmpPhilhealth'
    ,COALESCE(TotalCompPhilhealth,0) 'TotalCompPhilhealth'
    ,COALESCE(TotalEmpHDMF,0) 'TotalEmpHDMF'
    ,COALESCE(TotalCompHDMF,0) 'TotalCompHDMF'
    ,IF(DATE_FORMAT(NOW(),'%Y-%m-%d') BETWEEN PayFromDate AND PayToDate,'0',IF(DATE_FORMAT(NOW(),'%Y-%m-%d') > PayFromDate,'-1','1')) 'now_origin' FROM payperiod
    WHERE OrganizationID=payp_OrganizationID
    AND TotalGrossSalary=1
    AND `Month` = MONTH(param_Date)
    AND `Year` = YEAR(param_Date)
    ORDER BY PayFromDate,PayToDate;



ELSEIF PayFreqType = 'WEEKLY' THEN

    SELECT RowID
    ,COALESCE(DATE_FORMAT(PayFromDate,'%m/%d/%Y'),'') 'Pay period from'
    ,COALESCE(DATE_FORMAT(PayToDate,'%m/%d/%Y'),'') 'Pay period to'
    ,COALESCE(TotalGrossSalary,0) 'TotalGrossSalary'
    ,COALESCE(TotalNetSalary,0) 'TotalNetSalary'
    ,COALESCE(TotalEmpSSS,0) 'TotalEmpSSS'
    ,COALESCE(TotalEmpWithholdingTax,0) 'TotalEmpWithholdingTax'
    ,COALESCE(TotalCompSSS,0) 'TotalCompSSS'
    ,COALESCE(TotalEmpPhilhealth,0) 'TotalEmpPhilhealth'
    ,COALESCE(TotalCompPhilhealth,0) 'TotalCompPhilhealth'
    ,COALESCE(TotalEmpHDMF,0) 'TotalEmpHDMF'
    ,COALESCE(TotalCompHDMF,0) 'TotalCompHDMF'
    ,IF(DATE_FORMAT(NOW(),'%Y-%m-%d') BETWEEN PayFromDate AND PayToDate,'0',IF(DATE_FORMAT(NOW(),'%Y-%m-%d') > PayFromDate,'-1','1')) 'now_origin' FROM payperiod
    WHERE OrganizationID=payp_OrganizationID
    AND TotalGrossSalary=4
    AND `Month` = MONTH(param_Date)
    AND `Year` = YEAR(param_Date)
    ORDER BY PayFromDate;

END IF;

END//
DELIMITER ;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
