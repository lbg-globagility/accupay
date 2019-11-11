/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

DROP TRIGGER IF EXISTS `AFTINS_paystubbonus`;
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER `AFTINS_paystubbonus` AFTER INSERT ON `paystubbonus` FOR EACH ROW BEGIN

 INSERT INTO bonusloandeduction
 (
     OrganizationID
     ,CreatedBy
     ,LoanSchedID
     ,PayPeriodID
     ,DeductionLoanAmount
 ) SELECT NEW.OrganizationID
     ,NEW.CreatedBy
	  ,els.RowID
	  ,NEW.PayPeriodID
	  ,els.BonusPotentialPaymentForLoan
     FROM employeeloanschedule els
     
     INNER JOIN employeebonus eb
             ON eb.RowID = els.BonusID
                AND eb.EmployeeID = els.EmployeeID
                AND eb.OrganizationID = els.OrganizationID
                AND (eb.EffectiveStartDate >= NEW.PayFromDate OR eb.EffectiveEndDate >= NEW.PayFromDate)
                AND (eb.EffectiveStartDate <= NEW.PayToDate OR eb.EffectiveEndDate <= NEW.PayToDate)
      
     WHERE els.OrganizationID=NEW.OrganizationID
     
     AND els.LoanPayPeriodLeft >= 1
     # AND els.`Status`='In Progress'
     AND els.EmployeeID = NEW.EmployeeID
     AND els.OrganizationID = NEW.OrganizationID
     # AND els.EmployeeID IS NULL
     # AND els.DeductionSchedule IN ('First half','Per pay period')
     # AND (els.DedEffectiveDateFrom >= NEW.PayFromDate OR els.DedEffectiveDateTo >= NEW.PayFromDate)
     # AND (els.DedEffectiveDateFrom <= NEW.PayToDate OR els.DedEffectiveDateTo <= NEW.PayToDate)
 ON
 DUPLICATE
 KEY
 UPDATE
     LastUpd=CURRENT_TIMESTAMP()
     ,LastUpdBy=NEW.CreatedBy;
  
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
