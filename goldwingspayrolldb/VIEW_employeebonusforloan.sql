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

-- Dumping structure for procedure VIEW_employeebonusforloan
DROP PROCEDURE IF EXISTS `VIEW_employeebonusforloan`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `VIEW_employeebonusforloan`(IN `ebon_EmployeeID` INT, IN `ebon_OrganizationID` INT, IN `AssignBonusRowID` INT, IN `loan_startdate` DATE, IN `loan_enddate` DATE, IN `els_rowid` INT)
    DETERMINISTIC
BEGIN

DECLARE max_date_ofparamdate
       ,min_date_ofparamdate DATE;
       
SELECT
	MIN(pyp.PayFromDate)
	,MAX(pyp.PayToDate)
FROM payperiod pyp
INNER JOIN payperiod pp
        ON pp.OrganizationID=pyp.OrganizationID
		     AND (pp.PayFromDate >= loan_startdate OR pp.PayToDate >= loan_startdate)
			  AND (pp.PayFromDate <= loan_enddate OR pp.PayToDate <= loan_enddate)
WHERE pyp.OrganizationID=ebon_OrganizationID
AND pyp.`Year` = pp.`Year`
INTO min_date_ofparamdate
     ,max_date_ofparamdate;

SELECT i.*
FROM (
    SELECT TRUE
    ,ebon.RowID
    ,IFNULL(p.PartNo,'') AS `Type`
    ,COALESCE(ebon.BonusAmount,0) AS BonusAmount
    ,IFNULL(ebon.AllowanceFrequency,'') AS AllowanceFrequency
    ,IFNULL(DATE_FORMAT(ebon.EffectiveStartDate,'%m/%d/%Y'),'1/1/1900') AS EffectiveStartDate
    ,IFNULL(DATE_FORMAT(ebon.EffectiveEndDate,'%m/%d/%Y'),'1/1/1900') AS EffectiveEndDate
    ,IFNULL(ebon.ProductID,'') AS ProductID
    ,ebon.RemainingBalance
    ,els.Comments `Remarks` # ebon.Remarks
    ,els.LoanPayPeriodLeftForBonus `LoanPaymentPotential`
    ,'1st' `Grouping`
    FROM employeebonus ebon
    INNER JOIN employeeloanschedule els
            ON els.BonusID=ebon.RowID
               AND els.RowID=els_rowid
    INNER JOIN product p ON ebon.ProductID=p.RowID
    WHERE ebon.EmployeeID=ebon_EmployeeID
    AND ebon.OrganizationID=ebon_OrganizationID AND ebon.RowID=AssignBonusRowID
    
UNION
    SELECT FALSE
    ,ebon.RowID
    ,IFNULL(p.PartNo,'') AS `Type`
    ,COALESCE(ebon.BonusAmount,0) AS BonusAmount
    ,IFNULL(ebon.AllowanceFrequency,'') AS AllowanceFrequency
    ,IFNULL(DATE_FORMAT(ebon.EffectiveStartDate,'%m/%d/%Y'),'1/1/1900') AS EffectiveStartDate
    ,IFNULL(DATE_FORMAT(ebon.EffectiveEndDate,'%m/%d/%Y'),'1/1/1900') AS EffectiveEndDate
    ,IFNULL(ebon.ProductID,'') AS ProductID
    ,ebon.RemainingBalance
    ,ebon.Remarks
    ,0 `LoanPaymentPotential`
    ,'2nd' `Grouping`
    FROM employeebonus ebon
    INNER JOIN product p ON ebon.ProductID=p.RowID
    WHERE ebon.EmployeeID=ebon_EmployeeID
    AND ebon.OrganizationID=ebon_OrganizationID
	 AND (ebon.EffectiveStartDate >= loan_startdate OR ebon.EffectiveEndDate >= loan_startdate)
    AND (ebon.EffectiveStartDate <= loan_enddate OR ebon.EffectiveEndDate <= loan_enddate)
    AND ebon.RemainingBalance > 0
    
UNION
    SELECT FALSE
    ,ebon.RowID
    ,IFNULL(p.PartNo,'') AS `Type`
    ,COALESCE(ebon.BonusAmount,0) AS BonusAmount
    ,IFNULL(ebon.AllowanceFrequency,'') AS AllowanceFrequency
    ,IFNULL(DATE_FORMAT(ebon.EffectiveStartDate,'%m/%d/%Y'),'1/1/1900') AS EffectiveStartDate
    ,IFNULL(DATE_FORMAT(ebon.EffectiveEndDate,'%m/%d/%Y'),'1/1/1900') AS EffectiveEndDate
    ,IFNULL(ebon.ProductID,'') AS ProductID
    ,ebon.RemainingBalance
    ,ebon.Remarks
    ,0 `LoanPaymentPotential`
    ,'3rd' `Grouping`
    FROM employeebonus ebon
    INNER JOIN product p ON ebon.ProductID=p.RowID
    WHERE ebon.EmployeeID=ebon_EmployeeID
    AND ebon.OrganizationID=ebon_OrganizationID
	 # AND (ebon.EffectiveStartDate >= min_date_ofparamdate OR ebon.EffectiveEndDate >= min_date_ofparamdate)
    # AND (ebon.EffectiveStartDate <= max_date_ofparamdate OR ebon.EffectiveEndDate <= max_date_ofparamdate)
    AND ebon.RemainingBalance > 0
     ) i
GROUP BY i.RowID
;

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
